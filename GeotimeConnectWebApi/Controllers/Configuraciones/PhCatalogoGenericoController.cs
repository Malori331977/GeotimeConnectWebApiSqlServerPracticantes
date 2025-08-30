using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Configuraciones
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhCatalogoGenericoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhCatalogoGenericoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_CatalogoGenerico>> Get() => await _repoGT.GetPhCatalogoGenerico();

        [HttpGet("{nombre}")]
        public async Task<IEnumerable<cPh_CatalogoGenerico>> Get(string nombre) => await _repoGT.GetPhCatalogoGenerico(nombre);

        [HttpGet("{nombre}/{id}")]
        public async Task<cPh_CatalogoGenerico> Get(string nombre, string id) => await _repoGT.GetPhCatalogoGenerico(nombre, id);

    }
}
