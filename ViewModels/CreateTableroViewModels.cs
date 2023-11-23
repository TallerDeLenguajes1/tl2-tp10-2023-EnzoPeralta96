using tl2_tp10_2023_EnzoPeralta96.Models;
using System.ComponentModel.DataAnnotations;
namespace ViewModels;

public class CreateTableroViewModels
{

    [Required(ErrorMessage = "Campo requerido")]
    public int Id_usuario_propietario { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Descripcion { get; set; }
    public List<Usuario> Usuarios { get; set; }

    public CreateTableroViewModels()
    {
    }

    public CreateTableroViewModels(List<Usuario> usuariosDB)
    {
        Usuarios = usuariosDB;
    }

    
}