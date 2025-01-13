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
    public class MarcaResumenATransferirController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public MarcaResumenATransferirController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{idperiodo}")]
        public async Task<IEnumerable<cMarcaResumen>> Get(string idplanilla, string idperiodo) => await _repoGT.GetMarcasResumenATransferir(idplanilla,idperiodo);

	}
}
