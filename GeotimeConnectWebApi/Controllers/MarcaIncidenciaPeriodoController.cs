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
    public class MarcaIncidenciaPeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaIncidenciaPeriodoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{fechaInicio}/{fechaFinal}")]
        public async Task<IEnumerable<cMarcaIncidencia>> Get(string idplanilla, string fechaInicio, string fechaFinal) => await _repoGT.GetMarcaIncidenciaPeriodo(idplanilla, fechaInicio, fechaFinal);

        [HttpGet("{idplanilla}/{fechaInicio}/{fechaFinal}/{idnumero}")]
        public async Task<IEnumerable<cMarcaIncidencia>> Get(string idplanilla, string fechaInicio, string fechaFinal,string idnumero) => await _repoGT.GetMarcaIncidenciaPeriodo(idplanilla, fechaInicio, fechaFinal,idnumero);
    }
}
