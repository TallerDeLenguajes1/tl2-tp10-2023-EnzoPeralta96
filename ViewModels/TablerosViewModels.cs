using tl2_tp10_2023_EnzoPeralta96.Models;
namespace ViewModels;

public class TableroViewModels
{

    public int Id { get; set; }
    public int Id_usuario_propietario { get; set; }
    public string Nombre_usuario_propietario{get;set;}
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public TableroViewModels()
    {
    }
}

