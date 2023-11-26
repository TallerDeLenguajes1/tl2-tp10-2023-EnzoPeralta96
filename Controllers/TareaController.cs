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

    public TareaController(ILogger<TareaController> logger,  ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository)
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
    public IActionResult TareasByTablero(int idTablero)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        var tareas = _tareaRepository.GetTareasByTablero(idTablero);
        return View(tareas);
    }

    [HttpGet]
    public IActionResult CreateTarea()
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

        var creTareaVM = new CreateTareaViewModels();

        if (IsAdmin())
        {
            creTareaVM.Tableros = _tableroRepository.GetTableros();
        }
        else
        {
            var user = _usuarioRepository.GetAllUsers().FirstOrDefault(u => u.Nombre_de_usuario == HttpContext.Session.GetString("Usuario") && u.Password == HttpContext.Session.GetString("Password"));

            creTareaVM.Tableros = _tableroRepository.GetTableroByUser(user.Id);
        }

        return View(creTareaVM);
    }

    [HttpPost]
    public IActionResult CreateTarea(CreateTareaViewModels tareaVM)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!ModelState.IsValid) return RedirectToAction("CreateTarea");
        _tareaRepository.Create(tareaVM.Id_tablero, new Tarea(tareaVM));
        return RedirectToAction("TareasByTablero", new { idTablero = tareaVM.Id_tablero });
    }


    [HttpGet]
    public IActionResult UpdateTarea(int idTarea)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });

        var upTareaVM = new UpdateTareaViewModels(_tareaRepository.GetTareaById(idTarea));

        if (IsAdmin())
        {
            upTareaVM.Tableros = _tableroRepository.GetTableros();
        }else
        {
            var user = _usuarioRepository.GetAllUsers().FirstOrDefault(u => u.Nombre_de_usuario == HttpContext.Session.GetString("Usuario") && u.Password == HttpContext.Session.GetString("Password"));
            upTareaVM.Tableros = _tableroRepository.GetTableroByUser(user.Id); 
        }
        return View(upTareaVM);
    }


    [HttpPost]
    public IActionResult UpdateTarea(UpdateTareaViewModels upTareaVM)
    {
        if (!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        if (!ModelState.IsValid) return RedirectToAction("UpdateTarea", new {idTarea = upTareaVM.Id});
        _tareaRepository.Update(upTareaVM.Id, new Tarea(upTareaVM));
        return RedirectToAction("TareasByTablero", new { idTablero = upTareaVM.Id_tablero });
    }

    [HttpGet]
    public IActionResult AsignarUser(int idTarea)
    {
        var tarea = _tareaRepository.GetTareaById(idTarea);
        return View(tarea);
    }

    [HttpPost]
    public IActionResult AsignarUser(Tarea tarea)
    {
        var user = _usuarioRepository.GetUsuarioById((int)tarea.Id_usuario_asignado);
        tarea.Id_usuario_asignado = user.Id;
        _tareaRepository.AsignarUsuario(tarea.Id, (int)tarea.Id_usuario_asignado);
        return RedirectToAction("Index");
    }


    public IActionResult DeleteTarea(int idTableroDe,int idTareaDE)
    {
        _tareaRepository.Delete(idTareaDE);
        return RedirectToAction("TareasByTablero", new{idTablero = idTableroDe});
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
