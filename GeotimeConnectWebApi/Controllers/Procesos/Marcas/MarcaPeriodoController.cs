using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaPeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaPeriodoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{idsGrupos}/{idPlanilla}/{fechaInicio}/{fechaFin}/{idnumero}")]
        public async Task<IEnumerable<cMarcaPeriodo>> Get(string idsGrupos, string idPlanilla, string fechaInicio, string fechaFin, string idnumero) => await _repoGT.GetMarcasPeriodo(idsGrupos, idPlanilla, fechaInicio, fechaFin, idnumero);


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cMarcaPeriodo>? data;
            data = await Get(filtro.idGrupos!, filtro.idplanilla!, filtro.fechaInicio!, filtro.fechaFin!, filtro.idnumero!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

    }
}
