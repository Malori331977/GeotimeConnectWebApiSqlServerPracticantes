using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Utils;
using System.Text.Json;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

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
