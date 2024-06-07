namespace DataAccess.DataViewModel
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool Isdelete { get; set; }
        public bool Isselect { get; set; }
        public int? CustomerId { get; set; }
    }
}
