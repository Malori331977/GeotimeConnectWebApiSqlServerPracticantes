using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Periodos
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
