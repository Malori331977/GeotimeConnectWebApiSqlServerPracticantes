using Microsoft.AspNetCore.Mvc;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _repoUser;

        public UserController(IUserService repoUser)
        {
            _repoUser = repoUser;
        }

        [HttpPost]
        public async Task<IActionResult> Autentificar([FromBody] UserRequest model)
        {
            UserResponse respuesta = await _repoUser.Auth(model);

            if (respuesta == null)
                return BadRequest("Ocurrió un error al autentificar la sesión.");

            return Ok(respuesta);
        }
  
    }
}
