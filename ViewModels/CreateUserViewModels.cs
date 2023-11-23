using tl2_tp10_2023_EnzoPeralta96.Models;
using System.ComponentModel.DataAnnotations;
namespace ViewModels;

public class CreateUserViewModels
{

    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; }

    [Required]
    public RolUsuario Rol { get; set; }

    [Required(ErrorMessage = "Campo requerido")]

    [DataType(DataType.Password)]
    public string Password { get; set; }

}
