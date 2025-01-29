namespace GeoTimeConnectWebApi.Models
{
    public class cPh_RolSistemaDet
    {
        public string ROLSISTEMAID { get; set; }
        public string MENUSISTEMAID { get; set; }
        public string OPCIONSISTEMAID { get; set; }
        public bool HABILITADO { get; set; }
        public bool AGREGA { get; set; }
        public bool MODIFICA { get; set; }
        public bool ELIMINA { get; set; }
        public bool CONSULTA { get; set; }

        public cPh_RolSistema? cPh_RolSistema { get; set; }
    }
}
