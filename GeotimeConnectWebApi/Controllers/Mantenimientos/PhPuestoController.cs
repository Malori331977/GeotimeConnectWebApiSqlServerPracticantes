using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhPuestoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhPuestoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Puesto>> Get() => await _repoGT.GetPhPuesto();      

        [HttpGet("{id}")]
        public async Task<cPh_Puesto> Get(string id) => await _repoGT.GetPhPuesto(id);

       

    }
}
