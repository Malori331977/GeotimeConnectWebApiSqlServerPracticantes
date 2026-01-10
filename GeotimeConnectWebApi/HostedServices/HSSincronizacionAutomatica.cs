using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Utils;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KolegioApi.Data
{

    public class HSSincronizacionAutomatica : BackgroundService 
    {
        private readonly ILogger<HSSincronizacionAutomatica> logger;
        private readonly IGeoTimeConnectService _geoConnect;
        private IGeoTimeConnectServiceFactory _geoFactory;
        private IErpConnectServiceFactory _erpFactory;
        private ILogger<SincronizaErp> _loggerSincronizaErp;

        public HSSincronizacionAutomatica(ILogger<HSSincronizacionAutomatica> _logger, 
                                          IServiceScopeFactory factory,
                                          ILogger<SincronizaErp> loggerSincronizaErp)
        {
            logger = _logger;
            _loggerSincronizaErp = loggerSincronizaErp;

            
            _geoConnect = factory.CreateScope().ServiceProvider.GetRequiredService<IGeoTimeConnectService>();
            _geoFactory = factory.CreateScope().ServiceProvider.GetRequiredService<IGeoTimeConnectServiceFactory>();
            _erpFactory = factory.CreateScope().ServiceProvider.GetRequiredService<IErpConnectServiceFactory>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var section = config.GetSection("AppSettings");
                var ERPTimeSinc = section.GetSection("ERPTimeSinc").Value!;
                int frecuencia = 0;

                if (!String.IsNullOrEmpty(ERPTimeSinc))
                    frecuencia = int.Parse(ERPTimeSinc);

                if (frecuencia > 0)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await SincronizaCatalogosGeo();

                        await Task.Delay(TimeSpan.FromHours(frecuencia), stoppingToken);
                    }
                }

                
            }
            catch (OperationCanceledException exc)
            {
                logger.LogError($"HSSincronizacionAutomatica: Proceso de sincronización automática se detuvo: {exc.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"HSSincronizacionAutomatica: Proceso de sincronización automática falló: {ex.Message}");
            }
        }

        private async Task SincronizaCatalogosGeo()
        {
            try
            {
                logger.LogWarning("HSSincronizacionAutomatica: Iniciando proceso de sincronización automática de catálogos desde Control de Asistencia.");
                var companias = await _geoConnect.GetPhCompania();

                foreach (var compania in companias)
                {
                    if (!String.IsNullOrEmpty(compania.APIURLERP) && !String.IsNullOrEmpty(compania.APIDATABASEERP) && !String.IsNullOrEmpty(compania.APISCHEMAERP))
                    {
                        var geoConnect = _geoFactory.Create(compania.IDCOMP, compania.APIDATABASE!);
                        var erpConnect = _erpFactory.Create(compania.IDCOMP!, compania.APIDATABASE!);

                        var _sincronizaErp = new SincronizaErp(geoConnect, erpConnect, _loggerSincronizaErp);

                        Task task = new Task(async () =>
                        {
                            if (compania.SINCAUTODEPTO == 'T') await _sincronizaErp.SincronizaDepartamentos();
                            if (compania.SINCAUTOCCOSTO == 'T') await _sincronizaErp.SincronizaCentrosCosto();
                            if (compania.SINCAUTOPUESTO == 'T') await _sincronizaErp.SincronizaPuestos();
                            if (compania.SINCAUTONOMINA == 'T') await _sincronizaErp.SincronizaNominas();
                            if (compania.SINCAUTOEMPLEADO == 'T') await _sincronizaErp.SincronizaEmpleados();
                            await _sincronizaErp.PostSincronizacion("-1");
                        });

                        task.Start();
                    }
                    
                }
                logger.LogWarning("HSSincronizacionAutomatica: Proceso de sincronización automática de catálogos desde Control de Asistencia finalizado.");
            }
            catch (Exception ex)
            {
                logger.LogError($"HSSincronizacionAutomatica: Error en el proceso de sincronización automática de catálogos desde Control de Asistencia: {ex.Message}");

            }  
                
        }

        

    }


}
