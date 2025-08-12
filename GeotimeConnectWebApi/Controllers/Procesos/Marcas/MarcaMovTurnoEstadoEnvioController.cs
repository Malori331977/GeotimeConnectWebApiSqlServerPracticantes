using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaMovTurnoEstadoEnvioController : Controller
    {
        private readonly IMarcasService _repo;
        public MarcaMovTurnoEstadoEnvioController(IMarcasService repo)
        {

            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaMovTurno>> Get()
        {
            var marcasMovTurno = await _repo.GetMarcaMovTurnoEstadoEnvio();

            Task task = new Task(async () =>
            {
                var resp = await _repo.PostMarcaMovTurnoEstadoEnvio(marcasMovTurno);
            });
            task.Start();

            return marcasMovTurno;
        }     

        
    }
}
