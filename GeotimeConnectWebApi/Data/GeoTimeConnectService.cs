using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeotimeFuncionesLib.Utiles;

using GeoTimeServiceReference;
using JtSegEncrypta;
using LibEncripta;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SourceAFIS;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using static GeoTimeServiceReference.ServiceSoapClient;
using static com.gsitcr.geotime.Models.CalculoPeriodoParam;
using System.Web;


namespace com.gsitcr.geotime.Data
{
    public class GeoTimeConnectService : IGeoTimeConnectService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<GeoTimeConnectService> _logger;
        private readonly IGraphSendMail _sendMail;
        private readonly IEncriptaService _encriptaService;

        public GeoTimeConnectService(IHttpContextAccessor httpContextAccessor, 
                                     ILogger<GeoTimeConnectService> logger,
                                     IEncriptaService encriptaService,
                                     IGraphSendMail sendMail)
        {
            _httpContextAccessor = httpContextAccessor;
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext!.User.Claims;
            string schema = "";
            string bdname = "";
            _logger = logger;
            _sendMail = sendMail;
            _encriptaService = encriptaService;

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

        #region SQLMetodos

        //Creado por: Allan Prieto 
        //Fecha: 2024-3-26
        //Obtener datos de Ph_Login
        public async Task<IEnumerable<cPh_Login>> GetPhLogin()
        {
            List<cPh_Login> phlogin = new();

            try
            {
                phlogin = await _context.PH_LOGIN.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhLogin: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return phlogin;
        }

        /// <summary>
        /// GetPhLogin: Método para obtener un usuario de ph_login segun su cuenta de correo
        /// </summary>
        /// <returns>Una instancia de la clase cPhLogin</returns>
        /// ///<param name="id">Id del usuario requerido</param>
        public async Task<cPh_Login> GetPhLogin(string id)
        {
            cPh_Login? phlogin = new();
           

            try
            {
                phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.EMAIL!.ToUpper() == id.ToUpper());
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhLogin: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return phlogin;
        }

        /// <summary>
        /// GetPhLoginByUsuario: Método para obtener un ph_login por nombre de usuario
        /// </summary>
        /// <returns>Una instancia de la clase cPhLogin</returns>
        /// ///<param name="id">Id del usuario requerido</param>
        public async Task<cPh_Login> GetPhLoginByUsuario(string id)
        {
            cPh_Login? phlogin = new();

            try
            {
                phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.usuario.ToUpper() == id.ToUpper());
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhLoginByUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return phlogin;
        }

        /// <summary>
        /// GetPhLoginById: Método para obtener un ph_login por nombre de usuario
        /// </summary>
        /// <returns>Una instancia de la clase cPhLogin</returns>
        /// ///<param name="id">Id del usuario requerido</param>
        public async Task<cPh_Login> GetPhLoginById(int id)
        {
            cPh_Login? phlogin = new();

            try
            {
                phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.idusuario == id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhLoginById: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return phlogin;
        }

        /// <summary>
        /// PutPhLogin: metodo para actualizar campos de filtros del ph_login
        /// </summary>
        /// <param name="phLogin"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public async Task<EventResponse> PutPhLogin(cPh_Login phLogin)
        {
            EventResponse respuesta = new EventResponse();
            
            try
            {
                cPh_Login? loginBuscar = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.idusuario == phLogin.idusuario);
                var Pas = (phLogin.GLOBAL_CLAVE == "0") ? "0" : "1";
                
                if (loginBuscar is not null)
                {

                    loginBuscar.usuario = phLogin.usuario;
                    loginBuscar.descripcion = phLogin.descripcion;
                    loginBuscar.usa_wusuario = phLogin.usa_wusuario;
                    loginBuscar.OMITE_LIC = phLogin.OMITE_LIC;
                    loginBuscar.fcomp = phLogin.fcomp != loginBuscar.fcomp && !string.IsNullOrEmpty(phLogin.fcomp!)? phLogin.fcomp : loginBuscar.fcomp;
                    loginBuscar.idsesion = 0; // se reinicia la sesion
                    
                    // Verifica que sea necesario cambiar la clave
                    loginBuscar.clave = ("0" == Pas) ? loginBuscar.clave : FuncionesGlobales.Global_encrypt(Encripta.getDecryptTripleDES(phLogin.GLOBAL_CLAVE!));
                    // Verifica que sea necesario cambiar la clave
                    loginBuscar.GLOBAL_CLAVE = ("0" == Pas) ? loginBuscar.GLOBAL_CLAVE : FuncionesGlobales.Global_encrypt(Encripta.getDecryptTripleDES(phLogin.GLOBAL_CLAVE!));

                    //loginBuscar.GLOBAL_CLAVE = phLogin.GLOBAL_CLAVE;
                    loginBuscar.EMAIL = phLogin.EMAIL;
                    loginBuscar.companias = phLogin.companias;
                    

                    _context.PH_LOGIN.Update(loginBuscar);
                }
                else
                {
                    phLogin.clave = FuncionesGlobales.Global_encrypt(Encripta.getDecryptTripleDES(phLogin.clave));
                    phLogin.GLOBAL_CLAVE = FuncionesGlobales.Global_encrypt(Encripta.getDecryptTripleDES(phLogin.GLOBAL_CLAVE!));
                    // crear un nuevo registro si no existe
                    await _context.PH_LOGIN.AddAsync(phLogin);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización del login. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización del login. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina_PhLogin:  Metodo borrado de datos de la tabla Login
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhLogin(int id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_Login? model = await _context.PH_LOGIN
                    .FirstOrDefaultAsync(e => e.idusuario == id);

                if (model is not null)
                {
                    _context.PH_LOGIN.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Usuario. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-01-02
        //Obtener lista de Companias
        public async Task<List<cPh_Compania>> GetPhCompania()
        {
            List<cPh_Compania> companias = new();
            try
            {
                companias = (from e in await _context.PH_COMPANIAS.ToListAsync()
                             select new cPh_Compania
                             {
                                 IDCOMP = e.IDCOMP,
                                 COMPANIA = e.COMPANIA,
                                 NOM_CONECTOR = e.NOM_CONECTOR,
                                 STRING_SQL = e.STRING_SQL,
                                 STRING_SQL_ERP = e.STRING_SQL_ERP,
                                 PAIS = e.PAIS,
                                 AUTO_PROCESO = e.AUTO_PROCESO,
                                 REMOTE_ERPSERVICE = e.REMOTE_ERPSERVICE,
                                 MAIL_SERVER = e.MAIL_SERVER,
                                 MAIL_USER = e.MAIL_USER,
                                 MAIL_PASSWORD = e.MAIL_PASSWORD,
                                 MAIL_PORT = e.MAIL_PORT,
                                 MAIL_AUTH = e.MAIL_AUTH,
                                 MAIL_SSL = e.MAIL_SSL,
                                 HORA_SUP = e.HORA_SUP,
                                 HORA_EMP = e.HORA_EMP,
                                 SUPERVISOR_ACUM = e.SUPERVISOR_ACUM,
                                 MAIL_TLS = e.MAIL_TLS,
                                 IN_MARCAS = e.IN_MARCAS,
                                 HORA_CALC = e.HORA_CALC,
                                 APICLIENTID = String.IsNullOrEmpty(e.APICLIENTID) ? "" : Encripta.getDecryptTripleDES(e.APICLIENTID!),
                                 APIUSER = String.IsNullOrEmpty(e.APIUSER) ? "" : Encripta.getDecryptTripleDES(e.APIUSER!),
                                 APIPASSWORD = String.IsNullOrEmpty(e.APIPASSWORD) ? "" : Encripta.getDecryptTripleDES(e.APIPASSWORD!),
                                 APIDATABASE = String.IsNullOrEmpty(e.APIDATABASE) ? "" : Encripta.getDecryptTripleDES(e.APIDATABASE!),
                                 APIURL = String.IsNullOrEmpty(e.APIURL) ? "" : Encripta.getDecryptTripleDES(e.APIURL!), 
                                 ZONAHORARIA = e.ZONAHORARIA,
                             }).ToList();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhCompania: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return companias;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetPhCompania: Obtener una compañia especifica
        /// </summary>
        /// <param name="idcomp">Id de Compañia a buscar</param>
        /// <returns>Intancia de compañia</returns>
        public async Task<cPh_Compania> GetPhCompania(string idcomp)
        {
            cPh_Compania? compania = new();
            try
            {
                compania = (from e in await _context.PH_COMPANIAS.Where(e => e.IDCOMP == idcomp)
                             .ToListAsync()
                             select new cPh_Compania
                             {
                                 IDCOMP = e.IDCOMP,
                                 COMPANIA = e.COMPANIA,
                                 NOM_CONECTOR = e.NOM_CONECTOR,
                                 STRING_SQL = e.STRING_SQL,
                                 STRING_SQL_ERP = e.STRING_SQL_ERP,
                                 PAIS = e.PAIS,
                                 AUTO_PROCESO = e.AUTO_PROCESO,
                                 REMOTE_ERPSERVICE = e.REMOTE_ERPSERVICE,
                                 MAIL_SERVER = e.MAIL_SERVER,
                                 MAIL_USER = e.MAIL_USER,
                                 MAIL_PASSWORD = e.MAIL_PASSWORD,
                                 MAIL_PORT = e.MAIL_PORT,
                                 MAIL_AUTH = e.MAIL_AUTH,
                                 MAIL_SSL = e.MAIL_SSL,
                                 HORA_SUP = e.HORA_SUP,
                                 HORA_EMP = e.HORA_EMP,
                                 SUPERVISOR_ACUM = e.SUPERVISOR_ACUM,
                                 MAIL_TLS = e.MAIL_TLS,
                                 IN_MARCAS = e.IN_MARCAS,
                                 HORA_CALC = e.HORA_CALC,
                                 APICLIENTID = String.IsNullOrEmpty(e.APICLIENTID) ? "" : Encripta.getDecryptTripleDES(e.APICLIENTID!),
                                 APIUSER = String.IsNullOrEmpty(e.APIUSER) ? "" : Encripta.getDecryptTripleDES(e.APIUSER!),
                                 APIPASSWORD = String.IsNullOrEmpty(e.APIPASSWORD) ? "" : Encripta.getDecryptTripleDES(e.APIPASSWORD!),
                                 APIDATABASE = String.IsNullOrEmpty(e.APIDATABASE) ? "" : Encripta.getDecryptTripleDES(e.APIDATABASE!),
                                 APIURL = String.IsNullOrEmpty(e.APIURL) ? "" : Encripta.getDecryptTripleDES(e.APIURL!),
                                 ZONAHORARIA = e.ZONAHORARIA,
                             }).FirstOrDefault();

                                
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhCompania: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return compania;
        }

        /// <summary>
        /// Sincronizar_PhCompania: metodo para sincronizar las compañias 
        /// </summary>
        /// <param name="phCompanias"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_PhCompania(IEnumerable<cPh_Compania> phCompanias)
        {
            EventResponse respuesta = new EventResponse();
            bool IsNew = false;
            try
            {
                foreach (var item in phCompanias)
                {
                    cPh_Compania? objetoBuscar = await _context.PH_COMPANIAS
                                    .FirstOrDefaultAsync(e => e.IDCOMP == item.IDCOMP);
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        IsNew = false;
                        objetoBuscar.COMPANIA = item.COMPANIA;
                        objetoBuscar.NOM_CONECTOR = item.NOM_CONECTOR;
                        objetoBuscar.STRING_SQL = item.STRING_SQL;
                        objetoBuscar.STRING_SQL_ERP = item.STRING_SQL_ERP;
                        objetoBuscar.PAIS = item.PAIS;
                        objetoBuscar.AUTO_PROCESO = item.AUTO_PROCESO;
                        objetoBuscar.REMOTE_ERPSERVICE = item.REMOTE_ERPSERVICE;
                        objetoBuscar.MAIL_SERVER = item.MAIL_SERVER;
                        objetoBuscar.MAIL_USER = item.MAIL_USER;
                        objetoBuscar.MAIL_PASSWORD = item.MAIL_PASSWORD;
                        objetoBuscar.MAIL_PORT = item.MAIL_PORT;
                        objetoBuscar.MAIL_AUTH = item.MAIL_AUTH;
                        objetoBuscar.MAIL_SSL = item.MAIL_SSL;
                        objetoBuscar.HORA_SUP = item.HORA_SUP;
                        objetoBuscar.HORA_EMP = item.HORA_EMP;
                        objetoBuscar.SUPERVISOR_ACUM = item.SUPERVISOR_ACUM;
                        objetoBuscar.MAIL_TLS = item.MAIL_TLS;
                        objetoBuscar.HORA_CALC = item.HORA_CALC;
                        objetoBuscar.IN_MARCAS = item.IN_MARCAS;
                        objetoBuscar.APICLIENTID = Encripta.getEncryptTripleDES(item.APICLIENTID!);
                        objetoBuscar.APIUSER = Encripta.getEncryptTripleDES(item.APIUSER!);
                        objetoBuscar.APIPASSWORD = Encripta.getEncryptTripleDES(item.APIPASSWORD!);
                        objetoBuscar.APIURL = Encripta.getEncryptTripleDES(item.APIURL!); 
                        objetoBuscar.APIDATABASE = Encripta.getEncryptTripleDES(item.APIDATABASE!);
                        objetoBuscar.ZONAHORARIA = item.ZONAHORARIA;

                        _logger.LogError($"GeoTimeConnectService.Sincronizar_PhCompania.Update: {item.APIURL!}-{item.APIDATABASE}-{item.APICLIENTID}");

                        _context.PH_COMPANIAS.Update(objetoBuscar);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        IsNew = true;
                        item.APICLIENTID = Encripta.getEncryptTripleDES(item.APICLIENTID!);
                        item.APIUSER = Encripta.getEncryptTripleDES(item.APIUSER!);
                        item.APIPASSWORD = Encripta.getEncryptTripleDES(item.APIPASSWORD!);
                        item.APIURL = Encripta.getEncryptTripleDES(item.APIURL!);
                        item.APIDATABASE = Encripta.getEncryptTripleDES(item.APIDATABASE!);

                        _context.Add(item);
                        await _context.SaveChangesAsync();


                    }
                    if (IsNew)
                    {
                        var respCreaCompania = await PostCompaniaEnBD(item.IDCOMP);

                        if (respCreaCompania.Id != "0")
                        {
                            _logger.LogError($"GeoTimeConnectService.Sincronizar_PhCompania.PostCompaniaEnBD: {respCreaCompania.Descripcion}");
                            respuesta = respCreaCompania;
                        }
                    }
                        
                }
            }
            catch (Exception e)
            {                
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la compañía. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la compañía. Detalle de Error: " + e.InnerException.Message;

               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Sincronizar_PhCompania: {respuesta.Descripcion}");


            }

            return respuesta;

        }


        /// <summary>
        /// PutPhCompania: metodo para actualizar campos de la api en todos los registros de ph_compania
        /// </summary>
        /// <param name="phCompanias"></param>
        /// <returns>EventResponse, con el resultado del proceso</returns>
        public async Task<EventResponse> PutPhCompania(cPh_Compania phCompanias)
        {
            EventResponse respuesta = new EventResponse();
            
            try
            {
                await UpgradeTablesBD(phCompanias.IDCOMP);

                var companias = await _context.PH_COMPANIAS.ToListAsync();
                //actualizar datos de api para todas las compañias

                foreach(var comp in companias)
                {
                    comp.APICLIENTID = Encripta.getEncryptTripleDES(phCompanias.APICLIENTID!);
                    comp.APIUSER = Encripta.getEncryptTripleDES(phCompanias.APIUSER!);
                    comp.APIPASSWORD = Encripta.getEncryptTripleDES(phCompanias.APIPASSWORD!);
                    comp.APIURL = Encripta.getEncryptTripleDES(phCompanias.APIURL!);
                    comp.APIDATABASE = Encripta.getEncryptTripleDES(phCompanias.APIDATABASE!);
                    _context.PH_COMPANIAS.Update(comp);
                }
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de los datos del API. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de los datos del API. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.PutPhCompania: {respuesta.Descripcion}");


            }

            return respuesta;

        }

        /// <summary>
        /// Sincronizar_PhCompania: metodo para sincronizar la compañia inicial 
        /// </summary>
        /// <param name="compania"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_PhCompaniaInicial(cPh_Compania compania)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var respCreaCompania = await PostCompaniaEnBD(compania.IDCOMP,true);

                if (respCreaCompania.Id != "0")
                {
                    _logger.LogError($"GeoTimeConnectService.Sincronizar_PhCompania.PostCompaniaEnBD: {respCreaCompania.Descripcion}");
                    respuesta = respCreaCompania;
                }
                else
                {
                    compania.APICLIENTID = Encripta.getEncryptTripleDES(compania.APICLIENTID!);
                    compania.APIUSER = Encripta.getEncryptTripleDES(compania.APIUSER!);
                    compania.APIPASSWORD = Encripta.getEncryptTripleDES(compania.APIPASSWORD!);
                    compania.APIURL = Encripta.getEncryptTripleDES(compania.APIURL!);
                    compania.APIDATABASE = Encripta.getEncryptTripleDES(compania.APIDATABASE!);

                    _context.Add(compania);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la compañía inicial. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la compañía inicial. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Sincronizar_PhCompaniaInicial: {respuesta.Descripcion}");


            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-27
        //Obtener lista de tipos de planilla 
        public async Task<IEnumerable<cPh_Planilla>> GetPhPlanilla()
        {
            List<cPh_Planilla>? planilla = new();
            try
            {
                planilla = await _context.Ph_Planilla.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhPlanilla: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return planilla;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-27
        //Obtener un tipo de planilla especifico
        public async Task<cPh_Planilla> GetPhPlanilla(string idplanilla)
        {
            cPh_Planilla? planilla = new();
            try
            {
                planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.idplanilla == idplanilla);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhPlanilla: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return planilla;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-27
        //Obtener un tipo de planilla especifico por nom conector
        public async Task<cPh_Planilla> GetPhPlanilla(string nomConector, string descPlanilla)
        {
            cPh_Planilla? planilla = new();
            try
            {
                planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.nom_conector.ToLower() == nomConector.ToLower());

                if (planilla == null)
                {
                    planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.planilla.ToLower() == descPlanilla.ToLower());
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhPlanilla: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return planilla!;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2023-10-12
        //Sincronizar Planilla 
        //Parametro: Recibe una instancia de Planilla, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_PhPlanilla(IEnumerable<cPh_Planilla> PhPlanillas)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in PhPlanillas)
                {
                    cPh_Planilla? pla = await _context.Ph_Planilla
                                                        .FirstOrDefaultAsync(e => e.idplanilla == item.idplanilla);
                    if (pla is not null)
                    {
                        pla.planilla = item.planilla;
                        pla.nom_conector = item.nom_conector;
                        pla.tipo_planilla = item.tipo_planilla;
                        pla.c_ext = item.c_ext;
                        pla.c_inci = item.c_inci;
                        pla.c_adic = item.c_adic;
                        pla.m_desc = item.m_desc;
                        pla.proyecta = item.proyecta;
                        pla.dia_inicio = item.dia_inicio;
                        pla.auto_proceso = item.auto_proceso;
                        pla.tipo_dist = item.tipo_dist;
                        pla.est_nomina = item.est_nomina;
                        pla.ext_per_ant = item.ext_per_ant;
                        pla.ext_det = item.ext_det;
                        pla.agrup_salida = item.agrup_salida;
                        pla.tipo_adic = item.tipo_adic;
                        pla.nivel_aprob_ext = item.nivel_aprob_ext;
                        _context.Ph_Planilla.Update(pla);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la planilla. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_PhPlanilla:  Metodo borrado de datos de la tabla Ph_Planilla
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhPlanilla(string idplanilla)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPh_Planilla? model = await _context.Ph_Planilla
                    .FirstOrDefaultAsync(e => e.idplanilla == idplanilla);

                if (model is not null)
                {
                    _context.Ph_Planilla.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la planill. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-04-02
        //Obtener un tipo de planilla especifico
        public async Task<List<cPh_Planilla>> GetPhPlanillaByUsuario(string idUsuario)
        {
            List<cPh_Planilla>? planilla = new();
            try
            {
                var usuario = await _context.Ph_Usuarios.FirstOrDefaultAsync(u => u.IDUSUARIO.ToString() == idUsuario);
                if (usuario == null)
                {
                    // Si el usuario no existe, retornar una lista vacía
                    return new List<cPh_Planilla>();
                }

                var ids = usuario.PLANILLAS.Split(',').SelectMany(s => s.Split('°')).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                planilla = await _context.Ph_Planilla
                .Where(p => ids.Contains(p.idplanilla))
                .ToListAsync();
                
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhPlanillaByUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }

            return planilla;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-8
        /// <summary>
        /// GetTipo_Planilla: Obtener lista de registros de la tabla TIPOS_PLANILLA
        /// </summary>
        /// <returns>Lista de cTipo_Planilla </returns>
        /// 
        public async Task<List<cTipo_Planilla>> GetTipo_Planilla()
        {
            List<cTipo_Planilla> planillas = new();
            try
            {
                planillas = await _context.TIPOS_PLANILLA.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTipo_Planilla: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return planillas;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Departamentos
        public async Task<List<cDepartamento>> GetDepartamento()
        {
            List<cDepartamento> departamento = new();
            try
            {
                departamento = await _context.Ph_Departamento.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetDepartamento: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return departamento;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Departamento especifico
        //Parametros: idDepart=Id de departamento a buscar
        public async Task<cDepartamento> GetDepartamento(string idDepart)
        {
            cDepartamento? departamento = new();
            try
            {
                departamento = await _context.Ph_Departamento.FirstOrDefaultAsync(e => e.IDDEPART == idDepart);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetDepartamento: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return departamento;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Departamentos
        //Parametro: Recibe una instancia de departamento, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Departamento(IEnumerable<cDepartamento> departamentos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var departamento in departamentos)
                {
                    cDepartamento? depto = await _context.Ph_Departamento
                                                        .Where(e => e.IDDEPART == departamento.IDDEPART)
                                                        .FirstOrDefaultAsync();
                    //si el departamento existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (depto is not null)
                    {
                        depto.DESCRIPCION = departamento.DESCRIPCION;
                        _context.Ph_Departamento.Update(depto);
                    }
                    else
                    {
                        _context.Add(departamento);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Departamentos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Departamentos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Departamento:  Metodo boorado de datos de la tabla cDepartamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Departamento(string id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cDepartamento? model = await _context.Ph_Departamento
                    .FirstOrDefaultAsync(e => e.IDDEPART == id);

                if (model is not null)
                {
                    _context.Ph_Departamento.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Departamento. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar Departamento. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }

        //Fecha: 2022-10-30
        //Obtener lista de Conceptos
        public async Task<List<cPh_Grupo>> GetGrupo()
        {
            List<cPh_Grupo> grupo = new();
            try
            {
                grupo = await _context.Ph_Grupos.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetGrupo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return grupo;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Grupo especifico
        //Parametros: concepto=concepto a buscar
        public async Task<cPh_Grupo> GetGrupo(int idgrupo)
        {
            cPh_Grupo? grupos = new();
            try
            {
                grupos = await _context.Ph_Grupos.FirstOrDefaultAsync(e => e.idgrupo == idgrupo);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetGrupo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return grupos;
        }

        /// <summary>
        /// Sincronizar_Grupo: metodo para sincronizar los Grupos 
        /// </summary>
        /// <param name="PhGrupos"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_Grupo(IEnumerable<cPh_Grupo> phGrupos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in phGrupos)
                {
                    cPh_Grupo? objetoBuscar = await _context.Ph_Grupos
                                    .FirstOrDefaultAsync(e => e.idgrupo == item.idgrupo);
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro*
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.idgrupo = item.idgrupo;
                        objetoBuscar.descripcion = item.descripcion;
                        objetoBuscar.idcomp = item.idcomp;
                        objetoBuscar.idplanilla = item.idplanilla;
                        objetoBuscar.estado = item.estado;
                        objetoBuscar.idagrupamiento = item.idagrupamiento;
                        objetoBuscar.turno_continuo = item.turno_continuo;
                        objetoBuscar.OrganizacionId = item.OrganizacionId;

                        _context.Ph_Grupos.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Grupo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Grupo. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Grupo:  Metodo borrado de datos de la tabla Ph_Grupos
        /// </summary>
        /// <param name="idgrupo"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Grupo(int idgrupo)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPh_Grupo? model = await _context.Ph_Grupos
                    .FirstOrDefaultAsync(e => e.idgrupo == idgrupo);

                if (model is not null)
                {
                    _context.Ph_Grupos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Grupo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Grupo. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-12
        //Obtener un tipo de planilla especifico
        public async Task<List<cPh_Grupo>> GetPhGrupoByUsuario(string idUsuario)
        {
            List<cPh_Grupo>? grupo = new();
            try
            {
                var usuario = await _context.Ph_Usuarios.FirstOrDefaultAsync(u => u.IDUSUARIO.ToString() == idUsuario);
                if (usuario == null)
                {
                    // Si el usuario no existe, retornar una lista vacía
                    return new List<cPh_Grupo>();
                }

                var ids = usuario.GRUPOS.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                grupo = await _context.Ph_Grupos
                .Where(p => ids.Contains((p.idgrupo).ToString()))
                .ToListAsync();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhGrupoByUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }

            return grupo;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetEmpleado: Método para obtener una lista de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public async Task<List<cEmpleado>> GetEmpleado()
        {
            List<cEmpleado> empleado = new();
            try
            {
                empleado = (from e in await _context.Empleados
                                    .Include(e => e.Departamento)
                                    .Include(e => e.CentroCosto)
                                    .Include(e => e.Ph_Planilla)
                                    .Include(e => e.Ph_Grupo)
                                .Where(e => e.Estado == 'T').OrderBy(e => e.Nombre).ToListAsync()
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                puesto = e.puesto,
                                Departamento = e.Departamento == null ? null :
                                               new cDepartamento
                                               {
                                                   IDDEPART = e.Departamento.IDDEPART,
                                                   DESCRIPCION = e.Departamento.DESCRIPCION,
                                               },
                                CentroCosto = e.CentroCosto == null ? null :
                                               new cCentroCosto
                                               {
                                                   IdCCosto = e.CentroCosto.IdCCosto,
                                                   Descripcion = e.CentroCosto.Descripcion,
                                                   Distribuye = e.CentroCosto.Distribuye,
                                               },
                                Ph_Planilla = e.Ph_Planilla == null ? null :
                                               new cPh_Planilla
                                               {
                                                   idplanilla = e.Ph_Planilla.idplanilla,
                                                   planilla = e.Ph_Planilla.planilla,
                                                   nom_conector = e.Ph_Planilla.nom_conector,
                                                   tipo_planilla = e.Ph_Planilla.tipo_planilla,
                                                   c_ext = e.Ph_Planilla.c_ext,
                                                   c_inci = e.Ph_Planilla.c_inci,
                                                   c_adic = e.Ph_Planilla.c_adic,
                                                   m_desc = e.Ph_Planilla.m_desc,
                                                   proyecta = e.Ph_Planilla.proyecta,
                                                   dia_inicio = e.Ph_Planilla.dia_inicio,
                                                   auto_proceso = e.Ph_Planilla.auto_proceso,
                                                   tipo_dist = e.Ph_Planilla.tipo_dist,
                                                   est_nomina = e.Ph_Planilla.est_nomina,
                                                   ext_per_ant = e.Ph_Planilla.ext_per_ant,
                                                   ext_det = e.Ph_Planilla.ext_det,
                                                   agrup_salida = e.Ph_Planilla.agrup_salida,
                                                   tipo_adic = e.Ph_Planilla.tipo_adic,
                                                   nivel_aprob_ext = e.Ph_Planilla.nivel_aprob_ext,
                                               },
                                Ph_Grupo = e.Ph_Grupo == null ? null :
                                               new cPh_Grupo
                                               {
                                                   idgrupo = e.Ph_Grupo.idgrupo,
                                                   descripcion = e.Ph_Grupo.descripcion,
                                                   idcomp = e.Ph_Grupo.idcomp,
                                                   idplanilla = e.Ph_Grupo.idplanilla,
                                                   estado = e.Ph_Grupo.estado,
                                                   idagrupamiento = e.Ph_Grupo.idagrupamiento,
                                                   turno_continuo = e.Ph_Grupo.turno_continuo,
                                                   OrganizacionId = e.Ph_Grupo.OrganizacionId,
                                               },

                            }).ToList();


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }
        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetEmpleadoTotal: Método para obtener la lista total de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public async Task<List<cEmpleado>> GetEmpleadoTotal()
        {
            List<cEmpleado> empleado = new();
            try
            {
                empleado = (from e in await _context.Empleados
                                    .Include(e => e.Departamento)
                                    .Include(e => e.CentroCosto)
                                    .Include(e => e.Ph_Planilla)
                                    .Include(e => e.Ph_Grupo)
                                    .OrderBy(e => e.Nombre).ToListAsync()
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                puesto = e.puesto,
                                Departamento = e.Departamento == null ? null :
                                               new cDepartamento
                                               {
                                                   IDDEPART = e.Departamento.IDDEPART,
                                                   DESCRIPCION = e.Departamento.DESCRIPCION,
                                               },
                                CentroCosto = e.CentroCosto == null ? null :
                                               new cCentroCosto
                                               {
                                                   IdCCosto = e.CentroCosto.IdCCosto,
                                                   Descripcion = e.CentroCosto.Descripcion,
                                                   Distribuye = e.CentroCosto.Distribuye,
                                               },
                                Ph_Planilla = e.Ph_Planilla == null ? null :
                                               new cPh_Planilla
                                               {
                                                   idplanilla = e.Ph_Planilla.idplanilla,
                                                   planilla = e.Ph_Planilla.planilla,
                                                   nom_conector = e.Ph_Planilla.nom_conector,
                                                   tipo_planilla = e.Ph_Planilla.tipo_planilla,
                                                   c_ext = e.Ph_Planilla.c_ext,
                                                   c_inci = e.Ph_Planilla.c_inci,
                                                   c_adic = e.Ph_Planilla.c_adic,
                                                   m_desc = e.Ph_Planilla.m_desc,
                                                   proyecta = e.Ph_Planilla.proyecta,
                                                   dia_inicio = e.Ph_Planilla.dia_inicio,
                                                   auto_proceso = e.Ph_Planilla.auto_proceso,
                                                   tipo_dist = e.Ph_Planilla.tipo_dist,
                                                   est_nomina = e.Ph_Planilla.est_nomina,
                                                   ext_per_ant = e.Ph_Planilla.ext_per_ant,
                                                   ext_det = e.Ph_Planilla.ext_det,
                                                   agrup_salida = e.Ph_Planilla.agrup_salida,
                                                   tipo_adic = e.Ph_Planilla.tipo_adic,
                                                   nivel_aprob_ext = e.Ph_Planilla.nivel_aprob_ext,
                                               },
                                Ph_Grupo = e.Ph_Grupo == null ? null :
                                               new cPh_Grupo
                                               {
                                                   idgrupo = e.Ph_Grupo.idgrupo,
                                                   descripcion = e.Ph_Grupo.descripcion,
                                                   idcomp = e.Ph_Grupo.idcomp,
                                                   idplanilla = e.Ph_Grupo.idplanilla,
                                                   estado = e.Ph_Grupo.estado,
                                                   idagrupamiento = e.Ph_Grupo.idagrupamiento,
                                                   turno_continuo = e.Ph_Grupo.turno_continuo,
                                                   OrganizacionId = e.Ph_Grupo.OrganizacionId,
                                               },
                            }).ToList();


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetEmpleadoTotal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }

        public async Task<List<cEmpleado>> GetEmpleadoProgramador(string grupos)
        {
            List<cEmpleado> empleado = new();
            try
            {
                grupos = HttpUtility.UrlDecode(grupos);
                string[] ListGrupos = grupos.Split(',');
                List<cPh_Grupo> phgrupos = new List<cPh_Grupo>();

                foreach (var valor in ListGrupos)
                    phgrupos.Add(new cPh_Grupo
                    {
                        idgrupo = int.Parse(valor),
                    });


                empleado = (from e in await _context.Empleados.Where(e => e.Estado == 'T').ToListAsync()
                            join g in phgrupos on e.IdGrupo equals g.idgrupo
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                puesto = e.puesto,
                            }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);

                _logger.LogError($"GeoTimeConnectService.GetEmpleadoProgramador: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }


        public async Task<List<cEmpleado>> GetEmpleadoProgramador(string idplanilla, string grupos)
        {
            List<cEmpleado> empleado = new();
            try
            {
                grupos = HttpUtility.UrlDecode(grupos);
                // Convertir la cadena de grupos a una lista de enteros
                var grupoIds = grupos.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(g => int.Parse(g.Trim()))
                                     .ToList();

                // Filtrar directamente en la base de datos
                empleado = await _context.Empleados
                    .Where(e => e.IdPlanilla == idplanilla && e.Estado == 'T' && grupoIds.Contains(e.IdGrupo ?? 0))
                    .Select(e => new cEmpleado
                    {
                        IdNumero = e.IdNumero,
                        IdPlanilla = e.IdPlanilla,
                        Nombre = e.Nombre,
                        Tarjeta = e.Tarjeta,
                        Identificacion = e.Identificacion,
                        IdGrupo = e.IdGrupo,
                        IdDepartamento = e.IdDepartamento,
                        IdHorario = e.IdHorario,
                        Estado = e.Estado,
                        IdAgrupamiento = e.IdAgrupamiento,
                        foto = e.foto,
                        IdCCosto = e.IdCCosto,
                        exporta = e.exporta,
                        ubicacion = e.ubicacion,
                        rubro1 = e.rubro1,
                        rubro2 = e.rubro2,
                        rubro3 = e.rubro3,
                        rubro4 = e.rubro4,
                        rubro5 = e.rubro5,
                        rubro6 = e.rubro6,
                        rubro7 = e.rubro7,
                        rubro8 = e.rubro8,
                        rubro9 = e.rubro9,
                        rubro10 = e.rubro10,
                        rubro11 = e.rubro11,
                        rubro12 = e.rubro12,
                        rubro13 = e.rubro13,
                        rubro14 = e.rubro14,
                        rubro15 = e.rubro15,
                        rubro16 = e.rubro16,
                        rubro17 = e.rubro17,
                        rubro18 = e.rubro18,
                        rubro19 = e.rubro19,
                        rubro20 = e.rubro20,
                        rubro21 = e.rubro21,
                        rubro22 = e.rubro22,
                        rubro23 = e.rubro23,
                        rubro24 = e.rubro24,
                        rubro25 = e.rubro25,
                        Fecha_Ingreso = e.Fecha_Ingreso,
                        Email = e.Email,
                        Tipo_Marca = e.Tipo_Marca,
                        inicio_rol = e.inicio_rol,
                        web_pass = e.web_pass,
                        id_transfo_conc = e.id_transfo_conc,
                        widioma = e.widioma,
                        global_clave = e.global_clave,
                        def_fase = e.def_fase,
                        def_py = e.def_py,
                        def_cc = e.def_cc,
                        Fecha_Salida = e.Fecha_Salida,
                        global_code = e.global_code,
                        fecha_act_code = e.fecha_act_code,
                        puesto = e.puesto,
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetEmpleadoProgramador: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return empleado;
        }



        //Creado por: Marlon Loria
        //Fecha: 2025-08-26
        /// <summary>
        /// GetEmpleadoProgramadorByHorario: Método para obtener una lista de empleados asociados a un horarios y una planilla
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public async Task<List<cEmpleado>> GetEmpleadoProgramadorByHorario(string idplanilla, string horarios)
        {
            List<cEmpleado> empleado = new();
            try
            {
                horarios = HttpUtility.UrlDecode(horarios);
                string[] ListHorarios = horarios.Split(',');
                List<cPh_Horarios> ph_Horarios = new List<cPh_Horarios>();

                foreach (var valor in ListHorarios)
                    ph_Horarios.Add(new cPh_Horarios
                    {
                        IDHORARIO = int.Parse(valor),
                    });


                empleado = (from e in await _context.Empleados.Where(e => e.IdPlanilla == idplanilla && e.Estado == 'T').ToListAsync()
                            join g in ph_Horarios on e.IdHorario equals g.IDHORARIO
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                puesto = e.puesto,
                            }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);

                _logger.LogError($"GeoTimeConnectService.GetEmpleadoProgramador: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetEmpleado: Método para un empleado específico
        /// </summary>
        /// <returns>Una instancia de la clase cEmpleado</returns>
        /// ///<param name="idNumero">idNumero del empleado requerido</param>
        public async Task<cEmpleado> GetEmpleado(string idNumero)
        {
            cEmpleado? empleado = new();
            try
            {
                empleado = (from e in await _context.Empleados
                                    .Include(e => e.Departamento)
                                    .Include(e => e.CentroCosto)
                                    .Include(e => e.Ph_Planilla)
                                    .Include(e => e.Ph_Grupo)
                                .Where(e => e.IdNumero == idNumero).ToListAsync()
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                puesto = e.puesto,
                                Departamento = e.Departamento == null ? null :
                                               new cDepartamento
                                               {
                                                   IDDEPART = e.Departamento.IDDEPART,
                                                   DESCRIPCION = e.Departamento.DESCRIPCION,
                                               },
                                CentroCosto = e.CentroCosto == null ? null :
                                               new cCentroCosto
                                               {
                                                   IdCCosto = e.CentroCosto.IdCCosto,
                                                   Descripcion = e.CentroCosto.Descripcion,
                                                   Distribuye = e.CentroCosto.Distribuye,
                                               },
                                Ph_Planilla = e.Ph_Planilla == null ? null :
                                                   new cPh_Planilla
                                                   {
                                                       idplanilla = e.Ph_Planilla.idplanilla,
                                                       planilla = e.Ph_Planilla.planilla,
                                                       nom_conector = e.Ph_Planilla.nom_conector,
                                                       tipo_planilla = e.Ph_Planilla.tipo_planilla,
                                                       c_ext = e.Ph_Planilla.c_ext,
                                                       c_inci = e.Ph_Planilla.c_inci,
                                                       c_adic = e.Ph_Planilla.c_adic,
                                                       m_desc = e.Ph_Planilla.m_desc,
                                                       proyecta = e.Ph_Planilla.proyecta,
                                                       dia_inicio = e.Ph_Planilla.dia_inicio,
                                                       auto_proceso = e.Ph_Planilla.auto_proceso,
                                                       tipo_dist = e.Ph_Planilla.tipo_dist,
                                                       est_nomina = e.Ph_Planilla.est_nomina,
                                                       ext_per_ant = e.Ph_Planilla.ext_per_ant,
                                                       ext_det = e.Ph_Planilla.ext_det,
                                                       agrup_salida = e.Ph_Planilla.agrup_salida,
                                                       tipo_adic = e.Ph_Planilla.tipo_adic,
                                                       nivel_aprob_ext = e.Ph_Planilla.nivel_aprob_ext,
                                                   },
                                Ph_Grupo = e.Ph_Grupo == null ? null :
                                               new cPh_Grupo
                                               {
                                                   idgrupo = e.Ph_Grupo.idgrupo,
                                                   descripcion = e.Ph_Grupo.descripcion,
                                                   idcomp = e.Ph_Grupo.idcomp,
                                                   idplanilla = e.Ph_Grupo.idplanilla,
                                                   estado = e.Ph_Grupo.estado,
                                                   idagrupamiento = e.Ph_Grupo.idagrupamiento,
                                                   turno_continuo = e.Ph_Grupo.turno_continuo,
                                                   OrganizacionId = e.Ph_Grupo.OrganizacionId,
                                               },

                            }).FirstOrDefault();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }

        //Obtener Empleado por IdNumero
        public async Task<cEmpleado> GetEmpleadoByEmail(string email)
        {
            cEmpleado? empleado = new();

            try
            {
                empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Email == email && e.Estado == 'T');
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetEmpleadoByEmail: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Empleados
        public async Task<List<cEmpleado>> GetEmpleadoFiltrado(string idnumero, string nombre, string iddepartamento)
        {
            List<cEmpleado> empleado = new();

            if (idnumero == "all")
                idnumero = "";
            if (nombre == "all")
                nombre = "";
            if (iddepartamento == "all")
                iddepartamento = "";

            try
            {
                if (idnumero != "" && nombre != "" && iddepartamento != "")
                {
                    empleado = await _context.Empleados
                    .Where(e => e.IdNumero!.Contains(idnumero)
                           && e.Estado == 'T'
                           && e.Nombre!.ToLower().Contains(nombre.ToLower())
                           && e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                    .ToListAsync();
                }
                else
                {
                    if (idnumero == "" && nombre == "" && iddepartamento == "")
                    {
                        empleado = await _context.Empleados.Where(e => e.Estado == 'T')
                                            .ToListAsync();
                    }
                    else
                    {
                        if (idnumero != "")
                        {
                            empleado = await _context.Empleados
                             .Where(e => e.Estado == 'T' && e.IdNumero!.Contains(idnumero))
                             .ToListAsync();

                        }
                        else
                        {
                            if (nombre != "")
                            {
                                empleado = await _context.Empleados
                                 .Where(e => e.Estado == 'T' && e.Nombre!.ToLower().Contains(nombre.ToLower()))
                                 .ToListAsync();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = await _context.Empleados
                                     .Where(e => e.Estado == 'T' && e.IdDepartamento!.ToLower().Contains(iddepartamento))
                                     .ToListAsync();

                                }
                            }
                        }

                        if (idnumero != "")
                        {
                            if (nombre != "")
                            {
                                empleado = empleado
                                 .Where(e => e.Nombre!.ToLower().Contains(nombre.ToLower()))
                                 .ToList();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = empleado
                                            .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                                            .ToList();
                                }

                            }
                        }
                        else
                        {
                            if (nombre != "")
                            {
                                empleado = empleado
                                 .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                                 .ToList();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = empleado
                                            .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                                            .ToList();
                                }
                                else
                                {
                                    empleado = await _context.Empleados.Where(e => e.Estado == 'T')
                                    .ToListAsync();
                                }

                            }
                        }

                    }
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetEmpleadoFiltrado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return empleado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Empleados
        //Parametro: Recibe una instancia de Empleado, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Empleado(IEnumerable<cEmpleado> empleados)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                DateTime fechaIngreso;
                string idPlanillaAnt;
                foreach (var empleado in empleados)
                {
                    if (empleado.Fecha_Ingreso is null)
                        fechaIngreso = DateTime.Now;
                    else
                        fechaIngreso = (DateTime)DateTime.Parse(empleado.Fecha_Ingreso.ToString());


                    cEmpleado? emp = await _context.Empleados
                                    .Where(e => e.IdNumero == empleado.IdNumero)
                                    .FirstOrDefaultAsync();
                    //si el empleado existe se actualiza registro
                    //de lo contrario se agrega el registro
                    if (emp is not null)
                    {
                        //si estado nuevo es inactivo
                        if (empleado.Estado == 'F')
                            //si no viene la fecha de salida se asigna fecha del dia que se inactiva
                            if (empleado.Fecha_Salida is null)
                                emp.Fecha_Salida = DateTime.Now;
                            else
                                emp.Fecha_Salida = empleado.Fecha_Salida;
                        else
                            emp.Fecha_Salida = empleado.Fecha_Salida;

                        idPlanillaAnt = emp.IdPlanilla;
                        emp.Estado = empleado.Estado;
                        emp.Nombre = empleado.Nombre;
                        emp.IdDepartamento = empleado.IdDepartamento;
                        emp.IdCCosto = empleado.IdCCosto;
                        emp.IdPlanilla = empleado.IdPlanilla;
                        emp.Fecha_Ingreso = fechaIngreso;
                        emp.Identificacion = empleado.Identificacion;

                        emp.IdGrupo = (empleado.IdGrupo != null && empleado.IdGrupo != 0) ? empleado.IdGrupo : emp.IdGrupo;
                        emp.IdHorario = (empleado.IdHorario != null && empleado.IdHorario != 0) ? empleado.IdHorario : emp.IdHorario;
                        emp.Tipo_Marca = (empleado.Tipo_Marca != null && empleado.Tipo_Marca != "") ? empleado.Tipo_Marca : emp.Tipo_Marca;
                        emp.IdAgrupamiento = (empleado.IdAgrupamiento != null && empleado.IdAgrupamiento != 0) ? empleado.IdAgrupamiento : emp.IdAgrupamiento;
                        emp.Email = empleado.Email ?? emp.Email;
                        emp.Tarjeta = empleado.Tarjeta ?? emp.Tarjeta;
                        emp.exporta = (empleado.exporta != null) ? empleado.exporta : emp.exporta;
                        emp.id_transfo_conc = (empleado.id_transfo_conc != null && empleado.id_transfo_conc != 0) ? empleado.id_transfo_conc : emp.id_transfo_conc;
                        emp.puesto = empleado.puesto ?? emp.puesto;
                        _context.Empleados.Update(emp);
                        await _context.SaveChangesAsync();

                        if (idPlanillaAnt != empleado.IdPlanilla)
                        {
                            await EjecutaPostCambioPlanilla(empleado.IdNumero!, idPlanillaAnt!, empleado.IdPlanilla!);
                        }
                    }
                    else
                    {
                        empleado.Fecha_Ingreso = fechaIngreso;
                        empleado.Fecha_Salida = null;
                        empleado.IdGrupo = (empleado.IdGrupo == null || empleado.IdGrupo == 0) ? 1 : empleado.IdGrupo;
                        empleado.IdHorario = (empleado.IdHorario == null || empleado.IdHorario == 0) ? 1 : empleado.IdHorario;
                        empleado.Tipo_Marca = (empleado.Tipo_Marca == null || empleado.Tipo_Marca == "") ? "H" : empleado.Tipo_Marca;
                        empleado.IdAgrupamiento = (empleado.IdAgrupamiento == null) ? 0 : empleado.IdAgrupamiento;
                        empleado.Email = empleado.Email ?? "";
                        empleado.Tarjeta = empleado.Tarjeta ?? "";
                        empleado.exporta = empleado.exporta ?? 'T';

                        empleado.Departamento = null;
                        empleado.CentroCosto = null;
                        empleado.Ph_Planilla = null;
                        empleado.cAccionPersonal = null;


                        _context.Add(empleado);
                        await _context.SaveChangesAsync();
                    }
                }


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Empleado:  Metodo borrado de datos de la tabla Empleado
        /// </summary>
        /// <param name="idnumero"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Empleado(string idnumero)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cEmpleado? model = await _context.Empleados
                    .FirstOrDefaultAsync(e => e.IdNumero == idnumero);

                if (model is not null)
                {
                    _context.Empleados.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Empleado. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Turnos
        public async Task<List<cTurno>> GetTurno()
        {
            List<cTurno> turno = new();
            try
            {
                turno = (from e in await _context.Ph_Turnos
                            .Include(e=>e.PaletaColor)
                            .ToListAsync()
                            select new cTurno {
                                IdTurno = e.IdTurno,
                                Descripcion = e.Descripcion,
                                HEntra = e.HEntra,
                                HSale = e.HSale,
                                tar_apl = e.tar_apl,
                                ant_apl = e.ant_apl,
                                des_1_in = e.des_1_in,
                                des_1_out = e.des_1_out,
                                des_2_in = e.des_2_in,
                                des_2_out = e.des_2_out,
                                des_3_in = e.des_3_in,
                                des_3_out = e.des_3_out,
                                apl_des_1 = e.apl_des_1,
                                apl_des_2 = e.apl_des_2,
                                apl_des_3 = e.apl_des_3,
                                des_1_tiem = e.des_1_tiem,
                                des_2_tiem = e.des_2_tiem,
                                des_3_tiem = e.des_3_tiem,
                                marca_des_1 = e.marca_des_1,
                                marca_des_2 = e.marca_des_2,
                                marca_des_3 = e.marca_des_3,
                                tar_tiem = e.tar_tiem,
                                ant_tiem = e.ant_tiem,
                                con_1 = e.con_1,
                                con_2 = e.con_2,
                                con_3 = e.con_3,
                                con_4 = e.con_4,
                                con_5 = e.con_5,
                                con_6 = e.con_6,
                                cant_con_1 = e.cant_con_1,
                                cant_con_2 = e.cant_con_2,
                                cant_con_3 = e.cant_con_3,
                                cant_con_4 = e.cant_con_4,
                                cant_con_5 = e.cant_con_5,
                                cant_con_6 = e.cant_con_6,
                                min_con_1 = e.min_con_1,
                                min_con_2 = e.min_con_2,
                                min_con_3 = e.min_con_3,
                                min_con_4 = e.min_con_4,
                                min_con_5 = e.min_con_5,
                                min_con_6 = e.min_con_6,
                                Tipo = e.Tipo,
                                Tipo_Jor = e.Tipo_Jor,
                                fuerza_calc = e.fuerza_calc,
                                idagrupamiento = e.idagrupamiento,
                                apl_trans1 = e.apl_trans1,
                                id_trans1 = e.id_trans1,
                                apl_trans2 = e.apl_trans2,
                                id_trans2 = e.id_trans2,
                                apl_trans3 = e.apl_trans3,
                                id_trans3 = e.id_trans3,
                                apl_trans4 = e.apl_trans4,
                                id_trans4 = e.id_trans4,
                                apl_trans5 = e.apl_trans5,
                                id_trans5 = e.id_trans5,
                                apl_trans6 = e.apl_trans6,
                                id_trans6 = e.id_trans6,
                                apl_ben1 = e.apl_ben1,
                                id_ben1 = e.id_ben1,
                                apl_ben2 = e.apl_ben2,
                                id_ben2 = e.id_ben2,
                                apl_ben3 = e.apl_ben3,
                                id_ben3 = e.id_ben3,
                                apl_ben4 = e.apl_ben4,
                                id_ben4 = e.id_ben4,
                                apl_ben5 = e.apl_ben5,
                                id_ben5 = e.id_ben5,
                                apl_ben6 = e.apl_ben6,
                                id_ben6 = e.id_ben6,
                                conc_ben1 = e.conc_ben1,
                                conc_ben2 = e.conc_ben2,
                                conc_ben3 = e.conc_ben3,
                                conc_ben4 = e.conc_ben4,
                                conc_ben5 = e.conc_ben5,
                                conc_ben6 = e.conc_ben6,
                                apl_trans_post = e.apl_trans_post,
                                id_trans_post = e.id_trans_post,
                                apl_redond_entrada = e.apl_redond_entrada,
                                cant_redond_entrada = e.cant_redond_entrada,
                                auto_pan = e.auto_pan,
                                ColorId = e.ColorId,
                                PaletaColor = e.PaletaColor is null?null:
                                new cPaletaColor
                                {
                                    COLORID = e.PaletaColor.COLORID,
                                    DESCRIPCION = e.PaletaColor.DESCRIPCION,
                                    COLORFONDO = e.PaletaColor.COLORFONDO,
                                    COLORFUENTE = e.PaletaColor.COLORFUENTE,                                       
                                }
                            }).ToList();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return turno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Turno especifico
        //Parametros: codigo=Codigo de incidencia a buscar
        public async Task<cTurno> GetTurno(int idTurno)
        {
            cTurno? turno = new();
            try
            {
                turno = turno = (from e in await _context.Ph_Turnos
                            .Include(e => e.PaletaColor)
                            .Where(e=>e.IdTurno== idTurno)
                            .ToListAsync()
                                 select new cTurno
                                 {
                                     IdTurno = e.IdTurno,
                                     Descripcion = e.Descripcion,
                                     HEntra = e.HEntra,
                                     HSale = e.HSale,
                                     tar_apl = e.tar_apl,
                                     ant_apl = e.ant_apl,
                                     des_1_in = e.des_1_in,
                                     des_1_out = e.des_1_out,
                                     des_2_in = e.des_2_in,
                                     des_2_out = e.des_2_out,
                                     des_3_in = e.des_3_in,
                                     des_3_out = e.des_3_out,
                                     apl_des_1 = e.apl_des_1,
                                     apl_des_2 = e.apl_des_2,
                                     apl_des_3 = e.apl_des_3,
                                     des_1_tiem = e.des_1_tiem,
                                     des_2_tiem = e.des_2_tiem,
                                     des_3_tiem = e.des_3_tiem,
                                     marca_des_1 = e.marca_des_1,
                                     marca_des_2 = e.marca_des_2,
                                     marca_des_3 = e.marca_des_3,
                                     tar_tiem = e.tar_tiem,
                                     ant_tiem = e.ant_tiem,
                                     con_1 = e.con_1,
                                     con_2 = e.con_2,
                                     con_3 = e.con_3,
                                     con_4 = e.con_4,
                                     con_5 = e.con_5,
                                     con_6 = e.con_6,
                                     cant_con_1 = e.cant_con_1,
                                     cant_con_2 = e.cant_con_2,
                                     cant_con_3 = e.cant_con_3,
                                     cant_con_4 = e.cant_con_4,
                                     cant_con_5 = e.cant_con_5,
                                     cant_con_6 = e.cant_con_6,
                                     min_con_1 = e.min_con_1,
                                     min_con_2 = e.min_con_2,
                                     min_con_3 = e.min_con_3,
                                     min_con_4 = e.min_con_4,
                                     min_con_5 = e.min_con_5,
                                     min_con_6 = e.min_con_6,
                                     Tipo = e.Tipo,
                                     Tipo_Jor = e.Tipo_Jor,
                                     fuerza_calc = e.fuerza_calc,
                                     idagrupamiento = e.idagrupamiento,
                                     apl_trans1 = e.apl_trans1,
                                     id_trans1 = e.id_trans1,
                                     apl_trans2 = e.apl_trans2,
                                     id_trans2 = e.id_trans2,
                                     apl_trans3 = e.apl_trans3,
                                     id_trans3 = e.id_trans3,
                                     apl_trans4 = e.apl_trans4,
                                     id_trans4 = e.id_trans4,
                                     apl_trans5 = e.apl_trans5,
                                     id_trans5 = e.id_trans5,
                                     apl_trans6 = e.apl_trans6,
                                     id_trans6 = e.id_trans6,
                                     apl_ben1 = e.apl_ben1,
                                     id_ben1 = e.id_ben1,
                                     apl_ben2 = e.apl_ben2,
                                     id_ben2 = e.id_ben2,
                                     apl_ben3 = e.apl_ben3,
                                     id_ben3 = e.id_ben3,
                                     apl_ben4 = e.apl_ben4,
                                     id_ben4 = e.id_ben4,
                                     apl_ben5 = e.apl_ben5,
                                     id_ben5 = e.id_ben5,
                                     apl_ben6 = e.apl_ben6,
                                     id_ben6 = e.id_ben6,
                                     conc_ben1 = e.conc_ben1,
                                     conc_ben2 = e.conc_ben2,
                                     conc_ben3 = e.conc_ben3,
                                     conc_ben4 = e.conc_ben4,
                                     conc_ben5 = e.conc_ben5,
                                     conc_ben6 = e.conc_ben6,
                                     apl_trans_post = e.apl_trans_post,
                                     id_trans_post = e.id_trans_post,
                                     apl_redond_entrada = e.apl_redond_entrada,
                                     cant_redond_entrada = e.cant_redond_entrada,
                                     auto_pan = e.auto_pan,
                                     ColorId = e.ColorId,
                                     PaletaColor = e.PaletaColor is null ? null :
                                     new cPaletaColor
                                     {
                                         COLORID = e.PaletaColor.COLORID,
                                         DESCRIPCION = e.PaletaColor.DESCRIPCION,
                                         COLORFONDO = e.PaletaColor.COLORFONDO,
                                         COLORFUENTE = e.PaletaColor.COLORFUENTE,
                                     }
                                 }).FirstOrDefault();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return turno;
        }

        /// <summary>
        /// Sincronizar_Turno: metodo para sincronizar los Turnos 
        /// </summary>
        /// <param name="phTurno"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_Turno(IEnumerable<cTurno> phTurno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in phTurno)
                {
                    cTurno? objetoBuscar = await _context.Ph_Turnos
                                    .FirstOrDefaultAsync(e => e.IdTurno == item.IdTurno);
                    //si el Turno existe se actualiza descripción
                    //de lo contrario se agrega el registro*
                    if (objetoBuscar is not null)
                    {
                        /* Datos de relleno */
                        // objetoBuscar.IdTurno = item.IdTurno;
                        // objetoBuscar.Descripcion = item.Descripcion;

                        /* Datops listos para cuando se modifique el metodo correctamente */

                        objetoBuscar.IdTurno = item.IdTurno;
                        objetoBuscar.Descripcion = item.Descripcion;
                        objetoBuscar.HEntra = item.HEntra;
                        objetoBuscar.HSale = item.HSale;
                        objetoBuscar.tar_apl = item.tar_apl;
                        objetoBuscar.ant_apl = item.ant_apl;
                        objetoBuscar.des_1_in = item.des_1_in;
                        objetoBuscar.des_1_out = item.des_1_out;
                        objetoBuscar.des_2_in = item.des_2_in;
                        objetoBuscar.des_2_out = item.des_2_out;
                        objetoBuscar.des_3_in = item.des_3_in;
                        objetoBuscar.des_3_out = item.des_3_out;
                        objetoBuscar.apl_des_1 = item.apl_des_1;
                        objetoBuscar.apl_des_2 = item.apl_des_2;
                        objetoBuscar.apl_des_3 = item.apl_des_3;
                        objetoBuscar.des_1_tiem = item.des_1_tiem;
                        objetoBuscar.des_2_tiem = item.des_2_tiem;
                        objetoBuscar.des_3_tiem = item.des_3_tiem;
                        objetoBuscar.marca_des_1 = item.marca_des_1;
                        objetoBuscar.marca_des_2 = item.marca_des_2;
                        objetoBuscar.marca_des_3 = item.marca_des_3;
                        objetoBuscar.tar_tiem = item.tar_tiem;
                        objetoBuscar.ant_tiem = item.ant_tiem;
                        objetoBuscar.con_1 = item.con_1;
                        objetoBuscar.con_2 = item.con_2;
                        objetoBuscar.con_3 = item.con_3;
                        objetoBuscar.con_4 = item.con_4;
                        objetoBuscar.con_5 = item.con_5;
                        objetoBuscar.con_6 = item.con_6;
                        objetoBuscar.cant_con_1 = item.cant_con_1;
                        objetoBuscar.cant_con_2 = item.cant_con_2;
                        objetoBuscar.cant_con_3 = item.cant_con_3;
                        objetoBuscar.cant_con_4 = item.cant_con_4;
                        objetoBuscar.cant_con_5 = item.cant_con_5;
                        objetoBuscar.cant_con_6 = item.cant_con_6;
                        objetoBuscar.min_con_1 = item.min_con_1;
                        objetoBuscar.min_con_2 = item.min_con_2;
                        objetoBuscar.min_con_3 = item.min_con_3;
                        objetoBuscar.min_con_4 = item.min_con_4;
                        objetoBuscar.min_con_5 = item.min_con_5;
                        objetoBuscar.min_con_6 = item.min_con_6;
                        objetoBuscar.Tipo = item.Tipo;
                        objetoBuscar.Tipo_Jor = item.Tipo_Jor;
                        objetoBuscar.fuerza_calc = item.fuerza_calc;
                        objetoBuscar.idagrupamiento = item.idagrupamiento;
                        objetoBuscar.apl_trans1 = item.apl_trans1;
                        objetoBuscar.id_trans1 = item.id_trans1;
                        objetoBuscar.apl_trans2 = item.apl_trans2;
                        objetoBuscar.id_trans2 = item.id_trans2;
                        objetoBuscar.apl_trans3 = item.apl_trans3;
                        objetoBuscar.id_trans3 = item.id_trans3;
                        objetoBuscar.apl_trans4 = item.apl_trans4;
                        objetoBuscar.id_trans4 = item.id_trans4;
                        objetoBuscar.apl_trans5 = item.apl_trans5;
                        objetoBuscar.id_trans5 = item.id_trans5;
                        objetoBuscar.apl_trans6 = item.apl_trans6;
                        objetoBuscar.id_trans6 = item.id_trans6;
                        objetoBuscar.apl_ben1 = item.apl_ben1;
                        objetoBuscar.id_ben1 = item.id_ben1;
                        objetoBuscar.apl_ben2 = item.apl_ben2;
                        objetoBuscar.id_ben2 = item.id_ben2;
                        objetoBuscar.apl_ben3 = item.apl_ben3;
                        objetoBuscar.id_ben3 = item.id_ben3;
                        objetoBuscar.apl_ben4 = item.apl_ben4;
                        objetoBuscar.id_ben4 = item.id_ben4;
                        objetoBuscar.apl_ben5 = item.apl_ben5;
                        objetoBuscar.id_ben5 = item.id_ben5;
                        objetoBuscar.apl_ben6 = item.apl_ben6;
                        objetoBuscar.id_ben6 = item.id_ben6;
                        objetoBuscar.conc_ben1 = item.conc_ben1;
                        objetoBuscar.conc_ben2 = item.conc_ben2;
                        objetoBuscar.conc_ben3 = item.conc_ben3;
                        objetoBuscar.conc_ben4 = item.conc_ben4;
                        objetoBuscar.conc_ben5 = item.conc_ben5;
                        objetoBuscar.conc_ben6 = item.conc_ben6;
                        objetoBuscar.apl_trans_post = item.apl_trans_post;
                        objetoBuscar.id_trans_post = item.id_trans_post;
                        objetoBuscar.apl_redond_entrada = item.apl_redond_entrada;
                        objetoBuscar.cant_redond_entrada = item.cant_redond_entrada;
                        objetoBuscar.auto_pan = item.auto_pan;
                        objetoBuscar.ColorId = item.ColorId;

                        _context.Ph_Turnos.Update(objetoBuscar);
                    }
                    else
                    {
                        item.PaletaColor = null;
                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del PhTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del PhTurno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina_Turno:  Metodo borrado de datos de la tabla Ph_Turnos
        /// </summary>
        /// <param name="idturno"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Turno(int idturno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var descansos = await _context.Ph_Descansos_Turnos.Where(e => e.IDTURNO == idturno).ToListAsync();

                if (descansos is not null)
                {
                    _context.Ph_Descansos_Turnos.RemoveRange(descansos);
                    await _context.SaveChangesAsync();
                }

                var rolesTurnos = await _context.Ph_Roles_Turnos.Where(e => e.IDTURNO == idturno).ToListAsync();

                if (rolesTurnos is not null)
                {
                    _context.Ph_Roles_Turnos.RemoveRange(rolesTurnos);
                    await _context.SaveChangesAsync();
                }

                cTurno? model = await _context.Ph_Turnos
                    .FirstOrDefaultAsync(e => e.IdTurno == idturno);

                if (model is not null)
                {
                    _context.Ph_Turnos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el PhTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el PhTurno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener lista de Horarios
        public async Task<List<cPh_Horarios>> GetHorarios()
        {
            List<cPh_Horarios> horarios = new();
            try
            {
                horarios = (from e in await _context.Ph_Horarios
                            .Include(e => e.Ph_HorarioTurno)
                            .ToListAsync()
                            select new cPh_Horarios
                            {
                                IDHORARIO = e.IDHORARIO,
                                DESCRIPCION = e.DESCRIPCION,
                                Ph_HorarioTurno = e.Ph_HorarioTurno == null ? null :
                                                (from ht in e.Ph_HorarioTurno
                                                 select new cPh_HorarioTurno
                                                 {
                                                     IDHORARIO = ht.IDHORARIO,
                                                     ID_DIA = ht.ID_DIA,
                                                     T_1 = ht.T_1,
                                                     T_2 = ht.T_2,
                                                     T_3 = ht.T_3,
                                                     T_4 = ht.T_4,
                                                     T_5 = ht.T_5,
                                                 }).ToList(),
        
                            }
                            ).ToList();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetHorarios: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return horarios;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener un Horario especifico
        //Parametros: idHorario=Id de horarios a buscar
        public async Task<cPh_Horarios> GetHorarios(int IDHORARIO)
        {
            cPh_Horarios? horarios = new();
            try
            {
                horarios = await _context.Ph_Horarios.FirstOrDefaultAsync(e => e.IDHORARIO == IDHORARIO);

                horarios = (from e in await _context.Ph_Horarios
                                .Include(e => e.Ph_HorarioTurno)
                                .Where(e => e.IDHORARIO == IDHORARIO)
                                .ToListAsync()
                            select new cPh_Horarios
                            {
                                IDHORARIO = e.IDHORARIO,
                                DESCRIPCION = e.DESCRIPCION,
                                Ph_HorarioTurno = e.Ph_HorarioTurno == null ? null :
                                                (from ht in e.Ph_HorarioTurno
                                                 select new cPh_HorarioTurno
                                                 {
                                                     IDHORARIO = ht.IDHORARIO,
                                                     ID_DIA = ht.ID_DIA,
                                                     T_1 = ht.T_1,
                                                     T_2 = ht.T_2,
                                                     T_3 = ht.T_3,
                                                     T_4 = ht.T_4,
                                                     T_5 = ht.T_5,
                                                 }).ToList(),
                            }
                            ).FirstOrDefault();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetHorarios: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return horarios;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Sincronizar Horarios
        //Parametro: Recibe una instancia de horarios, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Horarios(IEnumerable<cPh_Horarios> horarios)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                IEnumerable<cPh_HorarioTurno> phHorarioTurnoList = null;

                await _context.Database.BeginTransactionAsync();

                foreach (var horario in horarios)
                {
                    phHorarioTurnoList = horario.Ph_HorarioTurno;

                    cPh_Horarios? hora = await _context.Ph_Horarios
                                                        .FirstOrDefaultAsync(e => e.IDHORARIO == horario.IDHORARIO);
                    //si el horario existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (hora is not null)
                    {
                        hora.DESCRIPCION = horario.DESCRIPCION;
                        _context.Ph_Horarios.Update(hora);
                    }
                    else
                    {
                        horario.Ph_HorarioTurno = null;
                        _context.Add(horario);
                    }
                    await _context.SaveChangesAsync();

                    if (phHorarioTurnoList is not null)
                    {
                        var resp = await Sincronizar_HorarioTurno(phHorarioTurnoList);
                        if (resp.Id != "0")
                        {
                            respuesta.Id = "1";
                            respuesta.Respuesta = "Error";
                            respuesta.Descripcion = resp.Descripcion;
                            await _context.Database.RollbackTransactionAsync();
                            return respuesta;
                        }
                    }

                }
                await _context.Database.CommitTransactionAsync();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios. Detalle de Error: " + e.InnerException.Message;

                await _context.Database.RollbackTransactionAsync();
            }

            return respuesta;

        }
        /// <summary>
        /// Elimina_PhHorarios:  Metodo boorado de datos de la tabla PhHorarios
        /// </summary>
        /// <param name="IDHORARIO"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Horarios(string IDHORARIO)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cPh_Horarios? model = await _context.Ph_Horarios
                    .FirstOrDefaultAsync(e => e.IDHORARIO == int.Parse(IDHORARIO));

                if (model is not null)
                {
                    _context.Ph_Horarios.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Horario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Horario. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener lista de Horarios
        public async Task<List<cPh_HorarioTurno>> GetHorario_Turno()
        {
            List<cPh_HorarioTurno> horario_turno = new();
            try
            {
                horario_turno = await _context.Ph_Horario_Turnos.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetHorario_Turno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return horario_turno;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener un Horario Turno especifico
        //Parametros: IDHORARIO= Id de horario turno a buscar
        public async Task<cPh_HorarioTurno> GetHorario_Turno(int IDHORARIO)
        {
            cPh_HorarioTurno? horario_turno = new();
            try
            {
                horario_turno = await _context.Ph_Horario_Turnos.FirstOrDefaultAsync(e => e.IDHORARIO == IDHORARIO);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetHorario_Turno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return horario_turno;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Sincronizar Horario Turno
        //Parametro: Recibe una instancia de HorarioTurno, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_HorarioTurno(IEnumerable<cPh_HorarioTurno> horario_turno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var horario in horario_turno)
                {
                    cPh_HorarioTurno? hora = await _context.Ph_Horario_Turnos
                                                        .FirstOrDefaultAsync(e => e.IDHORARIO == horario.IDHORARIO && e.ID_DIA == horario.ID_DIA);
                    //si el horario existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (hora is not null)
                    {
                        hora.IDHORARIO = horario.IDHORARIO;
                        hora.ID_DIA = horario.ID_DIA;
                        hora.T_1 = horario.T_1;
                        hora.T_2 = horario.T_2;
                        hora.T_3 = horario.T_3;
                        hora.T_4 = horario.T_4;
                        hora.T_5 = horario.T_5;


                        _context.Ph_Horario_Turnos.Update(hora);
                    }
                    else
                    {
                        _context.Add(horario);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios Turnos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios Turnos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Ph_Horario_Turno:  Metodo borrado de datos de la tabla Ph_Horario_Turno
        /// </summary>
        /// <param name="IDHORARIO"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Horario_Turno(string IDHORARIO)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPh_HorarioTurno? model = await _context.Ph_Horario_Turnos
                    .FirstOrDefaultAsync(e => e.IDHORARIO == int.Parse(IDHORARIO));

                if (model is not null)
                {
                    _context.Ph_Horario_Turnos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Horario Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Horario Turno. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-13-12
        /// <summary>
        /// GetPhRol: Obtener lista de registros de la tabla PH_ROLES
        /// </summary>
        /// <returns>Lista de cPh_Rol </returns>
        /// 
        public async Task<List<cPh_Rol>> GetPhRol()
        {
            List<cPh_Rol> roles = new();
            try
            {
                roles = await (from r in _context.Ph_Roles
                                .Include(r => r.cPh_RolTurno)!.ThenInclude(rt => rt.cTurno)
                               select new cPh_Rol
                               {
                                   IDROL = r.IDROL,
                                   DESCRIPCION = r.DESCRIPCION,
                                   cPh_RolTurno = r.cPh_RolTurno == null ? null :
                                   (from e in r.cPh_RolTurno.ToList()
                                    select new cPh_RolTurno
                                    {
                                        IDREGISTRO = e.IDREGISTRO,
                                        IDROL = e.IDROL,
                                        IDTURNO = e.IDTURNO,
                                        cTurno = e.cTurno == null ? null : new cTurno
                                        {
                                            IdTurno = e.cTurno.IdTurno,
                                            Descripcion = e.cTurno.Descripcion,
                                            HEntra = e.cTurno.HEntra,
                                            HSale = e.cTurno.HSale,
                                            tar_apl = e.cTurno.tar_apl,
                                            ant_apl = e.cTurno.ant_apl,
                                            des_1_in = e.cTurno.des_1_in,
                                            des_1_out = e.cTurno.des_1_out,
                                            des_2_in = e.cTurno.des_2_in,
                                            des_2_out = e.cTurno.des_2_out,
                                            des_3_in = e.cTurno.des_3_in,
                                            des_3_out = e.cTurno.des_3_out,
                                            apl_des_1 = e.cTurno.apl_des_1,
                                            apl_des_2 = e.cTurno.apl_des_2,
                                            apl_des_3 = e.cTurno.apl_des_3,
                                            des_1_tiem = e.cTurno.des_1_tiem,
                                            des_2_tiem = e.cTurno.des_2_tiem,
                                            des_3_tiem = e.cTurno.des_3_tiem,
                                            marca_des_1 = e.cTurno.marca_des_1,
                                            marca_des_2 = e.cTurno.marca_des_2,
                                            marca_des_3 = e.cTurno.marca_des_3,
                                            tar_tiem = e.cTurno.tar_tiem,
                                            ant_tiem = e.cTurno.ant_tiem,
                                            con_1 = e.cTurno.con_1,
                                            con_2 = e.cTurno.con_2,
                                            con_3 = e.cTurno.con_3,
                                            con_4 = e.cTurno.con_4,
                                            con_5 = e.cTurno.con_5,
                                            con_6 = e.cTurno.con_6,
                                            cant_con_1 = e.cTurno.cant_con_1,
                                            cant_con_2 = e.cTurno.cant_con_2,
                                            cant_con_3 = e.cTurno.cant_con_3,
                                            cant_con_4 = e.cTurno.cant_con_4,
                                            cant_con_5 = e.cTurno.cant_con_5,
                                            cant_con_6 = e.cTurno.cant_con_6,
                                            min_con_1 = e.cTurno.min_con_1,
                                            min_con_2 = e.cTurno.min_con_2,
                                            min_con_3 = e.cTurno.min_con_3,
                                            min_con_4 = e.cTurno.min_con_4,
                                            min_con_5 = e.cTurno.min_con_5,
                                            min_con_6 = e.cTurno.min_con_6,
                                            Tipo = e.cTurno.Tipo,
                                            Tipo_Jor = e.cTurno.Tipo_Jor,
                                            fuerza_calc = e.cTurno.fuerza_calc,
                                            idagrupamiento = e.cTurno.idagrupamiento,
                                            apl_trans1 = e.cTurno.apl_trans1,
                                            id_trans1 = e.cTurno.id_trans1,
                                            apl_trans2 = e.cTurno.apl_trans2,
                                            id_trans2 = e.cTurno.id_trans2,
                                            apl_trans3 = e.cTurno.apl_trans3,
                                            id_trans3 = e.cTurno.id_trans3,
                                            apl_trans4 = e.cTurno.apl_trans4,
                                            id_trans4 = e.cTurno.id_trans4,
                                            apl_trans5 = e.cTurno.apl_trans5,
                                            id_trans5 = e.cTurno.id_trans5,
                                            apl_trans6 = e.cTurno.apl_trans6,
                                            id_trans6 = e.cTurno.id_trans6,
                                            apl_ben1 = e.cTurno.apl_ben1,
                                            id_ben1 = e.cTurno.id_ben1,
                                            apl_ben2 = e.cTurno.apl_ben2,
                                            id_ben2 = e.cTurno.id_ben2,
                                            apl_ben3 = e.cTurno.apl_ben3,
                                            id_ben3 = e.cTurno.id_ben3,
                                            apl_ben4 = e.cTurno.apl_ben4,
                                            id_ben4 = e.cTurno.id_ben4,
                                            apl_ben5 = e.cTurno.apl_ben5,
                                            id_ben5 = e.cTurno.id_ben5,
                                            apl_ben6 = e.cTurno.apl_ben6,
                                            id_ben6 = e.cTurno.id_ben6,
                                            conc_ben1 = e.cTurno.conc_ben1,
                                            conc_ben2 = e.cTurno.conc_ben2,
                                            conc_ben3 = e.cTurno.conc_ben3,
                                            conc_ben4 = e.cTurno.conc_ben4,
                                            conc_ben5 = e.cTurno.conc_ben5,
                                            conc_ben6 = e.cTurno.conc_ben6,
                                            apl_trans_post = e.cTurno.apl_trans_post,
                                            id_trans_post = e.cTurno.id_trans_post,
                                            apl_redond_entrada = e.cTurno.apl_redond_entrada,
                                            cant_redond_entrada = e.cTurno.cant_redond_entrada,
                                            auto_pan = e.cTurno.auto_pan,
                                            ColorId = e.cTurno.ColorId,
                                        }

                                    }).ToList(),
                               }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhRol: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return roles;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        //Obtener un Rol especifico
        //Parametros: idrol=id a buscar
        public async Task<cPh_Rol> GetPhRol(int idrol)
        {
            cPh_Rol? roles = new();
            //cPh_Rol? roles = new cPh_Rol();
            try
            {
                roles = await (from r in _context.Ph_Roles
                                .Include(r => r.cPh_RolTurno)!.ThenInclude(rt => rt.cTurno)
                               where r.IDROL == idrol
                               select new cPh_Rol
                               {
                                   IDROL = r.IDROL,
                                   DESCRIPCION = r.DESCRIPCION,
                                   cPh_RolTurno = r.cPh_RolTurno ==null? null:
                                   (from e in r.cPh_RolTurno
                                    select new cPh_RolTurno
                                    {
                                        IDREGISTRO = e.IDREGISTRO,
                                        IDROL = e.IDROL,
                                        IDTURNO = e.IDTURNO,
                                        cTurno = e.cTurno == null ? null : new cTurno
                                        {
                                            IdTurno = e.cTurno.IdTurno,
                                            Descripcion = e.cTurno.Descripcion,
                                            HEntra = e.cTurno.HEntra,
                                            HSale = e.cTurno.HSale,
                                            tar_apl = e.cTurno.tar_apl,
                                            ant_apl = e.cTurno.ant_apl,
                                            des_1_in = e.cTurno.des_1_in,
                                            des_1_out = e.cTurno.des_1_out,
                                            des_2_in = e.cTurno.des_2_in,
                                            des_2_out = e.cTurno.des_2_out,
                                            des_3_in = e.cTurno.des_3_in,
                                            des_3_out = e.cTurno.des_3_out,
                                            apl_des_1 = e.cTurno.apl_des_1,
                                            apl_des_2 = e.cTurno.apl_des_2,
                                            apl_des_3 = e.cTurno.apl_des_3,
                                            des_1_tiem = e.cTurno.des_1_tiem,
                                            des_2_tiem = e.cTurno.des_2_tiem,
                                            des_3_tiem = e.cTurno.des_3_tiem,
                                            marca_des_1 = e.cTurno.marca_des_1,
                                            marca_des_2 = e.cTurno.marca_des_2,
                                            marca_des_3 = e.cTurno.marca_des_3,
                                            tar_tiem = e.cTurno.tar_tiem,
                                            ant_tiem = e.cTurno.ant_tiem,
                                            con_1 = e.cTurno.con_1,
                                            con_2 = e.cTurno.con_2,
                                            con_3 = e.cTurno.con_3,
                                            con_4 = e.cTurno.con_4,
                                            con_5 = e.cTurno.con_5,
                                            con_6 = e.cTurno.con_6,
                                            cant_con_1 = e.cTurno.cant_con_1,
                                            cant_con_2 = e.cTurno.cant_con_2,
                                            cant_con_3 = e.cTurno.cant_con_3,
                                            cant_con_4 = e.cTurno.cant_con_4,
                                            cant_con_5 = e.cTurno.cant_con_5,
                                            cant_con_6 = e.cTurno.cant_con_6,
                                            min_con_1 = e.cTurno.min_con_1,
                                            min_con_2 = e.cTurno.min_con_2,
                                            min_con_3 = e.cTurno.min_con_3,
                                            min_con_4 = e.cTurno.min_con_4,
                                            min_con_5 = e.cTurno.min_con_5,
                                            min_con_6 = e.cTurno.min_con_6,
                                            Tipo = e.cTurno.Tipo,
                                            Tipo_Jor = e.cTurno.Tipo_Jor,
                                            fuerza_calc = e.cTurno.fuerza_calc,
                                            idagrupamiento = e.cTurno.idagrupamiento,
                                            apl_trans1 = e.cTurno.apl_trans1,
                                            id_trans1 = e.cTurno.id_trans1,
                                            apl_trans2 = e.cTurno.apl_trans2,
                                            id_trans2 = e.cTurno.id_trans2,
                                            apl_trans3 = e.cTurno.apl_trans3,
                                            id_trans3 = e.cTurno.id_trans3,
                                            apl_trans4 = e.cTurno.apl_trans4,
                                            id_trans4 = e.cTurno.id_trans4,
                                            apl_trans5 = e.cTurno.apl_trans5,
                                            id_trans5 = e.cTurno.id_trans5,
                                            apl_trans6 = e.cTurno.apl_trans6,
                                            id_trans6 = e.cTurno.id_trans6,
                                            apl_ben1 = e.cTurno.apl_ben1,
                                            id_ben1 = e.cTurno.id_ben1,
                                            apl_ben2 = e.cTurno.apl_ben2,
                                            id_ben2 = e.cTurno.id_ben2,
                                            apl_ben3 = e.cTurno.apl_ben3,
                                            id_ben3 = e.cTurno.id_ben3,
                                            apl_ben4 = e.cTurno.apl_ben4,
                                            id_ben4 = e.cTurno.id_ben4,
                                            apl_ben5 = e.cTurno.apl_ben5,
                                            id_ben5 = e.cTurno.id_ben5,
                                            apl_ben6 = e.cTurno.apl_ben6,
                                            id_ben6 = e.cTurno.id_ben6,
                                            conc_ben1 = e.cTurno.conc_ben1,
                                            conc_ben2 = e.cTurno.conc_ben2,
                                            conc_ben3 = e.cTurno.conc_ben3,
                                            conc_ben4 = e.cTurno.conc_ben4,
                                            conc_ben5 = e.cTurno.conc_ben5,
                                            conc_ben6 = e.cTurno.conc_ben6,
                                            apl_trans_post = e.cTurno.apl_trans_post,
                                            id_trans_post = e.cTurno.id_trans_post,
                                            apl_redond_entrada = e.cTurno.apl_redond_entrada,
                                            cant_redond_entrada = e.cTurno.cant_redond_entrada,
                                            auto_pan = e.cTurno.auto_pan,
                                            ColorId = e.cTurno.ColorId,
                                        }

                                    }).ToList(),
                               }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhRol: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return roles;
        }

        /// <summary>
        /// Sincronizar_PhRol: metodo para sincronizar los Roles 
        /// </summary>
        /// <param name="phRoles"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_PhRol(IEnumerable<cPh_Rol> phRoles)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                await _context.Database.BeginTransactionAsync();

                foreach (var item in phRoles)
                {

                    IEnumerable<cPh_RolTurno>? phRolTurnoList = item.cPh_RolTurno;

                    cPh_Rol? objetoBuscar = await _context.Ph_Roles
                                    .FirstOrDefaultAsync(e => e.IDROL == item.IDROL);
                    //si el rol existe se actualiza descripción
                    //de lo contrario se agrega el registro*
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.IDROL = item.IDROL;
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;

                        _context.Ph_Roles.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                    if (phRolTurnoList is not null)
                    {
                        var resp = await Sincronizar_RolTurno(phRolTurnoList);
                        if (resp.Id != "0")
                        {
                            await _context.Database.RollbackTransactionAsync();
                            respuesta.Id = "1";
                            respuesta.Respuesta = "Error";
                            respuesta.Descripcion = resp.Descripcion;
                            return respuesta;
                        }
                    }


                }
                
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await _context.Database.RollbackTransactionAsync();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Rol. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Rol. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina_PhRol:  Metodo borrado de datos de la tabla Ph_Roles
        /// </summary>
        /// <param name="idrol"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhRol(int idrol)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                List<cPh_RolTurno>? listRolesTurnos = await _context.Ph_Roles_Turnos.Where(e => e.IDROL == idrol).ToListAsync();

                if (listRolesTurnos is not null && listRolesTurnos.Count > 0)
                    _context.Ph_Roles_Turnos.RemoveRange(listRolesTurnos);

                cPh_Rol? model = await _context.Ph_Roles.FirstOrDefaultAsync(e => e.IDROL == idrol);

                if (model is not null)
                    _context.Ph_Roles.Remove(model);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Rol. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Rol. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Incidencias

        public async Task<List<cIncidencia>> GetIncidencia()
        {


            List<cIncidencia> incidencia = new();
            try
            {
                incidencia = await _context.Incidencias
                        .ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetIncidencia: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return incidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener una Incidencia especifica
        //Parametros: codigo=Codigo de incidencia a buscar
        public async Task<cIncidencia> GetIncidencia(int id)
        {
            cIncidencia? incidencia = new();
            try
            {
                incidencia = await _context.Incidencias.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetIncidencia: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return incidencia;
        }
        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Incidencia
        //Parametro: Recibe una instancia de Incidencia, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Incidencia(IEnumerable<cIncidencia> incidencias)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var incidencia in incidencias)
                {
                    cIncidencia? incident = await _context.Incidencias
                                    .Where(e => e.Id == incidencia.Id)
                                    .FirstOrDefaultAsync();

                    if (incident is not null)
                    {
                        //incident.Id = incidencia.Id;
                        incident.Descripcion = incidencia.Descripcion;
                        incident.Codigo = incidencia.Codigo;
                        incident.nom_conector = incidencia.nom_conector;
                        incident.id_pago = incidencia.id_pago;
                        incident.tipo = incidencia.tipo;
                        incident.ed_tiempo = incidencia.ed_tiempo;
                        incident.requiere_accper = incidencia.requiere_accper;

                        _context.Incidencias.Update(incident);
                    }
                    else
                    {
                        incidencia.cAccionPersonal = null;
                        incidencia.Id = 0;
                        _context.Add(incidencia);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Incidencias. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Incidencias. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        //Obtener lista de Marcas Resumen
        /// <summary>
        /// Elimina_Incidencia:  Metodo borrado de datos de la tabla Incidencias
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Incidencia(int id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                cIncidencia? model = await _context.Incidencias
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (model is not null)
                {
                    _context.Incidencias.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Incidencia. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Incidencia. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un registro de Acción de Personal
        public async Task<cAccionPersonal> GetAccionPersonal(long idregistro)
        {
            cAccionPersonal? accionPersonal = new();
            try
            {
                accionPersonal = await (from ap in _context.Acciones_Personal
                                        .Include(e=>e.cIncidencia)
                                        .Where(e => e.IdRegistro == idregistro)
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = ap.cIncidencia==null?"":ap.cIncidencia.nom_conector,
                                        }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal!;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
                accionPersonal = await (from ap in _context.Acciones_Personal
                                        .Include(e => e.cIncidencia)
                                        .Where(e => e.IdPlanilla == IdPlanilla && e.Inicio >= FechaInicio && e.Fin <= FechaFin)
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = ap.cIncidencia == null ? "" : ap.cIncidencia.nom_conector,
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin, string usuario)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {


                accionPersonal = await (from ap in _context.Acciones_Personal
                                        .Include(e => e.cIncidencia)
                                        .Where(e => e.IdPlanilla == IdPlanilla
                                                    && e.Inicio >= FechaInicio
                                                    && e.Fin <= FechaFin
                                                    && (e.Usuario == usuario || e.Usuario == (usuario + "\\")))
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = ap.cIncidencia == null ? "" : ap.cIncidencia.nom_conector,
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string usuario, char estado)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {

                accionPersonal = await (from ap in _context.Acciones_Personal
                                        .Include(e => e.cIncidencia)
                                        .Where(e => e.IdPlanilla == IdPlanilla
                                        && e.Estado == estado
                                        && (e.Usuario!.Contains(usuario)))
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = ap.cIncidencia == null ? "" : ap.cIncidencia.nom_conector,
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonalPorEstado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-07-16
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string FechaInicio, string FechaFin, char estado, int idincidencia, string idgrupo)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}");

                idgrupo = HttpUtility.UrlDecode(idgrupo);
                string[] ListGrupos = idgrupo?.Split(',');
                List<int> groupIds = ListGrupos
                    .Select(int.Parse)
                    .ToList();

                var accionPersonalConsulta = await _context.Acciones_Personal
                                        .Include(e => e.cIncidencia)
                                        .Include(e => e.cEmpleado)
                                        .Where(e => e.IdPlanilla == IdPlanilla
                                            && e.Estado == (estado == 'X' ? e.Estado : estado) &&
                                            e.Inicio >= fechaMovInicio &&
                                            e.Fin <= fechaMovFinal &&
                                            e.IdIncidencia == (idincidencia == -1 ? e.IdIncidencia : idincidencia)).ToListAsync();

                var filteredAccionesPersonal = (from ap in accionPersonalConsulta
                                                join g in groupIds on ap.cEmpleado.IdGrupo equals g
                                                select ap).ToList();

                accionPersonal = filteredAccionesPersonal.Select(ap => new cAccionPersonal
                {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = ap.cIncidencia == null ? "" : ap.cIncidencia.nom_conector,
                                            cEmpleado = ap.cEmpleado == null?null:
                                                new cEmpleado
                                                {
                                                    IdNumero = ap.cEmpleado.IdNumero,
                                                    IdPlanilla = ap.cEmpleado.IdPlanilla,
                                                    Nombre = ap.cEmpleado.Nombre,
                                                    Tarjeta = ap.cEmpleado.Tarjeta,
                                                    Identificacion = ap.cEmpleado.Identificacion,
                                                    IdGrupo = ap.cEmpleado.IdGrupo,
                                                    IdDepartamento = ap.cEmpleado.IdDepartamento,
                                                    IdHorario = ap.cEmpleado.IdHorario,
                                                    Estado = ap.cEmpleado.Estado,
                                                    IdAgrupamiento = ap.cEmpleado.IdAgrupamiento,
                                                    foto = ap.cEmpleado.foto,
                                                    IdCCosto = ap.cEmpleado.IdCCosto,
                                                    exporta = ap.cEmpleado.exporta,
                                                    ubicacion = ap.cEmpleado.ubicacion,
                                                    rubro1 = ap.cEmpleado.rubro1,
                                                    rubro2 = ap.cEmpleado.rubro2,
                                                    rubro3 = ap.cEmpleado.rubro3,
                                                    rubro4 = ap.cEmpleado.rubro4,
                                                    rubro5 = ap.cEmpleado.rubro5,
                                                    rubro6 = ap.cEmpleado.rubro6,
                                                    rubro7 = ap.cEmpleado.rubro7,
                                                    rubro8 = ap.cEmpleado.rubro8,
                                                    rubro9 = ap.cEmpleado.rubro9,
                                                    rubro10 = ap.cEmpleado.rubro10,
                                                    rubro11 = ap.cEmpleado.rubro11,
                                                    rubro12 = ap.cEmpleado.rubro12,
                                                    rubro13 = ap.cEmpleado.rubro13,
                                                    rubro14 = ap.cEmpleado.rubro14,
                                                    rubro15 = ap.cEmpleado.rubro15,
                                                    rubro16 = ap.cEmpleado.rubro16,
                                                    rubro17 = ap.cEmpleado.rubro17,
                                                    rubro18 = ap.cEmpleado.rubro18,
                                                    rubro19 = ap.cEmpleado.rubro19,
                                                    rubro20 = ap.cEmpleado.rubro20,
                                                    rubro21 = ap.cEmpleado.rubro21,
                                                    rubro22 = ap.cEmpleado.rubro22,
                                                    rubro23 = ap.cEmpleado.rubro23,
                                                    rubro24 = ap.cEmpleado.rubro24,
                                                    rubro25 = ap.cEmpleado.rubro25,
                                                    Fecha_Ingreso = ap.cEmpleado.Fecha_Ingreso,
                                                    Email = ap.cEmpleado.Email,
                                                    Tipo_Marca = ap.cEmpleado.Tipo_Marca,
                                                    inicio_rol = ap.cEmpleado.inicio_rol,
                                                    web_pass = ap.cEmpleado.web_pass,
                                                    id_transfo_conc = ap.cEmpleado.id_transfo_conc,
                                                    widioma = ap.cEmpleado.widioma,
                                                    def_cc = ap.cEmpleado.def_cc,
                                                    def_py = ap.cEmpleado.def_py,
                                                    def_fase = ap.cEmpleado.def_fase,
                                                    global_clave = ap.cEmpleado.global_clave,
                                                    Fecha_Salida = ap.cEmpleado.Fecha_Salida,
                                                    global_code = ap.cEmpleado.global_code,
                                                    fecha_act_code = ap.cEmpleado.fecha_act_code,
                                                },
                                            cIncidencia = ap.cIncidencia == null ? null :
                                                new cIncidencia
                                                {
                                                    Id = ap.cIncidencia.Id,
                                                    Codigo = ap.cIncidencia.Codigo,
                                                    Descripcion = ap.cIncidencia.Descripcion,
                                                    id_pago = ap.cIncidencia.id_pago,
                                                    nom_conector = ap.cIncidencia.nom_conector,
                                                    tipo = ap.cIncidencia.tipo,
                                                    ed_tiempo = ap.cIncidencia.ed_tiempo,
                                                    requiere_accper = ap.cIncidencia.requiere_accper,
                                                    marca_web = ap.cIncidencia.marca_web,
                                                }
                                        }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonalPorEstado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, char estado)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {


                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdPlanilla == IdPlanilla
                                                                              && e.Estado == estado)
                                        join inc in _context.Incidencias on ap.IdIncidencia equals inc.Id
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = inc.nom_conector
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonalPorEstado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-30
        /// <summary>
        /// GetAccionPersonalPorPeriodo: Acciones de Personal 
        /// </summary>
        /// <param name="IdPlanilla"></param>
        /// <param name="usuario"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorPeriodo(string idnumero, string fecha, string IdPlanilla)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdNumero == idnumero
                                                                              && e.IdPlanilla == IdPlanilla
                                                                              && (e.Inicio >= periodoVigente.inicio && e.Inicio <= periodoVigente.fin && e.Fin <= periodoVigente.fin)
                                                                              || (e.Inicio < periodoVigente.inicio && e.Fin >= periodoVigente.inicio && e.Fin <= periodoVigente.fin)
                                                                              || (e.Inicio >= periodoVigente.inicio && e.Inicio <= periodoVigente.fin && e.Fin > periodoVigente.fin)
                                                                              || (e.Inicio < periodoVigente.inicio && e.Fin > periodoVigente.fin))
                                        join inc in _context.Incidencias on ap.IdIncidencia equals inc.Id
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = inc.nom_conector
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetAccionPersonalPorPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar acciones de personal
        //Parametro: Recibe una instancia de cAccionPersonal, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_AccionPersonal(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    accion.IdAccion = 0;
                    accion.cIncidencia = null;
                    accion.cEmpleado = null;

                    _context.Add(accion);
                    await _context.SaveChangesAsync();

                    var ultimaAccion = await _context.Acciones_Personal.MaxAsync(e => e.IdRegistro);

                    await EjecutaAplicaAccionPersonal(ultimaAccion);

                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Centros de Costo
        //Parametro: Recibe una instancia de centro de costo, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_AccionPersonal_AutoGestion(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    var planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.nom_conector == accion.IdPlanilla);

                    if (planilla is not null)
                    {
                        accion.IdAccion = 0;
                        accion.IdPlanilla = planilla.idplanilla;
                        accion.cIncidencia = null;
                        accion.cEmpleado = null;

                        _context.Add(accion);
                        await _context.SaveChangesAsync();

                        var ultimaAccion = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.SolicitudId == accion.SolicitudId);

                        if (ultimaAccion is not null)
                            await EjecutaAplicaAccionPersonal(ultimaAccion.IdRegistro);
                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "No logró encontrar el id de planilla asociado a nom_conector";
                    }


                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Centros de Costo
        //Parametro: Recibe una instancia de centro de costo, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_AccionPersonalNomConector(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {

                    var incidencia = await _context.Incidencias.FirstOrDefaultAsync(e => e.nom_conector == accion.Nom_Conector);

                    if (incidencia is not null)
                    {
                        var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.IdRegistro == accion.IdRegistro);

                        if (accionbuscar is not null)
                        {
                            accionbuscar.Inicio = accion.Inicio;
                            accionbuscar.Fin = accion.Fin;
                            accionbuscar.Dias = accion.Dias;
                            accionbuscar.Dias_Apl = accion.Dias_Apl;
                            accionbuscar.IdAccion = accion.IdAccion;
                            accionbuscar.Comentario = accion.Comentario;
                            if (accion.Estado != 'A')
                            {
                                accionbuscar.Estado = accion.Estado;
                            }

                            _context.Acciones_Personal.Update(accionbuscar);
                            await _context.SaveChangesAsync();

                            if (accion.Estado == 'A')
                            {
                                await EjecutaAplicaAccionPersonal(accionbuscar.IdRegistro);
                            }
                        }
                        else
                        {
                            accion.IdIncidencia = incidencia.Id;
                            accion.cIncidencia = null;
                            accion.cEmpleado = null;

                            _context.Add(accion);
                            await _context.SaveChangesAsync();

                            var ultimaAccion = await _context.Acciones_Personal.MaxAsync(e => e.IdRegistro);

                            await EjecutaAplicaAccionPersonal(ultimaAccion);
                        }

                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "No logró encontrar la incidencia asociada a nom_conector";
                    }

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        //Sincronizar AccionPersonal PreJustificacion
        //Parametro: Recibe una lista de Acciones de Personal y las crea en GeoTime
        public async Task<EventResponse> Sincronizar_AccionPersonal_PreJustificacion(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.IdRegistro == accion.IdRegistro);

                    if (accionbuscar is not null)
                    {
                        accionbuscar.IdIncidencia = accion.IdIncidencia;
                        accionbuscar.Inicio = accion.Inicio;
                        accionbuscar.Fin = accion.Fin;
                        accionbuscar.Dias = accion.Dias;
                        accionbuscar.Dias_Apl = accion.Dias_Apl;
                        accionbuscar.IdAccion = accion.IdAccion;
                        accionbuscar.Comentario = accion.Comentario;

                        _context.Acciones_Personal.Update(accionbuscar);
                        await _context.SaveChangesAsync();

                        var marcasIncidencias = await GetMarcaIncidencia(accion!.IdNumero!, accion!.IdPlanilla!, accion.Inicio, accion.Fin);

                        foreach (cMarcaIncidencia item in marcasIncidencias)
                        {
                            item.FECHA_JUST = DateTime.Now;
                            item.IDACC = accionbuscar.IdRegistro;
                            item.COMENTARIO = accion.Comentario;

                            _context.Marcas_Incidencias.Update(item);
                            await _context.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        accion.IdAccion = 0;
                        accion.cIncidencia = null;
                        accion.cEmpleado = null;

                        _context.Add(accion);
                        await _context.SaveChangesAsync();

                        var ultimaAccion = await _context.Acciones_Personal.MaxAsync(e => e.IdRegistro);
                        var marcasIncidencias = await GetMarcaIncidencia(accion.IdNumero, accion.IdPlanilla, accion.Inicio, accion.Fin);

                        foreach (cMarcaIncidencia item in marcasIncidencias)
                        {
                            item.FECHA_JUST = DateTime.Now;
                            item.IDACC = ultimaAccion;
                            item.COMENTARIO = accion.Comentario;

                            _context.Marcas_Incidencias.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }



                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2024-07-18
        /// <summary>
        /// Sincronizar_AccionPersonal_CA: Sincroniza acciones de personal provenientes de Control de Asistencia.  No se deben aplicar
        /// </summary>
        /// <param name="accionPersonal">Recibe una lista de Acciones de Personal y las crea en GeoTime</param>
        /// <returns>EventResponse: con el resultado de la operación</returns>
        public async Task<EventResponse> Sincronizar_AccionPersonal_CA(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.IdRegistro == accion.IdRegistro);

                    if (accionbuscar is not null)
                    {
                        accionbuscar.IdIncidencia = accion.IdIncidencia;
                        accionbuscar.Inicio = accion.Inicio;
                        accionbuscar.Fin = accion.Fin;
                        accionbuscar.Dias = accion.Dias;
                        accionbuscar.Dias_Apl = accion.Dias_Apl;
                        accionbuscar.IdAccion = accion.IdAccion;
                        accionbuscar.Comentario = accion.Comentario;

                        _context.Acciones_Personal.Update(accionbuscar);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        accion.IdAccion = 0;
                        accion.cIncidencia = null;
                        accion.cEmpleado = null;

                        _context.Add(accion);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-08-8
        /// <summary>
        /// Sincronizar_AccionPersonal_CAUpdate: Sincroniza acciones de personal provenientes de Control de Asistencia.  No se deben aplicar
        /// </summary>
        /// <param name="accionPersonal">Recibe una lista de Acciones de Personal y las crea en GeoTime</param>
        /// <returns>EventResponse: con el resultado de la operación</returns>
        public async Task<EventResponse> Sincronizar_AccionPersonal_CAUpdate(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.IdRegistro == accion.IdRegistro);

                    if (accionbuscar is not null)
                    {
                        accionbuscar.Estado = accion.Estado;

                        _context.Acciones_Personal.Update(accionbuscar);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        /// <summary>
        /// Elimina_AccionPersonal:  Metodo boorado de datos de la tabla cAccionPersonal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_AccionPersonal(Int64 id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                cAccionPersonal? model = await _context.Acciones_Personal
                    .FirstOrDefaultAsync(e => e.IdRegistro == id);

                if (model is not null)
                {
                    _context.Acciones_Personal.Remove(model);
                    await _context.SaveChangesAsync();

                    if (model!.Estado == 'A')
                    {
                        var marcasIncidencias = await _context.Marcas_Incidencias
                                           .Where(e => e.IDACC == id &&
                                                  e.IDNUMERO==model.IdNumero).ToListAsync();

                        if (marcasIncidencias is not null)
                        {
                            foreach (var item in marcasIncidencias)
                            {
                                _context.Marcas_Incidencias.Remove(item);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Accion de Personal. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Centros de Costo
        public async Task<List<cCentroCosto>> GetCentroCosto()
        {
            List<cCentroCosto> centrosCosto = new();
            try
            {
                centrosCosto = await _context.Ph_CCostos.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetCentroCosto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return centrosCosto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un centro de costo especifico
        //Parametros: idCCosto=centro de costo a buscar
        public async Task<cCentroCosto> GetCentroCosto(string idCCosto)
        {
            cCentroCosto? centrosCosto = new();
            try
            {
                centrosCosto = await _context.Ph_CCostos.FirstOrDefaultAsync(e => e.IdCCosto == idCCosto);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetCentroCosto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return centrosCosto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Centros de Costo
        //Parametro: Recibe una instancia de centro de costo, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Centro_Costo(IEnumerable<cCentroCosto> centrosCosto)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var centroCosto in centrosCosto)
                {
                    cCentroCosto? ceco = await _context.Ph_CCostos
                                    .Where(e => e.IdCCosto == centroCosto.IdCCosto)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (ceco is not null)
                    {
                        ceco.Descripcion = centroCosto.Descripcion;
                        ceco.Distribuye = centroCosto.Distribuye;
                        ceco.Alias_CCosto = centroCosto.Alias_CCosto;

                        _context.Ph_CCostos.Update(ceco);
                    }
                    else
                    {
                        _context.Add(centroCosto);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Centro de Costo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Centro de Costo. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Conceptos
        public async Task<List<cConcepto>> GetConcepto()
        {
            List<cConcepto> concepto = new();
            try
            {
                concepto = await _context.Ph_Conceptos.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return concepto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Concepto especifico
        //Parametros: concepto=concepto a buscar
        public async Task<cConcepto> GetConcepto(string concepto)
        {
            cConcepto? conceptos = new();
            try
            {
                conceptos = await _context.Ph_Conceptos.FirstOrDefaultAsync(e => e.Concepto == concepto);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return conceptos;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Conceptos
        //Parametro: Recibe una instancia de concepto, se verifica si existe, en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Concepto(IEnumerable<cConcepto> conceptos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var concepto in conceptos)
                {
                    cConcepto? concept = await _context.Ph_Conceptos
                                                        .Where(e => e.id == concepto.id)
                                                        .FirstOrDefaultAsync();
                    //si el consepto existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (concept is not null)
                    {
                        concept.id = concepto.id;
                        concept.Concepto = concepto.Concepto;
                        concept.Descripcion = concepto.Descripcion;
                        concept.tipo_j = concepto.tipo_j;
                        concept.tipo_h = concepto.tipo_h;
                        concept.columnar = concepto.columnar;
                        concept.nominaeq = concepto.nominaeq;
                        concept.factor = concepto.factor;
                        concept.tolerancia = concepto.tolerancia;
                        concept.ordinario = concepto.ordinario;
                        concept.autorizado = concepto.autorizado;
                        concept.transferir = concepto.transferir;
                        concept.adicional = concepto.adicional;
                        concept.tipo_ext_alm = concepto.tipo_ext_alm;
                        concept.muestra_resumen = concepto.muestra_resumen;

                        _context.Ph_Conceptos.Update(concept);
                    }
                    else
                    {
                        concepto.cMarcaResumen = null;
                        concepto.cMarcaDistribucion = null;
                        _context.Add(concepto);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Conceptos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Conceptos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Concepto:  Metodo borrado de datos de la tabla Concepto
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Concepto(int id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cConcepto? model = await _context.Ph_Conceptos
                    .FirstOrDefaultAsync(e => e.id == id);

                if (model is not null)
                {
                    _context.Ph_Conceptos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Concepto. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Concepto. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener una Incidencia especifica
        //Parametros: codigo=Codigo de incidencia a buscar
        public async Task<cIncidencia> GetIncidenciaByNomConector(string nom_conector)
        {
            cIncidencia? incidencia = new();
            try
            {
                incidencia = await _context.Incidencias.FirstOrDefaultAsync(e => e.nom_conector == nom_conector);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetIncidenciaByNomConector: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return incidencia;
        }

        public async Task<List<cIncidencia>> GetIncidenciaReqAccPer()
        {
            List<cIncidencia> incidencia = new();
            try
            {
                incidencia = await _context.Incidencias
                        .Where(e => e.requiere_accper == 'T')
                        .ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetIncidenciaReqAccPer: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return incidencia;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas Resumen
        public async Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla, string idPeriodo)
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = (from mr in await _context.Marcas_Resumen.Where(mr=> mr.IdPlanilla == idPlanilla && mr.IdPeriodo == idPeriodo).ToListAsync()
                                       join emp in await _context.Empleados.Where(e=>e.Estado == 'T').ToListAsync() on mr.IdNumero equals emp.IdNumero
                                       select mr
                                       ).ToList();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasResumen: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasResumen;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas Resumen que solo se pueden tranferir al ERP
        public async Task<List<cMarcaResumen>> GetMarcasResumenATransferir(string idPlanilla, string idPeriodo)
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = await( from mr in _context.Marcas_Resumen.Where(e => e.IdPlanilla == idPlanilla && e.IdPeriodo == idPeriodo)
                                       join emp in _context.Empleados.Where(e => e.Estado == 'T') on mr.IdNumero equals emp.IdNumero
                                       join c in _context.Ph_Conceptos.Where(e=>e.transferir=='T') on mr.IdConcepto equals c.id
                                       select mr).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasResumen: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasResumen;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas Resumen
        public async Task<List<cMarcaResumen>> GetMarcasResumen()
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = await (from mr in _context.Marcas_Resumen                                      
                                       select mr
                                       ).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasResumen: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasResumen;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas Resumen
        public async Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla)
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = await (from mr in _context.Marcas_Resumen.Where(mr => mr.IdPlanilla == idPlanilla )
                                       join emp in _context.Empleados.Where(e => e.Estado == 'T') on mr.IdNumero equals emp.IdNumero
                                       select mr
                                       ).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasResumen: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasResumen;
        }

        /// <summary>
        /// GetMarcaResumen:  Proceso para determinar resumen de marcas resumen para el periodo que se deben visualizar en el sistema
        /// </summary>
        /// <param name="IdPlanilla">id de planilla por el que se debe filtrar la informacion</param>
        /// <param name="idperiodo">periodo del reporte</param>
        /// <param name="idnumero">id del empleado que se desea obtener, si se envia un -1 trae todos los empleados</param>
        /// <returns>Lista de marcas resumen del periodo</returns>
        public async Task<IEnumerable<cMarcaResumen>> GetMarcasResumen(string IdPeriodo, string IdPlanilla, string idnumero)
        {
            List<cMarcaResumen>? marcasResumen = new();
            try
            {
                marcasResumen = await (from e in _context.Marcas_Resumen
                                                   .Include(e => e.cConcepto)
                                       .Where(e => e.IdPeriodo == IdPeriodo
                                               && e.IdNumero == idnumero
                                               && e.IdPlanilla == IdPlanilla)
                                           select new cMarcaResumen
                                           {
                                               IdPlanilla = e.IdPlanilla,
                                               IdNumero = e.IdNumero,
                                               IdConcepto = e.IdConcepto,
                                               NominaEq = e.NominaEq,
                                               Cantidad = e.Cantidad,
                                               Monto = e.Monto,
                                               IdCCosto = e.IdCCosto,
                                               Proyecto = e.Proyecto,
                                               Fase = e.Fase,
                                               IdPeriodo = e.IdPeriodo,
                                               cConcepto = e.cConcepto == null ? null :
                                                             new cConcepto
                                                             {
                                                                 id = e.cConcepto.id,
                                                                 Concepto = e.cConcepto.Concepto,
                                                                 Descripcion = e.cConcepto.Descripcion,
                                                                 tipo_j = e.cConcepto.tipo_j,
                                                                 tipo_h = e.cConcepto.tipo_h,
                                                                 columnar = e.cConcepto.columnar,
                                                                 nominaeq = e.cConcepto.nominaeq,
                                                                 factor = e.cConcepto.factor,
                                                                 tolerancia = e.cConcepto.tolerancia,
                                                                 ordinario = e.cConcepto.ordinario,
                                                                 autorizado = e.cConcepto.autorizado,
                                                                 transferir = e.cConcepto.transferir,
                                                                 adicional = e.cConcepto.adicional,
                                                                 tipo_ext_alm = e.cConcepto.tipo_ext_alm,
                                                                 muestra_resumen = e.cConcepto.muestra_resumen,
                                                             },

                                           }).ToListAsync();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaResumen: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;

            }
            return marcasResumen;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-11-29
        //Obtener lista de Marcas Resumen
        public async Task<List<cMarcaResumen>> GetMarcasResumenXPeriodo(string idPeriodo)
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = await (from mr in _context.Marcas_Resumen.Where(e => e.IdPeriodo == idPeriodo)
                                       join emp in _context.Empleados.Where(e => e.Estado == 'T') on mr.IdNumero equals emp.IdNumero
                                       select mr).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcasResumen;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas
        public async Task<List<cMarca>> GetMarcas()
        {
            List<cMarca> marcas = new();
            try
            {
                marcas = await _context.Marcas.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcas: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcas;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener las marcas de un empleado
        //Parametros: idnumero=numero de empleado a buscar
        public async Task<List<cMarca>> GetMarcas(string idnumero)
        {
            List<cMarca>? marca = new();
            try
            {
                marca = await _context.Marcas.Where(e => e.idnumero == idnumero).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcas: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetMarcas: Obtener las marcas de un empleado para el periodo activo
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarca>> GetMarcas(string idnumero, string fecha)
        {
            List<cMarca>? marca = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                if (periodoVigente is not null)
                {
                    marca = await _context.Marcas.Where(e => e.idnumero == idnumero 
                                                          && (DateTime)e.fecha_hora >= periodoVigente.inicio 
                                                          && (DateTime)e.fecha_hora <= periodoVigente.fin).ToListAsync();
                }
                                
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcas: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }

        /// <summary>
        /// GetMarcasDiaria: Obtener las marcas del dia para un empleado 
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha del dia</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarca>> GetMarcasDiaria(string idnumero, string fecha)
        {
            List<cMarca>? marca = new();
            try
            {
                DateTime fechaDia = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                marca = await _context.Marcas.Where(e => e.idnumero == idnumero
                                                 && e.fecha == fechaDia).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasDiaria: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Marcas
        //Parametro: Recibe una lista de marcas, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Marca(IEnumerable<cMarca> marcas)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var marca in marcas)
                {
                    _context.Add(marca);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-30
        //Obtener una marca en concreto
        public async Task<cMarca> GetMarcaProgramador(string idnumero, string idplanilla, string fecha)
        {
            cMarca? marcas = new();
            try
            {
                DateTime fechaParse = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                marcas = await _context.Marcas.FirstOrDefaultAsync(e => e.idnumero == idnumero
                                                       && e.idplanilla == idplanilla
                                                       && e.fecha == fechaParse && e.tipo == 1);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaProgramador: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcas;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Validar Clave de Usuario
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se valida contraseña indicada contra la registrada en la base de datos
        public async Task<EventResponse> ValidarClaveEmpleado(cLogin login)
        {
            EventResponse respuesta = new EventResponse();
            //funciones_geo funcionesGeo = new();

            try
            {
                var emp = await _context.Empleados.FirstAsync(e => e.IdNumero == login.Usuario);

                if (emp is not null)
                {
                    var pass = FuncionesGlobales.Global_encrypt(login.Password);


                    if (emp.global_clave != pass)
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "La contraseña indicada no es válida.";
                    }

                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos del empleado.";

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Validar Clave de Usuario Admin
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se valida contraseña indicada contra la registrada en la base de datos
        public async Task<EventResponse> ValidarClaveAdm(cLogin login)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var user = await _context.PH_LOGIN.FirstAsync(e => e.usuario.ToLower() == login.Usuario!.ToLower());

                if (user is not null)
                {
                    var pass = FuncionesGlobales.Global_encrypt(login.Password!);

                    if (user.GLOBAL_CLAVE != pass)
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "La contraseña indicada no es válida.";
                    }

                    int sesion= Random.Shared.Next();
                    int conteo_intentos = 0;
                    int c_sesiones = 1;
                    do
                    {
                        if (await _context.PH_LOGIN.AnyAsync(e => e.idsesion == sesion && e.idusuario != user.idusuario))
                            sesion = Random.Shared.Next();
                        else
                            break;
                        conteo_intentos++;                
                        if (conteo_intentos > 5) c_sesiones = 0;
                    } while (c_sesiones > 0);
                    if (conteo_intentos > 5)
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "No se pudo generar crear la sesión, por favor intente nuevamente.";
                        return respuesta;
                    }

                    user.idsesion = sesion;
                    user.ultimo_login = DateTime.Now;
                    user.proceso = 0;
                    _context.PH_LOGIN.Update(user);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos del usuario.";

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del usuario. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// ValidarLicencia: validar datos de la licencia
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<EventResponse> ValidarLicencia(cLogin login)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var usuario = await _context.PH_LOGIN.FirstAsync(e => e.usuario.ToLower() == login.Usuario!.ToLower());
                bool verifica_cant = true;
                DateTime fecha_control;
                cLicenciaGeo licencia = new cLicenciaGeo();
                int usuarios_lic = 1;
                int cat_omite_lic = 0;
                cat_omite_lic = await _context.PH_LOGIN.CountAsync(e => e.OMITE_LIC.Equals('T'));

                if (usuario is not null)
                {
                    if (usuario.OMITE_LIC.Equals('T') && cat_omite_lic == 1) verifica_cant = false;
                }
                
                var phSistema = await _context.Ph_Sistema.FirstOrDefaultAsync();

                if (phSistema is null)
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos de la licencia, por favor contacte al administrador del sistema.";
                    return respuesta;
                }

                try
                {
                    fecha_control = phSistema.F_CONTROL==null?DateTime.ParseExact(GeotimeFuncionesLib.Utiles.FuncionesGlobales.Decrypt(GeotimeFuncionesLib.Utiles.FuncionesGlobales.HexadecimalToString(phSistema.F_CONTROL!.ToString())), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) : DateTime.Now.AddDays(100);
                }
                catch (Exception q)
                {
                    fecha_control = DateTime.Now.AddDays(100);
                }
                try
                {
                    licencia.nom_comp = GeotimeFuncionesLib.Utiles.FuncionesGlobales.Decrypt(GeotimeFuncionesLib.Utiles.FuncionesGlobales.HexadecimalToString(phSistema.DATA_01));
                }
                catch (Exception q)
                {
                    licencia.nom_comp = "";
                }
                try
                {
                    licencia.ven_lic = GeotimeFuncionesLib.Utiles.FuncionesGlobales.Decrypt(GeotimeFuncionesLib.Utiles.FuncionesGlobales.HexadecimalToString(phSistema.DATA_02));
                }
                catch (Exception q)
                {
                    licencia.ven_lic = "";
                }
                
                
                licencia.dist_lic = "F";
                licencia.dist_lic_emp = "F";
                bool distribuye_lic_ususario = false;


                //validar si distribuye licencias                    
                if (licencia.dist_lic.Equals('T'))
                {
                    usuarios_lic = Convert.ToInt32(GeotimeFuncionesLib.Utiles.FuncionesGlobales.Decrypt(GeotimeFuncionesLib.Utiles.FuncionesGlobales.HexadecimalToString(phSistema.DATA_01))) + 1;
                }
                //else
                //{
                //    Session["dist_lic"] = "T";
                //    distribuye_lic_ususario = true;
                //    OleDbCommand nq = new OleDbCommand("select * from " + Funciones.funciones_geo.usuario_global + ".ph_companias where idcomp = '" + Session["comp_sel"].ToString() + "'", con);
                //    DataTable c_comp = new DataTable();
                //    OleDbDataAdapter adap_ccomp = new OleDbDataAdapter(nq);
                //    adap_ccomp.Fill(c_comp);
                //    OleDbConnection nc = new OleDbConnection((c_comp.Rows[0]["string_sql"] != DBNull.Value) ? seguridad.decripto(c_comp.Rows[0]["string_sql"].ToString()) : GeoTime.Properties.Settings.Default.conexion);
                //    nc.Open();
                //    OleDbCommand lic_q = new OleDbCommand("select * from " + Session["comp_sel"].ToString() + ".ph_opciones", nc);
                //    DataTable c_opc = new DataTable();
                //    OleDbDataAdapter c_opc_adap = new OleDbDataAdapter(lic_q);
                //    c_opc_adap.Fill(c_opc);
                //    usuarios_lic = Convert.ToInt32(seg.Decrypt(hex.HexadecimalToString(c_opc.Rows[0]["dist_lic_usr"].ToString()))) + 1;
                //    nc.Close();
                //}








            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del usuario. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Validar Clave de Usuario
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se valida contraseña indicada contra la registrada en la base de datos
        public async Task<EventResponse> CambiarClaveEmpleado(cEmpleado empleado)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var emp = await _context.Empleados.FirstAsync(e => e.IdNumero == empleado.IdNumero);

                if (emp is not null)
                {
                    var pass = FuncionesGlobales.Global_encrypt(empleado.global_clave!);

                    emp.global_clave = pass;

                    _context.Empleados.Update(emp);
                    await _context.SaveChangesAsync();

                    if (emp.Email is not null)
                    {
                        var phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e=>(e.EMAIL==null?"": e.EMAIL).ToLower()== emp.Email.ToLower());

                        if (phlogin is not null)
                        {
                            phlogin.GLOBAL_CLAVE = pass;
                            _context.PH_LOGIN.Update(phlogin);
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos del empleado.";

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-11-01
        //CambiarCodigoSeguridadEmpleado
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se actualiza codigo de seguridad
        public async Task<EventResponse> CambiarCodigoSeguridadEmpleado(cEmpleado empleado)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var emp = await _context.Empleados.FirstAsync(e => e.IdNumero == empleado.IdNumero);

                if (emp is not null)
                {
                    emp.global_code = empleado.global_code;
                    emp.fecha_act_code = DateTime.Now;

                    _context.Empleados.Update(emp);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos del empleado.";

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Obtener lista de Marcas_Mov_Turnos
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurno()
        {
            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                marcaMovTurno = await (from e in _context.Marcas_Mov_Turnos
                                       .Include(e => e.cEmpleado)
                                       .Include(e => e.cTurno)
                                       .Include(e => e.cPh_Planilla)
                                       select new cMarcaMovTurno
                                       {
                                           idregistro = e.idregistro,
                                           idplanilla = e.idplanilla,
                                           idnumero = e.idnumero,
                                           fecha = e.fecha,
                                           hora = e.hora,
                                           turno = e.turno,
                                           estado = e.estado,
                                           usuario = e.usuario,
                                           fecha_reg = e.fecha_reg,
                                           linea = e.linea,
                                           cEmpleado = e.cEmpleado==null?null:
                                                new cEmpleado
                                                {
                                                    IdNumero = e.cEmpleado.IdNumero,
                                                    IdPlanilla = e.cEmpleado.IdPlanilla,
                                                    Nombre = e.cEmpleado.Nombre,
                                                    Tarjeta = e.cEmpleado.Tarjeta,
                                                    Identificacion = e.cEmpleado.Identificacion,
                                                    IdGrupo = e.cEmpleado.IdGrupo,
                                                    IdDepartamento = e.cEmpleado.IdDepartamento,
                                                    IdHorario = e.cEmpleado.IdHorario,
                                                    Estado = e.cEmpleado.Estado,
                                                    IdAgrupamiento = e.cEmpleado.IdAgrupamiento,
                                                    foto = e.cEmpleado.foto,
                                                    IdCCosto = e.cEmpleado.IdCCosto,
                                                    exporta = e.cEmpleado.exporta,
                                                    ubicacion = e.cEmpleado.ubicacion,
                                                    rubro1 = e.cEmpleado.rubro1,
                                                    rubro2 = e.cEmpleado.rubro2,
                                                    rubro3 = e.cEmpleado.rubro3,
                                                    rubro4 = e.cEmpleado.rubro4,
                                                    rubro5 = e.cEmpleado.rubro5,
                                                    rubro6 = e.cEmpleado.rubro6,
                                                    rubro7 = e.cEmpleado.rubro7,
                                                    rubro8 = e.cEmpleado.rubro8,
                                                    rubro9 = e.cEmpleado.rubro9,
                                                    rubro10 = e.cEmpleado.rubro10,
                                                    rubro11 = e.cEmpleado.rubro11,
                                                    rubro12 = e.cEmpleado.rubro12,
                                                    rubro13 = e.cEmpleado.rubro13,
                                                    rubro14 = e.cEmpleado.rubro14,
                                                    rubro15 = e.cEmpleado.rubro15,
                                                    rubro16 = e.cEmpleado.rubro16,
                                                    rubro17 = e.cEmpleado.rubro17,
                                                    rubro18 = e.cEmpleado.rubro18,
                                                    rubro19 = e.cEmpleado.rubro19,
                                                    rubro20 = e.cEmpleado.rubro20,
                                                    rubro21 = e.cEmpleado.rubro21,
                                                    rubro22 = e.cEmpleado.rubro22,
                                                    rubro23 = e.cEmpleado.rubro23,
                                                    rubro24 = e.cEmpleado.rubro24,
                                                    rubro25 = e.cEmpleado.rubro25,
                                                    Fecha_Ingreso = e.cEmpleado.Fecha_Ingreso,
                                                    Email = e.cEmpleado.Email,
                                                    Tipo_Marca = e.cEmpleado.Tipo_Marca,
                                                    inicio_rol = e.cEmpleado.inicio_rol,
                                                    web_pass = e.cEmpleado.web_pass,
                                                    id_transfo_conc = e.cEmpleado.id_transfo_conc,
                                                    widioma = e.cEmpleado.widioma,
                                                    global_clave = e.cEmpleado.global_clave,
                                                    def_fase = e.cEmpleado.def_fase,
                                                    def_py = e.cEmpleado.def_py,
                                                    def_cc = e.cEmpleado.def_cc,
                                                    Fecha_Salida = e.cEmpleado.Fecha_Salida,
                                                    global_code = e.cEmpleado.global_code,
                                                    fecha_act_code = e.cEmpleado.fecha_act_code,
                                                },
                                            cPh_Planilla = e.cPh_Planilla == null ? null :
                                                    new cPh_Planilla
                                                    {
                                                        idplanilla = e.cPh_Planilla.idplanilla,
                                                        planilla = e.cPh_Planilla.planilla,
                                                        nom_conector = e.cPh_Planilla.nom_conector,
                                                        tipo_planilla = e.cPh_Planilla.tipo_planilla,
                                                        c_ext = e.cPh_Planilla.c_ext,
                                                        c_inci = e.cPh_Planilla.c_inci,
                                                        c_adic = e.cPh_Planilla.c_adic,
                                                        m_desc = e.cPh_Planilla.m_desc,
                                                        proyecta = e.cPh_Planilla.proyecta,
                                                        dia_inicio = e.cPh_Planilla.dia_inicio,
                                                        auto_proceso = e.cPh_Planilla.auto_proceso,
                                                        tipo_dist = e.cPh_Planilla.tipo_dist,
                                                        est_nomina = e.cPh_Planilla.est_nomina,
                                                        ext_per_ant = e.cPh_Planilla.ext_per_ant,
                                                        ext_det = e.cPh_Planilla.ext_det,
                                                        agrup_salida = e.cPh_Planilla.agrup_salida,
                                                        tipo_adic = e.cPh_Planilla.tipo_adic,
                                                        nivel_aprob_ext = e.cPh_Planilla.nivel_aprob_ext,
                                                    },
                                            cTurno = e.cTurno == null ? null :
                                                new cTurno
                                                {
                                                    IdTurno = e.cTurno.IdTurno,
                                                    Descripcion = e.cTurno.Descripcion,
                                                    HEntra = e.cTurno.HEntra,
                                                    HSale = e.cTurno.HSale,
                                                    tar_apl = e.cTurno.tar_apl,
                                                    ant_apl = e.cTurno.ant_apl,
                                                    des_1_in = e.cTurno.des_1_in,
                                                    des_1_out = e.cTurno.des_1_out,
                                                    des_2_in = e.cTurno.des_2_in,
                                                    des_2_out = e.cTurno.des_2_out,
                                                    des_3_in = e.cTurno.des_3_in,
                                                    des_3_out = e.cTurno.des_3_out,
                                                    apl_des_1 = e.cTurno.apl_des_1,
                                                    apl_des_2 = e.cTurno.apl_des_2,
                                                    apl_des_3 = e.cTurno.apl_des_3,
                                                    des_1_tiem = e.cTurno.des_1_tiem,
                                                    des_2_tiem = e.cTurno.des_2_tiem,
                                                    des_3_tiem = e.cTurno.des_3_tiem,
                                                    marca_des_1 = e.cTurno.marca_des_1,
                                                    marca_des_2 = e.cTurno.marca_des_2,
                                                    marca_des_3 = e.cTurno.marca_des_3,
                                                    tar_tiem = e.cTurno.tar_tiem,
                                                    ant_tiem = e.cTurno.ant_tiem,
                                                    con_1 = e.cTurno.con_1,
                                                    con_2 = e.cTurno.con_2,
                                                    con_3 = e.cTurno.con_3,
                                                    con_4 = e.cTurno.con_4,
                                                    con_5 = e.cTurno.con_5,
                                                    con_6 = e.cTurno.con_6,
                                                    cant_con_1 = e.cTurno.cant_con_1,
                                                    cant_con_2 = e.cTurno.cant_con_2,
                                                    cant_con_3 = e.cTurno.cant_con_3,
                                                    cant_con_4 = e.cTurno.cant_con_4,
                                                    cant_con_5 = e.cTurno.cant_con_5,
                                                    cant_con_6 = e.cTurno.cant_con_6,
                                                    min_con_1 = e.cTurno.min_con_1,
                                                    min_con_2 = e.cTurno.min_con_2,
                                                    min_con_3 = e.cTurno.min_con_3,
                                                    min_con_4 = e.cTurno.min_con_4,
                                                    min_con_5 = e.cTurno.min_con_5,
                                                    min_con_6 = e.cTurno.min_con_6,
                                                    Tipo = e.cTurno.Tipo,
                                                    Tipo_Jor = e.cTurno.Tipo_Jor,
                                                    fuerza_calc = e.cTurno.fuerza_calc,
                                                    idagrupamiento = e.cTurno.idagrupamiento,
                                                    apl_trans1 = e.cTurno.apl_trans1,
                                                    id_trans1 = e.cTurno.id_trans1,
                                                    apl_trans2 = e.cTurno.apl_trans2,
                                                    id_trans2 = e.cTurno.id_trans2,
                                                    apl_trans3 = e.cTurno.apl_trans3,
                                                    id_trans3 = e.cTurno.id_trans3,
                                                    apl_trans4 = e.cTurno.apl_trans4,
                                                    id_trans4 = e.cTurno.id_trans4,
                                                    apl_trans5 = e.cTurno.apl_trans5,
                                                    id_trans5 = e.cTurno.id_trans5,
                                                    apl_trans6 = e.cTurno.apl_trans6,
                                                    id_trans6 = e.cTurno.id_trans6,
                                                    apl_ben1 = e.cTurno.apl_ben1,
                                                    id_ben1 = e.cTurno.id_ben1,
                                                    apl_ben2 = e.cTurno.apl_ben2,
                                                    id_ben2 = e.cTurno.id_ben2,
                                                    apl_ben3 = e.cTurno.apl_ben3,
                                                    id_ben3 = e.cTurno.id_ben3,
                                                    apl_ben4 = e.cTurno.apl_ben4,
                                                    id_ben4 = e.cTurno.id_ben4,
                                                    apl_ben5 = e.cTurno.apl_ben5,
                                                    id_ben5 = e.cTurno.id_ben5,
                                                    apl_ben6 = e.cTurno.apl_ben6,
                                                    id_ben6 = e.cTurno.id_ben6,
                                                    conc_ben1 = e.cTurno.conc_ben1,
                                                    conc_ben2 = e.cTurno.conc_ben2,
                                                    conc_ben3 = e.cTurno.conc_ben3,
                                                    conc_ben4 = e.cTurno.conc_ben4,
                                                    conc_ben5 = e.cTurno.conc_ben5,
                                                    conc_ben6 = e.cTurno.conc_ben6,
                                                    apl_trans_post = e.cTurno.apl_trans_post,
                                                    id_trans_post = e.cTurno.id_trans_post,
                                                    apl_redond_entrada = e.cTurno.apl_redond_entrada,
                                                    cant_redond_entrada = e.cTurno.cant_redond_entrada,
                                                    auto_pan = e.cTurno.auto_pan,
                                                    ColorId = e.cTurno.ColorId,
                                                },
                                       }
                                 ).ToListAsync();


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Obtener un registro de Marcas_Mov_Turnos especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaMovTurno> GetMarcaMovTurno(int idregistro)
        {
            cMarcaMovTurno? marcaMovTurno = new();
            try
            {
                marcaMovTurno = await _context.Marcas_Mov_Turnos.FirstOrDefaultAsync(e => e.idregistro == idregistro);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Obtener un registro de Marcas_Mov_Turnos especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaMovTurno> GetMarcaMovTurno(string idnumero, string fecha, int idturno)
        {
            cMarcaMovTurno? marcaMovTurno = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                marcaMovTurno = await _context.Marcas_Mov_Turnos.FirstOrDefaultAsync(e => e.idnumero == idnumero && e.fecha == fechaMov && e.turno == idturno);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Allan Prieto
        //Fecha: 2024-05-31
        //Obtener un registro de Marcas_Mov_Turnos especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurno(string idplanilla, string estado, string fechaInicio, string fechaFinal)
        {
            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                marcaMovTurno = await (from e in _context.Marcas_Mov_Turnos
                                       .Include(e => e.cEmpleado)
                                       .Include(e => e.cTurno)
                                       .Include(e => e.cPh_Planilla)
                                       .Where(e => e.idplanilla == (idplanilla=="-1"? e.idplanilla : idplanilla) &&
                                              e.estado.ToString() == estado &&
                                              e.fecha >= fechaMovInicio &&
                                              e.fecha <= fechaMovFinal)
                                       select new cMarcaMovTurno
                                       {
                                           idregistro = e.idregistro,
                                           idplanilla = e.idplanilla,
                                           idnumero = e.idnumero,
                                           fecha = e.fecha,
                                           hora = e.hora,
                                           turno = e.turno,
                                           estado = e.estado,
                                           usuario = e.usuario,
                                           fecha_reg = e.fecha_reg,
                                           linea = e.linea,
                                           cEmpleado = e.cEmpleado == null ? null :
                                                new cEmpleado
                                                {
                                                    IdNumero = e.cEmpleado.IdNumero,
                                                    IdPlanilla = e.cEmpleado.IdPlanilla,
                                                    Nombre = e.cEmpleado.Nombre,
                                                    Tarjeta = e.cEmpleado.Tarjeta,
                                                    Identificacion = e.cEmpleado.Identificacion,
                                                    IdGrupo = e.cEmpleado.IdGrupo,
                                                    IdDepartamento = e.cEmpleado.IdDepartamento,
                                                    IdHorario = e.cEmpleado.IdHorario,
                                                    Estado = e.cEmpleado.Estado,
                                                    IdAgrupamiento = e.cEmpleado.IdAgrupamiento,
                                                    foto = e.cEmpleado.foto,
                                                    IdCCosto = e.cEmpleado.IdCCosto,
                                                    exporta = e.cEmpleado.exporta,
                                                    ubicacion = e.cEmpleado.ubicacion,
                                                    rubro1 = e.cEmpleado.rubro1,
                                                    rubro2 = e.cEmpleado.rubro2,
                                                    rubro3 = e.cEmpleado.rubro3,
                                                    rubro4 = e.cEmpleado.rubro4,
                                                    rubro5 = e.cEmpleado.rubro5,
                                                    rubro6 = e.cEmpleado.rubro6,
                                                    rubro7 = e.cEmpleado.rubro7,
                                                    rubro8 = e.cEmpleado.rubro8,
                                                    rubro9 = e.cEmpleado.rubro9,
                                                    rubro10 = e.cEmpleado.rubro10,
                                                    rubro11 = e.cEmpleado.rubro11,
                                                    rubro12 = e.cEmpleado.rubro12,
                                                    rubro13 = e.cEmpleado.rubro13,
                                                    rubro14 = e.cEmpleado.rubro14,
                                                    rubro15 = e.cEmpleado.rubro15,
                                                    rubro16 = e.cEmpleado.rubro16,
                                                    rubro17 = e.cEmpleado.rubro17,
                                                    rubro18 = e.cEmpleado.rubro18,
                                                    rubro19 = e.cEmpleado.rubro19,
                                                    rubro20 = e.cEmpleado.rubro20,
                                                    rubro21 = e.cEmpleado.rubro21,
                                                    rubro22 = e.cEmpleado.rubro22,
                                                    rubro23 = e.cEmpleado.rubro23,
                                                    rubro24 = e.cEmpleado.rubro24,
                                                    rubro25 = e.cEmpleado.rubro25,
                                                    Fecha_Ingreso = e.cEmpleado.Fecha_Ingreso,
                                                    Email = e.cEmpleado.Email,
                                                    Tipo_Marca = e.cEmpleado.Tipo_Marca,
                                                    inicio_rol = e.cEmpleado.inicio_rol,
                                                    web_pass = e.cEmpleado.web_pass,
                                                    id_transfo_conc = e.cEmpleado.id_transfo_conc,
                                                    widioma = e.cEmpleado.widioma,
                                                    global_clave = e.cEmpleado.global_clave,
                                                    def_fase = e.cEmpleado.def_fase,
                                                    def_py = e.cEmpleado.def_py,
                                                    def_cc = e.cEmpleado.def_cc,
                                                    Fecha_Salida = e.cEmpleado.Fecha_Salida,
                                                    global_code = e.cEmpleado.global_code,
                                                    fecha_act_code = e.cEmpleado.fecha_act_code,
                                                },
                                           cPh_Planilla = e.cPh_Planilla == null ? null :
                                                    new cPh_Planilla
                                                    {
                                                        idplanilla = e.cPh_Planilla.idplanilla,
                                                        planilla = e.cPh_Planilla.planilla,
                                                        nom_conector = e.cPh_Planilla.nom_conector,
                                                        tipo_planilla = e.cPh_Planilla.tipo_planilla,
                                                        c_ext = e.cPh_Planilla.c_ext,
                                                        c_inci = e.cPh_Planilla.c_inci,
                                                        c_adic = e.cPh_Planilla.c_adic,
                                                        m_desc = e.cPh_Planilla.m_desc,
                                                        proyecta = e.cPh_Planilla.proyecta,
                                                        dia_inicio = e.cPh_Planilla.dia_inicio,
                                                        auto_proceso = e.cPh_Planilla.auto_proceso,
                                                        tipo_dist = e.cPh_Planilla.tipo_dist,
                                                        est_nomina = e.cPh_Planilla.est_nomina,
                                                        ext_per_ant = e.cPh_Planilla.ext_per_ant,
                                                        ext_det = e.cPh_Planilla.ext_det,
                                                        agrup_salida = e.cPh_Planilla.agrup_salida,
                                                        tipo_adic = e.cPh_Planilla.tipo_adic,
                                                        nivel_aprob_ext = e.cPh_Planilla.nivel_aprob_ext,
                                                    },
                                           cTurno = e.cTurno == null ? null :
                                                new cTurno
                                                {
                                                    IdTurno = e.cTurno.IdTurno,
                                                    Descripcion = e.cTurno.Descripcion,
                                                    HEntra = e.cTurno.HEntra,
                                                    HSale = e.cTurno.HSale,
                                                    tar_apl = e.cTurno.tar_apl,
                                                    ant_apl = e.cTurno.ant_apl,
                                                    des_1_in = e.cTurno.des_1_in,
                                                    des_1_out = e.cTurno.des_1_out,
                                                    des_2_in = e.cTurno.des_2_in,
                                                    des_2_out = e.cTurno.des_2_out,
                                                    des_3_in = e.cTurno.des_3_in,
                                                    des_3_out = e.cTurno.des_3_out,
                                                    apl_des_1 = e.cTurno.apl_des_1,
                                                    apl_des_2 = e.cTurno.apl_des_2,
                                                    apl_des_3 = e.cTurno.apl_des_3,
                                                    des_1_tiem = e.cTurno.des_1_tiem,
                                                    des_2_tiem = e.cTurno.des_2_tiem,
                                                    des_3_tiem = e.cTurno.des_3_tiem,
                                                    marca_des_1 = e.cTurno.marca_des_1,
                                                    marca_des_2 = e.cTurno.marca_des_2,
                                                    marca_des_3 = e.cTurno.marca_des_3,
                                                    tar_tiem = e.cTurno.tar_tiem,
                                                    ant_tiem = e.cTurno.ant_tiem,
                                                    con_1 = e.cTurno.con_1,
                                                    con_2 = e.cTurno.con_2,
                                                    con_3 = e.cTurno.con_3,
                                                    con_4 = e.cTurno.con_4,
                                                    con_5 = e.cTurno.con_5,
                                                    con_6 = e.cTurno.con_6,
                                                    cant_con_1 = e.cTurno.cant_con_1,
                                                    cant_con_2 = e.cTurno.cant_con_2,
                                                    cant_con_3 = e.cTurno.cant_con_3,
                                                    cant_con_4 = e.cTurno.cant_con_4,
                                                    cant_con_5 = e.cTurno.cant_con_5,
                                                    cant_con_6 = e.cTurno.cant_con_6,
                                                    min_con_1 = e.cTurno.min_con_1,
                                                    min_con_2 = e.cTurno.min_con_2,
                                                    min_con_3 = e.cTurno.min_con_3,
                                                    min_con_4 = e.cTurno.min_con_4,
                                                    min_con_5 = e.cTurno.min_con_5,
                                                    min_con_6 = e.cTurno.min_con_6,
                                                    Tipo = e.cTurno.Tipo,
                                                    Tipo_Jor = e.cTurno.Tipo_Jor,
                                                    fuerza_calc = e.cTurno.fuerza_calc,
                                                    idagrupamiento = e.cTurno.idagrupamiento,
                                                    apl_trans1 = e.cTurno.apl_trans1,
                                                    id_trans1 = e.cTurno.id_trans1,
                                                    apl_trans2 = e.cTurno.apl_trans2,
                                                    id_trans2 = e.cTurno.id_trans2,
                                                    apl_trans3 = e.cTurno.apl_trans3,
                                                    id_trans3 = e.cTurno.id_trans3,
                                                    apl_trans4 = e.cTurno.apl_trans4,
                                                    id_trans4 = e.cTurno.id_trans4,
                                                    apl_trans5 = e.cTurno.apl_trans5,
                                                    id_trans5 = e.cTurno.id_trans5,
                                                    apl_trans6 = e.cTurno.apl_trans6,
                                                    id_trans6 = e.cTurno.id_trans6,
                                                    apl_ben1 = e.cTurno.apl_ben1,
                                                    id_ben1 = e.cTurno.id_ben1,
                                                    apl_ben2 = e.cTurno.apl_ben2,
                                                    id_ben2 = e.cTurno.id_ben2,
                                                    apl_ben3 = e.cTurno.apl_ben3,
                                                    id_ben3 = e.cTurno.id_ben3,
                                                    apl_ben4 = e.cTurno.apl_ben4,
                                                    id_ben4 = e.cTurno.id_ben4,
                                                    apl_ben5 = e.cTurno.apl_ben5,
                                                    id_ben5 = e.cTurno.id_ben5,
                                                    apl_ben6 = e.cTurno.apl_ben6,
                                                    id_ben6 = e.cTurno.id_ben6,
                                                    conc_ben1 = e.cTurno.conc_ben1,
                                                    conc_ben2 = e.cTurno.conc_ben2,
                                                    conc_ben3 = e.cTurno.conc_ben3,
                                                    conc_ben4 = e.cTurno.conc_ben4,
                                                    conc_ben5 = e.cTurno.conc_ben5,
                                                    conc_ben6 = e.cTurno.conc_ben6,
                                                    apl_trans_post = e.cTurno.apl_trans_post,
                                                    id_trans_post = e.cTurno.id_trans_post,
                                                    apl_redond_entrada = e.cTurno.apl_redond_entrada,
                                                    cant_redond_entrada = e.cTurno.cant_redond_entrada,
                                                    auto_pan = e.cTurno.auto_pan,
                                                    ColorId = e.cTurno.ColorId,
                                                },
                                       }
                                 ).ToListAsync();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }

        /// <summary>
        /// GetMarcaMovTurno: Método para obtener una lista de Marcas Mov Turnos por empleado 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="idnumero">Número de Empleado</param>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurno(string idnumero, string fechaPeriodo)
        {

            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                var periodos = await (from a in _context.Ph_Periodos
                                      join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla
                                      join c in _context.Empleados on b.idplanilla equals c.IdPlanilla
                                      where c.IdNumero == idnumero
                                        && fechaMov >= a.inicio && fechaMov <= a.fin
                                      select a).FirstOrDefaultAsync();
                if (periodos != null)
                {
                    marcaMovTurno = await _context.Marcas_Mov_Turnos
                                            .Where(e => e.idnumero == idnumero
                                                    && e.fecha >= periodos.inicio
                                                    && e.fecha <= periodos.fin).ToListAsync();
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }

        /// <summary>
        /// GetMarcaMovTurno: Método para obtener una lista de Marcas Mov Turnos de los empleados asignados a un supervisor 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        /// <param name="idgrupo">grupo de empleado</param>
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurnoByGrupo(string fechaPeriodo, string idgrupo)
        {

            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                idgrupo = HttpUtility.UrlDecode(idgrupo);
                var grupos = idgrupo.Split(",");

                List<cPh_Grupo> phgrupos = new List<cPh_Grupo>();
                foreach (var valor in grupos)
                    phgrupos.Add(new cPh_Grupo
                    {
                        idgrupo = int.Parse(valor),
                    });

                var periodos = await _context.Ph_Periodos.ToListAsync();
                var planillas = await _context.Ph_Planilla.ToListAsync();
                var empleados = await _context.Empleados.ToListAsync();
                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                
                periodos = (from a in periodos
                                      join b in planillas on a.tipo_planilla equals b.tipo_planilla
                                      join c in empleados on b.idplanilla equals c.IdPlanilla
                                      join g in phgrupos on c.IdGrupo equals g.idgrupo
                                      where fechaMov >= a.inicio && fechaMov <= a.fin
                                      select a).Distinct().ToList();

                foreach (var periodo in periodos)
                {
                    var marcasMovTurno = await _context.Marcas_Mov_Turnos
                                                .Where(m => m.fecha >= periodo.inicio
                                                 && m.fecha <= periodo.fin).ToListAsync();

                    var marcasperiodo = (from m in marcasMovTurno
                                               join c in empleados on new { idnumero = m.idnumero, idplanilla = m.idplanilla } equals new { idnumero = c.IdNumero, idplanilla = c.IdPlanilla }
                                               join g in phgrupos on c.IdGrupo equals g.idgrupo
                                               select new cMarcaMovTurno
                                               {
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
                                               }).ToList();


                    marcaMovTurno.AddRange(marcasperiodo);
                }


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurnoByGrupo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Sincronizar Marcas_MOv_Turnos
        //Parametro: Recibe una instancia de MarcaMovTurno, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcasMovTurnos(IEnumerable<cMarcaMovTurno> marcasMovTurnos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in marcasMovTurnos)
                {
                    cMarcaMovTurno? marcaMovTurno = await _context.Marcas_Mov_Turnos
                                    .Where(e => e.idnumero == item.idnumero && e.fecha == item.fecha && e.linea==item.linea)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (marcaMovTurno is not null)
                    {
                        marcaMovTurno.turno = item.turno;
                        marcaMovTurno.idplanilla = item.idplanilla;
                        marcaMovTurno.hora = "00:00";
                        _context.Marcas_Mov_Turnos.Update(marcaMovTurno);
                    }
                    else
                    {
                        item.hora = "00:00";
                        item.idregistro = 0;
                        _context.Add(item);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// PutMarcasMovTurnos: actualizar Marcas_MOv_Turnos, Recibe una instancia de MarcaMovTurno, se verifica si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="marcaMovTurno"></param>
        /// <returns>Instancia de eventresponse con el resultado del proceso</returns>
        public async Task<EventResponse> PutMarcasMovTurnos(cMarcaMovTurno marcasMovTurno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cMarcaMovTurno? model = await _context.Marcas_Mov_Turnos
                                 .Where(e => e.idnumero == marcasMovTurno.idnumero && e.fecha == marcasMovTurno.fecha)
                                 .FirstOrDefaultAsync();
                //si la marca existe se actualiza 
                if (model is not null)
                {
                    model.turno = marcasMovTurno.turno;
                    model.idplanilla = marcasMovTurno.idplanilla;
                    model.hora = "00:00";
                    _context.Marcas_Mov_Turnos.Update(model);
                }

                var turno = await _context.Ph_Turnos.FirstOrDefaultAsync(e => e.IdTurno == marcasMovTurno.turno);
                var fechaSalida = turno.HEntra.CompareTo(turno.HSale) > 0 ? marcasMovTurno.fecha.AddDays(1) : marcasMovTurno.fecha;

                cMarcaProceso? marcaProceso = await _context.Marcas_Proceso
                                .Where(e => e.idnumero == marcasMovTurno.idnumero 
                                         && e.fecha_entra == marcasMovTurno.fecha)
                                .FirstOrDefaultAsync();
                //si la marcaProceso existe se actualiza 
                if (marcaProceso is not null)
                {
                    marcaProceso.idturno = marcasMovTurno.turno;
                    marcaProceso.fecha_sale = fechaSalida;
                    _context.Marcas_Proceso.Update(marcaProceso);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de Marcas_Mov_Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de Marcas_Mov_Turno. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-04-02
        //Obtener lista de Marcas_Mov_hORARIOS
        public async Task<List<cMarcaMovHorario>> GetMarcaMovHorario()
        {
            List<cMarcaMovHorario> marcaMovHorario = new();
            try
            {
                marcaMovHorario = await _context.Marcas_Mov_Horarios.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovHorario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovHorario;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-04-02
        //Obtener un registro de Marcas_Mov_Horarios especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaMovHorario> GetMarcaMovHorario(int idregistro)
        {
            cMarcaMovHorario? marcaMovHorario = new();
            try
            {
                marcaMovHorario = await _context.Marcas_Mov_Horarios.FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovHorario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovHorario;
        }

        //Creado por: Allan Prieto
        //Fecha: 2024-07-16
        //Obtener un registro de cMarcaMovHorario especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<List<cMarcaMovHorario>> GetMarcaMovHorario(string idplanilla, string estado, string fechaInicio, string fechaFinal)
        {
            List<cMarcaMovHorario> marcaMovTurno = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                marcaMovTurno = await _context.Marcas_Mov_Horarios
                                  .Where(e => e.IDPLANILLA == idplanilla &&
                                              e.ESTADO.ToString() == estado &&
                                              e.FECHA >= fechaMovInicio &&
                                              e.FECHA <= fechaMovFinal).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaMovTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-04-02
        //Sincronizar Marcas_MOv_Turnos
        //Parametro: Recibe una instancia de MarcaMovHorario, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcasMovHorario(IEnumerable<cMarcaMovHorario> marcasMovHorarios)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in marcasMovHorarios)
                {
                    cMarcaMovHorario? marcaMovHorario = await _context.Marcas_Mov_Horarios
                                    .Where(e => e.IDNUMERO == item.IDNUMERO && e.FECHA == item.FECHA)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (marcaMovHorario is not null)
                    {
                        marcaMovHorario.IDHORARIO = item.IDHORARIO;
                        marcaMovHorario.IDPLANILLA = item.IDPLANILLA;
                        marcaMovHorario.HORA = "00:00";
                        _context.Marcas_Mov_Horarios.Update(marcaMovHorario);
                    }
                    else
                    {
                        item.HORA = "00:00";
                        item.IDREGISTRO = 0;
                        _context.Add(item);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Horario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Horario. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Sincronizar Marcas_Resumen
        //Parametro: Recibe una instancia de Marcas_Resumen, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcasResumen(IEnumerable<cMarcaResumen> marcasResumen)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in marcasResumen)
                {
                    var concepto = await _context.Ph_Conceptos.FirstOrDefaultAsync(e => e.id == item.IdConcepto);
                    var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == item.IdNumero);


                    cMarcaResumen? marcaRes = await _context.Marcas_Resumen
                                    .Where(e => e.IdPlanilla == empleado!.IdPlanilla && e.IdNumero == item.IdNumero
                                            && e.IdConcepto == item.IdConcepto && e.IdCCosto == empleado.IdCCosto)
                                    .FirstOrDefaultAsync();
                    if (marcaRes is not null)
                    {

                        marcaRes.NominaEq = item.NominaEq;
                        //marcaRes.Cantidad = item.Cantidad;
                        marcaRes.Monto = marcaRes.Monto + item.Monto;
                        marcaRes.Proyecto = item.Proyecto;
                        marcaRes.Fase = item.Fase;
                        marcaRes.IdPeriodo = item.IdPeriodo;
                        marcaRes.NominaEq = concepto!.nominaeq;

                        _context.Marcas_Resumen.Update(marcaRes);
                    }
                    else
                    {
                        item.cConcepto = null;
                        item.IdPlanilla = empleado.IdPlanilla;
                        item.IdCCosto = empleado.IdCCosto;
                        item.NominaEq = concepto!.nominaeq;
                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-07
        /// <summary>
        /// getPeriodo: Método para obtener una lista de Periodos 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        public async Task<IEnumerable<cPh_Periodos>> GetPeriodo()
        {
            List<cPh_Periodos>? periodos = new();
            try
            {
                periodos = await _context.Ph_Periodos.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return periodos;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-07
        /// <summary>
        /// getPeriodo: Método para un periodo específico
        /// </summary>
        /// <returns>Una instancia de la clase cPh_Periodos</returns>
        /// ///<param name="idperiodo">idperiodo del periodo requerido</param>
        public async Task<cPh_Periodos> GetPeriodo(string idperiodo)
        {
            cPh_Periodos? periodo = new();
            try
            {
                periodo = await _context.Ph_Periodos.FirstOrDefaultAsync(e => e.idperiodo == idperiodo);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return periodo;
        }

        //Creado por: Allan Prieto
        //Fecha: 2024-11-15
        /// <summary>
        /// getPeriodo: Método para obtener los periodos anteriores a la proyecion 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        /// <param name="idperiodo">Fecha del periodo</param>
        /// <param name="proyeccion">Fecha del periodo</param>
        /// <param name="vigente">Periodo está vigente </param>
        public async Task<IEnumerable<cPh_Periodos>> GetPeriodo(string idperiodo, string proyeccion, string vigente)
        {

            List<cPh_Periodos>? periodos = new();
            try
            {
                var periodo = await _context.Ph_Periodos.FindAsync(idperiodo);
                if (periodo == null || !periodo.inicio_proy.HasValue)
                {
                    // Manejar el caso en que el periodo no se encuentra o no tiene fecha de inicio_proyeccion
                    return new List<cPh_Periodos>();
                }
                DateTime fechaInicioProyeccion = periodo.inicio_proy.Value;
                periodos = await _context.Ph_Periodos
                    .Where(e => e.inicio_proy < fechaInicioProyeccion && e.estado == 'T')
                    .ToListAsync();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return periodos;
        }

        /// <summary>
        /// GetPeriodoVigenteUsuario: Método para obtener lista de periodos vigentes para un usuario  
        /// </summary>
        /// <returns>Una lista de cPh_Periodos vigentes</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="idusuario">id de usuario</param>
        public async Task<IEnumerable<cPh_Periodos>> GetPeriodoVigenteUsuario(int idusuario, string fechaPeriodo)
        {
            List<cPh_Periodos>? periodo = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                var phLogin = await _context.Ph_Usuarios.FirstOrDefaultAsync(e=>e.IDUSUARIO==idusuario);
                var planillas = phLogin.PLANILLAS.Split("°");

                periodo = await (from a in _context.Ph_Periodos
                                 join b in _context.Ph_Planilla.Where(e=> planillas.Contains(e.idplanilla)) 
                                    on a.tipo_planilla equals b.tipo_planilla    
                                 where fechaMov >= a.inicio && fechaMov <= a.fin
                                 select a).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPeriodoVigenteUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return periodo;


        }

        /// <summary>
        /// GetPeriodoVigenteEmpleado: Método para obtener el periodo vigenta para un empleado  
        /// </summary>
        /// <returns>Un item de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="idnumero">Número de empleado</param>
        public async Task<cPh_Periodos> GetPeriodoVigenteEmpleado(string idnumero, string fechaPeriodo)
        {
            cPh_Periodos? periodo = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                periodo = await (from a in _context.Ph_Periodos
                                 join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla
                                 join c in _context.Empleados on b.idplanilla equals c.IdPlanilla
                                 where c.IdNumero == idnumero
                                   && fechaMov >= a.inicio && fechaMov <= a.fin
                                 select a).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPeriodoVigenteEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return periodo;


        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-07
        /// <summary>
        /// getPeriodo: Método para obtener una lista de Periodos 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="vigente">Periodo está vigente </param>
        public async Task<IEnumerable<cPh_Periodos>> GetPeriodo(string fecha, string vigente)
        {

            List<cPh_Periodos>? periodos = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                periodos = await _context.Ph_Periodos.Where(e => fechaMov >= e.inicio && fechaMov <= e.fin)
                                                     .ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return periodos;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-1-15
        /// <summary>
        /// Sincronizar_Periodo: Método para registrar los registros en la tabla Ph_Periodos
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="ph_Periodo">Lista de registros de la clase cPh_Periodos</param>
        public async Task<EventResponse> Sincronizar_Periodo(IEnumerable<cPh_Periodos> ph_Periodo)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in ph_Periodo)
                {
                    cPh_Periodos? objetoBuscar = await _context.Ph_Periodos
                                    .Where(e => e.idperiodo == item.idperiodo)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.idperiodo = item.idperiodo;
                        objetoBuscar.tipo_planilla = item.tipo_planilla;
                        objetoBuscar.inicio = item.inicio;
                        objetoBuscar.fin = item.fin;
                        objetoBuscar.estado = item.estado;
                        objetoBuscar.inicio_proy = item.inicio_proy;
                        objetoBuscar.fin_proy = item.fin_proy;
                        objetoBuscar.periodo_proy = item.periodo_proy;
                        objetoBuscar.dia_inicio = item.dia_inicio;

                        _context.Ph_Periodos.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Ph_Periodos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Ph_Periodos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// Elimina_Periodo:  Metodo boorado de datos de la tabla cPh_Periodos
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Periodo(string id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cPh_Periodos? model = await _context.Ph_Periodos
                    .FirstOrDefaultAsync(e => e.idperiodo == id);

                if (model is not null)
                {
                    _context.Ph_Periodos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Periodo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar Periodo. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }



        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaIn: Método para obtener todos los datos de la tabla Marcas_In 
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaIn</returns>
        public async Task<List<cMarcaIn>> GetMarcaIn()
        {
            List<cMarcaIn> marcaIn = new();
            try
            {
                marcaIn = await _context.Marcas_In.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIn: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIn;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaIn: Método para obtener todos los datos de la tabla Marcas_In de un colaborador
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaIn para un colaborador en específico</returns>
        /// <param name="idtarjeta">Id de Tarjeta del colaborador</param>
        public async Task<List<cMarcaIn>> GetMarcaIn(string idtarjeta)
        {
            List<cMarcaIn> marcaIn = new();
            try
            {
                marcaIn = await _context.Marcas_In.Where(e => e.idtarjeta == idtarjeta).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIn: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIn;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// Sincronizar_MarcaIn: Método para registrar las marcas de ingreso, salida y descanso de los colaboradores en la tabla Marcas_In 
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasIn">Lista de registroS de la clase cMarcasIn</param>
        public async Task<EventResponse> Sincronizar_MarcaIn(IEnumerable<cMarcaIn> marcasIn)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                foreach (var marcaIn in marcasIn)
                {
                    var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == marcaIn.idnumero);
                    var relojDispositivo = await _context.RelojDispositivo.FirstOrDefaultAsync(e => e.CLOCK_SERIE == marcaIn.idterminal);

                    if (empleado is not null)
                    {
                        marcaIn.idtarjeta = empleado.Tarjeta;
                        marcaIn.idplanilla = empleado.IdPlanilla;
                        marcaIn.idterminal = relojDispositivo is null ? "WPE" : relojDispositivo.CLOCK_ID.ToString();

                        _context.Add(marcaIn);
                        await _context.SaveChangesAsync();                        

                        await EjecutaInMarcasWeb(marcaIn.idnumero!);
                    }
                    else
                    {
                        _logger.LogError($"Sincronizar_MarcaIn: El empleado {marcaIn.idnumero} no existe.");
                    }
                    
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la Marca. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        /// <summary>
        /// EditarMarca: ejecuta procedimiento almacenado para la actualizacion de Marcas
        /// </summary>
        /// <param name="marcas"></param>
        /// <returns>Devuelve una instancia de eventresponse con el resultado de la operacion</returns>
        public async Task<EventResponse> EditarMarca(cMarcaEditParam marcasEdit)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"{_schema}.SALVO_MARCA @fentra='{marcasEdit.fecha_entra.ToString("yyyy-MM-dd")}',@fsale='{marcasEdit.fecha_sale.ToString("yyyy-MM-dd")}',@hentra='{marcasEdit.hora_entra}',@hsale='{marcasEdit.hora_sale}', @turno={marcasEdit.idturno}, @idregistro={marcasEdit.idregistro},@usuario='{marcasEdit.usuario}',@comentario='{marcasEdit.comentario}'";
                        System.Data.Common.DbDataReader result = command.ExecuteReader();
                    }
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Marca. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaExtraApb: Método para obtener todos los datos de la tabla Marcas_Extras_Apb 
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaExtraApb</returns>
        public async Task<List<cMarcaExtraApb>> GetMarcaExtraApb()
        {
            List<cMarcaExtraApb> marcaExtraApb = new();
            try
            {
                marcaExtraApb = await _context.Marcas_Extras_Apb.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaExtraApb: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaExtraApb;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaExtraApb: Método para obtener todos los datos de la tabla Marcas_Extras_Apb para un empleado y un periodo especifico
        /// </summary>
        /// <param name="idnumero">id del empleado</param>
        /// <param name="fecha">fecha para determinar el periodo vigente</param>
        /// <param name="idplanilla">id planilla</param>
        /// <param name="PeriodoVigente">Si es verdadero trae marcas extras del periodo, sino, trae marcas del dia</param>
        /// <returns>Lista de registros de la clase cMarcaExtraApb</returns>
        public async Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string idnumero, string fecha, string idplanilla, bool byPeriodo)
        {
            List<cMarcaExtraApb> marcaExtraApb = new();
            try
            {

                if (byPeriodo)
                {
                    var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                    marcaExtraApb = await _context.Marcas_Extras_Apb
                                          .Where(e => e.idnumero == idnumero && e.idplanilla == idplanilla
                                                 && e.fecha >= periodoVigente.inicio)
                                          .ToListAsync();
                }
                else
                {
                    DateTime fechaDia = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                    marcaExtraApb = await _context.Marcas_Extras_Apb
                                          .Where(e => e.idnumero == idnumero && e.idplanilla == idplanilla
                                                 && e.fecha == fechaDia)
                                          .ToListAsync();
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaExtraApb: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaExtraApb;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaExtraApb: Método para obtener un registro de la tabla Marcas_Extras_Apb con un identificador específico
        /// </summary>
        /// <returns>Registro de la clase Marcas_Extras_Apb</returns>
        /// <param name="idregistro">Número identificador del registro</param>
        public async Task<cMarcaExtraApb> GetMarcaExtraApb(long idregistro)
        {
            cMarcaExtraApb? marcaExtraApb = new();
            try
            {
                marcaExtraApb = await _context.Marcas_Extras_Apb.FirstOrDefaultAsync(e => e.idregistro == idregistro);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaExtraApb: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaExtraApb!;
        }

        /// <summary>
        /// GetMarcaExtraApb: Método para obtener una lista de Horas Extras pendientes de Aprobación de los empleados asignados a un supervisor 
        /// </summary>
        /// <returns>Lista de cMarcaExtraApb</returns>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren las extras</param>
        /// <param name="idgrupo">grupo de empleado</param>
        public async Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string fechaPeriodo, string idgrupo)
        {

            List<cMarcaExtraApb> marcaExtraApb = new();
            try
            {
                idgrupo = HttpUtility.UrlDecode(idgrupo);

                var grupos = idgrupo.Split(",");

                List<cPh_Grupo> phgrupos = new List<cPh_Grupo>();
                foreach (var valor in grupos)
                    phgrupos.Add(new cPh_Grupo
                    {
                        idgrupo = int.Parse(valor),
                    });
                // join g in phgrupos on c.IdGrupo equals g.idgrupo

                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");

                var periodosVigentes = await _context.Ph_Periodos.Where(a => fechaMov >= a.inicio && fechaMov <= a.fin).ToListAsync();
                var planillas = await _context.Ph_Planilla.ToListAsync();

                var empleados = await _context.Empleados.Where(e => grupos.Contains(e.IdGrupo.ToString())).ToListAsync();


                var periodos = (from a in periodosVigentes
                                      join b in planillas on a.tipo_planilla equals b.tipo_planilla
                                      join c in empleados on b.idplanilla equals c.IdPlanilla
                                      select new
                                      {
                                          idplanilla = b.idplanilla,
                                          inicio = a.inicio,
                                          fin = a.fin,
                                          nivel_aprob_ext = b.nivel_aprob_ext,
                                      }).Distinct().ToList();
                foreach (var periodo in periodos)
                {
                    List<cMarcaExtraApb> marcasextrasApb = new();
                    switch (periodo.nivel_aprob_ext)
                    {
                        case 1:
                            var marcarPendientes = await _context.Marcas_Extras_Apb.Where(e => e.idplanilla == periodo.idplanilla && e.estado == 'A'
                                                                                                && ((e.aprob_nivel1 == 'F' || e.aprob_nivel1 == null) && e.fecha_aprob_nivel1 == null
                                                                                                && e.aprob_nivel1 == null) && ((e.fecha >= periodo.inicio && e.fecha <= periodo.fin)
                                                                                                || (e.fecha >= periodo.inicio && e.fecha > periodo.fin))).ToListAsync();
                            marcasextrasApb = (from m in marcarPendientes
                                               join c in empleados on new { idnumero = m.idnumero, idplanilla = m.idplanilla } 
                                                        equals new { idnumero = c.IdNumero, idplanilla = c.IdPlanilla }                                                     
                                                     select m).ToList();
                            break;
                        case 2:
                            marcasextrasApb = (from m in await _context.Marcas_Extras_Apb.Where(m => m.idplanilla == periodo.idplanilla && m.estado == 'A'
                                                                                                && (((m.aprob_nivel1 == 'F' || m.aprob_nivel1 == null) && m.fecha_aprob_nivel1 == null && m.aprob_nivel1 == null) ||
                                                                                                    ((m.aprob_nivel2 == 'F' || m.aprob_nivel2 == null) && m.fecha_aprob_nivel2 == null && m.aprob_nivel2 == null))
                                                                                                && ((m.fecha >= periodo.inicio && m.fecha <= periodo.fin)
                                                                                                    || (m.fecha >= periodo.inicio && m.fecha > periodo.fin))).ToListAsync()
                                                     join c in _context.Empleados on new { idnumero = m.idnumero, idplanilla = m.idplanilla } 
                                                        equals new { idnumero = c.IdNumero, idplanilla = c.IdPlanilla }
                                                     select m).ToList();
                            break;
                        case 3:
                            marcasextrasApb = (from m in await _context.Marcas_Extras_Apb.Where(m => m.idplanilla == periodo.idplanilla && m.estado == 'A'
                                                                                               && (((m.aprob_nivel1 == 'F' || m.aprob_nivel1 == null) && m.fecha_aprob_nivel1 == null && m.aprob_nivel1 == null) ||
                                                                                                   ((m.aprob_nivel2 == 'F' || m.aprob_nivel2 == null) && m.fecha_aprob_nivel2 == null && m.aprob_nivel2 == null) ||
                                                                                                   ((m.aprob_nivel3 == 'F' || m.aprob_nivel3 == null) && m.fecha_aprob_nivel3 == null && m.aprob_nivel3 == null))
                                                                                               && ((m.fecha >= periodo.inicio && m.fecha <= periodo.fin)
                                                                                                  || (m.fecha >= periodo.inicio && m.fecha > periodo.fin))).ToListAsync()
                                                     join c in _context.Empleados on new { idnumero = m.idnumero, idplanilla = m.idplanilla } 
                                                        equals new { idnumero = c.IdNumero, idplanilla = c.IdPlanilla }                                                     
                                                     select m).ToList();
                            break;

                    }


                    marcaExtraApb.AddRange(marcasextrasApb);
                }


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaExtraApb: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaExtraApb;
        }


        /// <summary>
        ///  GetMarcaExtraApb: Método para obtener una lista de Horas Extras en un rango de fechas para empleados de los grupos indicados en los parametros 
        /// </summary>
        /// <param name="idsgrupos"></param>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns>Lista de marcas por horas extras del periodo</returns>
        public async Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string idsgrupos, string idplanilla, string fechaInicio, string fechaFinal, char estado)
        {

            List<cMarcaExtraApb> marcaExtraApb = new();
            try
            {
                idsgrupos = HttpUtility.UrlDecode(idsgrupos);

                var grupos = idsgrupos.Split(",");

                List<cPh_Grupo> phgrupos = new List<cPh_Grupo>();

                foreach (var valor in grupos)
                    phgrupos.Add(new cPh_Grupo
                    {
                        idgrupo = int.Parse(valor),
                    });

                DateTime fechaInicioExt = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaFinExt = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                marcaExtraApb = (from m in await _context.Marcas_Extras_Apb.Where(e => e.idplanilla == idplanilla && e.fecha>= fechaInicioExt && e.fecha<=fechaFinExt && e.aprob_nivel1==estado && e.estado == 'A').ToListAsync()
                                            join c in await _context.Empleados.ToListAsync() on new { idnumero = m.idnumero, cidplanilla = m.idplanilla } equals new { idnumero = c.IdNumero, cidplanilla = c.IdPlanilla }
                                            join g in phgrupos on c.IdGrupo equals g.idgrupo
                                         select m).ToList();
                            


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaExtraApb: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); 
                throw;
            }
            return marcaExtraApb;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// Sincronizar_MarcaExtraApb: Método para registrar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public async Task<EventResponse> Sincronizar_MarcaExtraApb(IEnumerable<cMarcaExtraApb> marcasExtraApb)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var marcaExtraApb in marcasExtraApb)
                {
                    cMarcaExtraApb? marcaExtra = await _context.Marcas_Extras_Apb
                                    .Where(e => e.idregistro == marcaExtraApb.idregistro)
                                    .FirstOrDefaultAsync();
                    //si la marca de hora extra existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (marcaExtra is not null)
                    {
                        marcaExtra.cantidad = marcaExtraApb.cantidad;
                        marcaExtra.hora = marcaExtraApb.hora;
                        marcaExtra.comentario = marcaExtraApb.comentario;
                        marcaExtra.ccosto = marcaExtraApb.ccosto;

                        _context.Marcas_Extras_Apb.Update(marcaExtra);

                        var marcaProceso = await _context.Marcas_Proceso.FirstOrDefaultAsync(e => e.fecha_entra >= marcaExtra.fecha && e.fecha_sale <= marcaExtra.fecha && e.idnumero == marcaExtra.idnumero && e.idplanilla == marcaExtra.idplanilla);

                        if (marcaProceso is not null)
                        {
                            marcaProceso.EXTT = marcaExtra.cantidad;
                        }
                    }
                    else
                    {
                        marcaExtraApb.idregistro = 0;
                        _context.Add(marcaExtraApb);

                        var marcaProceso = await _context.Marcas_Proceso.FirstOrDefaultAsync(e => e.fecha_entra >= marcaExtraApb.fecha && e.fecha_sale <= marcaExtraApb.fecha && e.idnumero == marcaExtraApb.idnumero && e.idplanilla == marcaExtraApb.idplanilla);

                        if (marcaProceso is not null)
                        {
                            marcaProceso.EXTT = marcaExtraApb.cantidad;
                        }
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la Hora Extra. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la Hora Extra. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// Autorizar_MarcaExtraApb: Método para autorizar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public async Task<EventResponse> Autorizar_MarcaExtraApb(IEnumerable<cMarcaExtraApb> marcasExtraApb)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var marcaExtraApb in marcasExtraApb)
                {

                    cMarcaExtraApb? marcaExtra = await _context.Marcas_Extras_Apb
                                    .Where(e => e.idregistro == marcaExtraApb.idregistro)
                                    .FirstOrDefaultAsync();
                    //si la marca de hora extra existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (marcaExtra is not null)
                    {
                        marcaExtra.aprob_nivel1 = marcaExtraApb.aprob_nivel1;
                        marcaExtra.aprob_nivel2 = marcaExtraApb.aprob_nivel2;
                        marcaExtra.aprob_nivel3 = marcaExtraApb.aprob_nivel3;
                        marcaExtra.usuario_aprob_nivel1 = marcaExtraApb.usuario_aprob_nivel1;
                        marcaExtra.usuario_aprob_nivel2 = marcaExtraApb.usuario_aprob_nivel2;
                        marcaExtra.usuario_aprob_nivel3 = marcaExtraApb.usuario_aprob_nivel3;
                        marcaExtra.cantidad_aprob_nivel1 = marcaExtraApb.cantidad_aprob_nivel1;
                        marcaExtra.cantidad_aprob_nivel2 = marcaExtraApb.cantidad_aprob_nivel2;
                        marcaExtra.cantidad_aprob_nivel3 = marcaExtraApb.cantidad_aprob_nivel3;
                        marcaExtra.comentario_aprob_nivel1 = marcaExtraApb.comentario_aprob_nivel1;
                        marcaExtra.comentario_aprob_nivel2 = marcaExtraApb.comentario_aprob_nivel2;
                        marcaExtra.comentario_aprob_nivel3 = marcaExtraApb.comentario_aprob_nivel3;
                        marcaExtra.fecha_aprob_nivel1 = marcaExtraApb.fecha_aprob_nivel1;
                        marcaExtra.fecha_aprob_nivel2 = marcaExtraApb.fecha_aprob_nivel2;
                        marcaExtra.fecha_aprob_nivel3 = marcaExtraApb.fecha_aprob_nivel3;

                        string cantidadAprobada = marcaExtra.cantidad_aprob_nivel3 == null ? (marcaExtra.cantidad_aprob_nivel2 == null ? marcaExtra.cantidad_aprob_nivel1 : marcaExtra.cantidad_aprob_nivel2) : marcaExtra.cantidad_aprob_nivel3;

                        _context.Marcas_Extras_Apb.Update(marcaExtra);

                        var marcaProceso = await _context.Marcas_Proceso.FirstOrDefaultAsync(e => e.fecha_entra >= marcaExtra.fecha && e.fecha_sale <= marcaExtra.fecha && e.idnumero == marcaExtra.idnumero && e.idplanilla == marcaExtra.idplanilla);

                        if (marcaProceso is not null)
                        {
                            marcaProceso.EXTT = cantidadAprobada;
                        }
                    }

                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la autorización de la solicitud de Hora Extra. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la autorización de la solicitud de Hora Extra. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetProyecto: Obtener lista de Proyectos
        /// </summary>
        /// <returns>Lista de objetos del tipo cPh_Proyecto</returns>

        public async Task<List<cPh_Proyecto>> GetProyecto()
        {
            List<cPh_Proyecto> ph_Proyecto = new();
            try
            {
                ph_Proyecto = await _context.Ph_Proyecto.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetProyecto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return ph_Proyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetFaseProyecto: Obtener un Proyecto especifica
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_Proyecto</returns>
        public async Task<cPh_Proyecto> GetProyecto(string idproyecto)
        {
            cPh_Proyecto? ph_Proyecto = new();
            try
            {
                ph_Proyecto = await _context.Ph_Proyecto.FirstOrDefaultAsync(e => e.PROYECTO == idproyecto);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetProyecto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return ph_Proyecto;
        }

        public async Task<List<cPh_Proyecto>> GetProyectoByCCosto(string idccosto)
        {
            List<cPh_Proyecto> ph_Proyecto = new();
            try
            {
                ph_Proyecto = await _context.Ph_Proyecto.Where(e=>e.CENTRO_COSTO == idccosto).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetProyectoByCCosto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return ph_Proyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// Sincronizar_Proyectos: Sincronizar proyectos, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="phProyectos">Recibe una instancia del tipo cProyecto</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>.
        public async Task<EventResponse> Sincronizar_Proyectos(IEnumerable<cPh_Proyecto> phProyectos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var proyecto in phProyectos)
                {
                    cPh_Proyecto? proyectoBuscar = await _context.Ph_Proyecto
                                                        .Where(e => e.PROYECTO == proyecto.PROYECTO)
                                                        .FirstOrDefaultAsync();
                    //si el proyecto existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (proyectoBuscar is not null)
                    {
                        proyectoBuscar.DESCRIPCION = proyecto.DESCRIPCION;
                        proyectoBuscar.CENTRO_COSTO = proyecto.CENTRO_COSTO;

                        _context.Ph_Proyecto.Update(proyectoBuscar);
                    }
                    else
                    {
                        _context.Add(proyecto);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Proyectos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Proyectos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetFaseProyecto: Obtener lista de Fases de Proyectos
        /// </summary>
        /// <returns>Una lista de objetos del tipo cPh_FaseProyecto</returns>
        public async Task<List<cPh_FaseProyecto>> GetFaseProyecto()
        {
            List<cPh_FaseProyecto> phFaseProyecto = new();
            try
            {
                phFaseProyecto = await _context.Ph_FaseProyecto.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetFaseProyecto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phFaseProyecto;
        }

        /// <summary>
        /// GetFaseProyecto: Obtener lista de Fase de Proyecto para un proyecto.
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_FaseProyecto</returns>
        public async Task<List<cPh_FaseProyecto>> GetFaseProyecto(string idproyecto)
        {
            List<cPh_FaseProyecto> phFaseProyecto = new();
            try
            {
                phFaseProyecto = await _context.Ph_FaseProyecto.Where(e=>e.PROYECTO== idproyecto).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetFaseProyecto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phFaseProyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetFaseProyecto: Obtener una Fase de Proyecto especifica
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <param name="fase">fase del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_FaseProyecto</returns>
        public async Task<cPh_FaseProyecto> GetFaseProyecto(string idproyecto, string fase)
        {
            cPh_FaseProyecto? phFaseProyecto = new();
            try
            {
                phFaseProyecto = await _context.Ph_FaseProyecto.FirstOrDefaultAsync(e => e.PROYECTO == idproyecto && e.FASE == fase);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetFaseProyecto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phFaseProyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// Sincronizar_FaseProyectos: Sincronizar fases de proyectos, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="phFaseProyectos">Recibe una instancia del tipo cFaseProyecto</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>
        public async Task<EventResponse> Sincronizar_FaseProyectos(IEnumerable<cPh_FaseProyecto> phFaseProyectos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var proyectoFase in phFaseProyectos)
                {
                    cPh_FaseProyecto? proyectoFaseBuscar = await _context.Ph_FaseProyecto
                                                        .Where(e => e.PROYECTO == proyectoFase.PROYECTO && e.FASE == proyectoFase.FASE)
                                                        .FirstOrDefaultAsync();
                    //si el proyectoFase existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (proyectoFaseBuscar is not null)
                    {
                        proyectoFaseBuscar.NOMBRE = proyectoFase.NOMBRE;
                        proyectoFaseBuscar.DESCRIPCION = proyectoFase.DESCRIPCION;
                        proyectoFaseBuscar.ACEPTA_DATOS = proyectoFase.ACEPTA_DATOS;

                        _context.Ph_FaseProyecto.Update(proyectoFaseBuscar);
                    }
                    else
                    {
                        _context.Add(proyectoFase);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las Fases de Proyectos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las Fases de Proyectos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetMarcas: Obtener las marcas proceso para el periodo 
        /// </summary>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarcaProceso>> GetMarcasProceso(string fecha)
        {
            List<cMarcaProceso>? marca = new();
            try
            {
                var periodos = await GetPeriodo(fecha, "T");

                Task task = new Task(() =>
                {
                    marca = (from p in periodos
                             join pl in _context.Ph_Planilla on p.tipo_planilla equals pl.tipo_planilla
                             join m in _context.Marcas_Proceso on pl.idplanilla equals m.idplanilla
                             where m.fecha_entra >= p.inicio && m.fecha_entra <= p.fin
                             select m).ToList();
                });
                task.Start();
                task.Wait();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasProceso: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetMarcas: Obtener las marcas proceso de un empleado para el periodo 
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarcaProceso>> GetMarcasProceso(string idnumero, string fecha)
        {
            List<cMarcaProceso>? marca = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marca = await _context.Marcas_Proceso.Where(e => e.idnumero == idnumero && e.fecha_entra >= periodoVigente.inicio && e.fecha_entra <= periodoVigente.fin).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasProceso: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-09-2
        //Obtener lista de Marcas procesos para Entrada - Salida
        public async Task<List<cMarcaProceso>> GetMarcasProceso(string IdPlanilla, string FechaInicio, string FechaFin, string idgrupo)
        {
            List<cMarcaProceso> marcasProceso = new();
            try
            {

                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}");

                idgrupo = HttpUtility.UrlDecode(idgrupo);

                var grupoIds = idgrupo.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(g => int.Parse(g.Trim()))
                              .ToList();


                var query = _context.Marcas_Proceso
                            .Include(e => e.cEmpleado)
                            .Include(e => e.cTurno)
                    .Where(m =>
                        m.idplanilla == IdPlanilla &&
                        m.fecha_entra >= fechaMovInicio &&
                        m.fecha_entra <= fechaMovFinal);
                   

                var filteredMarcas = (from ap in query
                                      join g in grupoIds on ap.cEmpleado!.IdGrupo equals g
                                      select ap);

                marcasProceso = await filteredMarcas.Select(ap => new cMarcaProceso
                {
                    idregistro = ap.idregistro,
                    idplanilla = ap.idplanilla,
                    idnumero = ap.idnumero,
                    fecha_entra = ap.fecha_entra,
                    fecha_sale = ap.fecha_sale,
                    hora_entra = ap.hora_entra,
                    hora_sale = ap.hora_sale,
                    idturno = ap.idturno,
                    ORDC = ap.ORDC,
                    EXTC = ap.EXTC,
                    ORDT = ap.ORDT,
                    EXTT = ap.EXTT,
                    TC1 = ap.TC1,
                    TC2 = ap.TC2,
                    TC3 = ap.TC3,
                    TC4 = ap.TC4,
                    TC5 = ap.TC5,
                    CON_1 = ap.CON_1,
                    CON_2 = ap.CON_2,
                    CON_3 = ap.CON_3,
                    CON_4 = ap.CON_4,
                    CON_5 = ap.CON_5,
                    ID1 = ap.ID1,
                    ID2 = ap.ID2,
                    ID3 = ap.ID3,
                    FD1 = ap.FD1,
                    FD2 = ap.FD2,
                    FD3 = ap.FD3,
                    TOD = ap.TOD,
                    TED = ap.TED,
                    TDD = ap.TDD,
                    TIEMPO_CALC_JORN = ap.TIEMPO_CALC_JORN,
                    TDC = ap.TDC,
                    estado = ap.estado,
                    estado_inc = ap.estado_inc,
                    manticipo = ap.manticipo,
                    mtardia = ap.mtardia,
                    proyectado = ap.proyectado,
                    reg_sale = ap.reg_sale,
                    cEmpleado = ap.cEmpleado == null ? null :
                                                new cEmpleado
                                                {
                                                    IdNumero = ap.cEmpleado.IdNumero,
                                                    IdPlanilla = ap.cEmpleado.IdPlanilla,
                                                    Nombre = ap.cEmpleado.Nombre,
                                                    Tarjeta = ap.cEmpleado.Tarjeta,
                                                    Identificacion = ap.cEmpleado.Identificacion,
                                                    IdGrupo = ap.cEmpleado.IdGrupo,
                                                    IdDepartamento = ap.cEmpleado.IdDepartamento,
                                                    IdHorario = ap.cEmpleado.IdHorario,
                                                    Estado = ap.cEmpleado.Estado,
                                                    IdAgrupamiento = ap.cEmpleado.IdAgrupamiento,
                                                    foto = ap.cEmpleado.foto,
                                                    IdCCosto = ap.cEmpleado.IdCCosto,
                                                    exporta = ap.cEmpleado.exporta,
                                                    ubicacion = ap.cEmpleado.ubicacion,
                                                    rubro1 = ap.cEmpleado.rubro1,
                                                    rubro2 = ap.cEmpleado.rubro2,
                                                    rubro3 = ap.cEmpleado.rubro3,
                                                    rubro4 = ap.cEmpleado.rubro4,
                                                    rubro5 = ap.cEmpleado.rubro5,
                                                    rubro6 = ap.cEmpleado.rubro6,
                                                    rubro7 = ap.cEmpleado.rubro7,
                                                    rubro8 = ap.cEmpleado.rubro8,
                                                    rubro9 = ap.cEmpleado.rubro9,
                                                    rubro10 = ap.cEmpleado.rubro10,
                                                    rubro11 = ap.cEmpleado.rubro11,
                                                    rubro12 = ap.cEmpleado.rubro12,
                                                    rubro13 = ap.cEmpleado.rubro13,
                                                    rubro14 = ap.cEmpleado.rubro14,
                                                    rubro15 = ap.cEmpleado.rubro15,
                                                    rubro16 = ap.cEmpleado.rubro16,
                                                    rubro17 = ap.cEmpleado.rubro17,
                                                    rubro18 = ap.cEmpleado.rubro18,
                                                    rubro19 = ap.cEmpleado.rubro19,
                                                    rubro20 = ap.cEmpleado.rubro20,
                                                    rubro21 = ap.cEmpleado.rubro21,
                                                    rubro22 = ap.cEmpleado.rubro22,
                                                    rubro23 = ap.cEmpleado.rubro23,
                                                    rubro24 = ap.cEmpleado.rubro24,
                                                    rubro25 = ap.cEmpleado.rubro25,
                                                    Fecha_Ingreso = ap.cEmpleado.Fecha_Ingreso,
                                                    Email = ap.cEmpleado.Email,
                                                    Tipo_Marca = ap.cEmpleado.Tipo_Marca,
                                                    inicio_rol = ap.cEmpleado.inicio_rol,
                                                    web_pass = ap.cEmpleado.web_pass,
                                                    id_transfo_conc = ap.cEmpleado.id_transfo_conc,
                                                    widioma = ap.cEmpleado.widioma,
                                                    def_cc = ap.cEmpleado.def_cc,
                                                    def_py = ap.cEmpleado.def_py,
                                                    def_fase = ap.cEmpleado.def_fase,
                                                    global_clave = ap.cEmpleado.global_clave,
                                                    Fecha_Salida = ap.cEmpleado.Fecha_Salida,
                                                    global_code = ap.cEmpleado.global_code,
                                                    fecha_act_code = ap.cEmpleado.fecha_act_code,
                                                },
                    cTurno = ap.cTurno == null ? null :
                                                new cTurno
                                                {
                                                    IdTurno = ap.cTurno.IdTurno,
                                                    Descripcion = ap.cTurno.Descripcion,
                                                    HEntra = ap.cTurno.HEntra,
                                                    HSale = ap.cTurno.HSale,
                                                    tar_apl = ap.cTurno.tar_apl,
                                                    ant_apl = ap.cTurno.ant_apl,
                                                    des_1_in = ap.cTurno.des_1_in,
                                                    des_1_out = ap.cTurno.des_1_out,
                                                    des_2_in = ap.cTurno.des_2_in,
                                                    des_2_out = ap.cTurno.des_2_out,
                                                    des_3_in = ap.cTurno.des_3_in,
                                                    des_3_out = ap.cTurno.des_3_out,
                                                    apl_des_1 = ap.cTurno.apl_des_1,
                                                    apl_des_2 = ap.cTurno.apl_des_2,
                                                    apl_des_3 = ap.cTurno.apl_des_3,
                                                    des_1_tiem = ap.cTurno.des_1_tiem,
                                                    des_2_tiem = ap.cTurno.des_2_tiem,
                                                    des_3_tiem = ap.cTurno.des_3_tiem,
                                                    marca_des_1 = ap.cTurno.marca_des_1,
                                                    marca_des_2 = ap.cTurno.marca_des_2,
                                                    marca_des_3 = ap.cTurno.marca_des_3,
                                                    tar_tiem = ap.cTurno.tar_tiem,
                                                    ant_tiem = ap.cTurno.ant_tiem,
                                                    con_1 = ap.cTurno.con_1,
                                                    con_2 = ap.cTurno.con_2,
                                                    con_3 = ap.cTurno.con_3,
                                                    con_4 = ap.cTurno.con_4,
                                                    con_5 = ap.cTurno.con_5,
                                                    con_6 = ap.cTurno.con_6,
                                                    cant_con_1 = ap.cTurno.cant_con_1,
                                                    cant_con_2 = ap.cTurno.cant_con_2,
                                                    cant_con_3 = ap.cTurno.cant_con_3,
                                                    cant_con_4 = ap.cTurno.cant_con_4,
                                                    cant_con_5 = ap.cTurno.cant_con_5,
                                                    cant_con_6 = ap.cTurno.cant_con_6,
                                                    min_con_1 = ap.cTurno.min_con_1,
                                                    min_con_2 = ap.cTurno.min_con_2,
                                                    min_con_3 = ap.cTurno.min_con_3,
                                                    min_con_4 = ap.cTurno.min_con_4,
                                                    min_con_5 = ap.cTurno.min_con_5,
                                                    min_con_6 = ap.cTurno.min_con_6,
                                                    Tipo = ap.cTurno.Tipo,
                                                    Tipo_Jor = ap.cTurno.Tipo_Jor,
                                                    fuerza_calc = ap.cTurno.fuerza_calc,
                                                    idagrupamiento = ap.cTurno.idagrupamiento,
                                                    apl_trans1 = ap.cTurno.apl_trans1,
                                                    id_trans1 = ap.cTurno.id_trans1,
                                                    apl_trans2 = ap.cTurno.apl_trans2,
                                                    id_trans2 = ap.cTurno.id_trans2,
                                                    apl_trans3 = ap.cTurno.apl_trans3,
                                                    id_trans3 = ap.cTurno.id_trans3,
                                                    apl_trans4 = ap.cTurno.apl_trans4,
                                                    id_trans4 = ap.cTurno.id_trans4,
                                                    apl_trans5 = ap.cTurno.apl_trans5,
                                                    id_trans5 = ap.cTurno.id_trans5,
                                                    apl_trans6 = ap.cTurno.apl_trans6,
                                                    id_trans6 = ap.cTurno.id_trans6,
                                                    apl_ben1 = ap.cTurno.apl_ben1,
                                                    id_ben1 = ap.cTurno.id_ben1,
                                                    apl_ben2 = ap.cTurno.apl_ben2,
                                                    id_ben2 = ap.cTurno.id_ben2,
                                                    apl_ben3 = ap.cTurno.apl_ben3,
                                                    id_ben3 = ap.cTurno.id_ben3,
                                                    apl_ben4 = ap.cTurno.apl_ben4,
                                                    id_ben4 = ap.cTurno.id_ben4,
                                                    apl_ben5 = ap.cTurno.apl_ben5,
                                                    id_ben5 = ap.cTurno.id_ben5,
                                                    apl_ben6 = ap.cTurno.apl_ben6,
                                                    id_ben6 = ap.cTurno.id_ben6,
                                                    conc_ben1 = ap.cTurno.conc_ben1,
                                                    conc_ben2 = ap.cTurno.conc_ben2,
                                                    conc_ben3 = ap.cTurno.conc_ben3,
                                                    conc_ben4 = ap.cTurno.conc_ben4,
                                                    conc_ben5 = ap.cTurno.conc_ben5,
                                                    conc_ben6 = ap.cTurno.conc_ben6,
                                                    apl_trans_post = ap.cTurno.apl_trans_post,
                                                    id_trans_post = ap.cTurno.id_trans_post,
                                                    apl_redond_entrada = ap.cTurno.apl_redond_entrada,
                                                    cant_redond_entrada = ap.cTurno.cant_redond_entrada,
                                                    auto_pan = ap.cTurno.auto_pan,
                                                    ColorId = ap.cTurno.ColorId,
                                                }
                }).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasProceso: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasProceso;
        }

        /// <summary>
        /// GetMarcasProcesoHorasExtrasPendientes: Marcas Proceso con detalle de Horas Extras Pendientes
        /// </summary>
        /// <param name="IdPlanilla"></param>
        /// <param name="FechaInicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns></returns>
        public async Task<List<cMarcaProceso>> GetMarcasProcesoHorasExtrasPendientes(string IdPlanilla, string FechaInicio, string FechaFin)
        {
            List<cMarcaProceso> accionPersonal = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}");

                var marcasProcesos = await _context.Marcas_Proceso
                                        .Include(e => e.cEmpleado)
                                        .Include(e => e.cTurno)
                                        .Where(e => e.idplanilla == IdPlanilla &&
                                            e.EXTC!="00:00" && e.EXTT=="00:00" &&
                                            e.fecha_entra >= fechaMovInicio && e.fecha_sale <= fechaMovFinal).ToListAsync();

                accionPersonal = marcasProcesos.Select(ap => new cMarcaProceso
                {
                    idregistro = ap.idregistro,
                    idplanilla = ap.idplanilla,
                    idnumero = ap.idnumero,
                    fecha_entra = ap.fecha_entra,
                    fecha_sale = ap.fecha_sale,
                    hora_entra = ap.hora_entra,
                    hora_sale = ap.hora_sale,
                    idturno = ap.idturno,

                    ORDC = ap.ORDC,
                    EXTC = ap.EXTC,
                    ORDT = ap.ORDT,
                    EXTT = ap.EXTT,
                    TC1 = ap.TC1,
                    TC2 = ap.TC2,
                    TC3 = ap.TC3,
                    TC4 = ap.TC4,
                    TC5 = ap.TC5,
                    CON_1 = ap.CON_1,
                    CON_2 = ap.CON_2,
                    CON_3 = ap.CON_3,
                    CON_4 = ap.CON_4,
                    CON_5 = ap.CON_5,
                    ID1 = ap.ID1,
                    ID2 = ap.ID2,
                    ID3 = ap.ID3,
                    FD1 = ap.FD1,
                    FD2 = ap.FD2,
                    FD3 = ap.FD3,
                    TOD = ap.TOD,
                    TED = ap.TED,
                    TDD = ap.TDD,
                    TIEMPO_CALC_JORN = ap.TIEMPO_CALC_JORN,
                    TDC = ap.TDC,
                    estado = ap.estado,
                    estado_inc = ap.estado_inc,
                    manticipo = ap.manticipo,
                    mtardia = ap.mtardia,
                    proyectado = ap.proyectado,
                    reg_sale = ap.reg_sale,
                    cEmpleado = ap.cEmpleado == null ? null :
                                                new cEmpleado
                                                {
                                                    IdNumero = ap.cEmpleado.IdNumero,
                                                    IdPlanilla = ap.cEmpleado.IdPlanilla,
                                                    Nombre = ap.cEmpleado.Nombre,
                                                    Tarjeta = ap.cEmpleado.Tarjeta,
                                                    Identificacion = ap.cEmpleado.Identificacion,
                                                    IdGrupo = ap.cEmpleado.IdGrupo,
                                                    IdDepartamento = ap.cEmpleado.IdDepartamento,
                                                    IdHorario = ap.cEmpleado.IdHorario,
                                                    Estado = ap.cEmpleado.Estado,
                                                    IdAgrupamiento = ap.cEmpleado.IdAgrupamiento,
                                                    foto = ap.cEmpleado.foto,
                                                    IdCCosto = ap.cEmpleado.IdCCosto,
                                                    exporta = ap.cEmpleado.exporta,
                                                    ubicacion = ap.cEmpleado.ubicacion,
                                                    rubro1 = ap.cEmpleado.rubro1,
                                                    rubro2 = ap.cEmpleado.rubro2,
                                                    rubro3 = ap.cEmpleado.rubro3,
                                                    rubro4 = ap.cEmpleado.rubro4,
                                                    rubro5 = ap.cEmpleado.rubro5,
                                                    rubro6 = ap.cEmpleado.rubro6,
                                                    rubro7 = ap.cEmpleado.rubro7,
                                                    rubro8 = ap.cEmpleado.rubro8,
                                                    rubro9 = ap.cEmpleado.rubro9,
                                                    rubro10 = ap.cEmpleado.rubro10,
                                                    rubro11 = ap.cEmpleado.rubro11,
                                                    rubro12 = ap.cEmpleado.rubro12,
                                                    rubro13 = ap.cEmpleado.rubro13,
                                                    rubro14 = ap.cEmpleado.rubro14,
                                                    rubro15 = ap.cEmpleado.rubro15,
                                                    rubro16 = ap.cEmpleado.rubro16,
                                                    rubro17 = ap.cEmpleado.rubro17,
                                                    rubro18 = ap.cEmpleado.rubro18,
                                                    rubro19 = ap.cEmpleado.rubro19,
                                                    rubro20 = ap.cEmpleado.rubro20,
                                                    rubro21 = ap.cEmpleado.rubro21,
                                                    rubro22 = ap.cEmpleado.rubro22,
                                                    rubro23 = ap.cEmpleado.rubro23,
                                                    rubro24 = ap.cEmpleado.rubro24,
                                                    rubro25 = ap.cEmpleado.rubro25,
                                                    Fecha_Ingreso = ap.cEmpleado.Fecha_Ingreso,
                                                    Email = ap.cEmpleado.Email,
                                                    Tipo_Marca = ap.cEmpleado.Tipo_Marca,
                                                    inicio_rol = ap.cEmpleado.inicio_rol,
                                                    web_pass = ap.cEmpleado.web_pass,
                                                    id_transfo_conc = ap.cEmpleado.id_transfo_conc,
                                                    widioma = ap.cEmpleado.widioma,
                                                    def_cc = ap.cEmpleado.def_cc,
                                                    def_py = ap.cEmpleado.def_py,
                                                    def_fase = ap.cEmpleado.def_fase,
                                                    global_clave = ap.cEmpleado.global_clave,
                                                    Fecha_Salida = ap.cEmpleado.Fecha_Salida,
                                                    global_code = ap.cEmpleado.global_code,
                                                    fecha_act_code = ap.cEmpleado.fecha_act_code,
                                                },
                    cTurno = ap.cTurno == null ? null :
                                                new cTurno
                                                {
                                                    IdTurno = ap.cTurno.IdTurno,
                                                    Descripcion = ap.cTurno.Descripcion,
                                                    HEntra = ap.cTurno.HEntra,
                                                    HSale = ap.cTurno.HSale,
                                                    tar_apl = ap.cTurno.tar_apl,
                                                    ant_apl = ap.cTurno.ant_apl,
                                                    des_1_in = ap.cTurno.des_1_in,
                                                    des_1_out = ap.cTurno.des_1_out,
                                                    des_2_in = ap.cTurno.des_2_in,
                                                    des_2_out = ap.cTurno.des_2_out,
                                                    des_3_in = ap.cTurno.des_3_in,
                                                    des_3_out = ap.cTurno.des_3_out,
                                                    apl_des_1 = ap.cTurno.apl_des_1,
                                                    apl_des_2 = ap.cTurno.apl_des_2,
                                                    apl_des_3 = ap.cTurno.apl_des_3,
                                                    des_1_tiem = ap.cTurno.des_1_tiem,
                                                    des_2_tiem = ap.cTurno.des_2_tiem,
                                                    des_3_tiem = ap.cTurno.des_3_tiem,
                                                    marca_des_1 = ap.cTurno.marca_des_1,
                                                    marca_des_2 = ap.cTurno.marca_des_2,
                                                    marca_des_3 = ap.cTurno.marca_des_3,
                                                    tar_tiem = ap.cTurno.tar_tiem,
                                                    ant_tiem = ap.cTurno.ant_tiem,
                                                    con_1 = ap.cTurno.con_1,
                                                    con_2 = ap.cTurno.con_2,
                                                    con_3 = ap.cTurno.con_3,
                                                    con_4 = ap.cTurno.con_4,
                                                    con_5 = ap.cTurno.con_5,
                                                    con_6 = ap.cTurno.con_6,
                                                    cant_con_1 = ap.cTurno.cant_con_1,
                                                    cant_con_2 = ap.cTurno.cant_con_2,
                                                    cant_con_3 = ap.cTurno.cant_con_3,
                                                    cant_con_4 = ap.cTurno.cant_con_4,
                                                    cant_con_5 = ap.cTurno.cant_con_5,
                                                    cant_con_6 = ap.cTurno.cant_con_6,
                                                    min_con_1 = ap.cTurno.min_con_1,
                                                    min_con_2 = ap.cTurno.min_con_2,
                                                    min_con_3 = ap.cTurno.min_con_3,
                                                    min_con_4 = ap.cTurno.min_con_4,
                                                    min_con_5 = ap.cTurno.min_con_5,
                                                    min_con_6 = ap.cTurno.min_con_6,
                                                    Tipo = ap.cTurno.Tipo,
                                                    Tipo_Jor = ap.cTurno.Tipo_Jor,
                                                    fuerza_calc = ap.cTurno.fuerza_calc,
                                                    idagrupamiento = ap.cTurno.idagrupamiento,
                                                    apl_trans1 = ap.cTurno.apl_trans1,
                                                    id_trans1 = ap.cTurno.id_trans1,
                                                    apl_trans2 = ap.cTurno.apl_trans2,
                                                    id_trans2 = ap.cTurno.id_trans2,
                                                    apl_trans3 = ap.cTurno.apl_trans3,
                                                    id_trans3 = ap.cTurno.id_trans3,
                                                    apl_trans4 = ap.cTurno.apl_trans4,
                                                    id_trans4 = ap.cTurno.id_trans4,
                                                    apl_trans5 = ap.cTurno.apl_trans5,
                                                    id_trans5 = ap.cTurno.id_trans5,
                                                    apl_trans6 = ap.cTurno.apl_trans6,
                                                    id_trans6 = ap.cTurno.id_trans6,
                                                    apl_ben1 = ap.cTurno.apl_ben1,
                                                    id_ben1 = ap.cTurno.id_ben1,
                                                    apl_ben2 = ap.cTurno.apl_ben2,
                                                    id_ben2 = ap.cTurno.id_ben2,
                                                    apl_ben3 = ap.cTurno.apl_ben3,
                                                    id_ben3 = ap.cTurno.id_ben3,
                                                    apl_ben4 = ap.cTurno.apl_ben4,
                                                    id_ben4 = ap.cTurno.id_ben4,
                                                    apl_ben5 = ap.cTurno.apl_ben5,
                                                    id_ben5 = ap.cTurno.id_ben5,
                                                    apl_ben6 = ap.cTurno.apl_ben6,
                                                    id_ben6 = ap.cTurno.id_ben6,
                                                    conc_ben1 = ap.cTurno.conc_ben1,
                                                    conc_ben2 = ap.cTurno.conc_ben2,
                                                    conc_ben3 = ap.cTurno.conc_ben3,
                                                    conc_ben4 = ap.cTurno.conc_ben4,
                                                    conc_ben5 = ap.cTurno.conc_ben5,
                                                    conc_ben6 = ap.cTurno.conc_ben6,
                                                    apl_trans_post = ap.cTurno.apl_trans_post,
                                                    id_trans_post = ap.cTurno.id_trans_post,
                                                    apl_redond_entrada = ap.cTurno.apl_redond_entrada,
                                                    cant_redond_entrada = ap.cTurno.cant_redond_entrada,
                                                    auto_pan = ap.cTurno.auto_pan,
                                                    ColorId = ap.cTurno.ColorId,
                                                }
                }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasProceso: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-11-19
        //Obtener lista de Marcas Tiempo Adicional
        public async Task<cMarcaTiempoAdicional> GetMarcasTiempoAdicional(long IdRegistro)
        {
            cMarcaTiempoAdicional? accionPersonal = new();
            try
            {
                var accionPersonalConsulta = await _context.Marcas_Tiempo_Adicional
                                        .Include(e => e.cEmpleado)
                                        .Include(e => e.cCentroCosto)
                                        .Include(e => e.cConcepto)
                                        .Where(e => e.IDREGISTRO == IdRegistro).ToListAsync();

                accionPersonal = accionPersonalConsulta.Select(ap => new cMarcaTiempoAdicional
                {
                    IDREGISTRO = ap.IDREGISTRO,
                    IDPLANILLA = ap.IDPLANILLA,
                    IDNUMERO = ap.IDNUMERO,
                    PERIODO = ap.PERIODO,
                    IDCONCEPTO = ap.IDCONCEPTO,
                    CANTIDAD = ap.CANTIDAD,
                    FECHA_REFERENCIA = ap.FECHA_REFERENCIA,
                    USUARIO = ap.USUARIO,
                    FECHA_REGISTRO = ap.FECHA_REGISTRO,
                    FECHA_ACTUALIZA = ap.FECHA_ACTUALIZA,
                    CENTRO_COSTO = ap.CENTRO_COSTO,
                    COMENTARIO = ap.COMENTARIO,
                    USUARIO_ACTUALIZA = ap.USUARIO_ACTUALIZA,
                    ESTADO = ap.ESTADO,
                    TCANTIDAD = ap.TCANTIDAD,
                    PROYECTO = ap.PROYECTO,
                    FASE = ap.FASE,
                    idsolicitud = ap.idsolicitud,
                    cEmpleado = ap.cEmpleado == null ? null :
                                                new cEmpleado
                                                {
                                                    IdNumero = ap.cEmpleado.IdNumero,
                                                    IdPlanilla = ap.cEmpleado.IdPlanilla,
                                                    Nombre = ap.cEmpleado.Nombre,
                                                    Tarjeta = ap.cEmpleado.Tarjeta,
                                                    Identificacion = ap.cEmpleado.Identificacion,
                                                    IdGrupo = ap.cEmpleado.IdGrupo,
                                                    IdDepartamento = ap.cEmpleado.IdDepartamento,
                                                    IdHorario = ap.cEmpleado.IdHorario,
                                                    Estado = ap.cEmpleado.Estado,
                                                    IdAgrupamiento = ap.cEmpleado.IdAgrupamiento,
                                                    foto = ap.cEmpleado.foto,
                                                    IdCCosto = ap.cEmpleado.IdCCosto,
                                                    exporta = ap.cEmpleado.exporta,
                                                    ubicacion = ap.cEmpleado.ubicacion,
                                                    rubro1 = ap.cEmpleado.rubro1,
                                                    rubro2 = ap.cEmpleado.rubro2,
                                                    rubro3 = ap.cEmpleado.rubro3,
                                                    rubro4 = ap.cEmpleado.rubro4,
                                                    rubro5 = ap.cEmpleado.rubro5,
                                                    rubro6 = ap.cEmpleado.rubro6,
                                                    rubro7 = ap.cEmpleado.rubro7,
                                                    rubro8 = ap.cEmpleado.rubro8,
                                                    rubro9 = ap.cEmpleado.rubro9,
                                                    rubro10 = ap.cEmpleado.rubro10,
                                                    rubro11 = ap.cEmpleado.rubro11,
                                                    rubro12 = ap.cEmpleado.rubro12,
                                                    rubro13 = ap.cEmpleado.rubro13,
                                                    rubro14 = ap.cEmpleado.rubro14,
                                                    rubro15 = ap.cEmpleado.rubro15,
                                                    rubro16 = ap.cEmpleado.rubro16,
                                                    rubro17 = ap.cEmpleado.rubro17,
                                                    rubro18 = ap.cEmpleado.rubro18,
                                                    rubro19 = ap.cEmpleado.rubro19,
                                                    rubro20 = ap.cEmpleado.rubro20,
                                                    rubro21 = ap.cEmpleado.rubro21,
                                                    rubro22 = ap.cEmpleado.rubro22,
                                                    rubro23 = ap.cEmpleado.rubro23,
                                                    rubro24 = ap.cEmpleado.rubro24,
                                                    rubro25 = ap.cEmpleado.rubro25,
                                                    Fecha_Ingreso = ap.cEmpleado.Fecha_Ingreso,
                                                    Email = ap.cEmpleado.Email,
                                                    Tipo_Marca = ap.cEmpleado.Tipo_Marca,
                                                    inicio_rol = ap.cEmpleado.inicio_rol,
                                                    web_pass = ap.cEmpleado.web_pass,
                                                    id_transfo_conc = ap.cEmpleado.id_transfo_conc,
                                                    widioma = ap.cEmpleado.widioma,
                                                    def_cc = ap.cEmpleado.def_cc,
                                                    def_py = ap.cEmpleado.def_py,
                                                    def_fase = ap.cEmpleado.def_fase,
                                                    global_clave = ap.cEmpleado.global_clave,
                                                    Fecha_Salida = ap.cEmpleado.Fecha_Salida,
                                                    global_code = ap.cEmpleado.global_code,
                                                    fecha_act_code = ap.cEmpleado.fecha_act_code,
                                                },
                    cCentroCosto = ap.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = ap.cCentroCosto.IdCCosto,
                                                    Descripcion = ap.cCentroCosto.Descripcion,
                                                },
                    cConcepto = ap.cConcepto == null ? null :
                                                new cConcepto
                                                {
                                                    id = ap.cConcepto.id,
                                                    Concepto = ap.cConcepto.Concepto,
                                                    Descripcion = ap.cConcepto.Descripcion,
                                                    tipo_j = ap.cConcepto.tipo_j,
                                                    tipo_h = ap.cConcepto.tipo_h,
                                                    columnar = ap.cConcepto.columnar,
                                                    factor = ap.cConcepto.factor,
                                                    tolerancia = ap.cConcepto.tolerancia,
                                                    ordinario = ap.cConcepto.ordinario,
                                                    autorizado = ap.cConcepto.autorizado,
                                                    adicional = ap.cConcepto.adicional,
                                                    nominaeq = ap.cConcepto.nominaeq
                                                }
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasTiempoAdicional: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal!;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-09-2
        //Obtener lista de Marcas Tiempo Adicional
        public async Task<List<cMarcaTiempoAdicional>> GetMarcasTiempoAdicional(string IdPlanilla, string idPeriodo, string FechaInicio, string FechaFin, int idConcepto, string idgrupo)
        {
            List<cMarcaTiempoAdicional> accionPersonal = new();
            try
            {
                DateTime fechaMovInicio = DateTime.Parse($"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}");
                DateTime fechaMovFinal = DateTime.Parse($"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}");

                idgrupo = HttpUtility.UrlDecode(idgrupo);

                string[] ListGrupos = idgrupo?.Split(',');
                List<int> groupIds = ListGrupos
                    .Select(int.Parse)
                    .ToList();

                var accionPersonalConsulta = await _context.Marcas_Tiempo_Adicional
                                        .Include(e => e.cEmpleado)
                                        .Include(e => e.cCentroCosto)
                                        .Include(e => e.cConcepto)
                                        .Where(e => e.IDPLANILLA == IdPlanilla
                                                    && e.PERIODO == idPeriodo
                                                    && e.FECHA_REFERENCIA >= fechaMovInicio && e.FECHA_REFERENCIA <= fechaMovFinal
                                                    && e.IDCONCEPTO == (idConcepto == -1 ? e.IDCONCEPTO : idConcepto))
                                        .ToListAsync();

                var filteredAccionesPersonal = (from ap in accionPersonalConsulta
                                                join g in groupIds on ap.cEmpleado.IdGrupo equals g
                                                select ap).OrderBy(e => e.cEmpleado.Nombre).ToList();

                accionPersonal = filteredAccionesPersonal.Select(ap => new cMarcaTiempoAdicional
                {
                    IDREGISTRO = ap.IDREGISTRO,
                    IDPLANILLA = ap.IDPLANILLA,
                    IDNUMERO = ap.IDNUMERO,
                    PERIODO = ap.PERIODO,
                    IDCONCEPTO = ap.IDCONCEPTO,
                    CANTIDAD = ap.CANTIDAD,
                    FECHA_REFERENCIA = ap.FECHA_REFERENCIA,
                    USUARIO = ap.USUARIO,
                    FECHA_REGISTRO = ap.FECHA_REGISTRO,
                    FECHA_ACTUALIZA = ap.FECHA_ACTUALIZA,
                    CENTRO_COSTO = ap.CENTRO_COSTO,
                    COMENTARIO = ap.COMENTARIO,
                    USUARIO_ACTUALIZA = ap.USUARIO_ACTUALIZA,
                    ESTADO = ap.ESTADO,
                    TCANTIDAD = ap.TCANTIDAD,
                    PROYECTO = ap.PROYECTO,
                    FASE = ap.FASE,
                    idsolicitud = ap.idsolicitud,
                    cEmpleado = ap.cEmpleado == null ? null :
                                                new cEmpleado
                                                {
                                                    IdNumero = ap.cEmpleado.IdNumero,
                                                    IdPlanilla = ap.cEmpleado.IdPlanilla,
                                                    Nombre = ap.cEmpleado.Nombre,
                                                    Tarjeta = ap.cEmpleado.Tarjeta,
                                                    Identificacion = ap.cEmpleado.Identificacion,
                                                    IdGrupo = ap.cEmpleado.IdGrupo,
                                                    IdDepartamento = ap.cEmpleado.IdDepartamento,
                                                    IdHorario = ap.cEmpleado.IdHorario,
                                                    Estado = ap.cEmpleado.Estado,
                                                    IdAgrupamiento = ap.cEmpleado.IdAgrupamiento,
                                                    foto = ap.cEmpleado.foto,
                                                    IdCCosto = ap.cEmpleado.IdCCosto,
                                                    exporta = ap.cEmpleado.exporta,
                                                    ubicacion = ap.cEmpleado.ubicacion,
                                                    rubro1 = ap.cEmpleado.rubro1,
                                                    rubro2 = ap.cEmpleado.rubro2,
                                                    rubro3 = ap.cEmpleado.rubro3,
                                                    rubro4 = ap.cEmpleado.rubro4,
                                                    rubro5 = ap.cEmpleado.rubro5,
                                                    rubro6 = ap.cEmpleado.rubro6,
                                                    rubro7 = ap.cEmpleado.rubro7,
                                                    rubro8 = ap.cEmpleado.rubro8,
                                                    rubro9 = ap.cEmpleado.rubro9,
                                                    rubro10 = ap.cEmpleado.rubro10,
                                                    rubro11 = ap.cEmpleado.rubro11,
                                                    rubro12 = ap.cEmpleado.rubro12,
                                                    rubro13 = ap.cEmpleado.rubro13,
                                                    rubro14 = ap.cEmpleado.rubro14,
                                                    rubro15 = ap.cEmpleado.rubro15,
                                                    rubro16 = ap.cEmpleado.rubro16,
                                                    rubro17 = ap.cEmpleado.rubro17,
                                                    rubro18 = ap.cEmpleado.rubro18,
                                                    rubro19 = ap.cEmpleado.rubro19,
                                                    rubro20 = ap.cEmpleado.rubro20,
                                                    rubro21 = ap.cEmpleado.rubro21,
                                                    rubro22 = ap.cEmpleado.rubro22,
                                                    rubro23 = ap.cEmpleado.rubro23,
                                                    rubro24 = ap.cEmpleado.rubro24,
                                                    rubro25 = ap.cEmpleado.rubro25,
                                                    Fecha_Ingreso = ap.cEmpleado.Fecha_Ingreso,
                                                    Email = ap.cEmpleado.Email,
                                                    Tipo_Marca = ap.cEmpleado.Tipo_Marca,
                                                    inicio_rol = ap.cEmpleado.inicio_rol,
                                                    web_pass = ap.cEmpleado.web_pass,
                                                    id_transfo_conc = ap.cEmpleado.id_transfo_conc,
                                                    widioma = ap.cEmpleado.widioma,
                                                    def_cc = ap.cEmpleado.def_cc,
                                                    def_py = ap.cEmpleado.def_py,
                                                    def_fase = ap.cEmpleado.def_fase,
                                                    global_clave = ap.cEmpleado.global_clave,
                                                    Fecha_Salida = ap.cEmpleado.Fecha_Salida,
                                                    global_code = ap.cEmpleado.global_code,
                                                    fecha_act_code = ap.cEmpleado.fecha_act_code,
                                                },
                    cCentroCosto = ap.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = ap.cCentroCosto.IdCCosto,
                                                    Descripcion = ap.cCentroCosto.Descripcion,
                                                },
                    cConcepto = ap.cConcepto == null ? null :
                                                new cConcepto
                                                {
                                                    id = ap.cConcepto.id,
                                                    Concepto = ap.cConcepto.Concepto,
                                                    Descripcion = ap.cConcepto.Descripcion,
                                                    tipo_j = ap.cConcepto.tipo_j,
                                                    tipo_h = ap.cConcepto.tipo_h,
                                                    columnar = ap.cConcepto.columnar,
                                                    factor = ap.cConcepto.factor,
                                                    tolerancia = ap.cConcepto.tolerancia,
                                                    ordinario = ap.cConcepto.ordinario,
                                                    autorizado = ap.cConcepto.autorizado,
                                                    adicional = ap.cConcepto.adicional,
                                                    nominaeq = ap.cConcepto.nominaeq
                                                }
                }).ToList();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasTiempoAdicional: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return accionPersonal;
        }


        public async Task<List<cMarcaTiempoAdicional>> GetMarcasTiempoAdicional(string IdPlanilla, string idPeriodo, string idnumero)
        {
            List<cMarcaTiempoAdicional> marcasTiempo = new();
            try
            {

                marcasTiempo =  (from ap in  await _context.Marcas_Tiempo_Adicional
                                        .Include(e => e.cEmpleado)
                                        .Include(e => e.cCentroCosto)
                                        .Include(e => e.cConcepto)
                                        .Where(e => e.IDPLANILLA == IdPlanilla
                                                    && e.PERIODO == idPeriodo
                                                    && e.IDNUMERO == idnumero ).ToListAsync()
                               select new cMarcaTiempoAdicional
                                        {
                                            IDREGISTRO = ap.IDREGISTRO,
                                            IDPLANILLA = ap.IDPLANILLA,
                                            IDNUMERO = ap.IDNUMERO,
                                            PERIODO = ap.PERIODO,
                                            IDCONCEPTO = ap.IDCONCEPTO,
                                            CANTIDAD = ap.CANTIDAD,
                                            FECHA_REFERENCIA = ap.FECHA_REFERENCIA,
                                            USUARIO = ap.USUARIO,
                                            FECHA_REGISTRO = ap.FECHA_REGISTRO,
                                            FECHA_ACTUALIZA = ap.FECHA_ACTUALIZA,
                                            CENTRO_COSTO = ap.CENTRO_COSTO,
                                            COMENTARIO = ap.COMENTARIO,
                                            USUARIO_ACTUALIZA = ap.USUARIO_ACTUALIZA,
                                            ESTADO = ap.ESTADO,
                                            TCANTIDAD = ap.TCANTIDAD,
                                            PROYECTO = ap.PROYECTO,
                                            FASE = ap.FASE,
                                            idsolicitud = ap.idsolicitud,
                                            cEmpleado = ap.cEmpleado == null ? null :
                                                new cEmpleado
                                                {
                                                    IdNumero = ap.cEmpleado.IdNumero,
                                                    IdPlanilla = ap.cEmpleado.IdPlanilla,
                                                    Nombre = ap.cEmpleado.Nombre,
                                                    Tarjeta = ap.cEmpleado.Tarjeta,
                                                    Identificacion = ap.cEmpleado.Identificacion,
                                                    IdGrupo = ap.cEmpleado.IdGrupo,
                                                    IdDepartamento = ap.cEmpleado.IdDepartamento,
                                                    IdHorario = ap.cEmpleado.IdHorario,
                                                    Estado = ap.cEmpleado.Estado,
                                                    IdAgrupamiento = ap.cEmpleado.IdAgrupamiento,
                                                    foto = ap.cEmpleado.foto,
                                                    IdCCosto = ap.cEmpleado.IdCCosto,
                                                    exporta = ap.cEmpleado.exporta,
                                                    ubicacion = ap.cEmpleado.ubicacion,
                                                    rubro1 = ap.cEmpleado.rubro1,
                                                    rubro2 = ap.cEmpleado.rubro2,
                                                    rubro3 = ap.cEmpleado.rubro3,
                                                    rubro4 = ap.cEmpleado.rubro4,
                                                    rubro5 = ap.cEmpleado.rubro5,
                                                    rubro6 = ap.cEmpleado.rubro6,
                                                    rubro7 = ap.cEmpleado.rubro7,
                                                    rubro8 = ap.cEmpleado.rubro8,
                                                    rubro9 = ap.cEmpleado.rubro9,
                                                    rubro10 = ap.cEmpleado.rubro10,
                                                    rubro11 = ap.cEmpleado.rubro11,
                                                    rubro12 = ap.cEmpleado.rubro12,
                                                    rubro13 = ap.cEmpleado.rubro13,
                                                    rubro14 = ap.cEmpleado.rubro14,
                                                    rubro15 = ap.cEmpleado.rubro15,
                                                    rubro16 = ap.cEmpleado.rubro16,
                                                    rubro17 = ap.cEmpleado.rubro17,
                                                    rubro18 = ap.cEmpleado.rubro18,
                                                    rubro19 = ap.cEmpleado.rubro19,
                                                    rubro20 = ap.cEmpleado.rubro20,
                                                    rubro21 = ap.cEmpleado.rubro21,
                                                    rubro22 = ap.cEmpleado.rubro22,
                                                    rubro23 = ap.cEmpleado.rubro23,
                                                    rubro24 = ap.cEmpleado.rubro24,
                                                    rubro25 = ap.cEmpleado.rubro25,
                                                    Fecha_Ingreso = ap.cEmpleado.Fecha_Ingreso,
                                                    Email = ap.cEmpleado.Email,
                                                    Tipo_Marca = ap.cEmpleado.Tipo_Marca,
                                                    inicio_rol = ap.cEmpleado.inicio_rol,
                                                    web_pass = ap.cEmpleado.web_pass,
                                                    id_transfo_conc = ap.cEmpleado.id_transfo_conc,
                                                    widioma = ap.cEmpleado.widioma,
                                                    def_cc = ap.cEmpleado.def_cc,
                                                    def_py = ap.cEmpleado.def_py,
                                                    def_fase = ap.cEmpleado.def_fase,
                                                    global_clave = ap.cEmpleado.global_clave,
                                                    Fecha_Salida = ap.cEmpleado.Fecha_Salida,
                                                    global_code = ap.cEmpleado.global_code,
                                                    fecha_act_code = ap.cEmpleado.fecha_act_code,
                                                },
                                            cCentroCosto = ap.cCentroCosto == null ? null :
                                                new cCentroCosto
                                                {
                                                    IdCCosto = ap.cCentroCosto.IdCCosto,
                                                    Descripcion = ap.cCentroCosto.Descripcion,
                                                },
                                            cConcepto = ap.cConcepto == null ? null :
                                                new cConcepto
                                                {
                                                    id = ap.cConcepto.id,
                                                    Concepto = ap.cConcepto.Concepto,
                                                    Descripcion = ap.cConcepto.Descripcion,
                                                    tipo_j = ap.cConcepto.tipo_j,
                                                    tipo_h = ap.cConcepto.tipo_h,
                                                    columnar = ap.cConcepto.columnar,
                                                    factor = ap.cConcepto.factor,
                                                    tolerancia = ap.cConcepto.tolerancia,
                                                    ordinario = ap.cConcepto.ordinario,
                                                    autorizado = ap.cConcepto.autorizado,
                                                    adicional = ap.cConcepto.adicional,
                                                    nominaeq = ap.cConcepto.nominaeq
                                                }
                                        }).ToList();




            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasTiempoAdicional: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasTiempo;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-11-19
        //Sincronizar Marcas Tiempo Adicional
        //Parametro: Recibe una instancia de cMarcaTiempoAdicional, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcasTiempoAdicional(IEnumerable<cMarcaTiempoAdicional> marcasTiempoAdicional)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                foreach (var item in marcasTiempoAdicional)
                {
                    cMarcaTiempoAdicional? marcaTA = await _context.Marcas_Tiempo_Adicional
                                    .Where(e => e.IDREGISTRO == item.IDREGISTRO)
                                    .FirstOrDefaultAsync();
                    //si el registro existe se actualizan los datos necesarios
                    //de lo contrario se agrega como nuevo registro
                    if (marcaTA is not null)
                    {
                        marcaTA.FECHA_REGISTRO = item.FECHA_REGISTRO;
                        marcaTA.IDCONCEPTO = item.IDCONCEPTO;
                        marcaTA.CENTRO_COSTO = item.CENTRO_COSTO;
                        marcaTA.PROYECTO = item.PROYECTO;
                        marcaTA.FASE = item.FASE;
                        marcaTA.COMENTARIO = item.COMENTARIO;
                        marcaTA.CANTIDAD = item.CANTIDAD;
                        marcaTA.TCANTIDAD = item.TCANTIDAD;
                        marcaTA.FECHA_ACTUALIZA =  DateOnly.FromDateTime(DateTime.Now);
                        marcaTA.USUARIO_ACTUALIZA = item.USUARIO_ACTUALIZA;

                        _context.Marcas_Tiempo_Adicional.Update(marcaTA);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas Tiempo Adicional. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas Tiempo Adicional. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-11-19
        /// <summary>
        /// Elimina_MarcasTiempoAdicional:  Metodo borrado de datos de la tabla Marcas Tiempos Adicionales
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_MarcasTiempoAdicional(int id)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cMarcaTiempoAdicional? model = await _context.Marcas_Tiempo_Adicional
                    .FirstOrDefaultAsync(e => e.IDREGISTRO == id);

                if (model is not null)
                {
                    _context.Marcas_Tiempo_Adicional.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el registro de Marcas Tiempo Adicional. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el registro de Marcas Tiempo Adicional. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-25
        /// <summary>
        /// GetMarcasAudit: Obtener las marcas_audit para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Audit</returns>

        public async Task<List<cMarcaAudit>> GetMarcasAudit(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaAudit>? marca = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marca = await _context.Marcas_Audit.Where(e => e.IDNUMERO == (idnumero=="-1"? e.IDNUMERO : idnumero)
                                                        && e.FECHA >= periodoVigente.inicio
                                                        && e.FECHA <= periodoVigente.fin
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasAudit: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }

        /// <summary>
        /// GetMarcasAudit: obtiene las modificaciones a las marcas realizadas en el sistema
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idplanilla"></param>
        /// <returns></returns>
        public async Task<List<cMarcaAudit>> GetMarcasAudit(string idnumero, string fechaInicio, string fechaFinal, string idplanilla)
        {
            List<cMarcaAudit>? marca = new();
            try
            {
                DateTime fechaInicioExt = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaFinExt = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}T23:59:59.999");

                marca = await _context.Marcas_Audit.Where(e => e.IDNUMERO == (idnumero == "-1" ? e.IDNUMERO : idnumero)
                                                        && e.FECHA_ORIG >= fechaInicioExt
                                                        && e.FECHA_ORIG <= fechaFinExt
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasAudit: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marca;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-25
        /// <summary>
        /// GetMarcasAudit: Obtener las Marcas Descansos para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Descansos</returns>

        public async Task<List<cMarcaDescanso>> GetMarcasDescansos(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaDescanso>? marcaDescanso = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marcaDescanso = await _context.Marcas_Descansos.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= periodoVigente.inicio
                                                        && e.FECHA <= periodoVigente.fin
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcasDescansos: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaDescanso;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-09-01
        /// <summary>
        /// GetMarcaIncidencia: Obtener una Marca Incidencia especifica segun el id indicado en el parámetro
        /// </summary>
        /// <param name="id">numero de marca incidencia</param>
        /// <returns>Una instancia de Marcas Incidencias</returns>

        public async Task<cMarcaIncidencia> GetMarcaIncidencia(long id)
        {
            cMarcaIncidencia? marcaIncidencia = new();
            try
            {
                marcaIncidencia = await _context.Marcas_Incidencias.FirstOrDefaultAsync(e => e.INDICE == id);

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIncidencia: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIncidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-09-01
        /// <summary>
        /// GetMarcaIncidencia: Obtener las Marcas Incidencias para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Incidencias</returns>

        public async Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marcaIncidencia = await _context.Marcas_Incidencias.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= periodoVigente.inicio
                                                        && e.FECHA <= periodoVigente.fin
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIncidencia: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIncidencia;
        }      

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-09-01
        /// <summary>
        /// GetMarcaIncidencia: Obtener las Marcas Incidencias para un empleado, planilla y para un rango de fechas especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <param name="fechaInicio">Fecha de Inicio</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns>Lista de Marcas Incidencias</returns>

        public async Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string idplanilla, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {

                marcaIncidencia = await _context.Marcas_Incidencias.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= fechaInicio
                                                        && e.FECHA <= fechaFinal
                                                        && e.IDACC == null
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIncidencia: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIncidencia;
        }

        /// <summary>
        /// GetMarcaIncidenciaPeriodo: Obtener las Marcas Incidencias para un tipo planilla y un rango de fechas especifico
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns>Lista de incidencias del periodo</returns>
        public async Task<List<cMarcaIncidencia>> GetMarcaIncidenciaPeriodo(string idplanilla, string fechaInicio, string fechaFinal)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {
                DateTime fechaInicioExt = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaFinExt = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                marcaIncidencia = await (from e in _context.Marcas_Incidencias
                                                   .Include(e => e.cIncidencia)
                                                   .Include(e => e.cIncidenciaJust)
                                       .Where(e => e.FECHA >= fechaInicioExt
                                               && e.FECHA <= fechaFinExt
                                               && e.IDPLANILLA == idplanilla)
                                         select new cMarcaIncidencia
                                         {
                                             INDICE = e.INDICE,
                                             IDPLANILLA = e.IDPLANILLA,
                                             IDNUMERO = e.IDNUMERO,
                                             FECHA = e.FECHA,
                                             IDINCIDENCIA = e.IDINCIDENCIA,
                                             IDREGISTRO = e.IDREGISTRO,
                                             HENTRA = e.HENTRA,
                                             HSALE = e.HSALE,
                                             EST_P = e.EST_P,
                                             COMENTARIO = e.COMENTARIO,
                                             INCIDENCIA_JUST = e.INCIDENCIA_JUST,
                                             ESTADO = e.ESTADO,
                                             C_TIEMPO = e.C_TIEMPO,
                                             USUARIO = e.USUARIO,
                                             FECHA_JUST = e.FECHA_JUST,
                                             IDACC = e.IDACC,
                                             cIncidencia = e.cIncidencia == null ? null :
                                                           new cIncidencia
                                                           {
                                                               Id = e.cIncidencia.Id,
                                                               Codigo = e.cIncidencia.Codigo,
                                                               Descripcion = e.cIncidencia.Descripcion,
                                                               id_pago = e.cIncidencia.id_pago,
                                                               nom_conector = e.cIncidencia.nom_conector,
                                                               tipo = e.cIncidencia.tipo,
                                                               ed_tiempo = e.cIncidencia.ed_tiempo,
                                                               requiere_accper = e.cIncidencia.requiere_accper,
                                                               marca_web = e.cIncidencia.marca_web,
                                                           },
                                             cIncidenciaJust = e.cIncidenciaJust == null ? null :
                                                           new cIncidencia
                                                           {
                                                               Id = e.cIncidenciaJust.Id,
                                                               Codigo = e.cIncidenciaJust.Codigo,
                                                               Descripcion = e.cIncidenciaJust.Descripcion,
                                                               id_pago = e.cIncidenciaJust.id_pago,
                                                               nom_conector = e.cIncidenciaJust.nom_conector,
                                                               tipo = e.cIncidenciaJust.tipo,
                                                               ed_tiempo = e.cIncidenciaJust.ed_tiempo,
                                                               requiere_accper = e.cIncidenciaJust.requiere_accper,
                                                               marca_web = e.cIncidenciaJust.marca_web,
                                                           },


                                         }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIncidenciaPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIncidencia;
        }

        /// <summary>
        /// GetMarcaIncidenciaPeriodo: Obtener las Marcas Incidencias para un tipo planilla y un rango de fechas especifico
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idnumero"></param>
        /// <returns>Lista de incidencias del periodo</returns>
        public async Task<List<cMarcaIncidencia>> GetMarcaIncidenciaPeriodo(string idplanilla, string fechaInicio, string fechaFinal, string idnumero)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {
                DateTime fechaInicioExt = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaFinExt = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                marcaIncidencia = await (from e in _context.Marcas_Incidencias
                                                    .Include(e => e.cIncidencia)
                                                    .Include(e => e.cIncidenciaJust)
                                        .Where(e => e.FECHA >= fechaInicioExt
                                                && e.FECHA <= fechaFinExt
                                                && e.IDNUMERO == idnumero
                                                && e.IDPLANILLA == idplanilla)
                                        select new cMarcaIncidencia
                                        {
                                            INDICE = e.INDICE,
                                            IDPLANILLA = e.IDPLANILLA,
                                            IDNUMERO = e.IDNUMERO,
                                            FECHA = e.FECHA,
                                            IDINCIDENCIA = e.IDINCIDENCIA,
                                            IDREGISTRO = e.IDREGISTRO,
                                            HENTRA = e.HENTRA,
                                            HSALE = e.HSALE,
                                            EST_P = e.EST_P,
                                            COMENTARIO = e.COMENTARIO,
                                            INCIDENCIA_JUST = e.INCIDENCIA_JUST,
                                            ESTADO = e.ESTADO,
                                            C_TIEMPO = e.C_TIEMPO,
                                            USUARIO = e.USUARIO,
                                            FECHA_JUST = e.FECHA_JUST,
                                            IDACC = e.IDACC,
                                            cIncidencia = e.cIncidencia == null?null:
                                                          new cIncidencia
                                                          {
                                                              Id = e.cIncidencia.Id,
                                                              Codigo = e.cIncidencia.Codigo,
                                                              Descripcion = e.cIncidencia.Descripcion,
                                                              id_pago = e.cIncidencia.id_pago,
                                                              nom_conector = e.cIncidencia.nom_conector,
                                                              tipo = e.cIncidencia.tipo,
                                                              ed_tiempo = e.cIncidencia.ed_tiempo,
                                                              requiere_accper = e.cIncidencia.requiere_accper,
                                                              marca_web = e.cIncidencia.marca_web,
                                                          },
                                            cIncidenciaJust = e.cIncidenciaJust == null ? null :
                                                          new cIncidencia
                                                          {
                                                              Id = e.cIncidenciaJust.Id,
                                                              Codigo = e.cIncidenciaJust.Codigo,
                                                              Descripcion = e.cIncidenciaJust.Descripcion,
                                                              id_pago = e.cIncidenciaJust.id_pago,
                                                              nom_conector = e.cIncidenciaJust.nom_conector,
                                                              tipo = e.cIncidenciaJust.tipo,
                                                              ed_tiempo = e.cIncidenciaJust.ed_tiempo,
                                                              requiere_accper = e.cIncidenciaJust.requiere_accper,
                                                              marca_web = e.cIncidenciaJust.marca_web,
                                                          },


                                        }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIncidenciaPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIncidencia;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-28
        /// <summary>
        /// GetMarcaIncidenciaProgramador: Obtener las Marcas Incidencias para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Incidencias</returns>
        public async Task<List<cMarcaIncidencia>> GetMarcaIncidenciaProgramador(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");

                marcaIncidencia = await _context.Marcas_Incidencias.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA == fechaMov
                                                        && e.IDPLANILLA == idplanilla
                                                        && e.ESTADO == 'A').ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaIncidencia: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaIncidencia;
        }

        /// <summary>
        /// Sincronizar_MarcasIncidencias: Método para agregar o actualizar marcas incidencias
        /// </summary>
        /// <param name="indice"></param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>

        public async Task<EventResponse> Sincronizar_MarcasIncidencias(IEnumerable<cMarcaIncidencia> marcasIncidencias)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in marcasIncidencias)
                {
                    cMarcaIncidencia? marcaIncidencia = await _context.Marcas_Incidencias
                                    .Where(e => e.INDICE == item.INDICE)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (marcaIncidencia is not null)
                    {
                        marcaIncidencia.COMENTARIO = item.COMENTARIO;
                        if (marcaIncidencia.IDINCIDENCIA != item.IDINCIDENCIA)
                        {
                            if (marcaIncidencia.IDINCIDENCIA <= 7)
                                marcaIncidencia.INCIDENCIA_JUST = marcaIncidencia.IDINCIDENCIA;
                            marcaIncidencia.FECHA_JUST = DateTime.Now;
                        }                       
                        marcaIncidencia.IDINCIDENCIA = item.IDINCIDENCIA;

                        _context.Marcas_Incidencias.Update(marcaIncidencia);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_MarcasIncidencias: Método para eliminar marcas incidencias
        /// </summary>
        /// <param name="indice"></param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public async Task<EventResponse> Elimina_MarcasIncidencias(string indice)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cMarcaIncidencia? model = await _context.Marcas_Incidencias
                    .FirstOrDefaultAsync(e => e.INDICE == long.Parse(indice));

                if (model is not null)
                {
                    _context.Marcas_Incidencias.Remove(model);
                    await _context.SaveChangesAsync();
                }
            }

            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el registro de Marcas Incidencias. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el registro de Marcas Incidencias. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }


        /// <summary>
        /// GetMarcaDistribucion: Lista de Marcas Distribución segun parametros de fecha indicados
        /// </summary>
        /// <param name="FechaInicio">Fecha de Inicio del reporte</param>
        /// <param name="FechaFin">Fecha final del reporte</param>
        /// <returns>lista de registros de la clase cMarcaDistribucion</returns>
        public async Task<List<cMarcaDistribucion>> GetMarcaDistribucion(DateTime FechaInicio, DateTime FechaFin)
        {
            List<cMarcaDistribucion> marcasDistribuciones = new();
            try
            {
                marcasDistribuciones = await (from md in _context.Marcas_Distribuciones
                                              .Where(e => FechaInicio <= e.FECHA && FechaFin >= e.FECHA)
                                              select new cMarcaDistribucion
                                              {
                                                  IDPLANILLA = md.IDPLANILLA,
                                                  IDNUMERO = md.IDNUMERO,
                                                  FECHA = md.FECHA,
                                                  NOMINAEQ = md.NOMINAEQ,
                                                  CANTIDAD = md.CANTIDAD,
                                                  IDCCOSTO = md.IDCCOSTO,
                                              }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaDistribucion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcasDistribuciones;
        }

        /// <summary>
        /// GetMarcaDistribucion: Obtener las Marcas distribuciones para un tipo planilla, un empleado y un rango de fechas especifico
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idnumero"></param>
        /// <returns>Lista de marcas distribuciones</returns>
        public async Task<List<cMarcaDistribucion>> GetMarcaDistribucion(string idplanilla, string fechaInicio, string fechaFinal, string idnumero, string hora)
        {
            List<cMarcaDistribucion>? marcaDistribucion = new();
            try
            {
                DateTime fechaInicioExt = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaFinExt = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                marcaDistribucion = await (from e in _context.Marcas_Distribuciones
                                                    .Include(e => e.cConcepto)
                                        .Where(e => e.FECHA >= fechaInicioExt
                                                && e.FECHA <= fechaFinExt
                                                && e.IDNUMERO == idnumero
                                                && e.IDPLANILLA == idplanilla
                                                && e.ENTRADA == hora)
                                         select new cMarcaDistribucion
                                         {
                                             IDREGISTRO = e.IDREGISTRO,
                                             IDPLANILLA = e.IDPLANILLA,
                                             IDNUMERO = e.IDNUMERO,
                                             FECHA = e.FECHA,
                                             IDCONCEPTO = e.IDCONCEPTO,
                                             NOMINAEQ = e.NOMINAEQ,
                                             CANTIDAD = e.CANTIDAD,
                                             IDCCOSTO = e.IDCCOSTO,
                                             PROYECTO = e.PROYECTO,
                                             FASE = e.FASE,
                                             CONCEPTO = e.CONCEPTO,
                                             TIPO = e.TIPO,
                                             ESTADO = e.ESTADO,
                                             ENTRADA = e.ENTRADA,
                                             cConcepto = e.cConcepto == null ? null :
                                                           new cConcepto
                                                           {
                                                               id = e.cConcepto.id,
                                                               Concepto = e.cConcepto.Concepto,
                                                               Descripcion = e.cConcepto.Descripcion,
                                                               tipo_j = e.cConcepto.tipo_j,
                                                               tipo_h = e.cConcepto.tipo_h,
                                                               columnar = e.cConcepto.columnar,
                                                               nominaeq = e.cConcepto.nominaeq,
                                                               factor = e.cConcepto.factor,
                                                               tolerancia = e.cConcepto.tolerancia,
                                                               ordinario = e.cConcepto.ordinario,
                                                               autorizado = e.cConcepto.autorizado,
                                                               transferir = e.cConcepto.transferir,
                                                               adicional = e.cConcepto.adicional,
                                                               tipo_ext_alm = e.cConcepto.tipo_ext_alm,
                                                               muestra_resumen = e.cConcepto.muestra_resumen,
                                                           },

                                         }).ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaDistribucion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaDistribucion;
        }

       

        /// <summary>
        /// GetPhUsuarioById: Obtener datos de usuario por su ID 
        /// </summary>
        /// <param name="id">id numero del empleado</param>
        /// <returns>Instancia de phusuario con los datos del usuario </returns>
        public async Task<cPh_Usuario> GetPhUsuarioById(int id)
        {
            cPh_Usuario? phUsuario = new();

            try
            {
                phUsuario = await _context.Ph_Usuarios.FirstOrDefaultAsync(e => e.IDUSUARIO == id);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhUsuarioById: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phUsuario;
        }


        /// <summary>
        /// GetPhUsuario: Obtener datos de usuario 
        /// </summary>
        /// <param name="idnumero">id numero del empleado</param>
        /// <returns>Instancia de phusuario con los datos del usuario </returns>
        public async Task<cPh_Usuario> GetPhUsuario(string idnumero)
        {
            cPh_Usuario? phUsuario = new();

            try
            {
                phUsuario = await (from e in _context.Empleados.Where(e => e.IdNumero == idnumero)
                                   join l in _context.PH_LOGIN on e.Email equals l.EMAIL
                                   join u in _context.Ph_Usuarios on l.idusuario equals u.IDUSUARIO
                                   select u).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phUsuario;
        }

        /// <summary>
        /// PostPhUsuario: crear o actualizar registro de phUsuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<EventResponse> PostPhUsuario(cPh_Usuario usuario)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_Usuario? usuarioBuscar = await _context.Ph_Usuarios.FirstOrDefaultAsync(e => e.IDUSUARIO == usuario.IDUSUARIO);

                if (usuarioBuscar is not null)
                {
                    usuarioBuscar.PLANILLAS = usuario.PLANILLAS;
                    usuarioBuscar.NIVEL = usuario.NIVEL;
                    usuarioBuscar.GRUPOS = usuario.GRUPOS;
                    usuarioBuscar.ESTADO = usuario.ESTADO;
                    usuarioBuscar.NIVEL_APROB_EXT = usuario.NIVEL_APROB_EXT;
                    usuarioBuscar.ORDEN_EMP = usuario.ORDEN_EMP;
                    usuarioBuscar.FILT_PRGT = usuario.FILT_PRGT;
                    usuarioBuscar.TIPO_EDT = usuario.TIPO_EDT;
                    usuarioBuscar.PT_AGRUP = usuario.PT_AGRUP;

                    _context.Ph_Usuarios.Update(usuarioBuscar);
                }
                else
                {
                    _context.Ph_Usuarios.Add(usuario);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización del usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización del usuario. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// PutPhUsuario: utilizado para actualizar variables globales del usuario para los filtros
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<EventResponse> PutPhUsuario(cPh_Usuario usuario)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_Usuario? usuarioBuscar = await _context.Ph_Usuarios.FirstOrDefaultAsync(e => e.IDUSUARIO == usuario.IDUSUARIO);

                if (usuarioBuscar is not null)
                {
                    usuarioBuscar.FPERIODO = usuario.FPERIODO;
                    usuarioBuscar.FPLANILLA = usuario.FPLANILLA;
                    usuarioBuscar.FFECHA_EVALUAR = usuario.FFECHA_EVALUAR;
                    usuarioBuscar.FCANT_MESES = usuario.FCANT_MESES;

                    _context.Ph_Usuarios.Update(usuarioBuscar);
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización del usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización del usuario. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// PutPhUsuario: utilizado para actualizar variables de PhUsuario 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<EventResponse> PutActualizarPhUsuario(cPh_Usuario usuario)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_Usuario? usuarioBuscar = await _context.Ph_Usuarios.FirstOrDefaultAsync(e => e.IDUSUARIO == usuario.IDUSUARIO);

                if (usuarioBuscar is not null)
                {
                    usuarioBuscar.PLANILLAS = usuario.PLANILLAS;
                    usuarioBuscar.NIVEL = usuario.NIVEL;
                    usuarioBuscar.GRUPOS = usuario.GRUPOS;
                    usuarioBuscar.ESTADO = usuario.ESTADO;
                    usuarioBuscar.NIVEL_APROB_EXT = usuario.NIVEL_APROB_EXT;
                    usuarioBuscar.ORDEN_EMP = usuario.ORDEN_EMP;
                    usuarioBuscar.FILT_PRGT = usuario.FILT_PRGT;
                    usuarioBuscar.TIPO_EDT = usuario.TIPO_EDT;
                    usuarioBuscar.PT_AGRUP = usuario.PT_AGRUP;

                    _context.Ph_Usuarios.Update(usuarioBuscar);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización del usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización del usuario. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// GetPhSistema: Obtener datos de Sistema 
        /// </summary>
        /// <returns>Instancia de cPh_Sistema con los datos del sistema </returns>
        public async Task<cPh_Sistema> GetPhSistema()
        {
            cPh_Sistema? phSistema = new();

            try
            {
                phSistema = await _context.Ph_Sistema.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phSistema;
        }

        /// <summary>
        /// GetPortalConfig: Obtener datos de Configuración del Portal 
        /// </summary>
        /// <returns>Instancia de cPortal_Config con los datos del sistema </returns>
        public async Task<cPortal_Config> GetPortalConfig()
        {
            cPortal_Config? portalConfig = new();

            try
            {
                portalConfig = (from e in await _context.Portal_Config.Where(e => e.IDAPLICACION == "com.gsitcr.portalmarcasweb").ToListAsync()
                                select new cPortal_Config
                                {
                                    IDAPLICACION = e.IDAPLICACION,
                                    IDVERSION = e.IDVERSION,
                                    COMPANIA = e.COMPANIA,
                                    BASEDATOS = e.BASEDATOS,
                                    IDLICENCIA = Encripta.getDecryptTripleDES(e.IDLICENCIA),
                                    ACTIVA = e.ACTIVA,
                                    USORESTRINGIDO = e.USORESTRINGIDO,
                                    REGSITROLIC = Encripta.getDecryptTripleDES(e.REGSITROLIC),
                                    PERMANENTE = e.PERMANENTE,
                                    USARECONOCIMIENTOFACIAL = e.USARECONOCIMIENTOFACIAL,
                                    USARGEOLOCALIZACION = e.USARGEOLOCALIZACION,
                                    FECHAULTMODIFICA = e.FECHAULTMODIFICA,
                                    IDUSUARIOMODIFICA = e.IDUSUARIOMODIFICA,
                                    MAPAPIKEY = e.MAPAPIKEY != null ? Encripta.getDecryptTripleDES(e.MAPAPIKEY!) : null,
                                    FACEDIST = e.FACEDIST,
                                    FACETEXT = e.FACETEXT,
                                    VERLOGMARCAS = e.VERLOGMARCAS,
                                    ORGANIZACIONBASEID = "00",
                                    AUTOREGISTROROSTRO = e.AUTOREGISTROROSTRO,
                                    CANTMAXPLANTILLAS = e.CANTMAXPLANTILLAS,
                                }).FirstOrDefault();

                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();
                if (opciones is not null && portalConfig is not null)
                    portalConfig!.ORGANIZACIONBASEID = opciones.ORG_BASE!??"03";
                

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalConfig: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalConfig!;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// Sincronizar_PortalConfig: Sincronizar las configuraciones del Portal de Empleados, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="portalConfig">Recibe una instancia del tipo cPortal_Config</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>
        public async Task<EventResponse> Sincronizar_PortalConfig(cPortal_Config portalConfig)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPortal_Config? objetoBuscar = await _context.Portal_Config.FirstOrDefaultAsync(e => e.IDAPLICACION == portalConfig.IDAPLICACION);

                //si el proyectoFase existe se actualiza 
                //de lo contrario se agrega el registro
                if (objetoBuscar is not null)
                {
                    objetoBuscar.IDVERSION = portalConfig.IDVERSION;
                    objetoBuscar.COMPANIA = portalConfig.COMPANIA;
                    objetoBuscar.BASEDATOS = portalConfig.BASEDATOS;
                    objetoBuscar.IDLICENCIA = Encripta.getEncryptTripleDES(portalConfig.IDLICENCIA);  
                    objetoBuscar.ACTIVA = portalConfig.ACTIVA;
                    objetoBuscar.USORESTRINGIDO = portalConfig.USORESTRINGIDO;
                    objetoBuscar.REGSITROLIC = Encripta.getEncryptTripleDES(portalConfig.REGSITROLIC);
                    objetoBuscar.PERMANENTE = portalConfig.PERMANENTE;
                    objetoBuscar.USARECONOCIMIENTOFACIAL = portalConfig.USARECONOCIMIENTOFACIAL;
                    objetoBuscar.FECHAULTMODIFICA = portalConfig.FECHAULTMODIFICA;
                    objetoBuscar.IDUSUARIOMODIFICA = portalConfig.IDUSUARIOMODIFICA;
                    objetoBuscar.USARGEOLOCALIZACION = portalConfig.USARGEOLOCALIZACION;
                    objetoBuscar.MAPAPIKEY = portalConfig.MAPAPIKEY!=null?Encripta.getEncryptTripleDES(portalConfig.MAPAPIKEY!):null;
                    objetoBuscar.FACEDIST = portalConfig.FACEDIST;
                    objetoBuscar.FACETEXT = portalConfig.FACETEXT;
                    objetoBuscar.VERLOGMARCAS = portalConfig.VERLOGMARCAS;
                    objetoBuscar.AUTOREGISTROROSTRO = portalConfig.AUTOREGISTROROSTRO;
                    objetoBuscar.CANTMAXPLANTILLAS = portalConfig.CANTMAXPLANTILLAS;

                    _context.Portal_Config.Update(objetoBuscar);
                }
                else
                {
                    _context.Add(portalConfig);
                }

                var opciones = await _context.Ph_Opciones.FirstOrDefaultAsync();
                if (opciones is not null)
                {
                    if (String.IsNullOrEmpty(opciones.ORG_BASE) || opciones.ORG_BASE=="00")
                    {
                        opciones.ORG_BASE = portalConfig.ORGANIZACIONBASEID;
                        _context.Ph_Opciones.Update(opciones);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las configuraciones del Portal de Empleados. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las configuraciones del Portal de Empleados. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetPortalOpcion: Obtener lista de opciones del menu de Portal 
        /// </summary>
        /// <returns>Lista de Opciones del sistema</returns>
        public async Task<List<cPortal_Opcion>> GetPortalOpcion()
        {
            List<cPortal_Opcion>? portalOpcion = new();

            try
            {
                portalOpcion = (from e in await _context.Portal_Opciones
                                .Include(e => e.cPortal_Menu)
                                .ToListAsync()
                                select new cPortal_Opcion
                                {
                                    PARENTID = e.PARENTID,
                                    ID = e.ID,
                                    MENUTEXT = e.MENUTEXT,
                                    ICONID = e.ICONID,
                                    PRINCIPAL = e.PRINCIPAL,
                                    HREF = e.HREF,
                                    cPortal_Menu = e.cPortal_Menu == null ? null :
                                                    new cPortal_Menu
                                                    {
                                                        ID = e.cPortal_Menu.ID,
                                                        MENUTEXT = e.cPortal_Menu.MENUTEXT,
                                                        ICONID = e.cPortal_Menu.ICONID,
                                                    },
                                }
                           ).ToList();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalOpcion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalOpcion;
        }

        /// <summary>
        /// GetPortalOpcion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public async Task<cPortal_Opcion> GetPortalOpcion(string id)
        {
            cPortal_Opcion? portalOpcion = new();

            try
            {

                portalOpcion = (from e in await _context.Portal_Opciones.Where(e => e.ID == id)
                                .Include(e => e.cPortal_Menu)
                                .ToListAsync()
                                select new cPortal_Opcion
                                {
                                    PARENTID = e.PARENTID,
                                    ID = e.ID,
                                    MENUTEXT = e.MENUTEXT,
                                    ICONID = e.ICONID,
                                    PRINCIPAL = e.PRINCIPAL,
                                    HREF = e.HREF,
                                    cPortal_Menu = e.cPortal_Menu == null ? null :
                                                    new cPortal_Menu
                                                    {
                                                        ID = e.cPortal_Menu.ID,
                                                        MENUTEXT = e.cPortal_Menu.MENUTEXT,
                                                        ICONID = e.cPortal_Menu.ICONID,
                                                    },
                                }
                           ).FirstOrDefault();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalOpcion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalOpcion;
        }


        /// <summary>
        /// Sincronizar_PortalOpcion: Método para registrar las opciones del sistema Portal de empleados
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Opcion </param>
        public async Task<EventResponse> Sincronizar_PortalOpcion(IEnumerable<cPortal_Opcion> portalOpcion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in portalOpcion)
                {
                    cPortal_Opcion? portalOpcionBuscar = await _context.Portal_Opciones
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalOpcionBuscar is not null)
                    {
                        portalOpcionBuscar.HREF = item.HREF;
                        portalOpcionBuscar.ICONID = item.ICONID;
                        portalOpcionBuscar.PRINCIPAL = item.PRINCIPAL;
                        portalOpcionBuscar.MENUTEXT = item.MENUTEXT;
                        portalOpcionBuscar.PARENTID = item.PARENTID;

                        _context.Portal_Opciones.Update(portalOpcionBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la opción de menú. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la opción de menú. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-10-19
        /// <summary>
        /// GetPhFormulacion: Obtener lista de registros de la tabla PH_FROMULACION
        /// </summary>
        /// <returns>Lista de cPh_Formulacion </returns>
        /// 

        public async Task<List<cPh_Formulacion>> GetPhFormulacion()
        {
            List<cPh_Formulacion>? phFormulacion = new();

            try
            {
                phFormulacion = await _context.Ph_Formulacion.ToListAsync();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhFormulacion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phFormulacion;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-10-19
        /// <summary>
        /// GetPhFormulacion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public async Task<cPh_Formulacion> GetPhFormulacion(int id)
        {
            cPh_Formulacion? phFormulacion = new();

            try
            {
                phFormulacion = await _context.Ph_Formulacion.FirstOrDefaultAsync(e => e.ID == id);

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhFormulacion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phFormulacion;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-10-19
        /// <summary>
        /// Sincronizar_PhFormulacion: Método para registrar los registros en la tabla Ph_Formulacion
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="ph_Formulacion">Lista de registros de la clase cPh_Formulacion</param>
        public async Task<EventResponse> Sincronizar_PhFormulacion(IEnumerable<cPh_Formulacion> ph_Formulacion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in ph_Formulacion)
                {
                    cPh_Formulacion? objetoBuscar = await _context.Ph_Formulacion
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.FORMULA = item.FORMULA;

                        _context.Ph_Formulacion.Update(objetoBuscar);
                    }
                    else
                    {
                        item.ID = 0;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla PH_FORMULACION. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla PH_FORMULACION. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// Elimina_PhFormulacion:  Metodo boorado de datos de la tabla Ph_Formulacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhFormulacion(string id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cPh_Formulacion? model = await _context.Ph_Formulacion
                    .FirstOrDefaultAsync(e => e.ID == int.Parse(id));

                if (model is not null)
                {
                    _context.Ph_Formulacion.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la fórmula. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la fórmula. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }
        //Creado por: Marlon Loria Solano
        //Fecha: 2022-11-10
        //Obtener Parametros Email por Id
        public async Task<cParametroEmail> GetParametroEmail(int id)
        {
            cParametroEmail? parametroEmail = new();
            try
            {
                parametroEmail = await _context.ParametrosEmail.FirstOrDefaultAsync(e => e.Id == id);

                parametroEmail!.DefaultPassWord = String.IsNullOrEmpty(parametroEmail!.DefaultPassWord) ? "" : Encripta.getDecryptTripleDES(parametroEmail.DefaultPassWord);
                parametroEmail.ClientSecret = String.IsNullOrEmpty(parametroEmail!.ClientSecret) ? "" : Encripta.getDecryptTripleDES(parametroEmail.ClientSecret);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetParametroEmail: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
            }
            return parametroEmail!;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-02-13
        //Sincronizar ParametroEmail
        //Parametro: Recibe una instancia de ParametroEmail, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_ParametroEmail(cParametroEmail parametroEmail)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cParametroEmail? parametroBuscado = await _context.ParametrosEmail
                                .Where(e => e.Id == parametroEmail.Id)
                                .FirstOrDefaultAsync();
                //si el parametro existe se actualiza 
                //de lo contrario se agrega el registro
                if (parametroBuscado is not null)
                {
                    parametroBuscado.SmtpServer = parametroEmail.SmtpServer;
                    parametroBuscado.SmtpPort = parametroEmail.SmtpPort;
                    parametroBuscado.DefaultEmail = parametroEmail.DefaultEmail;
                    parametroBuscado.DefaultPassWord = String.IsNullOrEmpty(parametroEmail.DefaultPassWord!) ? "" : Encripta.getEncryptTripleDES(parametroEmail.DefaultPassWord!);
                    parametroBuscado.UserName = parametroEmail.UserName;
                    parametroBuscado.TipoServicio = parametroEmail.TipoServicio;
                    parametroBuscado.TenantId = parametroEmail.TenantId;
                    parametroBuscado.ClientId = parametroEmail.ClientId;
                    parametroBuscado.ClientSecret = String.IsNullOrEmpty(parametroEmail.ClientSecret) ? "" : Encripta.getEncryptTripleDES(parametroEmail.ClientSecret!);

                    _context.ParametrosEmail.Update(parametroBuscado);
                }
                else
                {
                    _context.Add(parametroEmail);
                }


                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Parámetro Email. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Parámetro Email. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        public async Task<EventResponse> EnviarCorreo(Email correo)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var parametrosCorreo = await GetParametroEmail(1);

                correo.De = parametrosCorreo.DefaultEmail;
                correo.SmtpServer = parametrosCorreo.SmtpServer;
                correo.SmtpPort = parametrosCorreo.SmtpPort;

                string password = parametrosCorreo.DefaultPassWord;

                SecureString secureString = new SecureString();
                foreach (char c in password.ToCharArray())
                {
                    secureString.AppendChar(c);
                }

                MailMessage message = new MailMessage(correo.De, correo.Para, correo.Asunto, correo.Cuerpo);
                message.IsBodyHtml = true;

                if (correo.Adjunto != "")
                {
                    Stream stream = new MemoryStream(correo.StreamAdjunto);
                    Attachment data = new Attachment(stream, correo.Adjunto, MediaTypeNames.Application.Octet);
                    message.Attachments.Add(data);
                }

                SmtpClient client = new SmtpClient(correo.SmtpServer, correo.SmtpPort);

                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                var Credentials = new NetworkCredential(correo.De, secureString);
                client.Credentials = Credentials;
                client.Send(message);


            }
            catch (System.Net.WebException e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.EnviarCorreo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + e.InnerException.Message;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (ex.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + ex.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + ex.InnerException.Message;

            }
            return respuesta;
        }


        //Creado por: Allan Prieto
        //Fecha: 2023-12-12
        /// <summary>
        /// GetPh_Tranformacion: Obtener lista de registros de la tabla PH_TRANFORMACION
        /// </summary>
        /// <returns>Lista de cPh_Tranformacion </returns>
        /// 
        public async Task<List<cPh_Transformacion>> GetPhTransformacion()
        {
            List<cPh_Transformacion> transformaciones = new();
            try
            {
                transformaciones = await _context.Ph_Transformacion.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhTransformacion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformaciones;
        }



        //Creado por: Allan Prieto  // No se ocupa 
        //Fecha: 2023-12-27
        /// <summary>
        /// GetEmpleado: Método para una lista de rolesTurno
        /// </summary>
        /// <returns>Una instancia de la clase cPh_RolTurno</returns>
        /// ///<param name="idrol">idNumero del empleado requerido</param>
        /// 
        /*
        public async Task<List<cPh_RolTurno>> GetRolTurno(int idrol)
        {
            List<cPh_RolTurno> rolturno = new();
            try
            {
                rolturno = (from e in await _context.Ph_Roles_Turnos
                                //.Include(e => e.Turno)
                                .Where(e => e.IDROL == idrol).ToListAsync()
                            select new cPh_RolTurno
                            {
                                IDREGISTRO = e.IDREGISTRO,
                                IDROL = e.IDROL,
                                IDTURNO = e.IDTURNO,
                                
                                Turno = e.Turno == null ? null :
                                               new cTurno
                                               {
                                                   IdTurno = e.Turno.IdTurno,
                                                   Descripcion = e.Turno.Descripcion,
                                               },

                            }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"GeoTimeConnectService.GetTipo_Planilla: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return rolturno;
        }
            */

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        //Sincronizar RolTurno
        //Parametro: Recibe una instancia de RolTurno, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_RolTurno(IEnumerable<cPh_RolTurno> roles_Turno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var rlTurno in roles_Turno)
                {
                    cPh_RolTurno? rturno = await _context.Ph_Roles_Turnos
                                                        .FirstOrDefaultAsync(e => e.IDREGISTRO == rlTurno.IDREGISTRO && e.IDROL == rlTurno.IDROL);
                    //si existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (rturno is not null)
                    {
                        rturno.IDTURNO = rlTurno.IDTURNO;

                        _context.Ph_Roles_Turnos.Update(rturno);
                    }
                    else
                    {
                        rlTurno.IDREGISTRO = 0;
                        _context.Add(rlTurno);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de RolTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de RolTurno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }

        /// <summary>
        /// Elimina_RolTurn:  Metodo borrado de datos de la tabla RolesTurnos
        /// </summary>
        /// <param name="idregistro"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_RolTurno(int idregistro)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_RolTurno? model = await _context.Ph_Roles_Turnos
                    .FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);

                if (model is not null)
                {
                    _context.Ph_Roles_Turnos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el registro de Empleado Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el registro de Empleado Turno. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-1-15
        /// <summary>
        /// GetTransformacion: Método para obtener una lista de Transformaciones 
        /// </summary>
        /// <returns>Lista de cTransformacion</returns>
        public async Task<IEnumerable<cTransformacion>> GetTransformacion()
        {
            List<cTransformacion>? transformaciones = new();
            try
            {
                transformaciones = await _context.Transformaciones.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformaciones;
        }

        //Creado por: Allan Prieto
        //Fecha: 2024-1-15
        /// <summary>
        /// GetTransformacion: Método para un periodo específico
        /// </summary>
        /// <returns>Una instancia de la clase cTransformacion</returns>
        /// ///<param name="id">idperiodo de la Transformacion requerido</param>
        public async Task<cTransformacion> GetTransformacion(int id)
        {
            cTransformacion? transformacion = new();
            try
            {
                transformacion = await _context.Transformaciones.FirstOrDefaultAsync(e => e.ID == id);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacion: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformacion;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2023-10-19
        /// <summary>
        /// Sincronizar_Transformacion: Método para registrar los registros en la tabla Transformacion
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="transformacion">Lista de registros de la clase cTransformacion</param>
        public async Task<EventResponse> Sincronizar_Transformacion(IEnumerable<cTransformacion> transformacion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in transformacion)
                {
                    cTransformacion? objetoBuscar = await _context.Transformaciones
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.ID = item.ID;
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.IDCONCEPTO_1 = item.IDCONCEPTO_1;
                        objetoBuscar.IDCONCEPTO_2 = item.IDCONCEPTO_2;
                        objetoBuscar.IDCONCEPTO_3 = item.IDCONCEPTO_3;
                        objetoBuscar.IDCONCEPTO_4 = item.IDCONCEPTO_4;
                        objetoBuscar.IDCONCEPTO_5 = item.IDCONCEPTO_5;
                        objetoBuscar.IDCONCEPTO_6 = item.IDCONCEPTO_6;
                        objetoBuscar.IDCONCEPTO_7 = item.IDCONCEPTO_7;
                        objetoBuscar.CALCULADAS = item.CALCULADAS;
                        objetoBuscar.HRS_CONCEPTO_1 = item.HRS_CONCEPTO_1;
                        objetoBuscar.HRS_CONCEPTO_2 = item.HRS_CONCEPTO_2;
                        objetoBuscar.HRS_CONCEPTO_3 = item.HRS_CONCEPTO_3;
                        objetoBuscar.HRS_CONCEPTO_4 = item.HRS_CONCEPTO_4;
                        objetoBuscar.HRS_CONCEPTO_5 = item.HRS_CONCEPTO_5;
                        objetoBuscar.HRS_CONCEPTO_6 = item.HRS_CONCEPTO_6;
                        objetoBuscar.HRS_CONCEPTO_7 = item.HRS_CONCEPTO_7;
                        objetoBuscar.MIN_CONCEPTO_1 = item.MIN_CONCEPTO_1;
                        objetoBuscar.MIN_CONCEPTO_2 = item.MIN_CONCEPTO_2;
                        objetoBuscar.MIN_CONCEPTO_3 = item.MIN_CONCEPTO_3;
                        objetoBuscar.MIN_CONCEPTO_4 = item.MIN_CONCEPTO_4;
                        objetoBuscar.MIN_CONCEPTO_5 = item.MIN_CONCEPTO_5;
                        objetoBuscar.MIN_CONCEPTO_6 = item.MIN_CONCEPTO_6;
                        objetoBuscar.MIN_CONCEPTO_7 = item.MIN_CONCEPTO_7;

                        _context.Transformaciones.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// Elimina_Transformacion:  Metodo boorado de datos de la tabla cTransformacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Transformacion(int id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cTransformacion? model = await _context.Transformaciones
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (model is not null)
                {
                    _context.Transformaciones.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Transformación. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Transformación. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }


        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-1-23
        /// <summary>
        /// GetTransformacion: Método para obtener una lista de Transformaciones Globales 
        /// </summary>
        /// <returns>Lista de cTransformacion</returns>
        public async Task<IEnumerable<cTransformacionGlobal>> GetTransformacionGlobal()
        {
            List<cTransformacionGlobal>? transformacionesGlobales = new();
            try
            {
                transformacionesGlobales = await _context.TransformacionesGlobales.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacionGlobal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformacionesGlobales;
        }

        //Creado por: Allan Prieto
        //Fecha: 2024-1-23
        /// <summary>
        /// GetTransformacionGlobal: Método para una Transformacion Global específica
        /// </summary>
        /// <returns>Una instancia de la clase cTransformacionGlobal</returns>
        /// ///<param name="id">idperiodo de la Transformacion Global requerido</param>
        public async Task<cTransformacionGlobal> GetTransformacionGlobal(int id)
        {
            cTransformacionGlobal? transformacionesGlobales = new();
            try
            {
                transformacionesGlobales = await _context.TransformacionesGlobales.FirstOrDefaultAsync(e => e.ID == id);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacionGlobal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformacionesGlobales;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-1-23
        /// <summary>
        /// Sincronizar_TransformacionGlobal: Método para registrar los registros en la tabla Transformaciones Globales
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="transformacion">Lista de registros de la clase cTransformacionGlobal</param>
        public async Task<EventResponse> Sincronizar_TransformacionGlobal(IEnumerable<cTransformacionGlobal> transformacion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in transformacion)
                {
                    cTransformacionGlobal? objetoBuscar = await _context.TransformacionesGlobales
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.ID = item.ID;
                        objetoBuscar.ID_ORDEN = item.ID_ORDEN;
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.IDPLANILLA = item.IDPLANILLA;
                        objetoBuscar.ESTADO = item.ESTADO;
                        objetoBuscar.FORMULA_APL = item.FORMULA_APL;
                        objetoBuscar.FORMULA_HRS = item.FORMULA_HRS;
                        objetoBuscar.FORMULA_CONCEPTO = item.FORMULA_CONCEPTO;

                        _context.TransformacionesGlobales.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion Global. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion Global. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// Elimina_Transformacion:  Metodo boorado de datos de la tabla cTransformacion Global
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_TransformacionGlobal(int id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cTransformacionGlobal? model = await _context.TransformacionesGlobales
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (model is not null)
                {
                    _context.TransformacionesGlobales.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Transformación Global. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Transformación Global. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-2-23
        /// <summary>
        /// GetTransformacionTipoMarca: Método para obtener una lista de Transformaciones Tipo Marca 
        /// </summary>
        /// <returns>Lista de cTransformacionTipoMarca</returns>
        public async Task<IEnumerable<cTransformacionTipoMarca>> GetTransformacionTipoMarca()
        {
            List<cTransformacionTipoMarca>? transformacionesTipoMarca = new();
            try
            {
                transformacionesTipoMarca = await _context.TransformacionesTipoMarca.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacionGlobal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformacionesTipoMarca;
        }

        //Creado por: Allan Prieto
        //Fecha: 2025-2-23
        /// <summary>
        /// GetTransformacionTipoMarca: Método para una Transformacion Tipo Marca específica
        /// </summary>
        /// <returns>Una instancia de la clase cTransformacionTipoMarca</returns>
        /// ///<param name="id">idperiodo de la Transformacion Global requerido</param>
        public async Task<cTransformacionTipoMarca> GetTransformacionTipoMarca(int id)
        {
            cTransformacionTipoMarca? transformacionesTipoMarcas = new();
            try
            {
                transformacionesTipoMarcas = await _context.TransformacionesTipoMarca.FirstOrDefaultAsync(e => e.TRANSFORMACIONID == id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacionGlobal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformacionesTipoMarcas;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-2-23
        /// <summary>
        /// Sincronizar_TransformacionTipoMarca: Método para registrar los registros en la tabla Transformaciones Tipo Marca
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="transformacionTM">Lista de registros de la clase cTransformacionTipoMarca</param>
        public async Task<EventResponse> Sincronizar_TransformacionTipoMarca(IEnumerable<cTransformacionTipoMarca> transformacionTM)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in transformacionTM)
                {
                    cTransformacionTipoMarca? objetoBuscar = await _context.TransformacionesTipoMarca
                                    .Where(e => e.TRANSFORMACIONID == item.TRANSFORMACIONID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.TRANSFORMACIONID = item.TRANSFORMACIONID;
                        objetoBuscar.HORA_INICIO = item.HORA_INICIO;
                        objetoBuscar.HORA_FIN = item.HORA_FIN;
                        objetoBuscar.RESTARDIA = item.RESTARDIA;

                        _context.TransformacionesTipoMarca.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion Tipo Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion Tipo Marca. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-2-23
        /// <summary>
        /// Elimina_TransformacionTipoMarca:  Metodo boorado de datos de la tabla cTransformacion Tipo Marca y 
        /// Los registros de detalle que contiene la tabla de TransformacionesTipoMarcaDetalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_TransformacionTipoMarca(int id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                cTransformacionTipoMarca? model = await _context.TransformacionesTipoMarca
                    .FirstOrDefaultAsync(e => e.TRANSFORMACIONID == id);

                if (model is not null)
                {
                    // Buscar y eliminar los detalles relacionados
                    var detalles = await _context.TransformacionesTipoMarcaDet
                        .Where(d => d.TRANSFORMACIONID == id)
                        .ToListAsync();

                    // Eliminar los detalles de la transformación
                    _context.TransformacionesTipoMarcaDet.RemoveRange(detalles);

                    await _context.SaveChangesAsync();

                    // Eliminar la transformación
                    _context.TransformacionesTipoMarca.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Transformación Tipo Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Transformación Tipo Marca. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-3-19
        /// <summary>
        /// Elimina_TransformacionTipoMarcaDet:  Metodo borado de datos de la tabla cTransformacionTipoMarcaDet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nivel"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_TransformacionTipoMarcaDet(string id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                var data = id.Split("|");
                int nivel = Convert.ToInt32(data[0]); // Nivel de eliminación: 0 para TRANSFORMACIONID, 
                int idregistro = Convert.ToInt32(data[1]); // 1 para IDREGISTRO

                if (nivel == 1)
                {
                    // Buscar el registro por IDREGISTRO
                    var registro = await _context.TransformacionesTipoMarcaDet
                        .FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);

                    if (registro is not null)
                    {
                        // Obtener el TRANSFORMACIONID del registro encontrado
                        int transformacionId = registro.TRANSFORMACIONID;

                        // Buscar todos los registros con el mismo TRANSFORMACIONID
                        var detalles = await _context.TransformacionesTipoMarcaDet
                            .Where(d => d.TRANSFORMACIONID == transformacionId)
                            .ToListAsync();

                        if (detalles.Any())
                        {
                            // Eliminar todos los registros asociados al TRANSFORMACIONID
                            _context.TransformacionesTipoMarcaDet.RemoveRange(detalles);
                            await _context.SaveChangesAsync();

                            respuesta.Id = "0";
                            respuesta.Respuesta = "Éxito";
                            respuesta.Descripcion = $"Se eliminaron todos los registros asociados al TRANSFORMACIONID {transformacionId}.";
                        }
                        else
                        {
                            respuesta.Id = "1";
                            respuesta.Respuesta = "Advertencia";
                            respuesta.Descripcion = $"No se encontraron registros asociados al TRANSFORMACIONID {transformacionId}.";
                        }
                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Advertencia";
                        respuesta.Descripcion = "No se encontró el registro con el IDREGISTRO especificado.";
                    }
                }
                else if (nivel == 2)
                {
                    // Eliminar solo el registro con el IDREGISTRO específico
                    var registro = await _context.TransformacionesTipoMarcaDet
                        .FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);

                    if (registro is not null)
                    {
                        _context.TransformacionesTipoMarcaDet.Remove(registro);
                        await _context.SaveChangesAsync();

                        respuesta.Id = "0";
                        respuesta.Respuesta = "Éxito";
                        respuesta.Descripcion = "Se eliminó el registro con el IDREGISTRO especificado.";
                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Advertencia";
                        respuesta.Descripcion = "No se encontró el registro con el IDREGISTRO especificado.";
                    }
                }
                else if (nivel == 3)
                {
                    var registro = await _context.TransformacionesTipoMarcaDet
                        .FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);

                    if (registro is not null)
                    {
                        // Obtener el TRANSFORMACIONID del registro encontrado
                        int transformacionId = registro.TRANSFORMACIONID;
                        string horaInicio = registro.HORA_INICIO!;

                        // Buscar todos los registros con el mismo TRANSFORMACIONID
                        var detalles = await _context.TransformacionesTipoMarcaDet
                            .Where(d => d.TRANSFORMACIONID == transformacionId && d.HORA_INICIO==horaInicio)
                            .ToListAsync();

                        if (detalles.Any())
                        {
                            // Eliminar todos los registros asociados al TRANSFORMACIONID
                            _context.TransformacionesTipoMarcaDet.RemoveRange(detalles);
                            await _context.SaveChangesAsync();

                            respuesta.Id = "0";
                            respuesta.Respuesta = "Éxito";
                            respuesta.Descripcion = $"Se eliminaron todos los registros asociados al TRANSFORMACIONID {transformacionId}.";
                        }
                        else
                        {
                            respuesta.Id = "1";
                            respuesta.Respuesta = "Advertencia";
                            respuesta.Descripcion = $"No se encontraron registros asociados al TRANSFORMACIONID {transformacionId}.";
                        }
                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Advertencia";
                        respuesta.Descripcion = "No se encontró el registro con el IDREGISTRO especificado.";
                    }
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "Nivel de eliminación no válido. Use 1 para eliminar por TRANSFORMACIONID o 2 para eliminar por IDREGISTRO.";
                }

                //cTransformacionTipoMarcaDet? model = await _context.TransformacionesTipoMarcaDet
                //    .FirstOrDefaultAsync(e => e.TRANSFORMACIONID == id);

                //if (model is not null)
                //{
                //    // Buscar y eliminar los detalles relacionados
                //    var detalles = await _context.TransformacionesTipoMarcaDet
                //        .Where(d => d.TRANSFORMACIONID == id)
                //        .ToListAsync();

                //    // Eliminar los detalles de la transformación
                //    _context.TransformacionesTipoMarcaDet.RemoveRange(detalles);

                //    // Eliminar la transformación
                //    _context.TransformacionesTipoMarca.Remove(model);
                //}
                //await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Transformación Tipo Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Transformación Tipo Marca. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-2-23
        /// <summary>
        /// GetTransformacionTipoMarcaDet: Método para obtener una lista de Transformaciones Tipo Marca Detalle 
        /// </summary>
        /// <returns>Lista de cTransformacionTipoMarcaDet</returns>
        public async Task<IEnumerable<cTransformacionTipoMarcaDet>> GetTransformacionTipoMarcaDet(int transformacionID)
        {
            List<cTransformacionTipoMarcaDet>? transformacionesTipoMarcaDet = new();
            try
            {
                transformacionesTipoMarcaDet = await (from e in _context.TransformacionesTipoMarcaDet
                                                      .Where(e => e.TRANSFORMACIONID == transformacionID)
                                                      select new cTransformacionTipoMarcaDet
                                                      {
                                                          TRANSFORMACIONID = e.TRANSFORMACIONID,
                                                          IDREGISTRO = e.IDREGISTRO,
                                                          VERIFICA_MARCA = e.VERIFICA_MARCA,
                                                          HORA_INICIO = e.HORA_INICIO,
                                                          HORA_FIN = e.HORA_FIN,
                                                          TIPO_MARCA = e.TIPO_MARCA,
                                                          TIPO_MARCA_TRANS = e.TIPO_MARCA_TRANS,
                                                          TIPO_DEFAULT = e.TIPO_DEFAULT
                                                      }).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTransformacionTipoMarcaDet: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return transformacionesTipoMarcaDet;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-2-23
        /// <summary>
        /// Sincronizar_TransformacionTipoMarcaDet: Método para registrar los registros en la tabla Transformaciones Tipo Marca Detalle
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="transformacionTMD">Lista de registros de la clase cTransformacionTipoMarcaDet</param>
        public async Task<EventResponse> Sincronizar_TransformacionTipoMarcaDet(IEnumerable<cTransformacionTipoMarcaDet> transformacionTMD)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in transformacionTMD)
                {
                    cTransformacionTipoMarcaDet? objetoBuscar = await _context.TransformacionesTipoMarcaDet
                                    .Where(e => e.IDREGISTRO == item.IDREGISTRO)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.TRANSFORMACIONID = item.TRANSFORMACIONID;
                        objetoBuscar.IDREGISTRO = item.IDREGISTRO;
                        objetoBuscar.VERIFICA_MARCA = item.VERIFICA_MARCA;
                        objetoBuscar.HORA_INICIO = item.HORA_INICIO;
                        objetoBuscar.HORA_FIN = item.HORA_FIN;
                        objetoBuscar.TIPO_MARCA = item.TIPO_MARCA;
                        objetoBuscar.TIPO_MARCA_TRANS = item.TIPO_MARCA_TRANS;
                        objetoBuscar.TIPO_DEFAULT = item.TIPO_DEFAULT;

                        _context.TransformacionesTipoMarcaDet.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion Tipo Marca Detalle. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Transformacion Tipo Marca Detalle. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-2-6
        /// <summary>
        /// GetIncidencia_Conf_Pago: Método para obtener una lista de IncidenciaConfPago 
        /// </summary>
        /// <returns>Lista de cIncidencia_Conf_Pago</returns>
        public async Task<IEnumerable<cIncidencia_Conf_Pago>> GetIncidencia_Conf_Pago()
        {
            List<cIncidencia_Conf_Pago>? IncidenciaConfPagos = new();
            try
            {
                IncidenciaConfPagos = await _context.Incidencias_Conf_Pago.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetIncidencia_Conf_Pago: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return IncidenciaConfPagos;
        }



        //Creado por: Allan Prieto
        //Fecha: 2024-2-6
        /// <summary>
        /// GetIncidencia_Conf_Pago: Método para una Incidencia_Con_Pago
        /// </summary>
        /// <returns>Una instancia de la clase cIncidencia_Conf_Pago</returns>
        /// ///<param name="id">Id Incidencia Conf Pago</param>
        public async Task<cIncidencia_Conf_Pago> GetIncidencia_Conf_Pago(int id)
        {
            cIncidencia_Conf_Pago? IncidenciaConfPago = new();
            try
            {
                IncidenciaConfPago = await _context.Incidencias_Conf_Pago.FirstOrDefaultAsync(e => e.ID == id);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetIncidencia_Conf_Pago: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return IncidenciaConfPago;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-2-6
        /// <summary>
        /// Sincronizar_Incidencia_Conf_Pago: Método para registrar los registros en la tabla Incidencia_Conf_Pago
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="incidenciasConfPago">Lista de registros de la clase cIncidencia_Conf_Pago</param>
        public async Task<EventResponse> Sincronizar_Incidencia_Conf_Pago(IEnumerable<cIncidencia_Conf_Pago> incidenciasConfPago)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in incidenciasConfPago)
                {
                    cIncidencia_Conf_Pago? objetoBuscar = await _context.Incidencias_Conf_Pago
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.ID = item.ID;
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.ID_APL = item.ID_APL;
                        objetoBuscar.ID_HRS = item.ID_HRS;
                        objetoBuscar.ID_CON = item.ID_CON;
                        objetoBuscar.ID_ADICIONAL = item.ID_ADICIONAL;
                        objetoBuscar.APL_TURNO = item.APL_TURNO;
                        objetoBuscar.TRAN_TURNO = item.TRAN_TURNO;

                        _context.Incidencias_Conf_Pago.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Inicdencia_Conf_Pago. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Incidencia_Conf_Pago. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// Elimina_Incidencia_Conf_Pago:  Metodo borrado de datos de la tabla cIncidencia_Conf_Pago
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Incidencia_Conf_Pago(int id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cIncidencia_Conf_Pago? model = await _context.Incidencias_Conf_Pago
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (model is not null)
                {
                    _context.Incidencias_Conf_Pago.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Incidencia_Cond_Pago. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Incidencia_Cond_Pago. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        /// <summary>
        /// GetPortalMenu: Obtener lista de menus de sistema 
        /// </summary>
        /// <returns>Lista de lista de menus del sistema</returns>
        public async Task<List<cPortal_Menu>> GetPortalMenu()
        {
            List<cPortal_Menu>? portalMenu = new();

            try
            {
                portalMenu = (from e in await _context.Portal_Menu
                                .Include(e => e.cPortal_Opcion)
                            .ToListAsync()
                              select new cPortal_Menu
                              {
                                  ID = e.ID,
                                  MENUTEXT = e.MENUTEXT,
                                  ICONID = e.ICONID,
                                  cPortal_Opcion = e.cPortal_Opcion == null ? null :
                                                  (from po in e.cPortal_Opcion
                                                   select new cPortal_Opcion
                                                   {
                                                       PARENTID = po.PARENTID,
                                                       ID = po.ID,
                                                       MENUTEXT = po.MENUTEXT,
                                                       ICONID = po.ICONID,
                                                       PRINCIPAL = po.PRINCIPAL,
                                                       HREF = po.HREF,
                                                   }).ToList()
                              }
                            ).ToList();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalMenu: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalMenu;
        }

        /// <summary>
        /// GetPortalMenu: Obtener datos de una opcion de menu de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Menu </returns>
        public async Task<cPortal_Menu> GetPortalMenu(string id)
        {
            cPortal_Menu? portalMenu = new();

            try
            {

                portalMenu = (from e in await _context.Portal_Menu.Where(e => e.ID == id)
                                .Include(e => e.cPortal_Opcion)
                            .ToListAsync()
                              select new cPortal_Menu
                              {
                                  ID = e.ID,
                                  MENUTEXT = e.MENUTEXT,
                                  ICONID = e.ICONID,
                                  cPortal_Opcion = e.cPortal_Opcion == null ? null :
                                                  (from po in e.cPortal_Opcion
                                                   select new cPortal_Opcion
                                                   {
                                                       PARENTID = po.PARENTID,
                                                       ID = po.ID,
                                                       MENUTEXT = po.MENUTEXT,
                                                       ICONID = po.ICONID,
                                                       PRINCIPAL = po.PRINCIPAL,
                                                       HREF = po.HREF,
                                                   }).ToList()
                              }
                            ).FirstOrDefault();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalMenu: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalMenu;
        }


        /// <summary>
        /// Sincronizar_PortalMenu: Método para registrar los menus del sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Menu </param>
        public async Task<EventResponse> Sincronizar_PortalMenu(IEnumerable<cPortal_Menu> portalMenu)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in portalMenu)
                {
                    cPortal_Menu? portalMenuBuscar = await _context.Portal_Menu
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalMenuBuscar is not null)
                    {
                        portalMenuBuscar.ICONID = item.ICONID;
                        portalMenuBuscar.MENUTEXT = item.MENUTEXT;

                        _context.Portal_Menu.Update(portalMenuBuscar);
                    }
                    else
                    {
                        item.cPortal_Opcion = null;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro del menú. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro del menú. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        /// <summary>
        /// GetPortalPol: Obtener lista de roles del Portal de empleado y Geotime.net 
        /// </summary>
        /// <returns>Lista de lista de roles del Portal de empleado y Geotime.net </returns>
        public async Task<List<cPortal_Rol>> GetPortalRol()
        {
            List<cPortal_Rol>? portalRol = new();

            try
            {
                portalRol = (from e in await _context.Portal_Rol
                                .Include(e => e.cPortal_RolDet)
                            .ToListAsync()
                             select new cPortal_Rol
                             {
                                 ID = e.ID,
                                 DESCRIPCION = e.DESCRIPCION,
                                 ROLDEFAULT = e.ROLDEFAULT,
                                 HABILITADO = e.HABILITADO,
                                 cPortal_RolDet = e.cPortal_RolDet == null ? null :
                                                 (from po in e.cPortal_RolDet
                                                  select new cPortal_RolDet
                                                  {
                                                      PORTALROLID = po.PORTALROLID,
                                                      PORTALMENUID = po.PORTALMENUID,
                                                      PORTALOPCIONID = po.PORTALOPCIONID,
                                                      HABILITADO = po.HABILITADO,
                                                  }).ToList()
                             }
                            ).ToList();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalRol: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalRol;
        }

        /// <summary>
        /// GetPortalRol: Obtener datos de un rol del portal especifico
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Rol </returns>
        public async Task<cPortal_Rol> GetPortalRol(string id)
        {
            cPortal_Rol? portalRol = new();

            try
            {

                portalRol = (from e in await _context.Portal_Rol.Where(e => e.ID == id)
                                .Include(e => e.cPortal_RolDet)
                            .ToListAsync()
                             select new cPortal_Rol
                             {
                                 ID = e.ID,
                                 DESCRIPCION = e.DESCRIPCION,
                                 ROLDEFAULT = e.ROLDEFAULT,
                                 HABILITADO = e.HABILITADO,
                                 cPortal_RolDet = e.cPortal_RolDet == null ? null :
                                                 (from po in e.cPortal_RolDet
                                                  select new cPortal_RolDet
                                                  {
                                                      PORTALROLID = po.PORTALROLID,
                                                      PORTALMENUID = po.PORTALMENUID,
                                                      PORTALOPCIONID = po.PORTALOPCIONID,
                                                      HABILITADO = po.HABILITADO,
                                                  }).ToList()
                             }
                            ).FirstOrDefault();

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalRol: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalRol;
        }

        /// <summary>
        /// Sincronizar_PortalRol: Método para registrar los roles del portal
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Rol </param>
        public async Task<EventResponse> Sincronizar_PortalRol(IEnumerable<cPortal_Rol> portalRol)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                List<cPortal_RolDet>? rolesDet;
                foreach (var item in portalRol)
                {

                    rolesDet = item.cPortal_RolDet.ToList();
                    cPortal_Rol? portalrolBuscar = await _context.Portal_Rol
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalrolBuscar is not null)
                    {
                        portalrolBuscar.DESCRIPCION = item.DESCRIPCION;
                        portalrolBuscar.ROLDEFAULT = item.ROLDEFAULT;
                        portalrolBuscar.HABILITADO = item.HABILITADO;

                        _context.Portal_Rol.Update(portalrolBuscar);
                    }
                    else
                    {
                        item.cPortal_RolDet = null;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                    foreach (var roldet in rolesDet)
                    {
                        cPortal_RolDet? portalroldetBuscar = await _context.Portal_RolDet
                                    .Where(e => e.PORTALROLID == roldet.PORTALROLID
                                             && e.PORTALMENUID == roldet.PORTALMENUID
                                             && e.PORTALOPCIONID == roldet.PORTALOPCIONID)
                                    .FirstOrDefaultAsync();
                        //si la opcion existe se actualiza 
                        //de lo contrario se agrega el registro
                        if (portalroldetBuscar is not null)
                        {
                            portalroldetBuscar.HABILITADO = roldet.HABILITADO;

                            _context.Portal_RolDet.Update(portalroldetBuscar);
                        }
                        else
                        {
                            _context.Add(roldet);
                        }

                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro del rol. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro del rol. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        /// <summary>
        /// EjecutaCalculoPlanilla: Recibe una lista de MarcaMovTurno y a partir de ella realiza el calculo de planilla de forma temporal
        /// </summary>
        /// <param name="marcasMovTurnos">lista de MarcaMovTurno</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public async Task<EventResponse> EjecutaCalculoPlanilla(IEnumerable<cMarcaMovTurno> marcasMovTurnos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cMarcaProceso? marcaProcesoEnCero;
                List<cMarcaProceso> marcasProcesoInicializada = new();
                var compania = await _context.PH_COMPANIAS.FirstOrDefaultAsync();


                foreach (var item in marcasMovTurnos)
                {
                    var horario = item.hora.Split("|");
                    var turno = await _context.Ph_Turnos.FirstOrDefaultAsync(e => e.IdTurno == item.turno);
                    var fechaSalida = turno!.HEntra!.CompareTo(turno.HSale) > 0 ? item.fecha.AddDays(1) : item.fecha;


                    cMarcaProceso? marcaProceso = new cMarcaProceso
                    {
                        idregistro = 0,
                        idplanilla = item.idplanilla,
                        idnumero = item.idnumero,
                        fecha_entra = item.fecha,
                        fecha_sale = fechaSalida,
                        hora_entra = horario[0] == "99:99" ? "00:00" : turno.HEntra,
                        hora_sale = horario[0] == "99:99" ? "00:00" : turno.HSale!,
                        idturno = item.turno,
                        CON_1 = 1,
                        CON_2 = 1,
                        CON_3 = 1,
                        CON_4 = 1,
                        CON_5 = 1,
                    };

                    marcaProcesoEnCero = new cMarcaProceso
                    {
                        idregistro = 0,
                        idplanilla = item.idplanilla,
                        idnumero = item.idnumero,
                        fecha_entra = item.fecha,
                        fecha_sale = fechaSalida,
                        hora_entra = "00:00", //se debe inicilizar la marca en 00:00 horas para que no afecte el proceso que realiza Geotime
                        hora_sale = "00:00", //se debe inicilizar la marca en 00:00 horas para que no afecte el proceso que realiza Geotime
                        idturno = item.turno,
                        CON_1 = 1,
                        CON_2 = 1,
                        CON_3 = 1,
                        CON_4 = 1,
                        CON_5 = 1,
                    };

                    marcasProcesoInicializada.Add(marcaProcesoEnCero);

                    var existeMarca = await _context.Marcas_Proceso
                                    .Where(e => e.idnumero == item.idnumero
                                            && e.fecha_entra == item.fecha
                                            && e.idturno == item.turno)
                                    .FirstOrDefaultAsync();

                    if (existeMarca is not null)
                    {
                        existeMarca.hora_entra = marcaProceso.hora_entra;
                        existeMarca.hora_sale = marcaProceso.hora_sale!;
                        existeMarca.fecha_sale = marcaProceso.fecha_sale;

                        _context.Marcas_Proceso.Update(existeMarca);
                    }
                    else
                    {
                        _context.Add(marcaProceso);
                    }
                }
                await _context.SaveChangesAsync();



                DateTime fechaInicial = marcasMovTurnos.Min(e => e.fecha);
                DateTime fechaFinal = marcasMovTurnos.Max(e => e.fecha);
                var periodos = await GetPeriodo(fechaInicial.ToString("yyyyMMdd"), "S");
                //var planillasEmpleados = marcasMovTurnos.Select(e=>e.idplanilla).Distinct();
                var phplanillas = await GetPhPlanilla();
                var listaEmpleados = (from e in marcasMovTurnos
                                      select new
                                      {
                                          idnumero = e.idnumero,
                                          idplanilla = e.idplanilla
                                      })
                                     .Distinct().ToList();

                //var empleados = await _context.Empleados.Where(e => listaEmpleados.Contains(e.IdNumero)).ToListAsync();
                //var listaGrupos = String.Join(",", empleados.Select(e => e.IdGrupo).Distinct().ToList());

                var phloginAdmin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.usuario.ToUpper() == "Admin");
                if (phloginAdmin is not null)
                {
                    phloginAdmin.ultimo_login = DateTime.Now;
                    phloginAdmin.ultimo_estado = DateTime.Now;
                    phloginAdmin.idsesion = 1;
                    _context.PH_LOGIN.Update(phloginAdmin);
                    await _context.SaveChangesAsync();

                }

            

                foreach (var empleado in listaEmpleados)
                {
                    var planilla = phplanillas.FirstOrDefault(e => e.idplanilla == empleado.idplanilla);
                    var periodo = periodos.FirstOrDefault(e => e.tipo_planilla == planilla!.tipo_planilla);

                    
                    calculo_periodo_empleadoRequest calculoPlanilla = new calculo_periodo_empleadoRequest
                    {
                        comp = compania!.IDCOMP!,
                        idpais = compania.PAIS!,
                        plan = planilla!.idplanilla,
                        sesion = (int)phloginAdmin!.idsesion!,
                        empleado = empleado.idnumero,
                        periodo = periodo!.idperiodo,
                        //inicio = fechaInicial.ToString("yyyy-MM-dd"),
                        //fin = fechaFinal.ToString("yyyy-MM-dd"),
                        inicio = periodo.inicio.ToString("yyyy-MM-dd"),
                        fin = periodo.fin.ToString("yyyy-MM-dd"),
                    };

                    EndpointConfiguration endpointConfiguration = new();
                    GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                    var result = await geoWebService.calculo_periodo_empleadoAsync(calculoPlanilla);
                    if (result.calculo_periodo_empleadoResult != "")
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = $"Error al procesar Empleado Id=: {empleado.idnumero}: {result.calculo_periodo_empleadoResult}";
                    }
                }

                if (respuesta.Id == "0")
                {
                    var MarcasEliminar = (from mp in await _context.Marcas_Proceso
                                                      .Where(e => e.fecha_entra >= fechaInicial && e.fecha_entra <= fechaFinal).ToListAsync()
                                          join e in listaEmpleados on mp.idnumero equals e.idnumero
                                          select mp).ToList();

                    _context.Marcas_Proceso.RemoveRange(MarcasEliminar);
                    await _context.SaveChangesAsync();

                    //Proceso requerido para Guandy.
                    //una vez finalizado el proceso de Calculo se debe volver a registrar en Marcas proceso pero con la hora de entrada y salida 
                    //inicializadas en cero.

                    var resp = await InicializaMarcaProceso(marcasProcesoInicializada);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el Calculo de Planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el Calculo de Planilla. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        /// <summary>
        /// InicializaMarcaProceso: Recibe una lista de MarcaMovTurno y a partir de ella inicializa la tabla Marcas Proceso
        /// </summary>
        /// <param name="marcasMovTurnos">lista de MarcaMovTurno</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public async Task<EventResponse> InicializaMarcaProceso(IEnumerable<cMarcaProceso> marcasProceso)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                foreach (var item in marcasProceso)
                {

                    var existeMarca = await _context.Marcas_Proceso
                                    .Where(e => e.idnumero == item.idnumero
                                            && e.fecha_entra == item.fecha_entra
                                            && e.idturno == item.idturno)
                                    .FirstOrDefaultAsync();

                    if (existeMarca is null)
                    {
                        _context.Add(item);
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
                    respuesta.Descripcion = "No se pudo realizar la Inicialización de las Marcas del Porceso. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Inicialización de las Marcas del Porceso. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        /// <summary>
        /// EjecutaCalculoPlanillaEmpleado:  ejecuta calculo de planilla para el empleado indicado en el parametro
        /// </summary>
        /// <param name="calculo_periodo_param"></param>
        /// <returns>Intancia de eventresponse con el resultado de la ejecucion del proceso</returns>
        public async Task<EventResponse> EjecutaCalculoPlanillaEmpleado(cCalculoPeriodoParam calculo_periodo_param)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var compania = await _context.PH_COMPANIAS.FirstOrDefaultAsync(e=>e.IDCOMP==calculo_periodo_param.comp);
                string fechaInicioExt = $"{calculo_periodo_param.inicio.Substring(0, 4)}-{calculo_periodo_param.inicio.Substring(4, 2)}-{calculo_periodo_param.inicio.Substring(6, 2)}";
                string fechaFinExt = $"{calculo_periodo_param.fin.Substring(0, 4)}-{calculo_periodo_param.fin.Substring(4, 2)}-{calculo_periodo_param.fin.Substring(6, 2)}";


                var phloginAdmin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.usuario.ToUpper() == "Admin");
                if (phloginAdmin is not null)
                {
                    phloginAdmin.ultimo_login = DateTime.Now;
                    phloginAdmin.ultimo_estado = DateTime.Now;
                    phloginAdmin.idsesion = 1;
                    _context.PH_LOGIN.Update(phloginAdmin);
                    await _context.SaveChangesAsync();

                }



                calculo_periodo_empleadoRequest calculoPlanilla = new calculo_periodo_empleadoRequest
                {
                    comp = compania!.IDCOMP,
                    idpais = compania.PAIS!,
                    plan = calculo_periodo_param.plan,
                    sesion = (int)phloginAdmin!.idsesion!,
                    empleado = calculo_periodo_param.empleado,
                    periodo = calculo_periodo_param.periodo,
                    inicio = fechaInicioExt,
                    fin = fechaFinExt,
                };

                EndpointConfiguration endpointConfiguration = new();
                GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                var result = await geoWebService.calculo_periodo_empleadoAsync(calculoPlanilla);
                if (result.calculo_periodo_empleadoResult != "")
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = $"Error al Ejecutar Calculo de Nómina para Empleado: {calculo_periodo_param.empleado} País: {calculo_periodo_param.idpais}, Compania:{calculo_periodo_param.comp} Plan:{calculo_periodo_param.plan}, Periodo: {calculo_periodo_param.periodo}, Inicio: {calculo_periodo_param.inicio} Fin: {calculo_periodo_param.fin}. Detalle de Error: {result.calculo_periodo_empleadoResult}";
                }


            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el Calculo de Planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el Calculo de Planilla. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Allan Prieto
        //Fecha: 2024-4-3
        /// <summary>
        /// GetPhDescansoTurno: Obtener lista de registros de la tabla cPh_DescansoTurno
        /// </summary>
        /// <returns>Lista de cPh_DescansoTurno </returns>
        /// 
        public async Task<List<cPh_DescansoTurno>> GetPhDescansoTurno(int idTurno, int idTiempo)
        {
            List<cPh_DescansoTurno> descansoTurno = new();
            //List<cPh_DescansoTurno> item = new();
            try
            {
                descansoTurno = await _context.Ph_Descansos_Turnos.Where(e => e.IDTURNO == idTurno &&
                                                                              e.IDTIEMPO <= idTiempo).ToListAsync();

                //for (int i = 1; i <= idTiempo; i++)
                //{
                //    int tiempoActual = i;
                //    var resultados =  await _context.Ph_Descansos_Turnos.Where(e => e.IDTURNO == idTurno && e.IDTIEMPO == tiempoActual).ToListAsync();
                //    descansoTurno.AddRange(resultados);
                //}

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhDescansoTurno: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return descansoTurno;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-4-3
        /// <summary>
        /// Sincronizar_PhDescansoTurno: Método para registrar los registros en la tabla Descansos de Turnos
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="ph_DescansoTurno">Lista de registros de la clase cPh_DescansoTurno</param>
        public async Task<EventResponse> Sincronizar_PhDescansoTurno(IEnumerable<cPh_DescansoTurno> ph_DescansoTurno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in ph_DescansoTurno)
                {
                    cPh_DescansoTurno? objetoBuscar = await _context.Ph_Descansos_Turnos
                                    .Where(e => e.IDTURNO == item.IDTURNO && e.IDTIEMPO == item.IDTIEMPO)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {

                        objetoBuscar.IDTURNO = item.IDTURNO;
                        objetoBuscar.IDTIEMPO = item.IDTIEMPO;
                        objetoBuscar.INICIO = item.INICIO;
                        objetoBuscar.FIN = item.FIN;
                        objetoBuscar.TIEMPO = item.TIEMPO;
                        objetoBuscar.DESCUENTA = item.DESCUENTA;
                        objetoBuscar.TIEMP_EXT = item.TIEMP_EXT;
                        objetoBuscar.DESC_EXC = item.DESC_EXC;

                        _context.Ph_Descansos_Turnos.Update(objetoBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Ph_DescansoTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla Ph_DescansoTurno. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// GetPhOpciones: Obtener datos de Sistema 
        /// </summary>
        /// <returns>Instancia de cPh_Opciones con las Opciones del sistema </returns>
        public async Task<cPh_Opciones> GetPhOpciones()
        {
            cPh_Opciones? phOpciones = new();

            try
            {
               
                phOpciones = await _context.Ph_Opciones.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhOpciones: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return phOpciones;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-13
        //Sincronizar Opciones del Sisstema
        //Parametro: Recibe una instancia de cPh_Opciones
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_PhOpciones(IEnumerable<cPh_Opciones> phopciones)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var item in phopciones)
                {
                    cPh_Opciones? parametroBuscado = await _context.Ph_Opciones
                                                            .FirstOrDefaultAsync();

                    if (parametroBuscado is not null)
                    {
                        parametroBuscado.IDOPCION = item.IDOPCION;
                        parametroBuscado.POST_EMP = item.POST_EMP;
                        parametroBuscado.POST_SINC = item.POST_SINC;
                        parametroBuscado.NUM_ALM = item.NUM_ALM;
                        parametroBuscado.UTILIZA_DESC = item.UTILIZA_DESC;
                        parametroBuscado.DESC_ABIERT = item.DESC_ABIERT;
                        //parametroBuscado.VER_DB = item.VER_DB; /* Dato no es necesario actualizarlo
                        parametroBuscado.CORTE_DIURNO = item.CORTE_DIURNO;
                        parametroBuscado.CORTE_NOCTURNO = item.CORTE_NOCTURNO;
                        parametroBuscado.USA_ALERTA_EXD = item.USA_ALERTA_EXD;
                        parametroBuscado.ALERTA_EXTRAS_DIARIAS = item.ALERTA_EXTRAS_DIARIAS;
                        parametroBuscado.COLOR_FONDO_ALERTD = item.COLOR_FONDO_ALERTD;
                        parametroBuscado.COLOR_FUENTE_ALERTD = item.COLOR_FUENTE_ALERTD;
                        parametroBuscado.DIST_TADIC = item.DIST_TADIC;
                        parametroBuscado.DIST_LIC_USR = item.DIST_LIC_USR;
                        parametroBuscado.DIST_LIC_EMP = item.DIST_LIC_EMP;
                        parametroBuscado.TIPO_DIST = item.TIPO_DIST;
                        parametroBuscado.ACC_BLOC_PT = item.ACC_BLOC_PT;
                        parametroBuscado.ORG_BASE = item.ORG_BASE;

                        _context.Ph_Opciones.Update(parametroBuscado);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Opciones del Sistema. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Opciones del Sistema. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }


        //Creado por: Marlon Loria Solano
        //11-03-2024
        /// <summary>
        /// GetPortalEmpleado: Lista de empleados con acceso al portal de marcas web
        /// </summary>
        /// <returns>Lista de empleados con acceso al portal de marcas web</returns>
        public async Task<List<cPortal_Empleado>> GetPortalEmpleado()
        {
            List<cPortal_Empleado> model = new();
            try
            {
                model = await _context.Portal_Empleado.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }

        //Creado por: Marlon Loria Solano
        //11-03-2024
        /// <summary>
        /// GetPortalEmpleado: Un empleado con acceso al portal de marcas web
        /// </summary>
        /// <param name="id">id de empleado a buscar</param>
        /// <returns>Un empleado con acceso al portal de marcas web</returns>
        public async Task<cPortal_Empleado> GetPortalEmpleado(string id)
        {
            cPortal_Empleado? model = new();
            try
            {
                model = await _context.Portal_Empleado.FirstOrDefaultAsync(e => e.IDNUMERO == id);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalEmpleado: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }

        /// <summary>
        /// Sincronizar_PortalEmpleado:  Crear o actualizar la lista de empleados con acceso a marcar web.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="portalEmpleados">Recibe una instancia de cPortal_Empleado</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public async Task<EventResponse> Sincronizar_PortalEmpleado(IEnumerable<cPortal_Empleado> portalEmpleados)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                int usuariosActivos = portalEmpleados.FirstOrDefault().USUARIOS_HABILITADOS;

                var portalConfig = await _context.Portal_Config.FirstOrDefaultAsync(e => e.IDAPLICACION == "com.gsitcr.portalmarcasweb");

                var licencia = Encripta.getDecryptTripleDES(portalConfig!.IDLICENCIA);                                 

                string dataLic = _encriptaService.Decrypt(licencia);
                string[] data = dataLic.Split("|");
                var fechaVence = data[1];
                var cantLicencias = data[2];
                var licPermanente = data[3] == "1";
                string strFechaVence = $"{fechaVence.Substring(0, 4)}-{fechaVence.Substring(4, 2)}-{fechaVence.Substring(6, 2)} 23:59:59";
                DateTime dfechaVence = DateTime.Parse(strFechaVence);

                if (!licPermanente)
                {
                    if (dfechaVence < DateTime.Now)
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "La licencia del sistema ha expirado.  Por favor contacte al administrador del sistema.";
                        return respuesta;
                    }
                }

                if (!portalConfig.USORESTRINGIDO)
                {
                    var listPortalEmpleados = await _context.Portal_Empleado.ToListAsync();
                    foreach (var empleado in listPortalEmpleados)
                    {
                        var existeEmpleado = portalEmpleados.FirstOrDefault(e => e.IDNUMERO == empleado.IDNUMERO);

                        if (existeEmpleado is null)
                        {
                            _context.Portal_Empleado.Remove(empleado);
                            usuariosActivos -= 1;
                        }

                    }
                    await _context.SaveChangesAsync();
                }

                foreach (var item in portalEmpleados)
                {
                    cPortal_Empleado? portalEmp = await _context.Portal_Empleado
                                                        .FirstOrDefaultAsync(e => e.IDNUMERO == item.IDNUMERO);
                    //si el empleado existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalEmp is not null)
                    {
                        if (!portalEmp.HABILITADO)
                        {
                            //si en el empleado estaba deshabilitado y se activa entonces se cuenta como una licencia nueva en uso
                            if (item.HABILITADO)
                                usuariosActivos+=1;
                        }
                        else
                        {
                            //si el empleado estaba habilitado y se inactiva se resta licencia
                            if (!item.HABILITADO)
                                usuariosActivos -=1;
                        }
                        portalEmp.PORTALROLID = item.PORTALROLID;
                        portalEmp.HABILITADO = item.HABILITADO;
                        _context.Portal_Empleado.Update(portalEmp);
                    }
                    else
                    {
                        //es un empleado que ingresa como usuario del portal de marcas habilitado, se suma licencia
                        if (item.HABILITADO)
                            usuariosActivos += 1;

                        _context.Add(item);
                    }
                }

                //antes de guardar se verifica que hayan licencias suficientes para efectuar la operación.
                if (int.Parse(cantLicencias)>= usuariosActivos)
                {
                    await _context.SaveChangesAsync();

                    portalConfig.REGSITROLIC = Encripta.getEncryptTripleDES(usuariosActivos.ToString());
                    _context.Portal_Config.Update(portalConfig);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = $"Se ha superado la cantidad de Empleados permitidos por el sistema. La cantidad de licencias registrada para la compania es de {cantLicencias} y actualmente se han utilizado {usuariosActivos}.";
                }

                
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados para el portal de Marcas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados para el portal de Marcas. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        /// <summary>
        /// PutPortalEmpleado:  Actualizar la lista de empleados con acceso a marcar web.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="portalEmpleado">Recibe una instancia de cPortal_Empleado</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public async Task<EventResponse> PutPortalEmpleado(cPortal_Empleado portalEmpleado)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                int usuariosActivos = portalEmpleado.USUARIOS_HABILITADOS;

                var portalConfig = await _context.Portal_Config.FirstOrDefaultAsync(e => e.IDAPLICACION == "com.gsitcr.portalmarcasweb");

                var licencia = Encripta.getDecryptTripleDES(portalConfig!.IDLICENCIA);

                string dataLic = _encriptaService.Decrypt(licencia);
                string[] data = dataLic.Split("|");
                var fechaVence = data[1];
                var cantLicencias = data[2];
                var licPermanente = data[3] == "1";
                string strFechaVence = $"{fechaVence.Substring(0, 4)}-{fechaVence.Substring(4, 2)}-{fechaVence.Substring(6, 2)} 23:59:59";
                DateTime dfechaVence = DateTime.Parse(strFechaVence);

                if (!licPermanente)
                {
                    if (dfechaVence < DateTime.Now)
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "La licencia del sistema ha expirado.  Por favor contacte al administrador del sistema.";
                        return respuesta;
                    }
                }


                //es un empleado que ingresa como usuario del portal de marcas habilitado, se suma licencia
                usuariosActivos += 1;
                _context.Add(portalEmpleado);

                //antes de guardar se verifica que hayan licencias suficientes para efectuar la operación.
                if (int.Parse(cantLicencias) >= usuariosActivos)
                {
                    await _context.SaveChangesAsync();

                    portalConfig.REGSITROLIC = Encripta.getEncryptTripleDES(usuariosActivos.ToString());
                    _context.Portal_Config.Update(portalConfig);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = $"Se ha superado la cantidad de Empleados permitidos por el sistema. Por favor contacte al administrador del sistema.";
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de Empleados para el portal de Marcas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de Empleados para el portal de Marcas. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //11-03-2024
        /// <summary>
        /// GetPortalDocMarca: Lista de documentos total de Documentos Marcas
        /// </summary>
        /// <returns>Lista de documentos total de Documentos Marcas</returns>
        public async Task<List<cPortal_DocMarca>> GetPortalDocMarca()
        {
            List<cPortal_DocMarca> model = new();
            try
            {
                model = await _context.Portal_DocsMarcas.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalDocMarca: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }

        //Creado por: Marlon Loria Solano
        //11-03-2024
        /// <summary>
        /// GetPortalDocMarca: Lista de documentos asociados al empleado en una fecha especifica
        /// </summary>
        /// <param name="idnumero">numero de empleado</param>
        /// <param name="fecha">fecha del documento</param>
        /// <returns>Lista de documentos asociados al empleado en una fecha especifica</returns>
        public async Task<List<cPortal_DocMarca>> GetPortalDocMarca(string idnumero, string fecha)
        {
            List<cPortal_DocMarca> model = new();
            try
            {
                DateTime fechaDoc = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                model = await _context.Portal_DocsMarcas
                                .Where(e => e.FECHA == fechaDoc && e.IDNUMERO == idnumero)
                                .ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalDocMarca: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }

        //Creado por: Marlon Loria Solano
        //11-03-2024
        /// <summary>
        /// GetPortalDocMarca: Obtiene un documento especifico de Portal DocsMarcas
        /// </summary>
        /// <param name="idregistro">id del registro</param>
        /// <returns>Un documento especifico de Portal DocsMarcas</returns>
        public async Task<cPortal_DocMarca> GetPortalDocMarca(long idregistro)
        {
            cPortal_DocMarca? model = new();
            try
            {
                model = await _context.Portal_DocsMarcas.FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPortalDocMarca: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }

        /// <summary>
        /// Sincronizar_PortalDocMarca:  Crear o actualizar la lista de documentos asociados a las marcas.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="portalDocsMarcas">Recibe una instancia de cPortal_DocMarca</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public async Task<EventResponse> Sincronizar_PortalDocMarca(IEnumerable<cPortal_DocMarca> portalDocsMarcas)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                foreach (var item in portalDocsMarcas)
                {
                    item.IDREGISTRO = 0;
                    _context.Add(item);

                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del documento asociado a la Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del documento asociado a la Marca. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetPaletaColor: obtiene lista de colores de la paleta
        /// </summary>
        /// <returns>lista de colores de la paleta</returns>
        public async Task<List<cPaletaColor>> GetPaletaColor()
        {
            List<cPaletaColor> colores = new();
            try
            {
                colores = await _context.PaletaColores.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPaletaColor: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return colores;
        }
        /// <summary>
        /// GetPaletaColor: obtiene un registro de la paleta de colores
        /// </summary>
        /// <param name="colorId">id de color a recuperar</param>
        /// <returns></returns>
        public async Task<cPaletaColor> GetPaletaColor(int colorId)
        {
            cPaletaColor? compania = new();
            try
            {
                compania = await _context.PaletaColores.FirstOrDefaultAsync(e => e.COLORID == colorId);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPaletaColor: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return compania;
        }
        /// <summary>
        /// Sincronizar_PaletaColor: metodo para sincronizar lista de colores en la Paleta de Colores 
        /// </summary>
        /// <param name="colores"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_PaletaColor(IEnumerable<cPaletaColor> colores)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in colores)
                {
                    cPaletaColor? objetoBuscar = await _context.PaletaColores
                                    .FirstOrDefaultAsync(e => e.COLORID == item.COLORID);
                    //si el color existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.COLORFONDO = item.COLORFONDO;
                        objetoBuscar.COLORFUENTE = item.COLORFUENTE;                       

                        _context.PaletaColores.Update(objetoBuscar);
                    }
                    else
                    {
                        item.COLORID = 0;
                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del color en la Paleta de Colores. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del color en la Paleta de Colores. Detalle de Error: " + e.InnerException.Message;
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Sincronizar_PaletaColor: {respuesta.Descripcion}");


            }

            return respuesta;

        }
        /// <summary>
        /// Elimina_PaletaColor:  Metodo borrado de datos de la tabla PaletaColores
        /// </summary>
        /// <param name="colorId"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PaletaColor(int colorId)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPaletaColor? model = await _context.PaletaColores
                    .FirstOrDefaultAsync(e => e.COLORID == colorId);

                if (model is not null)
                {
                    _context.PaletaColores.Remove(model);
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Color. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Color. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// GetNivel: obtiene lista de Niveles
        /// </summary>
        /// <returns>lista de niveles</returns>
        public async Task<List<cPh_Nivel>> GetNivel()
        {
            List<cPh_Nivel> niveles = new();
            try
            {
                niveles = await _context.Ph_Niveles.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetNivel: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return niveles;
        }
        /// <summary>
        /// GetNivel: obtiene un registro de niveles
        /// </summary>
        /// <param name="IdNivel">id de color a recuperar</param>
        /// <returns></returns>
        public async Task<cPh_Nivel> GetNivel(int IdNivel)
        {
            cPh_Nivel? nivel = new();
            try
            {
                nivel = await _context.Ph_Niveles.FirstOrDefaultAsync(e => e.IDNIVEL == IdNivel);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetNivel: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return nivel;
        }
        /// <summary>
        /// Sincronizar_Nivel: metodo para sincronizar lista de niveles
        /// </summary>
        /// <param name="niveles"></param>
        /// <returns>una instancia EventResponse con el resultado de los nivveles</returns>
        public async Task<EventResponse> Sincronizar_Nivel(IEnumerable<cPh_Nivel> niveles)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in niveles)
                {
                    cPh_Nivel? objetoBuscar = await _context.Ph_Niveles
                                    .FirstOrDefaultAsync(e => e.IDNIVEL == item.IDNIVEL);
                    //si el color existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.VARIABLES = item.VARIABLES;

                        _context.Ph_Niveles.Update(objetoBuscar);
                    }
                    else
                    {
                        //item.COLORID = 0;
                        _context.Add(item);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Nivel. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Nivel. Detalle de Error: " + e.InnerException.Message;

               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Sincronizar_Nivel: {respuesta.Descripcion}");
            }
            return respuesta;
        }
        /// <summary>
        /// Elimina_Nivel:  Metodo borrado de datos de la tabla Ph_Niveles
        /// </summary>
        /// <param name="IdNivel"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Nivel(int IdNivel)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_Nivel? model = await _context.Ph_Niveles
                    .FirstOrDefaultAsync(e => e.IDNIVEL == IdNivel);

                if (model is not null)
                {
                    _context.Ph_Niveles.Remove(model);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Nivel. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Nivel. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-27
        //Obtener lista de Marcas_Distribuciones_Conceptos
        public async Task<List<cMarcaDistribucionConcepto>> GetMarcaDtnConcepto()
        {
            List<cMarcaDistribucionConcepto> marcaDC = new();
            try
            {
                marcaDC = await _context.Marcas_Distribuciones_Conceptos.ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaDtnConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaDC;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-27
        //Obtener lista de Marcas_Distribuciones_Conceptos
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaDistribucionConcepto> GetMarcaDtnConcepto(int idregistro)
        {
            cMarcaDistribucionConcepto? marcaDC = new();
            try
            {
                marcaDC = await _context.Marcas_Distribuciones_Conceptos.FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaDtnConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return marcaDC;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-27
        //Obtener lista de Marcas_Distribuciones_Conceptos
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaDistribucionConcepto> GetMarcaDtnConcepto(string idnumero, string fecha, string idplanilla)
        {
            cMarcaDistribucionConcepto? item = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                item = await _context.Marcas_Distribuciones_Conceptos.FirstOrDefaultAsync(e => e.IDNUMERO == idnumero && e.FECHA == fechaMov && e.IDPLANILLA == idplanilla);
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaDtnConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return item;
        }

        //Marlon Loria 04-06-2024
        /// <summary>
        /// GetMarcaDtnConcepto: Obtener lista de Marcas_Distribuciones_Conceptos
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fecha"></param>
        /// <param name="idplanilla"></param>
        /// <returns>lista de Marcas_Distribuciones_Conceptos</returns>
        public async Task<IEnumerable<cMarcaDistribucionConcepto>> GetMarcaDtnConcepto(string idplanilla, string fechaInicio, string fechaFinal, string idnumero)
        {
            List<cMarcaDistribucionConcepto>? modelo = new();
            try
            {
                DateTime fechaInicioExt = DateTime.Parse($"{fechaInicio.Substring(0, 4)}-{fechaInicio.Substring(4, 2)}-{fechaInicio.Substring(6, 2)}");
                DateTime fechaFinExt = DateTime.Parse($"{fechaFinal.Substring(0, 4)}-{fechaFinal.Substring(4, 2)}-{fechaFinal.Substring(6, 2)}");

                modelo = await _context.Marcas_Distribuciones_Conceptos
                              .Where(e => e.IDNUMERO == idnumero 
                                    && e.FECHA >= fechaInicioExt 
                                    && e.FECHA <= fechaFinExt 
                                    && e.IDPLANILLA == idplanilla)
                              .ToListAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetMarcaDtnConcepto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return modelo;
        }


        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-05-27
        //Sincronizar Marcas_Distribuciones_Conceptos
        //Parametro: Recibe una instancia de Marcas_Distribuciones_Conceptos, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcaDtnConcepto(IEnumerable<cMarcaDistribucionConcepto> marcasDtnConcepto)
        {
            EventResponse respuesta = new EventResponse();
            
            try
            {
                foreach (var item in marcasDtnConcepto)
                {
                    int IDDIST = 0;
                    cPh_Distribucion_CCosto? distCCosto = await _context.Ph_Distribuciones_CCosto
                                    .Where(e => e.idccosto == item.IDCCOSTO && e.proyecto == item.PROYECTO && e.fase==item.FASE)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (distCCosto is not null)
                    {
                        IDDIST = distCCosto.idregistro;
                    }
                    else
                    {
                        cPh_Distribucion_CCosto dittNew = new cPh_Distribucion_CCosto
                        {
                            codigo = item.IDCCOSTO,
                            descripcion = item.IDCCOSTO,
                            idccosto = item.IDCCOSTO,
                            idregistro = 0,
                            proyecto = item.PROYECTO,
                            fase = item.FASE
                        };
                        _context.Add(dittNew);
                        await _context.SaveChangesAsync();
                        //obtener el id de distribucion a costo recién creado
                        IDDIST = await _context.Ph_Distribuciones_CCosto
                                    .Where(e => e.codigo== item.IDCCOSTO &&  e.idccosto == item.IDCCOSTO && e.proyecto==item.PROYECTO && e.fase==item.FASE)
                                    .Select(e => e.idregistro)
                                    .FirstOrDefaultAsync();
                    }


                    cMarcaDistribucionConcepto? marcaDC = await _context.Marcas_Distribuciones_Conceptos
                                    .Where(e => e.IDNUMERO == item.IDNUMERO && e.FECHA == item.FECHA && e.IDPLANILLA == item.IDPLANILLA && e.IDDIST== IDDIST && e.INICIO==item.INICIO)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (marcaDC is not null)
                    {
                        marcaDC.IDCCOSTO = item.IDCCOSTO;
                        marcaDC.INICIO = item.INICIO;
                        marcaDC.FIN = item.FIN;
                        marcaDC.ESTADO = 'A';
                        _context.Marcas_Distribuciones_Conceptos.Update(marcaDC);
                    }
                    else
                    {
                        item.IDREGISTRO = 0;
                        item.ESTADO = 'A';
                        item.IDDIST = IDDIST;

                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Sincronizar_MarcaDtnConcepto. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Sincronizar_MarcaDtnConcepto. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina_MarcaDtnConcepto:  Metodo borrado de datos de la tabla Marcas Distribucion Conceptos
        /// </summary>
        /// <param name="idregistro"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_MarcaDtnConcepto(long idregistro)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cMarcaDistribucionConcepto? model = await _context.Marcas_Distribuciones_Conceptos
                    .FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);

                if (model is not null)
                {
                    _context.Marcas_Distribuciones_Conceptos.Remove(model);
                    await _context.SaveChangesAsync();
                }
            }
                
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el registro de Marcas Distribuciones. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el registro de Marcas Distribuciones. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }


        /// <summary>
        /// GetTemplateHID: obtiene lista de TemplateHID (huellas de colaboradores HID)
        /// </summary>
        /// <returns>lista de TemplateHID</returns>
        public async Task<List<cTemplateHID>> GetTemplateHID()
        {
            List<cTemplateHID> model = new();
            try
            {
                model = await _context.TemplatesHID.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTemplateHID: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }
        /// <summary>
        /// GetNivel: obtiene una lista de registro de TemplateHID para un empleado especifico
        /// </summary>
        /// <param name="idnumero">id de color a recuperar</param>
        /// <returns></returns>
        public async Task<List<cTemplateHID>> GetTemplateHID(string idnumero)
        {
            List<cTemplateHID>? model = new();
            try
            {
                model = await _context.TemplatesHID.Where(e => e.IDNUMERO == idnumero).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTemplateHID: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }
        /// <summary>
        /// Sincronizar_TemplatesHID: metodo para sincronizar lista de TemplateHID
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>una instancia EventResponse con el resultado de los nivveles</returns>
        public async Task<EventResponse> Sincronizar_TemplateHID(IEnumerable<cTemplateHID> templates)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                //Task task = new Task(async () =>
                //{
                    var options = new FingerprintImageOptions() { Dpi = 512 };

                    foreach (var item in templates)
                    {
                        var image = new FingerprintImage(item.HID_TEMPLATE, options);
                        FingerprintTemplate template = new FingerprintTemplate(image);
                        byte[] serialized = template.ToByteArray();

                        cTemplateHID empleadoHID = new cTemplateHID
                        {
                            IDNUMERO = item.IDNUMERO!,
                            INDEXID = 0,
                            HID_TEMPLATE = serialized,
                        };

                        _context.Add(empleadoHID);

                    }
                    await _context.SaveChangesAsync();
                //});
                //task.Start();
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del template. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del template. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Sincronizar_TemplateHID: {respuesta.Descripcion}");
            }
            return respuesta;
        }
        /// <summary>
        /// Elimina_TemplateHID:  Metodo borrado de datos de la tabla Ph_Niveles para un empleado especifico
        /// </summary>
        /// <param name="IdNumero"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_TemplateHID(string IdNumero)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                List<cTemplateHID>? templates = await _context.TemplatesHID
                    .Where(e => e.IDNUMERO == IdNumero).ToListAsync();

                foreach (var item in templates)
                {
                    _context.TemplatesHID.Remove(item);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el TemplateHID. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el TemplateHID. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Verifica_TemplatesHID: metodo para verificar TemplateHID en lista de TemplateHID registrados
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>una instancia EventResponse con el resultado de los nivveles</returns>
        public async Task<EventResponseHID> Verifica_TemplateHID(IEnumerable<cTemplateHID> template)
        {
            FingerprintTemplate? vTemplate=null;
            EventResponseHID respuesta = new();
            try
            {
                foreach(var item in template)
                {
                    var options = new FingerprintImageOptions() { Dpi = 512 };
                    var image = new FingerprintImage(item.HID_TEMPLATE, options);
                    vTemplate = new FingerprintTemplate(image);
                    byte[] serialized = vTemplate.ToByteArray();
                    respuesta.Template = serialized;
                }
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la verificación del Template. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la verificación del Template. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Verifica_TemplateHID: {respuesta.Descripcion}");
            }
            return respuesta!;
        }


        /// <summary>
        /// GetTemplateFACE: obtiene lista de TemplateFACE (Rostros de colaboradores)
        /// </summary>
        /// <returns>lista de TemplateFACE</returns>
        public async Task<List<cTemplateFACE>> GetTemplateFACE()
        {
            List<cTemplateFACE> model = new();
            try
            {
                model = await _context.TemplatesFACES.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTemplateFACE: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }
        /// <summary>
        /// GetNivel: obtiene una lista de registro de TemplateFACE para un empleado especifico
        /// </summary>
        /// <param name="idnumero">id de empleado recuperar</param>
        /// <returns></returns>
        public async Task<List<cTemplateFACE>> GetTemplateFACE(string idnumero)
        {
            List<cTemplateFACE>? model = new();
            try
            {
                model = await _context.TemplatesFACES.Where(e => e.IDNUMERO == idnumero).ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetTemplateFACE: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }
        /// <summary>
        /// Sincronizar_TemplatesFACE: metodo para sincronizar lista de TemplateFACE
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>una instancia EventResponse con el resultado de los nivveles</returns>
        public async Task<EventResponse> Sincronizar_TemplateFACE(IEnumerable<cTemplateFACE> templates)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                foreach (var item in templates)
                {
                    cTemplateFACE empleadoFACE = new cTemplateFACE
                    {
                        IDNUMERO = item.IDNUMERO!,
                        INDEXID = 0,
                        FACETEMPLATE = item.FACETEMPLATE,
                    };

                    _context.Add(empleadoFACE);

                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del template. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del template. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Sincronizar_TemplateFACE: {respuesta.Descripcion}");
            }
            return respuesta;
        }
        /// <summary>
        /// Elimina_TemplateFACE:  Metodo borrado de datos de la tabla Ph_Niveles para un empleado especifico
        /// </summary>
        /// <param name="IdNumero"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_TemplateFACE(string IdNumero)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                List<cTemplateFACE>? templates = await _context.TemplatesFACES
                    .Where(e => e.IDNUMERO == IdNumero).ToListAsync();

                foreach (var item in templates)
                {
                    _context.TemplatesFACES.Remove(item);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el TemplateFACE. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el TemplateFACE. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }


        /// <summary>
        /// GetPhPuesto: Obtener lista de puestos de colaboradores
        /// </summary>
        /// <returns></returns>
        public async Task<List<cPh_Puesto>> GetPhPuesto()
        {
            List<cPh_Puesto> model = new();
            try
            {
                model = await _context.Ph_Puestos.ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhPuesto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }

        /// <summary>
        /// GetPhPuesto: Obtener un puesto especifico de colaborador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<cPh_Puesto> GetPhPuesto(string id)
        {
            cPh_Puesto? model = new();
            try
            {
                model = await _context.Ph_Puestos.FirstOrDefaultAsync(e => e.Puesto == id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhPuesto: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }

        #endregion

        #region SPMetodos
        //Creado por: Marlon Loria Solano
        //Fecha: 2022-01-02
        /// <summary>
        /// /Obtener lista de Compañias asociadas al usuario
        /// </summary>
        /// <returns>Lista de Compania de Usuario </returns>
        public async Task<List<cPh_CompaniaUsuario>> GetPhCompaniaUsuario(string idnumero)
        {
            List<cPh_CompaniaUsuario> companiasUsuario = new();
            DataTable table;

            try
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                string schemaAdmin = config.GetConnectionString("SchemaAdmin");

                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        
                        command.CommandText = $"{schemaAdmin}.VerificaCompaniaUsuarioWeb @IdNumero='{idnumero}',@DataBase='{Encripta.getEncryptTripleDES(_dataBase)}'";
                        _logger.LogError($"GeoTimeConnectService.GetPhCompaniaUsuario: Data: {command.CommandText}");
                        System.Data.Common.DbDataReader result = command.ExecuteReader();

                        table = new DataTable();
                        table.Load(result);

                        foreach (DataRow dr in table.Rows)
                        {
                            cPh_CompaniaUsuario usrComp = new cPh_CompaniaUsuario
                            {
                                idcomp = dr.ItemArray[0].ToString(),
                                compania = dr.ItemArray[1].ToString(),
                                nom_conector = dr.ItemArray[2].ToString(),
                                idnumero = dr.ItemArray[3].ToString(),
                            };
                            companiasUsuario.Add(usrComp);
                        }

                        // Close the reader
                        result.Close();

                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhCompaniaUsuario: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return companiasUsuario;
        }

        /// <summary>
        /// VerificaUsuariosActivosPMW:  Verifica cantidad de usuarios habilitados en la base de datos para el portal de marcas web
        /// </summary>
        /// <returns>cantidad de usuarios habilitados para el portal de marcas web</returns>
        public async Task<int> VerificaUsuariosActivosPMW()
        {

            DataTable table;
            int respuesta = 0;
            try
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                string schemaAdmin = config.GetConnectionString("SchemaAdmin");

                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"{schemaAdmin}.VerificaDatosPMW @Name='{Encripta.getEncryptTripleDES(connection.Database)}',@Object='Portal_Empleado', @field ='idnumero',@filter='habilitado=1' ";
                        System.Data.Common.DbDataReader result = command.ExecuteReader();

                        table = new DataTable();
                        table.Load(result);
                        result.Close();
                        respuesta = table.Rows.Count;

                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.VerificaUsuariosActivosPMW: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return respuesta;
        }

        public async Task EjecutaPostCambioPlanilla(string idnumero, string oldPlanilla, string newPlanilla)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + $".DM_POST_CAMBIOPLANILLA @idnumero='{idnumero}', @OLDPLANILLA='{oldPlanilla}',@NEWPLANILLA='{newPlanilla}'";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.EjecutaPostCambioPlanilla: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }

        }

        // Metodo para Activar el periodo 
        //Creado por: Allan Prieto 
        //Fecha: 2024-2-5
        // Ejecutar el procedimiento almacenado apertura periodo
        public async Task<EventResponse> ActivarPeriodoPAAsync(cActivarPeriodo parametros)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + ".apertura_periodo";
                        command.CommandType = CommandType.StoredProcedure;

                        // Parametros necesarios 
                        command.Parameters.Add(new SqlParameter("@IDPLANILLA", SqlDbType.VarChar) { Value = parametros.IdPlanilla });
                        command.Parameters.Add(new SqlParameter("@GRUPO", SqlDbType.Int) { Value = parametros.Grupo });
                        command.Parameters.Add(new SqlParameter("@PERIODO", SqlDbType.VarChar) { Value = parametros.Periodo });
                        command.Parameters.Add(new SqlParameter("@INICIO", SqlDbType.DateTime) { Value = parametros.Inicio });
                        command.Parameters.Add(new SqlParameter("@FIN", SqlDbType.DateTime) { Value = parametros.Fin });
                        command.Parameters.Add(new SqlParameter("@USUARIO", SqlDbType.Int) { Value = parametros.Usuario });

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.ActivarPeriodoPAAsync: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "Problemas al Abrir el periodo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "Problemas al Abrir el periodo. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        public async Task<EventResponse> CierroPeriodo(cCierroPeriodo parametros)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + ".cierro_periodo";
                        command.CommandType = CommandType.StoredProcedure;

                        // Parametros necesarios 
                        command.Parameters.Add(new SqlParameter("@IDPLANILLA", SqlDbType.VarChar) { Value = parametros.IdPlanilla });
                        command.Parameters.Add(new SqlParameter("@GRUPO", SqlDbType.VarChar) { Value = parametros.Grupo });
                        command.Parameters.Add(new SqlParameter("@PERIODO", SqlDbType.VarChar) { Value = parametros.Periodo });
                        command.Parameters.Add(new SqlParameter("@INICIO", SqlDbType.DateTime) { Value = parametros.Inicio });
                        command.Parameters.Add(new SqlParameter("@FIN", SqlDbType.DateTime) { Value = parametros.Fin });
                        command.Parameters.Add(new SqlParameter("@USUARIO", SqlDbType.Int) { Value = parametros.Usuario });
                        command.Parameters.Add(new SqlParameter("@EST_M", SqlDbType.Char) { Value = parametros.Est_M });

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.CierroPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "Problemas al Cerrar el periodo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "Problemas al Cerrar el periodo. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        public async Task EjecutaAplicaAccionPersonal(long idregistro)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + ".aplico_accpersonal @IDREGISTRO=" + idregistro;
                        System.Data.Common.DbDataReader result = command.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.EjecutaAplicaAccionPersonal: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// EjecutaInMarcasWeb: Método que ejecuta procedimiento almacenado IN_MARCAS_WEB, necesario para completar el registro de marca en Geotime
        /// </summary>
        /// <param name="idnumero">idnumero del empleado que realiza la marca</param>
        public async Task EjecutaInMarcasWeb(string idnumero)
        {
            try
            {
                var result = _context.Database.ExecuteSqlRaw($"exec {_schema}.IN_MARCAS_WEB @idnumero='{idnumero}'");

                //using (var connection = _context.Database.GetDbConnection())
                //{
                //    await connection.OpenAsync();
                //    using (var command = connection.CreateCommand())
                //    {
                //        command.CommandText = _schema + $".IN_MARCAS_WEB @idnumero='{idnumero}'";
                //        await command.ExecuteNonQueryAsync();
                //    }
                //}
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.EjecutaInMarcasWeb: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }

        }

        /// <summary>
        /// ConsultarMarcasPeriodo:  Proceso paradeterminar marcas del periodo que se deben visualizar en el sistema
        /// </summary>
        /// <param name="IdsGrupos">Listado de grupos separados por coma , que por los que se debe filtrar la informacion</param>
        /// <param name="IdPlanilla">id de planilla por el que se debe filtrar la informacion</param>
        /// <param name="FechaInicio">fecha de inicio del reporte</param>
        /// <param name="FechaFin">fecha final del reporte</param>
        /// <param name="idnumero">id del empleado que se desea obtener, si se envia un -1 trae todos los empleados</param>
        /// <returns>Lista de marcas del periodo</returns>
        public async Task<IEnumerable<cMarcaPeriodo>> GetMarcasPeriodo(string IdsGrupos, string IdPlanilla, string FechaInicio, string FechaFin, string idnumero)
        {
            List<cMarcaPeriodo>? marcasPeriodo = new(); 
            try
            {
                string fechaInicioExt = $"{FechaInicio.Substring(0, 4)}-{FechaInicio.Substring(4, 2)}-{FechaInicio.Substring(6, 2)}";
                string fechaFinExt = $"{FechaFin.Substring(0, 4)}-{FechaFin.Substring(4, 2)}-{FechaFin.Substring(6, 2)}";
                IdsGrupos = HttpUtility.UrlDecode(IdsGrupos);
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + $".DM_CONSULTAR_MARCAS_PERIODO @IdsGrupos='{IdsGrupos}', @IdPlanilla='{IdPlanilla}', @FechaInicio='{fechaInicioExt}', @FechaFin='{fechaFinExt}', @idnumero='{idnumero}',@IdComp='{_schema}'";

                        using (var reader = command.ExecuteReader())
                            return reader.Cast<IDataRecord>()
                                .Select(r => new cMarcaPeriodo
                                {
                                    idnumero = r.GetString(r.GetOrdinal("idnumero")),
                                    nombre = r.GetString(r.GetOrdinal("nombre")),
                                    fecha_entra = r.GetDateTime(r.GetOrdinal("fecha_entra")),
                                    fecha_sale = r.GetDateTime(r.GetOrdinal("fecha_sale")),
                                    hora_entra = r.GetString(r.GetOrdinal("hora_entra")),
                                    hora_sale = r.GetString(r.GetOrdinal("hora_sale")),
                                    idturno = r.GetInt32(r.GetOrdinal("idturno")),
                                    ordinario = r.GetDecimal(r.GetOrdinal("ordinario")),
                                    extras = r.GetDecimal(r.GetOrdinal("extras")),
                                    suma_extras = r.GetDecimal(r.GetOrdinal("suma_extras")),
                                    suma_dobles = r.GetDecimal(r.GetOrdinal("suma_dobles")),
                                    suma_otros = r.GetDecimal(r.GetOrdinal("suma_otros")),
                                    turno = r.GetString(r.GetOrdinal("turno")),
                                    estado = r.GetString(r.GetOrdinal("estado")),
                                    mtardia = r.GetString(r.GetOrdinal("mtardia")),
                                    manticipo = r.GetString(r.GetOrdinal("manticipo")),
                                    reg_sale =  r.GetInt64(r.GetOrdinal("reg_sale")),
                                    idregistro = r.GetInt64(r.GetOrdinal("idregistro")),
                                    fecha_ingreso = r.GetDateTime(r.GetOrdinal("fecha_ingreso")),
                                    iddepartamento = r.GetString(r.GetOrdinal("iddepartamento")),
                                }).ToList();
            
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.ConsultarMarcasPeriodo: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
                
            }
            
        }

        /// <summary>
        /// AutorizarExtrasPeriodo: proceso para autorizar extras del periodo de un empleado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<EventResponse> AutorizarExtrasPeriodo(cExtraAprobacion parametros)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                string fechaInicioExt = $"{parametros.Inicio.Substring(0, 4)}-{parametros.Inicio.Substring(4, 2)}-{parametros.Inicio.Substring(6, 2)}";
                string fechaFinExt = $"{parametros.Fin.Substring(0, 4)}-{parametros.Fin.Substring(4, 2)}-{parametros.Fin.Substring(6, 2)}";

                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + $".apruebo_extra_periodo @PLANILLA='{parametros.IdPlanilla}', @IDNUMERO='{parametros.IdNumero}', @COMENTARIO='{parametros.Comentario}', @INICIO='{fechaInicioExt}',@FIN='{fechaFinExt}',@USUARIO='{parametros.Usuario}'";
                        System.Data.Common.DbDataReader result = command.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la autorización de las horas extra. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la autorización de las horas extra. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        /// <summary>
        /// AutorizarExtrasMasiva: proceso para autorizar extras de varios empleados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>

        public async Task<EventResponse> AutorizarExtrasMasiva(IEnumerable<cExtraAprobacion> parametros)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                int maxConcurrency = 5; // Cambia este valor según tus necesidades
                using var semaphore = new SemaphoreSlim(maxConcurrency);

                var tasks = parametros.Select(async param =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        string commandString = _schema + $".apruebo_extra_masiva @REGISTRO={param.IdRegistro}, @CANTIDAD='{param.Cantidad}', @COMENTARIO='{param.Comentario}',@USUARIO='{param.Usuario}'";
                        await _context.Database.ExecuteSqlRawAsync(commandString);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error procesando parámetro {param.IdNumero}: {ex.Message}");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(tasks);
               
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "AutorizarExtrasMasiva: Ocurrió un error al autorizar las horas extra. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "AutorizarExtrasMasiva: Ocurrió un error al autorizar las horas extra. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        /// <summary>
        /// PreAutorizarExtrasMasiva: proceso para autorizar extras de varios empleados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<EventResponse> PreAutorizarExtrasMasiva(IEnumerable<cExtraAprobacion> parametros)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                int maxConcurrency = 5; // Cambia este valor según tus necesidades
                using var semaphore = new SemaphoreSlim(maxConcurrency);

                var tasks = parametros.Select(async param =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        string fecha = $"{param.Inicio!.Substring(0, 4)}-{param.Inicio.Substring(4, 2)}-{param.Inicio.Substring(6, 2)}";
                        string commandString = _schema + $".apruebo_preextra_masiva @PLANILLA={param.IdPlanilla},@IDNUMERO='{param.IdNumero}',@FECHA='{fecha}',@CANTIDAD='{param.Cantidad}', @COMENTARIO='{param.Comentario}',@USUARIO='{param.Usuario}'";
                        await _context.Database.ExecuteSqlRawAsync(commandString);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error procesando parámetro {param.IdNumero}: {ex.Message}");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(tasks);



            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "PreAutorizarExtrasMasiva: Ocurrió un error al autorizar las horas extra. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "PreAutorizarExtrasMasiva: Ocurrió un error al autorizar las horas extra. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        /// <summary>
        /// AutorizarExtras: proceso para autorizar un registro de extras
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<EventResponse> AutorizarExtras(cExtraAprobacion parametros)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + $".apruebo_extra @CANTIDAD='{parametros.Cantidad}',@COMENTARIO='{parametros.Comentario}',@USUARIO='{parametros.Usuario}', @IDREGISTRO={parametros.IdRegistro},@IDCCOSTO='{parametros.IdCCosto}'";
                        System.Data.Common.DbDataReader result = command.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la Sincronización de ERP. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Sincronización de ERP. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        public async Task<List<cPh_CatalogoGenerico>> GetPhCatalogoGenerico()
        {


            List<cPh_CatalogoGenerico> catalogoGenerico = new();
            try
            {
                catalogoGenerico = await _context.Ph_Catalogo_Generico
                        .ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhCatalogoGenerico: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return catalogoGenerico;
        }

        public async Task<List<cPh_CatalogoGenerico>> GetPhCatalogoGenerico(string nombre)
        {


            List<cPh_CatalogoGenerico> catalogoGenerico = new();
            try
            {
                catalogoGenerico = await _context.Ph_Catalogo_Generico.Where(e=>e.NombreCatalogo== nombre)
                        .ToListAsync();
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhCatalogoGenerico: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return catalogoGenerico;
        }

        public async Task<cPh_CatalogoGenerico> GetPhCatalogoGenerico(string nombre, string id)
        {


            cPh_CatalogoGenerico? catalogoGenerico = new();
            try
            {
                catalogoGenerico = await _context.Ph_Catalogo_Generico.FirstOrDefaultAsync(e => e.NombreCatalogo == nombre && e.Id==id);
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhCatalogoGenerico: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return catalogoGenerico!;
        }


        #endregion

        #region WSMetodos
        public async Task<EventResponse> Sincronizo_erp(IEnumerable<cSincronizo_erp> parametros)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in parametros)
                {
                    sincronizo_erpRequest sincronizoErp = new sincronizo_erpRequest
                    {
                        comp = item.IdComp,
                        plan = item.IdPlanilla,
                    };

                    EndpointConfiguration endpointConfiguration = new();
                    GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                    var result = await geoWebService.sincronizo_erpAsync(sincronizoErp);
                    if (result.sincronizo_erpResult == "")
                    {
                        respuesta.Id = "0";
                        respuesta.Respuesta = "Ok";
                        respuesta.Descripcion = $"Respuesta: {result.sincronizo_erpResult}";
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la Sincronización de ERP. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Sincronización de ERP. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }
        public async Task<EventResponse> Sincronizo_Acciones(IEnumerable<cSincronizo_Acciones> parametros)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var phloginAdmin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.usuario.ToUpper() == "Admin");
                if (phloginAdmin is not null)
                {
                    phloginAdmin.ultimo_login = DateTime.Now;
                    phloginAdmin.ultimo_estado = DateTime.Now;
                    phloginAdmin.idsesion = 1;
                    _context.PH_LOGIN.Update(phloginAdmin);
                    await _context.SaveChangesAsync();

                }

                foreach (var item in parametros)
                {

                    sincronizo_accionesRequest sincronizoAcciones = new sincronizo_accionesRequest
                    {
                        comp = item.IdComp,
                        plan = item.IdPlanilla,
                        inicio = item.inicio,
                        fin = item.fin,
                        //inicio = fechaInicial.ToString("yyyy-MM-dd"),
                        //fin = fechaFinal.ToString("yyyy-MM-dd"),
                        sesion = phloginAdmin.idsesion.ToString(),
                    };

                    EndpointConfiguration endpointConfiguration = new();
                    GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                    var result = await geoWebService.sincronizo_accionesAsync(sincronizoAcciones);
                    if (result.sincronizo_accionesResult == "")
                    {
                        respuesta.Id = "0";
                        respuesta.Respuesta = "Ok";
                        respuesta.Descripcion = $"Respuesta: {result.sincronizo_accionesResult}";
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la Sincronización de Acciones. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Sincronización de Acciones. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-03-12
        /// <summary>
        /// EjecutaInitPeriodo: Ejecuta WS de Init_Periodo
        /// </summary>
        /// <param name="parametros">Ejecuta el Web Service</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public async Task<EventResponse> Init_Periodo(IEnumerable<cInit_Periodo> parametros)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in parametros)
                {
                    init_periodoRequest initPeriodo = new init_periodoRequest
                    {
                        comp = item.IdComp,
                        periodo = item.IdPeriodo,
                        plan = item.IdPlanilla,
                    };

                    EndpointConfiguration endpointConfiguration = new();
                    GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                    var result = await geoWebService.init_periodoAsync(initPeriodo);
                    if (result.init_periodoResult != "")
                    {
                        respuesta.Id = "0";
                        respuesta.Respuesta = "Ok";
                        respuesta.Descripcion = $"Respuesta: {result.init_periodoResult}";
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la Activación del Periodo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Activación del Periodo. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }
        public async Task<EventResponse> Cal_Periodo_Planilla(IEnumerable<cCal_Periodo_Planilla> parametros)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var phloginAdmin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.usuario.ToUpper() == "ADMIN");
                if (phloginAdmin is not null)
                {
                    phloginAdmin.ultimo_login = DateTime.Now;
                    phloginAdmin.ultimo_estado = DateTime.Now;
                    phloginAdmin.idsesion = 1;
                    _context.PH_LOGIN.Update(phloginAdmin);
                    await _context.SaveChangesAsync();

                }

                foreach (var item in parametros)
                {
                    calculo_periodo_planillaRequest CalculoPariodoP = new calculo_periodo_planillaRequest
                    {
                        comp = item.IdComp,
                        periodo = item.IdPeriodo,
                        plan = item.IdPlanilla,
                        inicio = item.Inicio,
                        fin = item.Fin,
                        //inicio = fechaInicial.ToString("yyyy-MM-dd"),
                        //fin = fechaFinal.ToString("yyyy-MM-dd"),
                        grupo = item.Grupo,
                        sesion = (int)phloginAdmin.idsesion, //Verificar que se envie 
                        idpais = item.IdPais,
                    };

                    EndpointConfiguration endpointConfiguration = new();
                    ServiceSoapClient geoWebService = new(endpointConfiguration);

                    var result = await geoWebService.calculo_periodo_planillaAsync(CalculoPariodoP);
                    if (result.calculo_periodo_planillaResult != "")
                    {
                        respuesta.Id = "0";
                        respuesta.Respuesta = "Ok";
                        respuesta.Descripcion = $"Respuesta: {result.calculo_periodo_planillaResult}";
                    }
                }
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el Calculo Periodo Planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el Calculo Periodo Planilla. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2024-06-28
        /// <summary>
        /// PutMarcasProceso: Recibe una lista de MarcaMovTurno y a partir de ella realiza la actualizacion del turno en marcas Proceso
        /// </summary>
        /// <param name="marcasMovTurnos">lista de MarcaMovTurno</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public async Task<EventResponse> PutMarcasProceso(IEnumerable<cMarcaMovTurno> marcasMovTurnos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                List<cMarcaProceso> marcasProcesoInicializada = new();

                foreach (var item in marcasMovTurnos)
                {
                    var horario = item.hora.Split("|");
                    var turno = await _context.Ph_Turnos.FirstOrDefaultAsync(e => e.IdTurno == item.turno);
                    var fechaSalida = turno!.HEntra!.CompareTo(turno.HSale) > 0 ? item.fecha.AddDays(1) : item.fecha;

                    cMarcaProceso? marcaProceso = new cMarcaProceso
                    {
                        idregistro = 0,
                        idplanilla = item.idplanilla,
                        idnumero = item.idnumero,
                        fecha_entra = item.fecha,
                        fecha_sale = fechaSalida,
                        hora_entra = "00:00",
                        hora_sale = "00:00",
                        idturno = item.turno,
                        CON_1 = 1,
                        CON_2 = 1,
                        CON_3 = 1,
                        CON_4 = 1,
                        CON_5 = 1,
                    };

                    var marcasProceso = await _context.Marcas_Proceso
                                    .Where(e => e.idnumero == item.idnumero
                                            && e.fecha_entra == item.fecha)
                                    .ToListAsync();

                    if (marcasProceso.Count()>0)
                    {
                        foreach(var mp in marcasProceso)
                        {
                            if (mp.idturno != item.turno)
                            {
                                mp.idturno = item.turno;
                                mp.fecha_sale = fechaSalida;
                                _context.Marcas_Proceso.Update(mp);
                            }
                        }                        
                    }
                    else
                    {
                        _context.Add(marcaProceso);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
               string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de Marcas Proceso. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de Marcas Proceso. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        // Creado por: Allan Prieto Badilla
        /// <summary>
        /// Obtener_Conceptos: Obtiene la lista de conceptos de la compañia
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="sesion"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cObtengoConcepto>> Obtener_Conceptos(string compania, string sesion)
        {
            List<cObtengoConcepto>? listaConceptos = new();

            try
            {
                obtengo_conceptosRequest obtengoConcepto = new obtengo_conceptosRequest
                {
                    comp = compania,
                    sesion = sesion,
                };
                
                EndpointConfiguration endpointConfiguration = new();
                GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                var result = await geoWebService.obtengo_conceptosAsync(obtengoConcepto);
                string resultString = result.obtengo_conceptosResult;

                if (!string.IsNullOrEmpty(resultString))
                {
                    // Conversión de la respuesta JSON a la lista de objetos cObtengoConcepto
                    listaConceptos = JsonSerializer.Deserialize<List<cObtengoConcepto>>(resultString);
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.Obtener_Conceptos: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return listaConceptos;
        }

        // creando por Allan Prieto
        /// <summary>
        /// Obtener_TipoAccion: Obtiene la lista de tipos de acciones de la compañia
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="sesion"></param>
        /// <returns></returns>
        public async Task<IEnumerable<cObtengoTipoAccion>> Obtener_TipoAccion(string compania, string sesion)
        {
            List<cObtengoTipoAccion>? listaTipoAccion = new();

            try
            {
                obtengo_tipoaccionRequest obtengoTipoAccion = new obtengo_tipoaccionRequest
                {
                    comp = compania,
                    sesion = sesion,
                };

                EndpointConfiguration endpointConfiguration = new();
                GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                var result = await geoWebService.obtengo_tipoaccionAsync(obtengoTipoAccion);
                string resultString = result.obtengo_tipoaccionResult;

                if (!string.IsNullOrEmpty(resultString))
                {
                    // Conversión de la respuesta JSON a la lista de objetos cObtengoConcepto
                    listaTipoAccion = JsonSerializer.Deserialize<List<cObtengoTipoAccion>>(resultString);
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.obtengo_tipoaccionRequest: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return listaTipoAccion;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2025-02-01
        /// <summary>
        /// EjecutaInitPeriodo: Ejecuta WS de Exporto_Concepto
        /// </summary>
        /// <param name="parametros">Ejecuta el Web Service</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public async Task<EventResponse> Exporto_Concepto(IEnumerable<cExporto_Concepto> parametros)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in parametros)
                {
                    exporto_conceptosRequest exportoConceptos = new exporto_conceptosRequest
                    {
                        comp = item.IdComp,
                        periodo = item.IdPeriodo,
                        plan = item.IdPlanilla,
                        hora_labora = item.IdHoraLaboral,
                        sesion = item.Sesion,

                    };

                    EndpointConfiguration endpointConfiguration = new();
                    GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                    var result = await geoWebService.exporto_conceptosAsync(exportoConceptos);
                    if (result.exporto_conceptosResult != "")
                    {
                        respuesta.Id = "0";
                        respuesta.Respuesta = "Ok";
                        respuesta.Descripcion = $"Respuesta: {result.exporto_conceptosResult}";
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la Activación del Periodo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Activación del Periodo. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        public async Task<EventResponse> EvaluaFormula(cPh_Formulacion formula)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                    EndpointConfiguration endpointConfiguration = new();
                    GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                evaluo_formulaRequest evaluaFormula = new evaluo_formulaRequest
                {
                    formula = formula.FORMULA,
                };

                var result = await geoWebService.evaluo_formulaAsync(evaluaFormula);
                    if (result.evaluo_formulaResult != "")
                    {
                        respuesta.Id = "0";
                        respuesta.Respuesta = "Ok";
                        respuesta.Descripcion = $"Respuesta: {result.evaluo_formulaResult}";
                        respuesta.ValorRetorno = result.evaluo_formulaResult;
                }
                
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la Evaluación de la Fórmula. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la Evaluación de la Fórmula. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }


        public async Task<string> GetNivelesAutorizacion(string encriptado)
        {
            string respuesta = "";

            try
            {
                EndpointConfiguration endpointConfiguration = new();
                GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

                retorno_perfilRequest retorno_perfil = new retorno_perfilRequest
                {
                    datos = encriptado,
                    idsesion = 1
                };

                var result = await geoWebService.retorno_perfilAsync(retorno_perfil);
                if (result.retorno_perfilResult != "")
                {
                    respuesta= result.retorno_perfilResult;
                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
            }
            return respuesta;

        }


        #endregion


        #region Menus, Opciones, Roles de Usuario de Control de Asistencia

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhMenuSistema: Obtener lista de menus de sistema 
        /// </summary>
        /// <returns>Lista de lista de menus del sistema</returns>
        public async Task<List<cPh_MenuSistema>> GetPhMenuSistema()
        {
            List<cPh_MenuSistema>? portalMenu = new();

            try
            {
                portalMenu = (from e in await _context.Ph_Menus_Sistema
                                .Include(e => e.cPh_OpcionSistema)
                            .ToListAsync()
                              select new cPh_MenuSistema
                              {
                                  ID = e.ID,
                                  MENUTEXT = e.MENUTEXT,
                                  ICONID = e.ICONID,
                                  cPh_OpcionSistema = e.cPh_OpcionSistema == null ? null :
                                                  (from po in e.cPh_OpcionSistema
                                                   select new cPh_OpcionSistema
                                                   {
                                                       PARENTID = po.PARENTID,
                                                       ID = po.ID,
                                                       MENUTEXT = po.MENUTEXT,
                                                       ICONID = po.ICONID,
                                                       PRINCIPAL = po.PRINCIPAL,
                                                       HREF = po.HREF,
                                                   }).ToList()
                              }
                            ).ToList();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhMenuSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalMenu;
        }
        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhMenuSistema: Obtener datos de una opcion de menu de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPh_MenuSistema </returns>
        public async Task<cPh_MenuSistema> GetPhMenuSistema(string id)
        {
            cPh_MenuSistema? portalMenu = new();

            try
            {

                portalMenu = (from e in await _context.Ph_Menus_Sistema.Where(e => e.ID == id)
                                .Include(e => e.cPh_OpcionSistema)
                            .ToListAsync()
                              select new cPh_MenuSistema
                              {
                                  ID = e.ID,
                                  MENUTEXT = e.MENUTEXT,
                                  ICONID = e.ICONID,
                                  cPh_OpcionSistema = e.cPh_OpcionSistema == null ? null :
                                                  (from po in e.cPh_OpcionSistema
                                                   select new cPh_OpcionSistema
                                                   {
                                                       PARENTID = po.PARENTID,
                                                       ID = po.ID,
                                                       MENUTEXT = po.MENUTEXT,
                                                       ICONID = po.ICONID,
                                                       PRINCIPAL = po.PRINCIPAL,
                                                       HREF = po.HREF,
                                                   }).ToList()
                              }
                            ).FirstOrDefault();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhMenuSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalMenu;
        }

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// Sincronizar_PortalMenu: Método para registrar los menus del sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Menu </param>
        public async Task<EventResponse> Sincronizar_PhMenuSistema(IEnumerable<cPh_MenuSistema> portalMenu)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in portalMenu)
                {
                    cPh_MenuSistema? portalMenuBuscar = await _context.Ph_Menus_Sistema
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalMenuBuscar is not null)
                    {
                        portalMenuBuscar.ICONID = item.ICONID;
                        portalMenuBuscar.MENUTEXT = item.MENUTEXT;

                        _context.Ph_Menus_Sistema.Update(portalMenuBuscar);
                    }
                    else
                    {
                        item.cPh_OpcionSistema = null;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro del menú. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro del menú. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetPh_OpcionSistema: Obtener lista de opciones del menu de CA 
        /// </summary>
        /// <returns>Lista de Opciones del sistema</returns>
        public async Task<List<cPh_OpcionSistema>> GetPh_OpcionSistema()
        {
            List<cPh_OpcionSistema>? portalOpcion = new();

            try
            {
                portalOpcion = (from e in await _context.Ph_Opciones_Sistema
                                .Include(e => e.cPh_MenuSistema)
                                .ToListAsync()
                                select new cPh_OpcionSistema
                                {
                                    PARENTID = e.PARENTID,
                                    ID = e.ID,
                                    MENUTEXT = e.MENUTEXT,
                                    ICONID = e.ICONID,
                                    PRINCIPAL = e.PRINCIPAL,
                                    HREF = e.HREF,
                                    cPh_MenuSistema = e.cPh_MenuSistema == null ? null :
                                                    new cPh_MenuSistema
                                                    {
                                                        ID = e.cPh_MenuSistema.ID,
                                                        MENUTEXT = e.cPh_MenuSistema.MENUTEXT,
                                                        ICONID = e.cPh_MenuSistema.ICONID,
                                                    },
                                }
                           ).ToList();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPh_OpcionSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalOpcion;
        }

        /// <summary>
        /// GetPortalOpcion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public async Task<cPh_OpcionSistema> GetPh_OpcionSistema(string id)
        {
            cPh_OpcionSistema? portalOpcion = new();

            try
            {

                portalOpcion = (from e in await _context.Ph_Opciones_Sistema.Where(e => e.ID == id)
                                .Include(e => e.cPh_MenuSistema)
                                .ToListAsync()
                                select new cPh_OpcionSistema
                                {
                                    PARENTID = e.PARENTID,
                                    ID = e.ID,
                                    MENUTEXT = e.MENUTEXT,
                                    ICONID = e.ICONID,
                                    PRINCIPAL = e.PRINCIPAL,
                                    HREF = e.HREF,
                                    cPh_MenuSistema = e.cPh_MenuSistema == null ? null :
                                                    new cPh_MenuSistema
                                                    {
                                                        ID = e.cPh_MenuSistema.ID,
                                                        MENUTEXT = e.cPh_MenuSistema.MENUTEXT,
                                                        ICONID = e.cPh_MenuSistema.ICONID,
                                                    },
                                }
                           ).FirstOrDefault();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPh_OpcionSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return portalOpcion;
        }


        /// <summary>
        /// Sincronizar_PortalOpcion: Método para registrar las opciones del sistema Portal de empleados
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Opcion </param>
        public async Task<EventResponse> Sincronizar_PhOpcionSistema(IEnumerable<cPh_OpcionSistema> portalOpcion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in portalOpcion)
                {
                    cPh_OpcionSistema? portalOpcionBuscar = await _context.Ph_Opciones_Sistema
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalOpcionBuscar is not null)
                    {
                        portalOpcionBuscar.HREF = item.HREF;
                        portalOpcionBuscar.ICONID = item.ICONID;
                        portalOpcionBuscar.PRINCIPAL = item.PRINCIPAL;
                        portalOpcionBuscar.MENUTEXT = item.MENUTEXT;
                        portalOpcionBuscar.PARENTID = item.PARENTID;

                        _context.Ph_Opciones_Sistema.Update(portalOpcionBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la opción de menú. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la opción de menú. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistema: Obtener lista de Roles de sistema 
        /// </summary>
        /// <returns>Lista de lista de menus del sistema</returns>
        public async Task<List<cPh_RolSistema>> GetPhRolSistema()
        {
            List<cPh_RolSistema>? model = new();

            try
            {
                model = (from e in await _context.Ph_Roles_Sistema
                                .Include(e => e.cPh_RolSistemaDet)
                            .ToListAsync()
                              select new cPh_RolSistema
                              {
                                  ID = e.ID,
                                  DESCRIPCION = e.DESCRIPCION,
                                  HABILITADO = e.HABILITADO,
                                  IDCOMP = e.IDCOMP,
                                  IDNIVEL = e.IDNIVEL,
                                  cPh_RolSistemaDet = e.cPh_RolSistemaDet == null ? null :
                                                  (from det in e.cPh_RolSistemaDet
                                                   select new cPh_RolSistemaDet
                                                   {
                                                       ROLSISTEMAID = det.ROLSISTEMAID,
                                                       MENUSISTEMAID = det.MENUSISTEMAID,
                                                       OPCIONSISTEMAID = det.OPCIONSISTEMAID,
                                                       HABILITADO = det.HABILITADO,
                                                       AGREGA = det.AGREGA,
                                                       MODIFICA = det.MODIFICA,
                                                       ELIMINA = det.ELIMINA,
                                                       CONSULTA = det.CONSULTA,
                                                   }).ToList()
                              }
                            ).ToList();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhRolSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }
        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistema: Obtener datos de un rol de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPh_RolSistema </returns>
        public async Task<cPh_RolSistema> GetPhRolSistema(string id)
        {
            cPh_RolSistema? model = new();

            try
            {

                model = (from e in await _context.Ph_Roles_Sistema.Where(e => e.ID == id)
                                .Include(e => e.cPh_RolSistemaDet)
                            .ToListAsync()
                              select new cPh_RolSistema
                              {
                                  ID = e.ID,
                                  DESCRIPCION = e.DESCRIPCION,
                                  HABILITADO = e.HABILITADO,
                                  IDCOMP = e.IDCOMP,
                                  IDNIVEL = e.IDNIVEL,
                                  cPh_RolSistemaDet = e.cPh_RolSistemaDet == null ? null :
                                                  (from det in e.cPh_RolSistemaDet
                                                   select new cPh_RolSistemaDet
                                                   {
                                                       ROLSISTEMAID = det.ROLSISTEMAID,
                                                       MENUSISTEMAID = det.MENUSISTEMAID,
                                                       OPCIONSISTEMAID = det.OPCIONSISTEMAID,
                                                       HABILITADO = det.HABILITADO,
                                                       AGREGA = det.AGREGA,
                                                       MODIFICA = det.MODIFICA,
                                                       ELIMINA = det.ELIMINA,
                                                       CONSULTA = det.CONSULTA,
                                                   }).ToList()
                              }
                            ).FirstOrDefault();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhMenuSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model!;
        }

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// Sincronizar_PhRolSistema: Método para registrar los roles del sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="roles">Lista de registros de cPh_RolSistema </param>
        public async Task<EventResponse> Sincronizar_PhRolSistema(IEnumerable<cPh_RolSistema> roles)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                List<cPh_RolSistemaDet>? rolesDet;
                foreach (var item in roles)
                {
                    rolesDet = item.cPh_RolSistemaDet!.ToList();
                    cPh_RolSistema? objetoBuscar = await _context.Ph_Roles_Sistema
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.HABILITADO = item.HABILITADO;
                        objetoBuscar.IDCOMP = item.IDCOMP;
                        objetoBuscar.IDNIVEL = item.IDNIVEL;

                        _context.Ph_Roles_Sistema.Update(objetoBuscar);
                    }
                    else
                    {
                        item.cPh_RolSistemaDet = null;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                    foreach (var roldet in rolesDet)
                    {
                        cPh_RolSistemaDet? rolSistemaDet = await _context.Ph_Roles_SistemaDet
                                    .Where(e => e.ROLSISTEMAID == roldet.ROLSISTEMAID
                                             && e.MENUSISTEMAID == roldet.MENUSISTEMAID
                                             && e.OPCIONSISTEMAID == roldet.OPCIONSISTEMAID)
                                    .FirstOrDefaultAsync();
                        //si la opcion existe se actualiza 
                        //de lo contrario se agrega el registro
                        if (rolSistemaDet is not null)
                        {
                            rolSistemaDet.HABILITADO = roldet.HABILITADO;
                            rolSistemaDet.AGREGA = roldet.AGREGA;
                            rolSistemaDet.MODIFICA = roldet.MODIFICA;
                            rolSistemaDet.ELIMINA = roldet.ELIMINA;
                            rolSistemaDet.CONSULTA = roldet.CONSULTA;

                            _context.Ph_Roles_SistemaDet.Update(rolSistemaDet);
                        }
                        else
                        {
                            _context.Add(roldet);
                        }

                        await _context.SaveChangesAsync();
                    }

                }

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro del rol de sistema. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro del rol de sistema. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }


        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistemaDet: Obtener lista de Detalle de Roles de sistema 
        /// </summary>
        /// <returns>Lista de lista de detalle de roles del sistema</returns>
        public async Task<List<cPh_RolSistemaDet>> GetPhRolSistemaDet()
        {
            List<cPh_RolSistemaDet>? model = new();

            try
            {
                model = (from e in await _context.Ph_Roles_SistemaDet
                                .Include(e => e.cPh_RolSistema)
                            .ToListAsync()
                         select new cPh_RolSistemaDet
                         {
                             ROLSISTEMAID = e.ROLSISTEMAID,
                             MENUSISTEMAID = e.MENUSISTEMAID,
                             OPCIONSISTEMAID = e.OPCIONSISTEMAID,
                             HABILITADO = e.HABILITADO,
                             AGREGA = e.AGREGA,
                             MODIFICA = e.MODIFICA,
                             ELIMINA = e.ELIMINA,
                             CONSULTA = e.CONSULTA,
                             cPh_RolSistema = e.cPh_RolSistema == null?null:
                                new cPh_RolSistema
                                {
                                    ID = e.cPh_RolSistema.ID,
                                    DESCRIPCION = e.cPh_RolSistema.DESCRIPCION,
                                    HABILITADO = e.cPh_RolSistema.HABILITADO,
                                    IDCOMP = e.cPh_RolSistema.IDCOMP,
                                    IDNIVEL = e.cPh_RolSistema.IDNIVEL,
                                }
                         }
                            ).ToList();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhRolSistemaDet: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model;
        }

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistemaDet: Obtener datos de detalle de un rol de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>lista de opciones asociadas al rol cPh_RolSistemaDet </returns>
        public async Task<List<cPh_RolSistemaDet>> GetPhRolSistemaDet(string id)
        {
            List<cPh_RolSistemaDet>? model = new();

            try
            {

                model = (from e in await _context.Ph_Roles_SistemaDet
                                .Include(e => e.cPh_RolSistema)
                            .Where(e=>e.ROLSISTEMAID==id)
                            .ToListAsync()
                         select new cPh_RolSistemaDet
                         {
                             ROLSISTEMAID = e.ROLSISTEMAID,
                             MENUSISTEMAID = e.MENUSISTEMAID,
                             OPCIONSISTEMAID = e.OPCIONSISTEMAID,
                             HABILITADO = e.HABILITADO,
                             AGREGA = e.AGREGA,
                             MODIFICA = e.MODIFICA,
                             ELIMINA = e.ELIMINA,
                             CONSULTA = e.CONSULTA,
                             cPh_RolSistema = e.cPh_RolSistema == null ? null :
                                new cPh_RolSistema
                                {
                                    ID = e.cPh_RolSistema.ID,
                                    DESCRIPCION = e.cPh_RolSistema.DESCRIPCION,
                                    HABILITADO = e.cPh_RolSistema.HABILITADO,
                                    IDCOMP = e.cPh_RolSistema.IDCOMP,
                                    IDNIVEL = e.cPh_RolSistema.IDNIVEL,
                                }
                         }).ToList();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhMenuSistema: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model!;
        }


        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhUsuarioRol: Obtener datos de detalle roles de sistema para un usuario especifico 
        /// </summary>
        /// <param name="id">id de usuario</param>
        /// <returns>lista de roles asociados al usuario</returns>
        public async Task<List<cPh_UsuarioRol>> GetPhUsuarioRol(int id)
        {
            List<cPh_UsuarioRol>? model = new();

            try
            {

                model = (from e in await _context.Ph_Usuarios_Roles
                            .Where(e => e.IDUSUARIO == id)
                            .ToListAsync()
                         select new cPh_UsuarioRol
                         {
                             IDUSUARIO = e.IDUSUARIO,
                             IDREGISTRO = e.IDREGISTRO,
                             IDUSUARIOREGISTRA = e.IDUSUARIOREGISTRA,
                             FECHAREGISTRO = e.FECHAREGISTRO,
                             IDUSUARIOMODIFICA = e.IDUSUARIOMODIFICA,
                             FECHAMODIFICA = e.FECHAMODIFICA,
                             ROLID = GetRolId(e.ROL),
                             HABILITADO = GetRolStatus(e.ROL),
                             ROL = e.ROL,
                         }).ToList();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.GetPhUsuarioRol: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}"); throw;
            }
            return model!;
        }

        private string GetRolId(string rol)
        {
            if (!String.IsNullOrEmpty(rol))
            {
                var data = Encripta.getDecryptTripleDES(rol);

                var listValues = data.Split("|");
                return listValues[3];

            }
            return "-1";

        }
        private bool GetRolStatus(string rol)
        {
            if (!String.IsNullOrEmpty(rol))
            {
                var data = Encripta.getDecryptTripleDES(rol);

                var listValues = data.Split("|");
                return bool.Parse(listValues[4]);

            }
            return false;

        }

        /// <summary>
        /// Sincronizar_PhUsuarioRol:  Crear o actualizar la lista de usuarios y roles del sistema.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="usuariosRoles">Recibe una lista de cPh_UsuarioRol</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public async Task<EventResponse> Sincronizar_PhUsuarioRol(IEnumerable<cPh_UsuarioRol> usuariosRoles)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                foreach (var usuarioRol in usuariosRoles)
                {
                    cPh_UsuarioRol? objetoBuscar = await _context.Ph_Usuarios_Roles.FirstOrDefaultAsync(e => e.IDUSUARIO == usuarioRol.IDUSUARIO && e.IDREGISTRO == usuarioRol.IDREGISTRO);

                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.FECHAMODIFICA = DateTime.Now;
                        objetoBuscar.IDUSUARIOMODIFICA = usuarioRol.IDUSUARIOMODIFICA;

                        string fechaRegistro = objetoBuscar.FECHAREGISTRO.ToString("HHmmssddMMyyyy");
                        string rol = $"{fechaRegistro}|{objetoBuscar.IDUSUARIOREGISTRA}|{objetoBuscar.IDUSUARIO}|{usuarioRol.ROLID}|{usuarioRol.HABILITADO}";
                        objetoBuscar.ROL = Encripta.getEncryptTripleDES(rol);

                        _context.Ph_Usuarios_Roles.Update(objetoBuscar!);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var rolesExistentes = await GetPhUsuarioRol(usuarioRol.IDUSUARIO);

                        if (!rolesExistentes.Any(e => e.ROLID == usuarioRol.ROLID))
                        {
                            usuarioRol.FECHAREGISTRO = DateTime.Now;
                            usuarioRol.FECHAMODIFICA = DateTime.Now;

                            string fechaRegistro = usuarioRol.FECHAREGISTRO.ToString("HHmmssddMMyyyy");
                            string rol = $"{fechaRegistro}|{usuarioRol.IDUSUARIOREGISTRA}|{usuarioRol.IDUSUARIO}|{usuarioRol.ROLID}|{usuarioRol.HABILITADO}";
                            usuarioRol.ROL = Encripta.getEncryptTripleDES(rol);

                            _context.Add(usuarioRol);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de roles de usuario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de roles de usuario. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        #endregion

        #region NotificacionesCorreo

        public async Task<EventResponse> EnviarCorreo(IEnumerable<Email> correos)
        {
            EventResponse respuesta = new EventResponse();

            cParametroEmail parametrosCorreo = await GetParametroEmail(1);

            switch (parametrosCorreo.TipoServicio)
            {
                case 0: respuesta = _sendMail.SendMailSMTP(correos, parametrosCorreo); break;
                case 1: respuesta = _sendMail.SendMailMSGraph(correos, parametrosCorreo); break;
            }
            return respuesta;
        }
        #endregion


        #region Creacion Automática de Compañía

       

        private async Task<EventResponse> PostCompaniaEnBD(string compania, bool bCreateAdmin = false)
        {
            EventResponse resultado = new();
            try
            {
                //se obtiene ruta fisica de la Api, para buscar carpeta con los scripts a ejecutar

                var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "scripts");


                if (bCreateAdmin)
                {
                    //var resultUserAdmin = await _context.Database.ExecuteSqlRawAsync($"CREATE USER [CTADMIN] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[CTADMIN]");

                    //if (resultUserAdmin == -1)
                    //{
                    //    var resultSchema = await _context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA [CTADMIN]");
                    //}

                    //var resultSchema = await _context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA [CTADMIN]");

                    //string scriptAdmin = File.ReadAllText(Path.Combine(dirBase, "MSSQL_CREATE_ADMIN_TABLES_000.sql"));
                    //IEnumerable<string> commandStringsTablesAdm = Regex.Split(scriptAdmin, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    //foreach (string commandString in commandStringsTablesAdm)
                    //{
                    //    if (commandString.Trim() != "")
                    //    {
                    //        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                    //    }
                    //}

                    //scriptAdmin = File.ReadAllText(Path.Combine(dirBase, "MSSQL_INIT_ADMIN_TABLES.sql"));
                    //IEnumerable<string> commandStringsINITTBADM = Regex.Split(scriptAdmin, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    //foreach (string commandString in commandStringsINITTBADM)
                    //{
                    //    if (commandString.Trim() != "")
                    //    {
                    //        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                    //    }
                    //}

                }

                var resultUser = await _context.Database.ExecuteSqlRawAsync($"CREATE USER [{compania}] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[{compania}]");

                if (resultUser == -1)
                {
                    var resultSchema = await _context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA [{compania}]");
                }



                string script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_CREATE_TABLES_001.sql"));
                script = script.Replace("[dbo].", "[" + compania + "].");
                IEnumerable<string> commandStringsTables = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsTables)
                {
                    if (commandString.Trim() != "")
                    {
                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                    }
                }

                script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_CREATE_PROCEDURES_001.sql"));
                script = script.Replace("[dbo].", "[" + compania + "].");
                IEnumerable<string> commandStringsSP = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsSP)
                {
                    if (commandString.Trim() != "")
                    {
                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                    }
                }

                script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_INIT_TABLES.sql"));
                script = script.Replace("[dbo].", "[" + compania + "].");
                IEnumerable<string> commandStringsINITTB = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsINITTB)
                {
                    if (commandString.Trim() != "")
                    {
                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.PostCompaniaEnBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                resultado.Id = "1";
                resultado.Respuesta = "Error";
                resultado.Descripcion = $"GeoTimeConnectService.PostCompaniaEnBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
            }
            return resultado;
        }
        #endregion

        #region Actualización de estructuras de la base de datos por compañía
        

        public async Task<EventResponse> ActualizarCompaniaBD(cPh_Compania compania, bool migrate)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                await UpgradeTablesBD(compania.IDCOMP);
                await CretateNewTables(compania.IDCOMP);
                await AlterTablesAdd(compania.IDCOMP);
                await AlterTablesModify(compania.IDCOMP);
                await AddRelations(compania.IDCOMP);
                await CreateStoreProcedures(compania.IDCOMP);
                await UpgradeStoreProcedureBD(compania.IDCOMP);
                await CreateNewViews(compania.IDCOMP);
                await UpgradeInitTablesBD(compania.IDCOMP);
               
                if (migrate)
                {
                    await ActualizaPwdsUsuariosBD();
                    await CreaNivelesSeguridad(compania.IDCOMP);
                }
                    
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Compañía. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Compañía. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }


        public async Task<EventResponse> ActualizarCompaniaDatosApi(cPh_Compania compania)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                string commandString = "UPDATE ctadmin.PH_COMPANIAS SET " +
                                       $"ApiClientId = '{Encripta.getEncryptTripleDES(compania.APICLIENTID!)}', " +
                                       $"ApiPassword = '{Encripta.getEncryptTripleDES(compania.APIPASSWORD!)}', " +
                                       $"ApiUser = '{Encripta.getEncryptTripleDES(compania.APIUSER!)}', " +
                                       $"ApiDataBase = '{Encripta.getEncryptTripleDES(compania.APIDATABASE!)}', " +
                                       $"ApiUrl = '{Encripta.getEncryptTripleDES(compania.APIURL!)}'," +
                                       $"Zonahoraria = {compania.ZONAHORARIA} ";

                var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Compañía. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la actualización de la Compañía. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }

        private async Task<EventResponse> UpgradeTablesBD(string compania)
        {
            EventResponse resultado = new();
            try
            {
                //se obtiene ruta fisica de la Api, para buscar carpeta con los scripts a ejecutar

                var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "scripts");

                
                string script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_CREATE_ADMIN_TABLES_UPGRADE.sql"));
                IEnumerable<string> commandStringsTables = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsTables)
                {
                    if (commandString.Trim() != "")
                    {
                        try
                        {
                            var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"GeoTimeConnectService.UpgradeTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.UpgradeTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                resultado.Id = "1";
                resultado.Respuesta = "Error";
                resultado.Descripcion = $"GeoTimeConnectService.UpgradeTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
            }
            return resultado;
        }
        private async Task<EventResponse> UpgradeStoreProcedureBD(string compania)
        {
            EventResponse resultado = new();
            try
            {
                //se obtiene ruta fisica de la Api, para buscar carpeta con los scripts a ejecutar

                var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "scripts");


                string script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_CREATE_PROCEDURES_UPGRADE.sql"));
                script = script.Replace("[dbo].", "[" + compania + "].");
                IEnumerable<string> commandStringsTables = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsTables)
                {
                    if (commandString.Trim() != "")
                    {
                        try
                        {
                            var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"GeoTimeConnectService.UpgradeStoreProcedureBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.UpgradeStoreProcedureBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                resultado.Id = "1";
                resultado.Respuesta = "Error";
                resultado.Descripcion = $"GeoTimeConnectService.UpgradeStoreProcedureBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
            }
            return resultado;
        }
        private async Task<EventResponse> UpgradeInitTablesBD(string compania)
        {
            EventResponse resultado = new();
            try
            {
                //se obtiene ruta fisica de la Api, para buscar carpeta con los scripts a ejecutar

                var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "scripts");


                string script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_INIT_ADMIN_TABLES_UPGRADE.sql"));
                script = script.Replace("[dbo].", "[" + compania + "].");
                IEnumerable<string> commandStringsTables = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsTables)
                {
                    if (commandString.Trim() != "")
                    {
                        try
                        {
                            var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"GeoTimeConnectService.UpgradeInitTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                        }

                        
                    }
                }

                script = File.ReadAllText(Path.Combine(dirBase, "MSSQL_INIT_TABLES_UPGRADE.sql"));
                script = script.Replace("[dbo].", "[" + compania + "].");
                IEnumerable<string> commandStringsINITTB = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsINITTB)
                {
                    if (commandString.Trim() != "")
                    {
                        try
                        {
                            var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"GeoTimeConnectService.UpgradeInitTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.UpgradeInitTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                resultado.Id = "1";
                resultado.Respuesta = "Error";
                resultado.Descripcion = $"GeoTimeConnectService.UpgradeInitTablesBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
            }
            return resultado;
        }


        public async Task CretateNewTables(string companyName)
        {

            var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string fileSource = Path.Combine(dirBase, "create_tbl.xml");

            if (System.IO.File.Exists(fileSource))
            {
                using (XmlReader reader = XmlReader.Create(fileSource))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString().ToUpper())
                            {
                                case "CATABLES":
                                    break;
                                case "TABLA":
                                    var tabla = reader.GetAttribute("nombre");
                                    break;
                                case "COMANDOS":
                                    var commandString = reader.GetAttribute("ejecuto");
                                    commandString = commandString!.ToUpper().Replace("[GEOTIME]", $"[{companyName}]");
                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.CretateNewTables: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            }
        }

        public async Task AlterTablesAdd(string companyName)
        {

            var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string fileSource = Path.Combine(dirBase, "alter_add.xml");

            if (System.IO.File.Exists(fileSource))
            {
                using (XmlReader reader = XmlReader.Create(fileSource))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString().ToUpper())
                            {
                                case "CATABLES":
                                    break;
                                case "COLUMNA":
                                    var tabla = reader.GetAttribute("tabla");
                                    var nombre = reader.GetAttribute("nombre");
                                    var tipo = reader.GetAttribute("tipo");
                                    var nulo = reader.GetAttribute("nulo");
                                    var defecto = reader.GetAttribute("defecto");

                                    var commandString = $"alter table [{companyName}].{tabla} add {nombre} {tipo} {nulo} {(!String.IsNullOrEmpty(defecto)?defecto:"")}";
                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.AlterTablesAdd: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            }
        }

        public async Task AlterTablesModify(string companyName)
        {

            var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string fileSource = Path.Combine(dirBase, "alter_modify.xml");

            if (System.IO.File.Exists(fileSource))
            {
                using (XmlReader reader = XmlReader.Create(fileSource))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString().ToUpper())
                            {
                                case "CATABLES":
                                    break;
                                case "COLUMNA":
                                    var tabla = reader.GetAttribute("tabla");
                                    var nombre = reader.GetAttribute("nombre");
                                    var tipo = reader.GetAttribute("tipo");
                                    var nulo = reader.GetAttribute("nulo");
                                    var defecto = reader.GetAttribute("defecto");

                                    var commandString = $"alter table [{companyName}].{tabla} alter column {nombre} {tipo} {nulo} {(!String.IsNullOrEmpty(defecto) ? defecto : "")}";
                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.AlterTablesModify: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            }
        }

        public async Task AddRelations(string companyName)
        {

            var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string fileSource = Path.Combine(dirBase, "create_fks.xml");

            if (System.IO.File.Exists(fileSource))
            {
                using (XmlReader reader = XmlReader.Create(fileSource))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString().ToUpper())
                            {
                                case "CARELAT":
                                    break;
                                case "STORED":
                                    break;
                                case "COMANDOS":
                                    var commandString = reader.GetAttribute("ejecuto");
                                    commandString = commandString!.ToUpper().Replace("[GEOTIME]", $"[{companyName}]");

                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString.Split("%")[0]}");
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.AddRelations: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }

                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString.Split("%")[1]}");
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.AddRelations: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            }
        }

        public async Task CreateStoreProcedures(string companyName)
        {

            var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string fileSource = Path.Combine(dirBase, "create_sps.xml");

            if (System.IO.File.Exists(fileSource))
            {
                using (XmlReader reader = XmlReader.Create(fileSource))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString().ToUpper())
                            {
                                case "CASTORED":
                                    break;
                                case "STORED":
                                    break;
                                case "COMANDOS":
                                    var commandString = reader.GetAttribute("ejecuto");
                                    commandString = commandString!.ToUpper().Replace("[GEOTIME]", $"[{companyName}]");

                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.CreateStoreProcedures: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            }
        }
        public async Task CreateNewViews(string companyName)
        {

            var dirBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string fileSource = Path.Combine(dirBase, "create_vws.xml");

            if (System.IO.File.Exists(fileSource))
            {
                using (XmlReader reader = XmlReader.Create(fileSource))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString().ToUpper())
                            {
                                case "CAVISTAS":
                                    break;
                                case "VISTA":
                                    var tabla = reader.GetAttribute("nombre");
                                    break;
                                case "COMANDOS":
                                    var commandString = reader.GetAttribute("ejecuto");
                                    commandString = commandString!.ToUpper().Replace("[GEOTIME]", $"[{companyName}]");
                                    try
                                    {
                                        var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");

                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"GeoTimeConnectService.CretateNewViews: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                                    }
                                    break;
                            }
                        }
                    }
                }

            }
        }

        public async Task<EventResponse> ActualizaPwdsUsuariosBD()
        {
            EventResponse resultado = new EventResponse();

            try
            {
                var phLogins = await _context.PH_LOGIN.Where(e => String.IsNullOrEmpty(e.GLOBAL_CLAVE) == true).ToListAsync();

                foreach (var login in phLogins)
                {
                    string claveNueva = await CambioEncriptacion(login.clave);

                    if (claveNueva != "")
                    {
                        var commandString = $"update ctadmin.ph_login set global_clave='{claveNueva}' where IDUSUARIO={login.idusuario}";
                        try
                        {
                            var resultCommand = await _context.Database.ExecuteSqlRawAsync($"{commandString}");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"GeoTimeConnectService.ActualizaUsuariosBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
                        }
                    }

                    
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GeoTimeConnectService.ActualizaPwdsUsuariosBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                resultado.Id = "1";
                resultado.Respuesta = "Error";
                resultado.Descripcion = $"GeoTimeConnectService.ActualizaPwdsUsuariosBD: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
            }

            return resultado;
        }

        private async Task<string> CambioEncriptacion(string dato)
        {
            cambio_encryptRequest param = new cambio_encryptRequest
            {
                dato = dato               
            };

            EndpointConfiguration endpointConfiguration = new();
            GeoTimeServiceReference.ServiceSoapClient geoWebService = new(endpointConfiguration);

            var result = await geoWebService.cambio_encryptAsync(param);
            if (result.cambio_encryptResult != "" && result.cambio_encryptResult != "Error procesando el Dato")
                return result.cambio_encryptResult;

            return "";
        }

        public async Task CreaNivelesSeguridad(string compania)
        {
            try
            {
               
                var newContext = SchemaChangeDbContext.GetSchemaChangeDbContext(compania, _dataBase);

                if (newContext is not null)
                    _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, NewContext is not null");
                else
                {
                    _logger.LogError($"GeoTimeConnectService.CreaNivelesSeguridad, NewContext is null");
                    return;
                }

                var niveles = await newContext.Ph_Niveles.ToListAsync();
                List<cPhNivelesDet> nivelDetalle = new();

                
                foreach (var nivel in niveles)
                {
                    _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, Id nivel: {nivel.IDNIVEL}");


                    if (newContext.Ph_Usuarios is null)
                    {
                        _logger.LogError($"GeoTimeConnectService.CreaNivelesSeguridad, newContext.Ph_Usuarios is null");
                        return;
                    }


                    var phUsuarios = await newContext.Ph_Usuarios.ToListAsync();

                    


                    if (phUsuarios is not null)
                    {
                        _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, PhUsuarios: {phUsuarios.Count()}");

                        _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, Usuarios nivel {nivel.IDNIVEL}: {phUsuarios.Count()}");

                        if (nivel.IDNIVEL > 1)
                        {
                            phUsuarios = phUsuarios.Where(e => e.NIVEL == nivel.IDNIVEL).ToList();

                            var variables = await GetNivelesAutorizacion(nivel.VARIABLES);

                            _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, Variables: {variables}");

                            if (!String.IsNullOrEmpty(variables))
                            {
                                var opciones = variables.Split("°");

                                foreach (string item in opciones)
                                {
                                    _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, items: {item}");
                                    var det = item.Split("*");

                                    if (det.Length == 2)
                                    {
                                        nivelDetalle.Add(new cPhNivelesDet
                                        {
                                            IdNivelDet = det[0],
                                            Activo = det[1] == "T" ? true : false
                                        });
                                    }

                                }
                            }

                            await TranformarNivelSeguridad(nivelDetalle, nivel, compania, phUsuarios);
                        }
                        else
                        {
                            phUsuarios = phUsuarios.Where(e => e.NIVEL == nivel.IDNIVEL).ToList();

                            foreach (var usuario in phUsuarios!)
                            {
                                _logger.LogWarning($"GeoTimeConnectService.CreaNivelesSeguridad, Usuario: {usuario.IDUSUARIO}");
                                cPh_UsuarioRol usuarioRol = new cPh_UsuarioRol
                                {
                                    IDUSUARIO = usuario.IDUSUARIO,
                                    ROLID = "0001",
                                    IDUSUARIOMODIFICA = 1,
                                    IDUSUARIOREGISTRA = 1,
                                    FECHAREGISTRO = DateTime.Now,
                                    FECHAMODIFICA = DateTime.Now,
                                    IDREGISTRO = 0,
                                    HABILITADO = true,
                                    ROL = ""
                                };
                                var resp = await Sincronizar_PhUsuarioRol(new List<cPh_UsuarioRol>() { usuarioRol });
                            }

                        }
                        

                         

                    }
                    else
                    {
                        _logger.LogError($"GeoTimeConnectService.CreaNivelesSeguridad, PhUsuarios is null");
                    }


                }


            }
            catch (Exception e)
            {
                _logger.LogError($"GeoTimeConnectService.CreaNivelesSeguridad: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
            }
            
        }

        private async Task TranformarNivelSeguridad(List<cPhNivelesDet> nivelDetalle, cPh_Nivel nivel, string compania,List<cPh_Usuario> usuarios)
        {
            try
            {
                string RolId = "";

                /*

                    200	Mantenimientos	213	/paletacolorlist	Administración_de_Colores                
                    100	Procesos	108	/pprogramadorhorarios	Programador_de_Horarios
                    400	Configuracion	408	/transformaciontipomarcalist	Transformaciones_Funcion_Tipo                

                 */

                var phRolesCreados = await _context.Ph_Roles_Sistema.ToListAsync();

                phRolesCreados = phRolesCreados.Where(e => e.ID !="0001" ).ToList();

                if (phRolesCreados.Count() == 0)
                {
                    RolId = "0002";

                    cPh_RolSistema rol = new cPh_RolSistema
                    {
                        ID = RolId,
                        DESCRIPCION = $"{nivel.DESCRIPCION} ({compania})",
                        HABILITADO = true,
                        IDCOMP = compania,
                        IDNIVEL = nivel.IDNIVEL,
                    };

                    rol.cPh_RolSistemaDet = await GetDetalleOpciones(nivelDetalle, rol);

                    var resp = await Sincronizar_PhRolSistema(new List<cPh_RolSistema>() { rol });

                    foreach (var usuario in usuarios.Where(e => e.NIVEL == nivel.IDNIVEL))
                    {
                        cPh_UsuarioRol usuarioRol = new cPh_UsuarioRol
                        {
                            IDUSUARIO = usuario.IDUSUARIO,
                            ROLID = RolId,
                            IDUSUARIOMODIFICA = 1,
                            IDUSUARIOREGISTRA = 1,
                            FECHAREGISTRO = DateTime.Now,
                            FECHAMODIFICA = DateTime.Now,
                            IDREGISTRO = 0,
                            HABILITADO = true,
                            ROL = ""
                        };
                        var respUsu = await Sincronizar_PhUsuarioRol(new List<cPh_UsuarioRol>() { usuarioRol });
                    }
                }
                   
                else
                {
                    var rolExiste = phRolesCreados.FirstOrDefault(e => e.IDCOMP == compania && e.DESCRIPCION == $"{nivel.DESCRIPCION} ({compania})");

                    if (rolExiste is null)
                    {
                        //si no existe el rol para la compañia, se crea uno nuevo
                        //se debe buscar el maximo id y sumarle 1
                        var maxId = phRolesCreados.Max(e => int.Parse(e.ID));
                        RolId = (maxId + 1).ToString("0000");

                        cPh_RolSistema rol = new cPh_RolSistema
                        {
                            ID = RolId,
                            DESCRIPCION = $"{nivel.DESCRIPCION} ({compania})",
                            HABILITADO = true,
                            IDCOMP = compania,
                        };

                        

                        rol.cPh_RolSistemaDet = await GetDetalleOpciones(nivelDetalle,rol);

                        var resp = await Sincronizar_PhRolSistema(new List<cPh_RolSistema>() { rol });

                        foreach (var usuario in usuarios.Where(e => e.NIVEL == nivel.IDNIVEL))
                        {
                            cPh_UsuarioRol usuarioRol = new cPh_UsuarioRol
                            {
                                IDUSUARIO = usuario.IDUSUARIO,
                                ROLID = RolId,
                                IDUSUARIOMODIFICA = 1,
                                IDUSUARIOREGISTRA = 1,
                                FECHAREGISTRO = DateTime.Now,
                                FECHAMODIFICA = DateTime.Now,
                                IDREGISTRO = 0,
                                HABILITADO = true,
                                ROL = ""
                            };
                            var respUsu = await Sincronizar_PhUsuarioRol(new List<cPh_UsuarioRol>() { usuarioRol });
                        }

                    }

                   
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"GeoTimeConnectService.TranformarNivelSeguridad: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {e.Message}");
            }
        }

        private async Task<List<cPh_RolSistemaDet>> GetDetalleOpciones(List<cPhNivelesDet> nivelDetalle, cPh_RolSistema rol)
        {
            List<cPh_RolSistemaDet> rolesDet = new();
            var nivelesActivos = nivelDetalle.Where(e => e.Activo == true).ToList();
            foreach (var item in nivelesActivos)
            {
                switch (item.IdNivelDet)
                {
                    case "cmp_v": //mantenimiento/companias
                                  //200	Mantenimientos	201	/companialist	Compañias
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "201",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "cmp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "cmp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "cmp_e"),
                            CONSULTA = true
                        });
                        break;
                    case "pln_v": //mantenimiento/planilla
                                  //200	Mantenimientos	202	/planillalist	Nómina
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "202",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "pln_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "pln_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "pln_e"),
                            CONSULTA = true
                        });
                        break;
                    case "dep_v": //mantenimiento/departamento
                                  //200	Mantenimientos	203	/departamentolist	Departamentos
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "203",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "dep_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "dep_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "dep_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "grp_v": //mantenimiento/grupos
                                  //200	Mantenimientos	204	/grupolist	Grupo
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "204",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "grp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "grp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "grp_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "emp_v": //mantenimiento/empleados
                                  //200 Mantenimientos  205 / empleadolist   Empleado
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "205",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "emp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "emp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "emp_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "turn_v": //mantenimiento/turnos
                                   //200	Mantenimientos	206	/turnolist	Turnos
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "206",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "turn_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "turn_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "turn_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "hor_v": //mantenimiento/horarios  o hor_v = mantenimiento/roles
                                  // 200	Mantenimientos	207	/horarioturnoList	Horarios
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "207",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "hor_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "hor_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "hor_e"),
                            CONSULTA = true,
                        });

                        //200 Mantenimientos  208 / rollist    Roles
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "208",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "hor_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "hor_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "hor_e"),
                            CONSULTA = true,
                        });

                        break;
                    case "tipo_distribuye_cc": //"L" = mantenimiento/Labores

                        break;
                    case "inci_v": //Mantenimiento/incidencias
                                   //200	Mantenimientos	209	/incidlist	Incidencias
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "209",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "inci_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "inci_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "inci_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "ccost_v":
                        //200	Mantenimientos	212	/centrocostolist	Centro_de_Costo
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "212",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "ccost_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "ccost_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "ccost_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "tc_erp_v": //erp/erp_sinc
                                     // 700	Integracion_ERP	701	/sincronizaerp	Sincronizar_ERP
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "700",
                            OPCIONSISTEMAID = "701",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = true,
                            ELIMINA = false,
                            CONSULTA = false,
                        });
                        break;
                    case "im_acp_v": //erp/erp_ac_sinc
                                     //700    Integracion_ERP 703 /sincroniza acciones Importar_Acciones_Personal
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "700",
                            OPCIONSISTEMAID = "703",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = true,
                            ELIMINA = false,
                            CONSULTA = false,
                        });
                        break;
                    case "crp_v": //mantenimiento/periodos
                                  //100	Procesos	101	/periodomarcalist	Períodos_de_Marcas
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "101",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "crp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "crp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "crp_e"),
                            CONSULTA = false,
                        });
                        break;
                    case "acp_v": //procesos/filtro_inicio_periodo
                                  //100	Procesos	102	/actperiodoproceso	Activar_Período
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "102",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "acp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "acp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "acp_e"),
                            CONSULTA = false,
                        });
                        break;
                    case "ccap_v": //procesos/filtro_calc
                                   //100	Procesos	103	/clperiodomarca	Calcular_Período_de_Marca
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "103",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "ccap_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "ccap_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "ccap_e"),
                            CONSULTA = false,
                        });
                        break;
                    case "edt_v": //procesos/filtro_editot
                                  //100	Procesos	104	/empleadomarcafiltro	Editar_Marcas_del_Periodo
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "104",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "edt_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "edt_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "edt_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "cnd_esp": //cond_esp/fit_cond_esp
                                    //100	Procesos	105	/pcondicionesespeciales	Edición_Condiciones_Especiales
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "105",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "cnd_esp"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "cnd_esp"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "cnd_esp"),
                            CONSULTA = true,
                        });
                        break;
                    case "accp_v": //acc_personal/fit_acc
                                   //100	Procesos	106	/paccionpersonal	Acciones_de_Personal
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "106",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "accp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "accp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "accp_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "prgt_v": //procesos/filtro_prog_tur
                                   //100	Procesos	107	/pprogramadorturnos	Programador_de_Turnos
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "107",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "prgt_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "prgt_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "prgt_e"),
                            CONSULTA = true,
                        });

                        // 200	Mantenimientos	214	/programacionturnoscmlist	Carga_Masiva_Turnos    
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "200",
                            OPCIONSISTEMAID = "214",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "prgt_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "prgt_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "prgt_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "mapb_v": //aprobaciones/filt_apb_ext
                                   //100 Procesos    109 / pamasivaextras Aprobacion_Masiva_de_Extras
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "109",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "mapb_e"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "caex_v": //procesos/filtro_cambio_io_masivo
                                   //100	Procesos	110	/pcambiomasivo	Cambio_Masivo_de_Entrada_y_Salida
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "110",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "caex_v"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "exp_acc_v": //erp/erp_exp_acc
                                      //700	Integracion_ERP	704	/exportaraccionespersonal	Exportar_Acciones_de_Personal
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "700",
                            OPCIONSISTEMAID = "704",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "exp_acc_v"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "exp_conc_v": //erp/erp_exp_conc
                                       //700	Integracion_ERP	705	/exportarconceptos	Exportar_Conceptos
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "700",
                            OPCIONSISTEMAID = "705",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "exp_conc_v"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "clc_v": //procesos/filtro_cierre
                                  //100	Procesos	113	/pcierreperiodo	Cierre_de_Periodo
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "100",
                            OPCIONSISTEMAID = "113",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "clc_v"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "tc_con_v": //erp/conc_shw
                                     //700	Integracion_ERP	702	/importarconcepto	Importar_Concepto 
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "700",
                            OPCIONSISTEMAID = "702",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "tc_con_v"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "tc_acp_v": //erp/accp_shw                        
                                     //400	Configuracion	402	/importartipoaccion	Importar_Definición_Acciones_Per
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "402",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "tc_acp_v"),
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                    case "tipoh_v": //mantenimiento/conceptos
                                    // 400	Configuracion	403	/conceptolist	Tipos_de_Hora
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "403",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "tipoh_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "tipoh_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "tipoh_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "form_v": //mantenimiento/formulas
                                   //400	Configuracion	404	/formulacionlist	Formulas
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "404",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "form_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "form_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "form_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "trnt_v": //mantenimiento/transf_turn
                                   //400	Configuracion	405	/transformacionlist	Transformaciones_en_Turnos
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "405",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "trnt_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "trnt_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "trnt_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "trntcto_v": //mantenimiento/transformacion_conceptos

                        break;
                    case "trnp_v": //mantenimiento/transf_glob
                                   //400	Configuracion	406	/transformaciongloballist	Transformaciones_Post_Calculo
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "406",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "trnp_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "trnp_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "trnp_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "cinc_v": //mantenimiento/pag_inci
                                   // 400	Configuracion	407	/incidenciaconfpagolist	Configuracion_Pago_Incidencias
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "407",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "cinc_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "cinc_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "cinc_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "usr_v": //usuarios/usr_gui
                                  //600	Seguridad	602	/usuariosistemalist	Usuarios_del_Sistema
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "600",
                            OPCIONSISTEMAID = "602",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "usr_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "usr_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "usr_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "pfs_v": // niveles_acc/niveles_gui
                                  //600 Seguridad   601 / rolsistemalist Perfiles_de_seguridad
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "600",
                            OPCIONSISTEMAID = "601",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "pfs_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "pfs_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "pfs_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "usrc_v": //usuarios/usr_comp_gui
                                   // 600	Seguridad	603	/usuariocompanialist	Usuarios_Compañia
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "600",
                            OPCIONSISTEMAID = "603",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "usrc_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "usrc_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "usrc_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "lic_v": //licencias
                                  //500	Configuracion_Adicional	508	/cargarlicencia	Cargar_Licencia
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "500",
                            OPCIONSISTEMAID = "508",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "lic_e"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "lic_e"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "lic_e"),
                            CONSULTA = true,
                        });
                        break;
                    case "dist_lic": // = licencias/dist_lic.aspx

                        break;
                    case "main_mn_opc": //mantenimiento/sistema/opc_sis
                                        //400	Configuracion	409	/opcionessistema	Opciones_del_Sistema
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "409",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "main_mn_opc"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "main_mn_opc"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "main_mn_opc"),
                            CONSULTA = true,
                        });
                        break;
                    case "usr_pref_lng": //mantenimiento/sistema/pref_usr_gui
                                         // 400	Configuracion	410	/preferenciausuario	Preferencias_de_Usuarios
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "400",
                            OPCIONSISTEMAID = "410",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "usr_pref_lng"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "usr_pref_lng"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "usr_pref_lng"),
                            CONSULTA = true,
                        });
                        break;
                    case "est_v": //ayuda
                                  //  999 Ayuda   998 NULL Ayuda
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "999",
                            OPCIONSISTEMAID = "998",
                            HABILITADO = true,
                            AGREGA = nivelDetalle.Any(e => e.IdNivelDet == "est_v"),
                            MODIFICA = nivelDetalle.Any(e => e.IdNivelDet == "est_v"),
                            ELIMINA = nivelDetalle.Any(e => e.IdNivelDet == "est_v"),
                            CONSULTA = true,
                        });
                        break;
                    case "rep_v": //reportes
                                  //300	Reportes	301	/historicohorasextras	Historico_Horas_Extra
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "300",
                            OPCIONSISTEMAID = "301",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = false,
                            ELIMINA = false,
                            CONSULTA = true,
                        });

                        //300	Reportes	302	/historicoincidencias	Historico_Incidencias

                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "300",
                            OPCIONSISTEMAID = "302",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = false,
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        //300 Reportes    303 / historicomarcas    Historico_Marcas

                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "300",
                            OPCIONSISTEMAID = "303",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = false,
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        //300 Reportes    304 / historicocalculostiempos   Historico_Calculo_Tiempo
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "300",
                            OPCIONSISTEMAID = "304",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = false,
                            ELIMINA = false,
                            CONSULTA = true,
                        });

                        //300 Reportes    305 / historicoconceptosresumen  ResumenConceptos
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "300",
                            OPCIONSISTEMAID = "305",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = false,
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        //300 Reportes    306 / historicoconceptos ResumenConceptosEmpleado
                        rolesDet.Add(new cPh_RolSistemaDet
                        {
                            ROLSISTEMAID = rol.ID,
                            MENUSISTEMAID = "300",
                            OPCIONSISTEMAID = "306",
                            HABILITADO = true,
                            AGREGA = false,
                            MODIFICA = false,
                            ELIMINA = false,
                            CONSULTA = true,
                        });
                        break;
                }
            }
            return rolesDet;
        }



        #endregion

    }


}

/*
 * 

ed_dist_cc  = distribuciones
pcf_e
lb_anul
per_c_tip
c_acp_v
d_acp_v
ma_acp_v
mn_acp_v
a_acp_v
ff_acp_v
fa_acp_v
ae_edt_e
ar_edt_e
at_edt_e
cap_edt_e
cm_edt_e
ct_edt_e
er_edt_e
et_edt_e
ji_edt_e
mt_edt_e
edt_mapv
trntcto_e
trntcto_v mantenimiento/transformacion_conceptos
emp_hor_v
emp_hor_e
t_hor_v
t_hor_e
dist_lic_edt*/
