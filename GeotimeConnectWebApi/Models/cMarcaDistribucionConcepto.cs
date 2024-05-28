namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaDistribucionConcepto
    {
        public long IDREGISTRO { get; set; }
        public string IDPLANILLA { get; set; }
        public string IDNUMERO { get; set; }
        public DateTime FECHA { get; set; }
        public string IDCCOSTO { get; set; }
        public string? PROYECTO { get; set; }
        public string? FASE { get; set; }
        public decimal? CANTIDAD { get; set; }
        public string? INICIO { get; set; }
        public string? FIN { get; set; }
        public char ESTADO { get; set; }
        public int IDDIST { get; set; }
        public DateTime? FECHA_DIST { get; set; }
        public string? LON_REG { get; set; }
        public string? LAT_REG { get; set; }
        public string? COMENTARIO { get; set; }
    }
}
