using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models;
using TareaRepositorio;
using TableroRepositorio;
using Microsoft.AspNetCore.Identity;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;
    private readonly ITareaRepository _tareaRepository;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository userRepository, ITareaRepository tareaRepository, ITableroRepository tableroRepository)
    {
        _logger = logger;
        _usuarioRepository = userRepository;
        _tareaRepository = tareaRepository;
        _tableroRepository = tableroRepository;
    }

    private bool IsAdmin()
    {
        return HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "admin";
    }
    private bool IsLogged()
    {
        return HttpContext.Session != null && (HttpContext.Session.GetString("Rol") == "admin" || HttpContext.Session.GetString("Rol") == "operador");
    }

    private Usuario GetUserLogged()
    {
        int idUsuario = (int)HttpContext.Session.GetInt32("Id");
        return _usuarioRepository.GetUsuarioById(idUsuario);
    }

    private bool IsOwner(int IdUsuario)
    {
        return GetUserLogged().Id == IdUsuario;
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
    public IActionResult CreateUser()
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
    public IActionResult CreateUser(CreateUserViewModels user)
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
                return View("CreateUser", userVmMensaje);
            }

            if (!ModelState.IsValid) return RedirectToAction("CreateUser");

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
    public IActionResult UpdateUser(int idUsuario)
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
    public IActionResult UpdateUser(UpdateUserViewModels user)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //Hace falta verificar el usuario o el admin aquí

            if (_usuarioRepository.NameInUseUpdate(user.Name, user.Id))
            {
                var userVmMensaje = new UpdateUserViewModels
                {
                    MensajeDeError = "Nombre de usuario en uso"
                };
                return View("UpdateUser", userVmMensaje);
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

    public IActionResult DeleteUser(int idUsuario)
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