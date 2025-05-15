using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Biometricos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RelojUsuarioController : Controller
    {
        private readonly IRelojesServices _repoGT;
        public RelojUsuarioController(IRelojesServices repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cRelojUsuario>> Get() => await _repoGT.GetRelojUsuario();      

        [HttpGet("{id}")]
        public async Task<cRelojUsuario> Get(string id) => await _repoGT.GetRelojUsuario(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cRelojUsuario> usuarios)
        {
            EventResponse respuesta = await _repoGT.PostRelojUsuario(usuarios);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
