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

namespace GeoTimeConnectWebApi.Controllers.Configuraciones
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhRolSistemaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhRolSistemaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_RolSistema>> Get() => await _repoGT.GetPhRolSistema();      

        [HttpGet("{id}")]
        public async Task<cPh_RolSistema> Get(string id) => await _repoGT.GetPhRolSistema(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_RolSistema> roles)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhRolSistema(roles);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
