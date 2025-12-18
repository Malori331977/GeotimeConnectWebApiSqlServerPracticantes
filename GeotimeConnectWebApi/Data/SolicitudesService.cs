using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Utility = com.gsitcr.geotime.Models.Utils.Utility;

namespace com.gsitcr.geotime.Data
{
    public class SolicitudesService : ISolicitudesService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<SolicitudesService> _logger;
        private readonly IFlujosAutorizacionService _flujosServices;
        private readonly IOrganizacionService _organizacionService;
        private readonly IGeoTimeConnectService _geoServices;

        public SolicitudesService(SqlServerDataBaseContext context, IHttpContextAccessor httpContextAccessor, ILogger<SolicitudesService> logger,
                IFlujosAutorizacionService flujosServices, IOrganizacionService organizacionService, IGeoTimeConnectService geoServices)
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
            _flujosServices = flujosServices;
            _organizacionService = organizacionService;
            _geoServices = geoServices;
        }

        public async Task<List<cSolicitudConfiguracion>> GetSolicitudConfiguracion()
        {
            List<cSolicitudConfiguracion> model = new();
            try
            {
                model = await _context.SolicitudConfiguracion.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return model;
        }
        public async Task<cSolicitudConfiguracion> GetSolicitudConfiguracion(string id)
        {
            cSolicitudConfiguracion? model = new();
            try
            {
                model = await _context.SolicitudConfiguracion.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return model!;
        }

        public async Task<EventResponse> PostSolicitudConfiguracion(cSolicitudConfiguracion solicitudConfiguracion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cSolicitudConfiguracion? modelBuscar = await _context.SolicitudConfiguracion
                                            .Where(e => e.Id == solicitudConfiguracion.Id)
                                            .FirstOrDefaultAsync();

                if (modelBuscar is not null)
                {
                    modelBuscar.Descripcion = solicitudConfiguracion.Descripcion;
                    modelBuscar.FechaInicio = solicitudConfiguracion.FechaInicio;
                    modelBuscar.FechaFin = solicitudConfiguracion.FechaFin;
                    modelBuscar.HoraInicio = solicitudConfiguracion.HoraInicio;
                    modelBuscar.HoraFin = solicitudConfiguracion.HoraFin;
                    modelBuscar.CantidadHoras = solicitudConfiguracion.CantidadHoras;
                    modelBuscar.CantidadNumerico = solicitudConfiguracion.CantidadNumerico;
                    modelBuscar.CentroCosto = solicitudConfiguracion.CentroCosto;
                    modelBuscar.MultipleCentroCosto = solicitudConfiguracion.MultipleCentroCosto;
                    modelBuscar.FiltrarPuesto = solicitudConfiguracion.FiltrarPuesto;
                    modelBuscar.PuestosHabilitados = solicitudConfiguracion.PuestosHabilitados;
                    modelBuscar.Concepto = solicitudConfiguracion.Concepto;
                    modelBuscar.Destino = solicitudConfiguracion.Destino;
                    modelBuscar.MostrarCantidadHoras = solicitudConfiguracion.MostrarCantidadHoras;
                    modelBuscar.MostrarCantidadNum = solicitudConfiguracion.MostrarCantidadNum;
                    modelBuscar.CalcularHoras = solicitudConfiguracion.CalcularHoras;
                    modelBuscar.CalcularHorasNum = solicitudConfiguracion.CalcularHorasNum;
                    modelBuscar.CalcularDias = solicitudConfiguracion.CalcularDias;
                    modelBuscar.MaxCantidadDiasPasados = solicitudConfiguracion.MaxCantidadDiasPasados;
                    modelBuscar.MaxCantidadDiasFuturos = solicitudConfiguracion.MaxCantidadDiasFuturos;
                    modelBuscar.FiltrarNomina = solicitudConfiguracion.FiltrarNomina;
                    modelBuscar.NominasHabilitadas = solicitudConfiguracion.NominasHabilitadas;
                    modelBuscar.AdmiteDuplicados = solicitudConfiguracion.AdmiteDuplicados;

                    _context.SolicitudConfiguracion.Update(modelBuscar);
                }
                else
                {
                    _context.Add(solicitudConfiguracion);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Configuración del tipo de solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Configuración del tipo de solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        public async Task<EventResponse> DeleteSolicitudConfiguracion(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cSolicitudConfiguracion? modelBorrar = await _context.SolicitudConfiguracion
                                            .Where(e => e.Id == id)
                                            .FirstOrDefaultAsync();

                if (modelBorrar is not null)
                {
                    _context.SolicitudConfiguracion.Remove(modelBorrar);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Configuración del tipo de solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Configuración del tipo de solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2025-07-24
        //Obtener lista de tipos de Solicitudes
        public async Task<List<cTipoSolicitud>> GetTipoSolicitud()
        {
            List<cTipoSolicitud> tipoSolicitud = new();
            try
            {
                tipoSolicitud = await _context.TiposSolicitudes.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return tipoSolicitud;
        }
        public async Task<cTipoSolicitud> GetTipoSolicitud(int id)
        {
            cTipoSolicitud? tipoSolicitud = new();
            try
            {
                tipoSolicitud = await _context.TiposSolicitudes.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return tipoSolicitud!;
        }

        public async Task<EventResponse> PostTipoSolicitud(cTipoSolicitud tipoSolicitud)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cTipoSolicitud? modelBuscar = await _context.TiposSolicitudes
                                            .Where(e => e.Id == tipoSolicitud.Id)
                                            .FirstOrDefaultAsync();

                if (modelBuscar is not null)
                {
                    modelBuscar.Descripcion = tipoSolicitud.Descripcion;
                    modelBuscar.FechaModifica = DateTime.Now;
                    modelBuscar.IdUsuarioModifica = tipoSolicitud.IdUsuarioModifica;
                    modelBuscar.FlujoAutorizacionId = tipoSolicitud.FlujoAutorizacionId;
                    modelBuscar.Activa = tipoSolicitud.Activa;
                    modelBuscar.TipoConfiguracion = tipoSolicitud.TipoConfiguracion;
                    _context.TiposSolicitudes.Update(modelBuscar);
                }
                else
                {
                    _context.Add(tipoSolicitud);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización del TipoSolicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización del TipoSolicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-12-23
        //Eliminar  TipoSolicitud
        //Parametro: Recibe un registro de Tipo de Solicitud, se verifica si existen en cuyo caso
        //se elimina.
        public async Task<EventResponse> DeleteTipoSolicitud(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cTipoSolicitud? modelBorrar = await _context.TiposSolicitudes
                                            .Where(e => e.Id == int.Parse(id))
                                            .FirstOrDefaultAsync();

                if (modelBorrar is not null)
                {
                    _context.TiposSolicitudes.Remove(modelBorrar);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Tipo de Solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Tipo de Solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        /// <summary>
        /// GetSolicitudes: obtener todos los registros de Solicitudes
        /// </summary>
        /// <returns>Lista con todos los registros de Solicitudes</returns>
        public async Task<List<cSolicitud>> GetSolicitud()
        {
            List<cSolicitud> model = new();
            try
            {
                model = (from e in await _context.Solicitudes
                             .Include(e => e.cSolicitudAutorizacion)
                             .Include(e => e.cEstado)
                             .Include(e => e.cTipoSolicitud)
                             .Include(e => e.cCentroCosto)
                             .Include(e => e.cSolicitudDetalle)
                              .ToListAsync()
                         select new cSolicitud
                         {
                             Id = e.Id,
                             IdPlanilla = e.IdPlanilla,
                             IdNumero = e.IdNumero,
                             FechaInicio = e.FechaInicio,
                             FechaFin = e.FechaFin,
                             TipoSolicitudId = e.TipoSolicitudId,
                             IdDepart = e.IdDepart,
                             IdGrupo = e.IdGrupo,
                             IdCCosto = e.IdCCosto,
                             proyecto = e.proyecto,
                             fase = e.fase,
                             HoraInicio = e.HoraInicio,
                             HoraFin = e.HoraFin,
                             TotalHoras = e.TotalHoras,
                             Cantidad = e.Cantidad,
                             Comentario = e.Comentario,
                             EstadoId = e.EstadoId,
                             IdUsuarioRegistra = e.IdUsuarioRegistra,
                             FechaRegistro = e.FechaRegistro,
                             FechaModifica = e.FechaModifica,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                (from sa in e.cSolicitudAutorizacion
                                 select new cSolicitudAutorizacion
                                 {
                                     SolicitudId = sa.SolicitudId,
                                     EstadoId = sa.EstadoId,
                                     IdNumero = sa.IdNumero,
                                     FechaRegistro = sa.FechaRegistro,
                                     Comentario = sa.Comentario,
                                 }).ToList(),
                             cEstado = e.cEstado == null ? null :
                                new cEstado
                                {
                                    Id = e.cEstado.Id,
                                    Descripcion = e.cEstado.Descripcion,
                                    FechaRegistro = e.cEstado.FechaRegistro
                                },
                             cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                new cTipoSolicitud
                                {
                                    Id = e.cTipoSolicitud.Id,
                                    Descripcion = e.cTipoSolicitud.Descripcion,
                                    FechaModifica = e.cTipoSolicitud.FechaModifica,
                                    FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                    IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                    Activa = e.cTipoSolicitud.Activa,
                                    TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                },
                             cCentroCosto = e.cCentroCosto == null ? null :
                                new cCentroCosto
                                {
                                    IdCCosto = e.cCentroCosto.IdCCosto,
                                    Descripcion = e.cCentroCosto.Descripcion,
                                    Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                    Distribuye = e.cCentroCosto.Distribuye,

                                },
                             cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                (from sd in e.cSolicitudDetalle
                                 select new cSolicitudDetalle
                                 {
                                     IdRegistro = sd.IdRegistro,
                                     SolicitudId = sd.SolicitudId,
                                     Fecha = sd.Fecha,
                                     HoraInicio = sd.HoraInicio,
                                     HoraFin = sd.HoraFin,
                                     TotalHoras = sd.TotalHoras,
                                     Cantidad = sd.Cantidad,
                                     IdCCosto = sd.IdCCosto,
                                     Proyecto = sd.Proyecto,
                                     Fase = sd.Fase,
                                     IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                     FechaRegistro = sd.FechaRegistro,
                                 }).ToList()
                         }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SolicitudesService.GetSolicitud: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetSolicitudes: Obtiene un registro de una solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de solicitud segun el id de parametro indicado</returns>
        public async Task<cSolicitud> GetSolicitud(int id)
        {
            cSolicitud? model = new();
            try
            {
                model = (from e in await _context.Solicitudes
                                 .Include(e => e.cSolicitudAutorizacion)
                                 .Include(e => e.cEstado)
                                 .Include(e => e.cTipoSolicitud)
                                 .Include(e => e.cCentroCosto)
                                 .Include(e => e.cSolicitudDetalle)
                            .Where(e => e.Id == id)
                            .ToListAsync()
                         select new cSolicitud
                         {
                             Id = e.Id,
                             IdPlanilla = e.IdPlanilla,
                             IdNumero = e.IdNumero,
                             FechaInicio = e.FechaInicio,
                             FechaFin = e.FechaFin,
                             TipoSolicitudId = e.TipoSolicitudId,
                             IdDepart = e.IdDepart,
                             IdGrupo = e.IdGrupo,
                             IdCCosto = e.IdCCosto,
                             proyecto = e.proyecto,
                             fase = e.fase,
                             HoraInicio = e.HoraInicio,
                             HoraFin = e.HoraFin,
                             TotalHoras = e.TotalHoras,
                             Cantidad = e.Cantidad,
                             Comentario = e.Comentario,
                             EstadoId = e.EstadoId,
                             IdUsuarioRegistra = e.IdUsuarioRegistra,
                             FechaRegistro = e.FechaRegistro,
                             FechaModifica = e.FechaModifica,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                (from sa in e.cSolicitudAutorizacion
                                 select new cSolicitudAutorizacion
                                 {
                                     SolicitudId = sa.SolicitudId,
                                     EstadoId = sa.EstadoId,
                                     IdNumero = sa.IdNumero,
                                     FechaRegistro = sa.FechaRegistro,
                                     Comentario = sa.Comentario,
                                 }).ToList(),
                             cEstado = e.cEstado == null ? null :
                                new cEstado
                                {
                                    Id = e.cEstado.Id,
                                    Descripcion = e.cEstado.Descripcion,
                                    FechaRegistro = e.cEstado.FechaRegistro
                                },
                             cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                new cTipoSolicitud
                                {
                                    Id = e.cTipoSolicitud.Id,
                                    Descripcion = e.cTipoSolicitud.Descripcion,
                                    FechaModifica = e.cTipoSolicitud.FechaModifica,
                                    FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                    IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                    TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                },
                             cCentroCosto = e.cCentroCosto == null ? null :
                                new cCentroCosto
                                {
                                    IdCCosto = e.cCentroCosto.IdCCosto,
                                    Descripcion = e.cCentroCosto.Descripcion,
                                    Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                    Distribuye = e.cCentroCosto.Distribuye,

                                },
                             cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                (from sd in e.cSolicitudDetalle
                                 select new cSolicitudDetalle
                                 {
                                     IdRegistro = sd.IdRegistro,
                                     SolicitudId = sd.SolicitudId,
                                     Fecha = sd.Fecha,
                                     HoraInicio = sd.HoraInicio,
                                     HoraFin = sd.HoraFin,
                                     TotalHoras = sd.TotalHoras,
                                     Cantidad = sd.Cantidad,
                                     IdCCosto = sd.IdCCosto,
                                     Proyecto = sd.Proyecto,
                                     Fase = sd.Fase,
                                     IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                     FechaRegistro = sd.FechaRegistro,
                                 }).ToList()
                         }).FirstOrDefault();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SolicitudesService.GetSolicitud: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model!;
        }

        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes para un colaborador según el id de número, fecha inicial, fecha final y tipo de solicitud.
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cSolicitud>> GetSolicitud(string idnumero, string fechaInicial, string fechaFinal, int tipoSolicitud)
        {
            List<cSolicitud> solicitudes = new();
            DateTime fechaini = DateTime.Parse(fechaInicial.Substring(0, 4) + "-" + fechaInicial.Substring(4, 2) + "-" + fechaInicial.Substring(6, 2) + " 00:00:00");
            DateTime fechafin = DateTime.Parse(fechaFinal.Substring(0, 4) + "-" + fechaFinal.Substring(4, 2) + "-" + fechaFinal.Substring(6, 2) + " 23:59:59");

            try
            {
                solicitudes = (from e in await _context.Solicitudes
                                     .Include(e => e.cSolicitudAutorizacion)
                                     .Include(e => e.cEstado)
                                     .Include(e => e.cTipoSolicitud)
                                     .Include(e => e.cCentroCosto)
                                     .Include(e => e.cSolicitudDetalle)
                                .Where(e => e.IdNumero == idnumero && e.FechaInicio >= fechaini && e.FechaFin <= fechafin
                                     && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)).ToListAsync()
                               select new cSolicitud
                               {
                                   Id = e.Id,
                                   IdPlanilla = e.IdPlanilla,
                                   IdNumero = e.IdNumero,
                                   FechaInicio = e.FechaInicio,
                                   FechaFin = e.FechaFin,
                                   TipoSolicitudId = e.TipoSolicitudId,
                                   IdDepart = e.IdDepart,
                                   IdGrupo = e.IdGrupo,
                                   IdCCosto = e.IdCCosto,
                                   proyecto = e.proyecto,
                                   fase = e.fase,
                                   HoraInicio = e.HoraInicio,
                                   HoraFin = e.HoraFin,
                                   TotalHoras = e.TotalHoras,
                                   Cantidad = e.Cantidad,
                                   Comentario = e.Comentario,
                                   EstadoId = e.EstadoId,
                                   IdUsuarioRegistra = e.IdUsuarioRegistra,
                                   FechaRegistro = e.FechaRegistro,
                                   FechaModifica = e.FechaModifica,
                                   IdUsuarioModifica = e.IdUsuarioModifica,
                                   cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                           (from sa in e.cSolicitudAutorizacion
                                            select new cSolicitudAutorizacion
                                            {
                                                SolicitudId = sa.SolicitudId,
                                                EstadoId = sa.EstadoId,
                                                IdNumero = sa.IdNumero,
                                                FechaRegistro = sa.FechaRegistro,
                                                Comentario = sa.Comentario,
                                            }).ToList(),
                                   cEstado = e.cEstado == null ? null :
                                           new cEstado
                                           {
                                               Id = e.cEstado.Id,
                                               Descripcion = e.cEstado.Descripcion,
                                               FechaRegistro = e.cEstado.FechaRegistro
                                           },
                                   cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                           new cTipoSolicitud
                                           {
                                               Id = e.cTipoSolicitud.Id,
                                               Descripcion = e.cTipoSolicitud.Descripcion,
                                               FechaModifica = e.cTipoSolicitud.FechaModifica,
                                               FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                               IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                               TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                           },
                                   cCentroCosto = e.cCentroCosto == null ? null :
                                        new cCentroCosto
                                        {
                                            IdCCosto = e.cCentroCosto.IdCCosto,
                                            Descripcion = e.cCentroCosto.Descripcion,
                                            Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                            Distribuye = e.cCentroCosto.Distribuye,

                                        },
                                   cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                        (from sd in e.cSolicitudDetalle
                                         select new cSolicitudDetalle
                                         {
                                             IdRegistro = sd.IdRegistro,
                                             SolicitudId = sd.SolicitudId,
                                             Fecha = sd.Fecha,
                                             HoraInicio = sd.HoraInicio,
                                             HoraFin = sd.HoraFin,
                                             TotalHoras = sd.TotalHoras,
                                             Cantidad = sd.Cantidad,
                                             IdCCosto = sd.IdCCosto,
                                             Proyecto = sd.Proyecto,
                                             Fase = sd.Fase,
                                             IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                             FechaRegistro = sd.FechaRegistro,
                                         }).ToList()
                               }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SolicitudesService.GetSolicitud: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return solicitudes;
        }


        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes para un colaborador según el estado, tipo de solicitud, fecha inicial, fecha final 
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cSolicitud>> GetSolicitud(int estado, int tipoSolicitud, string fechaInicial, string fechaFinal)
        {
            List<cSolicitud> solicitudes = new();
            DateTime fechaini = DateTime.Parse(fechaInicial.Substring(0, 4) + "-" + fechaInicial.Substring(4, 2) + "-" + fechaInicial.Substring(6, 2) + " 00:00:00");
            DateTime fechafin = DateTime.Parse(fechaFinal.Substring(0, 4) + "-" + fechaFinal.Substring(4, 2) + "-" + fechaFinal.Substring(6, 2) + " 23:59:59");

            try
            {
                switch (estado)
                {
                    case 0: /*Registradas*/
                        solicitudes = (from e in await _context.Solicitudes
                                    .Include(e => e.cSolicitudAutorizacion)
                                    .Include(e => e.cEstado)
                                    .Include(e => e.cTipoSolicitud)
                                    .Include(e => e.cCentroCosto)
                                    .Include(e => e.cSolicitudDetalle)
                               .Where(e => e.FechaInicio >= fechaini && e.FechaFin <= fechafin && e.EstadoId == 0 && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)).ToListAsync()
                                       select new cSolicitud
                                       {
                                           Id = e.Id,
                                           IdPlanilla = e.IdPlanilla,
                                           IdNumero = e.IdNumero,
                                           FechaInicio = e.FechaInicio,
                                           FechaFin = e.FechaFin,
                                           TipoSolicitudId = e.TipoSolicitudId,
                                           IdDepart = e.IdDepart,
                                           IdGrupo = e.IdGrupo,
                                           IdCCosto = e.IdCCosto,
                                           proyecto = e.proyecto,
                                           fase = e.fase,
                                           HoraInicio = e.HoraInicio,
                                           HoraFin = e.HoraFin,
                                           TotalHoras = e.TotalHoras,
                                           Cantidad = e.Cantidad,
                                           Comentario = e.Comentario,
                                           EstadoId = e.EstadoId,
                                           IdUsuarioRegistra = e.IdUsuarioRegistra,
                                           FechaRegistro = e.FechaRegistro,
                                           FechaModifica = e.FechaModifica,
                                           IdUsuarioModifica = e.IdUsuarioModifica,
                                           cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                                   (from sa in e.cSolicitudAutorizacion
                                                    select new cSolicitudAutorizacion
                                                    {
                                                        SolicitudId = sa.SolicitudId,
                                                        EstadoId = sa.EstadoId,
                                                        IdNumero = sa.IdNumero,
                                                        FechaRegistro = sa.FechaRegistro,
                                                        Comentario = sa.Comentario,
                                                    }).ToList(),
                                           cEstado = e.cEstado == null ? null :
                                                   new cEstado
                                                   {
                                                       Id = e.cEstado.Id,
                                                       Descripcion = e.cEstado.Descripcion,
                                                       FechaRegistro = e.cEstado.FechaRegistro
                                                   },
                                           cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                                   new cTipoSolicitud
                                                   {
                                                       Id = e.cTipoSolicitud.Id,
                                                       Descripcion = e.cTipoSolicitud.Descripcion,
                                                       FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                       FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                       IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                       TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                                   },
                                           cCentroCosto = e.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = e.cCentroCosto.IdCCosto,
                                                    Descripcion = e.cCentroCosto.Descripcion,
                                                    Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                                    Distribuye = e.cCentroCosto.Distribuye,

                                                },
                                           cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                                (from sd in e.cSolicitudDetalle
                                                 select new cSolicitudDetalle
                                                 {
                                                     IdRegistro = sd.IdRegistro,
                                                     SolicitudId = sd.SolicitudId,
                                                     Fecha = sd.Fecha,
                                                     HoraInicio = sd.HoraInicio,
                                                     HoraFin = sd.HoraFin,
                                                     TotalHoras = sd.TotalHoras,
                                                     Cantidad = sd.Cantidad,
                                                     IdCCosto = sd.IdCCosto,
                                                     Proyecto = sd.Proyecto,
                                                     Fase = sd.Fase,
                                                     IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                     FechaRegistro = sd.FechaRegistro,
                                                 }).ToList()
                                       }).ToList();
                        break;
                    case 1: /*pendientes*/

                        solicitudes = (from e in await _context.Solicitudes
                                    .Include(e => e.cSolicitudAutorizacion)
                                    .Include(e => e.cEstado)
                                    .Include(e => e.cTipoSolicitud)
                                    .Include(e => e.cCentroCosto)
                                    .Include(e => e.cSolicitudDetalle)
                               .Where(e => e.FechaInicio >= fechaini && e.FechaFin <= fechafin && e.EstadoId > 0 && e.EstadoId < 99 && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)).ToListAsync()
                                       select new cSolicitud
                                       {
                                           Id = e.Id,
                                           IdPlanilla = e.IdPlanilla,
                                           IdNumero = e.IdNumero,
                                           FechaInicio = e.FechaInicio,
                                           FechaFin = e.FechaFin,
                                           TipoSolicitudId = e.TipoSolicitudId,
                                           IdDepart = e.IdDepart,
                                           IdGrupo = e.IdGrupo,
                                           IdCCosto = e.IdCCosto,
                                           proyecto = e.proyecto,
                                           fase = e.fase,
                                           HoraInicio = e.HoraInicio,
                                           HoraFin = e.HoraFin,
                                           TotalHoras = e.TotalHoras,
                                           Cantidad = e.Cantidad,
                                           Comentario = e.Comentario,
                                           EstadoId = e.EstadoId,
                                           IdUsuarioRegistra = e.IdUsuarioRegistra,
                                           FechaRegistro = e.FechaRegistro,
                                           FechaModifica = e.FechaModifica,
                                           IdUsuarioModifica = e.IdUsuarioModifica,
                                           cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                                   (from sa in e.cSolicitudAutorizacion
                                                    select new cSolicitudAutorizacion
                                                    {
                                                        SolicitudId = sa.SolicitudId,
                                                        EstadoId = sa.EstadoId,
                                                        IdNumero = sa.IdNumero,
                                                        FechaRegistro = sa.FechaRegistro,
                                                        Comentario = sa.Comentario,
                                                    }).ToList(),
                                           cEstado = e.cEstado == null ? null :
                                                   new cEstado
                                                   {
                                                       Id = e.cEstado.Id,
                                                       Descripcion = e.cEstado.Descripcion,
                                                       FechaRegistro = e.cEstado.FechaRegistro
                                                   },
                                           cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                                   new cTipoSolicitud
                                                   {
                                                       Id = e.cTipoSolicitud.Id,
                                                       Descripcion = e.cTipoSolicitud.Descripcion,
                                                       FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                       FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                       IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                       TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                                   },
                                           cCentroCosto = e.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = e.cCentroCosto.IdCCosto,
                                                    Descripcion = e.cCentroCosto.Descripcion,
                                                    Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                                    Distribuye = e.cCentroCosto.Distribuye,

                                                },
                                           cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                                (from sd in e.cSolicitudDetalle
                                                 select new cSolicitudDetalle
                                                 {
                                                     IdRegistro = sd.IdRegistro,
                                                     SolicitudId = sd.SolicitudId,
                                                     Fecha = sd.Fecha,
                                                     HoraInicio = sd.HoraInicio,
                                                     HoraFin = sd.HoraFin,
                                                     TotalHoras = sd.TotalHoras,
                                                     Cantidad = sd.Cantidad,
                                                     IdCCosto = sd.IdCCosto,
                                                     Proyecto = sd.Proyecto,
                                                     Fase = sd.Fase,
                                                     IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                     FechaRegistro = sd.FechaRegistro,
                                                 }).ToList()
                                       }).ToList();

                        break;
                    case 2: /*finalizadas*/
                        solicitudes = (from e in await _context.Solicitudes
                                    .Include(e => e.cSolicitudAutorizacion)
                                    .Include(e => e.cEstado)
                                    .Include(e => e.cTipoSolicitud)
                                    .Include(e => e.cCentroCosto)
                                    .Include(e => e.cSolicitudDetalle)
                               .Where(e => e.FechaInicio >= fechaini && e.FechaFin <= fechafin && e.EstadoId == 99 && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)).ToListAsync()
                                       select new cSolicitud
                                       {
                                           Id = e.Id,
                                           IdPlanilla = e.IdPlanilla,
                                           IdNumero = e.IdNumero,
                                           FechaInicio = e.FechaInicio,
                                           FechaFin = e.FechaFin,
                                           TipoSolicitudId = e.TipoSolicitudId,
                                           IdDepart = e.IdDepart,
                                           IdGrupo = e.IdGrupo,
                                           IdCCosto = e.IdCCosto,
                                           proyecto = e.proyecto,
                                           fase = e.fase,
                                           HoraInicio = e.HoraInicio,
                                           HoraFin = e.HoraFin,
                                           TotalHoras = e.TotalHoras,
                                           Cantidad = e.Cantidad,
                                           Comentario = e.Comentario,
                                           EstadoId = e.EstadoId,
                                           IdUsuarioRegistra = e.IdUsuarioRegistra,
                                           FechaRegistro = e.FechaRegistro,
                                           FechaModifica = e.FechaModifica,
                                           IdUsuarioModifica = e.IdUsuarioModifica,
                                           cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                                   (from sa in e.cSolicitudAutorizacion
                                                    select new cSolicitudAutorizacion
                                                    {
                                                        SolicitudId = sa.SolicitudId,
                                                        EstadoId = sa.EstadoId,
                                                        IdNumero = sa.IdNumero,
                                                        FechaRegistro = sa.FechaRegistro,
                                                        Comentario = sa.Comentario,
                                                    }).ToList(),
                                           cEstado = e.cEstado == null ? null :
                                                   new cEstado
                                                   {
                                                       Id = e.cEstado.Id,
                                                       Descripcion = e.cEstado.Descripcion,
                                                       FechaRegistro = e.cEstado.FechaRegistro
                                                   },
                                           cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                                   new cTipoSolicitud
                                                   {
                                                       Id = e.cTipoSolicitud.Id,
                                                       Descripcion = e.cTipoSolicitud.Descripcion,
                                                       FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                       FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                       IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                       TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                                   },
                                           cCentroCosto = e.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = e.cCentroCosto.IdCCosto,
                                                    Descripcion = e.cCentroCosto.Descripcion,
                                                    Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                                    Distribuye = e.cCentroCosto.Distribuye,

                                                },
                                           cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                                (from sd in e.cSolicitudDetalle
                                                 select new cSolicitudDetalle
                                                 {
                                                     IdRegistro = sd.IdRegistro,
                                                     SolicitudId = sd.SolicitudId,
                                                     Fecha = sd.Fecha,
                                                     HoraInicio = sd.HoraInicio,
                                                     HoraFin = sd.HoraFin,
                                                     TotalHoras = sd.TotalHoras,
                                                     Cantidad = sd.Cantidad,
                                                     IdCCosto = sd.IdCCosto,
                                                     Proyecto = sd.Proyecto,
                                                     Fase = sd.Fase,
                                                     IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                     FechaRegistro = sd.FechaRegistro,
                                                 }).ToList()
                                       }).ToList();
                        break;
                    case 3: /*anuladas/rechazadas*/
                        solicitudes = (from e in await _context.Solicitudes
                                     .Include(e => e.cSolicitudAutorizacion)
                                     .Include(e => e.cEstado)
                                     .Include(e => e.cTipoSolicitud)
                                     .Include(e => e.cCentroCosto)
                                     .Include(e => e.cSolicitudDetalle)
                                .Where(e => e.FechaInicio >= fechaini && e.FechaFin <= fechafin && (e.EstadoId == 100 || e.EstadoId == 101) && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)).ToListAsync()
                                       select new cSolicitud
                                       {
                                           Id = e.Id,
                                           IdPlanilla = e.IdPlanilla,
                                           IdNumero = e.IdNumero,
                                           FechaInicio = e.FechaInicio,
                                           FechaFin = e.FechaFin,
                                           TipoSolicitudId = e.TipoSolicitudId,
                                           IdDepart = e.IdDepart,
                                           IdGrupo = e.IdGrupo,
                                           IdCCosto = e.IdCCosto,
                                           proyecto = e.proyecto,
                                           fase = e.fase,
                                           HoraInicio = e.HoraInicio,
                                           HoraFin = e.HoraFin,
                                           TotalHoras = e.TotalHoras,
                                           Cantidad = e.Cantidad,
                                           Comentario = e.Comentario,
                                           EstadoId = e.EstadoId,
                                           IdUsuarioRegistra = e.IdUsuarioRegistra,
                                           FechaRegistro = e.FechaRegistro,
                                           FechaModifica = e.FechaModifica,
                                           IdUsuarioModifica = e.IdUsuarioModifica,
                                           cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                                   (from sa in e.cSolicitudAutorizacion
                                                    select new cSolicitudAutorizacion
                                                    {
                                                        SolicitudId = sa.SolicitudId,
                                                        EstadoId = sa.EstadoId,
                                                        IdNumero = sa.IdNumero,
                                                        FechaRegistro = sa.FechaRegistro,
                                                        Comentario = sa.Comentario,
                                                    }).ToList(),
                                           cEstado = e.cEstado == null ? null :
                                                   new cEstado
                                                   {
                                                       Id = e.cEstado.Id,
                                                       Descripcion = e.cEstado.Descripcion,
                                                       FechaRegistro = e.cEstado.FechaRegistro
                                                   },
                                           cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                                   new cTipoSolicitud
                                                   {
                                                       Id = e.cTipoSolicitud.Id,
                                                       Descripcion = e.cTipoSolicitud.Descripcion,
                                                       FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                       FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                       IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                       TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                                   },
                                           cCentroCosto = e.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = e.cCentroCosto.IdCCosto,
                                                    Descripcion = e.cCentroCosto.Descripcion,
                                                    Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                                    Distribuye = e.cCentroCosto.Distribuye,

                                                },
                                           cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                                (from sd in e.cSolicitudDetalle
                                                 select new cSolicitudDetalle
                                                 {
                                                     IdRegistro = sd.IdRegistro,
                                                     SolicitudId = sd.SolicitudId,
                                                     Fecha = sd.Fecha,
                                                     HoraInicio = sd.HoraInicio,
                                                     HoraFin = sd.HoraFin,
                                                     TotalHoras = sd.TotalHoras,
                                                     Cantidad = sd.Cantidad,
                                                     IdCCosto = sd.IdCCosto,
                                                     Proyecto = sd.Proyecto,
                                                     Fase = sd.Fase,
                                                     IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                     FechaRegistro = sd.FechaRegistro,
                                                 }).ToList()
                                       }).ToList();
                        break;
                   
                }

               
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SolicitudesService.GetSolicitud: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return solicitudes;
        }

        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes finalizadas en un periodo y tipo de solicitud.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cSolicitud>> GetSolicitud(string periodo, int tipoSolicitud)
        {
            List<cSolicitud> solicitudes = new();

            var periodoBuscar = await _context.Ph_Periodos.FirstOrDefaultAsync(p => p.idperiodo == periodo);

            if (periodoBuscar == null)
                return solicitudes;

            try
            {

                solicitudes = (from e in await _context.Solicitudes
                            .Include(e => e.cSolicitudAutorizacion)
                            .Include(e => e.cEstado)
                            .Include(e => e.cTipoSolicitud)
                            .Include(e => e.cCentroCosto)
                            .Include(e => e.cSolicitudDetalle)
                        .Where(e => e.EstadoId==99 
                            && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)
                            && e.cSolicitudAutorizacion!.FirstOrDefault(e => e.EstadoId == 99)!.FechaRegistro >= new DateTime(periodoBuscar.inicio.Year, periodoBuscar.inicio.Month, periodoBuscar.inicio.Day, 0, 0, 0)
                            && e.cSolicitudAutorizacion!.FirstOrDefault(e => e.EstadoId == 99)!.FechaRegistro <= new DateTime(periodoBuscar.fin.Year, periodoBuscar.fin.Month, periodoBuscar.fin.Day, 23, 59, 59)
                         ).ToListAsync()
                                select new cSolicitud
                                {
                                    Id = e.Id,
                                    IdPlanilla = e.IdPlanilla,
                                    IdNumero = e.IdNumero,
                                    FechaInicio = e.FechaInicio,
                                    FechaFin = e.FechaFin,
                                    TipoSolicitudId = e.TipoSolicitudId,
                                    IdDepart = e.IdDepart,
                                    IdGrupo = e.IdGrupo,
                                    IdCCosto = e.IdCCosto,
                                    proyecto = e.proyecto,
                                    fase = e.fase,
                                    HoraInicio = e.HoraInicio,
                                    HoraFin = e.HoraFin,
                                    TotalHoras = e.TotalHoras,
                                    Cantidad = e.Cantidad,
                                    Comentario = e.Comentario,
                                    EstadoId = e.EstadoId,
                                    IdUsuarioRegistra = e.IdUsuarioRegistra,
                                    FechaRegistro = e.FechaRegistro,
                                    FechaModifica = e.FechaModifica,
                                    IdUsuarioModifica = e.IdUsuarioModifica,
                                    cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                            (from sa in e.cSolicitudAutorizacion
                                            select new cSolicitudAutorizacion
                                            {
                                                SolicitudId = sa.SolicitudId,
                                                EstadoId = sa.EstadoId,
                                                IdNumero = sa.IdNumero,
                                                FechaRegistro = sa.FechaRegistro,
                                                Comentario = sa.Comentario,
                                            }).ToList(),
                                    cEstado = e.cEstado == null ? null :
                                            new cEstado
                                            {
                                                Id = e.cEstado.Id,
                                                Descripcion = e.cEstado.Descripcion,
                                                FechaRegistro = e.cEstado.FechaRegistro
                                            },
                                    cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                            new cTipoSolicitud
                                            {
                                                Id = e.cTipoSolicitud.Id,
                                                Descripcion = e.cTipoSolicitud.Descripcion,
                                                FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                            },
                                    cCentroCosto = e.cCentroCosto == null ? null :
                                        new cCentroCosto
                                        {
                                            IdCCosto = e.cCentroCosto.IdCCosto,
                                            Descripcion = e.cCentroCosto.Descripcion,
                                            Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                            Distribuye = e.cCentroCosto.Distribuye,

                                        },
                                    cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                        (from sd in e.cSolicitudDetalle
                                            select new cSolicitudDetalle
                                            {
                                                IdRegistro = sd.IdRegistro,
                                                SolicitudId = sd.SolicitudId,
                                                Fecha = sd.Fecha,
                                                HoraInicio = sd.HoraInicio,
                                                HoraFin = sd.HoraFin,
                                                TotalHoras = sd.TotalHoras,
                                                Cantidad = sd.Cantidad,
                                                IdCCosto = sd.IdCCosto,
                                                Proyecto = sd.Proyecto,
                                                Fase = sd.Fase,
                                                IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                FechaRegistro = sd.FechaRegistro,
                                            }).ToList()
                                }).ToList();
                      

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SolicitudesService.GetSolicitud: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return solicitudes;
        }

        /// <summary>
        /// GetSolicitud: obtiene lista de solictudes para un colaborador según el id de número, fecha inicial, fecha final y tipo de solicitud.
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipoSolicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cSolicitud>> GetSolicitudTercero(string idnumero, string fechaInicial, string fechaFinal, int tipoSolicitud)
        {
            List<cSolicitud> solicitudes = new();
            DateTime fechaini = DateTime.Parse(fechaInicial.Substring(0, 4) + "-" + fechaInicial.Substring(4, 2) + "-" + fechaInicial.Substring(6, 2) + " 00:00:00");
            DateTime fechafin = DateTime.Parse(fechaFinal.Substring(0, 4) + "-" + fechaFinal.Substring(4, 2) + "-" + fechaFinal.Substring(6, 2) + " 23:59:59");

            try
            {
                solicitudes = (from e in await _context.Solicitudes
                                     .Include(e => e.cSolicitudAutorizacion)
                                     .Include(e => e.cEstado)
                                     .Include(e => e.cTipoSolicitud)
                                     .Include(e => e.cCentroCosto)
                                     .Include(e => e.cSolicitudDetalle)
                                .Where(e => e.IdUsuarioRegistra == idnumero && e.IdNumero != idnumero && e.FechaInicio >= fechaini && e.FechaFin <= fechafin
                                     && e.TipoSolicitudId == (tipoSolicitud == 0 ? e.TipoSolicitudId : tipoSolicitud)).ToListAsync()
                               select new cSolicitud
                               {
                                   Id = e.Id,
                                   IdPlanilla = e.IdPlanilla,
                                   IdNumero = e.IdNumero,
                                   FechaInicio = e.FechaInicio,
                                   FechaFin = e.FechaFin,
                                   TipoSolicitudId = e.TipoSolicitudId,
                                   IdDepart = e.IdDepart,
                                   IdGrupo = e.IdGrupo,
                                   IdCCosto = e.IdCCosto,
                                   proyecto = e.proyecto,
                                   fase = e.fase,
                                   HoraInicio = e.HoraInicio,
                                   HoraFin = e.HoraFin,
                                   TotalHoras = e.TotalHoras,
                                   Cantidad = e.Cantidad,
                                   Comentario = e.Comentario,
                                   EstadoId = e.EstadoId,
                                   IdUsuarioRegistra = e.IdUsuarioRegistra,
                                   FechaRegistro = e.FechaRegistro,
                                   FechaModifica = e.FechaModifica,
                                   IdUsuarioModifica = e.IdUsuarioModifica,
                                   cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                           (from sa in e.cSolicitudAutorizacion
                                            select new cSolicitudAutorizacion
                                            {
                                                SolicitudId = sa.SolicitudId,
                                                EstadoId = sa.EstadoId,
                                                IdNumero = sa.IdNumero,
                                                FechaRegistro = sa.FechaRegistro,
                                                Comentario = sa.Comentario,
                                            }).ToList(),
                                   cEstado = e.cEstado == null ? null :
                                           new cEstado
                                           {
                                               Id = e.cEstado.Id,
                                               Descripcion = e.cEstado.Descripcion,
                                               FechaRegistro = e.cEstado.FechaRegistro
                                           },
                                   cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                           new cTipoSolicitud
                                           {
                                               Id = e.cTipoSolicitud.Id,
                                               Descripcion = e.cTipoSolicitud.Descripcion,
                                               FechaModifica = e.cTipoSolicitud.FechaModifica,
                                               FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                               IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                               TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                           },
                                   cCentroCosto = e.cCentroCosto == null ? null :
                                        new cCentroCosto
                                        {
                                            IdCCosto = e.cCentroCosto.IdCCosto,
                                            Descripcion = e.cCentroCosto.Descripcion,
                                            Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                            Distribuye = e.cCentroCosto.Distribuye,

                                        },
                                   cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                        (from sd in e.cSolicitudDetalle
                                         select new cSolicitudDetalle
                                         {
                                             IdRegistro = sd.IdRegistro,
                                             SolicitudId = sd.SolicitudId,
                                             Fecha = sd.Fecha,
                                             HoraInicio = sd.HoraInicio,
                                             HoraFin = sd.HoraFin,
                                             TotalHoras = sd.TotalHoras,
                                             Cantidad = sd.Cantidad,
                                             IdCCosto = sd.IdCCosto,
                                             Proyecto = sd.Proyecto,
                                             Fase = sd.Fase,
                                             IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                             FechaRegistro = sd.FechaRegistro,
                                         }).ToList()
                               }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SolicitudesService.GetSolicitudTercero: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return solicitudes;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2024-12-27      
        //obtener lista Solicitudes de Accion de Personal por Usuario autorizador Y FLUJO DE AUTORIZACION
        public async Task<IEnumerable<cSolicitud>> GetSolicitudPorAprobarFlujoAut(string idnumero)
        {
            List<cSolicitud> solicitudesPorAprobar = new();
            try
            {
                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();
                int vEstadoFinal = 99;

                var solicitudesPendientes = (from e in await _context.Solicitudes
                                                            .Include(e => e.cSolicitudAutorizacion)
                                                            .Include(e => e.cEstado)
                                                            .Include(e => e.cTipoSolicitud)
                                                            .Include(e => e.cCentroCosto)
                                                            .Include(e => e.cSolicitudDetalle)
                                                       .Where(e => e.EstadoId >= 1 && e.EstadoId < vEstadoFinal)
                                                       .ToListAsync()
                                             select new cSolicitud
                                             {
                                                 Id = e.Id,
                                                 IdPlanilla = e.IdPlanilla,
                                                 IdNumero = e.IdNumero,
                                                 FechaInicio = e.FechaInicio,
                                                 FechaFin = e.FechaFin,
                                                 TipoSolicitudId = e.TipoSolicitudId,
                                                 IdDepart = e.IdDepart,
                                                 IdGrupo = e.IdGrupo,
                                                 IdCCosto = e.IdCCosto,
                                                 proyecto = e.proyecto,
                                                 fase = e.fase,
                                                 HoraInicio = e.HoraInicio,
                                                 HoraFin = e.HoraFin,
                                                 TotalHoras = e.TotalHoras,
                                                 Cantidad = e.Cantidad,
                                                 Comentario = e.Comentario,
                                                 EstadoId = e.EstadoId,
                                                 IdUsuarioRegistra = e.IdUsuarioRegistra,
                                                 FechaRegistro = e.FechaRegistro,
                                                 FechaModifica = e.FechaModifica,
                                                 IdUsuarioModifica = e.IdUsuarioModifica,
                                                 cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                                     (from sa in e.cSolicitudAutorizacion
                                                      select new cSolicitudAutorizacion
                                                      {
                                                          SolicitudId = sa.SolicitudId,
                                                          EstadoId = sa.EstadoId,
                                                          IdNumero = sa.IdNumero,
                                                          FechaRegistro = sa.FechaRegistro,
                                                          Comentario = sa.Comentario,
                                                      }).ToList(),
                                                 cEstado = e.cEstado == null ? null :
                                                     new cEstado
                                                     {
                                                         Id = e.cEstado.Id,
                                                         Descripcion = e.cEstado.Descripcion,
                                                         FechaRegistro = e.cEstado.FechaRegistro
                                                     },
                                                 cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                                     new cTipoSolicitud
                                                     {
                                                         Id = e.cTipoSolicitud.Id,
                                                         Descripcion = e.cTipoSolicitud.Descripcion,
                                                         FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                         FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                         IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                         TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                                     },
                                                 cCentroCosto = e.cCentroCosto == null ? null :
                                                    new cCentroCosto
                                                    {
                                                        IdCCosto = e.cCentroCosto.IdCCosto,
                                                        Descripcion = e.cCentroCosto.Descripcion,
                                                        Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                                        Distribuye = e.cCentroCosto.Distribuye,

                                                    },
                                                 cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                                    (from sd in e.cSolicitudDetalle
                                                     select new cSolicitudDetalle
                                                     {
                                                         IdRegistro = sd.IdRegistro,
                                                         SolicitudId = sd.SolicitudId,
                                                         IdCCosto = sd.IdCCosto,
                                                         Fecha = sd.Fecha,
                                                         HoraInicio = sd.HoraInicio,
                                                         HoraFin = sd.HoraFin,
                                                         TotalHoras = sd.TotalHoras,
                                                         Cantidad = sd.Cantidad,
                                                         Proyecto = sd.Proyecto,
                                                         Fase = sd.Fase,
                                                         IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                         FechaRegistro = sd.FechaRegistro,
                                                     }).ToList()

                                             }).ToList();


                int organizacionPadre = 0;


                foreach (var solicitud in solicitudesPendientes)
                {

                    var grupo = await _context.Ph_Grupos.FirstOrDefaultAsync(e => e.idgrupo == solicitud.IdGrupo!);

                    if (grupo is not null)
                    {
                        var flujoAutorizacion = await _flujosServices.GetFlujoAutorizacion(solicitud.cTipoSolicitud!.FlujoAutorizacionId);

                        List<cOrganizacion> jerarquiaGrupo = new();

                        if (grupo.OrganizacionId != null)
                        {
                            organizacionPadre = grupo.OrganizacionId == null ? -1 : (int)grupo.OrganizacionId;
                            cOrganizacion? organizacion = await _organizacionService.GetOrganizacion(organizacionPadre);


                            if (organizacion is not null)
                                jerarquiaGrupo.Add(organizacion);

                            while (organizacion!.OrganizacionSuperior != 0)
                            {
                                organizacion = await _organizacionService.GetOrganizacion(organizacion.OrganizacionSuperior);
                                if (organizacion is not null)
                                    jerarquiaGrupo.Add(organizacion);
                            }



                            if (flujoAutorizacion is not null)
                            {
                                var flujoDetalle = flujoAutorizacion.cFlujoAutorizacionDetalle!.ToList();

                                if (flujoDetalle is not null && flujoDetalle.Count() > 0)
                                {
                                    foreach (var detFlujo in flujoDetalle.OrderBy(e => e.OrdenPrioridad))
                                    {
                                        if (detFlujo.OrganizacionEspecificaId is null)
                                        {
                                            //si es el flujo base, primer nivel de autorizacion --departamento
                                            if (opciones!.ORG_BASE == detFlujo.NivelOrganizacionId)
                                            {
                                                //se evalua si el solicitante tiene un autorizador directo
                                                var jefeResponsable = await _context.EmpleadosJefaturas.FirstOrDefaultAsync(e => e.IdNumero == solicitud.IdNumero);

                                                if (jefeResponsable is not null)
                                                {
                                                    if (jefeResponsable!.IdNumeroResponsable == idnumero || jefeResponsable.IdNumeroSuplente == idnumero)
                                                    {
                                                        if (solicitud.EstadoId == detFlujo.EstadoAnteriorId)
                                                            solicitudesPorAprobar.Add(solicitud);
                                                    }
                                                    else
                                                    {
                                                        //si no tiene autorizador directo verifica si el empleado es autorizador de departamento
                                                        var departamentoResponsable = await _organizacionService.GetOrganizacionBaseResponsable(solicitud.IdGrupo!.ToString()!);
                                                        var esResponsable = departamentoResponsable.Any(e => e.IdNumeroResponsable == idnumero);

                                                        if (esResponsable)
                                                            if (solicitud.EstadoId == detFlujo.EstadoAnteriorId)
                                                                solicitudesPorAprobar.Add(solicitud);
                                                    }
                                                }
                                                else
                                                {
                                                    //si no tiene autoriador directo verifica si el empleado es autorizador de departamento
                                                    var departamentoResponsable = await _organizacionService.GetOrganizacionBaseResponsable(solicitud.IdGrupo!.ToString()!);
                                                    var esResponsable = departamentoResponsable.Any(e => e.IdNumeroResponsable == idnumero);

                                                    if (esResponsable)
                                                        if (solicitud.EstadoId == detFlujo.EstadoAnteriorId)
                                                            solicitudesPorAprobar.Add(solicitud);
                                                }

                                            }
                                            else
                                            {
                                                organizacion = jerarquiaGrupo.FirstOrDefault(e => e.NivelOrganizacionId == detFlujo.NivelOrganizacionId);
                                                if (organizacion is not null)
                                                {
                                                    if (organizacion.cOrganizacionResponsable is not null)
                                                    {
                                                        var esResponsable = organizacion.cOrganizacionResponsable.Any(e => e.IdNumeroResponsable == idnumero);

                                                        if (esResponsable)
                                                            if (solicitud.EstadoId == detFlujo.EstadoAnteriorId)
                                                                solicitudesPorAprobar.Add(solicitud);
                                                    }
                                                }

                                            }

                                        }
                                        else
                                        {
                                            organizacion = await _organizacionService.GetOrganizacion((int)detFlujo.OrganizacionEspecificaId!);

                                            if (organizacion is not null)
                                            {
                                                if (organizacion.cOrganizacionResponsable is not null)
                                                {
                                                    var esResponsable = organizacion.cOrganizacionResponsable.Any(e => e.IdNumeroResponsable == idnumero);

                                                    if (esResponsable)
                                                    {
                                                        if (solicitud.EstadoId == detFlujo.EstadoAnteriorId)
                                                            solicitudesPorAprobar.Add(solicitud);
                                                    }
                                                }
                                            }
                                        }
                                    }


                                }
                            }
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (solicitudesPorAprobar.Count() > 0)
                solicitudesPorAprobar = (solicitudesPorAprobar.Distinct()).ToList();

            return solicitudesPorAprobar!;
        }


        public async Task<IEnumerable<cSolicitud>> GetSolicitudAprobadaUsuario(string idnumero)
        {
            List<cSolicitud> solicitudesAprobadas = new();
            try
            {
                var solicitudesUsuario = (from sa in await _context.SolicitudesAutorizacion.Where(e => e.IdNumero == idnumero).ToListAsync()
                                          select sa.SolicitudId).Distinct().ToList();  
                
                if (solicitudesUsuario.Count() == 0)
                    return solicitudesAprobadas;

                var solicitudesAprob = (from e in await _context.Solicitudes
                                                            .Include(e => e.cSolicitudAutorizacion)
                                                            .Include(e => e.cEstado)
                                                            .Include(e => e.cTipoSolicitud)
                                                            .Include(e => e.cCentroCosto)
                                                            .Include(e => e.cSolicitudDetalle)
                                            .Where(e=>(e.EstadoId >= 1 && solicitudesUsuario.Contains(e.Id)) || (e.EstadoId==101 && e.IdUsuarioModifica==idnumero)).ToListAsync()                                                   
                                            select new cSolicitud
                                             {
                                                 Id = e.Id,
                                                 IdPlanilla = e.IdPlanilla,
                                                 IdNumero = e.IdNumero,
                                                 FechaInicio = e.FechaInicio,
                                                 FechaFin = e.FechaFin,
                                                 TipoSolicitudId = e.TipoSolicitudId,
                                                 IdDepart = e.IdDepart,
                                                 IdGrupo = e.IdGrupo,
                                                 IdCCosto = e.IdCCosto,
                                                 proyecto = e.proyecto,
                                                 fase = e.fase,
                                                 HoraInicio = e.HoraInicio,
                                                 HoraFin = e.HoraFin,
                                                 TotalHoras = e.TotalHoras,
                                                 Cantidad = e.Cantidad,
                                                 Comentario = e.Comentario,
                                                 EstadoId = e.EstadoId,
                                                 IdUsuarioRegistra = e.IdUsuarioRegistra,
                                                 FechaRegistro = e.FechaRegistro,
                                                 FechaModifica = e.FechaModifica,
                                                 IdUsuarioModifica = e.IdUsuarioModifica,
                                                 cSolicitudAutorizacion = e.cSolicitudAutorizacion == null ? null :
                                                     (from sa in e.cSolicitudAutorizacion
                                                      select new cSolicitudAutorizacion
                                                      {
                                                          SolicitudId = sa.SolicitudId,
                                                          EstadoId = sa.EstadoId,
                                                          IdNumero = sa.IdNumero,
                                                          FechaRegistro = sa.FechaRegistro,
                                                          Comentario = sa.Comentario,
                                                      }).ToList(),
                                                 cEstado = e.cEstado == null ? null :
                                                     new cEstado
                                                     {
                                                         Id = e.cEstado.Id,
                                                         Descripcion = e.cEstado.Descripcion,
                                                         FechaRegistro = e.cEstado.FechaRegistro
                                                     },
                                                 cTipoSolicitud = e.cTipoSolicitud == null ? null :
                                                     new cTipoSolicitud
                                                     {
                                                         Id = e.cTipoSolicitud.Id,
                                                         Descripcion = e.cTipoSolicitud.Descripcion,
                                                         FechaModifica = e.cTipoSolicitud.FechaModifica,
                                                         FlujoAutorizacionId = e.cTipoSolicitud.FlujoAutorizacionId,
                                                         IdUsuarioModifica = e.cTipoSolicitud.IdUsuarioModifica,
                                                         TipoConfiguracion = e.cTipoSolicitud.TipoConfiguracion
                                                     },
                                                 cCentroCosto = e.cCentroCosto == null ? null :
                                                    new cCentroCosto
                                                    {
                                                        IdCCosto = e.cCentroCosto.IdCCosto,
                                                        Descripcion = e.cCentroCosto.Descripcion,
                                                        Alias_CCosto = e.cCentroCosto.Alias_CCosto,
                                                        Distribuye = e.cCentroCosto.Distribuye,

                                                    },
                                                 cSolicitudDetalle = e.cSolicitudDetalle == null ? null :
                                                    (from sd in e.cSolicitudDetalle
                                                     select new cSolicitudDetalle
                                                     {
                                                         IdRegistro = sd.IdRegistro,
                                                         SolicitudId = sd.SolicitudId,
                                                         IdCCosto = sd.IdCCosto,
                                                         Fecha = sd.Fecha,
                                                         HoraInicio = sd.HoraInicio,
                                                         HoraFin = sd.HoraFin,
                                                         TotalHoras = sd.TotalHoras,
                                                         Cantidad = sd.Cantidad,
                                                         Proyecto = sd.Proyecto,
                                                         Fase = sd.Fase,
                                                         IdUsuarioRegistra = sd.IdUsuarioRegistra,
                                                         FechaRegistro = sd.FechaRegistro,
                                                     }).ToList()

                                             }).ToList();


                solicitudesAprobadas = solicitudesAprob;


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return solicitudesAprobadas!;
        }

        /// <summary>
        /// PostSolicitudes:  Recibe una lista de registros de solicitudes, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostSolicitud(IEnumerable<cSolicitud> model)
        {
            EventResponse respuesta = new EventResponse();
            long maxId = 0;
            int estadoId = 0;
            bool regNew = false;
            IEnumerable<cSolicitudDetalle>? detalleSolicitud;
            try
            {
                foreach (var item in model)
                {
                    maxId = item.Id;
                    estadoId = item.EstadoId;
                    regNew = false;
                    detalleSolicitud = item.cSolicitudDetalle;

                    cSolicitud? modelBuscar = await _context.Solicitudes.Include(e => e.cTipoSolicitud)
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    if (modelBuscar is not null)
                    {
                        modelBuscar.IdPlanilla = item.IdPlanilla;
                        modelBuscar.IdNumero = item.IdNumero;
                        modelBuscar.FechaInicio = item.FechaInicio;
                        modelBuscar.FechaFin = item.FechaFin;
                        modelBuscar.TipoSolicitudId = item.TipoSolicitudId;
                        modelBuscar.IdDepart = item.IdDepart;
                        modelBuscar.IdGrupo = item.IdGrupo;
                        modelBuscar.IdCCosto = item.IdCCosto;
                        modelBuscar.proyecto = item.proyecto;
                        modelBuscar.fase = item.fase;
                        modelBuscar.HoraInicio = item.HoraInicio;
                        modelBuscar.HoraFin = item.HoraFin;
                        modelBuscar.TotalHoras = item.TotalHoras;
                        modelBuscar.Cantidad = item.Cantidad;
                        modelBuscar.Comentario = item.Comentario;
                        modelBuscar.EstadoId = item.EstadoId;
                        modelBuscar.FechaModifica = DateTime.Now;
                        modelBuscar.IdUsuarioModifica = item.IdUsuarioModifica;

                        _context.Solicitudes.Update(modelBuscar);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        regNew = true;
                        item.cEstado = null;
                        item.cTipoSolicitud = null;
                        item.cCentroCosto = null;
                        item.cSolicitudAutorizacion = null;
                        item.Id = 0;
                        item.FechaRegistro = DateTime.Now;
                        item.FechaModifica = DateTime.Now;
                        item.cSolicitudDetalle = null;
                        _context.Add(item);
                        await _context.SaveChangesAsync();

                        maxId = await _context.Solicitudes.MaxAsync(e => e.Id);

                    }

                    respuesta.ValorRetorno = maxId.ToString();

                    // Guardamos o actualizamos el detalle de la solicitud
                    if (detalleSolicitud is not null && detalleSolicitud.Count() > 0)
                    {
                        item.Id = maxId;
                        var respuestaDet = await PostSolicitudDetalle(detalleSolicitud, item);

                        if (respuestaDet.Id == "1")
                        {
                            return respuestaDet;
                        }
                    }

                    if (!regNew)
                    {
                        var correosPorEnviar = await PostNotificaResponsables(modelBuscar!, maxId, estadoId);
                        if (correosPorEnviar.Count() > 0)
                        {
                            await _geoServices.EnviarCorreo(correosPorEnviar);
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        /// <summary>
        /// PutSolicitudes:  Recibe una lista de registros de solicitudes y actualiza el registro
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PutSolicitud(IEnumerable<cSolicitud> model)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in model)
                {
                    cEmpleado? solicitante = new();

                    cSolicitud? modelBuscar = await _context.Solicitudes
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    if (modelBuscar is not null)
                    {
                        modelBuscar.EstadoId = item.EstadoId;
                        modelBuscar.Comentario = item.Comentario;
                        modelBuscar.FechaModifica = DateTime.Now;
                        modelBuscar.IdUsuarioModifica = item.IdUsuarioModifica;
                        solicitante = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == modelBuscar.IdNumero!);

                        _context.Solicitudes.Update(modelBuscar);
                        await _context.SaveChangesAsync();
                    }

                    //enviar notificacion por correo si el estado es enviado a autorizacion
                    
                    switch (item.EstadoId)
                    {
                        case 1:
                            
                            List<Email> correosEnviar = new();
                            var tipoSolicitud = await _context.TiposSolicitudes.FirstOrDefaultAsync(e => e.Id == item.TipoSolicitudId);
                            var correoSolicitante = await EnviaCorreoSolicitudASolicitante(item.Id,solicitante!, tipoSolicitud!);

                            if (correoSolicitante is not null) correosEnviar.Add(correoSolicitante);

                            var correosJefaturas = await PostNotificaResponsables(modelBuscar!, item.Id, item.EstadoId);
                            if (correosJefaturas.Count() > 0)                        
                                correosEnviar.AddRange(correosJefaturas);

                            if (correosEnviar.Count() >0)
                                await _geoServices.EnviarCorreo(correosEnviar);

                            break;

                        case 99:
                            //enviar correo a solicitante de solicitud aprobada
                            List<Email> correosAprobacion = new();
                            var correoSolicitanteAprob = await EnviaCorreoSolicitudAutorizada(solicitante!, item.Id);
                            if (correoSolicitanteAprob is not null) correosAprobacion.Add(correoSolicitanteAprob);
                            if (correosAprobacion.Count() > 0)
                                await _geoServices.EnviarCorreo(correosAprobacion);
                            break;
                        case 101:
                            //enviar correo a solicitante de solicitud rechazada
                            List<Email> correosRechazo = new();                           
                            var tipoSolicitudRechazo = await _context.TiposSolicitudes.FirstOrDefaultAsync(e => e.Id == item.TipoSolicitudId);
                            var correoSolicitanteRechazo = await EnviaCorreoSolicitudRechazada(solicitante!, item.Id, item.Comentario!);
                            if (correoSolicitanteRechazo is not null) correosRechazo.Add(correoSolicitanteRechazo);
                            if (correosRechazo.Count() > 0)
                                await _geoServices.EnviarCorreo(correosRechazo);
                            break;

                    }



                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        public async Task<EventResponse> PutSolicitudActGrupo(IEnumerable<cSolicitud> model)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in model)
                {

                    cSolicitud? modelBuscar = await _context.Solicitudes
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    if (modelBuscar is not null)
                    {
                        
                        modelBuscar.IdGrupo = item.IdGrupo;
                        modelBuscar.FechaModifica = DateTime.Now;
                        modelBuscar.IdUsuarioModifica = item.IdUsuarioModifica;

                        _context.Solicitudes.Update(modelBuscar);
                        await _context.SaveChangesAsync();
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de la solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de la solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// AnularSolicitud: anula un registro de solicitud según el id de parámetro indicado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> AnularSolicitud(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cSolicitud? model = await _context.Solicitudes
                                            .FirstOrDefaultAsync(e => e.Id.ToString() == id);

                if (model is not null)
                {
                    model.EstadoId = 100; // Cambiamos el estado a eliminado
                    model.FechaModifica = DateTime.Now;
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el flujo de autorización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el flujo de autorización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        private async Task<EventResponse> PostSolicitudDetalle(IEnumerable<cSolicitudDetalle> model, cSolicitud solicitud)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var item in model)
                {
                    cSolicitudDetalle? modelBuscar = await _context.SolicitudesDetalles
                                 .FirstOrDefaultAsync(e => e.IdRegistro == item.IdRegistro && e.SolicitudId== solicitud.Id);
                    if (modelBuscar is not null)
                    {
                        modelBuscar.Fecha = item.Fecha;
                        modelBuscar.IdCCosto = item.IdCCosto;
                        modelBuscar.HoraInicio = item.HoraInicio;
                        modelBuscar.HoraFin = item.HoraFin;
                        modelBuscar.TotalHoras = item.TotalHoras;
                        modelBuscar.Cantidad = item.Cantidad;
                        modelBuscar.Proyecto = item.Proyecto;
                        modelBuscar.Fase = item.Fase;
                       
                        _context.SolicitudesDetalles.Update(modelBuscar);
                    }
                    else
                    {
                        item.FechaRegistro = DateTime.Now;
                        item.SolicitudId = solicitud.Id;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();                    
                }

                var itemsActuales = await _context.SolicitudesDetalles.Where(e => e.SolicitudId == solicitud.Id).ToListAsync();
                foreach (var sd in itemsActuales)
                {
                    if (!model.Where(e=> e.IdRegistro != 0).Any(e=>e.SolicitudId==sd.SolicitudId && e.IdRegistro == sd.IdRegistro))
                    {
                        _context.SolicitudesDetalles.Remove(sd);
                        await _context.SaveChangesAsync();
                    }
                        
                }
                

            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "PostSolicitudDetalle: No se pudo realizar la actualización del detalle de la solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "PostSolicitudDetalle: No se pudo realizar la actualización del detalle de la solicitud. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// PostNotificaResponsables: crea los formatos de correo para los responsables de una nueva solicitud de accion de personal
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="id"></param>
        /// <param name="estadoId"></param>
        /// <returns></returns>
        public async Task<List<Email>> PostNotificaResponsables(cSolicitud solicitud, long id, int estadoId)
        {
            var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();

            List<Email> correosPorEnviar = new();

            try
            {

                if (estadoId >0)
                {
                    var autorizadorDirecto = await _context.EmpleadosJefaturas.FirstOrDefaultAsync(e => e.IdNumero == solicitud.IdNumero!);
                    var solicitante = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == solicitud.IdNumero!);

                    if (autorizadorDirecto is not null)
                    {

                        var jefatura = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == autorizadorDirecto.IdNumeroResponsable);

                        if (jefatura is not null)
                        {
                            var correo = await EnviaCorreoSolicitudAutorización(jefatura, id, solicitante!, solicitud.cTipoSolicitud!);
                            if (correo is not null) correosPorEnviar.Add(correo);
                        }
                    }
                    else
                    {

                        var flujoAutorizacion = await _flujosServices.GetFlujoAutorizacion(solicitud.cTipoSolicitud!.FlujoAutorizacionId);

                        if (flujoAutorizacion is null) return correosPorEnviar;

                        if (flujoAutorizacion.cFlujoAutorizacionDetalle is not null)
                        {
                            var nivelautorizacion = flujoAutorizacion.cFlujoAutorizacionDetalle.FirstOrDefault(e => e.EstadoAnteriorId == estadoId);
                            if (nivelautorizacion != null)
                            {
                                if (nivelautorizacion.OrganizacionEspecificaId is null)
                                {
                                    if (nivelautorizacion.NivelOrganizacionId == opciones.ORG_BASE)
                                    {
                                        var usersOrgBase = await (from d in _context.OrganizacionBaseResponsables.Where(e => e.IdOrgBase == solicitud.IdGrupo!.ToString() && e.OrdenJerarquia == 1)
                                                                  join u in _context.Empleados on d.IdNumeroResponsable equals u.IdNumero
                                                                  select u).ToListAsync();

                                        foreach (var user in usersOrgBase)
                                        {
                                            var correo = await EnviaCorreoSolicitudAutorización(user, id, solicitante!, solicitud.cTipoSolicitud!);
                                            if (correo is not null)
                                                correosPorEnviar.Add(correo);
                                        }
                                    }
                                }
                                else
                                {
                                    var userOrganizacion = await (from d in _context.OrganizacionResponsables.Where(e => e.OrganizacionId == nivelautorizacion.OrganizacionEspecificaId! && e.OrdenJerarquia == 1)
                                                                  join u in _context.Empleados on d.IdNumeroResponsable equals u.IdNumero
                                                                  select u).ToListAsync();

                                    foreach (var user in userOrganizacion)
                                    {
                                        var correo = await EnviaCorreoSolicitudAutorización(user, id, solicitante!, solicitud.cTipoSolicitud!);
                                        if (correo is not null)
                                            correosPorEnviar.Add(correo);
                                    }
                                }
                            }
                        }

                    }

                    return correosPorEnviar;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return correosPorEnviar;
        }


        private async Task<Email> EnviaCorreoSolicitudASolicitante(long solicitud, cEmpleado solicitante, cTipoSolicitud tipoSolicitud)
        {
            Email? email = null;

            try
            {

                if (!string.IsNullOrEmpty(solicitante.Email))
                {
                    var compania = await _context.PH_COMPANIAS.FirstOrDefaultAsync(e => e.IDCOMP == _schema);
                    email = new();

                    string cuerpoHtml = $"<!DOCTYPE html>" +
                        $"<html><head>Hola.</head>" +
                        $"<body><br><br>Se ha generado una solicitud No. {solicitud} en el Portal de Marcas con los siguientes datos:" +
                        $"<br>Solicitante: {solicitante.IdNumero}: {solicitante.Nombre}" +
                        $"<br>Tipo de Solicitud: {tipoSolicitud.Descripcion}." +
                        $"<br>Compania: {compania!.COMPANIA}.<br>" +
                        $"<br>Si necesita verificar la solicitud puede ingresar al portal de marcas web y consultarlo." +
                        $"Este mensaje se generó de forma automática.  Por favor no contestar.</footer></html>";


                    email.Asunto = $"Solicitud de Novedad: {tipoSolicitud.Descripcion}.";
                    email.Cuerpo = cuerpoHtml;
                    email.Para = solicitante.Email!;
                    email.Adjunto = "";
                    email.CC = "";

                }
            }
            catch (Exception)
            {
                email = null;
            }
            return email!;
        }
        private async Task<Email> EnviaCorreoSolicitudAutorización(cEmpleado empleado, long solicitud, cEmpleado solicitante, cTipoSolicitud tipoSolicitud)
        {
            Email? email = null;

            try
            {

                if (!string.IsNullOrEmpty(empleado.Email))
                {
                    var compania = await _context.PH_COMPANIAS.FirstOrDefaultAsync(e => e.IDCOMP == _schema);
                    email = new();

                    string cuerpoHtml = $"<!DOCTYPE html>" +
                        $"<html><head>Hola.</head>" +
                        $"<body><br><br>Se ha generado una solicitud No. {solicitud} en el Portal de Marcas que requiere de su Aprobación." +
                        $"<br>Colaborador: {solicitante.IdNumero}: {solicitante.Nombre}" +
                        $"<br>Tipo de Solicitud: {tipoSolicitud.Descripcion}." +
                        $"<br>Compania: {compania!.COMPANIA}.<br>" +
                        $"<br>Por favor vaya al portal de marcas web y verifique el documento." +
                        $"Este mensaje se generó de forma automática.  Por favor no contestar.</footer></html>";


                    email.Asunto = $"Solicitud de Autorización de {tipoSolicitud.Descripcion}.";
                    email.Cuerpo = cuerpoHtml;
                    email.Para = empleado.Email!;
                    email.Adjunto = "";
                    email.CC = "";

                }
            }
            catch (Exception)
            {
                email = null;
            }
            return email!;
        }

        public async Task<Email> EnviaCorreoSolicitudAutorizada(cEmpleado empleado, long solicitud)
        {
            Email? email = new();

            try
            {
                if (!string.IsNullOrEmpty(empleado.Email))
                {
                    string cuerpoHtml = $"<!DOCTYPE html>" +
                        $"<html><head>Hola {empleado.Nombre}.</head>" +
                        $"<body><br><br>La solicitud No. {solicitud} que usted realizó, ha sido autorizada.  Por favor vaya al portal de marcas y verifique el documento.<br>" +
                        $"Este mensaje se generó de forma automática.  Por favor no contestar.</footer></html>";

                    email.Asunto = $"Solicitud de Acción de Personal No. {solicitud} ha sido Autorizada.";
                    email.Cuerpo = cuerpoHtml;
                    email.Para = empleado.Email;
                    email.Adjunto = "";
                    email.CC = "";
                }
                else
                    email = null;
            }
            catch (Exception e)
            {
                email = null;
            }
            return email!;
        }

        public async Task<Email> EnviaCorreoSolicitudRechazada(cEmpleado empleado, long solicitud, string motivo)
        {
            Email? email = new();

            try
            {

                if (!string.IsNullOrEmpty(empleado.Email))
                {
                    string cuerpoHtml = $"<!DOCTYPE html>" +
                        $"<html><head>Hola {empleado.Nombre}.</head>" +
                        $"<body><br><br>La solicitud No. {solicitud} que usted realizó, ha sido rechazada. {motivo}.<br>" +
                        $"Este mensaje se generó de forma automática.  Por favor no contestar.</footer></html>";

                    email.Asunto = $"Su solicitud de No. {solicitud} ha sido Rechazada.";
                    email.Cuerpo = cuerpoHtml;
                    email.Para = empleado.Email;
                    email.Adjunto = "";
                    email.CC = "";
                }
                else
                    email = null;
            }
            catch (Exception e)
            {
                email = null;
            }
            return email!;
        }

        /// <summary>
        /// AutorizantesSolicitud: obtener lista de autorizantes de la solicitud
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cAutorizante>> GetAutorizantesSolicitud(cSolicitud solicitud)
        {
            List<cAutorizante> listAutorizantes = new();
            DateTime? fechaAutoriza = null;
            try
            {


                var estados = await _flujosServices.GetEstado();
                var grupo = await _geoServices.GetGrupo((int)solicitud.IdGrupo!);
                var orgBaseResp = await _organizacionService.GetOrganizacionBaseResponsable(solicitud.IdGrupo.ToString()!);
                if (orgBaseResp is null)
                    return listAutorizantes;
                var empleadoJefatura = await _context.EmpleadosJefaturas.FirstOrDefaultAsync(e => e.IdNumero == solicitud.IdNumero);
                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();


                var flujoAutorizacion = await _flujosServices.GetFlujoAutorizacion(solicitud.cTipoSolicitud!.FlujoAutorizacionId);
                cOrganizacion organizacionSuperior = new();
                var solicitudAut = await _context.SolicitudesAutorizacion.Where(e => e.SolicitudId == solicitud.Id).ToListAsync();
                string? IdNumeroAutoriza = null;
                int organizacionPadre = 0;

                List<cOrganizacion> jerarquiaGrupo = new();

                organizacionPadre = grupo.OrganizacionId == null ? -1 : (int)grupo.OrganizacionId;
                cOrganizacion? organizacion = await _organizacionService.GetOrganizacion(organizacionPadre);


                if (organizacion is not null)
                    jerarquiaGrupo.Add(organizacion);

                while (organizacion!.OrganizacionSuperior != 0)
                {
                    organizacion = await _organizacionService.GetOrganizacion(organizacion.OrganizacionSuperior);
                    if (organizacion is not null)
                        jerarquiaGrupo.Add(organizacion);
                }


                if (flujoAutorizacion is not null)
                {
                    if (flujoAutorizacion.cFlujoAutorizacionDetalle is not null)
                    {
                        foreach (var detalleFlujo in flujoAutorizacion.cFlujoAutorizacionDetalle.OrderBy(e => e.OrdenPrioridad))
                        {
                            var autorizacion = solicitudAut.FirstOrDefault(e => e.EstadoId == detalleFlujo.EstadoNuevoId);
                            if (autorizacion is not null)
                            {
                                IdNumeroAutoriza = autorizacion.IdNumero;
                                fechaAutoriza = autorizacion.FechaRegistro;
                                var estadoDoc = estados.FirstOrDefault(e => e.Id == autorizacion.EstadoId);

                                listAutorizantes.Add(new cAutorizante
                                {
                                    IdNumero = IdNumeroAutoriza!,
                                    IdNivelAutorizacion = Models.Utils.Utility.Right($"00{detalleFlujo.OrdenPrioridad}", 2),
                                    DescNivelAutorizacion = estadoDoc!.Descripcion,
                                    FechaAutorizacion = fechaAutoriza
                                });

                            }
                            else
                            {
                                //si la autorizacion va por flujo normal
                                if (detalleFlujo.OrganizacionEspecificaId is null)
                                {
                                    //es nivel de organizacion departamento
                                    if (opciones!.ORG_BASE == detalleFlujo.NivelOrganizacionId)
                                    {
                                        //se obtiene responsable del departamento
                                        if (orgBaseResp is not null)
                                        {
                                            if (empleadoJefatura is null)
                                            {
                                                var responsableGrupo = orgBaseResp!.OrderBy(e => e.OrdenJerarquia).FirstOrDefault();

                                                if (responsableGrupo is not null)
                                                {

                                                    listAutorizantes.Add(new cAutorizante
                                                    {
                                                        IdNumero = responsableGrupo!.IdNumeroResponsable,
                                                        IdNivelAutorizacion = Models.Utils.Utility.Right($"00{detalleFlujo.OrdenPrioridad}", 2),
                                                        DescNivelAutorizacion = $"Responsable de {grupo.descripcion}",
                                                        FechaAutorizacion = null
                                                    });

                                                    organizacionSuperior = await _organizacionService.GetOrganizacion((int)grupo.OrganizacionId!);
                                                }
                                                else
                                                {
                                                    organizacionSuperior = await _organizacionService.GetOrganizacion((int)grupo.OrganizacionId!);
                                                }
                                            }
                                            else
                                            {

                                                listAutorizantes.Add(new cAutorizante
                                                {
                                                    IdNumero = empleadoJefatura!.IdNumeroResponsable,
                                                    IdNivelAutorizacion = Models.Utils.Utility.Right($"00{detalleFlujo.OrdenPrioridad}", 2),
                                                    DescNivelAutorizacion = $"Responsable de {grupo.descripcion}",
                                                    FechaAutorizacion = null
                                                });

                                                organizacionSuperior = await _organizacionService.GetOrganizacion((int)grupo.OrganizacionId!);
                                            }
                                        }


                                    }
                                    else
                                    {

                                        //son niveles superiores de autorizacion
                                        //se debe identificar el nivel superior de organizacion asociado al departamento o la organizacion siguiente
                                        fechaAutoriza = null;
                                        organizacionSuperior = jerarquiaGrupo.FirstOrDefault(e => e.NivelOrganizacionId == detalleFlujo.NivelOrganizacionId)!;

                                        if (organizacionSuperior is not null)
                                        {
                                            var responsableOrg = await _organizacionService.GetResponsableOrganizacion(organizacionSuperior.Id, detalleFlujo.OrdenPrioridad, fechaAutoriza);
                                            if (responsableOrg is not null)
                                            {
                                                listAutorizantes.Add(responsableOrg);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    fechaAutoriza = null;
                                    var responsableOrg = await _organizacionService.GetResponsableOrganizacion((int)detalleFlujo.OrganizacionEspecificaId, detalleFlujo.OrdenPrioridad, fechaAutoriza);
                                    if (responsableOrg is not null)
                                    {
                                        listAutorizantes.Add(responsableOrg);
                                    }


                                }

                            }


                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return listAutorizantes;
        }


        /// <summary>
        /// AutorizantesSolicitud: obtener lista de autorizantes de la solicitud
        /// </summary>
        /// <param name="IdGrupo"></param>
        /// <param name="TipoSolicitudId"></param>
        /// <param name="IdNumero"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cAutorizante>> GetAutorizantesSolicitud(int IdGrupo, int TipoSolicitudId, string IdNumero, long Id)
        {
            List<cAutorizante> listAutorizantes = new();
            DateTime? fechaAutoriza = null;
            try
            {

                var tipoSolicitud = await _context.TiposSolicitudes.FirstOrDefaultAsync(e => e.Id == TipoSolicitudId);
                var estados = await _flujosServices.GetEstado();
                var grupo = await _geoServices.GetGrupo(IdGrupo!);
                var orgBaseResp = await _organizacionService.GetOrganizacionBaseResponsable(IdGrupo.ToString()!);
                if (orgBaseResp is null)
                    return listAutorizantes;
                var empleadoJefatura = await _context.EmpleadosJefaturas.FirstOrDefaultAsync(e => e.IdNumero == IdNumero);
                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();


                var flujoAutorizacion = await _flujosServices.GetFlujoAutorizacion(tipoSolicitud!.FlujoAutorizacionId);
                cOrganizacion organizacionSuperior = new();
                var solicitudAut = await _context.SolicitudesAutorizacion.Where(e => e.SolicitudId == Id).ToListAsync();
                string? IdNumeroAutoriza = null;
                int organizacionPadre = 0;

                List<cOrganizacion> jerarquiaGrupo = new();

                organizacionPadre = grupo.OrganizacionId == null ? -1 : (int)grupo.OrganizacionId;
                cOrganizacion? organizacion = await _organizacionService.GetOrganizacion(organizacionPadre);


                if (organizacion is not null)
                    jerarquiaGrupo.Add(organizacion);

                while (organizacion!.OrganizacionSuperior != 0)
                {
                    organizacion = await _organizacionService.GetOrganizacion(organizacion.OrganizacionSuperior);
                    if (organizacion is not null)
                        jerarquiaGrupo.Add(organizacion);
                }


                if (flujoAutorizacion is not null)
                {
                    if (flujoAutorizacion.cFlujoAutorizacionDetalle is not null)
                    {
                        foreach (var detalleFlujo in flujoAutorizacion.cFlujoAutorizacionDetalle.OrderBy(e => e.OrdenPrioridad))
                        {
                            var autorizacion = solicitudAut.FirstOrDefault(e => e.EstadoId == detalleFlujo.EstadoNuevoId);
                            if (autorizacion is not null)
                            {
                                IdNumeroAutoriza = autorizacion.IdNumero;
                                fechaAutoriza = autorizacion.FechaRegistro;
                                var estadoDoc = estados.FirstOrDefault(e => e.Id == autorizacion.EstadoId);

                                listAutorizantes.Add(new cAutorizante
                                {
                                    IdNumero = IdNumeroAutoriza!,
                                    IdNivelAutorizacion = Models.Utils.Utility.Right($"00{detalleFlujo.OrdenPrioridad}", 2),
                                    DescNivelAutorizacion = estadoDoc!.Descripcion,
                                    FechaAutorizacion = fechaAutoriza
                                });

                            }
                            else
                            {
                                //si la autorizacion va por flujo normal
                                if (detalleFlujo.OrganizacionEspecificaId is null)
                                {
                                    //es nivel de organizacion departamento
                                    if (opciones!.ORG_BASE == detalleFlujo.NivelOrganizacionId)
                                    {
                                        //se obtiene responsable del departamento
                                        if (orgBaseResp is not null)
                                        {
                                            if (empleadoJefatura is null)
                                            {
                                                var responsableGrupo = orgBaseResp!.OrderBy(e => e.OrdenJerarquia).FirstOrDefault();

                                                if (responsableGrupo is not null)
                                                {

                                                    listAutorizantes.Add(new cAutorizante
                                                    {
                                                        IdNumero = responsableGrupo!.IdNumeroResponsable,
                                                        IdNivelAutorizacion = Models.Utils.Utility.Right($"00{detalleFlujo.OrdenPrioridad}", 2),
                                                        DescNivelAutorizacion = $"Responsable de {grupo.descripcion}",
                                                        FechaAutorizacion = null
                                                    });

                                                    organizacionSuperior = await _organizacionService.GetOrganizacion((int)grupo.OrganizacionId!);
                                                }
                                                else
                                                {
                                                    organizacionSuperior = await _organizacionService.GetOrganizacion((int)grupo.OrganizacionId!);
                                                }
                                            }
                                            else
                                            {

                                                listAutorizantes.Add(new cAutorizante
                                                {
                                                    IdNumero = empleadoJefatura!.IdNumeroResponsable,
                                                    IdNivelAutorizacion = Models.Utils.Utility.Right($"00{detalleFlujo.OrdenPrioridad}", 2),
                                                    DescNivelAutorizacion = $"Responsable de {grupo.descripcion}",
                                                    FechaAutorizacion = null
                                                });

                                                organizacionSuperior = await _organizacionService.GetOrganizacion((int)grupo.OrganizacionId!);
                                            }
                                        }


                                    }
                                    else
                                    {

                                        //son niveles superiores de autorizacion
                                        //se debe identificar el nivel superior de organizacion asociado al departamento o la organizacion siguiente
                                        fechaAutoriza = null;
                                        organizacionSuperior = jerarquiaGrupo.FirstOrDefault(e => e.NivelOrganizacionId == detalleFlujo.NivelOrganizacionId)!;

                                        if (organizacionSuperior is not null)
                                        {
                                            var responsableOrg = await _organizacionService.GetResponsableOrganizacion(organizacionSuperior.Id, detalleFlujo.OrdenPrioridad, fechaAutoriza);
                                            if (responsableOrg is not null)
                                            {
                                                listAutorizantes.Add(responsableOrg);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    fechaAutoriza = null;
                                    var responsableOrg = await _organizacionService.GetResponsableOrganizacion((int)detalleFlujo.OrganizacionEspecificaId, detalleFlujo.OrdenPrioridad, fechaAutoriza);
                                    if (responsableOrg is not null)
                                    {
                                        listAutorizantes.Add(responsableOrg);
                                    }


                                }

                            }


                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return listAutorizantes;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2025-03-31
        /// <summary>
        /// EjecutaAutorizacionSolicitud: Realiza proceso de autorización de una solicitud. Recibe una solicitud, se verifica si existe y actualiza el estado de aprobacion correspondiente
        /// </summary>
        /// <param name="solicitudesPorAprobar">Lista de solicitudes a aprobar</param>
        /// <param name="conFlujoAut">Indica si se debe realizar el flujo de autorización</param>
        /// <param name="notificaAutorizacion">Indica si se debe notificar la autorización</param>
        /// <returns>Instancia EventResponse con el resultado de la operación.</returns>
        private async Task<bool> EjecutaAutorizacionSolicitud(cSolicitud solicitudAutorizar, bool conFlujoAut, bool notificaAutorizacion)
        {
            EventResponse respuesta = new EventResponse();
            bool Autorizada = false;
            List<Email> correosPorEnviar = new();
            bool repiteAutorizante = false;
            bool EsAutorizanteAnterior = false;
            string idAutorizanteAnterior = "";
            List<cAutorizante>? listaAutorizantesPendientes = new();
            try
            {
                string? autorizanteActual = null;
                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();
                int vEstadoFinal = 99;


                cSolicitud? soli = await _context.Solicitudes
                                        .Include(e => e.cTipoSolicitud)
                                        .Include(e=> e.cSolicitudDetalle)
                                .Where(e => e.Id == solicitudAutorizar.Id)
                                .FirstOrDefaultAsync();

                if (soli is not null)
                {
                    var listAutorizantes = await GetAutorizantesSolicitud(soli);

                    listaAutorizantesPendientes = listAutorizantes.Where(e => e.FechaAutorizacion == null).OrderBy(e => e.IdNivelAutorizacion).ToList();

                    foreach (var autorizante in listaAutorizantesPendientes)
                    {
                        if (autorizanteActual == null)
                        {
                            autorizanteActual = autorizante.IdNumero;

                            if (autorizanteActual != solicitudAutorizar.IdNumero)
                                autorizanteActual = solicitudAutorizar.IdNumero;
                        }
                        else
                        {
                            if (autorizanteActual == autorizante.IdNumero)
                            {
                                notificaAutorizacion = false;
                                repiteAutorizante = true;
                            }
                            else
                            {

                                var autorizanteAnterior = listAutorizantes.FirstOrDefault(e => e.IdNumero == autorizante.IdNumero && e.FechaAutorizacion != null);

                                //El autorizante ya ha realizado una autorización en esta solicitud
                                if (autorizanteAnterior != null)
                                {
                                    EsAutorizanteAnterior = true;
                                    idAutorizanteAnterior = autorizante.IdNumero;
                                    notificaAutorizacion = false;
                                }
                                else
                                {
                                    EsAutorizanteAnterior = false;
                                    notificaAutorizacion = true;
                                }

                            }
                            break;
                        }
                    }

                    //cNivelAutorizacion? nivelAutorizacion = await _context.NivelesAutorizacion
                    //                                            .FirstOrDefaultAsync(e => e.Id == incidenciaPermitida!.NivelAutorizacionId);



                    int organizacionPadre = 0;

                    List<cOrganizacion> jerarquiaGrupo = new();
                    var grupo = await _geoServices.GetGrupo((int)soli.IdGrupo!);

                    var solicitante = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == soli.IdNumero!);

                    organizacionPadre = grupo.OrganizacionId == null ? -1 : (int)grupo.OrganizacionId;
                    cOrganizacion? organizacion = await _organizacionService.GetOrganizacion(organizacionPadre);

                    if (organizacion is not null)
                        jerarquiaGrupo.Add(organizacion);

                    while (organizacion!.OrganizacionSuperior != 0)
                    {
                        organizacion = await _organizacionService.GetOrganizacion(organizacion.OrganizacionSuperior);
                        if (organizacion is not null)
                            jerarquiaGrupo.Add(organizacion);
                    }

                    var flujoAutorizacion = await _flujosServices.GetFlujoAutorizacion(soli.cTipoSolicitud!.FlujoAutorizacionId);
                    if (flujoAutorizacion is not null)
                    {
                        var detFlujo = flujoAutorizacion!.cFlujoAutorizacionDetalle!.FirstOrDefault(e => e.EstadoAnteriorId == soli.EstadoId);

                        if (detFlujo is not null)
                        {
                            soli.EstadoId = detFlujo.EstadoNuevoId;

                            var detFlujoSiguiente = flujoAutorizacion!.cFlujoAutorizacionDetalle!.FirstOrDefault(e => e.EstadoAnteriorId == detFlujo.EstadoNuevoId);

                            if (detFlujoSiguiente is not null)
                            {
                                if (detFlujoSiguiente.OrganizacionEspecificaId is null)
                                {
                                    organizacion = jerarquiaGrupo.FirstOrDefault(e => e.NivelOrganizacionId == detFlujoSiguiente.NivelOrganizacionId);
                                    if (notificaAutorizacion)
                                    {
                                        if (organizacion is not null)
                                        {
                                            var autorizadorResponsable = organizacion.cOrganizacionResponsable!.OrderBy(e => e.OrdenJerarquia).FirstOrDefault();
                                            if (autorizadorResponsable is not null)
                                            {
                                                var autorizante = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == autorizadorResponsable.IdNumeroResponsable);

                                                if (autorizante is not null)
                                                {

                                                    var correo = await EnviaCorreoSolicitudAutorización(autorizante, soli.Id, solicitante!, soli.cTipoSolicitud!);
                                                    if (correo is not null)
                                                        correosPorEnviar.Add(correo);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    organizacion = await _organizacionService.GetOrganizacion((int)detFlujoSiguiente.OrganizacionEspecificaId);
                                    if (organizacion is not null)
                                    {
                                        if (notificaAutorizacion)
                                        {
                                            var autorizadorResponsable = organizacion.cOrganizacionResponsable!.OrderBy(e => e.OrdenJerarquia).FirstOrDefault();
                                            if (autorizadorResponsable is not null)
                                            {
                                                var autorizante = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == autorizadorResponsable.IdNumeroResponsable);

                                                if (autorizante is not null)
                                                {
                                                    var correo = await EnviaCorreoSolicitudAutorización(autorizante, soli.Id, solicitante!, soli.cTipoSolicitud!);
                                                    if (correo is not null)
                                                        correosPorEnviar.Add(correo);
                                                }
                                            }
                                        }
                                    }

                                }


                            }

                        }
                    }
                    

                    _context.Solicitudes.Update(soli);
                    await _context.SaveChangesAsync();

                    cSolicitudAutorizacion solicitudAut = new cSolicitudAutorizacion
                    {
                        SolicitudId = solicitudAutorizar.Id,
                        EstadoId = soli.EstadoId,
                        IdNumero = solicitudAutorizar.IdNumero!,
                        FechaRegistro = DateTime.Now,
                        Comentario = ""
                    };

                    _context.SolicitudesAutorizacion.Add(solicitudAut);
                    await _context.SaveChangesAsync();

                    if (soli.EstadoId == vEstadoFinal)
                    {
                        await AplicaSolitud(soli, solicitudAutorizar.IdNumero);

                        //enviar correo a solicitante de solicitud aprobada
                        List<Email> correosAprobacion = new();
                        var correoSolicitanteAprob = await EnviaCorreoSolicitudAutorizada(solicitante!, soli.Id);
                        if (correoSolicitanteAprob is not null) correosAprobacion.Add(correoSolicitanteAprob);
                        if (correosAprobacion.Count() > 0)
                            await _geoServices.EnviarCorreo(correosAprobacion);
                    }


                }

                if (!repiteAutorizante)
                {
                    if (!EsAutorizanteAnterior)
                    {
                        var listAutorizantes = await GetAutorizantesSolicitud(soli!);

                        var autorizanteSiguiente = listAutorizantes.Where(e => e.FechaAutorizacion == null).OrderBy(e => e.IdNivelAutorizacion).FirstOrDefault();

                        if (autorizanteSiguiente is not null)
                        {
                            if (autorizanteSiguiente!.IdNumero == soli!.IdUsuarioRegistra)
                            {
                                solicitudAutorizar.IdNumero = soli.IdUsuarioRegistra;
                                await EjecutaAutorizacionSolicitud(solicitudAutorizar, conFlujoAut, notificaAutorizacion);
                            }
                            else
                            {
                                if (correosPorEnviar.Count() > 0)
                                    await _geoServices.EnviarCorreo(correosPorEnviar);
                            }
                        }
                        else
                        {
                            if (correosPorEnviar.Count() > 0)
                                await _geoServices.EnviarCorreo(correosPorEnviar);
                        }

                    }
                    else
                    {
                        solicitudAutorizar.IdNumero = idAutorizanteAnterior;
                        await EjecutaAutorizacionSolicitud(soli, conFlujoAut, notificaAutorizacion);
                    }

                }
                else
                {
                    await EjecutaAutorizacionSolicitud(soli, conFlujoAut, notificaAutorizacion);
                }

                Autorizada = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
            }

            return Autorizada;

        }

        /// <summary>
        /// AutorizaSolicitud: Realiza proceso de autorización de solicitudes. Recibe una lista de solicitudes, se verifica si existe y actualiza el estado de aprobacion correspondiente
        /// </summary>
        /// <param name="solicitudesPorAprobar">Lista de solicitudes a aprobar</param>
        /// <returns>Instancia EventResponse con el resultado de la operación.</returns>
        public async Task<EventResponse> AutorizaSolicitud(IEnumerable<cSolicitud> solicitudesPorAprobar)
        {
            EventResponse respuesta = new EventResponse();
            List<long> IdsAutorizados = new();
            List<Email> correosPorEnviar = new();
            try
            {

                foreach (var solicitud in solicitudesPorAprobar)
                {
                    if (await EjecutaAutorizacionSolicitud(solicitud, true, false))
                    {
                        IdsAutorizados.Add(solicitud.Id);
                    }

                }
                respuesta.ValorRetorno = String.Join("|", IdsAutorizados);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.ValorRetorno = String.Join("|", IdsAutorizados);
                respuesta.Descripcion = $"No se pudo realizar el proceso de autorización de Solicitudes. Detalle de Error: {(e.InnerException == null ? e.Message : e.InnerException.Message)}";
            }

            return respuesta;

        }

        public async Task<EventResponse> ReAplicarSolicitudesAprobadas(string fechas)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var arrayFechas = fechas.Split('|');
                string FechaInicio = arrayFechas[0];
                string FechaFin = arrayFechas[1];
                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}");

                var solicitudesAprobadas = await _context.Solicitudes
                                                .Include(e => e.cTipoSolicitud)
                                                .Include(e => e.cSolicitudDetalle)
                                                .Where(e => e.EstadoId == 99
                                                        && e.FechaInicio >= fechaMovInicio
                                                        && e.FechaFin <= fechaMovFinal)
                                                .ToListAsync();
                foreach (var soli in solicitudesAprobadas)
                {
                    await AplicaSolitud(soli, soli.IdUsuarioRegistra!);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = $"ReAplicarSolicitudesAprobadas: No se pudo realizar el proceso de autorización de Solicitudes. Detalle de Error: {(e.InnerException == null ? e.Message : e.InnerException.Message)}";
            }

            return respuesta;

        }

        public async Task<EventResponse> ReAplicarSolicitudesAprobadasById(int Id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
               
                var solicitudesAprobadas = await _context.Solicitudes
                                                .Include(e => e.cTipoSolicitud)
                                                .Include(e => e.cSolicitudDetalle)
                                                .Where(e => e.EstadoId == 99
                                                        && e.Id == Id)
                                                .ToListAsync();
                foreach (var soli in solicitudesAprobadas)
                {
                    await AplicaSolitud(soli, soli.IdUsuarioRegistra!);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = $"ReAplicarSolicitudesAprobadas: No se pudo realizar el proceso de autorización de Solicitudes. Detalle de Error: {(e.InnerException == null ? e.Message : e.InnerException.Message)}";
            }

            return respuesta;

        }

        private async Task AplicaSolitud(cSolicitud soli, string autorizante)
        {
            try
            {
                cSolicitudConfiguracion? configuracion = await _context.SolicitudConfiguracion.FirstOrDefaultAsync(e => e.Id == soli.cTipoSolicitud!.TipoConfiguracion!)!;
                if (configuracion is not null)
                {
                    switch (configuracion.Destino)
                    {
                        case "HE": //Horas extras
                            await RegistrarHoraExtra(soli, autorizante, configuracion);
                            break;
                        case "DT": //distribución de tiempo
                            await RegistrarDistribucion(soli, autorizante, configuracion);
                            break;
                        case "TA": //tiempo adicional
                            var respuesta = await RegistrarTiempoAdicional(soli, autorizante,configuracion);                                
                            break;
                        case "DM": //tiempo adicional y Registro de marcas
                            await RegistrarDistribucionYMarcas(soli, autorizante, configuracion);
                            break;
                    }                   
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
            }

        }
        private async Task RegistrarHoraExtra(cSolicitud soli, string autorizante, cSolicitudConfiguracion configuracion)
        {
            if (!configuracion.MultipleCentroCosto)
            {
                List<cMarcaExtraApb> listaMarcasApb = new List<cMarcaExtraApb>() {
                new cMarcaExtraApb
                    {
                        fecha = soli.FechaInicio,
                        idnumero = soli.IdNumero,
                        idplanilla = soli.IdPlanilla,
                        cantidad = soli.TotalHoras,
                        estado = 'A',
                        usuario = soli.IdUsuarioRegistra,
                        hora = soli.HoraInicio,
                        ccosto = soli.IdCCosto == "0"? null : soli.IdCCosto,
                        cantidad_aprob_nivel1 = soli.TotalHoras,
                        aprob_nivel1 = 'T',
                        usuario_aprob_nivel1 = autorizante,
                        fecha_aprob_nivel1 = DateTime.Now,
                        comentario = soli.Comentario,
                        comentario_aprob_nivel1 = $"MarcasWeb: Solicitud No.{soli.Id}",
                    }
                };

                await _geoServices.Sincronizar_MarcaExtraApb(listaMarcasApb);

            }
            else
            {
                List<cMarcaExtraApb> listaMarcasApbMultiple = new List<cMarcaExtraApb>();
                foreach (var detalle in soli.cSolicitudDetalle!)
                {
                    listaMarcasApbMultiple.Add(new cMarcaExtraApb
                    {
                        fecha = detalle.Fecha,
                        idnumero = soli.IdNumero,
                        idplanilla = soli.IdPlanilla,
                        cantidad = detalle.TotalHoras,
                        estado = 'A',
                        usuario = autorizante,
                        hora = detalle.HoraInicio,
                        ccosto = detalle.IdCCosto == "0" ? null : detalle.IdCCosto,
                        cantidad_aprob_nivel1 = detalle.TotalHoras,
                        aprob_nivel1 = 'T',
                        usuario_aprob_nivel1 = autorizante,
                        fecha_aprob_nivel1 = DateTime.Now,
                        comentario = soli.Comentario,
                        comentario_aprob_nivel1 = $"MarcasWeb: Solicitud No.{soli.Id}",
                    });
                }
                await _geoServices.Sincronizar_MarcaExtraApb(listaMarcasApbMultiple);
            }
               

            
        }

        private async Task RegistrarDistribucion(cSolicitud soli, string autorizante, cSolicitudConfiguracion configuracion)
        {
            if (!configuracion.MultipleCentroCosto)
            {
                List<cMarcaDistribucionConcepto> listaMarcasDist = new List<cMarcaDistribucionConcepto>() {
                    new cMarcaDistribucionConcepto
                        {
                            IDREGISTRO = 0,
                            IDPLANILLA = soli.IdPlanilla,
                            IDNUMERO = soli.IdNumero,
                            FECHA = soli.FechaInicio,
                            IDCCOSTO = soli.IdCCosto!,
                            PROYECTO = soli.proyecto,
                            FASE = soli.fase,
                            CANTIDAD = soli.Cantidad,
                            INICIO = soli.HoraInicio,
                            FIN = soli.HoraFin,
                            ESTADO = 'A',
                            IDDIST = 0,
                            FECHA_DIST = DateTime.Now,
                            LON_REG = null,
                            LAT_REG = null,
                            COMENTARIO = $"MarcasWeb: Solicitud No {soli.Id}, autorizada por: {autorizante}. {soli.Comentario} ",
                            idsolicitud = soli.Id,

                        }
                };
                await _geoServices.Sincronizar_MarcaDtnConcepto(listaMarcasDist);
            }
            else
            {
                List<cMarcaDistribucionConcepto> listaMarcasDistMultiple = new List<cMarcaDistribucionConcepto>();
                foreach (var detalle in soli.cSolicitudDetalle!)
                {
                    listaMarcasDistMultiple.Add(new cMarcaDistribucionConcepto
                    {
                        IDREGISTRO = 0,
                        IDPLANILLA = soli.IdPlanilla,
                        IDNUMERO = soli.IdNumero,
                        FECHA = detalle.Fecha,
                        IDCCOSTO = detalle.IdCCosto!,
                        PROYECTO = detalle.Proyecto,
                        FASE = detalle.Fase,
                        CANTIDAD = detalle.Cantidad,
                        INICIO = detalle.HoraInicio,
                        FIN = detalle.HoraFin,
                        ESTADO = 'A',
                        IDDIST = 0,
                        FECHA_DIST = DateTime.Now,
                        LON_REG = null,
                        LAT_REG = null,
                        COMENTARIO = $"MarcasWeb: Solicitud No {soli.Id}, autorizada por: {autorizante}. {soli.Comentario} ",
                        idsolicitud = soli.Id,
                    });
                }
                await _geoServices.Sincronizar_MarcaDtnConcepto(listaMarcasDistMultiple);
            }
               
        }

        private async Task RegistrarDistribucionYMarcas(cSolicitud soli, string autorizante, cSolicitudConfiguracion configuracion)
        {

            try
            {
                var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == soli.IdNumero);
                var horarioEmpleado = await _context.Ph_Horario_Turnos
                                            .Where(e => e.IDHORARIO == empleado!.IdHorario).ToListAsync();
                var Turnos = await _context.Ph_Turnos.ToListAsync();

                List<cHorarioTurno> horarioTurnos = new List<cHorarioTurno>();

                for (int cont = 1; cont <= 7; cont++)
                {
                    var turnoDia = horarioEmpleado.FirstOrDefault(e => e.ID_DIA == cont);
                    if (turnoDia is not null)
                    {
                        var turno = Turnos.FirstOrDefault(e => e.IdTurno == turnoDia.T_1);

                        if (turno is not null)
                        {
                            horarioTurnos.Add(new cHorarioTurno
                            {
                                Dia = cont,
                                IdTurno = turno.IdTurno,
                                HoraInicio = turno.HEntra!,
                                HoraFin = turno.HSale!,
                            });
                        }
                    }
                }

                
                if (!configuracion.MultipleCentroCosto)
                {
                    int dia = Utility.DiaDeLaSemana(soli.FechaInicio);

                    var turnoAsignado = horarioTurnos.FirstOrDefault(e => e.Dia == dia);

                    if (turnoAsignado is not null)
                    {
                        TimeOnly horaInicioTurno = TimeOnly.Parse(turnoAsignado.HoraInicio!);

                        List<cMarcaDistribucionConcepto> listaMarcasDist = new List<cMarcaDistribucionConcepto>() {
                    new cMarcaDistribucionConcepto
                        {
                            IDREGISTRO = 0,
                            IDPLANILLA = soli.IdPlanilla,
                            IDNUMERO = soli.IdNumero,
                            FECHA = soli.FechaInicio,
                            IDCCOSTO = soli.IdCCosto!,
                            PROYECTO = soli.proyecto,
                            FASE = soli.fase,
                            CANTIDAD = soli.Cantidad,
                            INICIO = turnoAsignado!.HoraInicio,
                            FIN = horaInicioTurno.AddHours((double)soli.Cantidad).ToString("HH:mm"),
                            ESTADO = 'A',
                            IDDIST = 0,
                            FECHA_DIST = DateTime.Now,
                            LON_REG = null,
                            LAT_REG = null,
                            COMENTARIO = $"MarcasWeb: Solicitud No {soli.Id}, autorizada por: {autorizante}. {soli.Comentario} ",
                            idsolicitud = soli.Id,
                        }
                    };
                        await _geoServices.Sincronizar_MarcaDtnConcepto(listaMarcasDist);
                    }
                }
                else
                {
                    int diaActual = 0;
                    string horaInicio = "00:00";
                    string horaFin = "00:00";

                    List<cMarcaDistribucionConcepto> listaMarcasDistMultiple = new List<cMarcaDistribucionConcepto>();
                    foreach (var detalle in soli.cSolicitudDetalle!.OrderBy(e => e.Fecha))
                    {
                        int dia = Utility.DiaDeLaSemana(detalle.Fecha);
                        var turnoAsignado = horarioTurnos.FirstOrDefault(e => e.Dia == dia);

                        if (diaActual != dia)
                        {
                            horaInicio = turnoAsignado!.HoraInicio!;
                            TimeOnly horaInicioTurno = TimeOnly.Parse(horaInicio);
                            horaFin = horaInicioTurno.AddHours((double)detalle.Cantidad).ToString("HH:mm");
                            diaActual = dia;
                        }
                        else
                        {
                            TimeOnly horaInicioTurno = TimeOnly.Parse(horaFin);
                            horaInicio = horaFin;
                            horaFin = horaInicioTurno.AddHours((double)detalle.Cantidad).ToString("HH:mm");
                        }

                        listaMarcasDistMultiple.Add(new cMarcaDistribucionConcepto
                        {
                            IDREGISTRO = 0,
                            IDPLANILLA = soli.IdPlanilla,
                            IDNUMERO = soli.IdNumero,
                            FECHA = detalle.Fecha,
                            IDCCOSTO = detalle.IdCCosto!,
                            PROYECTO = detalle.Proyecto,
                            FASE = detalle.Fase,
                            CANTIDAD = detalle.Cantidad,
                            INICIO = horaInicio,
                            FIN = horaFin,
                            ESTADO = 'A',
                            IDDIST = 0,
                            FECHA_DIST = DateTime.Now,
                            LON_REG = null,
                            LAT_REG = null,
                            COMENTARIO = $"MarcasWeb: Solicitud No {soli.Id}, autorizada por: {autorizante}. {soli.Comentario} ",
                            idsolicitud = soli.Id,
                        });
                    }
                    await _geoServices.Sincronizar_MarcaDtnConcepto(listaMarcasDistMultiple);
                }

                //registrar marcas via horario del colaborador
                await RegistraMarcasSegunHorario(soli, configuracion, empleado!, horarioTurnos);

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");

            }


            

        }

        private async Task RegistraMarcasSegunHorario(cSolicitud soli, cSolicitudConfiguracion configuracion, cEmpleado empleado, List<cHorarioTurno> horarioTurnos)
        {
            try
            {
                List<cMarcaIn> listMarcaIn = new List<cMarcaIn>();

                List<DateTime> fechas = new List<DateTime>();
                if (configuracion.MultipleCentroCosto)
                {
                    fechas = soli.cSolicitudDetalle!.Select(e => e.Fecha).Distinct().ToList();
                }
                else
                {
                    fechas.Add(soli.FechaInicio);
                }

                foreach (var item in fechas)
                {
                    int dia = Utility.DiaDeLaSemana(item);
                    var turnoAsignado = horarioTurnos.FirstOrDefault(e => e.Dia == dia);

                    //Registrar marca de entrada
                    if (turnoAsignado is not null)
                    {
                        //verifica en marcasProceso si ya existe la marca para la fecha y hora
                        if (!await _context.Marcas_Proceso.AnyAsync(e => e.idnumero == empleado!.IdNumero
                                                                        && e.fecha_entra == DateTime.Parse(item.ToString("yyyy-MM-dd"))
                                                                        && e.hora_entra != "00:00"))
                        {
                            listMarcaIn = new List<cMarcaIn>()
                            { new cMarcaIn
                                {
                                    idtarjeta = empleado!.Tarjeta,
                                    fecha = DateTime.Parse(item.ToString("yyyy-MM-dd")),
                                    hora = turnoAsignado.HoraInicio,
                                    idterminal = "WPE",
                                    tipo = 1,
                                    idplanilla = empleado.IdPlanilla,
                                    idnumero = empleado.IdNumero,
                                    estado = 'N',
                                    fecha_reg = DateTime.Now,
                                    long_reg = null,
                                    lat_reg = null,
                                    imagen_reg = null,
                                    gps_cell = null,
                                    host = null,
                                    dir_ip = null,
                                }
                            };

                            await _geoServices.Sincronizar_MarcaIn(listMarcaIn);
                        }


                        if (!await _context.Marcas_Proceso.AnyAsync(e => e.idnumero == empleado!.IdNumero
                                                                        && e.fecha_entra == DateTime.Parse(item.ToString("yyyy-MM-dd"))
                                                                        && e.hora_sale != "00:00"))
                        {
                            listMarcaIn = new List<cMarcaIn>()
                                {
                                    new cMarcaIn
                                    {
                                        idtarjeta = empleado!.Tarjeta,
                                        fecha = DateTime.Parse(item.ToString("yyyy-MM-dd")),
                                        hora = turnoAsignado.HoraFin,
                                        idterminal = "WPE",
                                        tipo = 2,
                                        idplanilla = empleado.IdPlanilla,
                                        idnumero = empleado.IdNumero,
                                        estado = 'N',
                                        fecha_reg = DateTime.Now,
                                        long_reg = null,
                                        lat_reg = null,
                                        imagen_reg = null,
                                        gps_cell = null,
                                        host = null,
                                        dir_ip = null,
                                    }
                                };

                            await _geoServices.Sincronizar_MarcaIn(listMarcaIn);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                   
            }

        }

        private async Task<EventResponse> RegistrarTiempoAdicional(cSolicitud soli, string autorizante, cSolicitudConfiguracion configuracion)
        {
            EventResponse response = new EventResponse();

            var periodoActual = await _geoServices.GetPeriodoVigenteEmpleado(soli.IdNumero!, DateTime.Now.ToString("yyyyMMdd"));

            if (periodoActual is not null)
            {
                if (!configuracion.MultipleCentroCosto)
                {
                    List<cMarcaTiempoAdicional> listaMarcastiempoadicional = new List<cMarcaTiempoAdicional>() {
                        new cMarcaTiempoAdicional
                            {
                                IDREGISTRO = 0,
                                IDPLANILLA = soli.IdPlanilla,
                                IDNUMERO = soli.IdNumero,
                                FECHA_REFERENCIA = soli.FechaInicio,
                                CENTRO_COSTO = soli.IdCCosto!,
                                PROYECTO = soli.proyecto,
                                FASE = soli.fase,
                                CANTIDAD = soli.TotalHoras=="00:00"?Models.Utils.Utility.MunitosAHoras(soli.Cantidad):soli.TotalHoras,
                                FECHA_REGISTRO = DateOnly.FromDateTime(DateTime.Now),
                                ESTADO = 'A',
                                USUARIO = autorizante,
                                USUARIO_ACTUALIZA = autorizante,
                                FECHA_ACTUALIZA = DateOnly.FromDateTime(DateTime.Now),
                                IDCONCEPTO = (int)configuracion.Concepto!,
                                TCANTIDAD = soli.Cantidad,
                                COMENTARIO = $"MarcasWeb: Solicitud No {soli.Id}, autorizada por: {autorizante}. {soli.Comentario} ",
                                PERIODO = periodoActual.idperiodo,
                                idsolicitud = soli.Id,
                            }
                    };

                    await _geoServices.Sincronizar_MarcasTiempoAdicional(listaMarcastiempoadicional);
                }
                else
                {
                    
                    List<cMarcaTiempoAdicional> listaMarcastiempoadicionalMultiple = new List<cMarcaTiempoAdicional>();
                    foreach (var detalle in soli.cSolicitudDetalle!)
                    {
                        listaMarcastiempoadicionalMultiple.Add(new cMarcaTiempoAdicional
                        {
                            IDREGISTRO = 0,
                            IDPLANILLA = soli.IdPlanilla,
                            IDNUMERO = soli.IdNumero,
                            FECHA_REFERENCIA = detalle.Fecha,
                            CENTRO_COSTO = detalle.IdCCosto!,
                            PROYECTO = detalle.Proyecto,
                            FASE = detalle.Fase,
                            CANTIDAD = detalle.TotalHoras == "00:00" ? Models.Utils.Utility.MunitosAHoras(detalle.Cantidad) : detalle.TotalHoras,
                            FECHA_REGISTRO = DateOnly.FromDateTime(DateTime.Now),
                            ESTADO = 'A',
                            USUARIO = autorizante,
                            USUARIO_ACTUALIZA = autorizante,
                            FECHA_ACTUALIZA = DateOnly.FromDateTime(DateTime.Now),
                            IDCONCEPTO = (int)configuracion.Concepto!,
                            TCANTIDAD = detalle.Cantidad,
                            COMENTARIO = $"MarcasWeb: Solicitud No {soli.Id}, autorizada por: {autorizante}. {soli.Comentario} ",
                            PERIODO = periodoActual.idperiodo,
                            idsolicitud = soli.Id,
                        });
                    }
                    await _geoServices.Sincronizar_MarcasTiempoAdicional(listaMarcastiempoadicionalMultiple);
                }

            }
            else
            {
                response.Id = "1";
                response.Respuesta = "Error";
                response.Descripcion = $"No se pudo registrar el tiempo adicional, no se encontró un periodo vigente para el empleado {soli.IdNumero} en la fecha {soli.FechaInicio:yyyy-MM-dd}";

            }

            return response;


        }
    }

}
