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
    public class SolicitudConfiguracionController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudConfiguracionController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        
        [HttpGet]
        public async Task<IEnumerable<cSolicitudConfiguracion>> Get() => await _repoGT.GetSolicitudConfiguracion();


        [HttpGet("{id}")]
        public async Task<cSolicitudConfiguracion> Get(string id) => await _repoGT.GetSolicitudConfiguracion(id);

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cSolicitudConfiguracion model)
        {
            EventResponse respuesta = await _repoGT.PostSolicitudConfiguracion(model);

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
