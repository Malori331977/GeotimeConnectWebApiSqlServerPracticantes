using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
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
