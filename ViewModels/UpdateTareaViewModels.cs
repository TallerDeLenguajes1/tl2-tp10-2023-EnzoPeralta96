using tl2_tp10_2023_EnzoPeralta96.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace ViewModels;

public class UpdateTareaViewModels
{
    [HiddenInput(DisplayValue = false)]
    [Required(ErrorMessage = "Campo requerido")]
    public int Id { get; set; }

    [HiddenInput(DisplayValue = false)]
    [Required(ErrorMessage = "Campo requerido")]
    public int Id_tablero { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Nombre:")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Estado de la tarea:")]
    public Estado EstadoTarea { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Descripción:")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [Display(Name = "Color:")]
    public string Color { get; set; }

    [HiddenInput(DisplayValue = false)]
    public int? Id_usuario_asignado { get; set; }

    [HiddenInput(DisplayValue = false)]
    public int Id_Propietario_Tablero { get; set; }

    public UpdateTareaViewModels()
    {
    }

    public UpdateTareaViewModels(Tarea tarea, int IdPropietarioTablero)
    {
        Id = tarea.Id;
        Id_tablero = tarea.Id_tablero;
        Id_Propietario_Tablero = IdPropietarioTablero;
        Nombre = tarea.Nombre;
        EstadoTarea = tarea.EstadoTarea;
        Descripcion = tarea.Descripcion;
        Color = tarea.Color;
        Id_usuario_asignado = tarea.Id_usuario_asignado;
    }

}