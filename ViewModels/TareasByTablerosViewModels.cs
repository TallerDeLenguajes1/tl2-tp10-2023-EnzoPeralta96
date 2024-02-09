namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;

public class TareasByTableroViewModels
{
    public List<Tarea> Tareas { get; set; }
    public List<Usuario> Usuarios { get; set; }
    
    public TareasByTableroViewModels(List<Tarea> tareas, List<Usuario> usuarios)
    {
        Tareas = tareas;
        Usuarios = usuarios;
    }
}
