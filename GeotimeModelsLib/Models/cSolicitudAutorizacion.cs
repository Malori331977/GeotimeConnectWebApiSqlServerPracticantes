namespace com.gsitcr.geotime.Models
{
    public class cSolicitudAutorizacion
    {
        public long SolicitudId { get; set; }
        public int EstadoId { get; set; }
        public string IdNumero { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Comentario { get; set; }

        public cSolicitud? cSolicitud { get; set; }
    }
}
