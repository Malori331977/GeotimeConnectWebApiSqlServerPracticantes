using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaIncidenciaEstadoEnvioController : Controller
    {
        private readonly IMarcasService _repo;
        public MarcaIncidenciaEstadoEnvioController(IMarcasService repo)
        {

            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<cMarcaIncidencia>> Get()
        {
            var marcasIncidencias = await _repo.GetMarcaIncidenciaEstadoEnvio();

            Task task = new Task(async () =>
            {
                var resp = await _repo.PostMarcaIncidenciaEstadoEnvio(marcasIncidencias);
            });
            task.Start();

            return marcasIncidencias;
        }     

        
    }
}
