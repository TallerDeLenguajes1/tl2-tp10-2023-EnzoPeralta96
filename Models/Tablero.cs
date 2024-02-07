using ViewModels;

namespace tl2_tp10_2023_EnzoPeralta96.Models;
public class Tablero
{
    
    public int Id{get;set;}
    public int Id_usuario_propietario{get;set;}
    public string Usuario_propietario {get;set;}
    public string Nombre{get;set;}
    public string Descripcion{get;set;}
    public int Activo{get;set;}
    

    public Tablero()
    {
        
    }

    public Tablero(CreateTableroViewModels tablero)
    {
        Id_usuario_propietario = tablero.Id_usuario_propietario;
        Nombre = tablero.Nombre;
        Descripcion = tablero.Descripcion;
    }

    public Tablero(UpdateTableroViewModels tablero)
    {
        Nombre = tablero.Nombre;
        Descripcion = tablero.Descripcion;
    }



    
}