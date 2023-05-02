using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ResidueContext : IdentityDbContext
{
    public ResidueContext()
    {
    }


    public DbSet<Municipality> Municipalities { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supervisor> Supervisors { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Contractor> Contractors { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Cargo> Cargoes { get; set; }
    public DbSet<CargoItem> CargoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-EMARSEM; Database=MunicipalityResidue; User Id=smh; Password=1234");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Cargo>(entity =>
        {
            entity.HasOne(l => l.Customer)
                .WithMany(l => l.Cargoes)
                .HasForeignKey(l => l.CustomerId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            entity.HasOne(l => l.Supervisor)
                .WithMany(l => l.Cargoes)
                .HasForeignKey(l => l.SupervisorId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            entity.HasOne(l => l.Supervisor)
                .WithMany(l => l.Cargoes)
                .HasForeignKey(l => l.SupervisorId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            entity.HasMany(l => l.CargoItems)
                .WithOne(l => l.Cargo)
                .HasForeignKey(l => l.CargoId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
        });

        builder.Entity<Municipality>().HasMany(l => l.Contractors)
            .WithOne(l => l.Municipality)
            .HasForeignKey(l => l.MunicipalityId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

        builder.Entity<Municipality>().HasMany(l => l.Supervisors)
            .WithOne(l => l.Municipality)
            .HasForeignKey(l => l.MunicipalityId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

        builder.Entity<Contractor>().HasMany(l => l.Employees)
            .WithOne(l => l.Contractor)
            .HasForeignKey(l => l.ContrantorId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

        builder.Ignore<IdentityUser>();
        
        builder.Entity<CargoItem>().Property(u => u.Type).HasConversion<string>();


        builder.Entity<Municipality>().ToTable("Municipalities").HasBaseType<BaseUser>();
        builder.Entity<Contractor>().ToTable("Contractors").HasBaseType<BaseUser>();
        builder.Entity<Customer>().ToTable("Customers").HasBaseType<BaseUser>();
        builder.Entity<Supervisor>().ToTable("Supervisors").HasBaseType<BaseUser>();
        builder.Entity<Employee>().ToTable("Employees").HasBaseType<BaseUser>();


        // builder.Ignore<Municipality>();
        // builder.Ignore<Contractor>();
        // builder.Ignore<Customer>();
        // builder.Ignore<Supervisor>();
        // builder.Ignore<Employee>();
    }
}