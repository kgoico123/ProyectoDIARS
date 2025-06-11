using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProyectoDIARS.Models
{
    public class Tutor
    {
        public int IdTutor { get; set; }
        public string userId { get; set; }
        public string direccion { get; set; }
        public IdentityUser? user { get; set; }
        public ICollection<Estudiante>? Estudiantes { get; set; }
        public ICollection<Notificacion>? Notificaciones { get; set; }
    }
}
