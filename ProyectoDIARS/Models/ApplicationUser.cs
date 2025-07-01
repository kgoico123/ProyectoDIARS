using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Identity;

namespace ProyectoDIARS.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? Dni { get; set; }
        public string? Apellido { get; set; }

    }
}
