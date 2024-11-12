using API_VietQR.Databases.UnitOfWork;
using API_VietQR.Services;
using API_VietQR.Services.Auth;
using API_VietQR.Services.Cache;
using API_VietQR.Services.VietQR;

namespace API_VietQR.Configurations
{
    public class DependenciesInjection : IServiceInstaller
	{
		public void Install(IServiceCollection services, IConfiguration configure)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();		
			services.AddScoped<IServiceHelper, ServiceHelper>();
			services.AddScoped<IAuthServices, AuthServices>();
			services.AddScoped<IVietQRServices, VietQRServices>();
		}
	}
}
