using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Configuraciones
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhSistemaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhSistemaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<cPh_Sistema> Get() => await _repoGT.GetPhSistema();      

       

    }
}
