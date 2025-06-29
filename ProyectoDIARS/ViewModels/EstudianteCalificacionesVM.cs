using System.Collections.Generic;
using ProyectoDIARS.Models;

namespace ProyectoDIARS.ViewModels
{
    public class EstudianteCalificacionesVM
    {
        public string NombreCurso { get; set; }
        public string NombreDocente { get; set; }
        public string Horario { get; set; }
        public List<Calificacion> Calificaciones { get; set; }
        public int? promedioAcumulado { get; set; }
    }
}