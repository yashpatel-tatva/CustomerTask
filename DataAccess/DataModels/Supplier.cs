using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerTask;

[Table("Supplier")]
public partial class Supplier
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = string.Empty;

    public int? GroupId { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Suppliers")]
    public virtual Group? Group { get; set; }
}
