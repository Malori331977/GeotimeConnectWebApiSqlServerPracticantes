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
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class PhCompaniaUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhCompaniaUsuarioController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet("{idnumero}")]
        public async Task<IEnumerable<cPh_CompaniaUsuario>> Get(string idnumero) => await _repoGT.GetPhCompaniaUsuario(idnumero);

    }
}
