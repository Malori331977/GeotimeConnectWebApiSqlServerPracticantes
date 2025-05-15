using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Utils;
using System.Text.Json;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhNivelController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhNivelController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Nivel>> Get() => await _repoGT.GetNivel();      

        [HttpGet("{id}")]
        public async Task<cPh_Nivel> Get(int id) => await _repoGT.GetNivel(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Nivel> niveles)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Nivel(niveles);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_Nivel(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
