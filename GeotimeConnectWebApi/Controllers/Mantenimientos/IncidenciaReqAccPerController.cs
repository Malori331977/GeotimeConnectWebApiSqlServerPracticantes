using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IncidenciaReqAccPerController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public IncidenciaReqAccPerController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cIncidencia>> Get() => await _repoGT.GetIncidenciaReqAccPer();

    }
}
