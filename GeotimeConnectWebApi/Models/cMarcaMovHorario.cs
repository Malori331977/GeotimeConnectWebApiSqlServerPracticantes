namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaMovHorario
    {
        public long IDREGISTRO { get; set; }
        public string IDPLANILLA { get; set; }
        public string IDNUMERO { get; set; }
        public DateTime FECHA { get; set; }
        public string HORA { get; set; }
        public int IDHORARIO { get; set; }
        public char ESTADO { get; set; }
        public string? USUARIO { get; set; }
        public DateTime? FECHA_REG { get; set; }
    }
}
