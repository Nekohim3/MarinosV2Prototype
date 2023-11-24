using MarinosV2Prototype.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace MarinosV2Prototype;

public class MarinosContext : DbContext
{
    public DbSet<SmsPartition>      SmsPartitions      { get; set; }
    public DbSet<SmsDocument>       SmsDocuments       { get; set; }
    public DbSet<SmsDocumentChange> SmsDocumentChanges { get; set; }
    public DbSet<SmsDocumentFile>   SmsDocumentFiles   { get; set; }

    public bool      IsValid   { get; set; }
    public Exception Exception { get; set; }

    public MarinosContext(bool resetDatabase = false)
    {
        try
        {
            if (resetDatabase)
                Database.EnsureDeleted();

            if (Database.EnsureCreated())
            {

            }

            IsValid = true;
        }
        catch (Exception e)
        {
            IsValid   = false;
            Exception = e;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (DatabaseConnection.DatabaseSettings == null)
            return;
        optionsBuilder.UseNpgsql($"Host={DatabaseConnection.DatabaseSettings.DatabaseHost};"         +
                                 $"Port={DatabaseConnection.DatabaseSettings.DatabasePort};"         +
                                 $"Database={DatabaseConnection.DatabaseSettings.DatabaseName};"     +
                                 $"Username={DatabaseConnection.DatabaseSettings.DatabaseUsername};" +
                                 $"Password={DatabaseConnection.DatabaseSettings.DatabasePassword}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SmsPartition>().HasOne(_ => _.Parent).WithMany(_ => _.Childs).HasForeignKey(_ => _.IdParent);
        modelBuilder.Entity<SmsPartition>().HasMany(_ => _.Documents).WithOne(_ => _.Partition).HasForeignKey(_ => _.IdPartition);

        modelBuilder.Entity<SmsDocument>().HasMany(_ => _.DocumentChanges).WithOne(_ => _.Document).HasForeignKey(_ => _.IdDocument);
        modelBuilder.Entity<SmsDocument>().HasMany(_ => _.DocumentFiles).WithOne(_ => _.Document).HasForeignKey(_ => _.IdDocument);
    }

    public void Test()
    {

    }
}