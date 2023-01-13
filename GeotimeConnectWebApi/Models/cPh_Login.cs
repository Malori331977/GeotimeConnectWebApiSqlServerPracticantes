namespace GeoTimeConnectWebApi.Models
{
    public class cPh_Login
    {
        public int idusuario { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public int? privilegio { get; set; }
        public int? grupo_base { get; set; }
        public long? idsesion { get; set; }
        public DateTime? ultimo_login { get; set; }
        public string? fcomp { get; set; }
        public string? fplani { get; set; }
        public string? fperiodo { get; set; }
        public string? grupos { get; set; }
        public int? proceso { get; set; }
        public DateTime? ultimo_estado { get; set; }
        public string? companias { get; set; }
        public string? planillas { get; set; }
        public string? idioma { get; set; }
        public string? stat_msg { get; set; }
        public string? turnos { get; set; }
        public char? usa_wusuario { get; set; }
        public char estado { get; set; }
        public string? EMAIL { get; set; }
        public char? OMITE_LIC { get; set; }
        public char ACTIVO { get; set; }
        public string? GLOBAL_CLAVE { get; set; }
    }
}
