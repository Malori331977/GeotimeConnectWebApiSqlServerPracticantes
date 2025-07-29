namespace com.gsitcr.geotime.Models
{
    public class cEmpleadoJefatura
    {
        public string IdNumero { get; set; }
        public string IdNumeroResponsable { get; set; }
        public string? IdNumeroSuplente { get; set; }
        public string IdUsuarioModifica { get; set; }
        public DateTime FechaUltModifica { get; set; }
    }
}
