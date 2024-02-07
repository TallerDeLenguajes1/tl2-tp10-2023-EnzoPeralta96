using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TareaRepositorio;
using RepositorioUsuario;
using TableroRepositorio;
using ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<TareaController> _logger;
    private readonly ITareaRepository _tareaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;

    public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository)
    {
        _logger = logger;
        _tareaRepository = tareaRepository;
        _usuarioRepository = usuarioRepository;
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

    private bool IsOwner(int Id_usuario_propietario)
    {
        return GetUserLogged().Id == Id_usuario_propietario;
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

    //hasta aquí llegue

   /* [HttpGet]
     public IActionResult UpdateTarea(int id_tablero, int idTarea, int IdPropietarioTablero)
     {
         try
         {
             if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

             if (!IsAdmin() && !IsOwner(IdPropietarioTablero)) return RedirectToAction("Index", new { idTablero = id_tablero, idPropietarioTablero = IdPropietarioTablero });

             var tarea = _tareaRepository.GetTareaById(idTarea);

             return View(new UpdateTareaViewModels(tarea, IdPropietarioTablero));
         }
         catch (System.Exception ex)
         {
             _logger.LogError(ex.ToString());
             return BadRequest();
         }
     }

     [HttpPost]
     public IActionResult UpdateTarea(UpdateTareaViewModels upTareaVM)
     {
         try
         {
             if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

             if (!ModelState.IsValid) return RedirectToAction("UpdateTarea", new { idTarea = upTareaVM.Id, IdPropietarioTablero = upTareaVM.Id_Propietario_Tablero });

             _tareaRepository.Update(upTareaVM.Id, new Tarea(upTareaVM));

             return RedirectToAction("Index", new { idTablero = upTareaVM.Id_tablero, idPropietarioTablero = upTareaVM.Id_Propietario_Tablero });
         }
         catch (System.Exception ex)
         {
             _logger.LogError(ex.ToString());
             return BadRequest();
         }

     }

     [HttpPost]
     public IActionResult UpdateEstadoTareaPropia(int idTablero, int idUsuario, int idTarea, int EstadoTarea)
     {
         try
         {
             if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

             if ((!IsAdmin() && !IsOwner(idUsuario)) || !Enum.IsDefined(typeof(Estado), EstadoTarea)) RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = idUsuario });

             _tareaRepository.UpdateEstado(idTarea, (Estado)EstadoTarea);

             return RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = idUsuario });
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
     public IActionResult AssignUser(int idTablero, int idTarea, int idUsuarioPropietario, int idUsuarioAsignado)
     {
         try
         {
             if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

             if ((!IsAdmin() && !IsOwner(idUsuarioPropietario)) || idUsuarioAsignado == 0) return RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = idUsuarioPropietario });

             _tareaRepository.AssignUser(idTarea, idUsuarioAsignado);

             return RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = idUsuarioPropietario });
         }
         catch (System.Exception ex)
         {
             _logger.LogError(ex.ToString());
             return BadRequest();
         }
     }


     public IActionResult RemoveUser(int id_tablero, int idUsuarioPropietario, int idTarea)
     {
         try
         {
             if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
             if (!IsAdmin() && !IsOwner(idUsuarioPropietario)) return RedirectToAction("Index", new { idTablero = id_tablero, idPropietarioTablero = idUsuarioPropietario });

             _tareaRepository.RemoveUser(idTarea);

             return RedirectToAction("Index", new { idTablero = id_tablero, idPropietarioTablero = idUsuarioPropietario });
         }
         catch (System.Exception ex)
         {

             _logger.LogError(ex.ToString());
             return BadRequest();
         }

     }

     public IActionResult DeleteTarea(int idTablero, int idTarea, int IdPropietarioTablero)
     {
         try
         {
             if (!IsAdmin() && !IsOwner(IdPropietarioTablero)) return RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = IdPropietarioTablero });

             if (_tareaRepository.TareaConUsuarioAsignado(idTarea))
             {

                 TempData["MensajeAlerta"] = "No es posible eliminar";
                 return RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = IdPropietarioTablero });
             }

             _tareaRepository.Delete(idTarea);

             return RedirectToAction("Index", new { idTablero = idTablero, idPropietarioTablero = IdPropietarioTablero });
         }
         catch (System.Exception ex)
         {
             _logger.LogError(ex.ToString());
             return BadRequest();
         }
     }*/

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

