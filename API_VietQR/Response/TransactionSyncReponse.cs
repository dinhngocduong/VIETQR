namespace API_VietQR.Response
{
	public class TransactionSyncReponse
	{
		public bool error { get; set; }
		public string errorReason { get; set; }
		public string toastMessage { get; set; }
		public Object @object { get; set; }

	}
	public class Object
	{
		public string reftransactionid { get; set; }
	}
}
