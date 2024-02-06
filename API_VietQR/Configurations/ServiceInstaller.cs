using System.Reflection;

namespace API_VietQR.Configurations
{
	public static class ServiceInstaller
	{
		public static IServiceCollection InstallServices(
			this IServiceCollection services,
			IConfiguration configuration,
			params Assembly[] assemblies)
		{
			IEnumerable<IServiceInstaller> serviceInstallers = assemblies.
				SelectMany(a => a.DefinedTypes)
				.Where(IsAssignableToType<IServiceInstaller>)
				.Select(Activator.CreateInstance)
				.Cast<IServiceInstaller>();

			foreach (IServiceInstaller serviceInstaller in serviceInstallers)
			{
				serviceInstaller.Install(services, configuration);
			}

			return services;

			static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
				typeof(T).IsAssignableFrom(typeInfo) &&
				!typeInfo.IsInterface &&
				!typeInfo.IsAbstract;
		}

	}
}
