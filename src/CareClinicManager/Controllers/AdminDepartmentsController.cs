using CareClinicManager.Services;
using CareClinicManager.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareClinicManager.Controllers;

[Authorize(Roles = "Admin")]
public class AdminDepartmentsController : Controller
{
    private readonly DepartmentService _service;

    public AdminDepartmentsController(DepartmentService service)
    {
        _service = service;
    }

    // ---- Departments ----

    [HttpGet]
    public async Task<IActionResult> Index(string? search, string? sort, string? filter, int page = 1)
    {
        const int pageSize = 10;
        if (page < 1)
        {
            page = 1;
        }

        var items = await _service.GetDepartmentsAsync(search, sort, filter, page, pageSize);
        var total = await _service.CountDepartmentsAsync(search, filter);

        var model = new DepartmentListViewModel
        {
            Items = items.Select(d => new DepartmentListItemViewModel { Id = d.Id, Name = d.Name }).ToList(),
            Search = search,
            Sort = sort,
            Filter = filter,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
        };

        return View("~/Views/Admin/Departments/Index.cshtml", model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Departments/Create.cshtml", new DepartmentFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Departments/Create.cshtml", model);
        }

        await _service.CreateDepartmentAsync(model.Name);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _service.GetDepartmentAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Departments/Edit.cshtml", new DepartmentFormViewModel { Id = department.Id, Name = department.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Departments/Edit.cshtml", model);
        }

        await _service.UpdateDepartmentAsync(id, model.Name);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteDepartmentAsync(id);
        if (!deleted)
        {
            TempData["Error"] = "This department cannot be deleted because doctors, staff or appointments are assigned to it.";
        }

        return RedirectToAction(nameof(Index));
    }

    // ---- Specializations ----

    [HttpGet]
    public async Task<IActionResult> Specializations(string? search, string? sort, string? filter, int page = 1)
    {
        const int pageSize = 10;
        if (page < 1)
        {
            page = 1;
        }

        var items = await _service.GetSpecializationsAsync(search, sort, filter, page, pageSize);
        var total = await _service.CountSpecializationsAsync(search, filter);

        var model = new SpecializationListViewModel
        {
            Items = items.Select(s => new DepartmentListItemViewModel { Id = s.Id, Name = s.Name }).ToList(),
            Search = search,
            Sort = sort,
            Filter = filter,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
        };

        return View("~/Views/Admin/Specializations/Index.cshtml", model);
    }

    [HttpGet]
    public IActionResult CreateSpecialization()
    {
        return View("~/Views/Admin/Specializations/Create.cshtml", new DepartmentFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSpecialization(DepartmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Specializations/Create.cshtml", model);
        }

        await _service.CreateSpecializationAsync(model.Name);
        return RedirectToAction(nameof(Specializations));
    }

    [HttpGet]
    public async Task<IActionResult> EditSpecialization(int id)
    {
        var specialization = await _service.GetSpecializationAsync(id);
        if (specialization is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Specializations/Edit.cshtml", new DepartmentFormViewModel { Id = specialization.Id, Name = specialization.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSpecialization(int id, DepartmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Specializations/Edit.cshtml", model);
        }

        await _service.UpdateSpecializationAsync(id, model.Name);
        return RedirectToAction(nameof(Specializations));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSpecialization(int id)
    {
        var deleted = await _service.DeleteSpecializationAsync(id);
        if (!deleted)
        {
            TempData["Error"] = "This specialization cannot be deleted because doctors are assigned to it.";
        }

        return RedirectToAction(nameof(Specializations));
    }
}
