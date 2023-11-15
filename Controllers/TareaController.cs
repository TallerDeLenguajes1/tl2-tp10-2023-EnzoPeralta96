using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TareaRepositorio;
using RepositorioUsuario;

using tl2_tp10_2023_EnzoPeralta96.Models;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

/*
En el controlador de tareas: Listar, Crear, Modificar y Eliminar Tareas. (Por el
momento asuma que el tablero al que pertenece la tarea es siempre la misma, y que
no posee usuario asignado)*/

public class TareaController : Controller
{
    private readonly ILogger<TareaController> _logger;
    private TareaRepository _tareaRepository;
    private UsuarioRepository _usuarioRepository;

    public TareaController(ILogger<TareaController> logger)
    {
        _logger = logger;
        _tareaRepository = new TareaRepository();
    }

    public IActionResult Index()
    {
        var tareas = _tareaRepository.GetTareasByTablero(1);
        return View(tareas);
    }


    [HttpGet]
    public IActionResult CreateTarea()
    {
        return View(new Tarea());
    }

    [HttpPost]
    public IActionResult CreateTarea(Tarea tarea)
    {
        _tareaRepository.Create(1, tarea);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult UpdateTarea(int idTarea)
    {
        var tarea = _tareaRepository.GetTareaById(idTarea);
        return View(tarea);
    }


    [HttpPost]
    public IActionResult UpdateTarea(Tarea tarea)
    {
        _tareaRepository.Update(tarea.Id, tarea);
        return RedirectToAction("Index");
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
        _usuarioRepository = new UsuarioRepository(); 
        var user = _usuarioRepository.GetUsuarioById((int)tarea.Id_usuario_asignado);
        tarea.Id_usuario_asignado = user.Id;
        _tareaRepository.AsignarUsuario(tarea.Id,(int)tarea.Id_usuario_asignado);
        return RedirectToAction("Index");
    }


    public IActionResult DeleteTarea(int idTarea)
    {
        _tareaRepository.Delete(idTarea);
        return RedirectToAction("Index");
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
