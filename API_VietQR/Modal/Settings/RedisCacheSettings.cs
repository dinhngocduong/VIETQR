namespace API_VietQR.Modal.Settings
{
	public class RedisCacheSettings
	{
		public bool Enabled { get; set; }
		public int Database { get; set; }
		public string ConnectionString { get; set; }
		public string Password { get; set; }
	}
}
