namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;


class TareasAsignadasByTablero
{
    public List<Tarea> Tareas { get; set; }
    
    public TareasAsignadasByTablero(List<Tarea> tareas)
    {
        Tareas = tareas;
    }

}
