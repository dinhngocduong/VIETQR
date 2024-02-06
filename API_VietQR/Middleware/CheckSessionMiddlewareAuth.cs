using API_VietQR.Constant;
using API_VietQR.Response;
using API_VietQR.Services.Auth;
using API_VietQR.Utilities;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;

namespace API_VietQR.Middleware
{
	public class CheckSessionMiddlewareAuth
	{
		private readonly RequestDelegate _next;		
		public CheckSessionMiddlewareAuth(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, IAuthServices authServices)
		{
			var authenticationToken = context.Request.Headers["Authorization"].ToString();
			var response = context.Response;
			authenticationToken = authenticationToken.Replace("Basic ", "");
			string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
			string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
			if (usernamePasswordArray.Length != 2)
			{
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}
			
			string uname = usernamePasswordArray[0];
			string pass = usernamePasswordArray[1];
			var objUserCheck = await authServices.Auth(uname, pass);
			if (String.IsNullOrEmpty(objUserCheck.access_token))
			{
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}
			
			response.StatusCode = (int)HttpStatusCode.OK;
			await response.WriteAsync(JsonSerializer.Serialize(objUserCheck, Utils.JSON_OPTIONS));
			return;

		}
	}
}
