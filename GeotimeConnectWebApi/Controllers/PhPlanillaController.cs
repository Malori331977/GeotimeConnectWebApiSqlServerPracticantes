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
    public class PhPlanillaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhPlanillaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Planilla>> Get() => await _repoGT.GetPhPlanilla();      

        [HttpGet("{idplanilla}")]
        public async Task<cPh_Planilla> Get(string idplanilla) => await _repoGT.GetPhPlanilla(idplanilla);

        [HttpGet("{nomconector}/{descplanilla}")]
        public async Task<cPh_Planilla> Get(string nomconector, string descplanilla) => await _repoGT.GetPhPlanilla(nomconector, descplanilla);

    }
}
