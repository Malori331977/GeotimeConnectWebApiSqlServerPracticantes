using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
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
