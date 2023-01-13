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
    public class MarcaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarca>> Get() => await _repoGT.GetMarcas();      

        [HttpGet("{idnumero}")]
        public async Task<IEnumerable<cMarca>> Get(string idnumero) => await _repoGT.GetMarcas(idnumero);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarca> marcas)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Marca(marcas);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
