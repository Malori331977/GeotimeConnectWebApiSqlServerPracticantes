using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace com.gsitcr.geotime.Controllers.ConexionServicioWeb
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WSInitPeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSInitPeriodoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cInit_Periodo> parametros)
        {
            EventResponse respuesta = await _repoGT.Init_Periodo(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
