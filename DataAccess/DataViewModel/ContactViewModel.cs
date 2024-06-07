namespace DataAccess.DataViewModel
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string Name { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? MailingList { get; set; }
        public bool Isdelete { get; set; }
        public int? CustomerId { get; set; }
    }
}
