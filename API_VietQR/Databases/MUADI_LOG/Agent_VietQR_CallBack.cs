using Dapper.Contrib.Extensions;

namespace API_VietQR.Databases.MUADI_LOG
{

	[Table("tbl_Agent_VietQR_CallBack")]
	public class Agent_VietQR_CallBack
	{
		public long ID { get; set; }
		public long AgentID { get; set; }
		public string RefTransactionId { get; set; }
		public string TransactionId { get; set; }
		public long TransactionTime { get; set; }
		public string ReferenceNumber { get; set; }
		public long Amount { get; set; }
		public string MessageContent { get; set; }
		public string BankAccount { get; set; }
		public string TransType { get; set; }
		public string OrderId { get; set; }
		public DateTime CreateDate { get; set; }
		public int Status { get; set; }	
	}
}
