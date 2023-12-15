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
        public DateTime? Fecha_Salida { get; set; }
        public string? global_code { get; set; }
        public DateTime? fecha_act_code { get; set; }
        public byte[]? foto { get; set; }
        public char? exporta { get; set; }
        public string? ubicacion { get; set; }
        public string? rubro1 { get; set; }
        public string? rubro2 { get; set; }
        public string? rubro3 { get; set; }
        public string? rubro4 { get; set; }
        public string? rubro5 { get; set; }
        public string? rubro6 { get; set; }
        public string? rubro7 { get; set; }
        public string? rubro8 { get; set; }
        public string? rubro9 { get; set; }
        public string? rubro10 { get; set; }
        public string? rubro11 { get; set; }
        public string? rubro12 { get; set; }
        public string? rubro13 { get; set; }
        public string? rubro14 { get; set; }
        public string? rubro15 { get; set; }
        public string? rubro16 { get; set; }
        public string? rubro17 { get; set; }
        public string? rubro18 { get; set; }
        public string? rubro19 { get; set; }
        public string? rubro20 { get; set; }
        public string? rubro21 { get; set; }
        public string? rubro22 { get; set; }
        public string? rubro23 { get; set; }
        public string? rubro24 { get; set; }
        public string? rubro25 { get; set; }
        public DateTime? inicio_rol { get; set; }
        public string? web_pass { get; set; }
        public int? id_transfo_conc { get; set; }
        public string? widioma { get; set; }
        public string? def_cc { get; set; }
        public string? def_py { get; set; }
        public string? def_fase { get; set; }

        public cDepartamento? Departamento { get; set; }
        public cCentroCosto? CentroCosto { get; set; }
        public cPh_Planilla? Ph_Planilla { get; set; }


    }
}
