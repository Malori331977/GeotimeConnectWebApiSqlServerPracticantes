using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
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
