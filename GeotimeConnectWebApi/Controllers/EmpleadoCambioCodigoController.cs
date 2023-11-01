using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using System.Text.Json;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmpleadoCambioCodigoController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EmpleadoCambioCodigoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cEmpleado empleado)
        {
            EventResponse respuesta = await _repoGT.CambiarCodigoSeguridadEmpleado(empleado);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        
    }
}
