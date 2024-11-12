using Dapper.Contrib.Extensions;

namespace API_VietQR.Databases.Booking
{
	[Table("tbl_Agent_Payment_B2C")]
	public class Agent_Payment_B2C
	{
		public long ID { get; set; }
		public string PaymentType { get; set; }
		public string PaymentTitle { get; set; }
		public long BookingID { get; set; }
		public long MemberID { get; set; }
		public long AgentID { get; set; }
		public long SubAgentID { get; set; }
		public string PayCode { get; set; }
		public string PayStatus { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal OtherFee { get; set; }
		public string BankCode { get; set; }
		public string BankNo { get; set; }
		public string BankName { get; set; }
		public string FullContent { get; set; }
		public string PayStatus_R { get; set; }
		public string PayTransaction_R { get; set; }
		public decimal PayTotal_R { get; set; }
		public DateTime PayCreateDate_R { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ModifeDate { get; set; }
	}
}
