using Microsoft.AspNetCore.Mvc;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data;

namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _repoUser;
        private ILogger<UserService> _logger;
        public UserController(IUserService repoUser, ILogger<UserService> logger )
        {
            _repoUser = repoUser;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Autentificar([FromBody] UserRequest model)
        {
            _logger.LogInformation($"Llegó a metodo User");
            UserResponse respuesta = await _repoUser.Auth(model);

            if (respuesta == null)
                return BadRequest("Ocurrió un error al autentificar la sesión.");

            return Ok(respuesta);
        }
  
    }
}
