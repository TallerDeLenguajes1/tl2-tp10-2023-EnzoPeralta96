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

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository userRepository, ITareaRepository tareaRepository, ITableroRepository tableroRepository):
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

            if (IsAdmin())
            {
                var user = GetUserLogged();
                var users = _usuarioRepository.GetRestUsers(user.Id);
                return View(new UsuariosViewModels(users,user));
            }
            else
            {
                var user = GetUserLogged();
                return View(new UsuariosViewModels(user));
            }
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
                var userVmMensaje = new CreateUserViewModels
                {
                    MensajeDeError = "Nombre de usuario en uso"
                };
                return View("Create", userVmMensaje);
            }

            if (!ModelState.IsValid) return RedirectToAction("Create");

            _usuarioRepository.Create(new Usuario(user));

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
                    MensajeDeError = "Nombre de usuario en uso"
                };
                return View("Update", userVmMensaje);
            }

            if (!ModelState.IsValid) return RedirectToAction("Index");

            _usuarioRepository.Update(user.Id, new Usuario(user));

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
                TempData["MensajeAlerta"] = "No es posible autoeliminarse";
                return RedirectToAction("Index");
            }

            if (_tareaRepository.UsuarioTieneTareasAsignadas(idUsuario) || _tableroRepository.TableroByUserConTareasAsignadas(idUsuario))
            {
                TempData["MensajeAlerta"] = "No es posible eliminar";
                return RedirectToAction("Index");
            }


            _usuarioRepository.Delete(idUsuario);

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