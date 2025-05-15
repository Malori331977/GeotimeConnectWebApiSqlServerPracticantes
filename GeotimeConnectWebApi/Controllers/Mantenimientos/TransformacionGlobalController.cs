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
    public class TransformacionGlobalController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public TransformacionGlobalController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTransformacionGlobal>> Get() => await _repoGT.GetTransformacionGlobal();

        [HttpGet("{id}")]
        public async Task<cTransformacionGlobal> Get(int id) => await _repoGT.GetTransformacionGlobal(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cTransformacionGlobal> transformacion)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_TransformacionGlobal(transformacion);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_TransformacionGlobal(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
