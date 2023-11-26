namespace ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_EnzoPeralta96.Models;

public class CreateTareaViewModels
{
    public List<Tablero> Tableros{get;set;}
    
    [Required(ErrorMessage = "Campo requerido")]
    public int Id_tablero{get;set;}

    [Required(ErrorMessage = "Campo requerido")]
    public string Nombre{get;set;}

    [Required(ErrorMessage = "Campo requerido")]
    public Estado EstadoTarea{get;set;}

    [Required(ErrorMessage = "Campo requerido")]
    public string Descripcion{get;set;}

    [Required(ErrorMessage = "Campo requerido")]
    public string Color{get;set;}

}