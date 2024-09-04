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
using GeotimeConnectWebApi.Models;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaTiempoAdicionalController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaTiempoAdicionalController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }
        // .

        //[HttpGet("{fecha}")]
        //public async Task<IEnumerable<cMarcaProceso>> Get(string fecha) => await _repoGT.GetMarcasProceso(fecha);

        //[HttpGet("{idnumero}/{fecha}")]
        //public async Task<IEnumerable<cMarcaProceso>> Get(string idnumero, string fecha) => await _repoGT.GetMarcasProceso(idnumero,fecha);

        // GET para Edición Condiciones especiales
        [HttpGet("{idplanilla}/{fechainicio}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaTiempoAdicional>> Get(string idplanilla, string fechainicio, string idgrupo) => await _repoGT.GetMarcasTiempoAdicional(idplanilla, fechainicio, idgrupo);


        //[HttpPut]
        //public async Task<IActionResult> Put([FromBody] IEnumerable<cMarcaMovTurno> marcasMovTurno)
        //{
        //    EventResponse respuesta = await _repoGT.PutMarcasProceso(marcasMovTurno);

        //    if (respuesta.Id != "0")
        //        return BadRequest(respuesta);

        //    return Ok(respuesta);
        //}

    }
}
