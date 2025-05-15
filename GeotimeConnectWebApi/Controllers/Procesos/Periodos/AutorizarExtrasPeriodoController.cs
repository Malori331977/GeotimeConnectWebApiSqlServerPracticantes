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
    public class AutorizarExtrasPeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AutorizarExtrasPeriodoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cExtraAprobacion parametros)
        {
            EventResponse respuesta = await _repoGT.AutorizarExtrasPeriodo(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
