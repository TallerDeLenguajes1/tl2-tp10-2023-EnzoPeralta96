namespace TableroRepositorio;
using Models.Tablero;

public interface ITableroRepository
{
   public void Create(Tablero tablero);
  public bool Update(int IdTablero, Tablero tablero);

  public Tablero GetTableroById(int IdTablero);

   public List<Tablero> GetTableros();

   public List<Tablero> GetTableroByUser(int IdUsuario);

  public bool Delete(int IdTablero);
}