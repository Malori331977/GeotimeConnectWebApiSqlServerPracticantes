using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Utils;
using System.Text.Json;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaExtraApbByPlanillaGrupoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaExtraApbByPlanillaGrupoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idsgrupos}/{idplanilla}/{fechaInicio}/{fechaFinal}/{estado}")]
        public async Task<IEnumerable<cMarcaExtraApb>> Get(string idsgrupos, string idplanilla, string fechaInicio, string fechaFinal, char estado) => await _repoGT.GetMarcaExtraApb(idsgrupos, idplanilla, fechaInicio, fechaFinal,estado);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cMarcaExtraApb>? data;
            data = await Get(filtro.idGrupos!,filtro.idplanilla!, filtro.fechaInicio!, filtro.fechaFin!, filtro.estado??'N');

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

    }
}
