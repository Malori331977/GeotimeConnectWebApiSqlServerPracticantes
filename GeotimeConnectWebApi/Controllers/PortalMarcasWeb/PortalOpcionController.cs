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
    public class PortalOpcionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PortalOpcionController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPortal_Opcion>> Get() => await _repoGT.GetPortalOpcion();      

        [HttpGet("{id}")]
        public async Task<cPortal_Opcion> Get(string id) => await _repoGT.GetPortalOpcion(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPortal_Opcion> portalOpcion)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PortalOpcion(portalOpcion);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
