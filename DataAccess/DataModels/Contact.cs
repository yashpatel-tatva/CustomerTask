using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerTask;

[Table("Contact")]
public partial class Contact
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? Username { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; }

    [Column(TypeName = "character varying")]
    public string? Telephone { get; set; }

    [Column(TypeName = "character varying")]
    public string? Email { get; set; }

    [Column("Mailing List", TypeName = "character varying")]
    public string? MailingList { get; set; }

    public bool Isdelete { get; set; }

    public int? CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Contacts")]
    public virtual Customer? Customer { get; set; }
}
