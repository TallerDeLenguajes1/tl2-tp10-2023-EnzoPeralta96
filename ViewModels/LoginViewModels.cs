namespace ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class LoginViewModels
{
    public string MensajeDeError { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name ="Usuario:")] 
    public string Usuario { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [DataType(DataType.Password)]
    [Display(Name ="Contraseña:")] 
    public string Password { get; set; }

    public bool TieneMensajeDeError => !string.IsNullOrEmpty(MensajeDeError);
   
}