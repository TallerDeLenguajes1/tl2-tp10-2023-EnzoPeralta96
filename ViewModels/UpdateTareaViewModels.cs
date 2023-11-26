using tl2_tp10_2023_EnzoPeralta96.Models;
using System.ComponentModel.DataAnnotations;
namespace ViewModels;

public class UpdateTareaViewModels
{
    public List<Tablero> Tableros { get; set; }
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public int Id_tablero { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public Estado EstadoTarea { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Color { get; set; }

    public UpdateTareaViewModels()
    {
    }

    public UpdateTareaViewModels(Tarea tarea)
    {
        Id = tarea.Id;
        Nombre = tarea.Nombre;
        EstadoTarea = tarea.EstadoTarea;
        Descripcion = tarea.Descripcion;
        Color = tarea.Color;
    }

}