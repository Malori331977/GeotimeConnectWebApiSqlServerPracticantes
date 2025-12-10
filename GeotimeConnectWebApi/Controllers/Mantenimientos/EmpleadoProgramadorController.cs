using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using System.Text.Json;
using com.gsitcr.geotime.Models.Response;

using System.Collections.Generic;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmpleadoProgramadorController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EmpleadoProgramadorController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{grupos}")]
        public async Task<IEnumerable<cEmpleado>> Get(string grupos) => await _repoGT.GetEmpleadoProgramador(grupos);

        [HttpGet("{idPlanilla}/{grupos}")]
        public async Task<IEnumerable<cEmpleado>> Get(string idPlanilla, string grupos) => await _repoGT.GetEmpleadoProgramador(idPlanilla, grupos);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroProceso filtro)
        {
            IEnumerable<cEmpleado>? data;
            if (filtro.idplanilla is null)
                data = await Get(filtro.idGrupos!);
            else
                data = await Get(filtro.idplanilla!, filtro.idGrupos!);

            EventResponse respuesta = new();
            respuesta.Data = JsonSerializer.Serialize(data);

            return Ok(respuesta);
        }

    }
}
