namespace ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_EnzoPeralta96.Models;
using Microsoft.AspNetCore.Mvc;

public class CreateTareaViewModels
{

    [HiddenInput(DisplayValue = false)]
    [Required(ErrorMessage = "Campo requerido")]
    public int Id_tablero { get; set; }


    [Display(Name = "Tablero:")]
    public string NombreTablero { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Nombre:")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Estado de la tarea:")]
    public Estado EstadoTarea { get; set; }

    [Display(Name = "Descripción:")]
    [Required(ErrorMessage = "Campo requerido")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Color:")]
    public string Color { get; set; }

    [HiddenInput(DisplayValue = false)]
    public int Id_Propietario_Tablero { get; set; }

    public List<Tablero> Tableros{get;set;}
    public CreateTareaViewModels()
    {
    }

    public CreateTareaViewModels(int idTablero, int idPropietarioTablero)
    {
        Id_tablero = idTablero;
        Id_Propietario_Tablero = idPropietarioTablero;
    }

    public CreateTareaViewModels(List<Tablero> tableros)
    {
        Tableros = tableros;
    }
    
}