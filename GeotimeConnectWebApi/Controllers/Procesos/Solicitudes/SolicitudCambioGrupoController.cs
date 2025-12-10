using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SolicitudCambioGrupoController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudCambioGrupoController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<cSolicitud> solicitud)
        {
            EventResponse respuesta = await _repoGT.PutSolicitudActGrupo(solicitud);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
