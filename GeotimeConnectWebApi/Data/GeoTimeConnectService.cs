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
using System.Runtime.CompilerServices;

namespace GeoTimeConnectWebApi.Data
{
    public class GeoTimeConnectService : IGeoTimeConnectService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";

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
            _schema = schema;
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
                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e=>e.IdPlanilla== IdPlanilla && e.Inicio>=FechaInicio && e.Fin<=FechaFin)
                                       join inc in _context.Incidencias on ap.IdIncidencia equals inc.Id
                                       select new cAccionPersonal
                                       {
                                           IdRegistro = ap.IdRegistro,
                                           IdPlanilla = ap.IdPlanilla,
                                           IdNumero = ap.IdNumero,
                                           Inicio = ap.Inicio,
                                           Fin = ap.Fin,
                                           IdIncidencia = ap.IdIncidencia,
                                           Estado = ap.Estado,
                                           IdAccion = ap.IdAccion,
                                           Comentario = ap.Comentario,
                                           Dias = ap.Dias,
                                           Usuario = ap.Usuario,
                                           Fecha_Just = ap.Fecha_Just,
                                           Dias_Apl = ap.Dias_Apl,
                                           SolicitudId = ap.SolicitudId,
                                           Nom_Conector = inc.nom_conector
                                       }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin, string usuario)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
             

                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdPlanilla == IdPlanilla 
                                                                              && e.Inicio >= FechaInicio 
                                                                              && e.Fin <= FechaFin
                                                                              && (e.Usuario == usuario || e.Usuario == (usuario + "\\")))
                                        join inc in _context.Incidencias on ap.IdIncidencia equals inc.Id
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = inc.nom_conector
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string usuario, char estado)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {


                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdPlanilla == IdPlanilla
                                                                              && e.Estado == estado
                                                                              && (e.Usuario == usuario || e.Usuario == (usuario + "\\")))
                                        join inc in _context.Incidencias on ap.IdIncidencia equals inc.Id
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = inc.nom_conector
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return accionPersonal;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-30
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, char estado)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {


                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdPlanilla == IdPlanilla
                                                                              && e.Estado == estado)
                                        join inc in _context.Incidencias on ap.IdIncidencia equals inc.Id
                                        select new cAccionPersonal
                                        {
                                            IdRegistro = ap.IdRegistro,
                                            IdPlanilla = ap.IdPlanilla,
                                            IdNumero = ap.IdNumero,
                                            Inicio = ap.Inicio,
                                            Fin = ap.Fin,
                                            IdIncidencia = ap.IdIncidencia,
                                            Estado = ap.Estado,
                                            IdAccion = ap.IdAccion,
                                            Comentario = ap.Comentario,
                                            Dias = ap.Dias,
                                            Usuario = ap.Usuario,
                                            Fecha_Just = ap.Fecha_Just,
                                            Dias_Apl = ap.Dias_Apl,
                                            SolicitudId = ap.SolicitudId,
                                            Nom_Conector = inc.nom_conector
                                        }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
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
                    accion.IdAccion = 0;
                    _context.Add(accion);
                    await _context.SaveChangesAsync();

                    var ultimaAccion = await _context.Acciones_Personal.FirstOrDefaultAsync(e=>e.SolicitudId==accion.SolicitudId);
                    
                    if (ultimaAccion is not null)
                        await EjecutaAplicaAccionPersonal(ultimaAccion.IdRegistro);

                }
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
        //Sincronizar Centros de Costo
        //Parametro: Recibe una instancia de centro de costo, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_AccionPersonalNomConector(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {

                    var incidencia = await _context.Incidencias.FirstOrDefaultAsync(e => e.nom_conector == accion.Nom_Conector);
                    
                    if (incidencia is not null)
                    {
                        var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e=>e.IdRegistro== accion.IdRegistro);

                        if (accionbuscar is not null)
                        {
                            accionbuscar.Inicio = accion.Inicio;
                            accionbuscar.Fin = accion.Fin;         
                            accionbuscar.Dias = accion.Dias;
                            accionbuscar.Dias_Apl = accion.Dias_Apl;
                            accionbuscar.IdAccion = accion.IdAccion;
                            accionbuscar.Comentario = accion.Comentario;
                            if (accion.Estado != 'A')
                            {
                                accionbuscar.Estado = accion.Estado;
                            }

                            _context.Acciones_Personal.Update(accionbuscar);
                            await _context.SaveChangesAsync();
                            
                            if (accion.Estado == 'A')
                            {
                                await EjecutaAplicaAccionPersonal(accionbuscar.IdRegistro);
                            }                               
                        }
                        else
                        {
                            accion.IdIncidencia = incidencia.Id;
                            _context.Add(accion);
                            await _context.SaveChangesAsync();

                            var ultimaAccion = await _context.Acciones_Personal.MaxAsync(e => e.IdRegistro);

                            await EjecutaAplicaAccionPersonal(ultimaAccion);
                        }
                        
                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";                      
                        respuesta.Descripcion = "No logró encontrar la incidencia asociada a nom_conector";
                    } 
                    
                }
                
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

        public async Task EjecutaAplicaAccionPersonal(long idregistro)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + ".aplico_accpersonal @IDREGISTRO=" + idregistro;
                        System.Data.Common.DbDataReader result = command.ExecuteReader();

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
           

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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
               empleado = await _context.Empleados.Where(e=>e.Estado=='T').ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return empleado;
        }

        //Obtener Empleado por IdNumero
        public async Task<cEmpleado> GetEmpleado(string idNumero)
        {
            cEmpleado? empleado = new();
            try
            {
                empleado = await _context.Empleados.FirstOrDefaultAsync(e=>e.IdNumero == idNumero && e.Estado == 'T');
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return empleado;
        }

        


        //Obtener Empleado por IdNumero
        public async Task<cEmpleado> GetEmpleadoByEmail(string email)
        {
            cEmpleado? empleado = new();
           
            try
            {
                empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Email == email && e.Estado == 'T');
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
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
                           && e.Estado == 'T'
                           && e.Nombre!.ToLower().Contains(nombre.ToLower())
                           && e.IdDepartamento!.ToLower().Contains(iddepartamento.ToLower()))
                    .ToListAsync();
                }
                else {
                    if (idnumero == "" && nombre == "" && iddepartamento == "")
                    {
                        empleado = await _context.Empleados.Where(e=>e.Estado == 'T')  
                                            .ToListAsync();
                    }
                    else
                    {
                        if (idnumero != "")
                        {
                            empleado = await _context.Empleados
                             .Where(e => e.Estado == 'T' && e.IdNumero!.Contains(idnumero))
                             .ToListAsync();

                        }
                        else
                        {
                            if (nombre != "")
                            {
                                empleado = await _context.Empleados
                                 .Where(e => e.Estado == 'T' && e.Nombre!.ToLower().Contains(nombre.ToLower()))
                                 .ToListAsync();

                            }
                            else
                            {
                                if (iddepartamento != "")
                                {
                                    empleado = await _context.Empleados
                                     .Where(e => e.Estado == 'T' && e.IdDepartamento!.ToLower().Contains(iddepartamento))
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
                                    empleado = await _context.Empleados.Where(e => e.Estado == 'T')
                                    .ToListAsync();
                                }
                                
                            }
                        }

                    }
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
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
                DateTime fechaIngreso;
                foreach(var empleado in empleados)
                {
                    fechaIngreso =(DateTime) DateTime.Parse(empleado.Fecha_Ingreso.ToString());

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
                        emp.Fecha_Ingreso = fechaIngreso;

                        _context.Empleados.Update(emp);
                    }
                    else
                    {
                        empleado.Fecha_Ingreso = fechaIngreso;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
            }
            return incidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener una Incidencia especifica
        //Parametros: codigo=Codigo de incidencia a buscar
        public async Task<cIncidencia> GetIncidenciaByNomConector(string nom_conector)
        {
            cIncidencia? incidencia = new();
            try
            {
                incidencia = await _context.Incidencias.FirstOrDefaultAsync(e => e.nom_conector == nom_conector);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                                    //.Where(e=>e.IdPlanilla==idPlanilla && e.IdPeriodo==idPeriodo)
                                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                phlogin = await _context.PH_LOGIN.FirstOrDefaultAsync(e => e.EMAIL.ToUpper() == id.ToUpper());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
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
                Console.WriteLine(e.Message); throw;
            }
            return compania;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Obtener lista de Marcas_Mov_Turnos
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurno()
        {
            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                marcaMovTurno = await _context.Marcas_Mov_Turnos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Obtener un registro de Marcas_Mov_Turnos especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaMovTurno> GetMarcaMovTurno(int idregistro)
        {
            cMarcaMovTurno? marcaMovTurno = new();
            try
            {
                marcaMovTurno = await _context.Marcas_Mov_Turnos.FirstOrDefaultAsync(e => e.idregistro == idregistro);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Obtener un registro de Marcas_Mov_Turnos especifico
        //Parametros: idregistro=consecutivo de registro
        public async Task<cMarcaMovTurno> GetMarcaMovTurno(string idnumero, string fecha, int idturno)
        {
            cMarcaMovTurno? marcaMovTurno = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                marcaMovTurno = await _context.Marcas_Mov_Turnos.FirstOrDefaultAsync(e => e.idnumero == idnumero && e.fecha== fechaMov && e.turno== idturno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaMovTurno;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Sincronizar Marcas_MOv_Turnos
        //Parametro: Recibe una instancia de MarcaMovTurno, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcasMovTurnos(IEnumerable<cMarcaMovTurno> marcasMovTurnos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in marcasMovTurnos)
                {
                    cMarcaMovTurno? marcaMovTurno = await _context.Marcas_Mov_Turnos
                                    .Where(e => e.idnumero == item.idnumero && e.fecha == item.fecha)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (marcaMovTurno is not null)
                    {
                        marcaMovTurno.turno = item.turno;
                        marcaMovTurno.idplanilla = item.idplanilla;
                        marcaMovTurno.hora = item.hora;
                        _context.Marcas_Mov_Turnos.Update(marcaMovTurno);
                    }
                    else
                    {
                        item.idregistro = 0;
                        _context.Add(item);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Marcas_Mov_Turno. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

    }
}
