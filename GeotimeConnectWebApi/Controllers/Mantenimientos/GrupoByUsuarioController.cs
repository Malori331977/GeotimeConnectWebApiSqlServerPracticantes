using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GrupoByUsuarioController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public GrupoByUsuarioController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }    

        [HttpGet("{idUsuario}")]
        public async Task<List<cPh_Grupo>> Get(string idUsuario) => await _repoGT.GetPhGrupoByUsuario(idUsuario);

       

    }
}
