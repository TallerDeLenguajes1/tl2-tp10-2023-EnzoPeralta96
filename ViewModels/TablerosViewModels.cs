namespace ViewModels;
using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;

public class TableroViewModels
{
    public string MensajeExito { get; set; }
    public bool TieneMensajeExito => !string.IsNullOrEmpty(MensajeExito);
    public string MensajeError { get; set; }
    public bool TieneMensajeError => !string.IsNullOrEmpty(MensajeError);
    public List<Tablero> Tableros { get; set; }

    public TableroViewModels(List<Tablero> tableros)
    {
        Tableros = tableros;
    }


}

