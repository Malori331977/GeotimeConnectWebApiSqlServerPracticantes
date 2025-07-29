namespace com.gsitcr.geotime.Models
{
    public class cOrganizacionResponsable
    {
        public int OrganizacionId { get; set; }
        public int OrdenJerarquia { get; set; }
        public string IdNumeroResponsable { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }

        public cOrganizacion? cOrganizacion { get; set; }
    }
}
