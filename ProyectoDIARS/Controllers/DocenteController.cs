using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.ViewModels;

namespace ProyectoDIARS.Controllers
{
    public class DocenteController : Controller
    {
        // Simulación de datos
        private static List<HorarioClaseVM> horarios = new()
        {
            new HorarioClaseVM { Id = 1, Grado = "5to de Primaria", Seccion = "A", HoraInicio = "08:00 AM", HoraFin = "09:30 AM" },
            new HorarioClaseVM { Id = 2, Grado = "5to de Primaria", Seccion = "B", HoraInicio = "10:00 AM", HoraFin = "11:30 AM" },
            new HorarioClaseVM { Id = 3, Grado = "6to de Secundaria", Seccion = "C", HoraInicio = "01:00 PM", HoraFin = "02:30 PM" }
        };

        private static List<SeccionNotasVM> secciones = new()
        {
            new SeccionNotasVM {
                Grado = "5to de Primaria", Seccion = "A",
                Alumnos = new List<AlumnoNotaVM>
                {
                    new AlumnoNotaVM { Id = 1, Nombre = "Laura Pérez" },
                    new AlumnoNotaVM { Id = 2, Nombre = "María Gómez" },
                    new AlumnoNotaVM { Id = 3, Nombre = "Lucía Rodríguez" }
                }
            },
            new SeccionNotasVM {
                Grado = "5to de Primaria", Seccion = "B",
                Alumnos = new List<AlumnoNotaVM>
                {
                    new AlumnoNotaVM { Id = 4, Nombre = "Ana López" },
                    new AlumnoNotaVM { Id = 5, Nombre = "Paola Morales" }
                }
            }
        };

        public IActionResult Dashboard(int? horarioId = null, string? seccion = null)
        {
            var vm = new DocenteDashboardVM
            {
                NombreDocente = "Prof. Andrés Pérez",
                Horarios = horarios,
                HorarioSeleccionado = horarioId.HasValue ? horarios.FirstOrDefault(h => h.Id == horarioId) : null,
                Secciones = secciones,
                SeccionSeleccionada = !string.IsNullOrEmpty(seccion) ? secciones.FirstOrDefault(s => s.Seccion == seccion) : null
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult SubirNotas(string seccion, List<AlumnoNotaVM> alumnos)
        {
            // Aquí deberías actualizar las notas en tu base de datos
            // Simulación: nada
            return RedirectToAction("Dashboard", new { seccion });
        }
    }
}

