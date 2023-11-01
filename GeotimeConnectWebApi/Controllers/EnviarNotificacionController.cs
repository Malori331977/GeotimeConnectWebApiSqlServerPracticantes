
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GeoTimeConnectWebApi.Controllers
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
        public async Task<IActionResult> Post([FromBody] Email email)
        {
            EventResponse respuesta = await _repoGT.EnviarCorreo(email);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
