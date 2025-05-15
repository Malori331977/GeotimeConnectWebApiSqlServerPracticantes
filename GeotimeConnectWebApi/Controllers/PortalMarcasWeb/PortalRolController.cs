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
    public class PortalRolController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PortalRolController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPortal_Rol>> Get() => await _repoGT.GetPortalRol();      

        [HttpGet("{id}")]
        public async Task<cPortal_Rol> Get(string id) => await _repoGT.GetPortalRol(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPortal_Rol> portalRol)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PortalRol(portalRol);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
