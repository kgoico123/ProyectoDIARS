using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Identity;

namespace ProyectoDIARS.Models
{
    public class Docente
    {
        public int IdDocente { get; set; }
        public string userId { get; set; }
        public IdentityUser? user { get; set; }
        public Curso? Curso { get; set; }
       
    }
}
