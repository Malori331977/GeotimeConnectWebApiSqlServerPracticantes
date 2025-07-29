namespace com.gsitcr.geotime.Models
{
    public class cOrganizacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } 
        public string NivelOrganizacionId { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public int OrganizacionSuperior { get; set; }
        public cOrganizacionNivel? cOrganizacionNivel { get; set; }
        public IEnumerable<cOrganizacionResponsable>? cOrganizacionResponsable { get; set; }
        //public IEnumerable<cDepartamento>? cDepartamento { get; set; }
    }
}
