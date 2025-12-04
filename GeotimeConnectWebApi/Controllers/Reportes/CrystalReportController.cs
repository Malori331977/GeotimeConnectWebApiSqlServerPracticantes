using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Reportes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CrystalReportController : Controller
    {
        private readonly ICrystalReportService _repo;
        public CrystalReportController(ICrystalReportService repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cFiltroReporte filtro)
        {
            EventResponse respuesta = await _repo.GeneratePdfReport(filtro);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
