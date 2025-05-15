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
