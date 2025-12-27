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
using System.Collections.Generic;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SincronizaNominaController : Controller
    {
        private readonly ISincronizaErp _repoGT;
        public SincronizaNominaController(ISincronizaErp repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cSincronizo_erp param)
        {
            EventResponse respuesta = await _repoGT.SincronizaNominas();

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
