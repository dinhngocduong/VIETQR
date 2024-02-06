using API_VietQR.Modal.Settings;
using API_VietQR.Services.Cache;
using StackExchange.Redis;

namespace API_VietQR.Configurations
{
	public class CacheInstaller : IServiceInstaller
	{
		public void Install(IServiceCollection services, IConfiguration configure)
		{
			var redisCacheSettings = new RedisCacheSettings();
			configure.GetSection(nameof(redisCacheSettings)).Bind(redisCacheSettings);
			services.AddSingleton(redisCacheSettings);
			if (!redisCacheSettings.Enabled)
			{
				return;
			}
			services.AddSingleton<IConnectionMultiplexer>(sp =>
				 ConnectionMultiplexer.Connect(new ConfigurationOptions
				 {
					 EndPoints = { redisCacheSettings.ConnectionString },
					 DefaultDatabase = redisCacheSettings.Database,
					 Password = redisCacheSettings.Password,
					 AbortOnConnectFail = false,
				 }));

			services.AddSingleton<ICacheServices, CacheServices>();
		}
	}
}
