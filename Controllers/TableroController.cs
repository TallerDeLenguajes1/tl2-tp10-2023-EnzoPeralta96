using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;


public class TableroController : HelperController
{
    private readonly ILogger<TableroController> _logger;
    private readonly ITableroRepository _tableroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITareaRepository _tareaRepository;

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ITareaRepository tareaRepository):
    base(logger,usuarioRepository)
    {
        _logger = logger;
        _tableroRepository = tableroRepository;
        _usuarioRepository = usuarioRepository;
        _tareaRepository = tareaRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            var user = GetUserLogged();
            var tablerosPropios = _tableroRepository.GetTableroByUser(user.Id);
            var tableros = new List<Tablero>();

            if (IsAdmin())
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
    public IActionResult Create()
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
    public IActionResult Create(CreateTableroViewModels tablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("Create");

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
    public IActionResult Update(int idTablero, int Id_usuario_propietario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            

            if (!IsAdmin() && !IsOwner(Id_usuario_propietario)) return RedirectToAction("Index");

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
    public IActionResult Update(UpdateTableroViewModels tablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin() && !IsOwner(tablero.Id_usuario_propietario)) return RedirectToAction("Index");

            if (!ModelState.IsValid) return RedirectToAction("Update");

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
    public IActionResult Delete(int idTablero, int Id_usuario_propietario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin() && !IsOwner(Id_usuario_propietario)) return RedirectToAction("Index");

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
