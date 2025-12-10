using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Reportes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HoraLaboradaEmpleadoController : Controller
    {
        private readonly IReportesServices _repoGT;
        public HoraLaboradaEmpleadoController(IReportesServices repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cVHoraLaboradaEmpleado>> Get() => await _repoGT.GetHoraLaboradaEmpleado();

        [HttpGet("{idPeriodo}/{idplanilla}/{iddepartamento}")]
        public async Task<IEnumerable<cVHoraLaboradaEmpleado>> Get(string idPeriodo, string idplanilla, string iddepartamento) => await _repoGT.GetHoraLaboradaEmpleado(idPeriodo, idplanilla, iddepartamento);
    }
}
