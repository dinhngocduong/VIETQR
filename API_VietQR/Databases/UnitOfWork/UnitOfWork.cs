using System.Data;
using System.Data.SqlClient;

namespace API_VietQR.Databases.UnitOfWork
{
	public interface IUnitOfWork
	{
		IDbConnection ConnectionBooking();
		IDbConnection Connection_LOG();
	}

	public class UnitOfWork : IUnitOfWork
	{
		private readonly IConfiguration _configuration;

		public UnitOfWork(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IDbConnection ConnectionBooking()
		{
			IDbConnection db = new SqlConnection(_configuration.GetConnectionString("Web_Booking"));
			return db;
		}

		public IDbConnection Connection_LOG()
		{
			IDbConnection db = new SqlConnection(_configuration.GetConnectionString("MUADI_LOG"));
			return db;
		}
	}
}
