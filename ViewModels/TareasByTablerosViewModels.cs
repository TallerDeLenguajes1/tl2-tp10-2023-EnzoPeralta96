using tl2_tp10_2023_EnzoPeralta96.Models;

namespace ViewModels;

public class TareasByTableroViewModels
{
    public List<Tarea> Tareas { get; set; }
    public List<Usuario> Usuarios { get; set; }

    public int IdPropietarioTablero {get;set;}

    public TareasByTableroViewModels(List<Tarea> tareas, List<Usuario> usuarios, int Id_usuario_propietario)
    {
        Tareas = tareas;
        Usuarios = usuarios;
        IdPropietarioTablero = Id_usuario_propietario;
    }

   

}
