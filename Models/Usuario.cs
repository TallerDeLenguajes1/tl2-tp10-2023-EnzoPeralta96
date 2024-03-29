namespace tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using ViewModels;

public enum RolUsuario
{
    admin,
    operador
}
public class Usuario
{
    public int Id {get; set;}
    public string Nombre_de_usuario{get; set;}
    public RolUsuario Rol{get;set;}
    public string Password{get;set;}
    public int Activo{get;set;}
    
    public Usuario()
    {

    }

    public Usuario(CreateUserViewModels userCreate)
    {
        Nombre_de_usuario = userCreate.Name;
        Rol = userCreate.Rol;
        Password = userCreate.Password;
    }

    public Usuario(UpdateUserViewModels userUpdate)
    {
        Id = userUpdate.Id;
        Nombre_de_usuario = userUpdate.Name;
        //Rol = userUpdate.Rol;
        Password = userUpdate.Password;
    }

}