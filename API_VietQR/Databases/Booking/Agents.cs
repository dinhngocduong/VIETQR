using Dapper.Contrib.Extensions;

namespace API_VietQR.Databases.Booking
{
	[Table("tbl_Agent")]
	public class Agents
	{
		public long ID { get; set; }
		public string AgentCode { get; set; }
		public string AgentName { get; set; }
		public string AgentRealName { get; set; }
		public int BookingType { get; set; }
		public string AgentEmail { get; set; }
		public string SignIn { get; set; }
		public string Password { get; set; }
		public string AirCode { get; set; }
		public bool Active { get; set; }
		public bool RedirectBooking { get; set; }
		public string Address { get; set; }
		public string Telephone { get; set; }
		public string Fax { get; set; }
		public bool ShowPNR { get; set; }
		public bool ShowAgentInfo { get; set; }
		public long MemberRBooking { get; set; }
		public bool SentAgentEmail { get; set; }
		public bool SentCustEmail { get; set; }
		public bool AgentBusy { get; set; }
		public DateTime AgentTimeOut { get; set; }
		public int? RedirectLevel { get; set; }
		public string BussinessCode { get; set; }
		public int LimitUser { get; set; }
		public int ManageAPI { get; set; }
		public bool UseAppMobile { get; set; }
	}
}
