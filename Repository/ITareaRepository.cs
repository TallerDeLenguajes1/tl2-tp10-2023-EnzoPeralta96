namespace tl2_tp10_2023_EnzoPeralta96.Repository.Tarea;
using tl2_tp10_2023_EnzoPeralta96.Models.Tarea;
public interface ITareaRepository
{
    public void Create(int idTablero, Tarea tarea);
    public void Update(int IdTarea, Tarea tarea);
    public void Delete(int idTarea);
    public void UpdateEstado(int IdTarea, Estado nuevoEstado);
    public void AssignUser(int IdTarea, int IdUsuario);
    public void RemoveUser(int IdTarea);
    public Tarea GetTareaById(int idTarea);
    public List<Tarea> GetTareasAsignadasByTablero(int idTablero, int idUsuario);
    public List<Tarea> GetTareasByTablero(int idTablero);
    public List<Tarea> GetTareasByUsuario(int IdUsuario);
    public List<Tarea> GetTareasAsignadasByUsuario(int IdUsuario);
    public bool UsuarioTieneTareasAsignadas(int idUsuario);
    public bool TareaConUsuarioAsignado(int IdUsuario);
}