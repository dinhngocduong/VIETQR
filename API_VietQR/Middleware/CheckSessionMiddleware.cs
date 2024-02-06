using API_VietQR.Constant;
using API_VietQR.Modal.Auth;
using API_VietQR.Modal.Settings;
using API_VietQR.Response;
using API_VietQR.Services.Cache;
using API_VietQR.Utilities;
using System.Net;
using System.Text.Json;

namespace API_VietQR.Middleware
{
	public class CheckSessionMiddleware
	{
		private readonly RequestDelegate _next;

		public CheckSessionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, ICacheServices cacheServices, JwtSettings jwtSetting, IConfiguration configuration)
		{
			var tokenClient = context.Request.Headers["Authorization"].ToString();
			if (string.IsNullOrEmpty(tokenClient))
			{
				tokenClient = context.Request.Query["access_token"];
			}
			var response = context.Response;
			response.ContentType = Constants.APPLICATION_JSON;
			if (string.IsNullOrEmpty(tokenClient))
			{				
				response.StatusCode = (int)HttpStatusCode.Unauthorized;			
				return;
			}
					
			string token = tokenClient;
			token = token.Split(" ")[1];
			
			var verify = JwtUtils.ValidateToken(token, jwtSetting.Secret);
			if (false.ToString().Equals(verify))
			{
				var result = new BaseResponse<string>() { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = Error.ErrorMessage.INVALID_TOKEN };
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await response.WriteAsync(JsonSerializer.Serialize(result, Utils.JSON_OPTIONS));
				return;
			}

			if (Constants.EXPIRED.Equals(verify))
			{
				var result = new BaseResponse<string>() { Code = Error.ErrorCode.EXPIRED_TOKEN, Message = Error.ErrorMessage.EXPIRED_TOKEN };
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await response.WriteAsync(JsonSerializer.Serialize(result, Utils.JSON_OPTIONS));
				return;
			}

			var memberId = JwtUtils.GetClaim(token, Constants.MemberIdClaim);
			if (string.IsNullOrEmpty(memberId))
			{
				var result = new BaseResponse<string>() { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = Error.ErrorMessage.INVALID_TOKEN };
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await response.WriteAsync(JsonSerializer.Serialize(result, Utils.JSON_OPTIONS));
				return;
			}

			var sessionCache = await cacheServices.GetCacheAsync(memberId);
			if (string.IsNullOrEmpty(sessionCache))
			{
				var result = new BaseResponse<string>() { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = Error.ErrorMessage.INVALID_TOKEN };
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await response.WriteAsync(JsonSerializer.Serialize(result, Utils.JSON_OPTIONS));
				return;
			}
			
			var session = JsonSerializer.Deserialize<SessionToken>(sessionCache);
			if (session == null)
			{
				var result = new BaseResponse<string>() { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = Error.ErrorMessage.INVALID_TOKEN };
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await response.WriteAsync(JsonSerializer.Serialize(result, Utils.JSON_OPTIONS));
				return;
			}

			if (!session.Token.Equals(token))
			{
				var result = new BaseResponse<string>() { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = Error.ErrorMessage.INVALID_TOKEN };
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await response.WriteAsync(JsonSerializer.Serialize(result, Utils.JSON_OPTIONS));
				return;
			}

			context.Request.Headers["Authorization"] = $"Bearer {token}";
			await _next(context);
		}
	}
}
