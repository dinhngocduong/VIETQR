using Dapper.Contrib.Extensions;

namespace API_VietQR.Databases.Booking
{
	[Table("tbl_Agent_VietQR")]
	public class Agent_VietQR
	{
		public long ID { get; set; }
		public long AgentID { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string BankCode { get; set; }
		public string BankAccount { get; set; }
		public bool Active { get; set; }
		public decimal Fee { get; set; }
		public decimal AmountUseFee { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
