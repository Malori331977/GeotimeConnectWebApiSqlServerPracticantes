using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.AccionesPersonal
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccionPersonalPorPeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalPorPeriodoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idnumero}/{fecha}/{idplanilla}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idnumero, string fecha, string idplanilla) => await _repoGT.GetAccionPersonalPorPeriodo(idnumero, fecha, idplanilla);

    }
}
