using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Utils;
using System.Text.Json;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Configuraciones
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhRolSistemaDetController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhRolSistemaDetController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_RolSistemaDet>> Get() => await _repoGT.GetPhRolSistemaDet();      

        [HttpGet("{id}")]
        public async Task<IEnumerable<cPh_RolSistemaDet>> Get(string id) => await _repoGT.GetPhRolSistemaDet(id);

        

    }
}
