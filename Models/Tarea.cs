namespace tl2_tp10_2023_EnzoPeralta96.Models;
using ViewModels;

public enum Estado
{
    ToDo,
    Doing,
    Review,
    Done
}
public class Tarea
{
    public int Id{get;set;}
    public int Id_tablero{get;set;}
    public string Nombre{get;set;}
    public Estado EstadoTarea{get;set;}
    public string Descripcion{get;set;}
    public string Color{get;set;}
    public int? Id_usuario_asignado{get;set;}
    public string Usuario_asignado{get;set;}
    public int Activo{get;set;}

    public Tarea()
    {

    }
    public Tarea(CreateTareaViewModels creTareaVM)
    {
        Id_tablero = creTareaVM.Id_tablero;
        Nombre = creTareaVM.Nombre;
        EstadoTarea = creTareaVM.EstadoTarea;
        Descripcion = creTareaVM.Descripcion;
        Color = creTareaVM.Color;
    }

    public Tarea(UpdateTareaViewModels upTareaVM)
    {
        Id_tablero = upTareaVM.Id_tablero;
        Nombre = upTareaVM.Nombre;    
        EstadoTarea = upTareaVM.EstadoTarea;
        Descripcion = upTareaVM.Descripcion;
        Color = upTareaVM.Color;
        Id_usuario_asignado = upTareaVM.Id_usuario_asignado;
    }
}