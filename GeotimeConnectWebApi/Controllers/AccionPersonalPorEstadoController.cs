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
    public class AccionPersonalPorEstadoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalPorEstadoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{estado}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, char estado ) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, estado);


        [HttpGet("{idplanilla}/{usuario}/{estado}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, string usuario, char estado) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, usuario, estado);

    }
}
