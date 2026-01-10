

using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using JtSegEncrypta;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace com.gsitcr.geotime.Data
{
    public class SincronizaErp : ISincronizaErp
    {
        private readonly IGeoTimeConnectService _geoConnect;
        private readonly IErpConnectService _erpConnect;
        private readonly ILogger<SincronizaErp> _logger;
        public SincronizaErp(IGeoTimeConnectService geoConnect, 
                             IErpConnectService erpConnect, 
                             ILogger<SincronizaErp> logger)
        {
            _logger = logger;
            _geoConnect = geoConnect;
            _erpConnect = erpConnect;
        }
        

        public async Task<EventResponse> SincronizaDepartamentos()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                
                var datosGeo = await _geoConnect.GetDepartamento();
                var datosErp = await _erpConnect.GetDepartamentoErp();

                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Departamentos de {_geoConnect._schema}.");

                List<cDepartamento> modelList = new List<cDepartamento>();

                // Lógica para sincronizar departamentos entre ERP y GeoTime
                foreach (var deptErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => d.IDDEPART == deptErp.Departamento);
                    if (modelGeo == null)
                    {
                        modelList.Add(new cDepartamento
                        {
                            IDDEPART = deptErp.Departamento,
                            DESCRIPCION = deptErp.Descripcion,

                        });
                    }
                    else
                    {
                        // Actualizar departamento existente en GeoTime si es necesario
                        bool necesitaActualizar = false;
                        if (modelGeo.DESCRIPCION != deptErp.Descripcion)
                        {
                            modelGeo.DESCRIPCION = deptErp.Descripcion;
                            necesitaActualizar = true;
                        }
                        if (necesitaActualizar) modelList.Add(modelGeo);
                    }
                }
                if (modelList.Count() > 0)
                {
                    respuesta = await _geoConnect.Sincronizar_Departamento(modelList);
                }
                _logger.LogWarning($"SincronizaErp: Fin de Sincronización de Departamentos de {_geoConnect._schema}.");
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los departamentos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los departamentos. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaDepartamentos: {respuesta.Descripcion}");
            }
            return respuesta;
        }

        public async Task<EventResponse> SincronizaCentrosCosto()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Centros de Costo de {_geoConnect._schema}.");

                var datosErp = await _erpConnect.GetCentroCostoErp();
                var datosGeo = await _geoConnect.GetCentroCosto();
                List<cCentroCosto> modelList = new List<cCentroCosto>();

                // Lógica para sincronizar centros de costo entre ERP y GeoTime
                foreach (var itemErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => d.IdCCosto == itemErp.Centro_Costo);
                    if (modelGeo == null)
                    {
                        modelList.Add(new cCentroCosto
                        {
                            IdCCosto = itemErp.Centro_Costo,
                            Descripcion = itemErp.Descripcion,
                            Distribuye = itemErp.Acepta_Datos == "S" ? 'T' : 'F',

                        });
                    }
                    else
                    {
                        // Actualizar CENTRO COSTO existente en GeoTime si es necesario
                        bool necesitaActualizar = false;
                        if (modelGeo.Descripcion != itemErp.Descripcion || (itemErp.Acepta_Datos == "S" ? 'T' : 'F') != modelGeo.Distribuye)
                        {
                            modelGeo.Descripcion = itemErp.Descripcion;
                            modelGeo.Distribuye = itemErp.Acepta_Datos == "S" ? 'T' : 'F';
                            necesitaActualizar = true;
                        }
                        if (necesitaActualizar) modelList.Add(modelGeo);

                    }
                }
                if (modelList.Count > 0)
                    respuesta = await _geoConnect.Sincronizar_Centro_Costo(modelList);

                _logger.LogWarning($"SincronizaErp: Fin de sincronización de Centros de Costo de {_geoConnect._schema}.");
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los departamentos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los departamentos. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaCentrosCosto: {respuesta.Descripcion}");
            }
            return respuesta;
        }

        public async Task<EventResponse> SincronizaPuestos()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Puestos de {_geoConnect._schema}.");

                var datosErp = await _erpConnect.GetPuestoErp();
                var datosGeo = await _geoConnect.GetPhPuesto();
                List<cPh_Puesto> modelList = new List<cPh_Puesto>();

                // Lógica para sincronizar puestos entre ERP y GeoTime
                foreach (var itemErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => d.Puesto == itemErp.Puesto);
                    if (modelGeo == null)
                    {
                        modelList.Add(new cPh_Puesto
                        {
                            Puesto = itemErp.Puesto,
                            Descripcion = itemErp.Descripcion,
                            Activo = itemErp.Activo??"S",

                        });
                    }
                    else
                    {
                        // Actualizar CENTRO COSTO existente en GeoTime si es necesario
                        bool necesitaActualizar = false;
                        if (modelGeo.Descripcion != itemErp.Descripcion || (itemErp.Activo != modelGeo.Activo))
                        {
                            modelGeo.Descripcion = itemErp.Descripcion;
                            modelGeo.Activo = itemErp.Activo;
                            necesitaActualizar = true;
                        }
                        if (necesitaActualizar) modelList.Add(modelGeo);

                    }
                }
                if (modelList.Count > 0)
                    respuesta = await _geoConnect.Sincronizar_PhPuesto(modelList);

                _logger.LogWarning($"SincronizaErp: Fin de sincronización de Puestos de {_geoConnect._schema}.");
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los puestos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los puestos. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaPuestos: {respuesta.Descripcion}");
            }
            return respuesta;
        }

        public async Task<EventResponse> SincronizaEmpleados()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Empleados de {_geoConnect._schema}.");

                var datosErp = await _erpConnect.GetEmpleadoByNominaErp("-1");
                var datosGeo = await _geoConnect.GetEmpleado();
                var planillas = await _geoConnect.GetPhPlanilla();

                List<cEmpleado> modelList = new List<cEmpleado>();

                // Lógica para sincronizar empleados entre ERP y GeoTime
                foreach (var itemErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => d.IdNumero == itemErp.EMPLEADO);

                    var planillaEmpleado = planillas.FirstOrDefault(p => p.nom_conector == itemErp.NOMINA);

                    if (planillaEmpleado is not null)
                    {
                        if (modelGeo == null)
                        {
                            modelList.Add(new cEmpleado
                            {
                                IdNumero = itemErp.EMPLEADO,
                                IdPlanilla = planillaEmpleado.idplanilla,
                                Nombre = itemErp.NOMBRE,
                                Tarjeta = itemErp.EMPLEADO,
                                Identificacion = itemErp.IDENTIFICACION,
                                IdGrupo = 1,
                                IdDepartamento = itemErp.DEPARTAMENTO,
                                IdHorario = 1,
                                Estado = itemErp.ACTIVO == "S" ? 'T' : 'F',
                                IdAgrupamiento = 0,
                                foto = itemErp.FOTOGRAFIA,
                                IdCCosto = itemErp.CENTRO_COSTO,
                                exporta = 'T',
                                ubicacion = itemErp.UBICACION,
                                rubro1 = itemErp.RUBRO1,
                                rubro2 = itemErp.RUBRO2,
                                rubro3 = itemErp.RUBRO3,
                                rubro4 = itemErp.RUBRO4,
                                rubro5 = itemErp.RUBRO5,
                                rubro6 = itemErp.RUBRO6,
                                rubro7 = itemErp.RUBRO7,
                                rubro8 = itemErp.RUBRO8,
                                rubro9 = itemErp.RUBRO9,
                                rubro10 = itemErp.RUBRO10,
                                rubro11 = itemErp.RUBRO11,
                                rubro12 = itemErp.RUBRO12,
                                rubro13 = itemErp.RUBRO13,
                                rubro14 = itemErp.RUBRO14,
                                rubro15 = itemErp.RUBRO15,
                                rubro16 = itemErp.RUBRO16,
                                rubro17 = itemErp.RUBRO17,
                                rubro18 = itemErp.RUBRO18,
                                rubro19 = itemErp.RUBRO19,
                                rubro20 = itemErp.RUBRO20,
                                rubro21 = itemErp.RUBRO21,
                                rubro22 = itemErp.RUBRO22,
                                rubro23 = itemErp.RUBRO23,
                                rubro24 = itemErp.RUBRO24,
                                rubro25 = itemErp.RUBRO25,
                                Fecha_Ingreso = itemErp.FECHA_INGRESO,
                                Email = itemErp.E_MAIL,
                                Tipo_Marca = "H",
                                inicio_rol = null,
                                web_pass = null,
                                id_transfo_conc = null,
                                widioma = null,
                                def_cc = null,
                                def_py = null,
                                global_code = null,
                                def_fase = null,
                                fecha_act_code = null,
                                global_clave = null,
                                Fecha_Salida = itemErp.FECHA_SALIDA.Equals(new DateTime(1980, 01, 01)) ? null : itemErp.FECHA_SALIDA,
                                puesto = itemErp.PUESTO,
                                TCompensacionAprobado = "00:00",
                                TCompensacionUtilizado = "00:00",
                                Sexo = itemErp.SEXO,
                                FechaNacimiento = itemErp.FECHA_NACIMIENTO,


                            });


                        }
                        else
                        {
                            modelList.Add(new cEmpleado
                            {
                                IdNumero = itemErp.EMPLEADO,
                                IdPlanilla = planillaEmpleado.idplanilla,
                                Nombre = itemErp.NOMBRE,
                                Identificacion = itemErp.IDENTIFICACION,
                                IdDepartamento = itemErp.DEPARTAMENTO,
                                Estado = itemErp.ACTIVO == "S" ? 'T' : 'F',
                                foto = itemErp.FOTOGRAFIA,
                                IdCCosto = itemErp.CENTRO_COSTO,
                                ubicacion = itemErp.UBICACION,
                                rubro1 = itemErp.RUBRO1,
                                rubro2 = itemErp.RUBRO2,
                                rubro3 = itemErp.RUBRO3,
                                rubro4 = itemErp.RUBRO4,
                                rubro5 = itemErp.RUBRO5,
                                rubro6 = itemErp.RUBRO6,
                                rubro7 = itemErp.RUBRO7,
                                rubro8 = itemErp.RUBRO8,
                                rubro9 = itemErp.RUBRO9,
                                rubro10 = itemErp.RUBRO10,
                                rubro11 = itemErp.RUBRO11,
                                rubro12 = itemErp.RUBRO12,
                                rubro13 = itemErp.RUBRO13,
                                rubro14 = itemErp.RUBRO14,
                                rubro15 = itemErp.RUBRO15,
                                rubro16 = itemErp.RUBRO16,
                                rubro17 = itemErp.RUBRO17,
                                rubro18 = itemErp.RUBRO18,
                                rubro19 = itemErp.RUBRO19,
                                rubro20 = itemErp.RUBRO20,
                                rubro21 = itemErp.RUBRO21,
                                rubro22 = itemErp.RUBRO22,
                                rubro23 = itemErp.RUBRO23,
                                rubro24 = itemErp.RUBRO24,
                                rubro25 = itemErp.RUBRO25,
                                Email = itemErp.E_MAIL,
                                Fecha_Salida = itemErp.FECHA_SALIDA.Equals(new DateTime(1980, 01, 01)) ? null : itemErp.FECHA_SALIDA,
                                puesto = itemErp.PUESTO,
                                Sexo = itemErp.SEXO,
                                FechaNacimiento = itemErp.FECHA_NACIMIENTO,
                                Fecha_Ingreso = itemErp.FECHA_INGRESO,
                            });

                        }
                    }
                }
                if (modelList.Count > 0)
                    respuesta = await _geoConnect.Sincronizar_Empleado(modelList);

                _logger.LogWarning($"SincronizaErp: Fin de sincronización de Empleados de {_geoConnect._schema}.");

            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los empleados. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los empleados. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaEmpleados: {respuesta.Descripcion}");
            }
            return respuesta;
        }

        public async Task<EventResponse> SincronizaEmpleados(cSincronizo_erp param)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Empleados de {_geoConnect._schema}.");

                var planilla = await _geoConnect.GetPhPlanilla(param.IdPlanilla);

                if (planilla == null || string.IsNullOrEmpty(planilla.nom_conector))
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontró la planilla con el conector especificado.";
                    _logger.LogError($"SincronizaErp.SincronizaEmpleados: {respuesta.Descripcion}");
                    return respuesta;
                }

                _logger.LogWarning($"SincronizaErp: Sincronizando empleados para la planilla {planilla.planilla} con conector {planilla.nom_conector}.");
                var datosErp = await _erpConnect.GetEmpleadoByNominaErp(planilla.nom_conector!);

                _logger.LogWarning($"SincronizaErp: Empleados obtenidos del ERP para la planilla {planilla.planilla}: {datosErp.Count()} registros.");
                var datosGeo = await _geoConnect.GetEmpleado();

                _logger.LogWarning($"SincronizaErp: Empleados obtenidos de GeoTime: {datosGeo.Count()} registros.");
                List<cEmpleado> modelList = new List<cEmpleado>();

                // Lógica para sincronizar empleados entre ERP y GeoTime
                foreach (var itemErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => d.IdNumero == itemErp.EMPLEADO);
                    if (modelGeo == null)
                    {
                        modelList.Add(new cEmpleado
                        {
                            IdNumero = itemErp.EMPLEADO,
                            IdPlanilla = itemErp.NOMINA,
                            Nombre = itemErp.NOMBRE,
                            Tarjeta = itemErp.EMPLEADO,
                            Identificacion = itemErp.IDENTIFICACION,
                            IdGrupo = 1,
                            IdDepartamento = itemErp.DEPARTAMENTO,
                            IdHorario = 1,
                            Estado = itemErp.ACTIVO == "S" ? 'T' : 'F',
                            IdAgrupamiento = 0,
                            foto = itemErp.FOTOGRAFIA,
                            IdCCosto = itemErp.CENTRO_COSTO,
                            exporta = 'T',
                            ubicacion = itemErp.UBICACION,
                            rubro1 = itemErp.RUBRO1,
                            rubro2 = itemErp.RUBRO2,
                            rubro3 = itemErp.RUBRO3,
                            rubro4 = itemErp.RUBRO4,
                            rubro5 = itemErp.RUBRO5,
                            rubro6 = itemErp.RUBRO6,
                            rubro7 = itemErp.RUBRO7,
                            rubro8 = itemErp.RUBRO8,
                            rubro9 = itemErp.RUBRO9,
                            rubro10 = itemErp.RUBRO10,
                            rubro11 = itemErp.RUBRO11,
                            rubro12 = itemErp.RUBRO12,
                            rubro13 = itemErp.RUBRO13,
                            rubro14 = itemErp.RUBRO14,
                            rubro15 = itemErp.RUBRO15,
                            rubro16 = itemErp.RUBRO16,
                            rubro17 = itemErp.RUBRO17,
                            rubro18 = itemErp.RUBRO18,
                            rubro19 = itemErp.RUBRO19,
                            rubro20 = itemErp.RUBRO20,
                            rubro21 = itemErp.RUBRO21,
                            rubro22 = itemErp.RUBRO22,
                            rubro23 = itemErp.RUBRO23,
                            rubro24 = itemErp.RUBRO24,
                            rubro25 = itemErp.RUBRO25,
                            Fecha_Ingreso = itemErp.FECHA_INGRESO,
                            Email = itemErp.E_MAIL,
                            Tipo_Marca = "H",
                            inicio_rol = null,
                            web_pass = null,
                            id_transfo_conc = null,
                            widioma = null,
                            def_cc = null,
                            def_py = null,
                            global_code = null,
                            def_fase = null,
                            fecha_act_code = null,
                            global_clave = null,
                            Fecha_Salida = itemErp.FECHA_SALIDA.Equals(new DateTime(1980, 01, 01)) ? null : itemErp.FECHA_SALIDA,
                            puesto = itemErp.PUESTO,
                            TCompensacionAprobado = "00:00",
                            TCompensacionUtilizado = "00:00",
                            Sexo = itemErp.SEXO,
                            FechaNacimiento = itemErp.FECHA_NACIMIENTO,


                        });
                    }
                    else
                    {
                        modelList.Add(new cEmpleado
                        {
                            IdNumero = itemErp.EMPLEADO,
                            IdPlanilla = itemErp.NOMINA,
                            Nombre = itemErp.NOMBRE,
                            Identificacion = itemErp.IDENTIFICACION,
                            IdDepartamento = itemErp.DEPARTAMENTO,
                            Estado = itemErp.ACTIVO == "S" ? 'T' : 'F',
                            foto = itemErp.FOTOGRAFIA,
                            IdCCosto = itemErp.CENTRO_COSTO,
                            ubicacion = itemErp.UBICACION,
                            rubro1 = itemErp.RUBRO1,
                            rubro2 = itemErp.RUBRO2,
                            rubro3 = itemErp.RUBRO3,
                            rubro4 = itemErp.RUBRO4,
                            rubro5 = itemErp.RUBRO5,
                            rubro6 = itemErp.RUBRO6,
                            rubro7 = itemErp.RUBRO7,
                            rubro8 = itemErp.RUBRO8,
                            rubro9 = itemErp.RUBRO9,
                            rubro10 = itemErp.RUBRO10,
                            rubro11 = itemErp.RUBRO11,
                            rubro12 = itemErp.RUBRO12,
                            rubro13 = itemErp.RUBRO13,
                            rubro14 = itemErp.RUBRO14,
                            rubro15 = itemErp.RUBRO15,
                            rubro16 = itemErp.RUBRO16,
                            rubro17 = itemErp.RUBRO17,
                            rubro18 = itemErp.RUBRO18,
                            rubro19 = itemErp.RUBRO19,
                            rubro20 = itemErp.RUBRO20,
                            rubro21 = itemErp.RUBRO21,
                            rubro22 = itemErp.RUBRO22,
                            rubro23 = itemErp.RUBRO23,
                            rubro24 = itemErp.RUBRO24,
                            rubro25 = itemErp.RUBRO25,
                            Email = itemErp.E_MAIL,
                            Fecha_Salida = itemErp.FECHA_SALIDA.Equals(new DateTime(1980, 01, 01)) ? null : itemErp.FECHA_SALIDA,
                            puesto = itemErp.PUESTO,
                            Sexo = itemErp.SEXO,
                            FechaNacimiento = itemErp.FECHA_NACIMIENTO,
                            Fecha_Ingreso = itemErp.FECHA_INGRESO,
                        });

                    }
                }
                if (modelList.Count > 0)
                    respuesta = await _geoConnect.Sincronizar_Empleado(modelList);

                respuesta = await PostSincronizacion(param.IdPlanilla);

                _logger.LogWarning($"SincronizaErp: Fin de sincronización de Empleados de {_geoConnect._schema}.");

                
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los empleados. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los empleados. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaEmpleados: {respuesta.Descripcion}");
            }
            return respuesta;
        }

        public async Task<EventResponse> SincronizaConceptos()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Conceptos de {_geoConnect._schema}.");

                var datosErp = await _erpConnect.GetConceptoErp();
                var datosGeo = await _geoConnect.GetConcepto();
                List<cConcepto> modelList = new List<cConcepto>();

                datosErp = datosErp.Where(d => d.CANT_EDITABLE=="S");

                // Lógica para sincronizar puestos entre ERP y GeoTime
                foreach (var itemErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => d.nominaeq == itemErp.CONCEPTO);
                    if (modelGeo == null)
                    {
                        modelList.Add(new cConcepto
                        {
                            Concepto = itemErp.CONCEPTO,
                            Descripcion = itemErp.DESCRIPCION,
                            nominaeq = itemErp.CONCEPTO,
                            ordinario = 'T',
                            transferir = 'T',
                            autorizado = 'T',
                            adicional = 'F',
                            tipo_ext_alm = 'F',
                            tipo_j = 1,
                            tipo_h = 1,
                            columnar = 1,
                            factor = 1,
                            tolerancia = 0,
                            muestra_resumen = null,
                        });
                    }
                }
                if (modelList.Count > 0)
                    respuesta = await _geoConnect.Sincronizar_Concepto(modelList);

                _logger.LogWarning($"SincronizaErp: Fin sincronización de Conceptos de {_geoConnect._schema}.");
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los conceptos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de los conceptos. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaConceptos: {respuesta.Descripcion}");
            }
            return respuesta;
        }

        public async Task<EventResponse> SincronizaNominas()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                _logger.LogWarning($"SincronizaErp: Iniciando sincronización de Nóminas de {_geoConnect._schema}.");

                var datosErp = await _erpConnect.GetNominaErp();
                var datosGeo = await _geoConnect.GetPhPlanilla();
                List<cPh_Planilla> modelList = new List<cPh_Planilla>();

                // Lógica para sincronizar nominas entre ERP y GeoTime
                foreach (var itemErp in datosErp)
                {
                    var modelGeo = datosGeo.FirstOrDefault(d => (d.nom_conector??"").ToUpper() == itemErp.NOMINA.ToUpper());
                    if (modelGeo == null)
                    {
                        modelList.Add(new cPh_Planilla
                        {
                            idplanilla = itemErp.NOMINA,
                            planilla = itemErp.DESCRIPCION,
                            nom_conector = itemErp.NOMINA,
                            tipo_planilla = itemErp.TIPO_NOMINA[0],
                            c_ext = 'F',
                            c_inci = 'F',
                            c_adic = 'F',
                            m_desc = 'F',
                            proyecta = 'F',
                            dia_inicio = 0,
                            auto_proceso = 'F',
                            tipo_dist = 'C',
                            est_nomina = "M",
                            ext_per_ant = 'F',
                            ext_det = 'F',
                            agrup_salida = 'F',
                            tipo_adic = 'H',
                            nivel_aprob_ext =1
                        });
                    }
                }
                if (modelList.Count > 0)
                    respuesta = await _geoConnect.Sincronizar_PhPlanilla(modelList);

                _logger.LogWarning($"SincronizaErp: Fin sincronización de Nóminas de {_geoConnect._schema}.");
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las nóminas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las nóminas. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.SincronizaNominas: {respuesta.Descripcion}");
            }
            return respuesta;
        }


        public async Task<EventResponse> PostSincronizacion(string idPlanilla)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                var planillas = await _geoConnect.GetPhPlanilla();

                if (idPlanilla != "-1")
                {
                    planillas = planillas.Where(p => p.idplanilla == idPlanilla).ToList();
                }

                foreach(var planilla in planillas)
                {
                    _logger.LogWarning($"SincronizaErp.PostSincronizacion: Iniciando Tareas posteriores en {_geoConnect._schema} para la nómina {planilla.idplanilla}");

                    respuesta = await _geoConnect.EjecutaPostSincroniza(planilla.idplanilla);

                    _logger.LogWarning($"SincronizaErp.PostSincronizacion: Fin sincronización de Tareas posteriores en {_geoConnect._schema} para la nómina {planilla.idplanilla}.");
                }
            }
            catch (Exception e)
            {
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar las tareas posteriores. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar las tareas posteriores. Detalle de Error: " + e.InnerException.Message;

                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"SincronizaErp.PostSincronizacion: {respuesta.Descripcion}");
            }
            return respuesta;
        }
    }
}
