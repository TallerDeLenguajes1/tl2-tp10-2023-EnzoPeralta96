namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


public class CreateTableroViewModels
{
    [HiddenInput(DisplayValue = false)]
    [Required(ErrorMessage = "Campo requerido")]
    public int Id_usuario_propietario { get; set; }

    [Display(Name = "Usuario propietario:")]
    [Required(ErrorMessage = "Campo requerido")]
    public string Usuario_propietario { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Nombre de tablero:")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Descripción:")]
    public string Descripcion { get; set; }

    public CreateTableroViewModels()
    {
    }

    public CreateTableroViewModels(Usuario User)
    {
        Id_usuario_propietario = User.Id;
        Usuario_propietario = User.Nombre_de_usuario;
    }
}