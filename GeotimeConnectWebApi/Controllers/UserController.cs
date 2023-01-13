using Microsoft.AspNetCore.Mvc;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
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
        public IActionResult Autentificar([FromBody] UserRequest model)
        {
            UserResponse respuesta = _repoUser.Auth(model);

            if (respuesta == null)
                return BadRequest("Ocurrió un error al autentificar la sesión.");

            return Ok(respuesta);
        }
  
    }
}
