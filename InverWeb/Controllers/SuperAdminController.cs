using InverWeb.DataAccess;
using InverWeb.DataAccess.Models;
using InverWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InverWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SuperAdminController : Controller
    {
        // Crear una instancia del Contexto de la base de datos
        private readonly GlobalDBContext _dbcontext;

        public SuperAdminController(GlobalDBContext dbcontext)
        {
            // Inicializar la base de datos
            _dbcontext = dbcontext;
        }

        [HttpGet("admin/superadmins")]
        public IActionResult Lista(string Filtro)
        {
            // Crear consulta de usuarios
            IQueryable<Usuario> consulta = _dbcontext.Usuarios
                                                .Where(u => u.Rol == Rol.Administrador)
                                                .AsQueryable();

            // Validar si se ha recibido información para filtrar
            if (!string.IsNullOrEmpty(Filtro))
            {
                ViewData["Filtro"] = Filtro;

                // Agregar % a la consulta para usar Like de base de datos
                Filtro = $"%{Filtro}%";

                // Modificar la consulta usando el filtro recibido
                consulta = consulta.Where(u => EF.Functions.Like(u.Nombres, Filtro) ||
                                            EF.Functions.Like(u.Nombres, Filtro) ||
                                            EF.Functions.Like(u.Correo, Filtro) ||
                                            EF.Functions.Like(u.Celular, Filtro) ||
                                            EF.Functions.Like(u.Cedula, Filtro));
            }

            // Devolver la vista con los administrador s filtrados
            return View(consulta.ToList());
        }

        [HttpGet("admin/superadmin")]
        public IActionResult Mostrar([FromQuery] int ID, string Correo)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este administrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            // Crear el modelo para la vista y agregar los datos del administrador 
            SuperAdminViewModel modelo = new()
            {
                Nombres = administrador.Nombres,
                Cedula = administrador.Cedula,
                Correo = administrador.Correo,
                Celular = administrador.Celular,
                Direccion = administrador.Direccion,
            };

            // Abrir la vista de información de administrador 
            return View(modelo);
        }

        [HttpGet("admin/superadmin/nuevo")]
        public IActionResult Nuevo()
        {
            // Abrir la vista de creación de administrador 
            return View();
        }

        [HttpPost("admin/superadmin/nuevo")]
        public IActionResult Nuevo(SuperAdminViewModel modelo)
        {
            // Validar si los campos del modelo se completaron correctamente
            if (ModelState.IsValid)
            {
                // Validar si ya existe un usuario con el correo recibido
                if (UsuarioExiste(modelo.Correo, modelo.Cedula))
                {
                    // Devolver la vista con error de administrador duplicado
                    TempData["Error"] = "Este administrador ya existe en la base de datos. \n Favor verificar correo y cédula";
                    return View(modelo);
                }

                //Generar Clave
                LogicaClaves.CrearClave(modelo.Clave, out byte[] ClaveHash, out byte[] ClaveSalt);

                // Pasar los datos del mdoelo a un nuevo objeto Usuario
                Usuario NuevoUsuario = new()
                {
                    Nombres = modelo.Nombres,
                    Cedula = modelo.Cedula,
                    Correo = modelo.Correo,
                    Celular = modelo.Celular,
                    Direccion = modelo.Direccion,
                    Rol = Rol.Administrador,
                    PasswordHash = ClaveHash,
                    PasswordSalt = ClaveSalt
                };

                _dbcontext.Usuarios.Add(NuevoUsuario);
                _dbcontext.SaveChanges();

                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Success"] = "Se ha creado el administrador satisfactoriamente";
                return RedirectToAction(nameof(Lista));
            }

            // Devolver la vista con los datos suministrados y un mensaje de error
            TempData["Error"] = "Error creando administrador , intente nuevamente.";
            return View(modelo);
        }

        [HttpGet("admin/superadmin/modificar")]
        public IActionResult Modificar([FromQuery] int ID, string Correo)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este administrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            // Crear el modelo para la vista y agregar los datos del administrador 
            SuperAdminViewModel modelo = new()
            {
                ID = administrador.ID,
                Nombres = administrador.Nombres,
                Cedula = administrador.Cedula,
                Correo = administrador.Correo,
                Celular = administrador.Celular,
                Direccion = administrador.Direccion
            };

            // Abrir la vista de creación de administrador 
            return View(modelo);
        }

        [HttpPost("admin/superadmin/modificar")]
        public IActionResult Modificar(SuperAdminViewModel modelo)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == modelo.ID &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "Error editanto el administrador";
                return View(modelo);
            }

            ModelState.ClearValidationState("Clave");
            ModelState.MarkFieldValid("Clave");

            ModelState.ClearValidationState("RepetirClave");
            ModelState.MarkFieldValid("RepetirClave");

            // Validar si los campos del modelo se completaron correctamente
            if (ModelState.IsValid)
            {
                // Validar si ya existe un usuario con el correo recibido
                if (UsuarioExiste(modelo.Correo, modelo.Cedula, modelo.ID))
                {
                    // Devolver la vista con error de administrador duplicado
                    TempData["Error"] = "Existe otro administrador con este correo o cédula";
                    return View(modelo);
                }

                // Agregar los nuevos datos del model al administrador 
                administrador.Nombres = modelo.Nombres;
                administrador.Cedula = modelo.Cedula;
                administrador.Correo = modelo.Correo;
                administrador.Celular = modelo.Celular;
                administrador.Direccion = modelo.Direccion;

                // Actualizar en la base de datos
                _dbcontext.Usuarios.Update(administrador);
                _dbcontext.SaveChanges();

                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Success"] = "Se ha modificado el administrador satisfactoriamente";
                return RedirectToAction(nameof(Lista));
            }

            // Devolver la vista con los datos suministrados y un mensaje de error
            TempData["Error"] = "Error modificando administrador, intente nuevamente.";
            return View(modelo);
        }

        [HttpGet("admin/superadmin/elimianar")]
        public IActionResult Eliminar([FromQuery] int ID)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este administrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            ViewData["ID"] = administrador.ID;
            ViewData["Nombres"] = administrador.Nombres;
            ViewData["Correo"] = administrador.Correo;
            ViewData["Cedula"] = administrador.Cedula;
            ViewData["Celular"] = administrador.Celular;

            // Abrir la vista de creación de administrador 
            return View();
        }

        [HttpPost("admin/superadmin/elimianar")]
        public IActionResult Eliminar([FromForm] int ID, string Correo)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de error
                TempData["Error"] = "No se ha encontrado este administrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            _dbcontext.Remove(administrador);
            _dbcontext.SaveChanges();

            // Devolver la lista de usuarios con un mensaje de validación
            TempData["Success"] = "Se ha eliminado el administrador satisfactoriamente.";
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet("admin/superadmin/cambiarclave")]
        public IActionResult CambiarClave([FromQuery] int ID, string Correo)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este administrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            ViewData["ID"] = administrador.ID;
            ViewData["Nombres"] = administrador.Nombres;
            ViewData["Correo"] = administrador.Correo;
            ViewData["Cedula"] = administrador.Cedula;
            ViewData["Celular"] = administrador.Celular;

            // Abrir la vista de creación de administrador 
            return View();
        }

        [HttpPost("admin/superadmin/cambiarclave")]
        public IActionResult CambiarClave(NuevaClaveViewModel modelo)
        {
            // Buscar el administrador con este ID y Correo
            Usuario administrador = _dbcontext.Usuarios
                                    .Where(u => u.ID == modelo.ID &&
                                            u.Correo.ToLower() == modelo.Correo.ToLower() &&
                                            u.Rol == Rol.Administrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún administrador devolver error
            if (administrador == null)
            {
                // Devolver la lista de usuarios con un mensaje de error
                TempData["Error"] = "No se ha encontrado este administrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            //Generar Clave
            LogicaClaves.CrearClave(modelo.Clave, out byte[] ClaveHash, out byte[] ClaveSalt);

            administrador.PasswordHash = ClaveHash;
            administrador.PasswordSalt = ClaveSalt;

            _dbcontext.Update(administrador);
            _dbcontext.SaveChanges();

            // Devolver la lista de usuarios con un mensaje de validación
            TempData["Success"] = "Se ha realizado el cambio de clave satisfactoriamente.";
            return RedirectToAction(nameof(Lista));
        }

        private bool UsuarioExiste(string Correo, string Cedula, int ID = 0)
        {
            // Crear consulta de usuario con cedula y correo
            IQueryable<Usuario> consulta = _dbcontext.Usuarios
                                    .Where(u => u.Correo.ToLower() == Correo.ToLower() ||
                                                u.Cedula == Cedula)
                                    .AsNoTracking();

            // Validación para usuarios al momento de modificar
            // Validar si hay otro usuario creado con este mismo ID
            if (ID != 0)
            {
                consulta = consulta.Where(a => a.ID != ID);
            }

            // Devolver true si el usuario existe
            // Devolver false si el usuario no existe
            return consulta.Any();
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
