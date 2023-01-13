using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;


namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TurnoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public TurnoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTurno>> Get() => await _repoGT.GetTurno();


        [HttpGet("{idturno}")]
        public async Task<cTurno> Get(int idturno) => await _repoGT.GetTurno(idturno);

        
    }
}
