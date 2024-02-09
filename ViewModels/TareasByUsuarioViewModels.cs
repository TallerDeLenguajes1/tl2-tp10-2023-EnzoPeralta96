namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;


class TareasByUsuarioViewModels
{
    public List<Tarea> TareasPropias { get; set; }
    public List<Tarea> TareasAsignadas { get; set; }
    public List<Usuario> Usuarios {get;set;}

    public TareasByUsuarioViewModels(List<Tarea> tareasPropias, List<Usuario> usuarios)
    {
        TareasPropias = tareasPropias;
        Usuarios = usuarios;
    }
    
    public TareasByUsuarioViewModels(List<Tarea> tareasAsignadas)
    {
        TareasAsignadas = tareasAsignadas;
    }
}