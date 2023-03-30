using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace GeoTimeConnectWebApi.Models
{
    public class cEmpleado
    {
        public String? IdNumero { get; set; }
        public string? IdPlanilla { get; set; }
        public String? Nombre { get; set; }
        public String? Tarjeta { get; set; }
        public String? Identificacion { get; set; }
        public String? IdDepartamento { get; set; }
        public char? Estado { get; set; }
        public String? IdCCosto { get; set; }
        public DateTime? Fecha_Ingreso { get; set; }
        public String? Email { get; set; }
        public int? IdGrupo { get; set; }
        public int? IdHorario { get; set; }
        public int? IdAgrupamiento { get; set; }
        public string? Tipo_Marca { get; set; }
        public string? global_clave { get; set; }


    }
}
