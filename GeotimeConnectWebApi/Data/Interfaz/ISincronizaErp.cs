using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface ISincronizaErp
    {
        public Task<EventResponse> SincronizaDepartamentos();
        public Task<EventResponse> SincronizaCentrosCosto();
        public Task<EventResponse> SincronizaPuestos();
        public Task<EventResponse> SincronizaEmpleados();
        public Task<EventResponse> SincronizaEmpleados(cSincronizo_erp param);
        public Task<EventResponse> SincronizaConceptos();
        public Task<EventResponse> SincronizaNominas();
        public Task<EventResponse> PostSincronizacion(string idPlanilla);
    }
}
