using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IncidenciaController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public IncidenciaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cIncidencia>> Get() => await _repoGT.GetIncidencia();

        [HttpGet("{id}")]
        public async Task<cIncidencia> Get(int id) => await _repoGT.GetIncidencia(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cIncidencia> incidencias)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Incidencia(incidencias);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_Incidencia(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
