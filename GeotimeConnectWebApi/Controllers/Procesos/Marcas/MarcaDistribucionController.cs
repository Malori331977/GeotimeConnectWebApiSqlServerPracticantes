using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaDistribucionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaDistribucionController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{idPlanilla}/{fechaInicio}/{fechaFin}/{idnumero}/{hora}")]
        public async Task<IEnumerable<cMarcaDistribucion>> Get(string idPlanilla, string fechaInicio, string fechaFin, string idnumero, string hora) => await _repoGT.GetMarcaDistribucion(idPlanilla, fechaInicio, fechaFin, idnumero,hora);

       


    }
}
