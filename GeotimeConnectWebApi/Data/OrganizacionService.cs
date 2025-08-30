using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class OrganizacionService:IOrganizacionService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<OrganizacionService> _logger;

        public OrganizacionService(SqlServerDataBaseContext context, IHttpContextAccessor httpContextAccessor, ILogger<OrganizacionService> logger)
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
        /// GetOrganizacionNivel: obtener todos los registros de Organizacion Niveles
        /// </summary>
        /// <returns>Lista con todos los registros de Organizacion Niveles</returns>
        public async Task<List<cOrganizacionNivel>> GetOrganizacionNivel()
        {
            List<cOrganizacionNivel> model = new();
            try
            {
                model = await _context.OrganizacionNiveles
                                       .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetOrganizacionNivel: Obtiene un registro de Organizacion Niveles
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de Organizacion Niveles segun el id de parametro indicado</returns>
        public async Task<cOrganizacionNivel> GetOrganizacionNivel(string id)
        {
            cOrganizacionNivel? model = new();
            try
            {
                model = await _context.OrganizacionNiveles.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return model!;
        }

        /// <summary>
        /// GetOrganizacionNivel: Obtiene un registro de Organizacion Niveles
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de Organizacion Niveles segun el id de parametro indicado</returns>
        public cOrganizacionNivel GetOrganizacionNivelById(string id)
        {
            cOrganizacionNivel? model = new();
            try
            {
                model = (from e in _context.OrganizacionNiveles
                            .Where(e => e.Id == id)
                         select new cOrganizacionNivel
                         {
                             Id = e.Id,
                             Descripcion = e.Descripcion,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             FechaModifica = e.FechaModifica,
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
        /// PostOrganizacionNivel:  Recibe un registro de OrganizacionNivel, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostOrganizacionNivel(IEnumerable<cOrganizacionNivel> model)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in model)
                {
                    cOrganizacionNivel? itemEncontrado = await _context.OrganizacionNiveles
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    if (itemEncontrado is not null)
                    {
                        itemEncontrado.Descripcion = item.Descripcion;
                        itemEncontrado.FechaModifica = DateTime.Now;
                        itemEncontrado.IdUsuarioModifica = item.IdUsuarioModifica;


                        _context.OrganizacionNiveles.Update(itemEncontrado);
                    }
                    else
                    {
                        item.FechaModifica = DateTime.Now;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Nivel de Organización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Nivel de Organización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// DeleteOrganizacionNivel: Elimina un registro de Nivel de Organización
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> DeleteOrganizacionNivel(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cOrganizacionNivel? model = await _context.OrganizacionNiveles
                                            .FirstOrDefaultAsync(e => e.Id == id);

                if (model is not null)
                {
                    _context.OrganizacionNiveles.Remove(model);
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Nivel de Organización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el  Nivel de Organización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetOrganizacion: obtener todos los registros de Organizacion
        /// </summary>
        /// <returns>Lista con todos los registros de Organizacion</returns>
        public async Task<List<cOrganizacion>> GetOrganizacion()
        {
            List<cOrganizacion> model = new();
            try
            {
                model = (from e in await _context.Organizacion
                             .Include(e => e.cOrganizacionNivel)
                             .Include(e => e.cOrganizacionResponsable)
                              .ToListAsync()
                         select new cOrganizacion
                         {
                             Id = e.Id,
                             Descripcion = e.Descripcion,
                             NivelOrganizacionId = e.NivelOrganizacionId,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             FechaModifica = e.FechaModifica,
                             OrganizacionSuperior = e.OrganizacionSuperior,
                             cOrganizacionNivel = e.cOrganizacionNivel == null ? null :
                                new cOrganizacionNivel
                                {
                                    Id = e.cOrganizacionNivel.Id,
                                    Descripcion = e.cOrganizacionNivel.Descripcion,
                                    IdUsuarioModifica = e.cOrganizacionNivel.IdUsuarioModifica,
                                    FechaModifica = e.cOrganizacionNivel.FechaModifica,
                                },
                             cOrganizacionResponsable = e.cOrganizacionResponsable == null ? null :
                                (from d in e.cOrganizacionResponsable
                                 select new cOrganizacionResponsable
                                 {
                                     OrganizacionId = d.OrganizacionId,
                                     OrdenJerarquia = d.OrdenJerarquia,
                                     IdNumeroResponsable = d.IdNumeroResponsable,
                                     IdUsuarioModifica = d.IdUsuarioModifica,
                                     FechaModifica = d.FechaModifica,
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
        /// GetOrganizacion: Obtiene un registro de Organizacion 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de Organizacion segun el id de parametro indicado</returns>
        public async Task<cOrganizacion> GetOrganizacion(int id)
        {
            cOrganizacion? model = new();
            try
            {
                model = (from e in await _context.Organizacion
                             .Include(e => e.cOrganizacionNivel)
                             .Include(e => e.cOrganizacionResponsable)
                             .Where(e => e.Id == id)
                              .ToListAsync()
                         select new cOrganizacion
                         {
                             Id = e.Id,
                             Descripcion = e.Descripcion,
                             NivelOrganizacionId = e.NivelOrganizacionId,
                             IdUsuarioModifica = e.IdUsuarioModifica,
                             FechaModifica = e.FechaModifica,
                             OrganizacionSuperior = e.OrganizacionSuperior,
                             cOrganizacionNivel = e.cOrganizacionNivel == null ? null :
                                new cOrganizacionNivel
                                {
                                    Id = e.cOrganizacionNivel.Id,
                                    Descripcion = e.cOrganizacionNivel.Descripcion,
                                    IdUsuarioModifica = e.cOrganizacionNivel.IdUsuarioModifica,
                                    FechaModifica = e.cOrganizacionNivel.FechaModifica,
                                },
                             cOrganizacionResponsable = e.cOrganizacionResponsable == null ? null :
                                (from d in e.cOrganizacionResponsable
                                 select new cOrganizacionResponsable
                                 {
                                     OrganizacionId = d.OrganizacionId,
                                     OrdenJerarquia = d.OrdenJerarquia,
                                     IdNumeroResponsable = d.IdNumeroResponsable,
                                     IdUsuarioModifica = d.IdUsuarioModifica,
                                     FechaModifica = d.FechaModifica,
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
        /// PostOrganizacion:  Recibe un registro de Organizacion, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostOrganizacion(IEnumerable<cOrganizacion> model)
        {
            EventResponse respuesta = new EventResponse();
            int maxId = 0;
            try
            {
                foreach (var item in model)
                {
                    cOrganizacion? itemEncontrado = await _context.Organizacion
                                                    .Where(e => e.Id == item.Id)
                                                    .FirstOrDefaultAsync();

                    IEnumerable<cOrganizacionResponsable>? responsables = item.cOrganizacionResponsable;

                    if (itemEncontrado is not null)
                    {
                        maxId = item.Id;
                        itemEncontrado.Descripcion = item.Descripcion;
                        itemEncontrado.NivelOrganizacionId = item.NivelOrganizacionId;
                        itemEncontrado.FechaModifica = DateTime.Now;
                        itemEncontrado.IdUsuarioModifica = item.IdUsuarioModifica;
                        itemEncontrado.OrganizacionSuperior = item.OrganizacionSuperior;

                        _context.Organizacion.Update(itemEncontrado);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        item.cOrganizacionNivel = null;
                        item.cOrganizacionResponsable = null;
                        //item.cDepartamento = null;

                        item.Id = 0;
                        item.FechaModifica = DateTime.Now;
                        _context.Add(item);
                        await _context.SaveChangesAsync();

                        maxId = await _context.Organizacion.MaxAsync(e => e.Id);
                    }

                    if (responsables is not null)
                        respuesta = await PostOrganizacionResponsable(responsables, maxId);

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la Organización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la Organización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// DeleteOrganizacionNivel: Elimina un registro de Nivel de Organización
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> DeleteOrganizacion(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cOrganizacion? model = await _context.Organizacion
                                            .FirstOrDefaultAsync(e => e.Id.ToString() == id);

                if (model is not null)
                {
                    _context.Organizacion.Remove(model);
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Organización. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Organización. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetOrganizacionResponsable: Obtiene lista de responsables por organizacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>lista de responsables por organizacion segun el id de parametro indicado</returns>
        public async Task<IEnumerable<cOrganizacionResponsable>> GetOrganizacionResponsable(int id)
        {
            List<cOrganizacionResponsable>? model = new();
            try
            {
                model = await _context.OrganizacionResponsables
                             .Where(e => e.OrganizacionId == id)
                              .ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return model!;
        }

        public async Task<cAutorizante> GetResponsableOrganizacion(int OrganizacionId, int ordenPrioridad, DateTime? fechaAutoriza)
        {
            cAutorizante? autorizante = null;

            //se obtiene responsable de la organizacion
            var organizacion = await GetOrganizacion((int)OrganizacionId);

            if (organizacion is not null)
            {
                if (organizacion.cOrganizacionResponsable is not null)
                {
                    var responsableOrg = organizacion.cOrganizacionResponsable!.OrderBy(e => e.OrdenJerarquia).FirstOrDefault();

                    if (responsableOrg is not null)
                    {
                        autorizante = new cAutorizante
                        {
                            IdNumero = responsableOrg!.IdNumeroResponsable,
                            IdNivelAutorizacion = Models.Utils.Utility.Right($"00{ordenPrioridad}", 2),
                            DescNivelAutorizacion = "Responsable de " + organizacion.Descripcion,
                            FechaAutorizacion = fechaAutoriza
                        };
                    }
                }
            }
            return autorizante!;
        }

        /// <summary>
        /// PostOrganizacionResponsable:  Recibe una lista de registros de PostOrganizacionResponsable, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostOrganizacionResponsable(IEnumerable<cOrganizacionResponsable> model, int OrganizacionId)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in model)
                {
                    cOrganizacionResponsable? itemEncontrado = await _context.OrganizacionResponsables
                                                    .Where(e => e.OrganizacionId == OrganizacionId
                                                            && e.OrdenJerarquia == item.OrdenJerarquia)
                                                    .FirstOrDefaultAsync();

                    if (itemEncontrado is not null)
                    {
                        itemEncontrado.IdNumeroResponsable = item.IdNumeroResponsable;
                        itemEncontrado.IdUsuarioModifica = item.IdUsuarioModifica;
                        itemEncontrado.FechaModifica = DateTime.Now;


                        _context.OrganizacionResponsables.Update(itemEncontrado);
                    }
                    else
                    {
                        item.cOrganizacion = null;
                        item.FechaModifica = DateTime.Now;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }

                var ListRevisar = await _context.OrganizacionResponsables
                                        .Where(e => e.OrganizacionId == OrganizacionId)
                                        .ToListAsync();

                foreach (var item in ListRevisar)
                {
                    if (!model.Any(e => e.OrdenJerarquia == item.OrdenJerarquia))
                    {
                        _context.OrganizacionResponsables.Remove(item);
                    }
                }
                await _context.SaveChangesAsync();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la Organización y Responsables. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la Organización y Responsables. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetOrganizacionBaseResponsable: Obtiene lista de responsables por organizacion base
        /// </summary>
        /// <param name="id"></param>
        /// <returns>lista de responsables por organizacion segun el id de parametro indicado</returns>
        public async Task<IEnumerable<cOrganizacionBaseResponsable>> GetOrganizacionBaseResponsable(string id)
        {
            List<cOrganizacionBaseResponsable>? model = new();
            try
            {
                model = await _context.OrganizacionBaseResponsables
                             .Where(e => e.IdOrgBase == id)
                              .ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return model!;
        }

        /// <summary>
        /// PostOrganizacionBaseResponsable:  Recibe una lista de registros de Responsables de la organización, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public async Task<EventResponse> PostOrganizacionBaseResponsable(IEnumerable<cOrganizacionBaseResponsable> model)
        {
            EventResponse respuesta = new EventResponse();
            string orgBase = "";
            try
            {
                foreach (var item in model)
                {
                    orgBase = item.IdOrgBase;
                    cOrganizacionBaseResponsable? itemEncontrado = await _context.OrganizacionBaseResponsables
                                                    .Where(e => e.IdOrgBase == item.IdOrgBase
                                                            && e.OrdenJerarquia == item.OrdenJerarquia)
                                                    .FirstOrDefaultAsync();

                    if (itemEncontrado is not null)
                    {
                        itemEncontrado.IdNumeroResponsable = item.IdNumeroResponsable;
                        itemEncontrado.IdUsuarioModifica = item.IdUsuarioModifica;
                        itemEncontrado.FechaModifica = DateTime.Now;


                        _context.OrganizacionBaseResponsables.Update(itemEncontrado);
                    }
                    else
                    {
                        item.FechaModifica = DateTime.Now;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }

                var ListRevisar = await _context.OrganizacionBaseResponsables
                                        .Where(e => e.IdOrgBase == orgBase)
                                        .ToListAsync();

                foreach (var item in ListRevisar)
                {
                    if (!model.Any(e => e.OrdenJerarquia == item.OrdenJerarquia))
                    {
                        _context.OrganizacionBaseResponsables.Remove(item);
                    }
                }
                await _context.SaveChangesAsync();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la organización base y sus Responsables. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la organización base y sus Responsables. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }



        //Creado por: Marlon Loria Solano
        //Fecha: 2023-12-05
        //Obtener lista de empleados y sus jefaturas responsables
        public async Task<List<cEmpleadoJefatura>> GetEmpleadoJefatura()
        {
            List<cEmpleadoJefatura> empleadoJefatura = new();
            try
            {
                empleadoJefatura = await _context.EmpleadosJefaturas
                                       .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return empleadoJefatura;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-12-05
        //Obtener un empleado y sus jefaturas 
        public async Task<cEmpleadoJefatura> GetEmpleadoJefatura(string id)
        {
            cEmpleadoJefatura? empleadoJefatura = new();
            try
            {
                empleadoJefatura = await _context.EmpleadosJefaturas.FirstOrDefaultAsync(e => e.IdNumero == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return empleadoJefatura!;
        }

        /// <summary>
        /// GetEmpleadoJefatura: muestra lista de empledos donde el id figura con autorizante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="esJefe"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cEmpleadoJefatura>> GetEmpleadoJefatura(string id, bool esJefe)
        {
            List<cEmpleadoJefatura>? empleadoJefatura = new();
            try
            {
                if (esJefe)
                    empleadoJefatura = await _context.EmpleadosJefaturas.Where(e => e.IdNumeroResponsable == id || e.IdNumeroSuplente == id)
                                        .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return empleadoJefatura!;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-12-05
        //Sincronizar Empleados y Jefaturas
        //Parametro: Recibe un registro de EmpleadoJefatura, se verifica si existen en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> PostEmpleadoJefatura(IEnumerable<cEmpleadoJefatura> empleadosJefaturas)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in empleadosJefaturas)
                {
                    cEmpleadoJefatura? empJefatura = await _context.EmpleadosJefaturas
                                            .Where(e => e.IdNumero == item.IdNumero)
                                            .FirstOrDefaultAsync();

                    if (empJefatura is not null)
                    {
                        empJefatura.FechaUltModifica = DateTime.Now;
                        empJefatura.IdNumeroResponsable = item.IdNumeroResponsable;
                        empJefatura.IdNumeroSuplente = item.IdNumeroSuplente;

                        _context.EmpleadosJefaturas.Update(empJefatura);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        item.FechaUltModifica = DateTime.Now;
                        _context.Add(item);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados y Jefaturas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados y Jefaturas. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-12-05
        //Eliminar Empleado Jefatura
        //Parametro: Recibe un registro de cEmpleadoJefatura, se verifica si existen en cuyo caso
        //se elimina.
        public async Task<EventResponse> DeleteEmpleadoJefatura(string id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cEmpleadoJefatura? aux = await _context.EmpleadosJefaturas
                                            .Where(e => e.IdNumero == id)
                                            .FirstOrDefaultAsync();

                if (aux is not null)
                {
                    _context.EmpleadosJefaturas.Remove(aux);
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Empleado y sus Jefaturas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Empleado y sus Jefaturas. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }



    }
}
