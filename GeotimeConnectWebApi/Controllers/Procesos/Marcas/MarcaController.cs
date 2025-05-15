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
