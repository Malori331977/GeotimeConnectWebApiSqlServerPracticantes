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
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhRolController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhRolController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Rol>> Get() => await _repoGT.GetPhRol();

        [HttpGet("{idrol}")]
        public async Task<cPh_Rol> Get(int idrol) => await _repoGT.GetPhRol(idrol);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Rol> phRol)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhRol(phRol);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idrol}")]
        public async Task<IActionResult> Delete(int idrol)
        {
            EventResponse respuesta = await _repoGT.Elimina_PhRol(idrol);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
