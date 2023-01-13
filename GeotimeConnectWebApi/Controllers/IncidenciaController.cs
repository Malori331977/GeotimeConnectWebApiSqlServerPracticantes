using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IncidenciaController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public IncidenciaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cIncidencia>> Get() => await _repoGT.GetIncidencia();


        [HttpGet("{id}")]
        public async Task<cIncidencia> Get(int id) => await _repoGT.GetIncidencia(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cIncidencia> incidencias)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Incidencia(incidencias);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
