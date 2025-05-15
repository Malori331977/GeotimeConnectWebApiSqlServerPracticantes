using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TipoPlanillaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public TipoPlanillaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTipo_Planilla>> Get() => await _repoGT.GetTipo_Planilla();      

    }
}
