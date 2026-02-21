using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CryoTracking.Infrastructure.Persistence;

public class CryoDbContextFactory
    : IDesignTimeDbContextFactory<CryoDbContext>
{
    public CryoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CryoDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=DMRYRKHTC;Database=CryoTrackingDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new CryoDbContext(optionsBuilder.Options);
    }
}