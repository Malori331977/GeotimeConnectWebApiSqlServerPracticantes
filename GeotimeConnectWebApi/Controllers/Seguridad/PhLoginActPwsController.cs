using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    public class PhLoginActPwsController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhLoginActPwsController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPut]
        public async Task<IActionResult> Post()
        {
            EventResponse respuesta = await _repoGT.ActualizaPwdsUsuariosBD();

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }



    }
}
