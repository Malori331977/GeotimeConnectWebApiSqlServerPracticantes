using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaPeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaPeriodoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{idsGrupos}/{idPlanilla}/{fechaInicio}/{fechaFin}/{idnumero}")]
        public async Task<IEnumerable<cMarcaPeriodo>> Get(string idsGrupos, string idPlanilla, string fechaInicio, string fechaFin, string idnumero) => await _repoGT.GetMarcasPeriodo(idsGrupos, idPlanilla, fechaInicio, fechaFin, idnumero);


    }
}
