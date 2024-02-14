using Dapper.Contrib.Extensions;

namespace API_VietQR.Databases.Booking
{
    [Table("tbl_CN_NhatKy")]
    public class CN_NhatKys
    {
        public long ID { get; set; }
        public string Doc_No { get; set; }
        public string Doc_Type { get; set; }
        public string Doc_Title { get; set; }
        public DateTime Doc_Date { get; set; }
        public decimal Amount_NT { get; set; }
        public decimal ROE { get; set; }
        public decimal Amount_VND { get; set; }
        public long SubAgent { get; set; }
        public long MemberID { get; set; }
        public long AgentID { get; set; }
        public int Status { get; set; }
        public string AccType { get; set; }
        public decimal OtherFee { get; set; }
    }
}
