namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using System.ComponentModel.DataAnnotations;

public class CreateUserViewModels
{
    public string MensajeDeError { get; set; }

    public bool TieneMensajeDeError => !string.IsNullOrEmpty(MensajeDeError);

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Nombre de usuario:")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Rol:")]
    public RolUsuario Rol { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña:")]
    public string Password { get; set; }
}
