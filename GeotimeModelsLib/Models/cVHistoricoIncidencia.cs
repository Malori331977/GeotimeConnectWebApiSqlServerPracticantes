namespace com.gsitcr.geotime.Models
{
    public class cVHistoricoIncidencia
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        public DateTime Fecha { get; set; }
        public string? IdNumero { get; set; }
        public string? Nombre { get; set; }
        public string? HEntra { get; set; }
        public string? HSale { get; set; }
        public string? CodigoIncidencia { get; set; }
        public string? DescIncidencia { get; set; }
        public string? DescGrupo { get; set; }
        public string? DescDepartamento { get; set; }
        public string? DescPlanilla { get; set; }
    }
}
