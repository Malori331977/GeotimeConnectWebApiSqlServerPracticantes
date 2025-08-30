using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class ReportesServices : IReportesServices
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<ReportesServices> _logger;
        private string InterfaceName = "ReportesServices";

        public ReportesServices(IHttpContextAccessor httpContextAccessor, 
                                     ILogger<ReportesServices> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext!.User.Claims;
            string schema = "";
            string bdname = "";
            _logger = logger;

            foreach (Claim clm in claims)
            {
                if (clm.Type.Contains("claims/givenname"))
                {
                    schema = clm.Value;
                }

                if (clm.Type.Contains("claims/spn"))
                {
                    bdname = clm.Value;
                }

                if (schema != "" && schema is not null && bdname != "" && bdname is not null)
                    break;

            }

            if (schema == "")
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                schema = config.GetConnectionString("Schema");
                bdname = config.GetConnectionString("DBName");
            }
            _schema = schema;
            _dataBase = bdname;
            _context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);
        }


        /// <summary>
        /// GetHoraExtraXTurno:  Obtine listado de horas extras por turno para reporte
        /// </summary>
        /// <returns> listado de horas extras por turno</returns>
        public async Task<IEnumerable<cVHoraExtraXTurno>> GetHoraExtraXTurno()
        {
            List<cVHoraExtraXTurno> model = new();

            try
            {
                model = await _context.VHorasExtrasXTurno.ToListAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHoraExtraXTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                 throw;
            }
            return model;
        }


        /// <summary>
        /// GetHoraExtraXTurno:  Obtine listado de horas extras por turno para un periodo en especifico
        /// </summary>
        /// <param name="idPeriodo"> id del periodo para el cual se desea obtener las horas extras por turno</param>
        /// <param name="idturno"> id del turno para el cual se desea obtener las horas extras</param>
        /// <returns> listado de horas extras por turno del periodo</returns>
        public async Task<IEnumerable<cVHoraExtraXTurno>> GetHoraExtraXTurno(string idPeriodo, int idturno)
        {
            List<cVHoraExtraXTurno> model = new();

            try
            {
                model = await _context.VHorasExtrasXTurno.Where(e=>e.idperiodo==(idPeriodo=="-1"?e.idperiodo:idPeriodo) &&
                                                                   e.idturno == (idturno == -1 ? e.idturno : idturno))
                                                         .ToListAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHoraExtraXTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetHoraLaboradaEmpleado:  Obtiene listado de horas laboradas por empleado para reporte
        /// </summary>
        /// <returns>Resumen de horas laboradas por empleado</returns>
        public async Task<IEnumerable<cVHoraLaboradaEmpleado>> GetHoraLaboradaEmpleado()
        {
            List<cVHoraLaboradaEmpleado> model = new();
            try
            {
                model = await _context.VHorasLaboradasEmpleado.ToListAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHoraLaboradaEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }
        /// <summary>
        /// GetHoraLaboradaEmpleado:  Obtiene listado de horas laboradas por empleado por periodo, planilla o departamento
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idplanilla"></param>
        /// <param name="iddepartamento"></param>
        /// <returns>Resumen de horas laboradas por empleado</returns>
        public async Task<IEnumerable<cVHoraLaboradaEmpleado>> GetHoraLaboradaEmpleado(string idPeriodo, string idplanilla, string iddepartamento)
        {
            List<cVHoraLaboradaEmpleado> model = new();
            try
            {
                model = await _context.VHorasLaboradasEmpleado.Where(e => e.idperiodo == (idPeriodo == "-1" ? e.idperiodo : idPeriodo) 
                                                                       && e.idplanilla == (idplanilla == "-1" ? e.idplanilla : idplanilla)
                                                                       && e.IdDepartamento == (iddepartamento == "-1" ? e.IdDepartamento : iddepartamento)).ToListAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHoraLaboradaEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetMarcaMovTurnoBitacora:  Obtiene listado de movimientos de la bitacora Marcas_Mov_Turnos por rango de fechas
        /// </summary>
        /// <param name="FechaInicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns>listado de movimientos de la bitacora Marcas_Mov_Turnos</returns>
        public async Task<IEnumerable<cMarcaMovTurnoBitacora>> GetMarcaMovTurnoBitacora(string FechaInicio, string FechaFin)
        {
            List<cMarcaMovTurnoBitacora> model = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}T00:00:00");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}T23:59:59");

                model = await _context.Marcas_Mov_Turnos_Bitacora.Where(e => e.fecha_modificacion >= fechaMovInicio &&
                                            e.fecha_modificacion <= fechaMovFinal).ToListAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetMarcaMovTurnoBitacora: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }


    }


}
