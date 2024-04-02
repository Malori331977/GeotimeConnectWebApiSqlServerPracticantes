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
    public class PhPlanillaByUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhPlanillaByUsuarioController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }    

        [HttpGet("{idUsuario}")]
        public async Task<List<cPh_Planilla>> Get(string idUsuario) => await _repoGT.GetPhPlanillaByUsuario(idUsuario);

       

    }
}
