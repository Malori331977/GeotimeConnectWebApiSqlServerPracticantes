using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EstadoController : Controller
    {
        private readonly IFlujosAutorizacionService _repoGT;
        public EstadoController(IFlujosAutorizacionService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cEstado>> Get() => await _repoGT.GetEstado();

        [HttpGet("{id}")]
        public async Task<cEstado> Get(int id) => await _repoGT.GetEstado(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cEstado estado)
        {
            EventResponse respuesta = await _repoGT.PostEstado (estado);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.DeleteEstado(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
