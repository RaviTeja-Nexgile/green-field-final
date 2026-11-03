using CareClinicManager.Data;
using CareClinicManager.Models;
using CareClinicManager.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareClinicManager.Tests;

public class DepartmentManagementTests : IDisposable
{
    private readonly string _dbPath;
    private readonly AppDbContext _db;
    private readonly DepartmentService _service;

    public DepartmentManagementTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"careclinic-dept-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _service = new DepartmentService(_db);
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

    // ---- Departments ----

    [Fact]
    public async Task CreateDepartment_SavesAndCanBeListed()
    {
        await _service.CreateDepartmentAsync("Cardiology");

        var list = await _service.GetDepartmentsAsync(null, null, null, 1, 10);
        Assert.Single(list);
        Assert.Equal("Cardiology", list[0].Name);
    }

    [Fact]
    public async Task CreateDepartment_TrimsName()
    {
        await _service.CreateDepartmentAsync("  Cardiology  ");

        var list = await _service.GetDepartmentsAsync(null, null, null, 1, 10);
        Assert.Equal("Cardiology", list[0].Name);
    }

    [Fact]
    public async Task GetDepartments_SearchFiltersByName()
    {
        await _service.CreateDepartmentAsync("Cardiology");
        await _service.CreateDepartmentAsync("Dermatology");

        var list = await _service.GetDepartmentsAsync("cardio", null, null, 1, 10);
        Assert.Single(list);
        Assert.Equal("Cardiology", list[0].Name);
    }

    [Fact]
    public async Task GetDepartments_SortsDescending()
    {
        await _service.CreateDepartmentAsync("Cardiology");
        await _service.CreateDepartmentAsync("Dermatology");

        var list = await _service.GetDepartmentsAsync(null, "name_desc", null, 1, 10);
        Assert.Equal("Dermatology", list[0].Name);
        Assert.Equal("Cardiology", list[1].Name);
    }

    [Fact]
    public async Task GetDepartments_FiltersByStartsWith()
    {
        await _service.CreateDepartmentAsync("Cardiology");
        await _service.CreateDepartmentAsync("Dermatology");

        var list = await _service.GetDepartmentsAsync(null, null, "C", 1, 10);
        Assert.Single(list);
        Assert.Equal("Cardiology", list[0].Name);
    }

    [Fact]
    public async Task GetDepartments_Paginates()
    {
        for (var i = 1; i <= 25; i++)
        {
            await _service.CreateDepartmentAsync($"Department {i:D2}");
        }

        var page1 = await _service.GetDepartmentsAsync(null, null, null, 1, 10);
        var page3 = await _service.GetDepartmentsAsync(null, null, null, 3, 10);

        Assert.Equal(10, page1.Count);
        Assert.Equal(5, page3.Count);
        Assert.Equal(25, await _service.CountDepartmentsAsync(null, null));
    }

    [Fact]
    public async Task UpdateDepartment_ChangesName()
    {
        var created = await _service.CreateDepartmentAsync("Cardiology");

        await _service.UpdateDepartmentAsync(created.Id, "Cardiology Updated");

        var updated = await _service.GetDepartmentAsync(created.Id);
        Assert.Equal("Cardiology Updated", updated!.Name);
    }

    [Fact]
    public async Task DeleteDepartment_RemovesFromList()
    {
        var created = await _service.CreateDepartmentAsync("Cardiology");

        var deleted = await _service.DeleteDepartmentAsync(created.Id);

        Assert.True(deleted);
        var list = await _service.GetDepartmentsAsync(null, null, null, 1, 10);
        Assert.Empty(list);
    }

    [Fact]
    public async Task DeleteDepartment_RefusedWhenDoctorReferencesIt()
    {
        var department = await _service.CreateDepartmentAsync("Cardiology");
        var specialization = await _service.CreateSpecializationAsync("Cardiologist");
        _db.Doctors.Add(new Doctor
        {
            Name = "Dr. Smith",
            Role = "Doctor",
            DepartmentId = department.Id,
            SpecializationId = specialization.Id,
        });
        await _db.SaveChangesAsync();

        var deleted = await _service.DeleteDepartmentAsync(department.Id);

        Assert.False(deleted);
        Assert.NotNull(await _service.GetDepartmentAsync(department.Id));
    }

