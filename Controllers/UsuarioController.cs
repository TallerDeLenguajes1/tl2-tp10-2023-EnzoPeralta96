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

            TempData["MensajeExito"] = "Usuario creado con éxito!";

            return RedirectToAction("Index");
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

            _usuarioRepository.Update(user.Id, new Usuario(user));

            TempData["MensajeExito"] = "Usuario modificado con éxito!";

            return RedirectToAction("Index");
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

            if (GetUserLogged().Id == idUsuario)
            {
                TempData["MensajeError"] = "No es posible autoeliminarse";
                return RedirectToAction("Index");
            }

            if (_tareaRepository.UsuarioTieneTareasAsignadas(idUsuario))
            {
                TempData["MensajeError"] = "No es posible eliminar, usuario con tareas asignadas";
                return RedirectToAction("Index");
            }

            if (_tableroRepository.TableroByUserConTareasAsignadas(idUsuario))
            {
                TempData["MensajeError"] = "No es posible eliminar, usuario tiene tablero con tareas asignadas";
                return RedirectToAction("Index");
            }

            _usuarioRepository.Delete(idUsuario);

            TempData["MensajeExito"] = "Usuario eliminado con éxito!";

            return RedirectToAction("Index");
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