﻿namespace GeoTimeConnectWebApi.Models
{
	public class cPh_Periodos
	{
		public string idperiodo { get; set; }
		public char tipo_planilla { get; set; }
		public DateTime inicio { get; set; }
		public DateTime fin { get; set; }
		public char? estado { get; set; }
		public DateTime? inicio_proy { get; set; }
		public DateTime? fin_proy { get; set; }
		public string? periodo_proy { get; set; }
        public int? dia_inicio { get; set; }
    }
}
