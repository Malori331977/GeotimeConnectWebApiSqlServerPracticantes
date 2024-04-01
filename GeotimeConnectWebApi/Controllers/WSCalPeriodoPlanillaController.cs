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
    public class WSCalPeriodoPlanillaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSCalPeriodoPlanillaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cCal_Periodo_Planilla> parametros)
        {
            EventResponse respuesta = await _repoGT.Cal_Periodo_Planilla(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
