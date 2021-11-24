using InverWeb.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace InverWeb.Controllers
{
    public class PrestamoController : Controller
    {
        // Crear una instancia del Contexto de la base de datos
        private readonly GlobalDBContext _dbcontext;

        public PrestamoController(GlobalDBContext dbcontext)
        {
            // Inicializar la base de datos
            _dbcontext = dbcontext;
        }

        [HttpGet("admin/prestamos")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/prestamo")]
        public IActionResult Mostrar()
        {
            return View();
        }
    }
}
