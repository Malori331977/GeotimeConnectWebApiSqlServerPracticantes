using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
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
               
                int frecuencia = 1;
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    await SincronizaCatalogosGeo();                    

                    await Task.Delay(TimeSpan.FromDays(frecuencia), stoppingToken);
                }
            }
            catch (OperationCanceledException exc)
            {
                logger.LogError($"HSSincronizacionAutomatica: Proceso de sincronización automática se detuvo: {exc.Message}");
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch (Exception ex)
            {
                logger.LogError($"HSSincronizacionAutomatica: Proceso de sincronización automática falló: {ex.Message}");

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }

        private async Task SincronizaCatalogosGeo()
        {

            var companias = await _geoConnect.GetPhCompania();

            foreach (var compania in companias)
            {
                var geoConnect = _geoFactory.Create(compania.IDCOMP, compania.APIDATABASE!);
                var erpConnect = _erpFactory.Create(compania.IDCOMP!, compania.APIDATABASE!);

                var _sincronizaErp = new SincronizaErp(geoConnect,erpConnect, _loggerSincronizaErp);

                Task task = new Task(async () =>
                {
                   // await _sincronizaErp.SincronizaDepartamentos();
                   // await _sincronizaErp.SincronizaCentrosCosto();
                    await _sincronizaErp.SincronizaPuestos();
                    //await _sincronizaErp.SincronizaEmpleados();
                });

                task.Start();



            }
        }

        

    }


}
