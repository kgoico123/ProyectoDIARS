using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDIARS.Data;
using ProyectoDIARS.Models;
using Microsoft.AspNetCore.Identity;

namespace ProyectoDIARS.Controllers
{
    public class TutorController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TutorController(AppDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Dashboard: muestra los estudiantes del tutor
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var tutor = await _context.Tutores
                .Include(t => t.Estudiantes)
                .ThenInclude(e => e.user)
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            var estudiantes = tutor?.Estudiantes?.ToList() ?? new List<Estudiante>();
            ViewBag.Estudiantes = estudiantes;
            return View();
        }

        // Notificaciones: muestra las notificaciones del estudiante seleccionado
        public async Task<IActionResult> Notificaciones(int estudianteId)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.user)
                .Include(e => e.Estudiante_Cursos)
                .FirstOrDefaultAsync(e => e.IdEstudiante == estudianteId);

            var notificaciones = await _context.Notificaciones
                .Where(n => n.TutorId == estudiante.TutorId)
                .OrderByDescending(n => n.fecha)
                .ToListAsync();

            ViewBag.Estudiante = estudiante;
            ViewBag.Notificaciones = notificaciones;
            return View();
        }

        // Conducta: muestra las conductas del estudiante seleccionado
        public async Task<IActionResult> Comportamiento(int estudianteId)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.user)
                .Include(e => e.Estudiante_Cursos)
                .FirstOrDefaultAsync(e => e.IdEstudiante == estudianteId);

            // Obtener los ids de Estudiante_Curso de este estudiante
            var estudianteCursoIds = estudiante.Estudiante_Cursos.Select(ec => ec.IdEstudianteCurso).ToList();

            var conductas = await _context.Comportamientos
                .Where(c => estudianteCursoIds.Contains(c.estudiante_CursoId))
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();

            ViewBag.Estudiante = estudiante;
            ViewBag.Conductas = conductas;
            return View();
        }

        // Calificaciones: muestra las calificaciones del estudiante seleccionado
        public async Task<IActionResult> Calificaciones(int estudianteId)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.user)
                .Include(e => e.Estudiante_Cursos)
                    .ThenInclude(ec => ec.Curso)
                .FirstOrDefaultAsync(e => e.IdEstudiante == estudianteId);

            var estudianteCursoIds = estudiante.Estudiante_Cursos.Select(ec => ec.IdEstudianteCurso).ToList();

            var calificaciones = await _context.Calificaciones
                .Include(c => c.Estudiante_Curso)
                .Include(c => c.Estudiante_Curso.Curso)
                .Where(c => estudianteCursoIds.Contains(c.estudiante_CursoId))
                .OrderByDescending(c => c.FechaCalificacion)
                .ToListAsync();

            // Obtener el último promedio acumulado por curso
            var promediosPorCurso = calificaciones
                .GroupBy(c => c.Estudiante_Curso?.Curso?.Nombre)
                .Where(g => g.Key != null)
                .Select(g =>
                {
                    var ultima = g.OrderByDescending(x => x.FechaCalificacion).FirstOrDefault();
                    return new
                    {
                        Curso = g.Key,
                        Promedio = ultima != null ? ultima.promedioAcumulado : 0
                    };
                })
                .ToList();

            ViewBag.PromediosPorCurso = promediosPorCurso;
            ViewBag.Estudiante = estudiante;
            ViewBag.Calificaciones = calificaciones;
            return View();
        }
    }
}
