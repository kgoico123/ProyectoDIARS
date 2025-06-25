using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using ProyectoDIARS.Data;
using ProyectoDIARS.shared;

namespace ProyectoDIARS.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDBContext _context;

        public AdministradorController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: /Administrador/Register
        public IActionResult Register()
        {
            ViewBag.Cursos = _context.Cursos.ToList();
            return View();
        }

        // POST: /Administrador/Register
        [HttpPost]
        public async Task<IActionResult> RegisterPost()
        {
            var form = Request.Form;
            string tipo = form["userType"].ToString() ?? string.Empty;

            // Crear usuario base
            var user = new ApplicationUser
            {
                UserName = form["Nombre"],
                Email = form["Email"],
                Dni = form["Dni"],
                Apellido = form["Apellido"],
                PhoneNumber = form["PhoneNumber"]
            };
            var result = await _userManager.CreateAsync(user, form["Password"]);
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
                ViewBag.Cursos = _context.Cursos.ToList();
                return View();
            }

            if (tipo == "Tutor")
            {
                await _userManager.AddToRoleAsync(user, VCG.Role_Tutor);
                var tutor = new Tutor
                {
                    UserId = user.Id,
                    direccion = "" // Puedes agregar campo en el formulario si lo necesitas
                };
                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                // Registrar hijo/a si se ingresó
                if (!string.IsNullOrEmpty(form["HijoNombre"]))
                {
                    var estudianteUser = new ApplicationUser
                    {
                        UserName = form["HijoNombre"],
                        Apellido = form["HijoApellido"],
                        Dni = form["HijoDni"]
                    };
                    await _userManager.CreateAsync(estudianteUser, "123456"); // Contraseña temporal
                    var estudiante = new Estudiante
                    {
                        UserId = estudianteUser.Id,
                        TutorId = tutor.IdTutor,
                        Grado = form["HijoGrado"]
                    };
                    _context.Estudiantes.Add(estudiante);
                    await _context.SaveChangesAsync();

                    // Registrar estudiante en un curso (si lo seleccionas en el formulario)
                    if (!string.IsNullOrEmpty(form["CursoId"]))
                    {
                        int cursoId = int.Parse(form["CursoId"]);
                        var estudianteCurso = new Estudiante_Curso
                        {
                            EstudianteId = estudiante.IdEstudiante,
                            CursoId = cursoId,
                            FechaRegistro = DateTime.Now
                        };
                        _context.Estudiantes_Cursos.Add(estudianteCurso);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else if (tipo == "Estudiante")
            {
                await _userManager.AddToRoleAsync(user, VCG.Role_Estudiante);
                int tutorId = 0;

                if (form["tutorOption"] == "existente")
                {
                    // Buscar tutor por ID (mejor que por nombre)
                    if (int.TryParse(form["TutorId"], out int id))
                    {
                        tutorId = id;
                    }
                }
                else
                {
                    // Registrar nuevo tutor
                    var tutorUser = new ApplicationUser
                    {
                        UserName = form["NuevoTutorNombre"],
                        Apellido = form["NuevoTutorApellido"],
                        Dni = form["NuevoTutorDni"],
                        PhoneNumber = form["NuevoTutorTelefono"]
                    };
                    await _userManager.CreateAsync(tutorUser, "123456"); // Contraseña temporal
                    var tutor = new Tutor
                    {
                        UserId = tutorUser.Id,
                        direccion = ""
                    };
                    _context.Tutores.Add(tutor);
                    await _context.SaveChangesAsync();
                    tutorId = tutor.IdTutor;
                }

                // Registrar estudiante
                var estudiante = new Estudiante
                {
                    UserId = user.Id,
                    TutorId = tutorId,
                    Grado = form["Grado"]
                };
                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync();

                // Registrar estudiante en un curso (si lo seleccionas en el formulario)
                if (!string.IsNullOrEmpty(form["CursoId"]))
                {
                    int cursoId = int.Parse(form["CursoId"]);
                    var estudianteCurso = new Estudiante_Curso
                    {
                        EstudianteId = estudiante.IdEstudiante,
                        CursoId = cursoId,
                        FechaRegistro = DateTime.Now
                    };
                    _context.Estudiantes_Cursos.Add(estudianteCurso);
                    await _context.SaveChangesAsync();
                }
            }
            else if (tipo == "Docente")
            {
                await _userManager.AddToRoleAsync(user, VCG.Role_Docente);
                Curso? curso = null;
                if (!string.IsNullOrEmpty(form["NuevoCursoNombre"]))
                {
                    curso = new Curso
                    {
                        Nombre = form["NuevoCursoNombre"],
                        aula = form["NuevoCursoAula"],
                        horario = DateTime.Parse(form["NuevoCursoHorario"])
                    };
                    _context.Cursos.Add(curso);
                    await _context.SaveChangesAsync();
                }
                else if (!string.IsNullOrEmpty(form["CursoId"]))
                {
                    int cursoId = int.Parse(form["CursoId"]);
                    curso = _context.Cursos.FirstOrDefault(c => c.IdCurso == cursoId);
                }
                var docente = new Docente
                {
                    UserId = user.Id,
                    Curso = curso
                };
                _context.Docentes.Add(docente);
                await _context.SaveChangesAsync();
            }
            else if (tipo == "Administrador")
            {
                await _userManager.AddToRoleAsync(user, "Administrador");
                // Puedes agregar lógica adicional si tienes tabla Administrador
            }

            return RedirectToAction("Dashboard");
        }

        // GET: /Administrador/RegisterCurso
        [HttpGet]
        public IActionResult RegisterCurso()
        {
            ViewBag.Docentes = _context.Docentes.ToList();
            return View();
        }

        // POST: /Administrador/RegisterCurso
        [HttpPost]
        public async Task<IActionResult> RegisterCursoPost()
        {
            var form = Request.Form;
            var curso = new Curso
            {
                Nombre = form["Nombre"],
                aula = form["Aula"],
                horario = DateTime.Parse(form["Horario"])
            };
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            // Asignar docente si corresponde
            if (!string.IsNullOrEmpty(form["DocenteId"]))
            {
                int docenteId = int.Parse(form["DocenteId"]);
                var docente = _context.Docentes.FirstOrDefault(d => d.IdDocente == docenteId);
                if (docente != null)
                {
                    docente.Curso = curso;
                    _context.Docentes.Update(docente);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
