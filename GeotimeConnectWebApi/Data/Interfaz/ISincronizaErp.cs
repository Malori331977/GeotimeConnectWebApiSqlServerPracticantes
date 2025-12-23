using com.gsitcr.geotime.Models.Response;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface ISincronizaErp
    {
        public Task<EventResponse> SincronizaDepartamentos();
        public Task<EventResponse> SincronizaCentrosCosto();
        public Task<EventResponse> SincronizaPuestos();
        public Task<EventResponse> SincronizaEmpleados();
    }
}
