using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
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

        [HttpDelete("{id}/{nivel}")]
        public async Task<IActionResult> Delete(int id, int nivel)
        {
            EventResponse respuesta = await _repoGT.Elimina_TransformacionTipoMarcaDet(id, nivel);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
