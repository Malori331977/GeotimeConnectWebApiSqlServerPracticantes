using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PeriodoVigenteEmpleadoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PeriodoVigenteEmpleadoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }
                
		[HttpGet("{idnumero}/{fechaPeriodo}")]
		public async Task<cPh_Periodos> Get(string idnumero,string fechaPeriodo) => await _repoGT.GetPeriodoVigenteEmpleado(idnumero, fechaPeriodo);


	}
}
