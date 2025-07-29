namespace com.gsitcr.geotime.Models
{
    public class cOrganizacionNivel
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }

        public IEnumerable<cOrganizacion>? cOrganizacion { get; set; }
        public IEnumerable<cFlujoAutorizacionDetalle>? cFlujoAutorizacionDetalle { get; set; }
    }
}
