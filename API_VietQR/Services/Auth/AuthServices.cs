using API_VietQR.Constant;
using API_VietQR.Databases.Booking;
using API_VietQR.Databases.UnitOfWork;
using API_VietQR.Modal.Auth;
using API_VietQR.Modal.Settings;
using API_VietQR.Response;
using API_VietQR.Services.Cache;
using API_VietQR.Utilities;
using Dapper;
using System.Security.Claims;
using System.Text.Json;

namespace API_VietQR.Services.Auth
{
    public interface IAuthServices
    {
        Task<LoginResponse> Auth(string username, string password);
    }
    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly ICacheServices _cacheServices;

		public AuthServices(IUnitOfWork unitOfWork, JwtSettings jwtSettings, ICacheServices cacheServices)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _cacheServices = cacheServices;

		}
        public async Task<LoginResponse> Auth(string username, string password)
        {
            using var db = _unitOfWork.ConnectionBooking();
            var objUserVietQR = db.QueryFirstOrDefault<Agent_VietQR>("select * from tbl_Agent_VietQR where Username=@Username and Password=@Password", new
            {
                Username = username,
                Password = password
            });
            if (objUserVietQR == null)
            {
				return new LoginResponse { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = "Agent chưa được cấu hình !" };
			}
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(Constants.AgentIdClaim, objUserVietQR.AgentID.ToString()));
            claims.Add(new Claim(Constants.MemberIdClaim, objUserVietQR.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, objUserVietQR.ID.ToString()));
            string accessToken = JwtUtils.GenerateToken(claims, _jwtSettings.Secret, _jwtSettings.TokenLifeTime);

			await _cacheServices.RemoveCacheAsync(objUserVietQR.ID.ToString());
			var saveCache = await _cacheServices.SetCacheTtlAsync(objUserVietQR.ID.ToString(), JsonSerializer.Serialize(new SessionToken
			{				
				Token = accessToken,				
			}), _jwtSettings.TokenLifeTime);
			if (!saveCache)
			{
				return new LoginResponse { Code = Error.ErrorCode.AUTHENTICATE_ERROR, Message = "Có lỗi trong quá trình xử lý !!!" };
			}
			return new LoginResponse { token_type = "Bearer", access_token = accessToken, expires_in = _jwtSettings.TokenLifeTime.ToString() };
        }
    }
}
