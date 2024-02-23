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
        public async Task<cPortal_Rol> Get(int id) => await _repoGT.GetPortalRol(id);

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
