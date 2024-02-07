using tl2_tp10_2023_EnzoPeralta96.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
namespace ViewModels;

public class UpdateTableroViewModels
{
    [HiddenInput(DisplayValue = false)]
    [Required(ErrorMessage = "Campo requerido")]
    public int Id { get; set; }

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


    public UpdateTableroViewModels()
    {
    }

    public UpdateTableroViewModels(Tablero tablero)
    {
        Id = tablero.Id;
        Id_usuario_propietario = tablero.Id_usuario_propietario;
        Usuario_propietario = tablero.Usuario_propietario;
        Nombre = tablero.Nombre;
        Descripcion = tablero.Descripcion;
    }


}