using System.ComponentModel.DataAnnotations;
using CareClinicManager.Models;

namespace CareClinicManager.ViewModels.Admin;

/// <summary>
/// A single department or specialization row shown in a list.
/// </summary>
public class DepartmentListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// The departments list screen: rows plus search/sort/filter/pagination state.
/// </summary>
public class DepartmentListViewModel
{
    public List<DepartmentListItemViewModel> Items { get; set; } = new();

    public string? Search { get; set; }
    public string? Sort { get; set; }
    public string? Filter { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }

    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}

/// <summary>
/// The specializations list screen: rows plus search/sort/filter/pagination state.
/// </summary>
public class SpecializationListViewModel
{
    public List<DepartmentListItemViewModel> Items { get; set; } = new();

    public string? Search { get; set; }
    public string? Sort { get; set; }
    public string? Filter { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }

    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}

/// <summary>
/// The create/edit form for a department or specialization. The same shape is
/// used for both, distinguished by the controller action that renders it.
/// </summary>
public class DepartmentFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
