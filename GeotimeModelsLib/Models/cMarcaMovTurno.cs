namespace com.gsitcr.geotime.Models
{
    public class cMarcaMovTurno
    {
        public long idregistro { get; set; }
        public string idplanilla { get; set; }
        public string idnumero { get; set; }
        public DateTime fecha { get; set; }
        public string hora { get; set; }
        public int turno { get; set; }
        public char estado { get; set; }
        public string? usuario { get; set; }
        public DateTime? fecha_reg { get; set; }
        public int? linea { get; set; }
        public string? hentra2 { get; set; }
        public bool estado_envio { get; set; }

        public cEmpleado? cEmpleado { get; set; }
        public cPh_Planilla? cPh_Planilla { get; set; }
        public cTurno? cTurno { get; set; }

    }
}
