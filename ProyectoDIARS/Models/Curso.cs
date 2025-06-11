using System.ComponentModel.DataAnnotations;

namespace ProyectoDIARS.Models
{
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }

        public string Nombre { get; set; }
        public DateTime horario { get; set; }
        public string aula { get; set; }

        public int DocenteId { get; set; }

        public ICollection<Docente> Docentes { get; set; }

        public Estudiante_Curso estudiante_Curso { get; set; }
    }
}
