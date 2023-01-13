using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DepartamentoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public DepartamentoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cDepartamento>> Get() => await _repoGT.GetDepartamento();


        [HttpGet("{iddepart}")]
        public async Task<cDepartamento> Get(string iddepart) => await _repoGT.GetDepartamento(iddepart);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cDepartamento> departamentos)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Departamento(departamentos);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
