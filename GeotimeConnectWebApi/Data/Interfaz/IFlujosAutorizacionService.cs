using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IFlujosAutorizacionService
    {
        

        /// <summary>
        /// GetFlujoAutorizacion: obtener todos los registros de flujos de autorizacion
        /// </summary>
        /// <returns>Lista con todos los registros de flujos de autorizacion</returns>
        public Task<List<cFlujoAutorizacion>> GetFlujoAutorizacion();
        /// <summary>
        /// GetFlujoAutorizacion: Obtiene un registro de flujo de autorizacion 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de flujo de autorizacion  segun el id de parametro indicado</returns>
        public Task<cFlujoAutorizacion> GetFlujoAutorizacion(int id);
        /// <summary>
        /// PostFlujoAutorizacion:  Recibe una lista de registros de Flujo Autorizacion, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PostFlujoAutorizacion(IEnumerable<cFlujoAutorizacion> model);
        /// <summary>
        /// DeleteFlujoAutorizacion: Elimina un registro de flujo de autorización
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> DeleteFlujoAutorizacion(string id);
        public Task<List<cEstado>> GetEstado();
        public Task<cEstado> GetEstado(int id);
        public Task<EventResponse> PostEstado(cEstado estado);
        public Task<EventResponse> DeleteEstado(string estadoid);
        public Task<List<cTipoSolicitud>> GetTipoSolicitud();
        public Task<cTipoSolicitud> GetTipoSolicitud(int id);
        public Task<EventResponse> PostTipoSolicitud(cTipoSolicitud tipoSolicitud);
        public Task<EventResponse> DeleteTipoSolicitud(string Id);
    }
}
