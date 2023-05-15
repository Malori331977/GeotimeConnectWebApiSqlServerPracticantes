using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
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
