namespace com.gsitcr.geotime.Models.Response
{
    public class EventResponseHID
    {
        public string? Id { get; set; }
        public string? Respuesta { get; set; }
        public string? Descripcion { get; set; }
        public byte[]? Template { get; set; }
        public EventResponseHID()
        {
            Id = "0";
            Respuesta = "OK";
            Descripcion = "El proceso se ejecutó con exito.";
            Template = null;
        }
    }
}
