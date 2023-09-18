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

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaMovTurnoByGrupoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaMovTurnoByGrupoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }
    
        [HttpGet("{fechaPeriodo}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaMovTurno>> Get(string fechaPeriodo, string idgrupo) => await _repoGT.GetMarcaMovTurnoByGrupo(fechaPeriodo,idgrupo);

    }
}
