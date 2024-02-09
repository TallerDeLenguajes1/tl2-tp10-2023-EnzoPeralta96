namespace tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;

public interface ITableroRepository
{
  public void Create(Tablero tablero);
  public void Update(int IdTablero, Tablero tablero);
  public Tablero GetTableroById(int IdTablero);
  public List<Tablero> GetRestTableros(int IdUsuario);
  public List<Tablero> GetTableroByUser(int IdUsuario);
  public List<Tablero> GetTableroByTareas(int IdUsuario);
  public bool TableroByUserConTareasAsignadas(int IdUsuario);
  public bool TableroConTareasAsignadas(int IdTablero);
  public void Delete(int IdTablero);
}