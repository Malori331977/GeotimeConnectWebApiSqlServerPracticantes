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
    public class SolicitudController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cSolicitud>> Get() => await _repoGT.GetSolicitud();

        [HttpGet("{id}")]
        public async Task<cSolicitud> Get(int id) => await _repoGT.GetSolicitud(id);

        [HttpGet("{idnumero}/{fechainicial}/{fechafinal}/{tipoSolicitud}")]
        public async Task<IEnumerable<cSolicitud>> Get(string idnumero, string fechainicial, string fechafinal, int tipoSolicitud) => await _repoGT.GetSolicitud(idnumero, fechainicial, fechafinal, tipoSolicitud);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cSolicitud> solicitud)
        {
            EventResponse respuesta = await _repoGT.PostSolicitud(solicitud);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<cSolicitud> solicitud)
        {
            EventResponse respuesta = await _repoGT.PutSolicitud(solicitud);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
