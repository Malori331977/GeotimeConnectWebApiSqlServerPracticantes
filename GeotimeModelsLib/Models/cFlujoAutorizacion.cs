namespace com.gsitcr.geotime.Models
{
    public class cFlujoAutorizacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }

        public IEnumerable<cFlujoAutorizacionDetalle>? cFlujoAutorizacionDetalle {  get; set; }
    }
}
