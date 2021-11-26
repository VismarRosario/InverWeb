using InverWeb.DataAccess;
using InverWeb.DataAccess.Models;
using InverWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InverWeb.Controllers
{
    public class ClienteController : Controller
    {
        // Crear una instancia del Contexto de la base de datos
        private readonly GlobalDBContext _dbcontext;

        public ClienteController(GlobalDBContext dbcontext)
        {
            // Inicializar la base de datos
            _dbcontext = dbcontext;
        }

        [HttpGet("admin/clientes")]
        public IActionResult Lista(string Filtro)
        {
            // Crear consulta de usuarios de tipo cliente
            IQueryable<Usuario> consulta = _dbcontext.Usuarios
                                                .Where(u => u.Rol == Rol.Cliente)
                                                .AsQueryable();

            // Validar si se ha recibido algun filtro

            if (!string.IsNullOrEmpty(Filtro))
            {
                ViewData["Filtro"] = Filtro;

                // Agregar % al criterio de la busqueda para usar Like en base de datos
                Filtro = $"%{Filtro}%"; // Filtro = "%" + Filtro + "%";

                // Modificar la consulta con el filtro recibido
                consulta = consulta.Where(usuario =>
                                    EF.Functions.Like(usuario.Nombres, Filtro) ||
                                    EF.Functions.Like(usuario.Correo, Filtro) ||
                                    EF.Functions.Like(usuario.Celular, Filtro) ||
                                    EF.Functions.Like(usuario.Cedula, Filtro));
            }

            // Devolver la vista con los clientes filtrados
            return View(consulta.ToList());
        }

        [HttpGet("admin/cliente")]
        public IActionResult Mostrar([FromQuery] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cliente)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cliente en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            // Crear el modelo para la vista y agregar los datos del cliente
            ClienteViewModel modelo = new()
            {
                Nombres = cliente.Nombres,
                Cedula = cliente.Cedula,
                Correo = cliente.Correo,
                Celular = cliente.Celular,
                Direccion = cliente.Direccion,
                EmpresaNombre = cliente.EmpresaNombre,
                EmpresaCargo = cliente.EmpresaCargo,
                EmpresaSalario = cliente.EmpresaSalario,
                EmpresaDireccion = cliente.EmpresaDireccion,
                EmpresaTelefono = cliente.EmpresaTelefono,
                EmpresaFechaIngreso = cliente.EmpresaFechaIngreso,
            };

            // Abrir la vista de información de cliente
            return View(modelo);
        }

        [HttpGet("admin/cliente/nuevo")]
        public IActionResult Nuevo()
        {
            // Abrir la vista de creación de cliente
            return View();
        }

        [HttpPost("admin/cliente/nuevo")]
        public IActionResult Nuevo(ClienteViewModel modelo)
        {
            // Validar si los campos del modelo se completaron correctamente
            if (ModelState.IsValid)
            {
                // Validar si ya existe un usuario con el correo recibido
                if (UsuarioExiste(modelo.Correo, modelo.Cedula))
                {
                    // Devolver la vista con error de cliente duplicado
                    TempData["Error"] = "Este Cliente ya existe en la base de datos. \n Favor verificar correo y cédula";
                    return View(modelo);
                }

                //Generar Clave ficticia para completar los campos requeridos de clave
                LogicaClaves.CrearClaveTemporal(out byte[] ClaveHash, out byte[] ClaveSalt);

                // Pasar los datos del mdoelo a un nuevo objeto Usuario
                Usuario NuevoUsuario = new()
                {
                    Nombres = modelo.Nombres,
                    Cedula = modelo.Cedula,
                    Correo = modelo.Correo,
                    Celular = modelo.Celular,
                    Direccion = modelo.Direccion,
                    Rol = Rol.Cliente,
                    EmpresaNombre = modelo.EmpresaNombre,
                    EmpresaCargo = modelo.EmpresaCargo,
                    EmpresaSalario = modelo.EmpresaSalario,
                    EmpresaDireccion = modelo.EmpresaDireccion,
                    EmpresaTelefono = modelo.EmpresaTelefono,
                    EmpresaFechaIngreso = modelo.EmpresaFechaIngreso,
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

        [HttpGet("admin/cliente/modificar")]
        public IActionResult Modificar([FromQuery] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cliente)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cliente en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            // Crear el modelo para la vista y agregar los datos del cliente
            ClienteViewModel modelo = new()
            {
                ID = cliente.ID,
                Nombres = cliente.Nombres,
                Cedula = cliente.Cedula,
                Correo = cliente.Correo,
                Celular = cliente.Celular,
                Direccion = cliente.Direccion,
                EmpresaNombre = cliente.EmpresaNombre,
                EmpresaCargo = cliente.EmpresaCargo,
                EmpresaSalario = cliente.EmpresaSalario,
                EmpresaDireccion = cliente.EmpresaDireccion,
                EmpresaTelefono = cliente.EmpresaTelefono,
                EmpresaFechaIngreso = cliente.EmpresaFechaIngreso,
            };

            // Abrir la vista de creación de cliente
            return View(modelo);
        }

        [HttpPost("admin/cliente/modificar")]
        public IActionResult Modificar(ClienteViewModel modelo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == modelo.ID &&
                                            u.Rol == Rol.Cliente)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "Error editanto el cliente";
                return View(modelo);
            }

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
                cliente.EmpresaNombre = modelo.EmpresaNombre;
                cliente.EmpresaCargo = modelo.EmpresaCargo;
                cliente.EmpresaSalario = modelo.EmpresaSalario;
                cliente.EmpresaDireccion = modelo.EmpresaDireccion;
                cliente.EmpresaTelefono = modelo.EmpresaTelefono;
                cliente.EmpresaFechaIngreso = modelo.EmpresaFechaIngreso;

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

        [HttpGet("admin/cliente/elimianar")]
        public IActionResult Eliminar([FromQuery] int ID)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Rol == Rol.Cliente)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de validación
                TempData["Error"] = "No se ha encontrado este cliente en el sistema";
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

        [HttpPost("admin/cliente/elimianar")]
        public IActionResult Eliminar([FromForm] int ID, string Correo)
        {
            // Buscar el cliente con este ID y Correo
            Usuario cliente = _dbcontext.Usuarios
                                    .Where(u => u.ID == ID &&
                                            u.Correo.ToLower() == Correo.ToLower() &&
                                            u.Rol == Rol.Cliente)
                                    .FirstOrDefault();

            // Si no se encuentra ningún cliente devolver error
            if (cliente == null)
            {
                // Devolver la lista de usuarios con un mensaje de error
                TempData["Error"] = "No se ha encontrado este cliente en el sistema";
                return RedirectToAction(nameof(Lista));
            }

            _dbcontext.Remove(cliente);
            _dbcontext.SaveChanges();

            // Devolver la lista de usuarios con un mensaje de validación
            TempData["Success"] = "Se ha eliminado el cliente satisfactoriamente.";
            return RedirectToAction(nameof(Lista));
        }

        private bool UsuarioExiste(string Correo, string Cedula, int ID = 0)
        {
            // Crear consulta de usuario con cedula y correo
            IQueryable<Usuario> consulta = _dbcontext.Usuarios
                                    .Where(u => u.Correo.ToLower() == Correo.ToLower() ||
                                                u.Cedula == Cedula)
                                    .AsQueryable();

            // Validación para usuarios al momento de modificar
            // Validar si hay otro usuario creado con este mismo ID
            if (ID != 0)
            {
                consulta = consulta.Where(u => u.ID != ID);
            }

            // Devolver true si el usuario existe
            // Devolver false si el usuario existe
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
