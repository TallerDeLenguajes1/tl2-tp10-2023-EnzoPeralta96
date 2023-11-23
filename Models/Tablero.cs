using ViewModels;

namespace tl2_tp10_2023_EnzoPeralta96.Models;
public class Tablero
{
    
    public int Id{get;set;}
    public int Id_usuario_propietario{get;set;}
    public string Nombre{get;set;}
    public string Descripcion{get;set;}

    public Tablero()
    {
    }

    public Tablero(CreateTableroViewModels CreTableroVM)
    {
        Id_usuario_propietario = CreTableroVM.Id_usuario_propietario;
        Nombre = CreTableroVM.Nombre;
        Descripcion = CreTableroVM.Descripcion;
    }

    public Tablero(UpdateTableroViewModels UpTableroVM)
    {
        Id_usuario_propietario = UpTableroVM.Id_usuario_propietario;
        Nombre = UpTableroVM.Nombre;
        Descripcion = UpTableroVM.Descripcion;
    }



    
}