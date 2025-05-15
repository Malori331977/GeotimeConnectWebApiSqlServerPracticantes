using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using System.Text.Json;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmpleadoCambioPasswordController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EmpleadoCambioPasswordController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cEmpleado empleado)
        {
            EventResponse respuesta = await _repoGT.CambiarClaveEmpleado(empleado);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        
    }
}
