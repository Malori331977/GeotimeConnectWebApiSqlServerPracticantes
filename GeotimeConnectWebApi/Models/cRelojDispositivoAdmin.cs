namespace GeoTimeConnectWebApi.Models
{
    public class cRelojDispositivoAdmin
    {
        public int CLOCK_ID { get; set; }
        public string? CLOCK_DESCRIPTION { get; set; }
        public string? CLOCK_IP { get; set; }
        public int? CLOCK_PORT { get; set; }
        public string? CLOCK_STATE { get; set; }
        public char? CLOCK_COMM { get; set; }
        public short? CLOCK_COMMID { get; set; }
        public int SITE_ID { get; set; }
        public string? CLOCK_TYPE { get; set; }
        public char? CLOCK_USE_FUNCTION { get; set; }
        public string? CLOCK_FUNCTION { get; set; }
        public string? CLOCK_PASS { get; set; }
        public char? BORRO_M { get; set; }
        public int? IDLOCACION { get; set; }
        public string? FG_MODEL { get; set; }
        public char USA_FACE { get; set; }
        public string? USUARIO_HIK { get; set; }
        public string? PASSWORD_HIK { get; set; }
        public string? CLOCK_SERIE { get; set; }
        public char? ALERTA_MASCARILLA { get; set; }
        public char? ALERTA_TEMPERATURA { get; set; }
        public string? CORTE_TEMPERATURA { get; set; }
        public string? DIRECCIONES_ALERTA { get; set; }
        public char? ACC { get; set; }
        public DateTime? ULTIMO_ESTADO { get; set; }

        public string? IDCOMP { get; set; }
    }
}
