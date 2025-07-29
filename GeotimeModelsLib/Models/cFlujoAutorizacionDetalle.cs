namespace com.gsitcr.geotime.Models
{
    public class cFlujoAutorizacionDetalle
    {
        public int FlujoAutorizacionId { get; set; }
        public string NivelOrganizacionId { get; set; }
        public short OrdenPrioridad { get; set; }
        public int EstadoAnteriorId { get; set; }
        public int EstadoNuevoId { get; set; }
        public bool EnvioSolicitud { get; set; }
        public int? OrganizacionEspecificaId { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }

        public cFlujoAutorizacion? cFlujoAutorizacion { get; set; }
        public cOrganizacionNivel? cOrganizacionNivel { get; set; }


    }
}
