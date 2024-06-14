namespace GeoTimeConnectWebApi.Models
{
    public class cPaletaColor
    {
        public int COLORID { get; set; }
        public string? DESCRIPCION { get; set; }
        public string COLORFONDO { get; set; }
        public string COLORFUENTE { get; set; }

        public IEnumerable<cTurno>? Turno { get; set; }
    }
}
