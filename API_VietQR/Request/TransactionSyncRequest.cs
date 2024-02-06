using System.Runtime.InteropServices;

namespace API_VietQR.Request
{
	public class TransactionSyncRequest
	{
		public string transactionid { get; set; }
		public long transactiontime { get; set; }
		public string referencenumber { get; set; }
		public long amount { get; set; }
		public string content { get; set; }
		public string bankaccount { get; set; }
		public string transType { get; set; }
		public string orderId { get; set; }
	}
}
