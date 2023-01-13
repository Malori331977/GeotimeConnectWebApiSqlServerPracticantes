using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;
using Seguridad_Geotime;

namespace GeoTimeConnectWebApi.Data
{
    public class GeoTimeConnectService : IGeoTimeConnectService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeoTimeConnectService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            IEnumerable <Claim> claims = _httpContextAccessor.HttpContext.User.Claims;
            string schema = "";
            string bdname = "";

            foreach (Claim clm in claims)
            {
                if (clm.Type.Contains("claims/givenname"))
                {
                    schema = clm.Value;
                }

                if (clm.Type.Contains("claims/spn"))
                {
                    bdname = clm.Value;
                }

                if (schema != "" && schema is not null && bdname != "" && bdname is not null)
                    break;

            }
            _context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
                accionPersonal = await _context.Acciones_Personal
                                       .Where(e=>e.IdPlanilla== IdPlanilla && e.Inicio==FechaInicio && e.Fin==FechaFin)
                                       .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Centros de Costo
        //Parametro: Recibe una instancia de centro de costo, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_AccionPersonal(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
    
                    //var accionBuscar = await _context.Acciones_Personal.Where(e=>e.IdAccion==accion.IdAccion && e.IdAccion!=null).FirstOrDefaultAsync();
                    
                    //if (accionBuscar != null)
                    //{
                    //    accionBuscar.IdIncidencia = accion.IdIncidencia;
                    //    accionBuscar.IdPlanilla = accion.IdPlanilla;
                    //    accionBuscar.IdNumero = accion.IdNumero;
                    //    accionBuscar.Inicio = accion.Inicio;
                    //    accionBuscar.Fin = accion.Fin;
                    //    accionBuscar.Dias = accion.Dias;
                    //    accionBuscar.Dias_Apl = accion.Dias_Apl;
                    //    accionBuscar.Estado = accion.Estado;
                    //    accionBuscar.Comentario = accion.Comentario;

                    //    _context.Acciones_Personal.Update(accion);

