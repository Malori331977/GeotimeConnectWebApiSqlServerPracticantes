using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using System.Text.Json;
using com.gsitcr.geotime.Models.Response;


namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmpleadoProgramadorByHorarioController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EmpleadoProgramadorByHorarioController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idPlanilla}/{horarios}")]
        public async Task<IEnumerable<cEmpleado>> Get(string idPlanilla, string horarios) => await _repoGT.GetEmpleadoProgramadorByHorario(idPlanilla, horarios);


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cEmpleado>? data;
            data = await _repoGT.GetEmpleadoProgramadorByHorario(filtro.idplanilla!, filtro.idGrupos!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

    }
}
