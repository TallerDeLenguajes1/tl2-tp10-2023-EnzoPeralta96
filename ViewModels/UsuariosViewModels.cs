namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;

public class UsuariosViewModels
{
    public string MensajeExito { get; set; }
    public bool TieneMensajeExito => !string.IsNullOrEmpty(MensajeExito);

    public string MensajeError { get; set; }
    public bool TieneMensajeError => !string.IsNullOrEmpty(MensajeError);

    public List<Usuario> Usuarios { get; set; }
    public Usuario Usuario { get; set; }

    public UsuariosViewModels(Usuario usuario, List<Usuario> usuarios)
    {
        Usuarios = usuarios;
        Usuario = usuario;
    }

    public UsuariosViewModels(Usuario user)
    {
        Usuario = user;
    }

    public UsuariosViewModels()
    {
    }


}