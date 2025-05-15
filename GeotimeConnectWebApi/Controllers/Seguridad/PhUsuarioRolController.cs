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
    public class PhUsuarioRolController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhUsuarioRolController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }     

        [HttpGet("{id}")]
        public async Task<IEnumerable<cPh_UsuarioRol>> Get(int id) => await _repoGT.GetPhUsuarioRol(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_UsuarioRol> usuarioRoles)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhUsuarioRol(usuarioRoles);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
