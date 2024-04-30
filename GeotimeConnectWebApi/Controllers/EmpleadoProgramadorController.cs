using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using System.Text.Json;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmpleadoProgramadorController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public EmpleadoProgramadorController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idPlanilla}/{grupos}")]
        public async Task<IEnumerable<cEmpleado>> Get(string idPlanilla, string grupos) => await _repoGT.GetEmpleadoProgramador(idPlanilla, grupos);

    }
}
