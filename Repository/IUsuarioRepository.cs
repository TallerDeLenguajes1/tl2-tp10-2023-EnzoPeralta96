using tl2_tp10_2023_EnzoPeralta96.Models;
namespace RepositorioUsuario;

public interface IUsuarioRepository
{
    public void Create(Usuario usuario);
    public void Update(int idUsuario, Usuario usuario);
    public List<Usuario> GetAllUsuarios();
    public Usuario GetUsuarioById(int idUsuario);
    public bool DeleteUsuarioById(int idUsuario);
}