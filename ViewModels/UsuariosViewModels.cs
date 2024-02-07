namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models;

class UsuariosViewModels
{
    public UsuariosViewModels(List<Usuario> usuarios, Usuario usuario)
    {
        Usuarios = usuarios;
        Usuario = usuario;
    }

    public UsuariosViewModels(Usuario user)
    {
        Usuario = user;
    }


    public List<Usuario> Usuarios{ get; set;}
    public Usuario Usuario{get;set;}

    
}