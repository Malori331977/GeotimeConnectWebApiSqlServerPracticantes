using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaResumenController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public MarcaResumenController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaResumen>> Get(string idplanilla, string idperiodo) => await _repoGT.GetMarcasResumen(idplanilla, idperiodo);

    }
}
