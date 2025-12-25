using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using JtSegEncrypta;
using Microsoft.EntityFrameworkCore;

public class GeoTimeConnectServiceFactory : IGeoTimeConnectServiceFactory
{
    private readonly ILogger<GeoTimeConnectService> _logger;
    private readonly IEncriptaService _encriptaService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGraphSendMail _sendMail;

    public GeoTimeConnectServiceFactory(
        ILogger<GeoTimeConnectService> logger,
        IEncriptaService encriptaService,
        IGraphSendMail sendMail)
    {
        _logger = logger;
        _encriptaService = encriptaService;
        _sendMail = sendMail;
    }

    public GeoTimeConnectService Create(string schema, string bdname)
    {
        // Crea el contexto usando el método adecuado de tu proyecto
        SqlServerDataBaseContext context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);

        // Crea la instancia del servicio con las dependencias necesarias
        return new GeoTimeConnectService(
            context,
            _logger,          
            _encriptaService,
            _sendMail,           
            bdname,
            schema
        );
    }


}
