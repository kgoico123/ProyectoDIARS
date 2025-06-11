using Microsoft.AspNetCore.Mvc;
using ProyectoDIARS.Data;


namespace ProyectoDIARS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _context;
        public AccountController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(string userType, string dni, string password)
        //{
        //    if (userType == "Padre")
        //    {
        //        // Buscar tutor solo por DNI
        //        var tutor = _context.Tutores.FirstOrDefault(t => t.Dni != null && t.Dni == dni);
        //        if (tutor != null)
        //        {
        //            // Lógica de autenticación de padre solo con DNI
        //            // Aquí podrías usar sesiones, claims, etc.
        //            return RedirectToAction("Dashboard", "Tutor", new { id = tutor.IdTutor });
        //        }
        //        ViewBag.Error = "DNI incorrecto.";
        //    }
        //    else if (userType == "Profesor")
        //    {
        //        // Buscar docente por DNI y contraseña
        //        var docente = _context.Docentes.FirstOrDefault(d => d.Dni == dni && d.Password == password);
        //        if (docente != null)
        //        {
        //            // Lógica de autenticación de docente
        //            return RedirectToAction("Dashboard", "Docente", new { id = docente.IdDocente });
        //        }
        //        ViewBag.Error = "DNI o contraseña incorrectos.";
        //    }
        //    return View();
        //}
    
    }
}
