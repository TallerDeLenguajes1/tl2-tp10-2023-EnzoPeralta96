namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;


class TareasByUsuarioViewModels
{
    public string MensajeExito { get; set; }
    public bool TieneMensajeExito => !string.IsNullOrEmpty(MensajeExito);

    public string MensajeError { get; set; }
    public bool TieneMensajeError => !string.IsNullOrEmpty(MensajeError);

    public string MensajeAdvertencia { get; set; }
    public bool TieneMensajeAdvertencia => !string.IsNullOrEmpty(MensajeAdvertencia);
    public List<Tarea> Tareas { get; set; }
    public List<Tarea> TareasAsignadas { get; set; }
    public List<Usuario> Usuarios { get; set; }

    public TareasByUsuarioViewModels(List<Tarea> tareas, List<Usuario> usuarios)
    {
        Tareas = tareas;
        Usuarios = usuarios;
    }

    public TareasByUsuarioViewModels(List<Tarea> tareasAsignadas)
    {
        TareasAsignadas = tareasAsignadas;
    }
}