using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;
using tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;

namespace tl2_tp10_2023_EnzoPeralta96.Controllers;

public class HelperController : Controller
{
    private readonly ILogger<HelperController> _logger;

    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITableroRepository _tableroRepository;

    public HelperController(ILogger<HelperController> logger)
    {
        _logger = logger;
    }
    public HelperController(ILogger<HelperController> logger, IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
    }

    public HelperController(ILogger<HelperController> logger, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _tableroRepository = tableroRepository;
    }

    protected void CreateSession(Usuario userLoged)
    {
        HttpContext.Session.SetInt32("Id", userLoged.Id);
        HttpContext.Session.SetString("Rol", userLoged.Rol.ToString());
        HttpContext.Session.SetString("Autenticado", "true");
    }

    protected bool IsAdmin()
    {
        return HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "admin";
    }
    protected bool IsLogged()
    {
        return HttpContext.Session != null && (HttpContext.Session.GetString("Rol") == "admin" || HttpContext.Session.GetString("Rol") == "operador");
    }
    protected Usuario GetUserLogged()
    {
        int idUsuario = (int)HttpContext.Session.GetInt32("Id");
        return _usuarioRepository.GetUsuarioById(idUsuario);
    }

    protected bool IsOwner(int idUsuario)
    {
        return GetUserLogged().Id == idUsuario;
    }

    protected int GetIdPropietarioTarea(Tarea tarea)
    {
        int idTablero = tarea.Id_tablero;
        return _tableroRepository.GetTableroById(idTablero).Id_usuario_propietario;
    }

}
