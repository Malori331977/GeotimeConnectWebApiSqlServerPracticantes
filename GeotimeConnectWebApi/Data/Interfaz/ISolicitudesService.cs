using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;


namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface ISolicitudesService
    {

        public Task<List<cSolicitudConfiguracion>> GetSolicitudConfiguracion();
        public Task<cSolicitudConfiguracion> GetSolicitudConfiguracion(string id);
        public Task<EventResponse> PostSolicitudConfiguracion(cSolicitudConfiguracion tipoSolicitud);
        public Task<EventResponse> DeleteSolicitudConfiguracion(string Id);

        public Task<List<cTipoSolicitud>> GetTipoSolicitud();
        public Task<cTipoSolicitud> GetTipoSolicitud(int id);
        public Task<EventResponse> PostTipoSolicitud(cTipoSolicitud tipoSolicitud);
        public Task<EventResponse> DeleteTipoSolicitud(string Id);

        /// <summary>
        /// GetSolicitudes: obtener todos los registros de Solicitudes
        /// </summary>
        /// <returns>Lista con todos los registros de Solicitudes</returns>
        public Task<List<cSolicitud>> GetSolicitud();
        /// <summary>
        /// GetSolicitudes: Obtiene un registro de una solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de solicitud segun el id de parametro indicado</returns>
        public Task<cSolicitud> GetSolicitud(int id);

        //Creado por: Marlon Loria Solano
        //Fecha: 2025-07-25  
        //obtener lista Solicitudes de Accion de Personal por Usuario autorizador Y FLUJO DE AUTORIZACION
        public Task<IEnumerable<cSolicitud>> GetSolicitudPorAprobarFlujoAut(string idnumero);

        /// <summary>
        /// GetSolicitudAprobadaUsuario: lista de solicitudes aprobadas por usuario
        /// </summary>
        /// <param name="idnumero"></param>
        /// <returns></returns>
        public Task<IEnumerable<cSolicitud>> GetSolicitudAprobadaUsuario(string idnumero);

        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes para un colaborador según el id de número, fecha inicial, fecha final y tipo de solicitud.
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public Task<IEnumerable<cSolicitud>> GetSolicitud(string idnumero, string fechaInicial, string fechaFinal, int tipoSolicitud);

        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes para un colaborador según el estado, tipo de solicitud, fecha inicial, fecha final 
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public Task<IEnumerable<cSolicitud>> GetSolicitud(int estado, int tipoSolicitud, string fechaInicial, string fechaFinal);

        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes para un colaborador según el id de número, fecha inicial, fecha final y tipo de solicitud.
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public Task<IEnumerable<cSolicitud>> GetSolicitudTercero(string idnumero, string fechaInicial, string fechaFinal, int tipoSolicitud);

        /// <summary>
        /// PostSolicitudes:  Recibe una lista de registros de solicitudes, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PostSolicitud(IEnumerable<cSolicitud> model);

        /// <summary>
        /// PutSolicitudes:  Recibe una lista de registros de solicitudes y actualiza el registro
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PutSolicitud(IEnumerable<cSolicitud> model);

        public Task<EventResponse> PutSolicitudActGrupo(IEnumerable<cSolicitud> model);

        /// <summary>
        /// AnularSolicitud: anula un registro de solicitud según el id de parámetro indicado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> AnularSolicitud(string id);

        /// <summary>
        /// AutorizaSolicitud: Realiza proceso de autorización de solicitudes. Recibe una lista de solicitudes, se verifica si existe y actualiza el estado de aprobacion correspondiente
        /// </summary>
        /// <param name="solicitudesPorAprobar">Lista de solicitudes a aprobar</param>
        /// <returns>Instancia EventResponse con el resultado de la operación.</returns>
        public Task<EventResponse> AutorizaSolicitud(IEnumerable<cSolicitud> solicitudesPorAprobar);

        /// <summary>
        /// AutorizantesSolicitud: obtener lista de autorizantes de la solicitud
        /// </summary>
        /// <param name="IdGrupo"></param>
        /// <param name="TipoSolicitudId"></param>
        /// <param name="IdNumero"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<IEnumerable<cAutorizante>> GetAutorizantesSolicitud(int IdGrupo, int TipoSolicitudId, string IdNumero, long Id);

        public Task<EventResponse> ReAplicarSolicitudesAprobadas(string fechas);
        public Task<EventResponse> ReAplicarSolicitudesAprobadasById(int Id);


    }
}
