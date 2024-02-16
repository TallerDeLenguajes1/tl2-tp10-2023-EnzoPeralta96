using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;


namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class UsuarioController : HelperController
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;
    private readonly ITareaRepository _tareaRepository;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository userRepository, ITareaRepository tareaRepository, ITableroRepository tableroRepository) :
    base(logger, userRepository)
    {
        _logger = logger;
        _usuarioRepository = userRepository;
        _tareaRepository = tareaRepository;
        _tableroRepository = tableroRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            UsuariosViewModels model = null;

            if (IsAdmin())
            {
                var user = GetUserLogged();
                var users = _usuarioRepository.GetRestUsers(user.Id);
                model = new UsuariosViewModels(user, users);

            }
            else
            {
                var user = GetUserLogged();
                model = new UsuariosViewModels(user);
            }

            model.MensajeExito = TempData["MensajeExito"] as string;
            model.MensajeError = TempData["MensajeError"] as string;

            return View(model);

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }


    public IActionResult AllUsers()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });

            UsuariosViewModels model = null;

            var user = GetUserLogged();
            var users = _usuarioRepository.GetRestUsers(user.Id);

            model = new UsuariosViewModels(user, users)
            {
                MensajeExito = TempData["MensajeExito"] as string,
                MensajeError = TempData["MensajeError"] as string
            };

            return View(model);

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!IsAdmin()) return RedirectToAction("Index");
            return View(new CreateUserViewModels());
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }


    [HttpPost]
    public IActionResult Create(CreateUserViewModels user)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin()) return RedirectToAction("Index");

            if (_usuarioRepository.NameInUse(user.Name))
            {
                var userMensajeError = new CreateUserViewModels
                {
                    MensajeDeError = "Nombre de usuario en uso, elija otro nombre por favor."
                };
                return View("Create", userMensajeError);
            }

            if (!ModelState.IsValid) return RedirectToAction("Create");

            _usuarioRepository.Create(new Usuario(user));

            TempData["MensajeExito"] = "El usuario " + "<b>" + user.Name + "</b>" + "  fue creado con éxito!";

            return RedirectToAction("AllUsers");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }


    [HttpGet]
    public IActionResult Update(int idUsuario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index");

            var user = _usuarioRepository.GetUsuarioById(idUsuario);

            return View(new UpdateUserViewModels(user));

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    [HttpPost]
    public IActionResult Update(UpdateUserViewModels user)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (_usuarioRepository.NameInUseUpdate(user.Name, user.Id))
            {
                var userVmMensaje = new UpdateUserViewModels
                {
                    MensajeDeError = "Nombre de usuario en uso, elija otro nombre por favor."
                };
                return View("Update", userVmMensaje);
            }

            if (!ModelState.IsValid) return RedirectToAction("Index");

            var usuario = new Usuario(user);

            _usuarioRepository.Update(usuario.Id, usuario);

            TempData["MensajeExito"] = " El usuario " + "<b>" + user.Name + "</b>" + " fue modificado con éxito!";

            return IsOwner(usuario.Id) ? RedirectToAction("Index") : RedirectToAction("AllUsers");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult Delete(int idUsuario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin()) return RedirectToAction("Index");

            string userName = _usuarioRepository.GetUsuarioById(idUsuario).Nombre_de_usuario;

            if (GetUserLogged().Id == idUsuario)
            {
                TempData["MensajeError"] = "No es posible autoeliminarse";
                return RedirectToAction("Index");
            }

            if (_tareaRepository.UsuarioTieneTareasAsignadas(idUsuario))
            {
                TempData["MensajeError"] = "No es posible eliminar, el usuario " + "<b>" + userName + "</b>" + " tiene tareas asignadas";
                return RedirectToAction("AllUsers");
            }

            if (_tableroRepository.TableroByUserConTareasAsignadas(idUsuario))
            {
                TempData["MensajeError"] = "No es posible eliminar, el usuario " + "<b>" + userName + "</b>" + " tiene tablero con tareas asignadas";
                return RedirectToAction("AllUsers");
            }

            _usuarioRepository.Delete(idUsuario);

            TempData["MensajeExito"] = "El usuario " + "<b>" + userName + "</b>" + " fue eliminado con éxito!";

            return RedirectToAction("AllUsers");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult Redirect(int idUsuario)
    {
        try
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllUsers");
        }
        catch (System.Exception ex)
        {

            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}