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
    public class TransformacionTipoMarcaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public TransformacionTipoMarcaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTransformacionTipoMarca>> Get() => await _repoGT.GetTransformacionTipoMarca();

        [HttpGet("{id}")]
        public async Task<cTransformacionTipoMarca> Get(int id) => await _repoGT.GetTransformacionTipoMarca(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cTransformacionTipoMarca> transformacionTM)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_TransformacionTipoMarca(transformacionTM);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_TransformacionTipoMarca(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
