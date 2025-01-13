using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhPlanillaByUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhPlanillaByUsuarioController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }    

        [HttpGet("{idUsuario}")]
        public async Task<List<cPh_Planilla>> Get(string idUsuario) => await _repoGT.GetPhPlanillaByUsuario(idUsuario);

       

    }
}
