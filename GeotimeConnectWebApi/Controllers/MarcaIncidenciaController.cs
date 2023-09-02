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
    public class MarcaIncidenciaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaIncidenciaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{id}")]
        public async Task<cMarcaIncidencia> Get(long id) => await _repoGT.GetMarcaIncidencia(id);

        [HttpGet("{idnumero}/{fecha}/{idplanilla}")]
        public async Task<IEnumerable<cMarcaIncidencia>> Get(string idnumero, string fecha, string idplanilla) => await _repoGT.GetMarcaIncidencia(idnumero,fecha,idplanilla);

    }
}
