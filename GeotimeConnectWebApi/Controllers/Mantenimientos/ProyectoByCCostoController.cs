using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProyectoByCCostoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public ProyectoByCCostoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idccosto}")]
        public async Task<IEnumerable<cPh_Proyecto>> Get(string idccosto) => await _repoGT.GetProyectoByCCosto(idccosto);


        
    }
}
