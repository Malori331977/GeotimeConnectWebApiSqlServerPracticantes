using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IReportesServices
    {
        /// <summary>
        /// GetHoraExtraXTurno:  Obtine listado de horas extras por turno para reporte
        /// </summary>
        /// <returns> listado de horas extras por turno</returns>
        public Task<IEnumerable<cVHoraExtraXTurno>> GetHoraExtraXTurno();

        /// <summary>
        /// GetHoraExtraXTurno:  Obtine listado de horas extras por turno para un periodo en especifico
        /// </summary>
        /// <param name="idPeriodo"> id del periodo para el cual se desea obtener las horas extras por turno</param>
        /// <param name="idturno"> id del turno para el cual se desea obtener las horas extras</param>
        /// <returns> listado de horas extras por turno del periodo</returns>
        public Task<IEnumerable<cVHoraExtraXTurno>> GetHoraExtraXTurno(string idPeriodo, int idturno);

        /// <summary>
        /// GetHoraLaboradaEmpleado:  Obtiene listado de horas laboradas por empleado para reporte
        /// </summary>
        /// <returns>Resumen de horas laboradas por empleado</returns>
        public Task<IEnumerable<cVHoraLaboradaEmpleado>> GetHoraLaboradaEmpleado();

        /// <summary>
        /// GetHoraLaboradaEmpleado:  Obtiene listado de horas laboradas por empleado por periodo, planilla o departamento
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idplanilla"></param>
        /// <param name="iddepartamento"></param>
        /// <returns>Resumen de horas laboradas por empleado</returns>
        public Task<IEnumerable<cVHoraLaboradaEmpleado>> GetHoraLaboradaEmpleado(string idPeriodo, string idplanilla, string iddepartamento);

        /// <summary>
        /// GetMarcaMovTurnoBitacora:  Obtiene listado de movimientos de la bitacora Marcas_Mov_Turnos por rango de fechas
        /// </summary>
        /// <param name="FechaInicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns>listado de movimientos de la bitacora Marcas_Mov_Turnos</returns>
        public Task<IEnumerable<cMarcaMovTurnoBitacora>> GetMarcaMovTurnoBitacora(string FechaInicio, string FechaFin, string idDepartamento);


        /// <summary>
        /// GetReporteHoraExtra:  Obtiene listado de horas extras por empleado para un rango de fechas
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public Task<EventResponse> GetReporteHoraExtra(cFiltroReporte filtro);

        /// <summary>
        /// GetReporteIncidencias:  Obtiene listado de marcas incidencias por empleado para un rango de fechas
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public Task<EventResponse> GetReporteIncidencias(cFiltroReporte filtro);

        /// <summary>
        /// GetReporteHistoricoMarca: Obtiene lsitado del registro de marcas para los colaboradores
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public Task<EventResponse> GetReporteHistoricoMarca(cFiltroReporte filtro);

        /// <summary>
        /// GetHistoricoCalculoTiempos: lista de calculos realizaods para los colaboradores en cada periodo de nomina
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public Task<EventResponse> GetHistoricoCalculoTiempos(cFiltroReporte filtro);





    }
}
