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
    public class PhDescansoTurnoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhDescansoTurnoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idTurno}/{idTiempo}")]
        public async Task<IEnumerable<cPh_DescansoTurno>> Get(int idTurno, int idTiempo) => await _repoGT.GetPhDescansoTurno(idTurno, idTiempo);      


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_DescansoTurno> ph_DescansoTurno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhDescansoTurno(ph_DescansoTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
