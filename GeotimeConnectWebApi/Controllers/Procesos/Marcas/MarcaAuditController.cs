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
    public class MarcaAuditController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaAuditController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idnumero}/{fecha}/{idplanilla}")]
        public async Task<IEnumerable<cMarcaAudit>> Get(string idnumero, string fecha, string idplanilla) => await _repoGT.GetMarcasAudit(idnumero,fecha,idplanilla);

        [HttpGet("{idnumero}/{fechaInicio}/{fechaFinal}/{idplanilla}")]
        public async Task<IEnumerable<cMarcaAudit>> Get(string idnumero, string fechaInicio, string fechaFinal, string idplanilla) => await _repoGT.GetMarcasAudit(idnumero, fechaInicio,fechaFinal, idplanilla);

    }
}
