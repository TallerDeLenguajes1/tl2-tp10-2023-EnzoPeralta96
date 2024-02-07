using tl2_tp10_2023_EnzoPeralta96.Models;
namespace RepositorioUsuario;

public interface IUsuarioRepository
{
    public void Create(Usuario usuario);
    public void Update(int idUsuario, Usuario usuario);
    public List<Usuario> GetRestUsers(int idUsuario);

    public List<Usuario> GetAllUsers();
    public Usuario GetUsuarioById(int idUsuario);
    public void Delete(int idUsuario);
    public Usuario UserExists(string nombre, string pass);
    public bool NameInUse(string nombre);
    public bool NameInUseUpdate(string nombre, int idUsuario);

}