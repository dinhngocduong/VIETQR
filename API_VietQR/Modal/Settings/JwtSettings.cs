namespace API_VietQR.Modal.Settings
{
	public class JwtSettings
	{
		public string Secret { get; set; }
		public TimeSpan TokenLifeTime { get; set; }
		public string RefreshSecret { get; set; }
		public TimeSpan RefreshLifeTime { get; set; }
	}
}
