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
    public class MarcaMovTurnoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaMovTurnoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaMovTurno>> Get() => await _repoGT.GetMarcaMovTurno();      

        [HttpGet("{idregistro}")]
        public async Task<cMarcaMovTurno> Get(int idregistro) => await _repoGT.GetMarcaMovTurno(idregistro);

        [HttpGet("{idnumero}/{fecha}/{idturno}")]
        public async Task<cMarcaMovTurno> Get(string idnumero, string fecha, int idturno) => await _repoGT.GetMarcaMovTurno(idnumero, fecha, idturno);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaMovTurno> marcaMovTurno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcasMovTurnos(marcaMovTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
