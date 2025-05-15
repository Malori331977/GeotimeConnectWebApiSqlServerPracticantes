using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.PortalMarcasWeb
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PortalConfigController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public PortalConfigController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet]
        public async Task<cPortal_Config> Get() => await _repoGT.GetPortalConfig();

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cPortal_Config portalConfig)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PortalConfig(portalConfig);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
