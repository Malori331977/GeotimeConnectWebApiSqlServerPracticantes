using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.AccionesPersonal
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccionPersonalCAUpdateController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalCAUpdateController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_AccionPersonal_CAUpdate(accionPersonal);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
