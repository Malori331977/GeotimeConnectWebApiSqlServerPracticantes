using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IMarcasService
    {

        /// <summary>
        /// GetMarcaMovTurnoEstadoEnvio: obtener todos los registros con estado falso (no enviados) de MarcasMovTurnos
        /// </summary>
        /// <returns>Lista con todos los registros no eneviados de MarcasMovTurnos</returns>
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurnoEstadoEnvio();

        /// <summary>
        /// PostMarcaMovTurnoEstadoEnvio: Actualizar estado de envio para las marcas listadas
        /// </summary>
        /// <param name="listaMarcasMovTurno"></param>
        /// <returns>EventResponse: con estado de la actualizacion</returns>
        public Task<EventResponse> PostMarcaMovTurnoEstadoEnvio(List<cMarcaMovTurno> listaMarcasMovTurno);

        /// <summary>
        /// GetMarcaIncidenciaEstadoEnvio: obtener todos los registros con estado falso (no enviados) de Marcas Incidencias
        /// </summary>
        /// <returns>Lista con todos los registros no enviados de Marcas Incidencias</returns>
        public Task<List<cMarcaIncidencia>> GetMarcaIncidenciaEstadoEnvio();

        /// <summary>
        /// PostMarcaMovTurnoEstadoEnvio: Actualizar estado de envio para las marcas listadas
        /// </summary>
        /// <param name="listaMarcasMovTurno"></param>
        /// <returns>EventResponse: con estado de la actualizacion</returns>
        public Task<EventResponse> PostMarcaIncidenciaEstadoEnvio(List<cMarcaIncidencia> listaMarcasMovTurno);


    }
}
