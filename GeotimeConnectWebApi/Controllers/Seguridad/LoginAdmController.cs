using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LoginAdmController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public LoginAdmController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }
       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cLogin login)
        {
            EventResponse respuesta = await _repoGT.ValidarClaveAdm(login);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
