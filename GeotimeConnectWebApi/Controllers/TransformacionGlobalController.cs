using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Utils;
using System.Text.Json;
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;
using GeotimeConnectWebApi.Models;

namespace GeoTimeConnectWebApi.Controllers
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
