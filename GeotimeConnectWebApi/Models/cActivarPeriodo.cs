using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeotimeConnectWebApi.Models
{
    public class cActivarPeriodo
    {
        public string IdPlanilla { get; set; }
        public int Grupo { get; set; }
        public string Periodo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int Usuario { get; set; }

    }
}
