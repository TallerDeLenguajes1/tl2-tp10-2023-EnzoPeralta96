using tl2_tp10_2023_EnzoPeralta96.Models;
namespace TareaRepositorio;
public interface ITareaRepository
{
    public void Create(int idTablero,Tarea tarea);
    public void Update(int IdTarea, Tarea tarea);
    public void UpdateEstado(int IdTarea, Estado nuevoEstado);
    public Tarea GetTareaById(int idTarea);
    public List<Tarea> GetTareasByUser(int idUsuario);
    public List<Tarea> GetTareasByTablero(int idTablero);
    public void Delete(int idTarea);
    public bool AsignarUsuario(int idTarea,int idUsuario);



}