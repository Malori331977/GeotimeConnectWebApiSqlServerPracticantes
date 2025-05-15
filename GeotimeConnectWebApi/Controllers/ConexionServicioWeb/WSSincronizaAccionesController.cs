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
    public class WSSincronizaAccionesController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSSincronizaAccionesController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cSincronizo_Acciones> parametros)
        {
            EventResponse respuesta = await _repoGT.Sincronizo_Acciones(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
