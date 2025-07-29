namespace com.gsitcr.geotime.Models
{
    public class cEstado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public IEnumerable<cSolicitud>? cSolicitud { get; set; }
    }
}
