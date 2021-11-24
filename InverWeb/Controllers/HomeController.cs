using InverWeb.DataAccess;
using InverWeb.DataAccess.Models;
using InverWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace InverWeb.Controllers
{
    public class HomeController : Controller
    {
        // Crear una instancia del Contexto de la base de datos
        private readonly GlobalDBContext _dbcontext;

        public HomeController(GlobalDBContext dbcontext)
        {
            // Inicializar la base de datos
            _dbcontext = dbcontext;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Account/Login")]
        public IActionResult Login(LoginViewModel modelo)
        {
            //Validar si los datos del modelo cumplen
            if (ModelState.IsValid)
            {
                Usuario login = _dbcontext.Usuarios
                                    .Where(usuario => usuario.Correo.ToLower() == modelo.Correo.ToLower())
                                    .FirstOrDefault();

                // Validar si se ha encontrado un usuario con el correo
                if (login == null)
                {
                    // Devolver error si no cumplen los datos del modelo
                    TempData["Error"] = "Error de usuario y/o contraseña";
                    return View("Index", modelo);
                }

                //Validar si la clave que se puso en el modelo desencripta los Hash de base de datos
                if (LogicaClaves.ValidarClave(modelo.Clave, login.PasswordHash, login.PasswordSalt))
                {
                    // Crear configuración de la Cookie de Autenticación
                    ClaimsIdentity identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, login.Correo),
                        new Claim(ClaimTypes.Role, login.Rol.ToString())
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Crear Cookie principal
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    // Autenticar el usuario
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Crear mensaje de bienvenida y redireccionar al panel de administración
                    TempData["Success"] = $"Bienvenid@ al Sistema, \n {login.Nombres}";
                    return RedirectToAction("Index", "Admin");
                }
            }

            // Devolver error si no cumplen los datos del modelo
            TempData["Error"] = "Error de usuario y/o contraseña";
            return View("Index", modelo);
        }

        [HttpGet("Account/Logout")]
        public IActionResult Logout()
        {
            // Validar si el usuario está autenticado
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // remover la cookie de autenticación || remover sesión
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbcontext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
