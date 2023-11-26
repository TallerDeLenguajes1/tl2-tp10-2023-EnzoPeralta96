using tl2_tp10_2023_EnzoPeralta96.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace ViewModels;

public class UpdateUserViewModels
{
  
    public int Id{get;set;}

    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; }

    [Required]
    public RolUsuario Rol { get; set; }

    [Required(ErrorMessage = "Campo requerido")]

    [DataType(DataType.Password)]
    public string Password { get; set; }

      public UpdateUserViewModels(int IdUsuario)
    {
        Id = IdUsuario;
    }

    public UpdateUserViewModels()
    {
        
    }

}