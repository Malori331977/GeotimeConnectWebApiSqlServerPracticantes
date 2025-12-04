using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                                     ILogger<ReportesServices> logger,
                                     IGeoTimeConnectService geoService)
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

        #region Reportes Punta Leona
       
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
        public async Task<IEnumerable<cMarcaMovTurnoBitacora>> GetMarcaMovTurnoBitacora(string FechaInicio, string FechaFin, string idDepartamento)
        {
            List<cMarcaMovTurnoBitacora> model = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}T00:00:00");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}T23:59:59");

                model = await (from m in _context.Marcas_Mov_Turnos_Bitacora
                                    .Include(e => e.cEmpleado)
                                    .Include(t => t.cTurno)
                                    .Include(p => p.cPh_Planilla)
                               .Where(e => e.fecha_modificacion >= fechaMovInicio &&
                                            e.fecha_modificacion <= fechaMovFinal)
                               join e in _context.Empleados on m.idnumero equals e.IdNumero
                               join d in _context.Ph_Departamento on e.IdDepartamento equals d.IDDEPART
                               join pl in _context.PH_LOGIN on m.usuario equals pl.usuario
                               where e.IdDepartamento == (idDepartamento == "-1" ? e.IdDepartamento : idDepartamento)
                               select new cMarcaMovTurnoBitacora
                               {
                                   idbitacora = m.idbitacora,
                                   fecha_modificacion = m.fecha_modificacion,
                                   UsuarioModifica = m.UsuarioModifica,
                                   idregistro = m.idregistro,
                                   idplanilla = m.idplanilla,
                                   idnumero = m.idnumero,
                                   fecha = m.fecha,
                                   hora = m.hora,
                                   turno = m.turno,
                                   estado = m.estado,
                                   usuario = m.usuario,
                                   fecha_reg = m.fecha_reg,
                                   linea = m.linea,
                                   hentra2 = m.hentra2,
                                   estado_envio = m.estado_envio,
                                   Accion = m.Accion,
                                   NombreEmpleado = m.cEmpleado == null?"": m.cEmpleado.Nombre,
                                   DescipcionTurno = m.cTurno == null ? "" : m.cTurno.Descripcion,
                                   DescripcionPlanilla = m.cPh_Planilla == null ? "" : m.cPh_Planilla.planilla,
                                   DescripcionDepartamento = d.DESCRIPCION,
                                   DescipcionAccion = m.Accion == 'I' ? "Registro Inicial" :
                                                       m.Accion == 'M' ? "Modificación" :
                                                       m.Accion == 'D' ? "Eliminación" : "Desconocida",
                                   cEmpleado = m.cEmpleado==null?null: 
                                            new cEmpleado
                                            {
                                                IdNumero = m.cEmpleado.IdNumero,
                                                Nombre = m.cEmpleado.Nombre,
                                                IdDepartamento = m.cEmpleado.IdDepartamento,
                                                IdCCosto = m.cEmpleado.IdCCosto,
                                                IdPlanilla = m.cEmpleado.IdPlanilla,
                                                Estado = m.cEmpleado.Estado
                                            },
                                   cTurno = m.cTurno == null ? null :
                                            new cTurno
                                            {
                                                IdTurno = m.cTurno.IdTurno,
                                                Descripcion = m.cTurno.Descripcion,
                                                HEntra = m.cTurno.HEntra,
                                                HSale = m.cTurno.HSale,
                                                min_con_6 = m.cTurno.min_con_6,
                                                fuerza_calc = m.cTurno.fuerza_calc,
                                                idagrupamiento = m.cTurno.idagrupamiento,
                                            },
                                   cPh_Planilla = m.cPh_Planilla==null? null :
                                            new cPh_Planilla
                                            {
                                                idplanilla = m.cPh_Planilla.idplanilla,
                                                planilla = m.cPh_Planilla.planilla,
                                            },
                                   NombreUsuarioModifica = pl == null ? "" : pl.descripcion

                               }).ToListAsync();



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

        #endregion


        #region Reportes Geotime

        public async Task<EventResponse> GetReporteHoraExtra(cFiltroReporte filtro)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var model = await (from me in _context.Marcas_Extras_Apb
                                   join emp in _context.Empleados.Include(e=>e.Ph_Grupo).Include(e => e.Ph_Planilla).Include(e => e.Departamento) on me.idnumero equals emp.IdNumero
                                   where me.estado == 'A' && me.fecha >= filtro.inicio && me.fecha <= filtro.fin
                                      && (filtro.Departamentos==null?true:filtro.Departamentos!.Contains(emp.IdDepartamento))
                                      && (filtro.Planillas == null ? true : filtro.Planillas!.Contains(emp.IdPlanilla))
                                      && (filtro.Empleados == null ? true : filtro.Empleados!.Contains(emp.IdNumero))
                                      && (me.ccosto==null || (filtro.CentrosCosto == null ? true : filtro.CentrosCosto!.Contains(me.ccosto)))
                                   select new cVHistoricoExtra
                                   {
                                       Inicio = filtro.inicio,
                                       Fin = filtro.fin,
                                       IdNumero = me.idnumero,
                                       Nombre = emp.Nombre,
                                       Fecha = me.fecha,
                                       Cantidad = me.cantidad,
                                       CantidadAprobada = me.aprob_nivel1=='T'?me.cantidad_aprob_nivel1:"",
                                       CCosto = me.ccosto,
                                       Comentarios = $"{me.comentario}",
                                       DescGrupo = emp.Ph_Grupo == null ? "" : emp.Ph_Grupo.descripcion,
                                       DescDepartamento = emp.Departamento == null ? "" : emp.Departamento.DESCRIPCION,
                                       DescPlanilla = emp.Ph_Planilla == null ? "" : emp.Ph_Planilla.planilla,

                                   }).ToListAsync();
                respuesta.Data = System.Text.Json.JsonSerializer.Serialize<IEnumerable<cVHistoricoExtra>>(model);

            }
            catch (Exception e)
            {
                respuesta.Respuesta = "Error";
                
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetReporteHoraExtra: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                respuesta.Descripcion = error;
               
                _logger.LogError(error);
                throw;
            }
            return respuesta;
        }

        public async Task<EventResponse> GetReporteIncidencias(cFiltroReporte filtro)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var model = await (from me in _context.Marcas_Incidencias.Include(m => m.cIncidencia)
                                   join emp in _context.Empleados.Include(e => e.Ph_Grupo).Include(e => e.Ph_Planilla).Include(e => e.Departamento) on me.IDNUMERO equals emp.IdNumero
                                   where me.FECHA >= filtro.inicio && me.FECHA <= filtro.fin
                                      && (filtro.Departamentos == null ? true : filtro.Departamentos!.Contains(emp.IdDepartamento))
                                      && (filtro.Planillas == null ? true : filtro.Planillas!.Contains(emp.IdPlanilla))
                                      && (filtro.Empleados == null ? true : filtro.Empleados!.Contains(emp.IdNumero))
                                      && (filtro.Incidencias == null ? true : filtro.Incidencias!.Contains(me.IDINCIDENCIA))
                                   select new cVHistoricoIncidencia
                                   {
                                       Inicio = filtro.inicio,
                                       Fin = filtro.fin,
                                       IdNumero = me.IDNUMERO,
                                       Nombre = emp.Nombre,
                                       Fecha = me.FECHA,
                                       HEntra = me.HENTRA,
                                       HSale = me.HSALE,
                                       CodigoIncidencia = me.cIncidencia == null ? "" : me.cIncidencia.Codigo,
                                       DescIncidencia = me.cIncidencia == null ? "" : me.cIncidencia.Descripcion,
                                       DescGrupo = emp.Ph_Grupo == null ? "" : emp.Ph_Grupo.descripcion,
                                       DescDepartamento = emp.Departamento == null ? "" : emp.Departamento.DESCRIPCION,
                                       DescPlanilla = emp.Ph_Planilla == null ? "" : emp.Ph_Planilla.planilla,

                                   }).ToListAsync();
                respuesta.Data = System.Text.Json.JsonSerializer.Serialize<IEnumerable<cVHistoricoIncidencia>>(model);

            }
            catch (Exception e)
            {
                respuesta.Respuesta = "Error";

                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetReporteIncidencias: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                respuesta.Descripcion = error;

                _logger.LogError(error);
                throw;
            }
            return respuesta;
        }

        public async Task<EventResponse> GetReporteHistoricoMarca(cFiltroReporte filtro)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var model = await (from mc in _context.VMarcas_Comedor
                                   join emp in _context.Empleados.Include(e => e.Ph_Grupo).Include(e => e.Ph_Planilla).Include(e => e.Departamento) on mc.idnumero equals emp.IdNumero
                                   where mc.fecha_entra >= filtro.inicio && mc.fecha_entra <= filtro.fin
                                      && (filtro.Departamentos == null ? true : filtro.Departamentos!.Contains(emp.IdDepartamento))
                                      && (filtro.Planillas == null ? true : filtro.Planillas!.Contains(emp.IdPlanilla))
                                      && (filtro.Empleados == null ? true : filtro.Empleados!.Contains(emp.IdNumero))
                                   select new cVHistoricoMarca
                                   {
                                       Inicio = filtro.inicio,
                                       Fin = filtro.fin,
                                       IdNumero = mc.idnumero,
                                       Nombre = emp.Nombre,
                                       fecha_entra = mc.fecha_entra,
                                       fecha_sale = mc.fecha_sale,
                                       hora_entra = mc.hora_entra,
                                       hora_sale = mc.hora_sale,
                                       desc1_ini = mc.desc1_ini,
                                       desc1_fin = mc.desc1_fin,
                                       desc2_ini = mc.desc2_ini,
                                       desc2_fin = mc.desc2_fin,
                                       desc3_ini = mc.desc3_ini,       
                                       desc3_fin = mc.desc3_fin,
                                       idterminal = mc.idterminal,
                                       DescGrupo = emp.Ph_Grupo == null ? "" : emp.Ph_Grupo.descripcion,
                                       DescDepartamento = emp.Departamento == null ? "" : emp.Departamento.DESCRIPCION,
                                       DescPlanilla = emp.Ph_Planilla == null ? "" : emp.Ph_Planilla.planilla,
                                       descanso1 = !mc.desc1_ini.Equals("00:00") || !mc.desc1_fin.Equals("00:00")? $"{mc.desc1_ini}-{mc.desc1_fin}":"",
                                       descanso2 = !mc.desc2_ini.Equals("00:00") || !mc.desc2_fin.Equals("00:00") ? $"{mc.desc2_ini}-{mc.desc2_fin}":"",
                                       descanso3 = !mc.desc3_ini.Equals("00:00") || !mc.desc3_fin.Equals("00:00") ? $"{mc.desc3_ini}-{mc.desc3_fin}":"",

                                   }).ToListAsync();
                respuesta.Data = System.Text.Json.JsonSerializer.Serialize<IEnumerable<cVHistoricoMarca>>(model);

            }
            catch (Exception e)
            {
                respuesta.Respuesta = "Error";

                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetReporteHistoricoMarca: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                respuesta.Descripcion = error;

                _logger.LogError(error);
                throw;
            }
            return respuesta;
        }

        public async Task<EventResponse> GetHistoricoCalculoTiempos(cFiltroReporte filtro)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var model = await (from mc in _context.Marcas_Reportes
                                   join emp in _context.Empleados.Include(e => e.Ph_Grupo).Include(e => e.Ph_Planilla).Include(e => e.Departamento) on mc.idnumero equals emp.IdNumero
                                   where mc.fecha >= filtro.inicio && mc.fecha <= filtro.fin
                                      && (filtro.Departamentos == null ? true : filtro.Departamentos!.Contains(emp.IdDepartamento))
                                      && (filtro.Planillas == null ? true : filtro.Planillas!.Contains(mc.idplanilla))
                                      && (filtro.Empleados == null ? true : filtro.Empleados!.Contains(emp.IdNumero))
                                   select new cVHistoricoCalculoTiempo
                                   {
                                       Inicio = filtro.inicio,
                                       Fin = filtro.fin,
                                       IdNumero = mc.idnumero,
                                       Nombre = emp.Nombre,                                       
                                       DescGrupo = emp.Ph_Grupo == null ? "" : emp.Ph_Grupo.descripcion,
                                       DescDepartamento = emp.Departamento == null ? "" : emp.Departamento.DESCRIPCION,
                                       DescPlanilla = emp.Ph_Planilla == null ? "" : emp.Ph_Planilla.planilla,
                                       fecha = mc.fecha,
                                       T1 = mc.T1,
                                       T2 = mc.T2,
                                       T3 = mc.T3,
                                       T4 = mc.T4,
                                       T5 = mc.T5,
                                       T6 = mc.T6,
                                       T7 = mc.T7,
                                       T8 = mc.T8,
                                       T9 = mc.T9,
                                       T10 = mc.T10,
                                       T11 = mc.T11,
                                       T12 = mc.T12,
                                       T13 = mc.T13,
                                       estado = mc.estado,
                                       periodo = mc.periodo,                                      
                                   }).ToListAsync();
                respuesta.Data = System.Text.Json.JsonSerializer.Serialize<IEnumerable<cVHistoricoCalculoTiempo>>(model);

            }
            catch (Exception e)
            {
                respuesta.Respuesta = "Error";

                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHistoricoCalculoTiempos: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                respuesta.Descripcion = error;

                _logger.LogError(error);
                throw;
            }
            return respuesta;
        }

        public async Task<EventResponse> GetHistoricoConcepto(cFiltroReporte filtro)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var periodos = from a in _context.Ph_Periodos
                                 join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla                                 
                                 where filtro.inicio >= a.inicio && filtro.inicio <= a.fin
                                    && (filtro.Planillas == null ? true : filtro.Planillas!.Contains(b.idplanilla))
                                 select new
                                 {
                                     IdPeriodo=a.idperiodo,
                                     TipoPlanilla = a.tipo_planilla,
                                     IdPlanilla = b.idplanilla,
                                     Planilla = b.planilla,
                                 };

                var model = await (from mr in _context.Marcas_Resumen.Include(e=>e.cConcepto)
                                   join emp in _context.Empleados.Include(e => e.Ph_Grupo).Include(e => e.Ph_Planilla).Include(e => e.Departamento) on mr.IdNumero equals emp.IdNumero
                                   join p in periodos on new { IdPeriodo = mr.IdPeriodo, IdPlanilla = mr.IdPlanilla } equals new { IdPeriodo = p.IdPeriodo, IdPlanilla = p.IdPlanilla } 
                                   where (filtro.Departamentos == null ? true : filtro.Departamentos!.Contains(emp.IdDepartamento))
                                      && (filtro.Empleados == null ? true : filtro.Empleados!.Contains(emp.IdNumero))
                                      && mr.Cantidad > 0
                                   select new cVHistoricoConcepto
                                   {
                                       Inicio = filtro.inicio,
                                       Fin = filtro.fin,
                                       IdNumero = mr.IdNumero,
                                       Nombre = emp.Nombre,
                                       DescGrupo = emp.Ph_Grupo == null ? "" : emp.Ph_Grupo.descripcion,
                                       DescDepartamento = emp.Departamento == null ? "" : emp.Departamento.DESCRIPCION,
                                       DescPlanilla = emp.Ph_Planilla == null ? "" : emp.Ph_Planilla.planilla,
                                       IdConcepto = mr.IdConcepto,
                                       Codigo = mr.cConcepto == null ? "" : mr.cConcepto.Concepto,
                                       DescConcepto = mr.cConcepto == null ? "" : mr.cConcepto.Descripcion,
                                       Cantidad = mr.Cantidad,
                                       CCosto = mr.IdCCosto,
                                       Proyecto = mr.Proyecto,
                                       Fase = mr.Fase,
                                   }).ToListAsync();
                respuesta.Data = System.Text.Json.JsonSerializer.Serialize<IEnumerable<cVHistoricoConcepto>>(model);

            }
            catch (Exception e)
            {
                respuesta.Respuesta = "Error";

                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHistoricoConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                respuesta.Descripcion = error;

                _logger.LogError(error);
                throw;
            }
            return respuesta;
        }

        public async Task<EventResponse> GetHistoricoConceptoResumen(cFiltroReporte filtro)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var periodos = from a in _context.Ph_Periodos
                                      join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla
                                      where filtro.inicio >= a.inicio && filtro.inicio <= a.fin
                                         && (filtro.Planillas == null ? true : filtro.Planillas!.Contains(b.idplanilla))
                                      select new
                                      {
                                          IdPeriodo = a.idperiodo,
                                          TipoPlanilla = a.tipo_planilla,
                                          IdPlanilla = b.idplanilla,
                                          Planilla = b.planilla,
                                      };


                var model = await (from mr in _context.Marcas_Resumen.Include(e => e.cConcepto)
                                   join emp in _context.Empleados on mr.IdNumero equals emp.IdNumero
                                   join p in periodos on new { IdPeriodo = mr.IdPeriodo, IdPlanilla = mr.IdPlanilla } equals new { IdPeriodo = p.IdPeriodo, IdPlanilla = p.IdPlanilla }
                                   where (filtro.Departamentos == null ? true : filtro.Departamentos!.Contains(emp.IdDepartamento))
                                      && (filtro.Empleados == null ? true : filtro.Empleados!.Contains(emp.IdNumero))
                                      && mr.Cantidad>0
                                   group mr by new { mr.IdConcepto, mr.cConcepto!.Concepto, mr.cConcepto.Descripcion } into g
                                   select new cVHistoricoConceptoResumen
                                   {
                                       IdConcepto = g.Key.IdConcepto,
                                       Codigo = g.Key.Concepto,
                                       DescConcepto = g.Key.Descripcion,
                                       Cantidad = g.Sum(x => x.Cantidad),
                                   }).ToListAsync();

                respuesta.Data = System.Text.Json.JsonSerializer.Serialize<IEnumerable<cVHistoricoConceptoResumen>>(model);

            }
            catch (Exception e)
            {
                respuesta.Respuesta = "Error";

                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                error = $"{InterfaceName}.GetHistoricoConceptoResumen: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                respuesta.Descripcion = error;

                _logger.LogError(error);
                throw;
            }
            return respuesta;
        }

        #endregion


    }


}
