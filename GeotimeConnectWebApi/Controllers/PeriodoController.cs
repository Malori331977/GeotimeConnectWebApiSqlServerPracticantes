﻿using Microsoft.AspNetCore.Authorization;
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
    public class PeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PeriodoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Periodos>> Get() => await _repoGT.GetPeriodo();      

        [HttpGet("{idperiodo}")]
        public async Task<cPh_Periodos> Get(string idperiodo) => await _repoGT.GetPeriodo(idperiodo);

		[HttpGet("{fecha}/{vigente}")]
		public async Task<IEnumerable<cPh_Periodos>> Get(string fecha, string vigente) => await _repoGT.GetPeriodo(fecha, vigente);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Periodos> ph_periodo)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Periodo(ph_periodo);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.Elimina_Periodo(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
