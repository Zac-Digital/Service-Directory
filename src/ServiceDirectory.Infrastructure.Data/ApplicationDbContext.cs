using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Domain.Service;

namespace ServiceDirectory.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Organisation> Organisations { get; init; } = null!;
    IQueryable<Organisation> IApplicationDbContext.Organisations => Organisations;

    public DbSet<Service> Services { get; init; } = null!;
    IQueryable<Service> IApplicationDbContext.Services => Services;

    public DbSet<Location> Locations { get; init; } = null!;
    
    public void AddOrganisationRange(List<Organisation> organisationList) => Organisations.AddRange(organisationList);
    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

    IQueryable<Location> IApplicationDbContext.Locations => Locations;

    public DbSet<Contact> Contacts { get; init; } = null!;
    public DbSet<Schedule> Schedules { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Organisation - Table Definition & Relationship Map
        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<Organisation>()
            .HasMany(e => e.Services)
            .WithOne()
            .HasForeignKey(e => e.OrganisationId)
            .IsRequired();
        // Organisation - Table Definition & Relationship Map

        // Service - Table Definition & Relationship Map
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OrganisationId).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Cost).IsRequired().HasMaxLength(5);
        });

        modelBuilder.Entity<Service>()
            .HasOne(e => e.Contact)
            .WithOne()
            .IsRequired();
        
        modelBuilder.Entity<Service>()
            .HasOne(e => e.Schedule)
            .WithOne()
            .IsRequired();
        
        modelBuilder.Entity<Service>()
            .HasMany(e => e.Locations)
            .WithMany().UsingEntity<Dictionary<string, object>>(entity =>
            {
                entity.Metadata.SetTableName(nameof(Service) + nameof(Location));
            });
        // Service - Table Definition & Relationship Map

        // Contact - Table Definition & Relationship Map
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Website).IsRequired();
        });
        // Contact - Table Definition & Relationship Map

        // Schedule - Table Definition & Relationship Map
        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OpeningTime).IsRequired();
            entity.Property(e => e.ClosingTime).IsRequired();
            entity.Property(e => e.DaysOfWeek).IsRequired().HasMaxLength(20);
        });
        // Schedule - Table Definition & Relationship Map

        // Location - Table Definition & Relationship Map
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.AddressLineOne).IsRequired();
            entity.Property(e => e.AddressLineTwo).IsRequired();
            entity.Property(e => e.County).IsRequired();
            entity.Property(e => e.TownOrCity).IsRequired();
            entity.Property(e => e.Postcode).IsRequired().HasMaxLength(8);
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
        });
        // Location - Table Definition & Relationship Map

        base.OnModelCreating(modelBuilder);
    }
}