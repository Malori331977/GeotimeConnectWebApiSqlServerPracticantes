namespace GeoTimeConnectWebApi.Models
{
    public class cPh_Compania
    {
        public string IDCOMP { get; set; }
        public string COMPANIA { get; set; }
        public string? NOM_CONECTOR { get; set; }
        public string? STRING_SQL { get; set; }
        public string? STRING_SQL_ERP { get; set; }
        public string? PAIS { get; set; }
        public char? AUTO_PROCESO { get; set; }
        public string? REMOTE_ERPSERVICE { get; set; }
        public string? MAIL_SERVER { get; set; }
        public string? MAIL_USER { get; set; }
        public string? MAIL_PASSWORD { get; set; }
        public int? MAIL_PORT { get; set; }
        public char? MAIL_AUTH { get; set; }
        public char? MAIL_SSL { get; set; }
        public string? HORA_SUP { get; set; }
        public string? HORA_EMP { get; set; }
        public char? SUPERVISOR_ACUM { get; set; }
        public char? MAIL_TLS { get; set; }
        public string? HORA_CALC { get; set; }
        public char? IN_MARCAS { get; set; }
    }
}
