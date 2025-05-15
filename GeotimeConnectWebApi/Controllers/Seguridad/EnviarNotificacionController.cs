using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EnviarNotificacionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EnviarNotificacionController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<Email> email)
        {
            EventResponse respuesta = await _repoGT.EnviarCorreo(email);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
