using CareClinicManager.Data;
using CareClinicManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class DbSeederTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;

    public DbSeederTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _db.Dispose();
        SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }

    [Fact]
    public void Seed_CreatesOneLoginPerRole()
    {
        DbSeeder.Seed(_db);

        var roles = _db.Users.Select(u => u.Role).OrderBy(r => r).ToList();
        Assert.Equal(new[] { "Admin", "Doctor", "Patient", "Receptionist" }, roles);
    }

    [Fact]
    public void Seed_StoresPasswordsAsHashes_NotPlainText()
    {
        DbSeeder.Seed(_db);

        var hasher = new PasswordHasher<User>();
        foreach (var user in _db.Users)
        {
            Assert.NotEqual(string.Empty, user.PasswordHash);
            Assert.DoesNotContain("123", user.PasswordHash);
        }

        var admin = _db.Users.Single(u => u.UserName == "admin");
        var result = hasher.VerifyHashedPassword(admin, admin.PasswordHash, "Admin@123");
        Assert.Equal(PasswordVerificationResult.Success, result);
    }

    [Fact]
    public void Seed_IsIdempotent_LeavesExistingDataUntouched()
    {
        DbSeeder.Seed(_db);
        var userCount = _db.Users.Count();

        DbSeeder.Seed(_db);

        Assert.Equal(userCount, _db.Users.Count());
    }

    [Fact]
    public void Seed_LinksPatientLoginToPatientRecord()
    {
        DbSeeder.Seed(_db);

        var patientUser = _db.Users.Single(u => u.UserName == "patient");
        Assert.NotNull(patientUser.PatientId);
        Assert.NotNull(_db.Patients.Find(patientUser.PatientId));
    }
}
