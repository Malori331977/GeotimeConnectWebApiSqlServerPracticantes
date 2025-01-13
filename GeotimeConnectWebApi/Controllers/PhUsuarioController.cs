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
    public class PhUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhUsuarioController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }     

        [HttpGet("{idnumero}")]
        public async Task<cPh_Usuario> Get(string idnumero) => await _repoGT.GetPhUsuario(idnumero);

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] cPh_Usuario usuario)
        {
            EventResponse respuesta = await _repoGT.PutPhUsuario(usuario);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
