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
    public class MarcaMovHorarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaMovHorarioController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaMovHorario>> Get() => await _repoGT.GetMarcaMovHorario();      

        [HttpGet("{idregistro}")]
        public async Task<cMarcaMovHorario> Get(int idregistro) => await _repoGT.GetMarcaMovHorario(idregistro);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaMovHorario> marcasMovHorarios)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcasMovHorario(marcasMovHorarios);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