    [Fact]
    public async Task DeleteDepartment_RefusedWhenStaffReferencesIt()
    {
        var department = await _service.CreateDepartmentAsync("Cardiology");
        _db.Staff.Add(new Staff
        {
            Name = "Nurse Jane",
            Role = "Nurse",
            DepartmentId = department.Id,
        });
        await _db.SaveChangesAsync();

        var deleted = await _service.DeleteDepartmentAsync(department.Id);

        Assert.False(deleted);
        Assert.NotNull(await _service.GetDepartmentAsync(department.Id));
    }

    // ---- Specializations ----

    [Fact]
    public async Task CreateSpecialization_SavesAndCanBeListed()
    {
        await _service.CreateSpecializationAsync("Cardiologist");

        var list = await _service.GetSpecializationsAsync(null, null, null, 1, 10);
        Assert.Single(list);
        Assert.Equal("Cardiologist", list[0].Name);
    }

    [Fact]
    public async Task GetSpecializations_SearchFiltersByName()
    {
        await _service.CreateSpecializationAsync("Cardiologist");
        await _service.CreateSpecializationAsync("Dermatologist");

        var list = await _service.GetSpecializationsAsync("derm", null, null, 1, 10);
        Assert.Single(list);
        Assert.Equal("Dermatologist", list[0].Name);
    }

    [Fact]
    public async Task GetSpecializations_SortsDescending()
    {
        await _service.CreateSpecializationAsync("Cardiologist");
        await _service.CreateSpecializationAsync("Dermatologist");

        var list = await _service.GetSpecializationsAsync(null, "name_desc", null, 1, 10);
        Assert.Equal("Dermatologist", list[0].Name);
        Assert.Equal("Cardiologist", list[1].Name);
    }

    [Fact]
    public async Task GetSpecializations_FiltersByStartsWith()
    {
        await _service.CreateSpecializationAsync("Cardiologist");
        await _service.CreateSpecializationAsync("Dermatologist");

        var list = await _service.GetSpecializationsAsync(null, null, "D", 1, 10);
        Assert.Single(list);
        Assert.Equal("Dermatologist", list[0].Name);
    }

    [Fact]
    public async Task GetSpecializations_Paginates()
    {
        for (var i = 1; i <= 25; i++)
        {
            await _service.CreateSpecializationAsync($"Specialization {i:D2}");
        }

        var page1 = await _service.GetSpecializationsAsync(null, null, null, 1, 10);
        var page3 = await _service.GetSpecializationsAsync(null, null, null, 3, 10);

        Assert.Equal(10, page1.Count);
        Assert.Equal(5, page3.Count);
        Assert.Equal(25, await _service.CountSpecializationsAsync(null, null));
    }

    [Fact]
    public async Task UpdateSpecialization_ChangesName()
    {
        var created = await _service.CreateSpecializationAsync("Cardiologist");

        await _service.UpdateSpecializationAsync(created.Id, "Cardiologist Updated");

        var updated = await _service.GetSpecializationAsync(created.Id);
        Assert.Equal("Cardiologist Updated", updated!.Name);
    }

    [Fact]
    public async Task DeleteSpecialization_RemovesFromList()
    {
        var created = await _service.CreateSpecializationAsync("Cardiologist");

        var deleted = await _service.DeleteSpecializationAsync(created.Id);

        Assert.True(deleted);
        var list = await _service.GetSpecializationsAsync(null, null, null, 1, 10);
        Assert.Empty(list);
    }

    [Fact]
    public async Task DeleteSpecialization_RefusedWhenDoctorReferencesIt()
    {
        var department = await _service.CreateDepartmentAsync("Cardiology");
        var specialization = await _service.CreateSpecializationAsync("Cardiologist");
        _db.Doctors.Add(new Doctor
        {
            Name = "Dr. Smith",
            Role = "Doctor",
            DepartmentId = department.Id,
            SpecializationId = specialization.Id,
        });
        await _db.SaveChangesAsync();

        var deleted = await _service.DeleteSpecializationAsync(specialization.Id);

        Assert.False(deleted);
        Assert.NotNull(await _service.GetSpecializationAsync(specialization.Id));
    }
}
