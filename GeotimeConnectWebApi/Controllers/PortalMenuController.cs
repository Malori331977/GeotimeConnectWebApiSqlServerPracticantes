using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Utils;
using System.Text.Json;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
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
