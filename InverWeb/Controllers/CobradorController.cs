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
    public class CobradorController : Controller
    {
        // Crear una instancia del Contexto de la base de datos
        private readonly GlobalDBContext _dbcontext;

        public CobradorController(GlobalDBContext dbcontext)
        {
            // Inicializar la base de datos
            _dbcontext = dbcontext;
        }

        [HttpGet("admin/cobradores")]
        public IActionResult Lista(string Filtro)
        {
            // Crear consulta de usuarios
            IQueryable<Usuario> consulta = _dbcontext.Usuarios
                                                .Where(u => u.Rol == Rol.Cobrador)
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

            // Devolver la vista con los clientes filtrados
            return View(consulta.ToList());
        }

        [HttpGet("admin/cobrador")]
        public IActionResult Mostrar([FromQuery] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cobrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            // Crear el modelo para la vista y agregar los datos del cliente
            CobradorViewModel modelo = new()
            {
                Nombres = cliente.Nombres,
                Cedula = cliente.Cedula,
                Correo = cliente.Correo,
                Celular = cliente.Celular,
                Direccion = cliente.Direccion,
            };

            // Abrir la vista de información de cliente
            return View(modelo);
        }

        [HttpGet("admin/cobrador/nuevo")]
        public IActionResult Nuevo()
        {
            // Abrir la vista de creación de cliente
            return View();
        }

        [HttpPost("admin/cobrador/nuevo")]
        public IActionResult Nuevo(CobradorViewModel modelo)
        {
            // Validar si los campos del modelo se completaron correctamente
            if (ModelState.IsValid)
            {
                // Validar si ya existe un usuario con el correo recibido
                if (UsuarioExiste(modelo.Correo, modelo.Cedula))
                {
                    // Devolver la vista con error de cliente duplicado
                    TempData["Error"] = "este cobrador ya existe en la base de datos. \n Favor verificar correo y cédula";
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
                    Rol = Rol.Cobrador,
                    PasswordHash = ClaveHash,
                    PasswordSalt = ClaveSalt
                };

                _dbcontext.Usuarios.Add(NuevoUsuario);
                _dbcontext.SaveChanges();

                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Success"] = "Se ha creado el cliente satisfactoriamente";
                return RedirectToAction(nameof(Lista));
            }

            // Devolver la vista con los datos suministrados y un mensaje de error
            TempData["Error"] = "Error creando cliente, intente nuevamente.";
            return View(modelo);
        }

        [HttpGet("admin/cobrador/modificar")]
        public IActionResult Modificar([FromQuery] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cobrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            // Crear el modelo para la vista y agregar los datos del cliente
            CobradorViewModel modelo = new()
            {
                ID = cliente.ID,
                Nombres = cliente.Nombres,
                Cedula = cliente.Cedula,
                Correo = cliente.Correo,
                Celular = cliente.Celular,
                Direccion = cliente.Direccion
            };

            // Abrir la vista de creación de cliente
            return View(modelo);
        }

        [HttpPost("admin/cobrador/modificar")]
        public IActionResult Modificar(CobradorViewModel modelo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == modelo.ID &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "Error editanto el cliente";
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
                    // Devolver la vista con error de cliente duplicado
                    TempData["Error"] = "Existe otro cliente con este correo o cédula";
                    return View(modelo);
                }

                // Agregar los nuevos datos del model al cliente
                cliente.Nombres = modelo.Nombres;
                cliente.Cedula = modelo.Cedula;
                cliente.Correo = modelo.Correo;
                cliente.Celular = modelo.Celular;
                cliente.Direccion = modelo.Direccion;

                // Actualizar en la base de datos
                _dbcontext.Usuarios.Update(cliente);
                _dbcontext.SaveChanges();

                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Success"] = "Se ha modificado el usuario satisfactoriamente";
                return RedirectToAction(nameof(Lista));
            }

            // Devolver la vista con los datos suministrados y un mensaje de error
            TempData["Error"] = "Error modificando cliente, intente nuevamente.";
            return View(modelo);
        }

        [HttpGet("admin/cobrador/elimianar")]
        public IActionResult Eliminar([FromQuery] int ID)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cobrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            ViewData["ID"] = cliente.ID;
            ViewData["Nombres"] = cliente.Nombres;
            ViewData["Correo"] = cliente.Correo;
            ViewData["Cedula"] = cliente.Cedula;
            ViewData["Celular"] = cliente.Celular;

            // Abrir la vista de creación de cliente
            return View();
        }

        [HttpPost("admin/cobrador/elimianar")]
        public IActionResult Eliminar([FromForm] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de error
                TempData["Error"] = "No se ha encontrado este cobrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            _dbcontext.Remove(cliente);
            _dbcontext.SaveChanges();

            // Devolver la lista de usuarios con un mensaje de validación
            TempData["Success"] = "Se ha eliminado el cliente satisfactoriamente.";
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet("admin/cobrador/cambiarclave")]
        public IActionResult CambiarClave([FromQuery] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cobrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            ViewData["ID"] = cliente.ID;
            ViewData["Nombres"] = cliente.Nombres;
            ViewData["Correo"] = cliente.Correo;
            ViewData["Cedula"] = cliente.Cedula;
            ViewData["Celular"] = cliente.Celular;

            // Abrir la vista de creación de cliente
            return View();
        }

        [HttpPost("admin/cobrador/cambiarclave")]
        public IActionResult CambiarClave(NuevaClaveViewModel modelo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == modelo.ID &&
                                            u.Correo.ToLower() == modelo.Correo.ToLower() &&
                                            u.Rol == Rol.Cobrador)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de error
                TempData["Error"] = "No se ha encontrado este cobrador en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            //Generar Clave
            LogicaClaves.CrearClave(modelo.Clave, out byte[] ClaveHash, out byte[] ClaveSalt);

            cliente.PasswordHash = ClaveHash;
            cliente.PasswordSalt = ClaveSalt;

            _dbcontext.Update(cliente);
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
