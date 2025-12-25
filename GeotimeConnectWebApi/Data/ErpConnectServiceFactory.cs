using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using JtSegEncrypta;
using Microsoft.EntityFrameworkCore;

public class ErpConnectServiceFactory : IErpConnectServiceFactory
{
    private readonly ILogger<ErpConnectServices> _logger;
    private readonly IGenericService _erpConnect;

    public ErpConnectServiceFactory(
        ILogger<ErpConnectServices> logger,
        IGenericService erpConnect)
    {
        _logger = logger;
        _erpConnect = erpConnect;
    }

    public ErpConnectServices Create(string schema, string bdname)
    {
        // Crea el contexto usando el método adecuado de tu proyecto
        SqlServerDataBaseContext context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);

        // Crea la instancia del servicio con las dependencias necesarias
        return new ErpConnectServices(
            context,
            _logger,
            _erpConnect,          
            bdname,
            schema
        );
    }


}
