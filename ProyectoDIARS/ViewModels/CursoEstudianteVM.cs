using System.Collections.Generic;

namespace ProyectoDIARS.ViewModels
{
    public class CursoEstudianteVM
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public string Aula { get; set; }
        public string Horario { get; set; }
        public string NombreDocente { get; set; }
        public int? PromedioAcumulado { get; set; }
    }
}