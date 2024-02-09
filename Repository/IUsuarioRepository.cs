namespace tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;

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

    public bool IsUserValid(int idUsuario);

}