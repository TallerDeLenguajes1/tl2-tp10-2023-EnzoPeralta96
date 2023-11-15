namespace tl2_tp10_2023_EnzoPeralta96.Models;

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
}