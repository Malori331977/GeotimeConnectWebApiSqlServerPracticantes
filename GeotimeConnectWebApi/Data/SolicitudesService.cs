using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models.Utils;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class SolicitudesService: ISolicitudesService
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
                                 }
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
                                }
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
                                            }
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
                                           }
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
                var tiposSolicitudes = await _context.TiposSolicitudes.Where(e=>e.Activa==true).ToListAsync();
                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();
                int vEstadoFinal = 99;

                var solicitudesPendientes = await (from e in _context.Solicitudes
                                                        .Include(e => e.cSolicitudAutorizacion)
                                                        .Include(e => e.cEstado)
                                                        .Include(e => e.cTipoSolicitud)
                                                   .Where(e => e.EstadoId >= 1 && e.EstadoId < vEstadoFinal)
                                                   join i in _context.TiposSolicitudes on e.TipoSolicitudId equals i.Id
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
                                                           }

                                                   }).ToListAsync();


                int organizacionPadre = 0;


                foreach (var solicitud in solicitudesPendientes)
                {

                    var grupo = await _context.Ph_Grupos.FirstOrDefaultAsync(e=>e.idgrupo==solicitud.IdGrupo!);

                    if (grupo is not null)
                    {
                        var flujoAutorizacion = await _flujosServices.GetFlujoAutorizacion(solicitud.cTipoSolicitud!.FlujoAutorizacionId);

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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (solicitudesPorAprobar.Count() > 0)
                solicitudesPorAprobar = (solicitudesPorAprobar.Distinct()).ToList();

            return solicitudesPorAprobar!;
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
            try
            {
                foreach (var item in model)
                {
                    maxId = item.Id;
                    estadoId = item.EstadoId;
                    regNew = false;

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
                        modelBuscar.IdUsuarioRegistra = item.IdUsuarioRegistra;
                        modelBuscar.FechaRegistro = item.FechaRegistro;

                        _context.Solicitudes.Update(modelBuscar);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        regNew = true;
                        item.cEstado = null;
                        item.cTipoSolicitud = null;
                        item.cSolicitudAutorizacion = null;
                        item.Id = 0;
                        item.FechaRegistro = DateTime.Now;
                        _context.Add(item);
                        await _context.SaveChangesAsync();

                        maxId = await _context.Solicitudes.MaxAsync(e => e.Id);

                    }

                    respuesta.ValorRetorno = maxId.ToString();

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
                  
                    cSolicitud? modelBuscar = await _context.Solicitudes
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    if (modelBuscar is not null)
                    {                        
                        modelBuscar.EstadoId = item.EstadoId;

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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la solicitud. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la solicitud. Detalle de Error: " + e.InnerException.Message;

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
                
                    if (estadoId == 2)
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
                                var nivelautorizacion = flujoAutorizacion.cFlujoAutorizacionDetalle.FirstOrDefault(e => e.EstadoAnteriorId == 2);
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


                    email.Asunto = $"Solicitud de Autorización de {tipoSolicitud}.";
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
                        $"<html><head>Hola.</head>" +
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

        /// <summary>
        /// AutorizantesSolicitud: obtener lista de autorizantes de la solicitud
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <param name="incidenciaId"></param>
        /// <param name="IdNumero"></param>
        /// <param name="solicitudId"></param>
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
                var solicitudAut = await _context.SolicitudesAutorizacion.Where(e=>e.SolicitudId==solicitud.Id).ToListAsync();
                string? IdNumeroAutoriza = null;
                int organizacionPadre = 0;

                List<cOrganizacion> jerarquiaGrupo = new();

                organizacionPadre = grupo.OrganizacionId == null ? -1 : (int) grupo.OrganizacionId;
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
        public async Task<bool> EjecutaAutorizacionSolicitud(cSolicitud solicitudAutorizar, bool conFlujoAut, bool notificaAutorizacion)
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
               

                cSolicitud? soli = await _context.Solicitudes.Include(e=>e.cTipoSolicitud)
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

    }
}
