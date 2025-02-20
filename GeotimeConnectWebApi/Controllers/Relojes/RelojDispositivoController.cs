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
    public class RelojDispositivoController : Controller
    {
        private readonly IRelojesServices _repoGT;
        public RelojDispositivoController(IRelojesServices repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cRelojDispositivo>> Get() => await _repoGT.GetRelojDispositivo();      

        [HttpGet("{id}")]
        public async Task<cRelojDispositivo> Get(int id) => await _repoGT.GetRelojDispositivo(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cRelojDispositivo> dispositivos)
        {
            EventResponse respuesta = await _repoGT.PostRelojDispositivo(dispositivos);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
