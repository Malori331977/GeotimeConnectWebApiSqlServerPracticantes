using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class RelojesServices : IRelojesServices
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<RelojesServices> _logger;
        private string InterfaceName = "RelojesServices";

        public RelojesServices(IHttpContextAccessor httpContextAccessor, 
                                     ILogger<RelojesServices> logger)
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
        /// GetRelojDispositivoAdm:  Obtine lista de dispositivos registrados 
        /// </summary>
        /// <returns>Lista de dispositivos registrados</returns>
        public async Task<IEnumerable<cRelojDispositivoAdmin>> GetRelojDispositivoAdmin()
        {
            List<cRelojDispositivoAdmin> model = new();

            try
            {
                model = await _context.RelojDispositivoAdmin.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojDispositivoAdm: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                 throw;
            }
            return model;
        }

        /// <summary>
        /// GetRelojDispositivoAdm:  Obtine lista de dispositivos registrados por compañia y estado
        /// </summary>
        /// <param name="compania"</param>
        /// <param name="estado"></param>
        /// <returns>Lista de dispositivos registrados</returns>
        public async Task<IEnumerable<cRelojDispositivoAdmin>> GetRelojDispositivoAdmin(string compania, string estado)
        {
            List<cRelojDispositivoAdmin> model = new();

            try
            {
                model = await _context.RelojDispositivoAdmin.Where(e=>e.IDCOMP==compania 
                                                                   && e.CLOCK_STATE==(estado=="-1"?e.CLOCK_STATE: estado))
                                                            .ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojDispositivoAdm: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetRelojDispositivoAdmin:  obtiene los datos de un dispositivo especifico de acuerdo al Id de dispositivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<cRelojDispositivoAdmin> GetRelojDispositivoAdmin(string id)
        {
            cRelojDispositivoAdmin? model = new();

            try
            {
                model = await _context.RelojDispositivoAdmin.FirstOrDefaultAsync(e => e.CLOCK_SERIE == id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojDispositivoAdm: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model!;
        }

        /// <summary>
        /// PostRelojDispositivoAdmin: metodo crear o modificar uno o varios registros de Relojes_Dispositivos en esquema ctadmin 
        /// </summary>
        /// <param name="relojes"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public async Task<EventResponse> PostRelojDispositivoAdmin(IEnumerable<cRelojDispositivoAdmin> relojes)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in relojes)
                {
                    cRelojDispositivoAdmin? objetoBuscar = await _context.RelojDispositivoAdmin
                        .FirstOrDefaultAsync(e => e.CLOCK_ID == item.CLOCK_ID || e.CLOCK_SERIE==item.CLOCK_SERIE);

                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.CLOCK_DESCRIPTION = item.CLOCK_DESCRIPTION;
                        objetoBuscar.CLOCK_IP = item.CLOCK_IP;
                        objetoBuscar.CLOCK_PORT = item.CLOCK_PORT;
                        objetoBuscar.CLOCK_STATE = item.CLOCK_STATE;
                        objetoBuscar.CLOCK_COMM = item.CLOCK_COMM;
                        objetoBuscar.CLOCK_COMMID = item.CLOCK_COMMID;
                        objetoBuscar.SITE_ID = item.SITE_ID;
                        objetoBuscar.CLOCK_TYPE = item.CLOCK_TYPE;
                        objetoBuscar.CLOCK_USE_FUNCTION = item.CLOCK_USE_FUNCTION;
                        objetoBuscar.CLOCK_FUNCTION = item.CLOCK_FUNCTION;
                        objetoBuscar.CLOCK_PASS = item.CLOCK_PASS;
                        objetoBuscar.BORRO_M = item.BORRO_M;
                        objetoBuscar.IDLOCACION = item.IDLOCACION;
                        objetoBuscar.FG_MODEL = item.FG_MODEL;
                        objetoBuscar.USA_FACE = item.USA_FACE;
                        objetoBuscar.USUARIO_HIK = item.USUARIO_HIK;
                        objetoBuscar.PASSWORD_HIK = item.PASSWORD_HIK;
                        objetoBuscar.CLOCK_SERIE = item.CLOCK_SERIE;
                        objetoBuscar.IDCOMP = item.IDCOMP;
                        
                        _context.RelojDispositivoAdmin.Update(objetoBuscar);
                    }
                    else
                    {

                        // crear un nuevo registro si no existe
                        item.CLOCK_ID = 0;
                        await _context.RelojDispositivoAdmin.AddAsync(item);
                    }

                    await _context.SaveChangesAsync();
                }
               
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.PostRelojDispositivoAdmin: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);

                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = error;
            }
            return respuesta;
        }



        /// <summary>
        /// GetRelojDispositivo:  Obtine lista de dispositivos registrados en una compañia
        /// </summary>
        /// <returns>Lista de dispositivos registrados</returns>
        public async Task<IEnumerable<cRelojDispositivo>> GetRelojDispositivo()
        {
            List<cRelojDispositivo> model = new();

            try
            {
                model = await _context.RelojDispositivo.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojDispositivo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetRelojDispositivo:  obtiene los datos de un dispositivo especifico de acuerdo al Id de dispositivo en una compañia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<cRelojDispositivo> GetRelojDispositivo(int id)
        {
            cRelojDispositivo? model = new();

            try
            {
                model = await _context.RelojDispositivo.FirstOrDefaultAsync(e => e.CLOCK_ID == id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojDispositivo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model!;
        }

        /// <summary>
        /// PostRelojDispositivo: metodo crear o modificar uno o varios registros de Relojes_Dispositivos en esquema de la compañia 
        /// </summary>
        /// <param name="relojes"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public async Task<EventResponse> PostRelojDispositivo(IEnumerable<cRelojDispositivo> relojes)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in relojes)
                {
                    cRelojDispositivo? objetoBuscar = await _context.RelojDispositivo.FirstOrDefaultAsync(e => e.CLOCK_SERIE == item.CLOCK_SERIE);

                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.CLOCK_DESCRIPTION = item.CLOCK_DESCRIPTION;
                        objetoBuscar.CLOCK_IP = item.CLOCK_IP;
                        objetoBuscar.CLOCK_PORT = item.CLOCK_PORT;
                        objetoBuscar.CLOCK_STATE = item.CLOCK_STATE;
                        objetoBuscar.CLOCK_COMM = item.CLOCK_COMM;
                        objetoBuscar.CLOCK_COMMID = item.CLOCK_COMMID;
                        objetoBuscar.SITE_ID = item.SITE_ID;
                        objetoBuscar.CLOCK_TYPE = item.CLOCK_TYPE;
                        objetoBuscar.CLOCK_USE_FUNCTION = item.CLOCK_USE_FUNCTION;
                        objetoBuscar.CLOCK_FUNCTION = item.CLOCK_FUNCTION;
                        objetoBuscar.CLOCK_PASS = item.CLOCK_PASS;
                        objetoBuscar.BORRO_M = item.BORRO_M;
                        objetoBuscar.IDLOCACION = item.IDLOCACION;
                        objetoBuscar.FG_MODEL = item.FG_MODEL;
                        objetoBuscar.USA_FACE = item.USA_FACE;
                        objetoBuscar.USUARIO_HIK = item.USUARIO_HIK;
                        objetoBuscar.PASSWORD_HIK = item.PASSWORD_HIK;
                        objetoBuscar.ALERTA_MASCARILLA = item.ALERTA_MASCARILLA;
                        objetoBuscar.ALERTA_TEMPERATURA = item.ALERTA_TEMPERATURA;
                        objetoBuscar.CORTE_TEMPERATURA = item.CORTE_TEMPERATURA;
                        objetoBuscar.DIRECCIONES_ALERTA = item.DIRECCIONES_ALERTA;
                        objetoBuscar.ACC = item.ACC;
                        objetoBuscar.ULTIMO_ESTADO = item.ULTIMO_ESTADO;

                        _context.RelojDispositivo.Update(objetoBuscar);
                    }
                    else
                    {
                        // crear un nuevo registro si no existe
                        item.CLOCK_ID = 0;
                        await _context.RelojDispositivo.AddAsync(item);
                    }

                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.PostRelojDispositivo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);

                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = error;
            }
            return respuesta;
        }


        /// <summary>
        /// GetRelojUsuario:  Obtiene lista de usuarios registrados en relojes de una compañia
        /// </summary>
        /// <returns>Lista de usuarios registrados en relojes</returns>
        public async Task<IEnumerable<cRelojUsuario>> GetRelojUsuario()
        {
            List<cRelojUsuario> model = new();

            try
            {
                model = await _context.RelojUsuario.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.RelojUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetRelojUsuario:  Obtiene un usuario asociado los relojes de una compañia 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<cRelojUsuario> GetRelojUsuario(string id)
        {
            cRelojUsuario? model = new();
            try
            {
                model = await _context.RelojUsuario.FirstOrDefaultAsync(e => e.FP_ENROLLID == id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.RelojUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model!;
        }

        /// <summary>
        /// PostRelojUsuario: metodo crear o modificar uno o varios registros de usuarios de Relojes en esquema de la compañia 
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public async Task<EventResponse> PostRelojUsuario(IEnumerable<cRelojUsuario> usuarios)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in usuarios)
                {
                    cRelojUsuario? objetoBuscar = await _context.RelojUsuario.FirstOrDefaultAsync(e => e.FP_ENROLLID == item.FP_ENROLLID);

                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.FP_USERNAME = item.FP_USERNAME;
                        objetoBuscar.FP_USERPASS = item.FP_USERPASS;
                        objetoBuscar.FP_PRIVILEGIO = item.FP_PRIVILEGIO;
                        objetoBuscar.FP_ESTADO = item.FP_ESTADO;
                        objetoBuscar.FP_TARJETA = item.FP_TARJETA;
                        objetoBuscar.FP_IDCOMP = item.FP_IDCOMP;
                        objetoBuscar.FACE = item.FACE;
                        objetoBuscar.FACE_LENG = item.FACE_LENG;
            

                        _context.RelojUsuario.Update(objetoBuscar);
                    }
                    else
                    {
                        // crear un nuevo registro si no existe
                        await _context.RelojUsuario.AddAsync(item);
                    }

                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.PostRelojUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);

                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = error;
            }
            return respuesta;
        }


        /// <summary>
        /// GetRelojTemplate:  Obtiene lista de templates de huellas registrados para compañia
        /// </summary>
        /// <returns>Lista de templates de huellas registrados</returns>
        public async Task<IEnumerable<cRelojTemplate>> GetRelojTemplate()
        {
            List<cRelojTemplate> model = new();

            try
            {
                model = await _context.RelojTemplate.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojTemplate: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetRelojTemplate:  Obtiene los templates de huellas registrados para un empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cRelojTemplate>> GetRelojTemplate(string id)
        {
            List<cRelojTemplate> model = new();

            try
            {
                model = await _context.RelojTemplate.Where(e=>e.FP_ENROLLID==id).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojTemplate: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// PostRelojTemplate: metodo crear o modificar uno o varios registros de templates de huellas en esquema de la compañia 
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public async Task<EventResponse> PostRelojTemplate(IEnumerable<cRelojTemplate> usuarios)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in usuarios)
                {
                    cRelojTemplate? objetoBuscar = await _context.RelojTemplate.FirstOrDefaultAsync(e => e.FP_ENROLLID == item.FP_ENROLLID);

                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.FP_INDEXID = item.FP_INDEXID;
                        objetoBuscar.FP_TEMPLATE = item.FP_TEMPLATE;
                        objetoBuscar.FP_LENGTH = item.FP_LENGTH;
                        objetoBuscar.FP_USER = item.FP_USER;
                        objetoBuscar.FP_SEC = item.FP_SEC;
                        objetoBuscar.IDUSER = item.IDUSER;


                        _context.RelojTemplate.Update(objetoBuscar);
                    }
                    else
                    {
                        // crear un nuevo registro si no existe
                        await _context.RelojTemplate.AddAsync(item);
                    }

                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.PostRelojTemplate: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);

                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = error;
            }
            return respuesta;
        }


        /// <summary>
        /// GetRelojTemplate:  Obtiene lista de templates de huellas registrados para compañia
        /// </summary>
        /// <returns>Lista de templates de huellas registrados</returns>
        public async Task<IEnumerable<cRelojTemplateFace>> GetRelojTemplateFace()
        {
            List<cRelojTemplateFace> model = new();

            try
            {
                model = await _context.RelojTemplateFace.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojTemplateFace: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetRelojTemplateFace:  Obtiene los templates de huellas registrados para un empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cRelojTemplateFace>> GetRelojTemplateFace(string id)
        {
            List<cRelojTemplateFace> model = new();

            try
            {
                model = await _context.RelojTemplateFace.Where(e => e.FACE_PIN == id).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetRelojTemplateFace: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        /// <summary>
        /// PostRelojTemplateFace: metodo crear o modificar uno o varios registros de templates de huellas en esquema de la compañia 
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public async Task<EventResponse> PostRelojTemplateFace(IEnumerable<cRelojTemplateFace> usuarios)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in usuarios)
                {
                    cRelojTemplateFace? objetoBuscar = await _context.RelojTemplateFace.FirstOrDefaultAsync(e => e.FACE_PIN == item.FACE_PIN && e.FACE_INDEX==item.FACE_INDEX);

                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.FACE_NO = item.FACE_NO;
                        objetoBuscar.FACE_VALID = item.FACE_VALID;
                        objetoBuscar.FACE_DURESS = item.FACE_DURESS;
                        objetoBuscar.FACE_TYPE = item.FACE_TYPE;
                        objetoBuscar.FACE_MAJORVER = item.FACE_MAJORVER;
                        objetoBuscar.FACE_MINORVER = item.FACE_MINORVER;
                        objetoBuscar.FACE_FORMAT = item.FACE_FORMAT;
                        objetoBuscar.FACE_TEMPLATE = item.FACE_TEMPLATE;


                        _context.RelojTemplateFace.Update(objetoBuscar);
                    }
                    else
                    {
                        // crear un nuevo registro si no existe
                        await _context.RelojTemplateFace.AddAsync(item);
                    }

                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.PostRelojTemplateFace: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);

                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = error;
            }
            return respuesta;
        }
    }


}
