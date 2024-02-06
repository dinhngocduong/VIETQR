namespace API_VietQR.Request
{
	public class TransactionTestRequest
	{
		public string bankAccount { get; set; }
		public string content { get; set; }

		public string amount { get; set; }
		public string transType { get; set; }
	}
}
