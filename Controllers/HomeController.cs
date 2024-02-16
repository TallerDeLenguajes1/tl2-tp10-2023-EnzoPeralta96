using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;


namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class HomeController : HelperController
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITareaRepository _tareaRepository;

    public HomeController(ILogger<HomeController> logger, ITareaRepository tareaRepository, IUsuarioRepository usuarioRepository) :
    base(logger, usuarioRepository)
    {
        _logger = logger;
        _tareaRepository = tareaRepository;
    }




    public IActionResult Index()
    {
        try
        {
            DatosViewModels model = null;
            if (IsLogged())
            {
                var user = GetUserLogged();
                model = new DatosViewModels()
                {
                    Usuario = user.Nombre_de_usuario,
                    TareasAsignadas = _tareaRepository.GetCantidadTareasAsignadasByUser(user.Id),
                    CantidadToDo = _tareaRepository.GetCantidadTareasAsignadasByEstado(user.Id, (int)Estado.ToDo),
                    CantidadDoing = _tareaRepository.GetCantidadTareasAsignadasByEstado(user.Id, (int)Estado.Doing),
                    CantidadReiview = _tareaRepository.GetCantidadTareasAsignadasByEstado(user.Id, (int)Estado.Review),
                    CantidadDone = _tareaRepository.GetCantidadTareasAsignadasByEstado(user.Id, (int)Estado.Done)
                };
            }
            return View(model);
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
