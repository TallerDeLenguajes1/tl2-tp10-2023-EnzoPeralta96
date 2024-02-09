namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;


public class UpdateUserViewModels
{

     public string MensajeDeError { get; set; }

    public bool TieneMensajeDeError => !string.IsNullOrEmpty(MensajeDeError);

    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Nombre de usuario:")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña:")]
    public string Password { get; set; }

    public UpdateUserViewModels(Usuario user)
    {
        Id = user.Id;
        Name = user.Nombre_de_usuario;
        //Rol = user.Rol;
        Password = user.Password;
    }

    public UpdateUserViewModels()
    {

    }

}