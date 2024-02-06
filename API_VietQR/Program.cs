
using API_VietQR.Configurations;
using API_VietQR.Middleware;

namespace API_VietQR
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseCors(x => x.AllowAnyHeader()
				  .AllowAnyMethod()
				  .AllowAnyOrigin().WithExposedHeaders("Version", "Time"));

			app.UseWhen(context => 
			context.Request.Path.StartsWithSegments("/api/token_generate") || 
			context.Request.Path.StartsWithSegments("/vqr/api/token_generate") ||
			context.Request.Path.StartsWithSegments("/vietqr/api/token_generate")
			, appBuilder =>
			{
				appBuilder.UseMiddleware<CheckSessionMiddlewareAuth>();
			});

			app.UseWhen(context => !(
			context.Request.Path.StartsWithSegments("/api/token_generate") ||
			context.Request.Path.StartsWithSegments("/vqr/api/token_generate") ||
			context.Request.Path.StartsWithSegments("/vietqr/api/token_generate")
			), appBuilder =>
			{
				appBuilder.UseMiddleware<CheckSessionMiddleware>();
			});

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();

		}
	}
}
