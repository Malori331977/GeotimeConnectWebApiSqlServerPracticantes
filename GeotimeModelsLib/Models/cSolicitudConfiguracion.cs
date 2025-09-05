using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cSolicitudConfiguracion
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public bool FechaInicio { get; set; }
        public bool FechaFin { get; set; }
        public bool HoraInicio { get; set; }
        public bool HoraFin { get; set; }
        public bool CantidadHoras { get; set; }
        public bool CantidadNumerico { get; set; }
        public bool CentroCosto { get; set; }
        public bool MultipleCentroCosto { get; set; }
        public bool FiltrarPuesto { get; set; }
        public string? PuestosHabilitados { get; set; }
        public int? Concepto { get; set; }
        public string Destino { get; set; }
        public bool MostrarCantidadHoras { get; set; }
        public bool MostrarCantidadNum { get; set; }
        public bool CalcularHoras { get; set; }
        public bool CalcularHorasNum { get; set; }
        public bool CalcularDias { get; set; }
        public int MaxCantidadDiasPasados { get; set; }
        public int MaxCantidadDiasFuturos { get; set; }
        public IEnumerable<cTipoSolicitud>? cTipoSolicitud { get; set; }
    }
}
