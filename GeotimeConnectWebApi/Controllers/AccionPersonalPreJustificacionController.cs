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
    public class AccionPersonalPreJustificacionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalPreJustificacionController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_AccionPersonal_PreJustificacion(accionPersonal);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
