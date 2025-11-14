using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    [ApiController]
    [Route("[controller]")]
   
    public class SolicitudAutorizaController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudAutorizaController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cSolicitud> solAccionPersonal)
        {
            EventResponse respuesta = await _repoGT.AutorizaSolicitud(solAccionPersonal);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]string fechas)
        {
            EventResponse respuesta = await _repoGT.ReAplicarSolicitudesAprobadas(fechas);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
