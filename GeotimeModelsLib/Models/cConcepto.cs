using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{
    public class cConcepto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
		public string Concepto { get; set; }
		public string Descripcion { get; set; }
		public int tipo_j { get; set; }
		public int tipo_h { get; set; }
		public int columnar { get; set; }
		public string? nominaeq { get; set; }
		public int factor { get; set; }
		public int tolerancia { get; set; }
		public char ordinario { get; set; }
		public char autorizado { get; set; }
		public char? transferir { get; set; }
		public char adicional { get; set; }
		public char? tipo_ext_alm { get; set; }
		public char? muestra_resumen { get; set; }

		public IEnumerable<cMarcaDistribucion>? cMarcaDistribucion { get; set; }
        public IEnumerable<cMarcaResumen>? cMarcaResumen { get; set; }
        public IEnumerable<cMarcaTiempoAdicional>? cMarcaTiempoAdicionals { get; set; }


    }
}
