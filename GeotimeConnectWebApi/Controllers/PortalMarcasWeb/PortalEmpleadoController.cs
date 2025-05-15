using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.PortalMarcasWeb
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PortalEmpleadoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PortalEmpleadoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPortal_Empleado>> Get() => await _repoGT.GetPortalEmpleado();      

        [HttpGet("{id}")]
        public async Task<cPortal_Empleado> Get(string id) => await _repoGT.GetPortalEmpleado(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPortal_Empleado> portalEmpleado)
        {

            EventResponse respuesta = await _repoGT.Sincronizar_PortalEmpleado(portalEmpleado);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] cPortal_Empleado portalEmpleado)
        {

            EventResponse respuesta = await _repoGT.PutPortalEmpleado(portalEmpleado);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
