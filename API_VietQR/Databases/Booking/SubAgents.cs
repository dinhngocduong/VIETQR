namespace API_VietQR.Databases.Booking
{
    public class SubAgents
    {
        public long ID { get; set; }
        public long ParentAgent { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public bool Active { get; set; }
        public string? AgentLogo { get; set; }
        public string? SignIn { get; set; }
        public string? Password { get; set; }
        public string? AirCode { get; set; }
        public bool? Deleted { get; set; }
    }
}
