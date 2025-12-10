using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Claims;

namespace com.gsitcr.geotime.Data
{
    public class CrystalReportService : ICrystalReportService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<CrystalReportService> _logger;

        public CrystalReportService(SqlServerDataBaseContext context, IHttpContextAccessor httpContextAccessor, ILogger<CrystalReportService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            IEnumerable<System.Security.Claims.Claim> claims = _httpContextAccessor.HttpContext!.User.Claims;
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
        public async Task<EventResponse> GeneratePdfReport(cFiltroReporte filtro)
        {

            EventResponse response = new EventResponse();
            // Ruta base de la carpeta CrystalReports(ajusta según tu estructura)
            string reportsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CrystalReports");
            string reportPath = Path.Combine(reportsFolder, filtro.RptName);

            if (!File.Exists(reportPath))
            {
                response.Respuesta = "Error";
                response.Descripcion = $"El reporte '{filtro.RptName}' no existe en la carpeta CrystalReports.";
                return response;
            }

            ReportDocument reportDocument = new ReportDocument();
            //buscar direccion del reporte y cargar

            reportDocument.Load(filtro.RptName);
            byte[] data = new byte[1];
            

            // If dbContext provided, try to obtain connection info and apply to every table and subreport
            if (_context != null)
            {
                // Try to get the underlying DbConnection via the DbContext's Database property
                try
                {
                    var dbConnection = _context.Database.GetDbConnection();
                    var connString = dbConnection.ConnectionString;

                    // Parse basic parts from connection string
                    var builder = new DbConnectionStringBuilder { ConnectionString = connString };
                    string? server = null, database = null, user = null, password = null;
                    if (builder.TryGetValue("Data Source", out var ds) || builder.TryGetValue("Server", out ds)) server = ds?.ToString();
                    if (builder.TryGetValue("Initial Catalog", out var ic) || builder.TryGetValue("Database", out ic)) database = ic?.ToString();
                    if (builder.TryGetValue("User ID", out var uid) || builder.TryGetValue("Uid", out uid)) user = uid?.ToString();
                    if (builder.TryGetValue("Password", out var pwd) || builder.TryGetValue("Pwd", out pwd)) password = pwd?.ToString();

                    CrystalDecisions.Shared.ConnectionInfo connInfo = new CrystalDecisions.Shared.ConnectionInfo();
                    if (!string.IsNullOrEmpty(server)) connInfo.ServerName = server!;
                    if (!string.IsNullOrEmpty(database)) connInfo.DatabaseName = database!;
                    if (!string.IsNullOrEmpty(user)) connInfo.UserID = user!;
                    if (!string.IsNullOrEmpty(password)) connInfo.Password = password!;

                    // Apply to main report tables
                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in reportDocument.Database.Tables)
                    {
                        CrystalDecisions.Shared.TableLogOnInfo li = table.LogOnInfo;
                        li.ConnectionInfo = connInfo;
                        table.ApplyLogOnInfo(li);
                        // keep existing location
                    }

                    // Apply to subreports
                    foreach (Section section in reportDocument.ReportDefinition.Sections)
                    {
                        foreach (ReportObject obj in section.ReportObjects)
                        {
                            if (obj.Kind != ReportObjectKind.SubreportObject) continue;
                            SubreportObject subObj = (SubreportObject)obj;
                            ReportDocument subDoc = subObj.OpenSubreport(subObj.SubreportName);
                            foreach (CrystalDecisions.CrystalReports.Engine.Table subTable in subDoc.Database.Tables)
                            {
                                CrystalDecisions.Shared.TableLogOnInfo li = subTable.LogOnInfo;
                                li.ConnectionInfo = connInfo;
                                subTable.ApplyLogOnInfo(li);
                            }
                        }
                    }
                }
                catch
                {
                    // don't fail report generation if unable to set DB connection; parameters may still work
                }
            }

            // If filtro provided, set common parameters and formula fields
            if (filtro != null)
            {
                // try set typical parameter names used in older code
                try { reportDocument.SetParameterValue("finicio", filtro.inicio); } catch { }
                try { reportDocument.SetParameterValue("ffin", filtro.fin); } catch { }
                // if lists provided, set as comma-separated strings for legacy reports
                if (filtro.Planillas != null)
                {
                    try { reportDocument.SetParameterValue("planilla", string.Join(",", filtro.Planillas)); } catch { }
                }
                if (filtro.Departamentos != null)
                {
                    try { reportDocument.SetParameterValue("departamento", string.Join(",", filtro.Departamentos)); } catch { }
                }
                if (filtro.CentrosCosto != null)
                {
                    try { reportDocument.SetParameterValue("ccosto", string.Join(",", filtro.CentrosCosto)); } catch { }
                }
                if (filtro.Grupos != null)
                {
                    try { reportDocument.SetParameterValue("grupo", string.Join(",", filtro.Grupos)); } catch { }
                }
                if (filtro.Empleados != null)
                {
                    try { reportDocument.SetParameterValue("empleados", string.Join(",", filtro.Empleados)); } catch { }
                }
                if (filtro.Incidencias != null)
                {
                    try { reportDocument.SetParameterValue("incidencias", string.Join(",", filtro.Incidencias)); } catch { }
                }

                // also set formula fields like {@fecha_inicio} if exist
                foreach (FormulaFieldDefinition ff in reportDocument.DataDefinition.FormulaFields)
                {
                    // FormulaName is an int-like index in some builds; use Name if available
                    string fname = ff.GetType().GetProperty("Name")?.GetValue(ff) as string ?? ff.FormulaName.ToString();
                    var name = fname.ToLower();
                    try
                    {
                        if (name.Contains("fecha_inicio") || name.Contains("@fecha_inicio"))
                        {
                            // set formula value text
                            ff.Text = "Date(" + filtro.inicio.ToString("yyyy") + "," + filtro.inicio.ToString("MM") + "," + filtro.inicio.ToString("dd") + ")";
                        }
                        else if (name.Contains("fecha_final") || name.Contains("@fecha_final"))
                        {
                            ff.Text = "Date(" + filtro.fin.ToString("yyyy") + "," + filtro.fin.ToString("MM") + "," + filtro.fin.ToString("dd") + ")";
                        }
                    }
                    catch { }
                }
            }


            // Exportar a PDF y devolver el arreglo de bytes
            using var stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] dataBytes = ms.ToArray();

            string documentPath = Convert.ToBase64String(dataBytes);
            response.Data = $"data:application/pdf;base64,{documentPath}";

            return response;
        }
    }


}
