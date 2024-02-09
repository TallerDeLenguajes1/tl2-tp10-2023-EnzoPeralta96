using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class TareaController : HelperController
{
    private readonly ILogger<TareaController> _logger;
    private readonly ITareaRepository _tareaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;

    public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository):
    base(logger,usuarioRepository)
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

            var user = GetUserLogged();

            var tareasPropias = _tareaRepository.GetTareasByUsuario(user.Id);

            var usuarios = _usuarioRepository.GetAllUsers();

            return View(new TareasByUsuarioViewModels(tareasPropias, usuarios));
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

            var user = GetUserLogged();

            var tareasAsignadas = _tareaRepository.GetTareasAsignadasByUsuario(user.Id);

            return View(new TareasByUsuarioViewModels(tareasAsignadas));
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
            return View(new CreateTareaViewModels(tableros));

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

            if (!ModelState.IsValid) return RedirectToAction("UpdateTarea", new { idTablero = tareaUpdate.Id_tablero, idTarea = tareaUpdate.Id });

            _tareaRepository.Update(tareaUpdate.Id, new Tarea(tareaUpdate));

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
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

            //hacer un metodo para controlar el id válido

            if ((!IsAdmin() && !IsOwner(idUsuario)) || !_usuarioRepository.IsUserValid(idUsuarioAsignado)) return RedirectToAction("Index");

            _tareaRepository.AssignUser(idTarea, idUsuarioAsignado);

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
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

                TempData["MensajeAlerta"] = "No es posible eliminar";
                return RedirectToAction("Index");
            }

            _tareaRepository.Delete(idTarea);

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

