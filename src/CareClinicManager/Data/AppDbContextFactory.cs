using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CareClinicManager.Data;

/// <summary>
/// Design-time factory so `dotnet ef migrations add` can build the model
/// without running the app (which would otherwise try to migrate/seed).
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=careclinic.db")
            .Options;
        return new AppDbContext(options);
    }
}
