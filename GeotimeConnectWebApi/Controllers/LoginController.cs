using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Response;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LoginController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public LoginController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{email}")]
        public async Task<cEmpleado> Get(string email) => await _repoGT.GetEmpleadoByEmail(email);

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cLogin login)
        {
            EventResponse respuesta = await _repoGT.ValidarClaveEmpleado(login);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
