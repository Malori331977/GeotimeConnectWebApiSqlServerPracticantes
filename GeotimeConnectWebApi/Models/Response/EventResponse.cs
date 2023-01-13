namespace GeoTimeConnectWebApi.Models.Response
{
    public class EventResponse
    {
        public string? Id { get; set; }
        public string? Respuesta { get; set; }
        public string? Descripcion { get; set; }

        public EventResponse()
        {
            Id = "0";
            Respuesta = "OK";
            Descripcion = "El proceso se ejecutó con exito.";
        }
    }
}
