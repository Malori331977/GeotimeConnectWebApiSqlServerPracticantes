namespace com.gsitcr.geotime.Models
{
    public class cTipoSolicitud
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public int FlujoAutorizacionId { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }
        public bool Activa { get; set; }
        public string? TipoConfiguracion { get; set; }
        public IEnumerable<cSolicitud>? cSolicitud { get; set; }
    }
}
