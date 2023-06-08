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
    public class PeriodoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PeriodoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Periodos>> Get() => await _repoGT.GetPeriodo();      

        [HttpGet("{idgrupo}")]
        public async Task<cPh_Periodos> Get(string idperiodo) => await _repoGT.GetPeriodo(idperiodo);

		[HttpGet("{fecha}/{vigente}")]
		public async Task<IEnumerable<cPh_Periodos>> Get(string fecha, string vigente) => await _repoGT.GetPeriodo(fecha, vigente);


	}
}
