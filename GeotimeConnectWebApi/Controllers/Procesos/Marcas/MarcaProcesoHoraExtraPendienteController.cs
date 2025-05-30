using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaProcesoHoraExtraPendienteController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaProcesoHoraExtraPendienteController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        
        // Nuevo GET para Cambio Masivo, Entrada - Salida
        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}")]
        public async Task<IEnumerable<cMarcaProceso>> Get(string idplanilla, string fechainicio, string fechafin) => await _repoGT.GetMarcasProcesoHorasExtrasPendientes(idplanilla, fechainicio, fechafin);



    }
}
