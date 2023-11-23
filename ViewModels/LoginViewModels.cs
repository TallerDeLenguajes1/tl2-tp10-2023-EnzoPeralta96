namespace ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class LoginViewModels
{
    [Required(ErrorMessage = "Campo requerido")]
    public string Usuario{get;set;}

    [Required(ErrorMessage = "Campo requerido")]

    [DataType(DataType.Password)]
    public string Password{get;set; }
}