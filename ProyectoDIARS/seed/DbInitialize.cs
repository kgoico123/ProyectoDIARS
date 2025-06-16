using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoDIARS.Data;
using ProyectoDIARS.Models;
using ProyectoDIARS.shared;

namespace ProyectoDIARS.seed;

public class DbInitialize : IDbInitialize
{
    private readonly AppDBContext _context;
    private readonly UserManager<ApplicationUser> _usertutor;
    private readonly RoleManager<IdentityRole> _userRole;
    public DbInitialize(AppDBContext context, RoleManager<IdentityRole> userRole, UserManager<ApplicationUser> usertutor)
    {
        _context = context;
        _usertutor = usertutor;
        _userRole = userRole;
    }
    public void Initialize()
    {
        try
        {
            //if(_context.Database.GetPendingMigrations().Count() > 0)
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception)
        {
            throw;
        }

        if (_context.Roles.Any()) return;

        _userRole.CreateAsync(new IdentityRole(VCG.Role_Admin)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Estudiante)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Tutor)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Docente)).GetAwaiter().GetResult();

        var admin = new ApplicationUser
        {
            Email = "admin@dev.cs",
            UserName = "admin@dev.cs",
            PhoneNumber = "123456789",
        };

        var estudiante = new ApplicationUser
        {
            Email = "estudiante@dev.cs",
            UserName = "estudiante@dev.cs",
            PhoneNumber = "123456789",
        };

        var tutor = new ApplicationUser
        {
            Email = "tutor@dev.cs",
            UserName = "tutor@dev.cs",
            PhoneNumber = "123456789",
        };

        var docente = new ApplicationUser
        {
            Email = "docente@dev.cs",
            UserName = "docente@dev.cs",
            PhoneNumber = "123456789",
        };

        _usertutor.CreateAsync(admin, "Admin123*").GetAwaiter().GetResult();
        _usertutor.CreateAsync(estudiante, "Estudiante123*").GetAwaiter().GetResult();
        _usertutor.CreateAsync(tutor, "Tutor123*").GetAwaiter().GetResult();
        _usertutor.CreateAsync(docente, "Docente123*").GetAwaiter().GetResult();

        _usertutor.AddToRoleAsync(admin, VCG.Role_Admin).GetAwaiter().GetResult();
        _usertutor.AddToRoleAsync(tutor, VCG.Role_Tutor).GetAwaiter().GetResult();
        _usertutor.AddToRoleAsync(estudiante, VCG.Role_Estudiante).GetAwaiter().GetResult();
        _usertutor.AddToRoleAsync(docente, VCG.Role_Docente).GetAwaiter().GetResult();
    }
}
