using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PeriodoVigenteUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PeriodoVigenteUsuarioController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }
                
		[HttpGet("{idusuario}/{fechaPeriodo}")]
		public async Task<IEnumerable<cPh_Periodos>> Get(int idusuario,string fechaPeriodo) => await _repoGT.GetPeriodoVigenteUsuario(idusuario, fechaPeriodo);


	}
}
