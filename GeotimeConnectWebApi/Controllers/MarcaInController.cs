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

namespace GeoTimeConnectWebApi.Controllers
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
