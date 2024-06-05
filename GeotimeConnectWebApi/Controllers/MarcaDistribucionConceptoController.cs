using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Utils;
using System.Text.Json;
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaDistribucionConceptoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaDistribucionConceptoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaDistribucionConcepto>> Get() => await _repoGT.GetMarcaDtnConcepto();      

        [HttpGet("{idregistro}")]
        public async Task<cMarcaDistribucionConcepto> Get(int idregistro) => await _repoGT.GetMarcaDtnConcepto(idregistro);

        [HttpGet("{idnumero}/{fecha}/{idplanilla}")]
        public async Task<cMarcaDistribucionConcepto> Get(string idnumero, string fecha, string idplanilla) => await _repoGT.GetMarcaDtnConcepto(idnumero, fecha, idplanilla);

        [HttpGet("{idPlanilla}/{fechaInicio}/{fechaFin}/{idnumero}")]
        public async Task<IEnumerable<cMarcaDistribucionConcepto>> Get(string idPlanilla, string fechaInicio, string fechaFin, string idnumero) => await _repoGT.GetMarcaDtnConcepto(idPlanilla, fechaInicio, fechaFin, idnumero);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaDistribucionConcepto> marcaDC)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcaDtnConcepto(marcaDC);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idregistro}")]
        public async Task<IActionResult> Delete(long idregistro)
        {
            EventResponse respuesta = await _repoGT.Elimina_MarcaDtnConcepto(idregistro);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
