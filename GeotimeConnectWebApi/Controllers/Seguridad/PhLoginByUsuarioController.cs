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

namespace GeoTimeConnectWebApi.Controllers.Seguridad
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
        public async Task<cPh_Login> Get(string id) => await _repoGT.GetPhLoginByUsuario(id);

    }
}
