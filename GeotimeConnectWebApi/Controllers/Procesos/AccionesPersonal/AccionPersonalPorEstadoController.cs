using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GeoTimeConnectWebApi.Controllers.Procesos.AccionesPersonal
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccionPersonalPorEstadoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalPorEstadoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{estado}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, char estado ) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, estado);


        [HttpGet("{idplanilla}/{usuario}/{estado}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, string usuario, char estado) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, usuario, estado);

        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}/{estado}/{idincidencia}/{idgrupo}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, string fechainicio, string fechafin, char estado, int idincidencia, string idgrupo) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, fechainicio, fechafin, estado, idincidencia, idgrupo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cAccionPersonal>? data;
            data = await Get(filtro.idplanilla!, filtro.fechaInicio!,filtro.fechaFin!,filtro.estado??'A',(int)filtro.idincidencia!,filtro.idGrupos!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }
    }
}
