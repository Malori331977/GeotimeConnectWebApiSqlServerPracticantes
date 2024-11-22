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
    public class PhLoginByUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhLoginByUsuarioController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }     

        [HttpGet("{id}")]
        public async Task<cPh_Login> Get(int id) => await _repoGT.GetPhLoginByUsuario(id);

    }
}
