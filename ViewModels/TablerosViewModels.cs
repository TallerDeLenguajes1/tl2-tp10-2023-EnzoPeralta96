namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;

public class TableroViewModels
{
    public List<Tablero> MisTableros { get; set; }

    public List<Tablero> Tableros { get; set; }

    public TableroViewModels(List<Tablero> tablerosPropios, List<Tablero> tableros)
    {
        MisTableros = tablerosPropios;
        Tableros = tableros;
    }


}

