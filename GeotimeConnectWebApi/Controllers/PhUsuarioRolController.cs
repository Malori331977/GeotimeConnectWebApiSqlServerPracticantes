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
    public class PhUsuarioRolController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhUsuarioRolController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }     

        [HttpGet("{id}")]
        public async Task<IEnumerable<cPh_UsuarioRol>> Get(int id) => await _repoGT.GetPhUsuarioRol(id);

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_UsuarioRol> usuarioRoles)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhUsuarioRol(usuarioRoles);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
