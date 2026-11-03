using System.ComponentModel.DataAnnotations;

namespace CareClinicManager.ViewModels.Account;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "User name")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Full name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of birth")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Display(Name = "Gender")]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [Phone]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Address")]
    public string Address { get; set; } = string.Empty;
}
