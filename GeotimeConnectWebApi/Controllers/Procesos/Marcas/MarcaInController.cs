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

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaInController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaInController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaIn>> Get() => await _repoGT.GetMarcaIn();      

        [HttpGet("{idtarjeta}")]
        public async Task<IEnumerable<cMarcaIn>> Get(string idtarjeta) => await _repoGT.GetMarcaIn(idtarjeta);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaIn> marcasIn)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcaIn(marcasIn);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
