

using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace com.gsitcr.geotime.Data
{
    public class SincronizaErp:ISincronizaErp
    {
        private readonly IGeoTimeConnectService _geoConnect;
        private readonly IErpConnectService _erpConnect;
        private readonly ILogger<GeoTimeConnectService> _logger;
        public SincronizaErp(IGeoTimeConnectService geoConnect, IErpConnectService erpConnect, ILogger<GeoTimeConnectService> logger)
        {
            _geoConnect = geoConnect;
            _erpConnect = erpConnect;
        }

        public async Task<EventResponse> SincronizaDepartamentos()
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var datosErp = await _erpConnect.GetDepartamentoErp();
                var datosGeo = await _geoConnect.GetDepartamento();
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
                            Distribuye = itemErp.Acepta_Datos=="S"?'T':'F',

                        });
                    }
                    else
                    {
                        // Actualizar CENTRO COSTO existente en GeoTime si es necesario
                        bool necesitaActualizar = false;
                        if (modelGeo.Descripcion != itemErp.Descripcion || (itemErp.Acepta_Datos == "S" ? 'T' : 'F')!= modelGeo.Distribuye )
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
                            Activo = itemErp.Activo,

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
                var datosErp = await _erpConnect.GetEmpleadoErp();
                var datosGeo = await _geoConnect.GetEmpleado();
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
                            Estado = itemErp.ACTIVO=="S"?'T':'F',
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
                            Fecha_Salida = itemErp.FECHA_SALIDA.Equals(new DateTime(1980,01,01)) ? null: itemErp.FECHA_SALIDA,
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
    }
}
