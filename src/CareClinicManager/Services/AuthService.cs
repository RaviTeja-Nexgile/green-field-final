using CareClinicManager.Data;
using CareClinicManager.Models;
using Microsoft.AspNetCore.Identity;

namespace CareClinicManager.Services;

/// <summary>
/// Handles sign-in verification and patient self-registration.
/// Passwords are stored only as PBKDF2 hashes via <see cref="PasswordHasher{TUser}"/>.
/// </summary>
public class AuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Verifies a user name and password. Returns the matching <see cref="User"/>
    /// when the credentials are correct, or <c>null</c> when they are not.
    /// </summary>
    public User? ValidateCredentials(string userName, string password)
    {
        var user = _db.Users.SingleOrDefault(u => u.UserName == userName);
        if (user is null)
        {
            return null;
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success ? user : null;
    }

    /// <summary>
    /// Registers a new Patient account. Returns <c>null</c> when the user name is
    /// already taken, otherwise the newly created <see cref="User"/>.
    /// </summary>
    public User? RegisterPatient(string userName, string password, string name,
        DateTime dateOfBirth, string gender, string phone, string email, string address)
    {
        if (_db.Users.Any(u => u.UserName == userName))
        {
            return null;
        }

        var patient = new Patient
        {
            Name = name,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            Phone = phone,
            Email = email,
            Address = address
        };
        _db.Patients.Add(patient);
        _db.SaveChanges();

        var user = new User
        {
            UserName = userName,
            Role = "Patient",
            PatientId = patient.Id
        };
        user.PasswordHash = _hasher.HashPassword(user, password);
        _db.Users.Add(user);
        _db.SaveChanges();

        return user;
    }

    /// <summary>
    /// Maps a role to the controller that hosts its dashboard.
    /// </summary>
    public static string DashboardControllerFor(string role) => role switch
    {
        "Admin" => "Admin",
        "Doctor" => "Doctor",
        "Receptionist" => "Receptionist",
        _ => "Patient"
    };
}
