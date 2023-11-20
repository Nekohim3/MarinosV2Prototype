using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype
{
    public class MarinosContext : DbContext
    {
        public bool      IsValid   { get; set; }
        public Exception Exception { get; set; }

        public MarinosContext(bool resetDatabase = false)
        {
            try
            {
                if (resetDatabase)
                    Database.EnsureDeleted();

                Database.EnsureCreated();
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
    }
}
