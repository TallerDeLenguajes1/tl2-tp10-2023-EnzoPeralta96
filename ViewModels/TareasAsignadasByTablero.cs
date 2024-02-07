using tl2_tp10_2023_EnzoPeralta96.Models;
namespace ViewModels;

class TareasAsignadasByTablero
{
    public List<Tarea> Tareas { get; set; }
    
    public TareasAsignadasByTablero(List<Tarea> tareas)
    {
        Tareas = tareas;
    }

}
