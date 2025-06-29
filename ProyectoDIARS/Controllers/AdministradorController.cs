using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using ProyectoDIARS.Data;
using ProyectoDIARS.shared;
using Microsoft.EntityFrameworkCore;
using ProyectoDIARS.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoDIARS.Controllers
{
    [Authorize(Roles = VCG.Role_Admin)]
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
            NewRegisterTypeUserVM dataResponse = new NewRegisterTypeUserVM
            {
                cursos = _context.Cursos.ToList(),
                tutores = _context.Tutores.Include(t => t.user).ToList()
            };
            return View(dataResponse);
        }

        // POST: /Administrador/Register
        [HttpPost]
        public async Task<IActionResult> Register(NewRegisterTypeUserVM userVM)
        {
            string tipo = userVM.tipo ?? string.Empty;
            Console.WriteLine("======================================== tipo ========================================");
            Console.WriteLine(tipo);


            var result = await _userManager.CreateAsync(userVM.User, userVM.User.Dni);
            if (!result.Succeeded)
            {
                Console.WriteLine("======================================== error ========================================");
                // si hay errores los imprimimos
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
                // Recargar los datos necesarios para la vista
                userVM.cursos = _context.Cursos.ToList();
                userVM.tutores = _context.Tutores.Include(t => t.user).ToList();
                return View(userVM); // Pasar el modelo de vuelta a la vista
            }

            if (tipo == TypesRegister.Tutor)
            {
                await _userManager.AddToRoleAsync(userVM.User, VCG.Role_Tutor);
                var tutor = new Tutor
                {
                    UserId = userVM.User.Id,
                    direccion = "" // Puedes agregar campo en el formulario si lo necesitas
                };
                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                // Registrar hijo/a si se ingresó
                if (!string.IsNullOrEmpty(userVM.estudiante.user.UserName))
                {
                    await _userManager.CreateAsync(userVM.estudiante.user, userVM.estudiante.user.Dni); // Contraseña temporal
                    var estudiante = new Estudiante
                    {
                        UserId = userVM.estudiante.user.Id,
                        TutorId = tutor.IdTutor,
                        Grado = userVM.estudiante.Grado
                    };
                    _context.Estudiantes.Add(estudiante);
                    await _context.SaveChangesAsync();
                }
            }
            else if (tipo == TypesRegister.Estudiante)
            {
                await _userManager.AddToRoleAsync(userVM.User, VCG.Role_Estudiante);
                if (userVM.estudiante.TutorId <= 0 && !string.IsNullOrEmpty(userVM.tutor.user.UserName))
                {
                    await _userManager.CreateAsync(userVM.tutor.user, userVM.tutor.user.Dni); // Contraseña temporal
                    Console.WriteLine("================================================================================");
                    Console.WriteLine($"user id: {userVM.tutor.user.Id}");
                    userVM.tutor.UserId = userVM.tutor.user.Id;
                    _context.Tutores.Add(userVM.tutor);
                    await _context.SaveChangesAsync();
                    userVM.estudiante.TutorId = userVM.tutor.IdTutor;
                }
                Console.WriteLine($"id final del tutor: {userVM.estudiante.TutorId}");
                userVM.estudiante.UserId = userVM.User.Id;

                _context.Estudiantes.Add(userVM.estudiante);
                await _context.SaveChangesAsync();
            }
            else if (tipo == TypesRegister.Docente)
            {
                await _userManager.AddToRoleAsync(userVM.User, VCG.Role_Docente);
                if (!string.IsNullOrEmpty(userVM.curso.Nombre))
                {
                    _context.Cursos.Add(userVM.curso);
                    await _context.SaveChangesAsync();
                }
                else if (userVM.curso.IdCurso > 0)
                {
                    userVM.curso = _context.Cursos.FirstOrDefault(c => c.IdCurso == userVM.curso.IdCurso);
                }
                var docente = new Docente
                {
                    UserId = userVM.User.Id,
                    Curso = userVM.curso
                };
                _context.Docentes.Add(docente);
                await _context.SaveChangesAsync();
            }
            else if (tipo == TypesRegister.Administrador)
            {
                await _userManager.AddToRoleAsync(userVM.User, VCG.Role_Admin);
                // Puedes agregar lógica adicional si tienes tabla Administrador
            }

            return RedirectToAction("Dashboard");
        }

        // GET: /Administrador/RegisterCurso
        [HttpGet]
        public IActionResult RegisterCurso()
        {
            NewCursoVM cursoVM = new NewCursoVM()
            {
                Curso = new Curso(),
                DocenteId = 0,
                Docentes = _context.Docentes.Include(u => u.user).ToList(),
            };
            return View(cursoVM);
        }

        // POST: /Administrador/RegisterCurso
        [HttpPost]
        public async Task<IActionResult> RegisterCurso(NewCursoVM cursoVM)
        {
            _context.Cursos.Add(cursoVM.Curso);
            await _context.SaveChangesAsync();

            // Asignar docente si corresponde
            if (cursoVM.DocenteId > 0)
            {
                int docenteId = cursoVM.DocenteId;
                var docente = _context.Docentes.FirstOrDefault(d => d.IdDocente == docenteId);
                if (docente != null)
                {
                    docente.Curso = cursoVM.Curso;
                    _context.Docentes.Update(docente);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Dashboard");
        }

        // ...existing code...
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.User = await _userManager.GetUserAsync(User);
            ViewBag.AlumnosCount = await _context.Estudiantes.CountAsync();
            ViewBag.DocentesCount = await _context.Docentes.CountAsync();
            ViewBag.CursosCount = await _context.Cursos.CountAsync();
            return View();
        }

    }
}
