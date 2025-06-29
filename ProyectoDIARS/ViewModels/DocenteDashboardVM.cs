using ProyectoDIARS.Models;

namespace ProyectoDIARS.ViewModels;

public class DocenteDashboardVM
{
    public Docente docente { get; set; }
    public Curso curso { get; set; }
    public int CantidadAlumnos { get; set; }
}
