using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.AccionesPersonal
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccionPersonalPorEstadoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalPorEstadoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{estado}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, char estado ) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, estado);


        [HttpGet("{idplanilla}/{usuario}/{estado}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, string usuario, char estado) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, usuario, estado);

        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}/{estado}/{idincidencia}/{idgrupo}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, string fechainicio, string fechafin, char estado, int idincidencia, string idgrupo) => await _repoGT.GetAccionPersonalPorEstado(idplanilla, fechainicio, fechafin, estado, idincidencia, idgrupo);

    }
}
