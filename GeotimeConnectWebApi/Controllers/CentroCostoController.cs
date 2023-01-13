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
    public class CentroCostoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public CentroCostoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cCentroCosto>> Get() => await _repoGT.GetCentroCosto();      

        [HttpGet("{idccosto}")]
        public async Task<cCentroCosto> Get(string idccosto) => await _repoGT.GetCentroCosto(idccosto);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cCentroCosto> centrosCosto)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Centro_Costo(centrosCosto);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
