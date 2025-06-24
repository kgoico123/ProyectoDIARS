using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.Data;
using ProyectoDIARS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ProyectoDIARS.Controllers
{
    public class DocenteController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocenteController(AppDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard(int? horarioId = null, string? seccion = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var docente = await _context.Docentes
                .Include(d => d.Curso)
                    .ThenInclude(c => c.estudiante_Curso)
                        .ThenInclude(ec => ec.Estudiante)
                            .ThenInclude(e => e.user)
                .FirstOrDefaultAsync(d => d.user.UserName == user.UserName);

            Console.WriteLine($"Docente encontrado: {docente?.IdDocente}");
            if (docente == null)
                return Unauthorized();

            // Usar el curso directamente desde el docente
            var curso = docente.Curso;

            // Obtener estudiantes por curso y sección
            var secciones = new List<object>();
            if (curso != null)
            {
                secciones.Add(new
                {
                    Grado = curso.Nombre,
                    Seccion = curso.aula,
                    Alumnos = curso.estudiante_Curso.Select(ec => new
                    {
                        ec.Estudiante.IdEstudiante,
                        ec.Estudiante.UserId,
                        Nombre = ec.Estudiante.user.UserName
                    }).ToList()
                });
            }
            Console.WriteLine($"Secciones encontradas: {secciones.Count}");

            ViewBag.Docente = docente;
            ViewBag.Secciones = secciones ?? new List<object>();

            return View();
        }

        [HttpPost]
        public IActionResult SubirNotas(string seccion, List<int> alumnosId, List<string> notas, List<string> comentarios)
        {
            for (int i = 0; i < alumnosId.Count; i++)
            {
                var estudianteCurso = _context.Estudiantes_Cursos
                    .FirstOrDefault(ec => ec.EstudianteId == alumnosId[i]);
                if (estudianteCurso != null)
                {
                    // Obtener todas las calificaciones anteriores de este estudiante en este curso
                    var calificacionesAnteriores = _context.Calificaciones
                        .Where(c => c.estudiante_CursoId == estudianteCurso.IdEstudianteCurso)
                        .OrderBy(c => c.FechaCalificacion)
                        .ToList();

                    // Solo permitir máximo 4 registros (bimestres)
                    if (calificacionesAnteriores.Count >= 4)
                        continue;

                    // Convertir nota literal a numérica
                    int nota = 0;
                    switch ((notas[i] ?? "").Trim().ToUpper())
                    {
                        case "AD":
                            nota = 20;
                            break;
                        case "A":
                            nota = 16;
                            break;
                        case "B":
                            nota = 12;
                            break;
                        case "C":
                            nota = 8;
                            break;
                        default:
                            nota = 0;
                            break;
                    }

                    // Calcular el nuevo promedio acumulado (máximo 20)
                    var listaNotas = calificacionesAnteriores.Select(c => c.Puntaje).ToList();
                    listaNotas.Add(nota);
                    double promedio = listaNotas.Any() ? listaNotas.Average() : 0;
                    if (promedio > 20) promedio = 20;

                    var calificacion = new Calificacion
                    {
                        estudiante_CursoId = estudianteCurso.IdEstudianteCurso,
                        Puntaje = nota,
                        FechaCalificacion = DateTime.Now,
                        promedioAcumulado = (int)Math.Round(promedio),
                        Comentario = comentarios[i] ?? "Sin comentario"
                    };
                    _context.Calificaciones.Add(calificacion);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Dashboard", new { seccion });
        }
    }
}

