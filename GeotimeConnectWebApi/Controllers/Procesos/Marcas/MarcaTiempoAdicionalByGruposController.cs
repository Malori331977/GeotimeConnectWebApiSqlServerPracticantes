using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaTiempoAdicionalByGruposController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaTiempoAdicionalByGruposController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

       
        [HttpGet("{idplanilla}/{idPeriodo}/{fechaInicio}/{fechaFin}/{idconcepto}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaTiempoAdicional>> Get(string idplanilla, string idPeriodo, string fechaInicio,string fechaFin, int idconcepto, string idgrupo) => await _repoGT.GetMarcasTiempoAdicional(idplanilla, idPeriodo, fechaInicio, fechaFin, idconcepto, idgrupo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cMarcaTiempoAdicional>? data;
            data = await Get(filtro.idplanilla!, filtro.idperiodo!, filtro.fechaInicio!, filtro.fechaFin!, (int)filtro.idconcepto!,filtro.idGrupos!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

        
    }
}
