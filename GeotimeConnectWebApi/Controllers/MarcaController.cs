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

        [HttpGet("{idnumero}/{fecha}")]
        public async Task<IEnumerable<cMarca>> Get(string idnumero, string fecha) => await _repoGT.GetMarcas(idnumero,fecha);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarca> marcas)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Marca(marcas);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] cMarcaEditParam marca)
        {
            EventResponse respuesta = await _repoGT.EditarMarca(marca);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
