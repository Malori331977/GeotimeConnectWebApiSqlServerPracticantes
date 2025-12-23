using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models.ErpClases
{
    public class cConceptoErp
    {
        public string CONCEPTO { get; set; }
        public string ALIAS { get; set; }
        public string TIPO_CONCEPTO { get; set; }
        public string DESCRIPCION { get; set; }
        public string? UNIDADES { get; set; }
        public string SALARIAL { get; set; }
        public string FIJO { get; set; }
        public string LIQUIDABLE { get; set; }
        public string EXCLUYENTE { get; set; }
        public string? VARIABLE_DECISION { get; set; }
        public string CANT_EDITABLE { get; set; }
        public string MONTO_EDITABLE { get; set; }
        public string USA_CANTIDAD { get; set; }
        public string USA_MONTO { get; set; }
        public string PERIODICIDAD { get; set; }
        public short DIAS_PERIODO { get; set; }
        public short MAX_NIVEL { get; set; }
        public string CARGA_POR_DLL { get; set; }
        public string CALCULO_POR_DLL { get; set; }
        public decimal FACTOR_REDONDEO { get; set; }
        public string FORMULA_DEFINIDA { get; set; }
        public string IMPRIMIR_COMP_PAGO { get; set; }
        public string IMPRIMIR_ACUMULADO { get; set; }
        public string IMPRIMIR_COND_PAGO { get; set; }
        public string ELIMINAR_CONC_CERO { get; set; }
        public string SDI_CALC_ACUMULADO { get; set; }
        public string SDI_CALC_FORMULA { get; set; }
        public string DISTRIBUYE_CENTROS { get; set; }
        public string? PROVEEDOR { get; set; }
        public string CARGA_HORAS { get; set; }
        public string INCLUIR_DOC_RH { get; set; }
        public string? NOTAS { get; set; }
        public string CALCULO_SPD { get; set; }
        public string? CODIGO_ANEXO { get; set; }
        public DateTime? PERIODO_INICIAL_EXCLUYENTE { get; set; }
        public DateTime? PERIODO_FINAL_EXCLUYENTE { get; set; }
        public string OMITIR_MONT_CANT_CERO { get; set; }
        public string? FUNCION_INTERFAZ { get; set; }
        public string? CONTABILIZACION { get; set; }
        public string? NIT { get; set; }
        public short NoteExistsFlag { get; set; }
        public DateTime RecordDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string? CLIENTE { get; set; }
        public string? TIPO_HORA_EXTRA { get; set; }
    }
}
