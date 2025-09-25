namespace com.gsitcr.geotime.Models.Response
{
    public class EventResponse
    {
        public string? Id { get; set; }
        public string? Respuesta { get; set; }
        public string? Descripcion { get; set; }
        public string? ValorRetorno { get; set; }
        public string? Data { get; set; } = null;

        public EventResponse()
        {
            Id = "0";
            Respuesta = "OK";
            Descripcion = "El proceso se ejecutó con exito.";
            ValorRetorno = "";
        }
    }
}
