namespace API_VietQR.Response
{
	public class LoginResponse
	{
		public string? Code { get; set; }
		public string? Message { get; set; }
		public string? access_token { get; set; }
		public string? token_type { get; set; }
		public string? expires_in { get; set; }
	}
}
