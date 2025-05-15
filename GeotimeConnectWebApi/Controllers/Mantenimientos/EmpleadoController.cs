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
    public class EmpleadoController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EmpleadoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cEmpleado>> Get() => await _repoGT.GetEmpleado();

        [HttpGet("{idnumero}")]
        public async Task<cEmpleado> Get(string idnumero) => await _repoGT.GetEmpleado(idnumero);

        [HttpGet("{idnumero}/{nombre}/{iddepartamento}")]
        public async Task<IEnumerable<cEmpleado>> Get(string idnumero, string nombre, string iddepartamento) => await _repoGT.GetEmpleadoFiltrado(idnumero, nombre, iddepartamento);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cEmpleado> empleados)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Empleado(empleados);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idnumero}")]
        public async Task<IActionResult> Delete(string idnumero)
        {
            EventResponse respuesta = await _repoGT.Elimina_Empleado(idnumero);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
