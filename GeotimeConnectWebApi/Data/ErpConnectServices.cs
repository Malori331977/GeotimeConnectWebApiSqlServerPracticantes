using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.ErpClases;
using com.gsitcr.geotime.Models.Utils;
using JtSegEncrypta;
using LibEncripta;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class ErpConnectServices : IErpConnectService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IGenericService _erpConnect;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _schema = "";
        private readonly string _dataBase = "";
        private readonly ILogger<ErpConnectServices> _logger;
        private string InterfaceName = "ErpConnectServices";

        public ErpConnectServices(IHttpContextAccessor httpContextAccessor, 
                                     ILogger<ErpConnectServices> logger,
                                     IGenericService erpConnect,
                                     SqlServerDataBaseContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _erpConnect = erpConnect;           
            string schema = "";
            string bdname = "";
            _logger = logger;

            if (_httpContextAccessor is not null && _httpContextAccessor.HttpContext is not null)
            {
                IEnumerable<Claim> claims = _httpContextAccessor.HttpContext!.User.Claims;
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
            }

            if (schema == "")
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                schema = config.GetConnectionString("Schema")!;
                bdname = config.GetConnectionString("DBName")!;
            }
            _schema = schema!;
            _dataBase = bdname!;
            _context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);

        }

        public ErpConnectServices(SqlServerDataBaseContext context,
                                     ILogger<ErpConnectServices> logger,
                                     IGenericService erpConnect,                                     
                                     string DbName,
                                     string Schema)
        {
            _context = context;
            _logger = logger;
            _erpConnect = erpConnect;
            _dataBase = DbName;
            _schema = Schema;
            _context = SchemaChangeDbContext.GetSchemaChangeDbContext(_schema, _dataBase);
        }

        private async Task<cPh_Compania> GetCompania()
        {
            cPh_Compania? model = null;
            try
            {
                model = await _context.PH_COMPANIAS.FirstOrDefaultAsync(c => c.IDCOMP == _schema);

                if (model is not null)
                {
                    _erpConnect.SetCompania(new UrlCompania
                    {
                        BaseDatos = String.IsNullOrEmpty(model.APIDATABASEERP) ? "" : Encripta.getDecryptTripleDES(model.APIDATABASEERP),
                        SchemaBd = String.IsNullOrEmpty(model.APISCHEMAERP) ? "" : Encripta.getDecryptTripleDES(model.APISCHEMAERP),
                        ApiClientId = "197ac2e4bd0843c3974725a6544e1089c4a7dcae59087543ba6428c9914c35d9",
                        ApiPassword = "c5bbf3d10de5c6dfdad016e6e948a27d343b5e22f35471324388460c4e14a27c",
                        ApiUser = "GSITCR",
                        Url = String.IsNullOrEmpty(model.APIURLERP) ? "" : Encripta.getDecryptTripleDES(model.APIURLERP),
                        Compania = model.IDCOMP!
                    });

                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetCompania: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model!;
        }

        public async Task<IEnumerable<cDepartamentoErp>> GetDepartamentoErp()
        {
            IEnumerable<cDepartamentoErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cDepartamentoErp>();
                    string error = $"{InterfaceName}.GetDepartamentoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cDepartamentoErp>("Departamento");


            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cDepartamentoErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetDepartamentoErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                 throw;
            }
            return model;
        }

        public async Task<IEnumerable<cCentroCostoErp>> GetCentroCostoErp()
        {
            IEnumerable<cCentroCostoErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cCentroCostoErp>();
                    string error = $"{InterfaceName}.GetCentroCostoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cCentroCostoErp>("CentroCosto");


            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cCentroCostoErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetCentroCostoErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }


        public async Task<IEnumerable<cConceptoErp>> GetConceptoErp()
        {
            IEnumerable<cConceptoErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cConceptoErp>();
                    string error = $"{InterfaceName}.GetConceptoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cConceptoErp>("Concepto");


            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cConceptoErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetConceptoErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        public async Task<IEnumerable<cEmpleadoErp>> GetEmpleadoErp()
        {
            IEnumerable<cEmpleadoErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cEmpleadoErp>();
                    string error = $"{InterfaceName}.GetEmpleadoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cEmpleadoErp>("Empleado");


            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cEmpleadoErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetEmpleadoErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        public async Task<IEnumerable<cEmpleadoErp>> GetEmpleadoByNominaErp(string nomina)
        {
            IEnumerable<cEmpleadoErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cEmpleadoErp>();
                    string error = $"{InterfaceName}.GetEmpleadoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cEmpleadoErp>($"EmpleadoByNomina/{nomina}");


            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cEmpleadoErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetEmpleadoByNominaErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        public async Task<IEnumerable<cPuestoErp>> GetPuestoErp()
        {
            IEnumerable<cPuestoErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cPuestoErp>();
                    string error = $"{InterfaceName}.GetPuestoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cPuestoErp>("Puesto");


            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cPuestoErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetPuestoErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }

        public async Task<IEnumerable<cNominaErp>> GetNominaErp()
        {
            IEnumerable<cNominaErp>? model;

            try
            {
                cPh_Compania? compania = await GetCompania();

                if (compania is null)
                {
                    model = Enumerable.Empty<cNominaErp>();
                    string error = $"{InterfaceName}.GetConceptoErp: No se ha podido obtener la compañia para el esquema actual {_schema}.";
                    _logger.LogError(error);
                    throw new Exception(error);
                }

                model = await _erpConnect.Get<cNominaErp>("Nomina");

            }
            catch (Exception e)
            {
                model = Enumerable.Empty<cNominaErp>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                error = $"{InterfaceName}.GetNominaErp: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}";
                _logger.LogError(error);
                throw;
            }
            return model;
        }


    }


}
