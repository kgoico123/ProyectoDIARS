using ProyectoDIARS.Models;
using System.Collections.Generic;

namespace ProyectoDIARS.ViewModels;

public class DocenteCalificacionesVM
{
    public Docente docente { get; set; }
    public List<Secciones> secciones { get; set; }
    public string seccion { get; set; }
    public List<int> alumnosId { get; set; }
    public List<string> notas { get; set; }
    public List<string> comentarios { get; set; }

}

public class Secciones
{
    public string Grado { get; set; }
    public string Seccion { get; set; }
    public IEnumerable<AlumnoCalificacionVM> Alumnos { get; set; } // Cambiado de Estudiante_Curso
}

// Nueva clase para representar los datos del alumno en esta vista
public class AlumnoCalificacionVM
{
    public int IdEstudiante { get; set; }
    public string UserId { get; set; }
    public string Nombre { get; set; }
}
