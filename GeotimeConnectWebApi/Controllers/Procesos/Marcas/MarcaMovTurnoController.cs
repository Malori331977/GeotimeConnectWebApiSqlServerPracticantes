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

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
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

        [HttpGet("{idplanilla}/{estado}/{fechaInicio}/{fechaFinal}")]
        public async Task<IEnumerable<cMarcaMovTurno>> Get(string idplanilla, string estado, string fechaInicio, string fechaFinal) => await _repoGT.GetMarcaMovTurno(idplanilla, estado, fechaInicio, fechaFinal);

        [HttpGet("{idnumero}/{fechaPeriodo}")]
        public async Task<IEnumerable<cMarcaMovTurno>> Get(string idnumero, string fechaPeriodo) => await _repoGT.GetMarcaMovTurno(idnumero,fechaPeriodo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaMovTurno> marcaMovTurno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcasMovTurnos(marcaMovTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] cMarcaMovTurno marcaMovTurno)
        {
            EventResponse respuesta = await _repoGT.PutMarcasMovTurnos(marcaMovTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
