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
    public class MarcaDescansoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaDescansoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idnumero}/{fecha}/{idplanilla}")]
        public async Task<IEnumerable<cMarcaDescanso>> Get(string idnumero, string fecha, string idplanilla) => await _repoGT.GetMarcasDescansos(idnumero,fecha,idplanilla);

    }
}
