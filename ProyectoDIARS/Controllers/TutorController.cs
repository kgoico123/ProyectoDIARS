using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.Data;
using ProyectoDIARS.ViewModels;

namespace ProyectoDIARS.Controllers
{
    public class TutorController : Controller
    {
        private readonly AppDBContext _context;

        public TutorController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard(int id)
        {
            // Aquí debes obtener los datos reales de la BD.
            // Ejemplo con datos simulados:
            var model = new DashboardPadreViewModel
            {
                NombrePadre = "Juan Pérez",
                NombreEstudiante = "Juan Pérez",
                Notas = new List<NotaEstudianteVM>
                {
                    new NotaEstudianteVM
                    {
                        Curso = "Matemáticas",
                        DesempenoColor = "verde",
                        Docente = "Prof. María González",
                        Recomendacion = "Muy bien en las matemáticas. Mantener el enfoque."
                    },
                    new NotaEstudianteVM
                    {
                        Curso = "Lengua y Literatura",
                        DesempenoColor = "amarillo",
                        Docente = "Prof. Juan López",
                        Recomendacion = "Debe mejorar en la comprensión de lectura. Se recomienda leer más."
                    },
                    new NotaEstudianteVM
                    {
                        Curso = "Ciencias Sociales",
                        DesempenoColor = "rojo",
                        Docente = "Prof. Marta Díaz",
                        Recomendacion = "Necesita trabajar más en la participación en clase."
                    }
                }
            };
            return View(model);
        }
    }
}
