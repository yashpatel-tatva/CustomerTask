using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerTask;

[Table("Group")]
public partial class Group
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    public bool Isdelete { get; set; }

    public bool Isselect { get; set; }

    public int? CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Groups")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<Mapping> Mappings { get; set; } = new List<Mapping>();

    [InverseProperty("Group")]
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
