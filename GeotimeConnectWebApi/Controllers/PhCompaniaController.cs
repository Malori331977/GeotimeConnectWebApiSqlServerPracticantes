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
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;
using GeotimeConnectWebApi.Models;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhCompaniaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhCompaniaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Compania>> Get() => await _repoGT.GetPhCompania();      

        [HttpGet("{idcomp}")]
        public async Task<cPh_Compania> Get(string idcomp) => await _repoGT.GetPhCompania(idcomp);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Compania> phCompanias)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhCompania(phCompanias);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
