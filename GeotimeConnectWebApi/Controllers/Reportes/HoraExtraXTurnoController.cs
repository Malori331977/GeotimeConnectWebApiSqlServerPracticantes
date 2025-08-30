using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Reportes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HoraExtraXTurnoController : Controller
    {
        private readonly IReportesServices _repoGT;
        public HoraExtraXTurnoController(IReportesServices repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cVHoraExtraXTurno>> Get() => await _repoGT.GetHoraExtraXTurno();

        [HttpGet("{idPeriodo}/{idturno}")]
        public async Task<IEnumerable<cVHoraExtraXTurno>> Get(string idPeriodo, int idturno) => await _repoGT.GetHoraExtraXTurno(idPeriodo, idturno);

    }
}
