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
    public class PortalMenuController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PortalMenuController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPortal_Menu>> Get() => await _repoGT.GetPortalMenu();      

        [HttpGet("{id}")]
        public async Task<cPortal_Menu> Get(string id) => await _repoGT.GetPortalMenu(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPortal_Menu> portalMenu)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PortalMenu(portalMenu);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
