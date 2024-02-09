using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class TareasByTableroController : HelperController
{
    private readonly ILogger<TareasByTableroController> _logger;
    private readonly ITareaRepository _tareaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;

    public TareasByTableroController(ILogger<TareasByTableroController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository):
    base(logger,usuarioRepository)
    {
        _logger = logger;
        _tareaRepository = tareaRepository;
        _usuarioRepository = usuarioRepository;
        _tableroRepository = tableroRepository;
    }
    
    public IActionResult Index(int idTablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToRoute(new { controller = "Home", action = "Index" });

            var tareas = _tareaRepository.GetTareasByTablero(idTablero);

            var usuarios = _usuarioRepository.GetAllUsers();

            return View(new TareasByTableroViewModels(tareas, usuarios));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult TareasAsignadasByTablero(int idTablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            var user = GetUserLogged();

            var tareas = _tareaRepository.GetTareasAsignadasByTablero(idTablero, user.Id);

            return View(new TareasAsignadasByTablero(tareas));
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

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToRoute(new { controller = "Tablero", action = "Index" });

            return View(new CreateTareaViewModels(idTablero));

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    [HttpPost]
    public IActionResult AddTarea(CreateTareaViewModels nuevaTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("AddTarea", new { idTablero = nuevaTarea.Id_tablero });

            _tareaRepository.Create(nuevaTarea.Id_tablero, new Tarea(nuevaTarea));

            return RedirectToAction("Index", new { idTablero = nuevaTarea.Id_tablero });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult UpdateTarea(int idTablero, int idTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index", new { idTablero = idTablero });

            var tarea = _tareaRepository.GetTareaById(idTarea);

            return View(new UpdateTareaViewModels(tarea));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult UpdateTarea(UpdateTareaViewModels tareaModificada)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("UpdateTarea", new { idTablero = tareaModificada.Id_tablero, idTarea = tareaModificada.Id });

            _tareaRepository.Update(tareaModificada.Id, new Tarea(tareaModificada));

            return RedirectToAction("Index", new { idTablero = tareaModificada.Id_tablero });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    [HttpPost]
    public IActionResult UpdateEstadoTareaPropia(int idTablero, int idTarea, int EstadoTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if ((!IsAdmin() && !IsOwner(idUsuario)) || !Enum.IsDefined(typeof(Estado), EstadoTarea)) RedirectToAction("Index", new { idTablero = idTablero });

            _tareaRepository.UpdateEstado(idTarea, (Estado)EstadoTarea);

            return RedirectToAction("Index", new { idTablero = idTablero });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult UpdateEstadoTareaAsignada(int idTablero, int idUsuarioAsignado, int idTarea, int EstadoTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if ((!IsAdmin() && !IsOwner(idUsuarioAsignado)) || !Enum.IsDefined(typeof(Estado), EstadoTarea)) RedirectToAction("TareasAsignadasByTablero", new { idTablero = idTablero });

            _tareaRepository.UpdateEstado(idTarea, (Estado)EstadoTarea);

            return RedirectToAction("TareasAsignadasByTablero", new { idTablero = idTablero });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }


    [HttpPost]
    public IActionResult AssignUser(int idTablero, int idTarea, int idUsuarioAsignado)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if ((!IsAdmin() && !IsOwner(idUsuario)) || !_usuarioRepository.IsUserValid(idUsuarioAsignado)) return RedirectToAction("Index", new { idTablero = idTablero });

            _tareaRepository.AssignUser(idTarea, idUsuarioAsignado);

            return RedirectToAction("Index", new { idTablero = idTablero });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }


    public IActionResult RemoveUser(int idTablero, int idTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index", new { idTablero = idTablero });

            _tareaRepository.RemoveUser(idTarea);

            return RedirectToAction("Index", new { idTablero = idTablero });
        }
        catch (System.Exception ex)
        {

            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    public IActionResult DeleteTarea(int idTablero, int idTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index", new { idTablero = idTablero });

            if (_tareaRepository.TareaConUsuarioAsignado(idTarea))
            {

                TempData["MensajeAlerta"] = "No es posible eliminar";
                return RedirectToAction("Index", new { idTablero = idTablero });
            }

            _tareaRepository.Delete(idTarea);

            return RedirectToAction("Index", new { idTablero = idTablero });
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
