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

    public TareasByTableroController(ILogger<TareasByTableroController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository) :
    base(logger, usuarioRepository)
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

            TareasByTableroViewModels model;

            int idUsuario = _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToRoute(new { controller = "Home", action = "Index" });

            var tareas = _tareaRepository.GetTareasByTablero(idTablero);

            var usuarios = _usuarioRepository.GetAllUsers();

            model = new TareasByTableroViewModels(tareas, usuarios)
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

    public IActionResult TareasAsignadasByTablero(int idTablero)
    {
        try
        {
            if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

            TareasAsignadasByTablero model;

            var user = GetUserLogged();

            var tareas = _tareaRepository.GetTareasAsignadasByTablero(idTablero, user.Id);

            model = new TareasAsignadasByTablero(tareas)
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

            TempData["MensajeExito"] = "La tarea " + "<b>" + tareaModificada.Nombre + "</b>" + " fue modificada con éxito";

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

            string nameTarea = _tareaRepository.GetTareaById(idTarea).Nombre;

            TempData["MensajeExito"] = "El estado de la tarea " + "<b>" + nameTarea + "</b>" + " actualizado con éxito";

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

            string nameTarea = _tareaRepository.GetTareaById(idTarea).Nombre;

            TempData["MensajeExito"] = "El estado de la tarea " + "<b>" + nameTarea + "</b>" + " actualizado con éxito";

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

            if (!IsAdmin() && !IsOwner(idUsuario)) return RedirectToAction("Index", new { idTablero = idTablero });

            if (!_usuarioRepository.IsUserValid(idUsuarioAsignado))
            {
                TempData["MensajeAdvertencia"] = "Debe seleccionar un usuario de la lista.";
                return RedirectToAction("Index", new { idTablero = idTablero });
            }

            _tareaRepository.AssignUser(idTarea, idUsuarioAsignado);

            TempData["MensajeExito"] = "El usuario fue asignado con éxito!";

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

            TempData["MensajeExito"] = "Usuario removido con éxito!";

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

            string nameTarea = _tareaRepository.GetTareaById(idTarea).Nombre;

            if (_tareaRepository.TareaConUsuarioAsignado(idTarea))
            {

                TempData["MensajeError"] = "No es posible eliminar, la tarea " + "<b>" + nameTarea + "</b>" + " fue asignada a un usuario";
                return RedirectToAction("Index", new { idTablero = idTablero });
            }

            _tareaRepository.Delete(idTarea);

            TempData["MensajeExito"] = "La tarea " + "<b>" + nameTarea + "</b>" + " fue borrada con éxito!";

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
