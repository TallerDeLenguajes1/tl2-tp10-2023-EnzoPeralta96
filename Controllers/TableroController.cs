using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TableroRepositorio;

using tl2_tp10_2023_EnzoPeralta96.Models;
namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

/*En el controlador de tableros: Listar, Crear, Modificar y Eliminar Tableros. (Por el
momento asuma que el usuario propietario es siempre el mismo)
*/
public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private TableroRepository _tableroRepository;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        _tableroRepository = new TableroRepository();
    }

    public IActionResult Index()
    {
        var tableros = _tableroRepository.GetTableros();
        return View(tableros);
    }


    [HttpGet]
    public IActionResult CreateTablero()
    {
        return View(new Tablero());
    }

    [HttpPost]
    public IActionResult CreateTablero(Tablero tablero)
    {
        _tableroRepository.Create(tablero);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult UpdateTablero(int idTablero)
    {
        var tablero = _tableroRepository.GetTableroById(idTablero);
        return View(tablero);
    }


   [HttpPost]
    public IActionResult UpdateTablero(Tablero tablero)
    {
        _tableroRepository.Update(tablero.Id, tablero);
        return RedirectToAction("Index");
    }

    
    public IActionResult DeleteTablero(int idTablero)
    {
        _tableroRepository.Delete(idTablero);
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
