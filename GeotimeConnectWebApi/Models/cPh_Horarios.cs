namespace GeoTimeConnectWebApi.Models
{
    public class cPh_Horarios
    {
        public int IDHORARIO { get; set; }
        public string DESCRIPCION { get; set; }
        public int? ColorId { get; set; }
        public cPaletaColor? PaletaColor { get; set; }

        public IEnumerable<cPh_HorarioTurno>? Ph_HorarioTurno { get; set; }

    }
}
