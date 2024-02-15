using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;


namespace tl2_tp10_2023_EnzoPeralta96.Controllers;


public class TableroController : HelperController
{
    private readonly ILogger<TableroController> _logger;
    private readonly ITableroRepository _tableroRepository;
    private readonly ITareaRepository _tareaRepository;
    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ITareaRepository tareaRepository) :
    base(logger, usuarioRepository)
    {
        _logger = logger;
        _tableroRepository = tableroRepository;
        _tareaRepository = tareaRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            TableroViewModels model;

            var user = GetUserLogged();

            var tablerosPropios = _tableroRepository.GetTableroByUser(user.Id);

            model = new TableroViewModels(tablerosPropios)
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

    public IActionResult TablerosRemaining()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            TableroViewModels model;

            var user = GetUserLogged();

            var tableros = new List<Tablero>();

            if (IsAdmin())
            {
                tableros = _tableroRepository.GetRestTableros(user.Id);
            }
            else
            {
                tableros = _tableroRepository.GetTableroByTareasAsignadas(user.Id);
            }

            model = new TableroViewModels(tableros)
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

            TempData["MensajeExito"] = "El tablero " + "<b>" + tablero.Nombre + "</b>" + " fue creado con éxito!";

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

            TempData["MensajeExito"] = "Tablero " + "<b>" + tablero.Nombre + "</b>" + " modificado con éxito!";

            return  IsOwner(tablero.Id_usuario_propietario) ? RedirectToAction("Index") : RedirectToAction("TablerosRemaining");

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }


    public IActionResult Delete(int idTablero, int Id_usuario_propietario)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!IsAdmin() && !IsOwner(Id_usuario_propietario)) return RedirectToAction("Index");

            if (_tableroRepository.TableroConTareasAsignadas(idTablero))
            {
                TempData["MensajeError"] = "No es posible eliminar, el tablero tiene tareas asignadas.";

                return IsOwner(Id_usuario_propietario) ?  RedirectToAction("Index") : RedirectToAction("TablerosRemaining");
            }

            _tableroRepository.Delete(idTablero);

            TempData["MensajeExito"] = "Tablero eliminado con éxito!";

            return IsOwner(Id_usuario_propietario) ? RedirectToAction("Index") : RedirectToAction("TablerosRemaining");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult AddTarea(int idTablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            var tablero = _tableroRepository.GetTableroById(idTablero);
            int idUsuario = tablero.Id_usuario_propietario;
            TempData["NombreTablero"] = tablero.Nombre;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index");

            return View(new CreateTareaViewModels(idTablero));

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    [HttpPost]
    public IActionResult AddTarea(CreateTareaViewModels tarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("AddTarea", new { idTablero = tarea.Id_tablero });

            _tareaRepository.Create(tarea.Id_tablero, new Tarea(tarea));

            TempData["MensajeExito"] = "Tarea agregada con exito al tablero: " + "<b>" + TempData["NombreTablero"].ToString() + "</b>";

            int idUsuario = _tableroRepository.GetTableroById(tarea.Id_tablero).Id_usuario_propietario;

            return IsOwner(idUsuario) ?  RedirectToAction("Index") : RedirectToAction("TablerosRemaining");
          
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
