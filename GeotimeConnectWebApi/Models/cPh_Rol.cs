﻿namespace GeoTimeConnectWebApi.Models
{
    public class cPh_Rol
    {
        public int IDROL { get; set; }
        public string DESCRIPCION { get; set; }

        public List<cTurno>? Turno { get; set; }
    }
}
