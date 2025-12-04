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
    public class MarcaProcesoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaProcesoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{fecha}")]
        public async Task<IEnumerable<cMarcaProceso>> Get(string fecha) => await _repoGT.GetMarcasProceso(fecha);

        [HttpGet("{idnumero}/{fecha}")]
        public async Task<IEnumerable<cMarcaProceso>> Get(string idnumero, string fecha) => await _repoGT.GetMarcasProceso(idnumero,fecha);

        // Nuevo GET para Cambio Masivo, Entrada - Salida
        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaProceso>> Get(string idplanilla, string fechainicio, string fechafin, string idgrupo) => await _repoGT.GetMarcasProceso(idplanilla, fechainicio, fechafin, idgrupo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cMarcaProceso>? data;
            data = await Get(filtro.idplanilla!, filtro.fechaInicio!, filtro.fechaFin!, filtro.idGrupos!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<cMarcaMovTurno> marcasMovTurno)
        {
            EventResponse respuesta = await _repoGT.PutMarcasProceso(marcasMovTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
