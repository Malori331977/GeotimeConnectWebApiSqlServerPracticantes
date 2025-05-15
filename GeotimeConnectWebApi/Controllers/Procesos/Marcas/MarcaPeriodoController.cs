using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
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
