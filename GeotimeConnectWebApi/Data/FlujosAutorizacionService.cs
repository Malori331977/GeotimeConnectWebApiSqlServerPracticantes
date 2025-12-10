using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class FlujosAutorizacionService: IFlujosAutorizacionService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganizacionService _organizacionService;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<FlujosAutorizacionService> _logger;

        public FlujosAutorizacionService(SqlServerDataBaseContext context, IHttpContextAccessor httpContextAccessor, ILogger<FlujosAutorizacionService> logger, IOrganizacionService organizacionService)
        {
            _httpContextAccessor = httpContextAccessor;
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext!.User.Claims;
            string schema = "";
            string bdname = "";
            _logger = logger;
            _organizacionService = organizacionService;

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
            _organizacionService = organizacionService;
        }


        /// <summary>
        /// GetFlujoAutorizacion: obtener todos los registros de flujos de autorizacion
        /// </summary>
        /// <returns>Lista con todos los registros de flujos de autorizacion</returns>
        public async Task<List<cFlujoAutorizacion>> GetFlujoAutorizacion()
        {
            List<cFlujoAutorizacion> model = new();
            try
            {
                model = (from e in await _context.FlujosAutorizacion
                             .Include(e => e.cFlujoAutorizacionDetalle)
                              .ToListAsync()
                         select new cFlujoAutorizacion
                         {
                             Id = e.Id,
                             Descripcion = e.Descripcion,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             FechaModifica = e.FechaModifica,

                             cFlujoAutorizacionDetalle = e.cFlujoAutorizacionDetalle == null ? null :
                                (from fa in e.cFlujoAutorizacionDetalle
                                 select new cFlujoAutorizacionDetalle
                                 {
                                     FlujoAutorizacionId = fa.FlujoAutorizacionId,
                                     NivelOrganizacionId = fa.NivelOrganizacionId,
                                     EstadoAnteriorId = fa.EstadoAnteriorId,
                                     EstadoNuevoId = fa.EstadoNuevoId,
                                     EnvioSolicitud = fa.EnvioSolicitud,
                                     OrganizacionEspecificaId = fa.OrganizacionEspecificaId,
                                     IdUsuarioModifica = fa.IdUsuarioModifica,
                                     FechaModifica = fa.FechaModifica,
                                     OrdenPrioridad = fa.OrdenPrioridad,
                                     cOrganizacionNivel = _organizacionService.GetOrganizacionNivelById(fa.NivelOrganizacionId),
                                 }).ToList(),


                         }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetFlujoAutorizacion: Obtiene un registro de flujo de autorizacion 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de flujo de autorizacion  segun el id de parametro indicado</returns>
        public async Task<cFlujoAutorizacion> GetFlujoAutorizacion(int id)
        {
            cFlujoAutorizacion? model = new();
            try
            {
                model = (from e in await _context.FlujosAutorizacion
                             .Include(e => e.cFlujoAutorizacionDetalle!)
                             .Where(e => e.Id == id)
                              .ToListAsync()
                         select new cFlujoAutorizacion
                         {
                             Id = e.Id,
                             Descripcion = e.Descripcion,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             FechaModifica = e.FechaModifica,
                             cFlujoAutorizacionDetalle = e.cFlujoAutorizacionDetalle == null ? null :
                                (from fa in e.cFlujoAutorizacionDetalle
                                 select new cFlujoAutorizacionDetalle
                                 {
                                     FlujoAutorizacionId = fa.FlujoAutorizacionId,
                                     NivelOrganizacionId = fa.NivelOrganizacionId,
                                     EstadoAnteriorId = fa.EstadoAnteriorId,
                                     EstadoNuevoId = fa.EstadoNuevoId,
                                     EnvioSolicitud = fa.EnvioSolicitud,
                                     OrganizacionEspecificaId = fa.OrganizacionEspecificaId,
                                     IdUsuarioModifica = fa.IdUsuarioModifica,
                                     FechaModifica = fa.FechaModifica,
                                     OrdenPrioridad = fa.OrdenPrioridad,
                                     cOrganizacionNivel = _organizacionService.GetOrganizacionNivelById(fa.NivelOrganizacionId),
                                 }).ToList(),


                         }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return model!;
        }

        /// <summary>
        /// PostFlujoAutorizacion:  Recibe una lista de registros de Flujo Autorizacion, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostFlujoAutorizacion(IEnumerable<cFlujoAutorizacion> model)
        {
            EventResponse respuesta = new EventResponse();
            int maxId = 0;
            try
            {
                foreach (var item in model)
                {
                    cFlujoAutorizacion? itemEncontrado = await _context.FlujosAutorizacion
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    IEnumerable<cFlujoAutorizacionDetalle>? detalle = item.cFlujoAutorizacionDetalle;

                    if (itemEncontrado is not null)
                    {
                        maxId = item.Id;
                        itemEncontrado.Descripcion = item.Descripcion;
                        itemEncontrado.FechaModifica = DateTime.Now;
                        itemEncontrado.IdUsuarioModifica = item.IdUsuarioModifica;

                        _context.FlujosAutorizacion.Update(itemEncontrado);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        item.cFlujoAutorizacionDetalle = null;
                        item.Id = 0;
                        item.FechaModifica = DateTime.Now;
                        _context.Add(item);
                        await _context.SaveChangesAsync();

                        maxId = await _context.FlujosAutorizacion.MaxAsync(e => e.Id);
                    }

                    if (detalle is not null)
                        respuesta = await PostFlujoAutorizacionDetalle(detalle, maxId);

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del flujo de autorización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del flujo de autorización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// DeleteFlujoAutorizacion: Elimina un registro de flujo de autorización
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> DeleteFlujoAutorizacion(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cFlujoAutorizacion? model = await _context.FlujosAutorizacion
                                            .FirstOrDefaultAsync(e => e.Id.ToString() == id);

                if (model is not null)
                {
                    _context.FlujosAutorizacion.Remove(model);
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
        /// PostOrganizacionResponsable:  Recibe una lista de registros de PostOrganizacionResponsable, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostFlujoAutorizacionDetalle(IEnumerable<cFlujoAutorizacionDetalle> model, int flujoid)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in model)
                {
                    cFlujoAutorizacionDetalle? itemEncontrado = await _context.FlujosAutorizacionDetalle
                                                    .Where(e => e.FlujoAutorizacionId == flujoid
                                                            && e.NivelOrganizacionId == item.NivelOrganizacionId
                                                            && e.OrdenPrioridad == item.OrdenPrioridad)
                                                    .FirstOrDefaultAsync();

                    if (itemEncontrado is not null)
                    {
                        itemEncontrado.EstadoAnteriorId = item.EstadoAnteriorId;
                        itemEncontrado.EstadoNuevoId = item.EstadoNuevoId;
                        itemEncontrado.EnvioSolicitud = item.EnvioSolicitud;
                        itemEncontrado.OrganizacionEspecificaId = item.OrganizacionEspecificaId;
                        itemEncontrado.IdUsuarioModifica = item.IdUsuarioModifica;
                        itemEncontrado.FechaModifica = DateTime.Now;

                        _context.FlujosAutorizacionDetalle.Update(itemEncontrado);
                    }
                    else
                    {
                        item.cFlujoAutorizacion = null;
                        item.cOrganizacionNivel = null;
                        //item.cEstadoAnterior = null;
                        //item.cEstadoNuevo = null;

                        item.FechaModifica = DateTime.Now;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }


                var itemsActuales = await _context.FlujosAutorizacionDetalle
                                          .Where(e => e.FlujoAutorizacionId == flujoid).ToListAsync();

                foreach (var item in itemsActuales)
                {
                    var itemExiste = model.FirstOrDefault(e => e.NivelOrganizacionId == item.NivelOrganizacionId
                                                            && e.EstadoAnteriorId == item.EstadoAnteriorId
                                                            && e.EstadoNuevoId == item.EstadoNuevoId
                                                            && e.OrdenPrioridad == item.OrdenPrioridad);

                    if (itemExiste is null)
                    {
                        _context.FlujosAutorizacionDetalle.Remove(item);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización del detalle del flujo de Autorización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del detalle del flujo de Autorización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-11-10
        //Obtener lista de Estados de Solicitudes
        public async Task<List<cEstado>> GetEstado()
        {
            List<cEstado> estado = new();
            try
            {
                estado = await _context.Estados
                                       .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return estado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-11-10
        //Obtener un Estado especifico
        public async Task<cEstado> GetEstado(int id)
        {
            cEstado? estado = new();
            try
            {
                estado = await _context.Estados.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return estado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-12-23
        //Sincronizar  Estados
        //Parametro: Recibe un registro de Estado, se verifica si existen en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> PostEstado(cEstado estado)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cEstado? est = await _context.Estados
                                            .Where(e => e.Id == estado.Id)
                                            .FirstOrDefaultAsync();

                if (est is not null)
                {
                    est.Descripcion = estado.Descripcion;
                    est.FechaRegistro = DateTime.Now;
                    _context.Estados.Update(est);
                }
                else
                {
                    _context.Add(estado);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Estado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Estado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-12-23
        //Eliminar  Estados
        //Parametro: Recibe un registro de Estado, se verifica si existen en cuyo caso
        //se elimina.
        public async Task<EventResponse> DeleteEstado(string estadoid)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cEstado? est = await _context.Estados
                                            .Where(e => e.Id == int.Parse(estadoid))
                                            .FirstOrDefaultAsync();

                if (est is not null)
                {
                    _context.Estados.Remove(est);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Estado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Estado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


       
    }
}
