using Microsoft.EntityFrameworkCore;

namespace CustomerTask;

public partial class CustomerDbContext : DbContext
{
    public CustomerDbContext()
    {
    }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Mapping> Mappings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID = postgres;Password=yashbpatel;Server=localhost;Port=5432;Database=Customer;Integrated Security=true;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Contact_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Contacts).HasConstraintName("Contact_CustomerId_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.Property(e => e.Isdelete).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Group_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Groups).HasConstraintName("Group_CustomerId_fkey");
        });

        modelBuilder.Entity<Mapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Mapping_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Mappings).HasConstraintName("withcustomer");

            entity.HasOne(d => d.Group).WithMany(p => p.Mappings).HasConstraintName("withgroup");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Supplier_pkey");

            entity.HasOne(d => d.Group).WithMany(p => p.Suppliers).HasConstraintName("withgroup");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
