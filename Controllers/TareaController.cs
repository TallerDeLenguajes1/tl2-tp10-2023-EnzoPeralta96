using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;
using System.Data.Entity.Core.Common.EntitySql;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class TareaController : HelperController
{
    private readonly ILogger<TareaController> _logger;
    private readonly ITareaRepository _tareaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;

    public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository) :
    base(logger, usuarioRepository,tableroRepository)
    {
        _logger = logger;
        _tareaRepository = tareaRepository;
        _usuarioRepository = usuarioRepository;
        _tableroRepository = tableroRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            TareasByUsuarioViewModels model;

            var user = GetUserLogged();
            var tareasPropias = _tareaRepository.GetTareasByUsuario(user.Id);
            var usuarios = _usuarioRepository.GetAllUsers();

            model = new TareasByUsuarioViewModels(tareasPropias, usuarios)
            {
                MensajeExito = TempData["MensajeExito"] as string,
                MensajeError = TempData["MensajeError"] as string,
                MensajeAdvertencia = TempData["MensajeAdvertencia"] as string
            };

            return View(model);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult TareasAsignadas()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            TareasByUsuarioViewModels model;

            var user = GetUserLogged();

            var tareasAsignadas = _tareaRepository.GetTareasAsignadasByUsuario(user.Id);

            model = new TareasByUsuarioViewModels(tareasAsignadas)
            {
                MensajeExito = TempData["MensajeExito"] as string,
                MensajeAdvertencia = TempData["MensajeAdvertencia"] as string
            };

            return View(model);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult AllTareas()
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });

            TareasByUsuarioViewModels model;

            var user = GetUserLogged();

            var tareas = _tareaRepository.GetTareasUsers(user.Id);
            var usuarios = _usuarioRepository.GetAllUsers();

            model = new TareasByUsuarioViewModels(tareas, usuarios)
            {
                MensajeExito = TempData["MensajeExito"] as string,
                MensajeError = TempData["MensajeError"] as string,
                MensajeAdvertencia = TempData["MensajeAdvertencia"] as string
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
            var tableros = _tableroRepository.GetTableroByUser(user.Id);

            return tableros.Count() > 0 ?  View(new CreateTareaViewModels(tableros)) : View("Warning");
      
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    [HttpPost]
    public IActionResult Create(CreateTareaViewModels tareaVM)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("Create");

            _tareaRepository.Create(tareaVM.Id_tablero, new Tarea(tareaVM));

            TempData["MensajeExito"] = "Tarea " + "<b>" + tareaVM.Nombre + "</b>" + " creada con éxito!";

            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Update(int idTablero, int idTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index");

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
    public IActionResult Update(UpdateTareaViewModels tareaUpdate)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid) return RedirectToAction("Update", new { idTablero = tareaUpdate.Id_tablero, idTarea = tareaUpdate.Id });

            var tarea = new Tarea(tareaUpdate);

            _tareaRepository.Update(tarea.Id, tarea);

            TempData["MensajeExito"] = "Tarea " + "<b>" + tarea.Nombre + "</b>" + " modificada con éxito!";

            int idUsuario = GetIdPropietarioTarea(tarea);

            return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }



    [HttpPost]
    public IActionResult UpdateEstadoTarea(int idTablero, int idTarea, int EstadoTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if ((!IsAdmin() && !IsOwner(idUsuario)) || !Enum.IsDefined(typeof(Estado), EstadoTarea)) RedirectToAction("Index");

            _tareaRepository.UpdateEstado(idTarea, (Estado)EstadoTarea);

            TempData["MensajeExito"] = "Estado de la tarea actualizado con éxito!";

            return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult UpdateEstadoTareaAsignada(int idUsuarioAsignado, int idTarea, int EstadoTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            if ((!IsAdmin() && !IsOwner(idUsuarioAsignado)) || !Enum.IsDefined(typeof(Estado), EstadoTarea)) RedirectToAction("TareasAsignadas");

            _tareaRepository.UpdateEstado(idTarea, (Estado)EstadoTarea);

            TempData["MensajeExito"] = "Estado de la tarea actualizado con éxito!";

            return RedirectToAction("TareasAsignadas");
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

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index");

             if (!_usuarioRepository.IsUserValid(idUsuarioAsignado))
            {
                TempData["MensajeAdvertencia"] = "Debe seleccionar un usuario de la lista.";
                return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
            }

            _tareaRepository.AssignUser(idTarea, idUsuarioAsignado);

            TempData["MensajeExito"] = "Usuario asignado con éxito!";

            return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult RemoveUser(int id_tablero, int idTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(id_tablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index");

            _tareaRepository.RemoveUser(idTarea);

            TempData["MensajeExito"] = "Usuario removido con éxito!";

            return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    public IActionResult Delete(int idTablero, int idTarea)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index");

            if (_tareaRepository.TareaConUsuarioAsignado(idTarea))
            {

                TempData["MensajeError"] = "No es posible eliminar, tarea asignada a un usuario";
                return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
            }

            _tareaRepository.Delete(idTarea);

            TempData["MensajeExito"] = "Tarea eliminada con éxito!";

            return IsOwner(idUsuario) ? RedirectToAction("Index") : RedirectToAction("AllTareas");
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

