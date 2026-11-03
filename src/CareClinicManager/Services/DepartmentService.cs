using CareClinicManager.Data;
using CareClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Services;

/// <summary>
/// Business rules for departments and specializations: list with search,
/// sorting, filtering and pagination, plus create, update and delete.
/// </summary>
public class DepartmentService
{
    private readonly AppDbContext _db;

    public DepartmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Department>> GetDepartmentsAsync(
        string? search, string? sort, string? filter, int page, int pageSize)
    {
        var query = _db.Departments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(d => d.Name.StartsWith(filter));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(d => d.Name.ToLower().Contains(term));
        }

        query = sort switch
        {
            "name_desc" => query.OrderByDescending(d => d.Name),
            _ => query.OrderBy(d => d.Name),
        };

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountDepartmentsAsync(string? search, string? filter)
    {
        var query = _db.Departments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(d => d.Name.StartsWith(filter));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(d => d.Name.ToLower().Contains(term));
        }

        return await query.CountAsync();
    }

    public async Task<List<Specialization>> GetSpecializationsAsync(
        string? search, string? sort, string? filter, int page, int pageSize)
    {
        var query = _db.Specializations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s => s.Name.StartsWith(filter));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(term));
        }

        query = sort switch
        {
            "name_desc" => query.OrderByDescending(s => s.Name),
            _ => query.OrderBy(s => s.Name),
        };

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountSpecializationsAsync(string? search, string? filter)
    {
        var query = _db.Specializations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s => s.Name.StartsWith(filter));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(term));
        }

        return await query.CountAsync();
    }

    public async Task<Department?> GetDepartmentAsync(int id)
    {
        return await _db.Departments.FindAsync(id);
    }

    public async Task<Specialization?> GetSpecializationAsync(int id)
    {
        return await _db.Specializations.FindAsync(id);
    }

    public async Task<Department> CreateDepartmentAsync(string name)
    {
        var department = new Department { Name = name.Trim() };
        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return department;
    }

    public async Task<Specialization> CreateSpecializationAsync(string name)
    {
        var specialization = new Specialization { Name = name.Trim() };
        _db.Specializations.Add(specialization);
        await _db.SaveChangesAsync();
        return specialization;
    }

    public async Task UpdateDepartmentAsync(int id, string name)
    {
        var department = await _db.Departments.FindAsync(id);
        if (department is null)
        {
            return;
        }

        department.Name = name.Trim();
        await _db.SaveChangesAsync();
    }

    public async Task UpdateSpecializationAsync(int id, string name)
    {
        var specialization = await _db.Specializations.FindAsync(id);
        if (specialization is null)
        {
            return;
        }

        specialization.Name = name.Trim();
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a department when no doctor, staff member or appointment
    /// references it. Returns true when the record was removed, false when
    /// it is still referenced (the FK is DeleteBehavior.Restrict).
    /// </summary>
    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        var department = await _db.Departments.FindAsync(id);
        if (department is null)
        {
            return false;
        }

        var referenced = await _db.Doctors.AnyAsync(d => d.DepartmentId == id)
            || await _db.Staff.AnyAsync(s => s.DepartmentId == id)
            || await _db.Appointments.AnyAsync(a => a.DepartmentId == id);

        if (referenced)
        {
            return false;
        }

        _db.Departments.Remove(department);
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes a specialization when no doctor references it. Returns true
    /// when the record was removed, false when it is still referenced.
    /// </summary>
    public async Task<bool> DeleteSpecializationAsync(int id)
    {
        var specialization = await _db.Specializations.FindAsync(id);
        if (specialization is null)
        {
            return false;
        }

        var referenced = await _db.Doctors.AnyAsync(d => d.SpecializationId == id);

        if (referenced)
        {
            return false;
        }

        _db.Specializations.Remove(specialization);
        await _db.SaveChangesAsync();
        return true;
    }
}
