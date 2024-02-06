namespace API_VietQR.Constant
{
	public class Error
	{
		protected Error() { }
		public static class ErrorCode
		{
			public const string AUTHENTICATE_ERROR = "12";
			public const string SUCCESS = "00";			
			public const string BAD_REQUEST = "03";
			public const string EXPIRED_TOKEN = "01";
			
		}
		public static class ErrorMessage
		{
			public const string INVALID_TOKEN = "Token không hợp lệ !!!";
			public const string EXPIRED_TOKEN = "Token hết hạn !!!";
			public const string INVALID_AUTHENTICATION = "Sai định danh !!!";			
		}
	}
}
