using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaResumenXPeriodoController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public MarcaResumenXPeriodoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idperiodo}")]
        public async Task<IEnumerable<cMarcaResumen>> Get(string idperiodo) => await _repoGT.GetMarcasResumenXPeriodo(idperiodo);

	}
}
