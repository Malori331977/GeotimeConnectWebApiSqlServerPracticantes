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

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaExtraApbController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaExtraApbController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idregistro}")]
        public async Task<cMarcaExtraApb> Get(long idregistro) => await _repoGT.GetMarcaExtraApb(idregistro);

        [HttpGet("{idnumero}/{fecha}/{idplanilla}/{byPeriodo}")]
        public async Task<IEnumerable<cMarcaExtraApb>> Get(string idnumero, string fecha, string idplanilla, bool byPeriodo) => await _repoGT.GetMarcaExtraApb(idnumero,fecha,idplanilla, byPeriodo);

        [HttpGet("{fechaPeriodo}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaExtraApb>> Get(string fechaPeriodo, string idgrupo) => await _repoGT.GetMarcaExtraApb(fechaPeriodo, idgrupo);

        [HttpGet("{idsgrupos}/{idplanilla}/{fechaInicio}/{fechaFinal}/{estado}")]
        public async Task<IEnumerable<cMarcaExtraApb>> Get(string idsgrupos, string idplanilla, string fechaInicio, string fechaFinal, char estado) => await _repoGT.GetMarcaExtraApb(idsgrupos, idplanilla, fechaInicio, fechaFinal,estado);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaExtraApb> marcaExtraApb)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcaExtraApb(marcaExtraApb);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<cMarcaExtraApb> marcaExtraApb)
        {
            EventResponse respuesta = await _repoGT.Autorizar_MarcaExtraApb(marcaExtraApb);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
