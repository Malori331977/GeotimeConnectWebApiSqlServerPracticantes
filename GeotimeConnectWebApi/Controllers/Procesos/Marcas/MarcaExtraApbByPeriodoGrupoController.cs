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
    public class MarcaExtraApbByPeriodoGrupoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaExtraApbByPeriodoGrupoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{fechaPeriodo}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaExtraApb>> Get(string fechaPeriodo, string idgrupo) => await _repoGT.GetMarcaExtraApb(fechaPeriodo, idgrupo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cMarcaExtraApb>? data;
            data = await Get(filtro.fechaInicio!, filtro.idGrupos!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

    }
}
