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
    public class ConceptoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public ConceptoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cConcepto>> Get() => await _repoGT.GetConcepto();      

        [HttpGet("{concepto}")]
        public async Task<cConcepto> Get(string concepto) => await _repoGT.GetConcepto(concepto);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cConcepto> conceptos)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Concepto(conceptos);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
