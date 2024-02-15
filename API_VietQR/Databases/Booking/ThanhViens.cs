using Dapper.Contrib.Extensions;

namespace API_VietQR.Databases.Booking
{
	[Table("tbl_ThanhVien")]
	public partial class ThanhViens
	{
		public Int64 ID { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public string? HoTen { get; set; }
		public DateTime? NgaySinh { get; set; }
		public bool? GioiTinh { get; set; }
		public string? DienThoai { get; set; }
		public DateTime? NgayDangKy { get; set; }
		public string? Email { get; set; }
		public Int64 AgentID { get; set; }
		public bool AgentAdmin { get; set; }
		public string? AgentImage { get; set; }
		public bool ChangePassNextLogin { get; set; }
		public bool Active { get; set; }
		public string? LastIP { get; set; }
		public int AccountType { get; set; }
		public Int64 Permission { get; set; }
		public Int64 SubAgent { get; set; }
		public bool? Deleted { get; set; }
		public Int64? MemberID_Created { get; set; }
		public Int64? MemberID_Update { get; set; }
		public string? PINCODE { get; set; }
		public Int64? LoginFail { get; set; }
		public Int64? IsFirstLogin { get; set; }
		public long SubAgentF3 { get; set; }
	}
}
