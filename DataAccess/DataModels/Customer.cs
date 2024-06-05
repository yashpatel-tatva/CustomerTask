using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CustomerTask;

[Table("customer")]
public partial class Customer
{
    [Column("name", TypeName = "character varying")]
    public string Name { get; set; } = string.Empty;

    [Column("postcode", TypeName = "character varying")]
    public string? Postcode { get; set; }

    [Column("country", TypeName = "character varying")]
    public string? Country { get; set; }

    [Column("telephone", TypeName = "character varying")]
    public string? Telephone { get; set; }

    [Column("relation", TypeName = "character varying")]
    public string? Relation { get; set; }

    [Column("currency", TypeName = "character varying")]
    public string? Currency { get; set; }

    [Column("address1", TypeName = "character varying")]
    public string? Address1 { get; set; }

    [Column("address2", TypeName = "character varying")]
    public string? Address2 { get; set; }

    [Column("town", TypeName = "character varying")]
    public string? Town { get; set; }

    [Column("county", TypeName = "character varying")]
    public string? County { get; set; }

    [Column("email", TypeName = "character varying")]
    public string? Email { get; set; }

    [Column("issubscribe")]
    public bool? Issubscribe { get; set; }

    [Column("isdelete")]
    public bool? Isdelete { get; set; }

    [Column("AC", TypeName = "character varying")]
    public string Ac { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    [InverseProperty("Customer")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    [InverseProperty("Customer")]
    public virtual ICollection<Mapping> Mappings { get; set; } = new List<Mapping>();
}
