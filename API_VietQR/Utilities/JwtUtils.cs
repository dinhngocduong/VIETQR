using API_VietQR.Constant;
using API_VietQR.Modal.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API_VietQR.Utilities
{
	public static class JwtUtils
	{
		public static string GenerateToken(List<Claim> claims, string secret, TimeSpan lifeTime)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var jwt = new JwtSecurityToken(
				issuer: "VNISC",
				claims: claims,
				expires: DateTime.UtcNow.Add(lifeTime),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		public static string GenerateToken(IEnumerable<Claim> claims, string secret, TimeSpan lifeTime)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var jwt = new JwtSecurityToken(
				issuer: "VNISC",
				claims: claims,
				expires: DateTime.UtcNow.Add(lifeTime),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		public static string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		public static string GenerateRefreshToken(long memberId, string secret, TimeSpan lifeTime)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			List<Claim> claims = new List<Claim>();
			claims.Add(new Claim("MemberId", memberId.ToString()));
			var jwt = new JwtSecurityToken(
				issuer: "VNISC",
				claims: claims,
				expires: DateTime.UtcNow.Add(lifeTime),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new AppException(System.Net.HttpStatusCode.Forbidden, Error.ErrorCode.AUTHENTICATE_ERROR, Error.ErrorMessage.INVALID_TOKEN);

			return principal;
		}

		public static ClaimsPrincipal GetPrincipal(string token, string secret)
		{

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			TokenValidationParameters parameters = new TokenValidationParameters()
			{
				RequireExpirationTime = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
				ClockSkew = TimeSpan.Zero
			};
			SecurityToken securityToken;
			ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
			return principal;
		}

		public static string GetClaim(string token, string claimType)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
				if (securityToken == null) { return string.Empty; }
				var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
				return stringClaimValue;
			}
			catch
			{
				throw new AppException(System.Net.HttpStatusCode.Unauthorized, Error.ErrorCode.AUTHENTICATE_ERROR, Error.ErrorMessage.INVALID_AUTHENTICATION);
			}
		}

		public static string ValidateToken(string token, string secret)
		{

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
				ValidateLifetime = false,
				ClockSkew = TimeSpan.Zero
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				return false.ToString();
			}
			var checkExpired = CustomLifetimeValidator(jwtSecurityToken.ValidTo);
			if (checkExpired)
			{
				return Constants.EXPIRED;
			}
			return true.ToString();

		}

		public static bool CustomLifetimeValidator(DateTime? expires)
		{
			if (expires != null)
			{
				return expires < DateTime.UtcNow;
			}
			return false;
		}
	}
}
