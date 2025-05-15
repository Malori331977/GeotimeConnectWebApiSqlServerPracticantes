using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Periodos
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
