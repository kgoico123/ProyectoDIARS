using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.Data;
using ProyectoDIARS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProyectoDIARS.shared;

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

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var docente = await _context.Docentes
                .Include(d => d.Curso)
                    .ThenInclude(c => c.estudiante_Curso)
                        .ThenInclude(ec => ec.Estudiante)
                .FirstOrDefaultAsync(d => d.user.UserName == user.UserName);

            ViewBag.Docente = docente;
            ViewBag.Curso = docente?.Curso;
            ViewBag.CantidadAlumnos = docente?.Curso?.estudiante_Curso?.Count() ?? 0;

            return View();
        }

        public async Task<IActionResult> Calificaciones(int? horarioId = null, string? seccion = null)
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
        public async Task<IActionResult> SubirNotas(string seccion, List<int> alumnosId, List<string> notas, List<string> comentarios)
        {
            var user = await _userManager.GetUserAsync(User);
            var docente = await _context.Docentes
                .Include(d => d.Curso)
                    .ThenInclude(c => c.estudiante_Curso)
                        .ThenInclude(ec => ec.Estudiante)
                            .ThenInclude(e => e.user)
                .FirstOrDefaultAsync(d => d.user.UserName == user.UserName);
            for (int i = 0; i < alumnosId.Count; i++)
            {
                var estudianteCurso = _context.Estudiantes_Cursos
                    .Where(ec => ec.EstudianteId == alumnosId[i] && ec.CursoId == docente.Curso.IdCurso)
                    .ToList();

                foreach (var ec in estudianteCurso)
                {
                    Console.WriteLine($"Estudiante: {ec.EstudianteId}, Curso: {ec.CursoId}, Id: {ec.IdEstudianteCurso}");
                    if (estudianteCurso != null)
                    {
                        // Obtener todas las calificaciones anteriores de este estudiante en este curso
                        var calificacionesAnteriores = _context.Calificaciones
                            .Where(c => c.estudiante_CursoId == ec.IdEstudianteCurso)
                            .OrderBy(c => c.FechaCalificacion)
                            .ToList();

                        Console.WriteLine($"Calificaciones : {calificacionesAnteriores.Count}");

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
                            estudiante_CursoId = ec.IdEstudianteCurso,
                            Puntaje = nota,
                            FechaCalificacion = DateTime.Now,
                            promedioAcumulado = (int)Math.Round(promedio),
                            Comentario = comentarios[i] ?? "Sin comentario"
                        };
                        _context.Calificaciones.Add(calificacion);
                    }
                }

            }
            _context.SaveChanges();
            return RedirectToAction("Dashboard", new { seccion });
        }

        public async Task<IActionResult> Conducta()
        {
            var user = await _userManager.GetUserAsync(User);
            var docente = await _context.Docentes
                .Include(d => d.Curso)
                    .ThenInclude(c => c.estudiante_Curso)
                        .ThenInclude(ec => ec.Estudiante)
                        .ThenInclude(u => u.user)
                .FirstOrDefaultAsync(d => d.user.UserName == user.UserName);

            ViewBag.Docente = docente;
            ViewBag.Curso = docente?.Curso;
            ViewBag.Estudiantes = docente?.Curso?.estudiante_Curso.Select(ec => ec.Estudiante).ToList() ?? new List<Estudiante>();

            return View();
        }

        [HttpPost]
        public IActionResult RegistrarConductas(List<int> alumnosId, List<string> conductas, List<string> comentarios)
        {
            for (int i = 0; i < alumnosId.Count; i++)
            {
                var estudianteCurso = _context.Estudiantes_Cursos.FirstOrDefault(ec => ec.EstudianteId == alumnosId[i]);
                if (estudianteCurso != null && !string.IsNullOrWhiteSpace(conductas[i]))
                {
                    var comportamiento = new Comportamiento
                    {
                        estudiante_CursoId = estudianteCurso.IdEstudianteCurso,
                        FechaRegistro = DateTime.Now,
                        Calificacion = conductas[i],
                        Descripcion = comentarios[i] ?? ""
                    };
                    _context.Comportamientos.Add(comportamiento);

                    // Si la conducta es "C" o "B", crear notificación al tutor
                    if (conductas[i].Trim().ToUpper() == "C" || conductas[i].Trim().ToUpper() == "B")
                    {
                        // Obtener el estudiante y su tutor
                        var estudiante = _context.Estudiantes.Include(e => e.user).FirstOrDefault(e => e.IdEstudiante == alumnosId[i]);
                        if (estudiante != null)
                        {
                            var notificacion = new Notificacion
                            {
                                TutorId = estudiante.TutorId,
                                fecha = DateTime.Now,
                                Titulo = "Alerta de Conducta",
                                Mensaje = $"Se ha registrado una conducta '{conductas[i]}' para el estudiante {estudiante.user?.UserName ?? "Desconocido"}. {comentarios[i]}",
                                Leida = false,
                                Tipo = VCG.TipoNotificacion.advertencia
                            };
                            _context.Notificaciones.Add(notificacion);
                        }
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Conducta");
        }
    }
}

