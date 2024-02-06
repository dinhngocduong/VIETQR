using API_VietQR.Modal.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_VietQR.Configurations
{
	public class JwtInstaller : IServiceInstaller
	{
		public void Install(IServiceCollection services, IConfiguration configure)
		{
			var jwtSettings = new JwtSettings();
			configure.Bind(nameof(jwtSettings), jwtSettings);
			services.AddSingleton(jwtSettings);

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
				ValidateIssuer = false,
				ValidateAudience = false,
				RequireExpirationTime = false,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					options.TokenValidationParameters = tokenValidationParameters;
					options.Events = new JwtBearerEvents
					{
						OnAuthenticationFailed = context =>
						{
							return Task.CompletedTask;
						}
					};
				});
		}
	}
}
