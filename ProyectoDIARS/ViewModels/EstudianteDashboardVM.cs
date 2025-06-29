using System.Collections.Generic;

namespace ProyectoDIARS.ViewModels
{
    public class EstudianteDashboardVM
    {
        
        public string NombreEstudiante { get; set; }
        public List<CursoEstudianteVM> Cursos { get; set; } = new List<CursoEstudianteVM>();
    }
}