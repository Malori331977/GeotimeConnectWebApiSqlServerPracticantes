using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Reportes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaMovTurnoBitacoraController : Controller
    {
        private readonly IReportesServices _repoGT;
        public MarcaMovTurnoBitacoraController(IReportesServices repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{fechaInicio}/{fechaFinal}")]
        public async Task<IEnumerable<cMarcaMovTurnoBitacora>> Get(string fechaInicio, string fechaFinal) => await _repoGT.GetMarcaMovTurnoBitacora(fechaInicio, fechaFinal);
    }
}
