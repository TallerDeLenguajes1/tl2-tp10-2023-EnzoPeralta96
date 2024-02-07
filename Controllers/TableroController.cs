using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositorioUsuario;
using TableroRepositorio;
using TareaRepositorio;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;


public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private readonly ITableroRepository _tableroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITareaRepository _tareaRepository;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ITareaRepository tareaRepository)
    {
        _logger = logger;
        _tableroRepository = tableroRepository;
        _usuarioRepository = usuarioRepository;
        _tareaRepository = tareaRepository;
    }

    private bool UserLogedIsAdmin()
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

    private bool IsOwner(int idUsuario)
    {
        return GetUserLogged().Id == idUsuario;
    }

    public IActionResult Index()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            var user = GetUserLogged();
            var tablerosPropios = _tableroRepository.GetTableroByUser(user.Id);
            var tableros = new List<Tablero>();

            if (UserLogedIsAdmin())
            {
                tableros = _tableroRepository.GetRestTableros(user.Id);
            }
            else
            {
                tableros = _tableroRepository.GetTableroByTareas(user.Id);
            }

            return View(new TableroViewModels(tablerosPropios, tableros));

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    


    [HttpGet]
    public IActionResult CreateTablero()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            var user = GetUserLogged();

            return View(new CreateTableroViewModels(user));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }


    [HttpPost]
    public IActionResult CreateTablero(CreateTableroViewModels tablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("CreateTablero");

            _tableroRepository.Create(new Tablero(tablero));

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }



    [HttpGet]
    public IActionResult UpdateTablero(int idTablero, int Id_usuario_propietario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!UserLogedIsAdmin() && !IsOwner(Id_usuario_propietario)) return RedirectToAction("Index");

            var tablero = _tableroRepository.GetTableroById(idTablero);

            return View(new UpdateTableroViewModels(tablero));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult UpdateTablero(UpdateTableroViewModels tablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!UserLogedIsAdmin() && !IsOwner(tablero.Id_usuario_propietario)) return RedirectToAction("Index");

            if (!ModelState.IsValid) return RedirectToAction("UpdateTablero");

            _tableroRepository.Update(tablero.Id, new Tablero(tablero));

            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    //Ver cuestiones de las tareas asignadas
    public IActionResult DeleteTablero(int idTablero, int Id_usuario_propietario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!UserLogedIsAdmin() && !IsOwner(Id_usuario_propietario)) return RedirectToAction("Index");

            if (_tableroRepository.TableroConTareasAsignadas(idTablero))
            {
                TempData["MensajeAlerta"] = "No es posible eliminar";
                return RedirectToAction("Index");
            }

            _tableroRepository.Delete(idTablero);

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
