using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TipoSolicitudController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public TipoSolicitudController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        
        [HttpGet]
        public async Task<IEnumerable<cTipoSolicitud>> Get() => await _repoGT.GetTipoSolicitud();


        [HttpGet("{id}")]
        public async Task<cTipoSolicitud> Get(int id) => await _repoGT.GetTipoSolicitud(id);

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cTipoSolicitud model)
        {
            EventResponse respuesta = await _repoGT.PostTipoSolicitud(model);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.DeleteTipoSolicitud(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