                    //}
                    //else
                    //{
                        //accion.IdAccion = null;
                        _context.Add(accion);
                    //}
                                       
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Personal. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Accion de Persona. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Centros de Costo
        public async Task<List<cCentroCosto>> GetCentroCosto()
        {
            List<cCentroCosto> centrosCosto = new();
            try
            {
                centrosCosto = await _context.Ph_CCostos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return centrosCosto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un centro de costo especifico
        //Parametros: idCCosto=centro de costo a buscar
        public async Task<cCentroCosto> GetCentroCosto(string idCCosto)
        {
            cCentroCosto? centrosCosto = new();
            try
            {
                centrosCosto = await _context.Ph_CCostos.FirstOrDefaultAsync(e => e.IdCCosto == idCCosto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return centrosCosto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Centros de Costo
        //Parametro: Recibe una instancia de centro de costo, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Centro_Costo(IEnumerable<cCentroCosto> centrosCosto)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var centroCosto in centrosCosto)
                {
                    cCentroCosto? ceco = await _context.Ph_CCostos
                                    .Where(e => e.IdCCosto == centroCosto.IdCCosto)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (ceco is not null)
                    {
                        ceco.Descripcion = centroCosto.Descripcion;
                        _context.Ph_CCostos.Update(ceco);
                    }
                    else
                    {
                        _context.Add(centroCosto);
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Centro de Costo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Centro de Costo. Detalle de Error: " + e.InnerException.Message;

            }
            
            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Conceptos
        public async Task<List<cConcepto>> GetConcepto()
        {
            List<cConcepto> concepto = new();
            try
            {
                concepto = await _context.Ph_Conceptos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return concepto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Concepto especifico
        //Parametros: concepto=concepto a buscar
        public async Task<cConcepto> GetConcepto(string concepto)
        {
            cConcepto? conceptos = new();
            try
            {
                conceptos = await _context.Ph_Conceptos.FirstOrDefaultAsync(e => e.Concepto == concepto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return conceptos;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Conceptos
        //Parametro: Recibe una instancia de concepto, se verifica si existe, en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Concepto(IEnumerable<cConcepto> conceptos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var concepto in conceptos)
                {
                    cConcepto? concept = await _context.Ph_Conceptos
                                                        .Where(e => e.Concepto == concepto.Concepto)
                                                        .FirstOrDefaultAsync();
                    //si el departamento existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (concept is not null)
                    {
                        concept.Descripcion = concepto.Descripcion;
                        _context.Ph_Conceptos.Update(concept);
                    }
                    else
                    {
                        concepto.tipo_h = 1;
                        concepto.tipo_j= 1;
                        concepto.columnar = 1;
                        concepto.factor = 1;
                        concepto.tolerancia = 0;
                        concepto.ordinario = 'T';
                        concepto.autorizado = 'T';
                        concepto.adicional = 'F';

                        _context.Add(concepto);
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Conceptos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Conceptos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Departamentos
        public async Task<List<cDepartamento>> GetDepartamento()
        {
            List<cDepartamento> departamento = new();
            try
            {
                departamento = await _context.Ph_Departamento.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return departamento;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Departamento especifico
        //Parametros: idDepart=Id de departamento a buscar
        public async Task<cDepartamento> GetDepartamento(string idDepart)
        {
            cDepartamento? departamento = new();
            try
            {
                departamento = await _context.Ph_Departamento.FirstOrDefaultAsync(e => e.IdDepart == idDepart);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return departamento;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Departamentos
        //Parametro: Recibe una instancia de departamento, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Departamento(IEnumerable<cDepartamento> departamentos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var departamento in departamentos)
                {
                    cDepartamento? depto = await _context.Ph_Departamento
                                                        .Where(e => e.IdDepart == departamento.IdDepart)
                                                        .FirstOrDefaultAsync();
                    //si el departamento existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (depto is not null)
                    {
                        depto.Descripcion = departamento.Descripcion;
                        _context.Ph_Departamento.Update(depto);
                    }
                    else
                    {
                        _context.Add(departamento);
                    }
                }                
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Departamentos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Departamentos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Empleados
        public async Task<List<cEmpleado>> GetEmpleado()
        {
            List<cEmpleado> empleado = new();
            try
            {
               empleado = await _context.Empleados.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return empleado;
        }

        //Obtener Empleado por IdNumero
        public async Task<cEmpleado> GetEmpleado(string idNumero)
        {
            cEmpleado? empleado = new();
            try
            {
                empleado = await _context.Empleados.FirstOrDefaultAsync(e=>e.IdNumero == idNumero);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return empleado;
        }

        


        //Obtener Empleado por IdNumero
        public async Task<cEmpleado> GetEmpleadoByEmail(string email)
        {
            cEmpleado? empleado = new();
           
            try
            {
                empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Email == email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return empleado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Empleados
        public async Task<List<cEmpleado>> GetEmpleadoFiltrado(string idnumero, string nombre, string iddepartamento)
        {
            List<cEmpleado> empleado = new();

            if (idnumero == "all")
                idnumero = "";
            if (nombre == "all")
                nombre = "";
            if (iddepartamento == "all")
                iddepartamento = "";

            try
            {
                if (idnumero != "" && nombre != "" && iddepartamento != "")
                {
                    empleado = await _context.Empleados
                    .Where(e => e.IdNumero!.Contains(idnumero)
                           && e.Nombre!.ToLower().Contains(nombre.ToLower())
                           && e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                    .ToListAsync();
                }
                else {
                    if (idnumero == "" && nombre == "" && iddepartamento == "")
                    {
                        empleado = await _context.Empleados  
                                            .ToListAsync();
                    }
                    else
                    {
                        if (idnumero != "")
                        {
                            empleado = await _context.Empleados
                             .Where(e => e.IdNumero!.Contains(idnumero))
                             .ToListAsync();

                        }
                        else
                        {
                            if (nombre != "")
                            {
                                empleado = await _context.Empleados
                                 .Where(e => e.Nombre!.ToLower().Contains(nombre.ToLower()))
                                 .ToListAsync();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = await _context.Empleados
                                     .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento))
                                     .ToListAsync();

                                }
                            }
                        }

                        if (idnumero != "")
                        {
                            if (nombre != "")
                            {
                                empleado = empleado
                                 .Where(e => e.Nombre!.ToLower().Contains(nombre.ToLower()))
                                 .ToList();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = empleado
                                            .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                                            .ToList();
                                }

                            }
                        }
                        else
                        {
                            if (nombre != "")
                            {
                                empleado = empleado
                                 .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                                 .ToList();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = empleado
                                            .Where(e => e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                                            .ToList();
                                }
                                else
                                {
                                    empleado = await _context.Empleados
                                    .ToListAsync();
                                }
                                
                            }
                        }

                    }
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return empleado;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Empleados
        //Parametro: Recibe una instancia de Empleado, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Empleado(IEnumerable<cEmpleado> empleados)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var empleado in empleados)
                {
                    cEmpleado? emp = await _context.Empleados
                                    .Where(e => e.IdNumero == empleado.IdNumero)
                                    .FirstOrDefaultAsync();
                    //si el empleado existe se actualiza registro
                    //de lo contrario se agrega el registro
                    if (emp is not null)
                    {
                        emp.Estado = empleado.Estado;
                        emp.Nombre = empleado.Nombre;
                        emp.IdDepartamento = empleado.IdDepartamento;
                        emp.IdCCosto = empleado.IdCCosto;
                        emp.IdPlanilla = empleado.IdPlanilla;

                        _context.Empleados.Update(emp);
                    }
                    else
                    {
                        empleado.IdGrupo = 1;
                        empleado.IdHorario = 1;
                        empleado.Tipo_Marca = "H";
                        empleado.IdAgrupamiento = 0;
                        _context.Add(empleado);
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Empleados. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Incidencias

        public async Task<List<cIncidencia>> GetIncidencia()
        {
            List<cIncidencia> incidencia = new();
            try
            {
                incidencia = await _context.Incidencias
                        .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return incidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener una Incidencia especifica
        //Parametros: codigo=Codigo de incidencia a buscar
        public async Task<cIncidencia> GetIncidencia(int id)
        {
            cIncidencia? incidencia = new();
            try
            {
                incidencia = await _context.Incidencias.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return incidencia;
        }

        public async Task<List<cIncidencia>> GetIncidenciaReqAccPer()
        {
            List<cIncidencia> incidencia = new();
            try
            {
                incidencia = await _context.Incidencias
                        .Where(e => e.requiere_accper == "T")
                        .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return incidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Incidencia
        //Parametro: Recibe una instancia de Incidencia, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Incidencia(IEnumerable<cIncidencia> incidencias)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach(var incidencia in incidencias)
                {
                    cIncidencia? incident = await _context.Incidencias
                                    .Where(e => e.Codigo == incidencia.Codigo)
                                    .FirstOrDefaultAsync();
                    //si el departamento existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (incident is not null)
                    {
                        incident.Descripcion = incidencia.Descripcion;
                        _context.Incidencias.Update(incident);
                    }
                    else
                    {
                        _context.Add(incidencia);
                    }
                }                
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Incidencias. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Incidencias. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas Resumen
        public async Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla, string idPeriodo)
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = await _context.Marcas_Resumen
                                    .Where(e=>e.IdPlanilla==idPlanilla && e.IdPeriodo==idPeriodo)
                                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return marcasResumen;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Turnos
        public async Task<List<cTurno>> GetTurno()
        {
            List<cTurno> turno = new();
            try
            {
                turno = await _context.Ph_Turnos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return turno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Turno especifico
        //Parametros: codigo=Codigo de incidencia a buscar
        public async Task<cTurno> GetTurno(int idTurno)
        {
            cTurno? turno = new();
            try
            {
                turno = await _context.Ph_Turnos.FirstOrDefaultAsync(e => e.IdTurno == idTurno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return turno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Marcas
        public async Task<List<cMarca>> GetMarcas()
        {
            List<cMarca> marcas = new();
            try
            {
                marcas = await _context.Marcas.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return marcas;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener las marcas de un empleado
        //Parametros: idnumero=numero de empleado a buscar
        public async Task<List<cMarca>> GetMarcas(string idnumero)
        {
            List<cMarca>? marca = new();
            try
            {
                marca = await _context.Marcas.Where(e => e.idnumero == idnumero).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return marca;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Sincronizar Marcas
        //Parametro: Recibe una lista de marcas, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Marca(IEnumerable<cMarca> marcas)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var marca in marcas)
                {
                    _context.Add(marca);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Validar Clave de Usuario
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se valida contraseña indicada contra la registrada en la base de datos
        public async Task<EventResponse> ValidarClaveEmpleado(cLogin login)
        {
            EventResponse respuesta = new EventResponse();
            funciones.funciones_geo funcionesGeo = new();

            try
            {
                var emp = await _context.Empleados.FirstAsync(e => e.IdNumero == login.Usuario);

                if (emp is not null)
                {
                    var pass = funcionesGeo.Global_encrypt(login.Password);

                    if (emp.global_clave!= pass)
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "La contraseña indicada no es válida.";
                    }
                    
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos del empleado.";

                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Validar Clave de Usuario
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se valida contraseña indicada contra la registrada en la base de datos
        public async Task<EventResponse> CambiarClaveEmpleado(cEmpleado empleado)
        {
            EventResponse respuesta = new EventResponse();
            funciones.funciones_geo funcionesGeo = new();

            try
            {
                var emp = await _context.Empleados.FirstAsync(e => e.IdNumero == empleado.IdNumero);

                if (emp is not null)
                {
                    var pass = funcionesGeo.Global_encrypt(empleado.global_clave);

                    emp.global_clave = pass;

                    _context.Empleados.Update(emp);

                    var phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.EMAIL.ToLower() == emp.Email.ToLower());
                    if (phlogin is not null)
                    {
                        phlogin.GLOBAL_CLAVE = pass;
                        _context.PH_LOGIN.Update(phlogin);
                    }
                    

                    await _context.SaveChangesAsync();
                }
                else
                {
                    respuesta.Id = "1";
                    respuesta.Respuesta = "Error";
                    respuesta.Descripcion = "No se encontraron los datos del empleado.";

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo validar los datos del empleado. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener datos de phlogin de una cuenta de usuario
        //Parametros: id=email de empleado a buscar
        public async Task<cPh_Login> GetPhLogin(string id)
        {
            cPh_Login? phlogin = new();

            try
            {
                phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.EMAIL == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return phlogin;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-01-02
        //Obtener lista de Centros de Costo
        public async Task<List<cPh_Compania>> GetPhCompania()
        {
            List<cPh_Compania> companias = new();
            try
            {
                companias = await _context.PH_COMPANIAS.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return companias;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un centro de costo especifico
        //Parametros: idCCosto=centro de costo a buscar
        public async Task<cPh_Compania> GetPhCompania(string idcomp)
        {
            cPh_Compania? compania = new();
            try
            {
                compania = await _context.PH_COMPANIAS.FirstOrDefaultAsync(e => e.idcomp == idcomp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return compania;
        }

    }
}
