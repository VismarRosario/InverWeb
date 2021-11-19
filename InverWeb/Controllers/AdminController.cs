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
    public class AdminController : Controller
    {
        // Crear una instancia del Contexto de la base de datos
        private readonly GlobalDBContext _dbcontext;

        public AdminController(GlobalDBContext dbcontext)
        {
            // Inicializar la base de datos
            _dbcontext = dbcontext;
        }

        [HttpGet("admin/index")]
        public IActionResult Index()
        {
            return View();
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
