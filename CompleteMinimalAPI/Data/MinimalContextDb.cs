namespace CompleteMinimalAPI.Data;
using CompleteMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

public class MinimalContextDb : DbContext
{
    public MinimalContextDb(DbContextOptions options) : base(options) { }

    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>().HasKey(p => p.Id);
        modelBuilder.Entity<Provider>().Property(p => p.Name).IsRequired().HasColumnType("varchar(200)");
        modelBuilder.Entity<Provider>().Property(p => p.Document).IsRequired().HasColumnType("varchar(14)");
        modelBuilder.Entity<Provider>().ToTable("Providers");
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("DataSource = productCatalog; Cache=Shared");
}

