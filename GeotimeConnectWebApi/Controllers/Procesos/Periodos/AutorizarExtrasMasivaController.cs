using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Periodos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AutorizarExtrasMasivaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AutorizarExtrasMasivaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cExtraAprobacion> parametros)
        {
            EventResponse respuesta = await _repoGT.AutorizarExtrasMasiva(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<cExtraAprobacion> parametros)
        {
            EventResponse respuesta = await _repoGT.PreAutorizarExtrasMasiva(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
