using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Utils;
using System.Text.Json;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
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
