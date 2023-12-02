namespace TableroRepositorio;
using tl2_tp10_2023_EnzoPeralta96.Models;

public interface ITableroRepository
{
  public void Create(Tablero tablero);
  public void Update(int IdTablero, Tablero tablero);

  public Tablero GetTableroById(int IdTablero);

  public List<Tablero> GetTableros();

  public List<Tablero> GetTableroByUser(int IdUsuario);

  public void Delete(int IdTablero);
}