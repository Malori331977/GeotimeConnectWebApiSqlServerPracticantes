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
    public class MarcaExtraApbController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaExtraApbController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idregistro}")]
        public async Task<cMarcaExtraApb> Get(long idregistro) => await _repoGT.GetMarcaExtraApb(idregistro);

        [HttpGet("{idnumero}/{fecha}/{idplanilla}")]
        public async Task<IEnumerable<cMarcaExtraApb>> Get(string idnumero, string fecha, string idplanilla) => await _repoGT.GetMarcaExtraApb(idnumero,fecha,idplanilla);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaExtraApb> marcaExtraApb)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcaExtraApb(marcaExtraApb);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
