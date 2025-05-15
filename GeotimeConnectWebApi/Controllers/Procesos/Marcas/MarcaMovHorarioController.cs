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

        [HttpGet("{idplanilla}/{estado}/{fechaInicio}/{fechaFinal}")]
        public async Task<IEnumerable<cMarcaMovHorario>> Get(string idplanilla, string estado, string fechaInicio, string fechaFinal) => await _repoGT.GetMarcaMovHorario(idplanilla, estado, fechaInicio, fechaFinal);


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
