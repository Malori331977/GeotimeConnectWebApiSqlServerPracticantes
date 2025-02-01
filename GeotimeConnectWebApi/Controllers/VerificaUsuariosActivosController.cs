using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class VerificaUsuariosActivosController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public VerificaUsuariosActivosController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<int> Get() => await _repoGT.VerificaUsuariosActivosPMW();

        
    }
}
