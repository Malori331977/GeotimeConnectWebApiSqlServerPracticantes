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
    public class AccionPersonalController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cAccionPersonal>> Get() => await _repoGT.GetAccionPersonal();

        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, DateTime fechainicio, DateTime fechafin) => await _repoGT.GetAccionPersonal(idplanilla, fechainicio, fechafin);

        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}/{usuario}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, DateTime fechainicio, DateTime fechafin,string usuario) => await _repoGT.GetAccionPersonal(idplanilla, fechainicio, fechafin,usuario);


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_AccionPersonal(accionPersonal);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
