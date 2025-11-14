using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    [ApiController]
    [Route("[controller]")]
   
    public class SolicitudReAplicarController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudReAplicarController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }
       
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]int id)
        {
            EventResponse respuesta = await _repoGT.ReAplicarSolicitudesAprobadasById(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
