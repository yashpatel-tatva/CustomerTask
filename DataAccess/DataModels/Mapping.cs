using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CustomerTask;

[Table("Mapping")]
public partial class Mapping
{
    [Key]
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? GroupId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Mappings")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Mappings")]
    public virtual Group? Group { get; set; }
}
