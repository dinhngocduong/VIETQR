namespace API_VietQR.Response
{
	public class BaseResponse<T>
	{
		public bool Success { get; set; } = false;
		public T Data { get; set; }
		public string Code { get; set; }
		public string Message { get; set; }
	}
}
