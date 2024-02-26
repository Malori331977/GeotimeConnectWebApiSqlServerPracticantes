
using GeoTimeConnectWebApi.Models.Utils;

namespace GeoTimeServiceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://geotime.ddns.net/", ConfigurationName="GeoTimeServiceReference.Service1Soap")]
    public interface ServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/init_periodo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.init_periodoResponse> init_periodoAsync(GeoTimeServiceReference.init_periodoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/calculo_periodo_empleado", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.calculo_periodo_empleadoResponse> calculo_periodo_empleadoAsync(GeoTimeServiceReference.calculo_periodo_empleadoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/calculo_periodo_planilla", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.calculo_periodo_planillaResponse> calculo_periodo_planillaAsync(GeoTimeServiceReference.calculo_periodo_planillaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/ncalculo_periodo_planilla", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.ncalculo_periodo_planillaResponse> ncalculo_periodo_planillaAsync(GeoTimeServiceReference.ncalculo_periodo_planillaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/sincronizo_erp", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.sincronizo_erpResponse> sincronizo_erpAsync(GeoTimeServiceReference.sincronizo_erpRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/sincronizo_rerp", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.sincronizo_rerpResponse> sincronizo_rerpAsync(GeoTimeServiceReference.sincronizo_rerpRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/sincronizo_acciones", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.sincronizo_accionesResponse> sincronizo_accionesAsync(GeoTimeServiceReference.sincronizo_accionesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/obtengo_rdat", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_rdatResponse> obtengo_rdatAsync(GeoTimeServiceReference.obtengo_rdatRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/inporto_acciones", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.inporto_accionesResponse> inporto_accionesAsync(GeoTimeServiceReference.inporto_accionesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/importo_racciones", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.importo_raccionesResponse> importo_raccionesAsync(GeoTimeServiceReference.importo_raccionesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/exporto_conceptos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.exporto_conceptosResponse> exporto_conceptosAsync(GeoTimeServiceReference.exporto_conceptosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/obtengo_conceptos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_conceptosResponse> obtengo_conceptosAsync(GeoTimeServiceReference.obtengo_conceptosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/obtengo_rconceptos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_rconceptosResponse> obtengo_rconceptosAsync(GeoTimeServiceReference.obtengo_rconceptosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/obtengo_tipoaccion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_tipoaccionResponse> obtengo_tipoaccionAsync(GeoTimeServiceReference.obtengo_tipoaccionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/obtengo_rtipoaccion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_rtipoaccionResponse> obtengo_rtipoaccionAsync(GeoTimeServiceReference.obtengo_rtipoaccionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/evaluo_formula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.evaluo_formulaResponse> evaluo_formulaAsync(GeoTimeServiceReference.evaluo_formulaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/proceso_empleado", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.proceso_empleadoResponse> proceso_empleadoAsync(GeoTimeServiceReference.proceso_empleadoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/evaluo_movile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.evaluo_movileResponse> evaluo_movileAsync(GeoTimeServiceReference.evaluo_movileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/sinc_movile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.sinc_movileResponse> sinc_movileAsync(GeoTimeServiceReference.sinc_movileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/up_movile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.up_movileResponse> up_movileAsync(GeoTimeServiceReference.up_movileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/verif_face", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.verif_faceResponse> verif_faceAsync(GeoTimeServiceReference.verif_faceRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/actualizo_compania", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.actualizo_companiaResponse> actualizo_companiaAsync(GeoTimeServiceReference.actualizo_companiaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/test_rdat", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.test_rdatResponse> test_rdatAsync(GeoTimeServiceReference.test_rdatRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/auto_sync_erp", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.auto_sync_erpResponse> auto_sync_erpAsync(GeoTimeServiceReference.auto_sync_erpRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/auto_calc_comp", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.auto_calc_compResponse> auto_calc_compAsync(GeoTimeServiceReference.auto_calc_compRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/auto_sup", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.auto_supResponse> auto_supAsync(GeoTimeServiceReference.auto_supRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/auto_emp", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.auto_empResponse> auto_empAsync(GeoTimeServiceReference.auto_empRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/proceso_archivo_compania", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.proceso_archivo_companiaResponse> proceso_archivo_companiaAsync(GeoTimeServiceReference.proceso_archivo_companiaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/UploadFile_1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.UploadFile_1Response> UploadFile_1Async(GeoTimeServiceReference.UploadFile_1Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/UploadDataProy", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.UploadDataProyResponse> UploadDataProyAsync(GeoTimeServiceReference.UploadDataProyRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/UploadFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.UploadFileResponse> UploadFileAsync(GeoTimeServiceReference.UploadFileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/compania_auto", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.compania_autoResponse> compania_autoAsync(GeoTimeServiceReference.compania_autoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/actualizo_clave", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.actualizo_claveResponse> actualizo_claveAsync(GeoTimeServiceReference.actualizo_claveRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/test_mail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.test_mailResponse> test_mailAsync(GeoTimeServiceReference.test_mailRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/auto_exp_acc", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.auto_exp_accResponse> auto_exp_accAsync(GeoTimeServiceReference.auto_exp_accRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/auto_sync_acc", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.auto_sync_accResponse> auto_sync_accAsync(GeoTimeServiceReference.auto_sync_accRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/prueba_hcm", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.prueba_hcmResponse> prueba_hcmAsync(GeoTimeServiceReference.prueba_hcmRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://geotime.ddns.net/UploadClock", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<GeoTimeServiceReference.UploadClockResponse> UploadClockAsync(GeoTimeServiceReference.UploadClockRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="init_periodo", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class init_periodoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string periodo;
        
        public init_periodoRequest()
        {
        }
        
        public init_periodoRequest(string comp, string plan, string periodo)
        {
            this.comp = comp;
            this.plan = plan;
            this.periodo = periodo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="init_periodoResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class init_periodoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string init_periodoResult;
        
        public init_periodoResponse()
        {
        }
        
        public init_periodoResponse(string init_periodoResult)
        {
            this.init_periodoResult = init_periodoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculo_periodo_empleado", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class calculo_periodo_empleadoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string periodo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=5)]
        public string empleado;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=6)]
        public int sesion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=7)]
        public string idpais;
        
        public calculo_periodo_empleadoRequest()
        {
        }
        
        public calculo_periodo_empleadoRequest(string comp, string plan, string periodo, string inicio, string fin, string empleado, int sesion, string idpais)
        {
            this.comp = comp;
            this.plan = plan;
            this.periodo = periodo;
            this.inicio = inicio;
            this.fin = fin;
            this.empleado = empleado;
            this.sesion = sesion;
            this.idpais = idpais;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculo_periodo_empleadoResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class calculo_periodo_empleadoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string calculo_periodo_empleadoResult;
        
        public calculo_periodo_empleadoResponse()
        {
        }
        
        public calculo_periodo_empleadoResponse(string calculo_periodo_empleadoResult)
        {
            this.calculo_periodo_empleadoResult = calculo_periodo_empleadoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculo_periodo_planilla", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class calculo_periodo_planillaRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string periodo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=5)]
        public string grupo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=6)]
        public int sesion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=7)]
        public string idpais;
        
        public calculo_periodo_planillaRequest()
        {
        }
        
        public calculo_periodo_planillaRequest(string comp, string plan, string periodo, string inicio, string fin, string grupo, int sesion, string idpais)
        {
            this.comp = comp;
            this.plan = plan;
            this.periodo = periodo;
            this.inicio = inicio;
            this.fin = fin;
            this.grupo = grupo;
            this.sesion = sesion;
            this.idpais = idpais;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculo_periodo_planillaResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class calculo_periodo_planillaResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string calculo_periodo_planillaResult;
        
        public calculo_periodo_planillaResponse()
        {
        }
        
        public calculo_periodo_planillaResponse(string calculo_periodo_planillaResult)
        {
            this.calculo_periodo_planillaResult = calculo_periodo_planillaResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ncalculo_periodo_planilla", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class ncalculo_periodo_planillaRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string periodo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=5)]
        public string grupo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=6)]
        public int sesion;
        
        public ncalculo_periodo_planillaRequest()
        {
        }
        
        public ncalculo_periodo_planillaRequest(string comp, string plan, string periodo, string inicio, string fin, string grupo, int sesion)
        {
            this.comp = comp;
            this.plan = plan;
            this.periodo = periodo;
            this.inicio = inicio;
            this.fin = fin;
            this.grupo = grupo;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ncalculo_periodo_planillaResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class ncalculo_periodo_planillaResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string ncalculo_periodo_planillaResult;
        
        public ncalculo_periodo_planillaResponse()
        {
        }
        
        public ncalculo_periodo_planillaResponse(string ncalculo_periodo_planillaResult)
        {
            this.ncalculo_periodo_planillaResult = ncalculo_periodo_planillaResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sincronizo_erp", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sincronizo_erpRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        public sincronizo_erpRequest()
        {
        }
        
        public sincronizo_erpRequest(string comp, string plan)
        {
            this.comp = comp;
            this.plan = plan;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sincronizo_erpResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sincronizo_erpResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string sincronizo_erpResult;
        
        public sincronizo_erpResponse()
        {
        }
        
        public sincronizo_erpResponse(string sincronizo_erpResult)
        {
            this.sincronizo_erpResult = sincronizo_erpResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sincronizo_rerp", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sincronizo_rerpRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        public sincronizo_rerpRequest()
        {
        }
        
        public sincronizo_rerpRequest(string comp, string plan)
        {
            this.comp = comp;
            this.plan = plan;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sincronizo_rerpResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sincronizo_rerpResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string sincronizo_rerpResult;
        
        public sincronizo_rerpResponse()
        {
        }
        
        public sincronizo_rerpResponse(string sincronizo_rerpResult)
        {
            this.sincronizo_rerpResult = sincronizo_rerpResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sincronizo_acciones", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sincronizo_accionesRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string sesion;
        
        public sincronizo_accionesRequest()
        {
        }
        
        public sincronizo_accionesRequest(string comp, string plan, string inicio, string fin, string sesion)
        {
            this.comp = comp;
            this.plan = plan;
            this.inicio = inicio;
            this.fin = fin;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sincronizo_accionesResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sincronizo_accionesResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string sincronizo_accionesResult;
        
        public sincronizo_accionesResponse()
        {
        }
        
        public sincronizo_accionesResponse(string sincronizo_accionesResult)
        {
            this.sincronizo_accionesResult = sincronizo_accionesResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_rdat", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_rdatRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string planilla;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string sesion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public int funcion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=5)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=6)]
        public string aux;
        
        public obtengo_rdatRequest()
        {
        }
        
        public obtengo_rdatRequest(string comp, string planilla, string sesion, int funcion, string inicio, string fin, string aux)
        {
            this.comp = comp;
            this.planilla = planilla;
            this.sesion = sesion;
            this.funcion = funcion;
            this.inicio = inicio;
            this.fin = fin;
            this.aux = aux;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_rdatResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_rdatResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string obtengo_rdatResult;
        
        public obtengo_rdatResponse()
        {
        }
        
        public obtengo_rdatResponse(string obtengo_rdatResult)
        {
            this.obtengo_rdatResult = obtengo_rdatResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="inporto_acciones", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class inporto_accionesRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string sesion;
        
        public inporto_accionesRequest()
        {
        }
        
        public inporto_accionesRequest(string comp, string plan, string inicio, string fin, string sesion)
        {
            this.comp = comp;
            this.plan = plan;
            this.inicio = inicio;
            this.fin = fin;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="inporto_accionesResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class inporto_accionesResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string inporto_accionesResult;
        
        public inporto_accionesResponse()
        {
        }
        
        public inporto_accionesResponse(string inporto_accionesResult)
        {
            this.inporto_accionesResult = inporto_accionesResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="importo_racciones", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class importo_raccionesRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string inicio;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string fin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string sesion;
        
        public importo_raccionesRequest()
        {
        }
        
        public importo_raccionesRequest(string comp, string plan, string inicio, string fin, string sesion)
        {
            this.comp = comp;
            this.plan = plan;
            this.inicio = inicio;
            this.fin = fin;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="importo_raccionesResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class importo_raccionesResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string importo_raccionesResult;
        
        public importo_raccionesResponse()
        {
        }
        
        public importo_raccionesResponse(string importo_raccionesResult)
        {
            this.importo_raccionesResult = importo_raccionesResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="exporto_conceptos", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class exporto_conceptosRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string plan;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string periodo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=3)]
        public string hora_labora;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=4)]
        public string sesion;
        
        public exporto_conceptosRequest()
        {
        }
        
        public exporto_conceptosRequest(string comp, string plan, string periodo, string hora_labora, string sesion)
        {
            this.comp = comp;
            this.plan = plan;
            this.periodo = periodo;
            this.hora_labora = hora_labora;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="exporto_conceptosResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class exporto_conceptosResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string exporto_conceptosResult;
        
        public exporto_conceptosResponse()
        {
        }
        
        public exporto_conceptosResponse(string exporto_conceptosResult)
        {
            this.exporto_conceptosResult = exporto_conceptosResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_conceptos", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_conceptosRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string sesion;
        
        public obtengo_conceptosRequest()
        {
        }
        
        public obtengo_conceptosRequest(string comp, string sesion)
        {
            this.comp = comp;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_conceptosResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_conceptosResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string obtengo_conceptosResult;
        
        public obtengo_conceptosResponse()
        {
        }
        
        public obtengo_conceptosResponse(string obtengo_conceptosResult)
        {
            this.obtengo_conceptosResult = obtengo_conceptosResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_rconceptos", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_rconceptosRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string sesion;
        
        public obtengo_rconceptosRequest()
        {
        }
        
        public obtengo_rconceptosRequest(string comp, string sesion)
        {
            this.comp = comp;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_rconceptosResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_rconceptosResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string obtengo_rconceptosResult;
        
        public obtengo_rconceptosResponse()
        {
        }
        
        public obtengo_rconceptosResponse(string obtengo_rconceptosResult)
        {
            this.obtengo_rconceptosResult = obtengo_rconceptosResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_tipoaccion", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_tipoaccionRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string sesion;
        
        public obtengo_tipoaccionRequest()
        {
        }
        
        public obtengo_tipoaccionRequest(string comp, string sesion)
        {
            this.comp = comp;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_tipoaccionResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_tipoaccionResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string obtengo_tipoaccionResult;
        
        public obtengo_tipoaccionResponse()
        {
        }
        
        public obtengo_tipoaccionResponse(string obtengo_tipoaccionResult)
        {
            this.obtengo_tipoaccionResult = obtengo_tipoaccionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_rtipoaccion", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_rtipoaccionRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string sesion;
        
        public obtengo_rtipoaccionRequest()
        {
        }
        
        public obtengo_rtipoaccionRequest(string comp, string sesion)
        {
            this.comp = comp;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtengo_rtipoaccionResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class obtengo_rtipoaccionResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string obtengo_rtipoaccionResult;
        
        public obtengo_rtipoaccionResponse()
        {
        }
        
        public obtengo_rtipoaccionResponse(string obtengo_rtipoaccionResult)
        {
            this.obtengo_rtipoaccionResult = obtengo_rtipoaccionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="evaluo_formula", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class evaluo_formulaRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string formula;
        
        public evaluo_formulaRequest()
        {
        }
        
        public evaluo_formulaRequest(string formula)
        {
            this.formula = formula;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="evaluo_formulaResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class evaluo_formulaResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string evaluo_formulaResult;
        
        public evaluo_formulaResponse()
        {
        }
        
        public evaluo_formulaResponse(string evaluo_formulaResult)
        {
            this.evaluo_formulaResult = evaluo_formulaResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="proceso_empleado", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class proceso_empleadoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string sesion;
        
        public proceso_empleadoRequest()
        {
        }
        
        public proceso_empleadoRequest(string sesion)
        {
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="proceso_empleadoResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class proceso_empleadoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string proceso_empleadoResult;
        
        public proceso_empleadoResponse()
        {
        }
        
        public proceso_empleadoResponse(string proceso_empleadoResult)
        {
            this.proceso_empleadoResult = proceso_empleadoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="evaluo_movile", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class evaluo_movileRequest
    {
        
        public evaluo_movileRequest()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="evaluo_movileResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class evaluo_movileResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string evaluo_movileResult;
        
        public evaluo_movileResponse()
        {
        }
        
        public evaluo_movileResponse(string evaluo_movileResult)
        {
            this.evaluo_movileResult = evaluo_movileResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sinc_movile", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sinc_movileRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string disp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public int opc;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string data;
        
        public sinc_movileRequest()
        {
        }
        
        public sinc_movileRequest(string disp, int opc, string data)
        {
            this.disp = disp;
            this.opc = opc;
            this.data = data;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sinc_movileResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class sinc_movileResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string sinc_movileResult;
        
        public sinc_movileResponse()
        {
        }
        
        public sinc_movileResponse(string sinc_movileResult)
        {
            this.sinc_movileResult = sinc_movileResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="up_movile", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class up_movileRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string disp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public int opc;
        
        public up_movileRequest()
        {
        }
        
        public up_movileRequest(string disp, int opc)
        {
            this.disp = disp;
            this.opc = opc;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="up_movileResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class up_movileResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string up_movileResult;
        
        public up_movileResponse()
        {
        }
        
        public up_movileResponse(string up_movileResult)
        {
            this.up_movileResult = up_movileResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="verif_face", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class verif_faceRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string idcomp;
        
        public verif_faceRequest()
        {
        }
        
        public verif_faceRequest(string idcomp)
        {
            this.idcomp = idcomp;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="verif_faceResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class verif_faceResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string verif_faceResult;
        
        public verif_faceResponse()
        {
        }
        
        public verif_faceResponse(string verif_faceResult)
        {
            this.verif_faceResult = verif_faceResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="actualizo_compania", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class actualizo_companiaRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string comp;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string usuario;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string sesion;
        
        public actualizo_companiaRequest()
        {
        }
        
        public actualizo_companiaRequest(string comp, string usuario, string sesion)
        {
            this.comp = comp;
            this.usuario = usuario;
            this.sesion = sesion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="actualizo_companiaResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class actualizo_companiaResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string actualizo_companiaResult;
        
        public actualizo_companiaResponse()
        {
        }
        
        public actualizo_companiaResponse(string actualizo_companiaResult)
        {
            this.actualizo_companiaResult = actualizo_companiaResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="test_rdat", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class test_rdatRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public int func;
        
        public test_rdatRequest()
        {
        }
        
        public test_rdatRequest(int func)
        {
            this.func = func;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="test_rdatResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class test_rdatResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string test_rdatResult;
        
        public test_rdatResponse()
        {
        }
        
        public test_rdatResponse(string test_rdatResult)
        {
            this.test_rdatResult = test_rdatResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_sync_erp", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_sync_erpRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        public auto_sync_erpRequest()
        {
        }
        
        public auto_sync_erpRequest(string dato)
        {
            this.dato = dato;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_sync_erpResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_sync_erpResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string auto_sync_erpResult;
        
        public auto_sync_erpResponse()
        {
        }
        
        public auto_sync_erpResponse(string auto_sync_erpResult)
        {
            this.auto_sync_erpResult = auto_sync_erpResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_calc_comp", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_calc_compRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string idcomp;
        
        public auto_calc_compRequest()
        {
        }
        
        public auto_calc_compRequest(string dato, string idcomp)
        {
            this.dato = dato;
            this.idcomp = idcomp;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_calc_compResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_calc_compResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string auto_calc_compResult;
        
        public auto_calc_compResponse()
        {
        }
        
        public auto_calc_compResponse(string auto_calc_compResult)
        {
            this.auto_calc_compResult = auto_calc_compResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_sup", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_supRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string idcomp;
        
        public auto_supRequest()
        {
        }
        
        public auto_supRequest(string dato, string idcomp)
        {
            this.dato = dato;
            this.idcomp = idcomp;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_supResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_supResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string auto_supResult;
        
        public auto_supResponse()
        {
        }
        
        public auto_supResponse(string auto_supResult)
        {
            this.auto_supResult = auto_supResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_emp", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_empRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string idcomp;
        
        public auto_empRequest()
        {
        }
        
        public auto_empRequest(string dato, string idcomp)
        {
            this.dato = dato;
            this.idcomp = idcomp;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_empResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_empResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string auto_empResult;
        
        public auto_empResponse()
        {
        }
        
        public auto_empResponse(string auto_empResult)
        {
            this.auto_empResult = auto_empResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="proceso_archivo_compania", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class proceso_archivo_companiaRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string fileName;
        
        public proceso_archivo_companiaRequest()
        {
        }
        
        public proceso_archivo_companiaRequest(string fileName)
        {
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="proceso_archivo_companiaResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class proceso_archivo_companiaResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string proceso_archivo_companiaResult;
        
        public proceso_archivo_companiaResponse()
        {
        }
        
        public proceso_archivo_companiaResponse(string proceso_archivo_companiaResult)
        {
            this.proceso_archivo_companiaResult = proceso_archivo_companiaResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadFile_1", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadFile_1Request
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] archivo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string fileName;
        
        public UploadFile_1Request()
        {
        }
        
        public UploadFile_1Request(byte[] archivo, string fileName)
        {
            this.archivo = archivo;
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadFile_1Response", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadFile_1Response
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string UploadFile_1Result;
        
        public UploadFile_1Response()
        {
        }
        
        public UploadFile_1Response(string UploadFile_1Result)
        {
            this.UploadFile_1Result = UploadFile_1Result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadDataProy", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadDataProyRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] archivo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string fileName;
        
        public UploadDataProyRequest()
        {
        }
        
        public UploadDataProyRequest(byte[] archivo, string fileName)
        {
            this.archivo = archivo;
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadDataProyResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadDataProyResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string UploadDataProyResult;
        
        public UploadDataProyResponse()
        {
        }
        
        public UploadDataProyResponse(string UploadDataProyResult)
        {
            this.UploadDataProyResult = UploadDataProyResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadFile", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadFileRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] archivo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string fileName;
        
        public UploadFileRequest()
        {
        }
        
        public UploadFileRequest(byte[] archivo, string fileName)
        {
            this.archivo = archivo;
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadFileResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadFileResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string UploadFileResult;
        
        public UploadFileResponse()
        {
        }
        
        public UploadFileResponse(string UploadFileResult)
        {
            this.UploadFileResult = UploadFileResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="compania_auto", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class compania_autoRequest
    {
        
        public compania_autoRequest()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="compania_autoResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class compania_autoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string compania_autoResult;
        
        public compania_autoResponse()
        {
        }
        
        public compania_autoResponse(string compania_autoResult)
        {
            this.compania_autoResult = compania_autoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="actualizo_clave", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class actualizo_claveRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string usuario;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string idm;
        
        public actualizo_claveRequest()
        {
        }
        
        public actualizo_claveRequest(string usuario, string idm)
        {
            this.usuario = usuario;
            this.idm = idm;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="actualizo_claveResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class actualizo_claveResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string actualizo_claveResult;
        
        public actualizo_claveResponse()
        {
        }
        
        public actualizo_claveResponse(string actualizo_claveResult)
        {
            this.actualizo_claveResult = actualizo_claveResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="test_mail", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class test_mailRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string compania;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public int sesion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=2)]
        public string destinatarios;
        
        public test_mailRequest()
        {
        }
        
        public test_mailRequest(string compania, int sesion, string destinatarios)
        {
            this.compania = compania;
            this.sesion = sesion;
            this.destinatarios = destinatarios;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="test_mailResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class test_mailResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string test_mailResult;
        
        public test_mailResponse()
        {
        }
        
        public test_mailResponse(string test_mailResult)
        {
            this.test_mailResult = test_mailResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_exp_acc", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_exp_accRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string dias;
        
        public auto_exp_accRequest()
        {
        }
        
        public auto_exp_accRequest(string dato, string dias)
        {
            this.dato = dato;
            this.dias = dias;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_exp_accResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_exp_accResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string auto_exp_accResult;
        
        public auto_exp_accResponse()
        {
        }
        
        public auto_exp_accResponse(string auto_exp_accResult)
        {
            this.auto_exp_accResult = auto_exp_accResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_sync_acc", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_sync_accRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string dias;
        
        public auto_sync_accRequest()
        {
        }
        
        public auto_sync_accRequest(string dato, string dias)
        {
            this.dato = dato;
            this.dias = dias;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="auto_sync_accResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class auto_sync_accResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string auto_sync_accResult;
        
        public auto_sync_accResponse()
        {
        }
        
        public auto_sync_accResponse(string auto_sync_accResult)
        {
            this.auto_sync_accResult = auto_sync_accResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="prueba_hcm", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class prueba_hcmRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string dato;
        
        public prueba_hcmRequest()
        {
        }
        
        public prueba_hcmRequest(string dato)
        {
            this.dato = dato;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="prueba_hcmResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class prueba_hcmResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string prueba_hcmResult;
        
        public prueba_hcmResponse()
        {
        }
        
        public prueba_hcmResponse(string prueba_hcmResult)
        {
            this.prueba_hcmResult = prueba_hcmResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadClock", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadClockRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] archivo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=1)]
        public string fileName;
        
        public UploadClockRequest()
        {
        }
        
        public UploadClockRequest(byte[] archivo, string fileName)
        {
            this.archivo = archivo;
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadClockResponse", WrapperNamespace="http://geotime.ddns.net/", IsWrapped=true)]
    public partial class UploadClockResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://geotime.ddns.net/", Order=0)]
        public string UploadClockResult;
        
        public UploadClockResponse()
        {
        }
        
        public UploadClockResponse(string UploadClockResult)
        {
            this.UploadClockResult = UploadClockResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface ServiceSoapChannel : GeoTimeServiceReference.ServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<GeoTimeServiceReference.ServiceSoap>, GeoTimeServiceReference.ServiceSoap
    {
        
        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(ServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), ServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.init_periodoResponse> init_periodoAsync(GeoTimeServiceReference.init_periodoRequest request)
        {
            return base.Channel.init_periodoAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.calculo_periodo_empleadoResponse> calculo_periodo_empleadoAsync(GeoTimeServiceReference.calculo_periodo_empleadoRequest request)
        {
            return base.Channel.calculo_periodo_empleadoAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.calculo_periodo_planillaResponse> calculo_periodo_planillaAsync(GeoTimeServiceReference.calculo_periodo_planillaRequest request)
        {
            return base.Channel.calculo_periodo_planillaAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.ncalculo_periodo_planillaResponse> ncalculo_periodo_planillaAsync(GeoTimeServiceReference.ncalculo_periodo_planillaRequest request)
        {
            return base.Channel.ncalculo_periodo_planillaAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.sincronizo_erpResponse> sincronizo_erpAsync(GeoTimeServiceReference.sincronizo_erpRequest request)
        {
            return base.Channel.sincronizo_erpAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.sincronizo_rerpResponse> sincronizo_rerpAsync(GeoTimeServiceReference.sincronizo_rerpRequest request)
        {
            return base.Channel.sincronizo_rerpAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.sincronizo_accionesResponse> sincronizo_accionesAsync(GeoTimeServiceReference.sincronizo_accionesRequest request)
        {
            return base.Channel.sincronizo_accionesAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_rdatResponse> obtengo_rdatAsync(GeoTimeServiceReference.obtengo_rdatRequest request)
        {
            return base.Channel.obtengo_rdatAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.inporto_accionesResponse> inporto_accionesAsync(GeoTimeServiceReference.inporto_accionesRequest request)
        {
            return base.Channel.inporto_accionesAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.importo_raccionesResponse> importo_raccionesAsync(GeoTimeServiceReference.importo_raccionesRequest request)
        {
            return base.Channel.importo_raccionesAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.exporto_conceptosResponse> exporto_conceptosAsync(GeoTimeServiceReference.exporto_conceptosRequest request)
        {
            return base.Channel.exporto_conceptosAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_conceptosResponse> obtengo_conceptosAsync(GeoTimeServiceReference.obtengo_conceptosRequest request)
        {
            return base.Channel.obtengo_conceptosAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_rconceptosResponse> obtengo_rconceptosAsync(GeoTimeServiceReference.obtengo_rconceptosRequest request)
        {
            return base.Channel.obtengo_rconceptosAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_tipoaccionResponse> obtengo_tipoaccionAsync(GeoTimeServiceReference.obtengo_tipoaccionRequest request)
        {
            return base.Channel.obtengo_tipoaccionAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.obtengo_rtipoaccionResponse> obtengo_rtipoaccionAsync(GeoTimeServiceReference.obtengo_rtipoaccionRequest request)
        {
            return base.Channel.obtengo_rtipoaccionAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.evaluo_formulaResponse> evaluo_formulaAsync(GeoTimeServiceReference.evaluo_formulaRequest request)
        {
            return base.Channel.evaluo_formulaAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.proceso_empleadoResponse> proceso_empleadoAsync(GeoTimeServiceReference.proceso_empleadoRequest request)
        {
            return base.Channel.proceso_empleadoAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.evaluo_movileResponse> evaluo_movileAsync(GeoTimeServiceReference.evaluo_movileRequest request)
        {
            return base.Channel.evaluo_movileAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.sinc_movileResponse> sinc_movileAsync(GeoTimeServiceReference.sinc_movileRequest request)
        {
            return base.Channel.sinc_movileAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.up_movileResponse> up_movileAsync(GeoTimeServiceReference.up_movileRequest request)
        {
            return base.Channel.up_movileAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.verif_faceResponse> verif_faceAsync(GeoTimeServiceReference.verif_faceRequest request)
        {
            return base.Channel.verif_faceAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.actualizo_companiaResponse> actualizo_companiaAsync(GeoTimeServiceReference.actualizo_companiaRequest request)
        {
            return base.Channel.actualizo_companiaAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.test_rdatResponse> test_rdatAsync(GeoTimeServiceReference.test_rdatRequest request)
        {
            return base.Channel.test_rdatAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.auto_sync_erpResponse> auto_sync_erpAsync(GeoTimeServiceReference.auto_sync_erpRequest request)
        {
            return base.Channel.auto_sync_erpAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.auto_calc_compResponse> auto_calc_compAsync(GeoTimeServiceReference.auto_calc_compRequest request)
        {
            return base.Channel.auto_calc_compAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.auto_supResponse> auto_supAsync(GeoTimeServiceReference.auto_supRequest request)
        {
            return base.Channel.auto_supAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.auto_empResponse> auto_empAsync(GeoTimeServiceReference.auto_empRequest request)
        {
            return base.Channel.auto_empAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.proceso_archivo_companiaResponse> proceso_archivo_companiaAsync(GeoTimeServiceReference.proceso_archivo_companiaRequest request)
        {
            return base.Channel.proceso_archivo_companiaAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.UploadFile_1Response> UploadFile_1Async(GeoTimeServiceReference.UploadFile_1Request request)
        {
            return base.Channel.UploadFile_1Async(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.UploadDataProyResponse> UploadDataProyAsync(GeoTimeServiceReference.UploadDataProyRequest request)
        {
            return base.Channel.UploadDataProyAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.UploadFileResponse> UploadFileAsync(GeoTimeServiceReference.UploadFileRequest request)
        {
            return base.Channel.UploadFileAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.compania_autoResponse> compania_autoAsync(GeoTimeServiceReference.compania_autoRequest request)
        {
            return base.Channel.compania_autoAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.actualizo_claveResponse> actualizo_claveAsync(GeoTimeServiceReference.actualizo_claveRequest request)
        {
            return base.Channel.actualizo_claveAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.test_mailResponse> test_mailAsync(GeoTimeServiceReference.test_mailRequest request)
        {
            return base.Channel.test_mailAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.auto_exp_accResponse> auto_exp_accAsync(GeoTimeServiceReference.auto_exp_accRequest request)
        {
            return base.Channel.auto_exp_accAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.auto_sync_accResponse> auto_sync_accAsync(GeoTimeServiceReference.auto_sync_accRequest request)
        {
            return base.Channel.auto_sync_accAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.prueba_hcmResponse> prueba_hcmAsync(GeoTimeServiceReference.prueba_hcmRequest request)
        {
            return base.Channel.prueba_hcmAsync(request);
        }
        
        public System.Threading.Tasks.Task<GeoTimeServiceReference.UploadClockResponse> UploadClockAsync(GeoTimeServiceReference.UploadClockRequest request)
        {
            return base.Channel.UploadClockAsync(request);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Service1Soap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.Service1Soap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            var appSettingsSection = config.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            //schemaAdmin = config.GetConnectionString("SchemaAdmin");

            if ((endpointConfiguration == EndpointConfiguration.Service1Soap))
            {
                return new System.ServiceModel.EndpointAddress(appSettings.WSEndPoint);
            }
            if ((endpointConfiguration == EndpointConfiguration.Service1Soap12))
            {
                return new System.ServiceModel.EndpointAddress(appSettings.WSEndPoint);
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            Service1Soap,
            
            Service1Soap12,
        }
    }
}
