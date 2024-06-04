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
    public class MarcaDistribucionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaDistribucionController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{idPlanilla}/{fechaInicio}/{fechaFin}/{idnumero}")]
        public async Task<IEnumerable<cMarcaDistribucion>> Get(string idPlanilla, string fechaInicio, string fechaFin, string idnumero) => await _repoGT.GetMarcaDistribucion(idPlanilla, fechaInicio, fechaFin, idnumero);


    }
}
