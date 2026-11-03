using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class AccountTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly AuthService _auth;

    public AccountTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-account-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _auth = new AuthService(_db);
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
    public void ValidateCredentials_ReturnsUser_ForCorrectPassword()
    {
        DbSeeder.Seed(_db);

        var user = _auth.ValidateCredentials("admin", "Admin@123");

        Assert.NotNull(user);
        Assert.Equal("Admin", user!.Role);
    }

    [Fact]
    public void ValidateCredentials_ReturnsNull_ForWrongPassword()
    {
        DbSeeder.Seed(_db);

        var user = _auth.ValidateCredentials("admin", "wrong-password");

        Assert.Null(user);
    }

    [Fact]
    public void ValidateCredentials_ReturnsNull_ForUnknownUser()
    {
        DbSeeder.Seed(_db);

        var user = _auth.ValidateCredentials("nobody", "whatever");

        Assert.Null(user);
    }

    [Fact]
    public void RegisterPatient_CreatesPatientAndHashedLogin()
    {
        var user = _auth.RegisterPatient(
            "newpatient", "NewPatient@123", "Grace Hopper",
            new DateTime(1992, 1, 15), "Female", "555-0301",
            "grace@example.com", "1 Main Street");

        Assert.NotNull(user);
        Assert.Equal("Patient", user!.Role);
        Assert.NotNull(user.PatientId);

        var stored = _db.Users.Single(u => u.UserName == "newpatient");
        Assert.NotEqual("NewPatient@123", stored.PasswordHash);
        Assert.DoesNotContain("NewPatient@123", stored.PasswordHash);

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(stored, stored.PasswordHash, "NewPatient@123");
        Assert.Equal(PasswordVerificationResult.Success, result);

        var patient = _db.Patients.Single(p => p.Id == user.PatientId);
        Assert.Equal("Grace Hopper", patient.Name);
    }

    [Fact]
    public void RegisterPatient_ReturnsNull_WhenUserNameTaken()
    {
        DbSeeder.Seed(_db);

        var user = _auth.RegisterPatient(
            "admin", "Whatever@123", "Someone Else",
            new DateTime(1990, 1, 1), "Female", "555-0000",
            "someone@example.com", "Nowhere");

        Assert.Null(user);
    }

    [Fact]
    public void RegisterPatient_StoresPasswordAsHash_NeverPlainText()
    {
        _auth.RegisterPatient(
            "hashcheck", "HashCheck@123", "Ada Lovelace",
            new DateTime(1980, 2, 2), "Female", "555-0404",
            "ada@example.com", "2 Logic Lane");

        var stored = _db.Users.Single(u => u.UserName == "hashcheck");
        Assert.NotEqual(string.Empty, stored.PasswordHash);
        Assert.DoesNotContain("HashCheck@123", stored.PasswordHash);
    }

    [Theory]
    [InlineData("Admin", "Admin")]
    [InlineData("Doctor", "Doctor")]
    [InlineData("Receptionist", "Receptionist")]
    [InlineData("Patient", "Patient")]
    public void DashboardControllerFor_MapsRoleToController(string role, string expected)
    {
        Assert.Equal(expected, AuthService.DashboardControllerFor(role));
    }
}
