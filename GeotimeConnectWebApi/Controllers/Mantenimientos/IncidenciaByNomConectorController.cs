using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IncidenciaByNomConectorController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public IncidenciaByNomConectorController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

       
        [HttpGet("{nom_conector}")]
        public async Task<cIncidencia> Get(string nom_conector) => await _repoGT.GetIncidenciaByNomConector(nom_conector);

        
    }
}
