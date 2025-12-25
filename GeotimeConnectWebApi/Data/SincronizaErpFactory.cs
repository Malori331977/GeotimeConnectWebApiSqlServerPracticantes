using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using JtSegEncrypta;
using Microsoft.EntityFrameworkCore;

public class SincronizaErpFactory : ISincronizaErpFactory
{
    private readonly ILogger<SincronizaErp> _logger;
   
    public SincronizaErpFactory(ILogger<SincronizaErp> logger)
    {
        _logger = logger;
    }

    public SincronizaErp Create(GeoTimeConnectService geoConnect, IErpConnectService erpConnect)
    {
        // Crea la instancia del servicio con las dependencias necesarias
        return new SincronizaErp(geoConnect, erpConnect, _logger);
    }
}
