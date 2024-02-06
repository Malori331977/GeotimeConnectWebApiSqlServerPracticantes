using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IncidenciaConfPagoController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public IncidenciaConfPagoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cIncidencia_Conf_Pago>> Get() => await _repoGT.GetIncidencia_Conf_Pago();

        [HttpGet("{id}")]
        public async Task<cIncidencia_Conf_Pago> Get(int id) => await _repoGT.GetIncidencia_Conf_Pago(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cIncidencia_Conf_Pago> incidenciasConfPago)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Incidencia_Conf_Pago(incidenciasConfPago);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_Incidencia_Conf_Pago(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
