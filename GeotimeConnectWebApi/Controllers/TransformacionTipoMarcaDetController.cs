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
    public class TransformacionTipoMarcaDetController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public TransformacionTipoMarcaDetController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<cTransformacionTipoMarcaDet>> Get(int id) => await _repoGT.GetTransformacionTipoMarcaDet(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cTransformacionTipoMarcaDet> transformacionTMD)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_TransformacionTipoMarcaDet(transformacionTMD);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
