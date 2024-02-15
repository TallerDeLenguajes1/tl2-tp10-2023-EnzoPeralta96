namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;


class TareasAsignadasByTablero
{
    public string MensajeExito { get; set; }
    public bool TieneMensajeExito => !string.IsNullOrEmpty(MensajeExito);

    public string MensajeError { get; set; }
    public bool TieneMensajeError => !string.IsNullOrEmpty(MensajeError);
    public List<Tarea> Tareas { get; set; }

    public TareasAsignadasByTablero(List<Tarea> tareas)
    {
        Tareas = tareas;
    }

}
