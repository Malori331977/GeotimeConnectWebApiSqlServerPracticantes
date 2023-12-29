using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;
using Microsoft.EntityFrameworkCore;
using Seguridad_Geotime;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using LibEncripta;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext.User.Claims;
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

            if (schema == "")
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                schema = config.GetConnectionString("Schema");
            }
            _schema = schema;
            _context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un registro de Acción de Personal
        public async Task<cAccionPersonal> GetAccionPersonal(long idregistro)
        {
            cAccionPersonal? accionPersonal = new();
            try
            {
                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdRegistro == idregistro)
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
                                            Nom_Conector = inc.nom_conector,
                                        }).FirstOrDefaultAsync();
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
        public async Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdPlanilla == IdPlanilla && e.Inicio >= FechaInicio && e.Fin <= FechaFin)
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
                                                                              && (e.Usuario.Contains(usuario)))
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
        //Fecha: 2023-05-30
        /// <summary>
        /// GetAccionPersonalPorPeriodo: Acciones de Personal 
        /// </summary>
        /// <param name="IdPlanilla"></param>
        /// <param name="usuario"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        //Obtener lista de Acciones de Personal
        public async Task<List<cAccionPersonal>> GetAccionPersonalPorPeriodo(string idnumero, string fecha, string IdPlanilla)
        {
            List<cAccionPersonal> accionPersonal = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                accionPersonal = await (from ap in _context.Acciones_Personal.Where(e => e.IdNumero == idnumero
                                                                              && e.IdPlanilla == IdPlanilla
                                                                              && (e.Inicio >= periodoVigente.inicio && e.Inicio <= periodoVigente.fin && e.Fin <= periodoVigente.fin)
                                                                              || (e.Inicio < periodoVigente.inicio && e.Fin >= periodoVigente.inicio && e.Fin <= periodoVigente.fin)
                                                                              || (e.Inicio >= periodoVigente.inicio && e.Inicio <= periodoVigente.fin && e.Fin > periodoVigente.fin)
                                                                              || (e.Inicio < periodoVigente.inicio && e.Fin > periodoVigente.fin))
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
        //Sincronizar acciones de personal
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

                    var ultimaAccion = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.SolicitudId == accion.SolicitudId);

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
        public async Task<EventResponse> Sincronizar_AccionPersonal_AutoGestion(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    var planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.nom_conector == accion.IdPlanilla);

                    if (planilla is not null)
                    {
                        accion.IdAccion = 0;
                        accion.IdPlanilla = planilla.idplanilla;

                        _context.Add(accion);
                        await _context.SaveChangesAsync();

                        var ultimaAccion = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.SolicitudId == accion.SolicitudId);

                        if (ultimaAccion is not null)
                            await EjecutaAplicaAccionPersonal(ultimaAccion.IdRegistro);
                    }
                    else
                    {
                        respuesta.Id = "1";
                        respuesta.Respuesta = "Error";
                        respuesta.Descripcion = "No logró encontrar el id de planilla asociado a nom_conector";
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
                        var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.IdRegistro == accion.IdRegistro);

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

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        //Sincronizar AccionPersonal PreJustificacion
        //Parametro: Recibe una lista de Acciones de Personal y las crea en GeoTime
        public async Task<EventResponse> Sincronizar_AccionPersonal_PreJustificacion(IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var accion in accionPersonal)
                {
                    var accionbuscar = await _context.Acciones_Personal.FirstOrDefaultAsync(e => e.IdRegistro == accion.IdRegistro);

                    if (accionbuscar is not null)
                    {
                        accionbuscar.IdIncidencia = accion.IdIncidencia;
                        accionbuscar.Inicio = accion.Inicio;
                        accionbuscar.Fin = accion.Fin;
                        accionbuscar.Dias = accion.Dias;
                        accionbuscar.Dias_Apl = accion.Dias_Apl;
                        accionbuscar.IdAccion = accion.IdAccion;
                        accionbuscar.Comentario = accion.Comentario;

                        _context.Acciones_Personal.Update(accionbuscar);
                        await _context.SaveChangesAsync();

                        var marcasIncidencias = await GetMarcaIncidencia(accion.IdNumero, accion.IdPlanilla, accion.Inicio, accion.Fin);

                        foreach (cMarcaIncidencia item in marcasIncidencias)
                        {
                            item.FECHA_JUST = DateTime.Now;
                            item.IDACC = accionbuscar.IdRegistro;

                            _context.Marcas_Incidencias.Update(item);
                            await _context.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        accion.IdAccion = 0;
                        _context.Add(accion);
                        await _context.SaveChangesAsync();

                        var ultimaAccion = await _context.Acciones_Personal.MaxAsync(e => e.IdRegistro);
                        var marcasIncidencias = await GetMarcaIncidencia(accion.IdNumero, accion.IdPlanilla, accion.Inicio, accion.Fin);

                        foreach (cMarcaIncidencia item in marcasIncidencias)
                        {
                            item.FECHA_JUST = DateTime.Now;
                            item.IDACC = ultimaAccion;

                            _context.Marcas_Incidencias.Update(item);
                            await _context.SaveChangesAsync();
                        }
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
                foreach (var centroCosto in centrosCosto)
                {
                    cCentroCosto? ceco = await _context.Ph_CCostos
                                    .Where(e => e.IdCCosto == centroCosto.IdCCosto)
                                    .FirstOrDefaultAsync();
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (ceco is not null)
                    {
                        ceco.Descripcion = centroCosto.Descripcion;
                        ceco.Distribuye = centroCosto.Distribuye;

                        _context.Ph_CCostos.Update(ceco);
                    }
                    else
                    {
                        _context.Add(centroCosto);
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
                foreach (var concepto in conceptos)
                {
                    cConcepto? concept = await _context.Ph_Conceptos
                                                        .Where(e => e.Concepto == concepto.Concepto)
                                                        .FirstOrDefaultAsync();
                    //si el consepto existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (concept is not null)
                    {
                        concept.Descripcion = concepto.Descripcion;
                        _context.Ph_Conceptos.Update(concept);
                    }
                    else
                    {
                        concepto.tipo_h = 1;
                        concepto.tipo_j = 1;
                        concepto.columnar = 1;
                        concepto.factor = 1;
                        concepto.tolerancia = 0;
                        concepto.ordinario = 'T';
                        concepto.autorizado = 'T';
                        concepto.adicional = 'F';
                        concepto.tipo_ext_alm = null;
                        concepto.muestra_resumen = null;

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
                departamento = await _context.Ph_Departamento.FirstOrDefaultAsync(e => e.IDDEPART == idDepart);
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
                foreach (var departamento in departamentos)
                {
                    cDepartamento? depto = await _context.Ph_Departamento
                                                        .Where(e => e.IDDEPART == departamento.IDDEPART)
                                                        .FirstOrDefaultAsync();
                    //si el departamento existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (depto is not null)
                    {
                        depto.DESCRIPCION = departamento.DESCRIPCION;
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
        /// <summary>
        /// GetEmpleado: Método para obtener una lista de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public async Task<List<cEmpleado>> GetEmpleado()
        {
            List<cEmpleado> empleado = new();
            try
            {
                empleado = (from e in await _context.Empleados
                                    .Include(e => e.Departamento)
                                    .Include(e => e.CentroCosto)
                                    .Include(e => e.Ph_Planilla)
                                .Where(e => e.Estado == 'T').ToListAsync()
                                select new cEmpleado
                                {
                                    IdNumero = e.IdNumero,
                                    IdPlanilla = e.IdPlanilla,
                                    Nombre = e.Nombre,
                                    Tarjeta = e.Tarjeta,
                                    Identificacion = e.Identificacion,
                                    IdGrupo = e.IdGrupo,
                                    IdDepartamento = e.IdDepartamento,
                                    IdHorario = e.IdHorario,
                                    Estado = e.Estado,
                                    IdAgrupamiento = e.IdAgrupamiento,
                                    foto = e.foto,
                                    IdCCosto = e.IdCCosto,
                                    exporta = e.exporta,
                                    ubicacion = e.ubicacion,
                                    rubro1 = e.rubro1,
                                    rubro2 = e.rubro2,
                                    rubro3 = e.rubro3,
                                    rubro4 = e.rubro4,
                                    rubro5 = e.rubro5,
                                    rubro6 = e.rubro6,
                                    rubro7 = e.rubro7,
                                    rubro8 = e.rubro8,
                                    rubro9 = e.rubro9,
                                    rubro10 = e.rubro10,
                                    rubro11 = e.rubro11,
                                    rubro12 = e.rubro12,
                                    rubro13 = e.rubro13,
                                    rubro14 = e.rubro14,
                                    rubro15 = e.rubro15,
                                    rubro16 = e.rubro16,
                                    rubro17 = e.rubro17,
                                    rubro18 = e.rubro18,
                                    rubro19 = e.rubro19,
                                    rubro20 = e.rubro20,
                                    rubro21 = e.rubro21,
                                    rubro22 = e.rubro22,
                                    rubro23 = e.rubro23,
                                    rubro24 = e.rubro24,
                                    rubro25 = e.rubro25,
                                    Fecha_Ingreso = e.Fecha_Ingreso,
                                    Email = e.Email,
                                    Tipo_Marca = e.Tipo_Marca,
                                    inicio_rol = e.inicio_rol,
                                    web_pass = e.web_pass,
                                    id_transfo_conc = e.id_transfo_conc,
                                    widioma = e.widioma,
                                    global_clave = e.global_clave,
                                    def_fase = e.def_fase,
                                    def_py = e.def_py,
                                    def_cc = e.def_cc,
                                    Fecha_Salida = e.Fecha_Salida,
                                    global_code = e.global_code,
                                    fecha_act_code = e.fecha_act_code,
                                    Departamento = e.Departamento==null?null:
                                                   new cDepartamento
                                                   {
                                                       IDDEPART = e.Departamento.IDDEPART,
                                                       DESCRIPCION = e.Departamento.DESCRIPCION,
                                                   },
                                    CentroCosto = e.CentroCosto == null ? null :
                                                   new cCentroCosto
                                                   {
                                                       IdCCosto = e.CentroCosto.IdCCosto,
                                                       Descripcion = e.CentroCosto.Descripcion,
                                                       Distribuye = e.CentroCosto.Distribuye,                                                       
                                                   },
                                    Ph_Planilla = e.Ph_Planilla == null ? null :
                                                   new cPh_Planilla
                                                   {
                                                       idplanilla = e.Ph_Planilla.idplanilla,
                                                       planilla = e.Ph_Planilla.planilla,
                                                       nom_conector = e.Ph_Planilla.nom_conector,
                                                       tipo_planilla = e.Ph_Planilla.tipo_planilla,
                                                       c_ext = e.Ph_Planilla.c_ext,
                                                       c_inci = e.Ph_Planilla.c_inci,
                                                       c_adic = e.Ph_Planilla.c_adic,
                                                       m_desc = e.Ph_Planilla.m_desc,
                                                       proyecta = e.Ph_Planilla.proyecta,
                                                       dia_inicio = e.Ph_Planilla.dia_inicio,
                                                       auto_proceso = e.Ph_Planilla.auto_proceso,
                                                       tipo_dist = e.Ph_Planilla.tipo_dist,
                                                       est_nomina = e.Ph_Planilla.est_nomina,
                                                       ext_per_ant = e.Ph_Planilla.ext_per_ant,
                                                       ext_det = e.Ph_Planilla.ext_det,
                                                       agrup_salida = e.Ph_Planilla.agrup_salida,
                                                       tipo_adic = e.Ph_Planilla.tipo_adic,
                                                       nivel_aprob_ext = e.Ph_Planilla.nivel_aprob_ext,
                                                   },

                                }).ToList();

     
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return empleado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetEmpleadoTotal: Método para obtener la lista total de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public async Task<List<cEmpleado>> GetEmpleadoTotal()
        {
            List<cEmpleado> empleado = new();
            try
            {
                empleado = (from e in await _context.Empleados
                                    .Include(e => e.Departamento)
                                    .Include(e => e.CentroCosto)
                                    .Include(e => e.Ph_Planilla)
                                    .ToListAsync()
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                Departamento = e.Departamento == null ? null :
                                               new cDepartamento
                                               {
                                                   IDDEPART = e.Departamento.IDDEPART,
                                                   DESCRIPCION = e.Departamento.DESCRIPCION,
                                               },
                                CentroCosto = e.CentroCosto == null ? null :
                                               new cCentroCosto
                                               {
                                                   IdCCosto = e.CentroCosto.IdCCosto,
                                                   Descripcion = e.CentroCosto.Descripcion,
                                                   Distribuye = e.CentroCosto.Distribuye,
                                               },
                                Ph_Planilla = e.Ph_Planilla == null ? null :
                                               new cPh_Planilla
                                               {
                                                   idplanilla = e.Ph_Planilla.idplanilla,
                                                   planilla = e.Ph_Planilla.planilla,
                                                   nom_conector = e.Ph_Planilla.nom_conector,
                                                   tipo_planilla = e.Ph_Planilla.tipo_planilla,
                                                   c_ext = e.Ph_Planilla.c_ext,
                                                   c_inci = e.Ph_Planilla.c_inci,
                                                   c_adic = e.Ph_Planilla.c_adic,
                                                   m_desc = e.Ph_Planilla.m_desc,
                                                   proyecta = e.Ph_Planilla.proyecta,
                                                   dia_inicio = e.Ph_Planilla.dia_inicio,
                                                   auto_proceso = e.Ph_Planilla.auto_proceso,
                                                   tipo_dist = e.Ph_Planilla.tipo_dist,
                                                   est_nomina = e.Ph_Planilla.est_nomina,
                                                   ext_per_ant = e.Ph_Planilla.ext_per_ant,
                                                   ext_det = e.Ph_Planilla.ext_det,
                                                   agrup_salida = e.Ph_Planilla.agrup_salida,
                                                   tipo_adic = e.Ph_Planilla.tipo_adic,
                                                   nivel_aprob_ext = e.Ph_Planilla.nivel_aprob_ext,
                                               },

                            }).ToList();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return empleado;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetEmpleado: Método para un empleado específico
        /// </summary>
        /// <returns>Una instancia de la clase cEmpleado</returns>
        /// ///<param name="idNumero">idNumero del empleado requerido</param>
        public async Task<cEmpleado> GetEmpleado(string idNumero)
        {
            cEmpleado? empleado = new();
            try
            {
                empleado = (from e in await _context.Empleados
                                    .Include(e => e.Departamento)
                                    .Include(e => e.CentroCosto)
                                    .Include(e => e.Ph_Planilla)
                                .Where(e => e.IdNumero == idNumero).ToListAsync()
                            select new cEmpleado
                            {
                                IdNumero = e.IdNumero,
                                IdPlanilla = e.IdPlanilla,
                                Nombre = e.Nombre,
                                Tarjeta = e.Tarjeta,
                                Identificacion = e.Identificacion,
                                IdGrupo = e.IdGrupo,
                                IdDepartamento = e.IdDepartamento,
                                IdHorario = e.IdHorario,
                                Estado = e.Estado,
                                IdAgrupamiento = e.IdAgrupamiento,
                                foto = e.foto,
                                IdCCosto = e.IdCCosto,
                                exporta = e.exporta,
                                ubicacion = e.ubicacion,
                                rubro1 = e.rubro1,
                                rubro2 = e.rubro2,
                                rubro3 = e.rubro3,
                                rubro4 = e.rubro4,
                                rubro5 = e.rubro5,
                                rubro6 = e.rubro6,
                                rubro7 = e.rubro7,
                                rubro8 = e.rubro8,
                                rubro9 = e.rubro9,
                                rubro10 = e.rubro10,
                                rubro11 = e.rubro11,
                                rubro12 = e.rubro12,
                                rubro13 = e.rubro13,
                                rubro14 = e.rubro14,
                                rubro15 = e.rubro15,
                                rubro16 = e.rubro16,
                                rubro17 = e.rubro17,
                                rubro18 = e.rubro18,
                                rubro19 = e.rubro19,
                                rubro20 = e.rubro20,
                                rubro21 = e.rubro21,
                                rubro22 = e.rubro22,
                                rubro23 = e.rubro23,
                                rubro24 = e.rubro24,
                                rubro25 = e.rubro25,
                                Fecha_Ingreso = e.Fecha_Ingreso,
                                Email = e.Email,
                                Tipo_Marca = e.Tipo_Marca,
                                inicio_rol = e.inicio_rol,
                                web_pass = e.web_pass,
                                id_transfo_conc = e.id_transfo_conc,
                                widioma = e.widioma,
                                global_clave = e.global_clave,
                                def_fase = e.def_fase,
                                def_py = e.def_py,
                                def_cc = e.def_cc,
                                Fecha_Salida = e.Fecha_Salida,
                                global_code = e.global_code,
                                fecha_act_code = e.fecha_act_code,
                                Departamento = e.Departamento == null ? null :
                                               new cDepartamento
                                               {
                                                   IDDEPART = e.Departamento.IDDEPART,
                                                   DESCRIPCION = e.Departamento.DESCRIPCION,
                                               },
                                CentroCosto = e.CentroCosto == null ? null :
                                               new cCentroCosto
                                               {
                                                   IdCCosto = e.CentroCosto.IdCCosto,
                                                   Descripcion = e.CentroCosto.Descripcion,
                                                   Distribuye = e.CentroCosto.Distribuye,
                                               },
                                Ph_Planilla = e.Ph_Planilla == null ? null :
                                                   new cPh_Planilla
                                                   {
                                                       idplanilla = e.Ph_Planilla.idplanilla,
                                                       planilla = e.Ph_Planilla.planilla,
                                                       nom_conector = e.Ph_Planilla.nom_conector,
                                                       tipo_planilla = e.Ph_Planilla.tipo_planilla,
                                                       c_ext = e.Ph_Planilla.c_ext,
                                                       c_inci = e.Ph_Planilla.c_inci,
                                                       c_adic = e.Ph_Planilla.c_adic,
                                                       m_desc = e.Ph_Planilla.m_desc,
                                                       proyecta = e.Ph_Planilla.proyecta,
                                                       dia_inicio = e.Ph_Planilla.dia_inicio,
                                                       auto_proceso = e.Ph_Planilla.auto_proceso,
                                                       tipo_dist = e.Ph_Planilla.tipo_dist,
                                                       est_nomina = e.Ph_Planilla.est_nomina,
                                                       ext_per_ant = e.Ph_Planilla.ext_per_ant,
                                                       ext_det = e.Ph_Planilla.ext_det,
                                                       agrup_salida = e.Ph_Planilla.agrup_salida,
                                                       tipo_adic = e.Ph_Planilla.tipo_adic,
                                                       nivel_aprob_ext = e.Ph_Planilla.nivel_aprob_ext,
                                                   },

                            }).FirstOrDefault();
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
                else
                {
                    if (idnumero == "" && nombre == "" && iddepartamento == "")
                    {
                        empleado = await _context.Empleados.Where(e => e.Estado == 'T')
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
                string idPlanillaAnt;
                foreach (var empleado in empleados)
                {
                    if (empleado.Fecha_Ingreso is null)
                        fechaIngreso = DateTime.Now;
                    else
                        fechaIngreso = (DateTime)DateTime.Parse(empleado.Fecha_Ingreso.ToString());


                    cEmpleado? emp = await _context.Empleados
                                    .Where(e => e.IdNumero == empleado.IdNumero)
                                    .FirstOrDefaultAsync();
                    //si el empleado existe se actualiza registro
                    //de lo contrario se agrega el registro
                    if (emp is not null)
                    {
                        //si estado nuevo es inactivo
                        if (empleado.Estado == 'F')
                            //si no viene la fecha de salida se asigna fecha del dia que se inactiva
                            if (empleado.Fecha_Salida is null)
                                emp.Fecha_Salida = DateTime.Now;
                            else
                                emp.Fecha_Salida = empleado.Fecha_Salida;
                        else
                            emp.Fecha_Salida = empleado.Fecha_Salida;

                        idPlanillaAnt = emp.IdPlanilla;
                        emp.Estado = empleado.Estado;
                        emp.Nombre = empleado.Nombre;
                        emp.IdDepartamento = empleado.IdDepartamento;
                        emp.IdCCosto = empleado.IdCCosto;
                        emp.IdPlanilla = empleado.IdPlanilla;
                        emp.Fecha_Ingreso = fechaIngreso;

                        emp.IdGrupo = (empleado.IdGrupo != null && empleado.IdGrupo != 0) ? empleado.IdGrupo : emp.IdGrupo;
                        emp.IdHorario = (empleado.IdHorario != null && empleado.IdHorario != 0) ? empleado.IdHorario : emp.IdHorario;
                        emp.Tipo_Marca = (empleado.Tipo_Marca != null && empleado.Tipo_Marca != "") ? empleado.Tipo_Marca : emp.Tipo_Marca;
                        emp.IdAgrupamiento = (empleado.IdAgrupamiento != null && empleado.IdAgrupamiento != 0) ? empleado.IdAgrupamiento : emp.IdAgrupamiento;
                        emp.Email = empleado.Email ?? emp.Email;
                        emp.Tarjeta = empleado.Tarjeta ?? emp.Tarjeta;
                        emp.exporta = (empleado.exporta != null) ? empleado.exporta : emp.exporta;  
                        emp.id_transfo_conc = (empleado.id_transfo_conc != null && empleado.id_transfo_conc != 0) ? empleado.id_transfo_conc : emp.id_transfo_conc;

                        _context.Empleados.Update(emp);
                        await _context.SaveChangesAsync();

                        if (idPlanillaAnt != empleado.IdPlanilla)
                        {
                            await EjecutaPostCambioPlanilla(empleado.IdNumero, idPlanillaAnt, empleado.IdPlanilla);
                        }
                    }
                    else
                    {
                        empleado.Fecha_Ingreso = fechaIngreso;
                        empleado.Fecha_Salida = null;
                        empleado.IdGrupo = (empleado.IdGrupo==null || empleado.IdGrupo==0)? 1:empleado.IdGrupo;
                        empleado.IdHorario = (empleado.IdHorario == null || empleado.IdHorario == 0) ? 1 : empleado.IdHorario; 
                        empleado.Tipo_Marca = (empleado.Tipo_Marca == null || empleado.Tipo_Marca == "") ? "H" : empleado.Tipo_Marca;
                        empleado.IdAgrupamiento = (empleado.IdAgrupamiento == null) ? 0 : empleado.IdAgrupamiento;
                        empleado.Email = empleado.Email??"";
                        empleado.Tarjeta = empleado.Tarjeta ?? "";
                        empleado.exporta = empleado.exporta ?? 'T';

                        empleado.Departamento = null;
                        empleado.CentroCosto = null;
                        empleado.Ph_Planilla = null;

                       

                        _context.Add(empleado);
                        await _context.SaveChangesAsync();
                    }
                }


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

        /// <summary>
        /// Elimina_Empleado:  Metodo borrado de datos de la tabla Empleado
        /// </summary>
        /// <param name="idnumero"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Empleado(string idnumero)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cEmpleado? model = await _context.Empleados
                    .FirstOrDefaultAsync(e => e.IdNumero == idnumero);

                if (model is not null)
                {
                    _context.Empleados.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Empleado. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Empleado. Detalle de Error: " + e.InnerException.Message;

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
                foreach (var incidencia in incidencias)
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

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        //Obtener lista de Marcas Resumen
        /// <summary>
        /// Elimina_Incidencia:  Metodo borrado de datos de la tabla Incidencias
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Incidencia(int id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {
                cIncidencia? model = await _context.Incidencias
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (model is not null)
                {
                    _context.Incidencias.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la Incidencia. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la Incidencia. Detalle de Error: " + e.InnerException.Message;
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
                                    .Where(e => e.IdPlanilla == idPlanilla && e.IdPeriodo == idPeriodo)
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
        //Obtener lista de Marcas Resumen
        public async Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla)
        {
            List<cMarcaResumen> marcasResumen = new();
            try
            {
                marcasResumen = await _context.Marcas_Resumen
                                    .Where(e => e.IdPlanilla == idPlanilla)
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

        /// <summary>
        /// Sincronizar_Turno: metodo para sincronizar los Turnos 
        /// </summary>
        /// <param name="phTurno"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_Turno(IEnumerable<cTurno> phTurno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in phTurno)
                {
                    cTurno? objetoBuscar = await _context.Ph_Turnos
                                    .FirstOrDefaultAsync(e => e.IdTurno == item.IdTurno);
                    //si el Turno existe se actualiza descripción
                    //de lo contrario se agrega el registro*
                    if (objetoBuscar is not null)
                    {
                        /* Datos de relleno */
                        objetoBuscar.IdTurno = item.IdTurno;
                        objetoBuscar.Descripcion = item.Descripcion;

                        /* Datops listos para cuando se modifique el metodo correctamente */

                        objetoBuscar.IdTurno = item.IdTurno;
                        objetoBuscar.Descripcion = item.Descripcion;
                        objetoBuscar.HEntra = item.HEntra;
                        objetoBuscar.HSale = item.HSale;
                        objetoBuscar.tar_apl = item.tar_apl;
                        objetoBuscar.ant_apl = item.ant_apl;
                        objetoBuscar.des_1_in = item.des_1_in;
                        objetoBuscar.des_1_out = item.des_1_out;
                        objetoBuscar.des_2_in = item.des_2_in;
                        objetoBuscar.des_2_out = item.des_2_out;
                        objetoBuscar.des_3_in = item.des_3_in;
                        objetoBuscar.des_3_out = item.des_3_out;
                        objetoBuscar.apl_des_1 = item.apl_des_1;
                        objetoBuscar.apl_des_2 = item.apl_des_2;
                        objetoBuscar.apl_des_3 = item.apl_des_3;
                        objetoBuscar.des_1_tiem = item.des_1_tiem;
                        objetoBuscar.des_2_tiem = item.des_2_tiem;
                        objetoBuscar.des_3_tiem = item.des_3_tiem;
                        objetoBuscar.marca_des_1 = item.marca_des_1;
                        objetoBuscar.marca_des_2 = item.marca_des_2;
                        objetoBuscar.marca_des_3 = item.marca_des_3;
                        objetoBuscar.tar_tiem = item.tar_tiem;
                        objetoBuscar.ant_tiem = item.ant_tiem;
                        objetoBuscar.con_1 = item.con_1;
                        objetoBuscar.con_2 = item.con_2;
                        objetoBuscar.con_3 = item.con_3;
                        objetoBuscar.con_4 = item.con_4;
                        objetoBuscar.con_5 = item.con_5;
                        objetoBuscar.con_6 = item.con_6;
                        objetoBuscar.cant_con_1 = item.cant_con_1;
                        objetoBuscar.cant_con_2 = item.cant_con_2;
                        objetoBuscar.cant_con_3 = item.cant_con_3;
                        objetoBuscar.cant_con_4 = item.cant_con_4;
                        objetoBuscar.cant_con_5 = item.cant_con_5;
                        objetoBuscar.cant_con_6 = item.cant_con_6;
                        objetoBuscar.min_con_1 = item.min_con_1;
                        objetoBuscar.min_con_2 = item.min_con_2;
                        objetoBuscar.min_con_3 = item.min_con_3;
                        objetoBuscar.min_con_4 = item.min_con_4;
                        objetoBuscar.min_con_5 = item.min_con_5;
                        objetoBuscar.min_con_6 = item.min_con_6;
                        objetoBuscar.Tipo = item.Tipo;
                        objetoBuscar.Tipo_Jor = item.Tipo_Jor;
                        objetoBuscar.fuerza_calc = item.fuerza_calc;
                        objetoBuscar.idagrupamiento = item.idagrupamiento;
                        objetoBuscar.apl_trans1 = item.apl_trans1;
                        objetoBuscar.id_trans1 = item.id_trans1;
                        objetoBuscar.apl_trans2 = item.apl_trans2;
                        objetoBuscar.id_trans2 = item.id_trans2;
                        objetoBuscar.apl_trans3 = item.apl_trans3;
                        objetoBuscar.id_trans3 = item.id_trans3;
                        objetoBuscar.apl_trans4 = item.apl_trans4;
                        objetoBuscar.id_trans4 = item.id_trans4;
                        objetoBuscar.apl_trans5 = item.apl_trans5;
                        objetoBuscar.id_trans5 = item.id_trans5;
                        objetoBuscar.apl_trans6 = item.apl_trans6;
                        objetoBuscar.id_trans6 = item.id_trans6;
                        objetoBuscar.apl_ben1 = item.apl_ben1;
                        objetoBuscar.id_ben1 = item.id_ben1;
                        objetoBuscar.apl_ben2 = item.apl_ben2;
                        objetoBuscar.id_ben2 = item.id_ben2;
                        objetoBuscar.apl_ben3 = item.apl_ben3;
                        objetoBuscar.id_ben3 = item.id_ben3;
                        objetoBuscar.apl_ben4 = item.apl_ben4;
                        objetoBuscar.id_ben4 = item.id_ben4;
                        objetoBuscar.apl_ben5 = item.apl_ben5;
                        objetoBuscar.id_ben5 = item.id_ben5;
                        objetoBuscar.apl_ben6 = item.apl_ben6;
                        objetoBuscar.id_ben6 = item.id_ben6;
                        objetoBuscar.conc_ben1 = item.conc_ben1;
                        objetoBuscar.conc_ben2 = item.conc_ben2;
                        objetoBuscar.conc_ben3 = item.conc_ben3;
                        objetoBuscar.conc_ben4 = item.conc_ben4;
                        objetoBuscar.conc_ben5 = item.conc_ben5;
                        objetoBuscar.conc_ben6 = item.conc_ben6;
                        objetoBuscar.apl_trans_post = item.apl_trans_post;
                        objetoBuscar.id_trans_post = item.id_trans_post;
                        objetoBuscar.apl_redond_entrada = item.apl_redond_entrada;
                        objetoBuscar.cant_redond_entrada = item.cant_redond_entrada;
                        objetoBuscar.auto_pan = item.auto_pan;

                        _context.Ph_Turnos.Update(objetoBuscar);
                    }
                    else
                    {
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización del PhTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del PhTurno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina_Turno:  Metodo borrado de datos de la tabla Ph_Turnos
        /// </summary>
        /// <param name="idturno"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Turno(int idturno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cTurno? model = await _context.Ph_Turnos
                    .FirstOrDefaultAsync(e => e.IdTurno == idturno);

                if (model is not null)
                {
                    _context.Ph_Turnos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el PhTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el PhTurno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
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
        /// <summary>
        /// GetMarcas: Obtener las marcas de un empleado para el periodo activo
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarca>> GetMarcas(string idnumero, string fecha)
        {
            List<cMarca>? marca = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marca = await _context.Marcas.Where(e => e.idnumero == idnumero && (DateTime)e.fecha_hora >= periodoVigente.inicio && (DateTime)e.fecha_hora <= periodoVigente.fin).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marca;
        }

        /// <summary>
        /// GetMarcasDiaria: Obtener las marcas del dia para un empleado 
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha del dia</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarca>> GetMarcasDiaria(string idnumero, string fecha)
        {
            List<cMarca>? marca = new();
            try
            {
                DateTime fechaDia = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                marca = await _context.Marcas.Where(e => e.idnumero == idnumero 
                                                 && e.fecha == fechaDia).ToListAsync();
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
                    // var m = funcionesGeo.Encrypt(login.Password);

                    if (emp.global_clave != pass)
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
        //Fecha: 2023-11-01
        //CambiarCodigoSeguridadEmpleado
        //Parametro: Recibe una instancia de empleado, se verifica si existe
        //y se actualiza codigo de seguridad
        public async Task<EventResponse> CambiarCodigoSeguridadEmpleado(cEmpleado empleado)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var emp = await _context.Empleados.FirstAsync(e => e.IdNumero == empleado.IdNumero);

                if (emp is not null)
                {
                    emp.global_code = empleado.global_code;
                    emp.fecha_act_code = DateTime.Now;

                    _context.Empleados.Update(emp);

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
                compania = await _context.PH_COMPANIAS.FirstOrDefaultAsync(e => e.IDCOMP == idcomp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return compania;
        }

        /// <summary>
        /// Sincronizar_PhCompania: metodo para sincronizar las compañias 
        /// </summary>
        /// <param name="phCompanias"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_PhCompania(IEnumerable<cPh_Compania> phCompanias)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in phCompanias)
                {
                    cPh_Compania? objetoBuscar = await _context.PH_COMPANIAS
                                    .FirstOrDefaultAsync(e => e.IDCOMP == item.IDCOMP);
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.COMPANIA = item.COMPANIA;
                        objetoBuscar.NOM_CONECTOR = item.NOM_CONECTOR;
                        objetoBuscar.STRING_SQL = item.STRING_SQL;
                        objetoBuscar.STRING_SQL_ERP = item.STRING_SQL_ERP;
                        objetoBuscar.PAIS = item.PAIS;
                        objetoBuscar.AUTO_PROCESO = item.AUTO_PROCESO;
                        objetoBuscar.REMOTE_ERPSERVICE = item.REMOTE_ERPSERVICE;
                        objetoBuscar.MAIL_SERVER = item.MAIL_SERVER;
                        objetoBuscar.MAIL_USER = item.MAIL_USER;
                        objetoBuscar.MAIL_PASSWORD = item.MAIL_PASSWORD;
                        objetoBuscar.MAIL_PORT = item.MAIL_PORT;
                        objetoBuscar.MAIL_AUTH = item.MAIL_AUTH;
                        objetoBuscar.MAIL_SSL = item.MAIL_SSL;
                        objetoBuscar.HORA_SUP = item.HORA_SUP;
                        objetoBuscar.HORA_EMP = item.HORA_EMP;
                        objetoBuscar.SUPERVISOR_ACUM = item.SUPERVISOR_ACUM;
                        objetoBuscar.MAIL_TLS = item.MAIL_TLS;
                        objetoBuscar.HORA_CALC = item.HORA_CALC;
                        objetoBuscar.IN_MARCAS = item.IN_MARCAS;

                        _context.PH_COMPANIAS.Update(objetoBuscar);
                    }
                    else
                    {
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la compañía. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la compañía. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

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
                marcaMovTurno = await _context.Marcas_Mov_Turnos.FirstOrDefaultAsync(e => e.idnumero == idnumero && e.fecha == fechaMov && e.turno == idturno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaMovTurno;
        }

        /// <summary>
        /// GetMarcaMovTurno: Método para obtener una lista de Marcas Mov Turnos por empleado 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="idnumero">Número de Empleado</param>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurno(string idnumero, string fechaPeriodo)
        {

            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                var periodos = await (from a in _context.Ph_Periodos
                                      join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla
                                      join c in _context.Empleados on b.idplanilla equals c.IdPlanilla
                                      where c.IdNumero == idnumero
                                        && fechaMov >= a.inicio && fechaMov <= a.fin
                                      select a).FirstOrDefaultAsync();
                if (periodos != null)
                {
                    marcaMovTurno = await _context.Marcas_Mov_Turnos
                                            .Where(e => e.idnumero == idnumero
                                                    && e.fecha >= periodos.inicio
                                                    && e.fecha <= periodos.fin).ToListAsync();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaMovTurno;
        }

        /// <summary>
        /// GetMarcaMovTurno: Método para obtener una lista de Marcas Mov Turnos de los empleados asignados a un supervisor 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        /// <param name="idgrupo">grupo de empleado</param>
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurnoByGrupo(string fechaPeriodo, string idgrupo)
        {

            List<cMarcaMovTurno> marcaMovTurno = new();
            try
            {
                var grupos = idgrupo.Split(",");

                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                var periodos = await (from a in _context.Ph_Periodos
                                      join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla
                                      join c in _context.Empleados on b.idplanilla equals c.IdPlanilla
                                      where grupos.Contains(c.IdGrupo.ToString())
                                        && fechaMov >= a.inicio && fechaMov <= a.fin
                                      select a).Distinct().ToListAsync();
                foreach (var periodo in periodos)
                {
                    var marcasperiodo = await (from m in _context.Marcas_Mov_Turnos
                                               join c in _context.Empleados on new { idnumero = m.idnumero, idplanilla = m.idplanilla } equals new { idnumero = c.IdNumero, idplanilla = c.IdPlanilla }
                                               where grupos.Contains(c.IdGrupo.ToString())
                                                 && m.fecha >= periodo.inicio
                                                 && m.fecha <= periodo.fin
                                               select m).ToListAsync();

                    marcaMovTurno.AddRange(marcasperiodo);
                }


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
                        marcaMovTurno.hora = "00:00";
                        _context.Marcas_Mov_Turnos.Update(marcaMovTurno);
                    }
                    else
                    {
                        item.hora = "00:00";
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

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        //Sincronizar Marcas_Resumen
        //Parametro: Recibe una instancia de Marcas_Resumen, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_MarcasResumen(IEnumerable<cMarcaResumen> marcasResumen)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in marcasResumen)
                {
                    var concepto = await _context.Ph_Conceptos.FirstOrDefaultAsync(e => e.id == item.IdConcepto);
                    var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == item.IdNumero);


                    cMarcaResumen? marcaRes = await _context.Marcas_Resumen
                                    .Where(e => e.IdPlanilla == empleado!.IdPlanilla && e.IdNumero == item.IdNumero
                                            && e.IdConcepto == item.IdConcepto && e.IdCCosto == empleado.IdCCosto)
                                    .FirstOrDefaultAsync();
                    if (marcaRes is not null)
                    {

                        marcaRes.NominaEq = item.NominaEq;
                        //marcaRes.Cantidad = item.Cantidad;
                        marcaRes.Monto = marcaRes.Monto + item.Monto;
                        marcaRes.Proyecto = item.Proyecto;
                        marcaRes.Fase = item.Fase;
                        marcaRes.IdPeriodo = item.IdPeriodo;
                        marcaRes.NominaEq = concepto!.nominaeq;

                        _context.Marcas_Resumen.Update(marcaRes);
                    }
                    else
                    {
                        item.IdPlanilla = empleado.IdPlanilla;
                        item.IdCCosto = empleado.IdCCosto;
                        item.NominaEq = concepto!.nominaeq;
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

        //Fecha: 2022-10-30
        //Obtener lista de Conceptos
        public async Task<List<cPh_Grupo>> GetGrupo()
        {
            List<cPh_Grupo> grupo = new();
            try
            {
                grupo = await _context.Ph_Grupos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return grupo;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        //Obtener un Grupo especifico
        //Parametros: concepto=concepto a buscar
        public async Task<cPh_Grupo> GetGrupo(int idgrupo)
        {
            cPh_Grupo? grupos = new();
            try
            {
                grupos = await _context.Ph_Grupos.FirstOrDefaultAsync(e => e.idgrupo == idgrupo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return grupos;
        }

        /// <summary>
        /// Sincronizar_Grupo: metodo para sincronizar los Grupos 
        /// </summary>
        /// <param name="PhGrupos"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_Grupo(IEnumerable<cPh_Grupo> phGrupos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in phGrupos)
                {
                    cPh_Grupo? objetoBuscar = await _context.Ph_Grupos
                                    .FirstOrDefaultAsync(e => e.idgrupo == item.idgrupo);
                    //si el centro de costo existe se actualiza descripción
                    //de lo contrario se agrega el registro*
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.idgrupo = item.idgrupo;
                        objetoBuscar.descripcion = item.descripcion;
                        objetoBuscar.idcomp = item.idcomp;
                        objetoBuscar.idplanilla = item.idplanilla;
                        objetoBuscar.estado = item.estado;
                        objetoBuscar.idagrupamiento = item.idagrupamiento;
                        objetoBuscar.turno_continuo = item.turno_continuo;

                        _context.Ph_Grupos.Update(objetoBuscar);
                    }
                    else
                    {
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Grupo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Grupo. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Grupo:  Metodo borrado de datos de la tabla Ph_Grupos
        /// </summary>
        /// <param name="idgrupo"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Grupo(int idgrupo)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPh_Grupo? model = await _context.Ph_Grupos
                    .FirstOrDefaultAsync(e => e.idgrupo == idgrupo);

                if (model is not null)
                {
                    _context.Ph_Grupos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Grupo. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Grupo. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-07
        /// <summary>
        /// getPeriodo: Método para obtener una lista de Periodos 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        public async Task<IEnumerable<cPh_Periodos>> GetPeriodo()
        {
            List<cPh_Periodos>? periodos = new();
            try
            {
                periodos = await _context.Ph_Periodos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return periodos;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-07
        /// <summary>
        /// getPeriodo: Método para un periodo específico
        /// </summary>
        /// <returns>Una instancia de la clase cPh_Periodos</returns>
        /// ///<param name="idperiodo">idperiodo del periodo requerido</param>
        public async Task<cPh_Periodos> GetPeriodo(string idperiodo)
        {
            cPh_Periodos? periodo = new();
            try
            {
                periodo = await _context.Ph_Periodos.FirstOrDefaultAsync(e => e.idperiodo == idperiodo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return periodo;
        }

        /// <summary>
        /// GetPeriodoVigenteEmpleado: Método para obtener el periodo vigenta para un empleado  
        /// </summary>
        /// <returns>Un item de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="idnumero">Número de empleado</param>
        public async Task<cPh_Periodos> GetPeriodoVigenteEmpleado(string idnumero, string fechaPeriodo)
        {
            cPh_Periodos? periodo = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fechaPeriodo.Substring(0, 4)}-{fechaPeriodo.Substring(4, 2)}-{fechaPeriodo.Substring(6, 2)}");
                periodo = await (from a in _context.Ph_Periodos
                                 join b in _context.Ph_Planilla on a.tipo_planilla equals b.tipo_planilla
                                 join c in _context.Empleados on b.idplanilla equals c.IdPlanilla
                                 where c.IdNumero == idnumero
                                   && fechaMov >= a.inicio && fechaMov <= a.fin
                                 select a).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return periodo;


        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-07
        /// <summary>
        /// getPeriodo: Método para obtener una lista de Periodos 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="vigente">Periodo está vigente </param>
        public async Task<IEnumerable<cPh_Periodos>> GetPeriodo(string fecha, string vigente)
        {

            List<cPh_Periodos>? periodos = new();
            try
            {
                DateTime fechaMov = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                periodos = await _context.Ph_Periodos.Where(e => fechaMov >= e.inicio && fechaMov <= e.fin)
                                                     .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return periodos;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-27
        //Obtener lista de tipos de planilla 
        public async Task<IEnumerable<cPh_Planilla>> GetPhPlanilla()
        {
            List<cPh_Planilla>? planilla = new();
            try
            {
                planilla = await _context.Ph_Planilla.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return planilla;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-27
        //Obtener un tipo de planilla especifico
        public async Task<cPh_Planilla> GetPhPlanilla(string idplanilla)
        {
            cPh_Planilla? planilla = new();
            try
            {
                planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.idplanilla == idplanilla);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return planilla;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-06-27
        //Obtener un tipo de planilla especifico por nom conector
        public async Task<cPh_Planilla> GetPhPlanilla(string nomConector, string descPlanilla)
        {
            cPh_Planilla? planilla = new();
            try
            {
                planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.nom_conector.ToLower() == nomConector.ToLower());

                if (planilla == null)
                {
                    planilla = await _context.Ph_Planilla.FirstOrDefaultAsync(e => e.planilla.ToLower() == descPlanilla.ToLower());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return planilla;
        }

        //Creado por: Allan Prieto Badilla
        //Fecha: 2023-10-12
        //Sincronizar Planilla 
        //Parametro: Recibe una instancia de Planilla, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_PhPlanilla(IEnumerable<cPh_Planilla> PhPlanillas)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in PhPlanillas)
                {
                    cPh_Planilla? pla = await _context.Ph_Planilla
                                                        .FirstOrDefaultAsync(e => e.idplanilla == item.idplanilla);
                    
                    if (pla is not null)
                    {
                        pla.planilla = item.planilla;
                        pla.nom_conector = item.nom_conector;
                        pla.tipo_planilla = item.tipo_planilla;
                        pla.c_ext = item.c_ext;
                        pla.c_inci = item.c_inci;
                        pla.c_adic = item.c_adic;
                        pla.m_desc = item.m_desc;
                        pla.proyecta = item.proyecta;
                        pla.dia_inicio = item.dia_inicio;
                        pla.auto_proceso = item.auto_proceso;
                        pla.tipo_dist = item.tipo_dist;
                        pla.est_nomina = item.est_nomina;
                        pla.ext_per_ant = item.ext_per_ant;
                        pla.ext_det = item.ext_det;
                        pla.agrup_salida = item.agrup_salida;
                        pla.tipo_adic = item.tipo_adic;
                        pla.nivel_aprob_ext = item.nivel_aprob_ext;
                        _context.Ph_Planilla.Update(pla);
                    }
                    else
                    {
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de la planilla. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_PhPlanilla:  Metodo borrado de datos de la tabla Ph_Planilla
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhPlanilla(string idplanilla)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPh_Planilla? model = await _context.Ph_Planilla
                    .FirstOrDefaultAsync(e => e.idplanilla == idplanilla);

                if (model is not null)
                {
                    _context.Ph_Planilla.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la planilla. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la planill. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        public async Task EjecutaPostCambioPlanilla(string idnumero, string oldPlanilla, string newPlanilla)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + $".DM_POST_CAMBIOPLANILLA @idnumero='{idnumero}', @OLDPLANILLA='{oldPlanilla}',@NEWPLANILLA='{newPlanilla}'";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaIn: Método para obtener todos los datos de la tabla Marcas_In 
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaIn</returns>
        public async Task<List<cMarcaIn>> GetMarcaIn()
        {
            List<cMarcaIn> marcaIn = new();
            try
            {
                marcaIn = await _context.Marcas_In.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaIn;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaIn: Método para obtener todos los datos de la tabla Marcas_In de un colaborador
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaIn para un colaborador en específico</returns>
        /// <param name="idtarjeta">Id de Tarjeta del colaborador</param>
        public async Task<List<cMarcaIn>> GetMarcaIn(string idtarjeta)
        {
            List<cMarcaIn> marcaIn = new();
            try
            {
                marcaIn = await _context.Marcas_In.Where(e => e.idtarjeta == idtarjeta).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaIn;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// Sincronizar_MarcaIn: Método para registrar las marcas de ingreso, salida y descanso de los colaboradores en la tabla Marcas_In 
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasIn">Lista de registroS de la clase cMarcasIn</param>
        public async Task<EventResponse> Sincronizar_MarcaIn(IEnumerable<cMarcaIn> marcasIn)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var marcaIn in marcasIn)
                {
                    _context.Add(marcaIn);
                    await _context.SaveChangesAsync();

                    await EjecutaInMarcasWeb(marcaIn.idnumero);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la Marca. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la Marca. Detalle de Error: " + e.InnerException.Message;
            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaExtraApb: Método para obtener todos los datos de la tabla Marcas_Extras_Apb 
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaExtraApb</returns>
        public async Task<List<cMarcaExtraApb>> GetMarcaExtraApb()
        {
            List<cMarcaExtraApb> marcaExtraApb = new();
            try
            {
                marcaExtraApb = await _context.Marcas_Extras_Apb.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaExtraApb;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaExtraApb: Método para obtener todos los datos de la tabla Marcas_Extras_Apb para un empleado y un periodo especifico
        /// </summary>
        /// <param name="idnumero">id del empleado</param>
        /// <param name="fecha">fecha para determinar el periodo vigente</param>
        /// <param name="idplanilla">id planilla</param>
        /// <param name="PeriodoVigente">Si es verdadero trae marcas extras del periodo, sino, trae marcas del dia</param>
        /// <returns>Lista de registros de la clase cMarcaExtraApb</returns>
        public async Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string idnumero, string fecha, string idplanilla, bool byPeriodo)
        {
            List<cMarcaExtraApb> marcaExtraApb = new();
            try
            {

                if (byPeriodo)
                {
                    var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                    marcaExtraApb = await _context.Marcas_Extras_Apb
                                          .Where(e => e.idnumero == idnumero && e.idplanilla == idplanilla
                                                 && e.fecha >= periodoVigente.inicio)
                                          .ToListAsync();
                }
                else
                {
                    DateTime fechaDia = DateTime.Parse($"{fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                    marcaExtraApb = await _context.Marcas_Extras_Apb
                                          .Where(e => e.idnumero == idnumero && e.idplanilla == idplanilla
                                                 && e.fecha == fechaDia)
                                          .ToListAsync();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaExtraApb;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// GetMarcaExtraApb: Método para obtener un registro de la tabla Marcas_Extras_Apb con un identificador específico
        /// </summary>
        /// <returns>Registro de la clase Marcas_Extras_Apb</returns>
        /// <param name="idregistro">Número identificador del registro</param>
        public async Task<cMarcaExtraApb> GetMarcaExtraApb(long idregistro)
        {
            cMarcaExtraApb marcaExtraApb = new();
            try
            {
                marcaExtraApb = await _context.Marcas_Extras_Apb.FirstOrDefaultAsync(e => e.idregistro == idregistro);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaExtraApb;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// Sincronizar_MarcaExtraApb: Método para registrar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public async Task<EventResponse> Sincronizar_MarcaExtraApb(IEnumerable<cMarcaExtraApb> marcasExtraApb)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var marcaExtraApb in marcasExtraApb)
                {
                    cMarcaExtraApb? marcaExtra = await _context.Marcas_Extras_Apb
                                    .Where(e => e.idregistro == marcaExtraApb.idregistro)
                                    .FirstOrDefaultAsync();
                    //si la marca de hora extra existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (marcaExtra is not null)
                    {
                        marcaExtra.cantidad = marcaExtraApb.cantidad;
                        marcaExtra.hora = marcaExtraApb.hora;
                        marcaExtra.comentario = marcaExtraApb.comentario;
                        marcaExtra.ccosto = marcaExtraApb.ccosto;

                        _context.Marcas_Extras_Apb.Update(marcaExtra);

                        var marcaProceso = await _context.Marcas_Proceso.FirstOrDefaultAsync(e => e.fecha_entra >= marcaExtra.fecha && e.fecha_sale <= marcaExtra.fecha && e.idnumero == marcaExtra.idnumero && e.idplanilla == marcaExtra.idplanilla);

                        if (marcaProceso is not null)
                        {
                            marcaProceso.EXTT = marcaExtra.cantidad;
                        }
                    }
                    else
                    {
                        marcaExtraApb.idregistro = 0;
                        _context.Add(marcaExtraApb);

                        var marcaProceso = await _context.Marcas_Proceso.FirstOrDefaultAsync(e => e.fecha_entra >= marcaExtraApb.fecha && e.fecha_sale <= marcaExtraApb.fecha && e.idnumero == marcaExtraApb.idnumero && e.idplanilla == marcaExtraApb.idplanilla);

                        if (marcaProceso is not null)
                        {
                            marcaProceso.EXTT = marcaExtraApb.cantidad;
                        }
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la Hora Extra. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la Hora Extra. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// EjecutaInMarcasWeb: Método que ejecuta procedimiento almacenado IN_MARCAS_WEB, necesario para completar el registro de marca en Geotime
        /// </summary>
        /// <param name="idnumero">idnumero del empleado que realiza la marca</param>
        public async Task EjecutaInMarcasWeb(string idnumero)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _schema + $".IN_MARCAS_WEB @idnumero='{idnumero}'";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetProyecto: Obtener lista de Proyectos
        /// </summary>
        /// <returns>Lista de objetos del tipo cPh_Proyecto</returns>

        public async Task<List<cPh_Proyecto>> GetProyecto()
        {
            List<cPh_Proyecto> ph_Proyecto = new();
            try
            {
                ph_Proyecto = await _context.Ph_Proyecto.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return ph_Proyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetFaseProyecto: Obtener un Proyecto especifica
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_Proyecto</returns>
        public async Task<cPh_Proyecto> GetProyecto(string idproyecto)
        {
            cPh_Proyecto? ph_Proyecto = new();
            try
            {
                ph_Proyecto = await _context.Ph_Proyecto.FirstOrDefaultAsync(e => e.PROYECTO == idproyecto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return ph_Proyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// Sincronizar_Proyectos: Sincronizar proyectos, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="phProyectos">Recibe una instancia del tipo cProyecto</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>.
        public async Task<EventResponse> Sincronizar_Proyectos(IEnumerable<cPh_Proyecto> phProyectos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var proyecto in phProyectos)
                {
                    cPh_Proyecto? proyectoBuscar = await _context.Ph_Proyecto
                                                        .Where(e => e.PROYECTO == proyecto.PROYECTO)
                                                        .FirstOrDefaultAsync();
                    //si el proyecto existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (proyectoBuscar is not null)
                    {
                        proyectoBuscar.DESCRIPCION = proyecto.DESCRIPCION;
                        proyectoBuscar.CENTRO_COSTO = proyecto.CENTRO_COSTO;

                        _context.Ph_Proyecto.Update(proyectoBuscar);
                    }
                    else
                    {
                        _context.Add(proyecto);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Proyectos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Proyectos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetFaseProyecto: Obtener lista de Fases de Proyectos
        /// </summary>
        /// <returns>Una lista de objetos del tipo cPh_FaseProyecto</returns>
        public async Task<List<cPh_FaseProyecto>> GetFaseProyecto()
        {
            List<cPh_FaseProyecto> phFaseProyecto = new();
            try
            {
                phFaseProyecto = await _context.Ph_FaseProyecto.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return phFaseProyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// GetFaseProyecto: Obtener una Fase de Proyecto especifica
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <param name="fase">fase del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_FaseProyecto</returns>
        public async Task<cPh_FaseProyecto> GetFaseProyecto(string idproyecto, string fase)
        {
            cPh_FaseProyecto? phFaseProyecto = new();
            try
            {
                phFaseProyecto = await _context.Ph_FaseProyecto.FirstOrDefaultAsync(e => e.PROYECTO == idproyecto && e.FASE == fase);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return phFaseProyecto;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// Sincronizar_FaseProyectos: Sincronizar fases de proyectos, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="phFaseProyectos">Recibe una instancia del tipo cFaseProyecto</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>
        public async Task<EventResponse> Sincronizar_FaseProyectos(IEnumerable<cPh_FaseProyecto> phFaseProyectos)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var proyectoFase in phFaseProyectos)
                {
                    cPh_FaseProyecto? proyectoFaseBuscar = await _context.Ph_FaseProyecto
                                                        .Where(e => e.PROYECTO == proyectoFase.PROYECTO && e.FASE == proyectoFase.FASE)
                                                        .FirstOrDefaultAsync();
                    //si el proyectoFase existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (proyectoFaseBuscar is not null)
                    {
                        proyectoFaseBuscar.NOMBRE = proyectoFase.NOMBRE;
                        proyectoFaseBuscar.DESCRIPCION = proyectoFase.DESCRIPCION;
                        proyectoFaseBuscar.ACEPTA_DATOS = proyectoFase.ACEPTA_DATOS;

                        _context.Ph_FaseProyecto.Update(proyectoFaseBuscar);
                    }
                    else
                    {
                        _context.Add(proyectoFase);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las Fases de Proyectos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las Fases de Proyectos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetMarcas: Obtener las marcas proceso para el periodo 
        /// </summary>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarcaProceso>> GetMarcasProceso(string fecha)
        {
            List<cMarcaProceso>? marca = new();
            try
            {
                var periodos = await GetPeriodo(fecha, "T");

                Task task = new Task(() =>
                {
                    marca = (from p in periodos
                             join pl in _context.Ph_Planilla on p.tipo_planilla equals pl.tipo_planilla
                             join m in _context.Marcas_Proceso on pl.idplanilla equals m.idplanilla
                             where m.fecha_entra >= p.inicio && m.fecha_entra <= p.fin
                             select m).ToList();
                });
                task.Start();
                task.Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marca;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetMarcas: Obtener las marcas proceso de un empleado para el periodo 
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public async Task<List<cMarcaProceso>> GetMarcasProceso(string idnumero, string fecha)
        {
            List<cMarcaProceso>? marca = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marca = await _context.Marcas_Proceso.Where(e => e.idnumero == idnumero && e.fecha_entra >= periodoVigente.inicio && e.fecha_entra <= periodoVigente.fin).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marca;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-25
        /// <summary>
        /// GetMarcasAudit: Obtener las marcas_audit para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Audit</returns>

        public async Task<List<cMarcaAudit>> GetMarcasAudit(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaAudit>? marca = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marca = await _context.Marcas_Audit.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= periodoVigente.inicio
                                                        && e.FECHA <= periodoVigente.fin
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marca;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-25
        /// <summary>
        /// GetMarcasAudit: Obtener las Marcas Descansos para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Descansos</returns>

        public async Task<List<cMarcaDescanso>> GetMarcasDescansos(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaDescanso>? marcaDescanso = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marcaDescanso = await _context.Marcas_Descansos.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= periodoVigente.inicio
                                                        && e.FECHA <= periodoVigente.fin
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaDescanso;
        }


        //Creado por: Marlon Loria Solano
        //Fecha: 2022-01-02
        /// <summary>
        /// /Obtener lista de Compañias asociadas al usuario
        /// </summary>
        /// <returns>Lista de Compania de Usuario </returns>

        public async Task<List<cPh_CompaniaUsuario>> GetPhCompaniaUsuario(string idnumero)
        {
            List<cPh_CompaniaUsuario> companiasUsuario = new();
            DataTable table;

            try
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                string schemaAdmin = config.GetConnectionString("SchemaAdmin");

                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"{schemaAdmin}.VerificaCompaniaUsuarioWeb @IdNumero='{idnumero}'";
                        System.Data.Common.DbDataReader result = command.ExecuteReader();

                        table = new DataTable();
                        table.Load(result);

                        foreach (DataRow dr in table.Rows)
                        {
                            cPh_CompaniaUsuario usrComp = new cPh_CompaniaUsuario
                            {
                                idcomp = dr.ItemArray[0].ToString(),
                                compania = dr.ItemArray[1].ToString(),
                                nom_conector = dr.ItemArray[2].ToString(),
                                idnumero = dr.ItemArray[3].ToString(),
                            };
                            companiasUsuario.Add(usrComp);
                        }

                        // Close the reader
                        result.Close();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return companiasUsuario;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-09-01
        /// <summary>
        /// GetMarcaIncidencia: Obtener las Marcas Incidencias para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Incidencias</returns>

        public async Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string fecha, string idplanilla)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {
                var periodoVigente = await GetPeriodoVigenteEmpleado(idnumero, fecha);

                marcaIncidencia = await _context.Marcas_Incidencias.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= periodoVigente.inicio
                                                        && e.FECHA <= periodoVigente.fin
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaIncidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-09-01
        /// <summary>
        /// GetMarcaIncidencia: Obtener una Marca Incidencia especifica segun el id indicado en el parámetro
        /// </summary>
        /// <param name="id">numero de marca incidencia</param>
        /// <returns>Una instancia de Marcas Incidencias</returns>

        public async Task<cMarcaIncidencia> GetMarcaIncidencia(long id)
        {
            cMarcaIncidencia? marcaIncidencia = new();
            try
            {
                marcaIncidencia = await _context.Marcas_Incidencias.FirstOrDefaultAsync(e => e.INDICE == id);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaIncidencia;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-09-01
        /// <summary>
        /// GetMarcaIncidencia: Obtener las Marcas Incidencias para un empleado, planilla y para un rango de fechas especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <param name="fechaInicio">Fecha de Inicio</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns>Lista de Marcas Incidencias</returns>

        public async Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string idplanilla, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<cMarcaIncidencia>? marcaIncidencia = new();
            try
            {
                marcaIncidencia = await _context.Marcas_Incidencias.Where(e => e.IDNUMERO == idnumero
                                                        && e.FECHA >= fechaInicio
                                                        && e.FECHA <= fechaFinal
                                                        && e.FECHA_JUST == null
                                                        && e.IDPLANILLA == idplanilla).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcaIncidencia;
        }

        /// <summary>
        /// GetMarcaDistribucion: Lista de Marcas Distribución segun parametros de fecha indicados
        /// </summary>
        /// <param name="FechaInicio">Fecha de Inicio del reporte</param>
        /// <param name="FechaFin">Fecha final del reporte</param>
        /// <returns>lista de registros de la clase cMarcaDistribucion</returns>
        public async Task<List<cMarcaDistribucion>> GetMarcaDistribucion(DateTime FechaInicio, DateTime FechaFin)
        {
            List<cMarcaDistribucion> marcasDistribuciones = new();
            try
            {
                marcasDistribuciones = await (from md in _context.Marcas_Distribuciones
                                              .Where(e => FechaInicio <= e.FECHA && FechaFin >= e.FECHA)
                                              select new cMarcaDistribucion
                                              {
                                                  IDPLANILLA = md.IDPLANILLA,
                                                  IDNUMERO = md.IDNUMERO,
                                                  FECHA = md.FECHA,
                                                  NOMINAEQ = md.NOMINAEQ,
                                                  CANTIDAD = md.CANTIDAD,
                                                  IDCCOSTO = md.IDCCOSTO,
                                              }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return marcasDistribuciones;
        }



        /// <summary>
        /// GetPhUsuario: Obtener datos de usuario 
        /// </summary>
        /// <param name="idnumero">id numero del empleado</param>
        /// <returns>Instancia de phusuario con los datos del usuario </returns>
        public async Task<cPh_Usuario> GetPhUsuario(string idnumero)
        {
            cPh_Usuario? phUsuario = new();

            try
            {
                phUsuario = await (from e in _context.Empleados.Where(e => e.IdNumero == idnumero)
                                   join l in _context.PH_LOGIN on e.Email equals l.EMAIL
                                   join u in _context.Ph_Usuarios on l.idusuario equals u.IDUSUARIO
                                   select u).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return phUsuario;
        }

        /// <summary>
        /// GetPhSistema: Obtener datos de Sistema 
        /// </summary>
        /// <returns>Instancia de cPh_Sistema con los datos del sistema </returns>
        public async Task<cPh_Sistema> GetPhSistema()
        {
            cPh_Sistema? phSistema = new();

            try
            {
                phSistema = await _context.Ph_Sistema.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return phSistema;
        }

        /// <summary>
        /// GetPortalConfig: Obtener datos de Configuración del Portal 
        /// </summary>
        /// <returns>Instancia de cPortal_Config con los datos del sistema </returns>
        public async Task<cPortal_Config> GetPortalConfig()
        {
            cPortal_Config? portalConfig = new();

            try
            {
                portalConfig = (from pc in await _context.Portal_Config.ToListAsync()
                                select new cPortal_Config
                                {
                                    ID = pc.ID,
                                    DATA_01 = Encripta.getDecryptTripleDES(pc.DATA_01),
                                    LIC_PORTAL = pc.LIC_PORTAL
                                }).FirstOrDefault();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return portalConfig;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-23
        /// <summary>
        /// Sincronizar_PortalConfig: Sincronizar las configuraciones del Portal de Empleados, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="portalConfig">Recibe una instancia del tipo cPortal_Config</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>
        public async Task<EventResponse> Sincronizar_PortalConfig(cPortal_Config portalConfig)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPortal_Config? portConfBuscar = await _context.Portal_Config.FirstOrDefaultAsync(e => e.ID == portalConfig.ID);
                string dataEncrypt = Encripta.getEncryptTripleDES(portalConfig.DATA_01);

                //si el proyectoFase existe se actualiza 
                //de lo contrario se agrega el registro
                if (portConfBuscar is not null)
                {
                    portConfBuscar.LIC_PORTAL = portalConfig.LIC_PORTAL;
                    portConfBuscar.DATA_01 = dataEncrypt;

                    _context.Portal_Config.Update(portConfBuscar);
                }
                else
                {
                    portalConfig.ID = Guid.NewGuid();
                    _context.Add(portalConfig);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las configuraciones del Portal de Empleados. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de las configuraciones del Portal de Empleados. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// GetPortalOpcion: Obtener lista de opciones del menu de Portal 
        /// </summary>
        /// <returns>Lista de Opciones del sistema</returns>
        public async Task<List<cPortal_Opcion>> GetPortalOpcion()
        {
            List<cPortal_Opcion>? portalOpcion = new();

            try
            {
                portalOpcion = await _context.Portal_Opciones.ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return portalOpcion;
        }

        /// <summary>
        /// GetPortalOpcion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public async Task<cPortal_Opcion> GetPortalOpcion(string id)
        {
            cPortal_Opcion? portalOpcion = new();

            try
            {
                portalOpcion = await _context.Portal_Opciones.FirstOrDefaultAsync(e => e.ID == id);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return portalOpcion;
        }


        /// <summary>
        /// Sincronizar_PortalOpcion: Método para registrar las opciones del sistema Portal de empleados
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Opcion </param>
        public async Task<EventResponse> Sincronizar_PortalOpcion(IEnumerable<cPortal_Opcion> portalOpcion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in portalOpcion)
                {
                    cPortal_Opcion? portalOpcionBuscar = await _context.Portal_Opciones
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (portalOpcionBuscar is not null)
                    {
                        portalOpcionBuscar.HREF = item.HREF;
                        portalOpcionBuscar.ICONID = item.ICONID;
                        portalOpcionBuscar.PRINCIPAL = item.PRINCIPAL;
                        portalOpcionBuscar.MENUTEXT = item.MENUTEXT;
                        portalOpcionBuscar.PARENTID = item.PARENTID;

                        _context.Portal_Opciones.Update(portalOpcionBuscar);
                    }
                    else
                    {
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro de la opción de menú. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro de la opción de menú. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-10-19
        /// <summary>
        /// GetPhFormulacion: Obtener lista de registros de la tabla PH_FROMULACION
        /// </summary>
        /// <returns>Lista de cPh_Formulacion </returns>
        /// 

        public async Task<List<cPh_Formulacion>> GetPhFormulacion()
        {
            List<cPh_Formulacion>? phFormulacion = new();

            try
            {
                phFormulacion = await _context.Ph_Formulacion.ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return phFormulacion;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-10-19
        /// <summary>
        /// GetPhFormulacion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public async Task<cPh_Formulacion> GetPhFormulacion(int id)
        {
            cPh_Formulacion? phFormulacion = new();

            try
            {
                phFormulacion = await _context.Ph_Formulacion.FirstOrDefaultAsync(e => e.ID == id);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return phFormulacion;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-10-19
        /// <summary>
        /// Sincronizar_PhFormulacion: Método para registrar los registros en la tabla Ph_Formulacion
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="cPh_Formulacion">Lista de registros de la clase cPh_Formulacion</param>
        public async Task<EventResponse> Sincronizar_PhFormulacion(IEnumerable<cPh_Formulacion> ph_Formulacion)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in ph_Formulacion)
                {
                    cPh_Formulacion? objetoBuscar = await _context.Ph_Formulacion
                                    .Where(e => e.ID == item.ID)
                                    .FirstOrDefaultAsync();
                    //si la opcion existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;
                        objetoBuscar.FORMULA = item.FORMULA;

                        _context.Ph_Formulacion.Update(objetoBuscar);
                    }
                    else
                    {
                        item.ID = 0;
                        _context.Add(item);
                    }
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla PH_FORMULACION. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el registro en la tabla PH_FORMULACION. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;
        }

        /// <summary>
        /// Elimina_PhFormulacion:  Metodo boorado de datos de la tabla Ph_Formulacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhFormulacion(string id)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cPh_Formulacion? model = await _context.Ph_Formulacion
                    .FirstOrDefaultAsync(e => e.ID == int.Parse(id));

                if (model is not null)
                {
                    _context.Ph_Formulacion.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar la fórmula. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar la fórmula. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }
        //Creado por: Marlon Loria Solano
        //Fecha: 2022-11-10
        //Obtener Parametros Email por Id
        public async Task<cParametroEmail> GetParametroEmail(int id)
        {
            cParametroEmail parametroEmail = new();
            try
            {
                parametroEmail = await _context.ParametrosEmail.FirstOrDefaultAsync(e => e.Id == id);
                parametroEmail.DefaultPassWord = Encripta.getDecryptTripleDES(parametroEmail.DefaultPassWord);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return parametroEmail;
        }

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-02-13
        //Sincronizar ParametroEmail
        //Parametro: Recibe una instancia de ParametroEmail, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_ParametroEmail(cParametroEmail parametroEmail)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cParametroEmail? parametroBuscado = await _context.ParametrosEmail
                                .Where(e => e.Id == parametroEmail.Id)
                                .FirstOrDefaultAsync();
                //si el parametro existe se actualiza 
                //de lo contrario se agrega el registro
                if (parametroBuscado is not null)
                {
                    parametroBuscado.SmtpServer = parametroEmail.SmtpServer;
                    parametroBuscado.SmtpPort = parametroEmail.SmtpPort;
                    parametroBuscado.DefaultEmail = parametroEmail.DefaultEmail;
                    parametroBuscado.DefaultPassWord = Encripta.getEncryptTripleDES(parametroEmail.DefaultPassWord);

                    _context.ParametrosEmail.Update(parametroBuscado);
                }
                else
                {
                    _context.Add(parametroEmail);
                }


                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Parámetro Email. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Parámetro Email. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        public async Task<EventResponse> EnviarCorreo(Email correo)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                var parametrosCorreo = await GetParametroEmail(1);

                correo.De = parametrosCorreo.DefaultEmail;
                correo.SmtpServer = parametrosCorreo.SmtpServer;
                correo.SmtpPort = parametrosCorreo.SmtpPort;

                string password = parametrosCorreo.DefaultPassWord;

                SecureString secureString = new SecureString();
                foreach (char c in password.ToCharArray())
                {
                    secureString.AppendChar(c);
                }

                MailMessage message = new MailMessage(correo.De, correo.Para, correo.Asunto, correo.Cuerpo);
                message.IsBodyHtml = true;

                if (correo.Adjunto != "")
                {
                    Stream stream = new MemoryStream(correo.StreamAdjunto);
                    Attachment data = new Attachment(stream, correo.Adjunto, MediaTypeNames.Application.Octet);
                    message.Attachments.Add(data);
                }

                SmtpClient client = new SmtpClient(correo.SmtpServer, correo.SmtpPort);

                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                var Credentials = new NetworkCredential(correo.De, secureString);
                client.Credentials = Credentials;
                client.Send(message);


            }
            catch (System.Net.WebException e)
            {
                Console.WriteLine(e.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + e.InnerException.Message;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (ex.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + ex.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar el envio del correo electrónico. Detalle de Error: " + ex.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener lista de Horarios
        public async Task<List<cPh_Horarios>> GetHorarios()
        {
            List<cPh_Horarios> horarios = new();
            try
            {
                horarios = (from e in await _context.Ph_Horarios
                                .Include(e=>e.Ph_HorarioTurno)
                            .ToListAsync()
                            select new cPh_Horarios
                            {
                                IDHORARIO = e.IDHORARIO,
                                DESCRIPCION = e.DESCRIPCION,
                                Ph_HorarioTurno = e.Ph_HorarioTurno==null?null:
                                                ( from ht in e.Ph_HorarioTurno
                                                  select new cPh_HorarioTurno
                                                  {
                                                      IDHORARIO = ht.IDHORARIO,
                                                      ID_DIA = ht.ID_DIA,
                                                      T_1 = ht.T_1,
                                                      T_2 = ht.T_2,
                                                      T_3 = ht.T_3,
                                                      T_4 = ht.T_4,
                                                      T_5 = ht.T_5,
                                                  }).ToList()}
                            ).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return horarios;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener un Horario especifico
        //Parametros: idHorario=Id de horarios a buscar
        public async Task<cPh_Horarios> GetHorarios(int IDHORARIO)
        {
            cPh_Horarios? horarios = new();
            try
            {
                horarios = await _context.Ph_Horarios.FirstOrDefaultAsync(e => e.IDHORARIO == IDHORARIO);

                horarios = (from e in await _context.Ph_Horarios
                                .Include(e => e.Ph_HorarioTurno)
                                .Where(e => e.IDHORARIO == IDHORARIO)
                                .ToListAsync()
                            select new cPh_Horarios
                            {
                                IDHORARIO = e.IDHORARIO,
                                DESCRIPCION = e.DESCRIPCION,
                                Ph_HorarioTurno = e.Ph_HorarioTurno == null ? null :
                                                (from ht in e.Ph_HorarioTurno
                                                 select new cPh_HorarioTurno
                                                 {
                                                     IDHORARIO = ht.IDHORARIO,
                                                     ID_DIA = ht.ID_DIA,
                                                     T_1 = ht.T_1,
                                                     T_2 = ht.T_2,
                                                     T_3 = ht.T_3,
                                                     T_4 = ht.T_4,
                                                     T_5 = ht.T_5,
                                                 }).ToList()
                            }
                            ).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return horarios;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Sincronizar Horarios
        //Parametro: Recibe una instancia de horarios, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_Horarios(IEnumerable<cPh_Horarios> horarios)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                IEnumerable<cPh_HorarioTurno> phHorarioTurnoList = null;

                await _context.Database.BeginTransactionAsync();

                foreach (var horario in horarios)
                {
                    phHorarioTurnoList = horario.Ph_HorarioTurno;

                    cPh_Horarios? hora = await _context.Ph_Horarios
                                                        .FirstOrDefaultAsync(e => e.IDHORARIO == horario.IDHORARIO);
                    //si el horario existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (hora is not null)
                    {
                        hora.DESCRIPCION = horario.DESCRIPCION;
                        _context.Ph_Horarios.Update(hora);
                    }
                    else
                    {
                        horario.Ph_HorarioTurno = null;
                        _context.Add(horario);
                    }
                    await _context.SaveChangesAsync();

                    if (phHorarioTurnoList is not null)
                    {
                        var resp=await Sincronizar_HorarioTurno(phHorarioTurnoList);
                        if (resp.Id != "0")
                        {
                            respuesta.Id = "1";
                            respuesta.Respuesta = "Error";
                            respuesta.Descripcion = resp.Descripcion;
                            await _context.Database.RollbackTransactionAsync();
                            return respuesta;
                        }
                    }

                }
                await _context.Database.CommitTransactionAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios. Detalle de Error: " + e.InnerException.Message;

                await _context.Database.RollbackTransactionAsync();
            }
  
            return respuesta;

        }
        /// <summary>
        /// Elimina_PhHorarios:  Metodo boorado de datos de la tabla PhHorarios
        /// </summary>
        /// <param name="IDHORARIO"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Horarios(string IDHORARIO)
        {
            EventResponse respuesta = new EventResponse();
            try
            {

                cPh_Horarios? model = await _context.Ph_Horarios
                    .FirstOrDefaultAsync(e => e.IDHORARIO == int.Parse(IDHORARIO));

                if (model is not null)
                {
                    _context.Ph_Horarios.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Horario. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Horario. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener lista de Horarios
        public async Task<List<cPh_HorarioTurno>> GetHorario_Turno()
        {
            List<cPh_HorarioTurno> horario_turno = new();
            try
            {
                horario_turno = await _context.Ph_Horario_Turnos.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return horario_turno;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Obtener un Horario Turno especifico
        //Parametros: IDHORARIO= Id de horario turno a buscar
        public async Task<cPh_HorarioTurno> GetHorario_Turno(int IDHORARIO)
        {
            cPh_HorarioTurno? horario_turno = new();
            try
            {
                horario_turno = await _context.Ph_Horario_Turnos.FirstOrDefaultAsync(e => e.IDHORARIO == IDHORARIO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return horario_turno;
        }

        //Creado por: María José Sánchez
        //Fecha: 2023-11-08
        //Sincronizar Horario Turno
        //Parametro: Recibe una instancia de HorarioTurno, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_HorarioTurno(IEnumerable<cPh_HorarioTurno> horario_turno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var horario in horario_turno)
                {
                    cPh_HorarioTurno? hora = await _context.Ph_Horario_Turnos
                                                        .FirstOrDefaultAsync(e => e.IDHORARIO == horario.IDHORARIO && e.ID_DIA == horario.ID_DIA);
                    //si el horario existe se actualiza descripción
                    //de lo contrario se agrega el registro
                    if (hora is not null)
                    {
                        hora.IDHORARIO = horario.IDHORARIO;
                        hora.ID_DIA = horario.ID_DIA;
                        hora.T_1 = horario.T_1;
                        hora.T_2 = horario.T_2;
                        hora.T_3 = horario.T_3;
                        hora.T_4 = horario.T_4;
                        hora.T_5 = horario.T_5;


                        _context.Ph_Horario_Turnos.Update(hora);
                    }
                    else
                    {
                        _context.Add(horario);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios Turnos. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios Turnos. Detalle de Error: " + e.InnerException.Message;

            }

            return respuesta;

        }

        /// <summary>
        /// Elimina_Ph_Horario_Turno:  Metodo borrado de datos de la tabla Ph_Horario_Turno
        /// </summary>
        /// <param name="IDHORARIO"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_Horario_Turno(string IDHORARIO)
        {
            EventResponse respuesta = new EventResponse();

            try
            {

                cPh_HorarioTurno? model = await _context.Ph_Horario_Turnos
                    .FirstOrDefaultAsync(e => e.IDHORARIO == int.Parse(IDHORARIO));

                if (model is not null)
                {
                    _context.Ph_Horario_Turnos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Horario Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Horario Turno. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-8
        /// <summary>
        /// GetTipo_Planilla: Obtener lista de registros de la tabla TIPOS_PLANILLA
        /// </summary>
        /// <returns>Lista de cTipo_Planilla </returns>
        /// 
        public async Task<List<cTipo_Planilla>> GetTipo_Planilla()
        {
            List<cTipo_Planilla> planillas = new();
            try
            {
                planillas = await _context.TIPOS_PLANILLA.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return planillas;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-12
        /// <summary>
        /// GetPh_Tranformacion: Obtener lista de registros de la tabla PH_TRANFORMACION
        /// </summary>
        /// <returns>Lista de cPh_Tranformacion </returns>
        /// 
        public async Task<List<cPh_Transformacion>> GetPhTransformacion()
        {
            List<cPh_Transformacion> transformaciones = new();
            try
            {
                transformaciones = await _context.Ph_Transformacion.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return transformaciones;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-13-12
        /// <summary>
        /// GetPhRol: Obtener lista de registros de la tabla PH_ROLES
        /// </summary>
        /// <returns>Lista de cPh_Rol </returns>
        /// 
        public async Task<List<cPh_Rol>> GetPhRol()
        {
            List<cPh_Rol> roles = new();
            try
            {
                roles = await _context.Ph_Roles.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return roles;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        //Obtener un Rol especifico
        //Parametros: idrol=id a buscar
        public async Task<cPh_Rol> GetPhRol(int idrol)
        {
            cPh_Rol? roles = new();
            try
            {
                roles = await _context.Ph_Roles.FirstOrDefaultAsync(e => e.IDROL == idrol);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return roles;
        }

        /// <summary>
        /// Sincronizar_PhRol: metodo para sincronizar los Roles 
        /// </summary>
        /// <param name="phRoles"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public async Task<EventResponse> Sincronizar_PhRol(IEnumerable<cPh_Rol> phRoles)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var item in phRoles)
                {
                    cPh_Rol? objetoBuscar = await _context.Ph_Roles
                                    .FirstOrDefaultAsync(e => e.IDROL == item.IDROL);
                    //si el rol existe se actualiza descripción
                    //de lo contrario se agrega el registro*
                    if (objetoBuscar is not null)
                    {
                        objetoBuscar.IDROL = item.IDROL;
                        objetoBuscar.DESCRIPCION = item.DESCRIPCION;

                        _context.Ph_Roles.Update(objetoBuscar);
                    }
                    else
                    {
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Rol. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización del Rol. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina_PhRol:  Metodo borrado de datos de la tabla Ph_Roles
        /// </summary>
        /// <param name="idrol"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_PhRol(int idrol)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_Rol? model = await _context.Ph_Roles
                    .FirstOrDefaultAsync(e => e.IDROL == idrol);

                if (model is not null)
                {
                    _context.Ph_Roles.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el Rol. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el Rol. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        /// <summary>
        /// GetEmpleado: Método para una lista de rolesTurno
        /// </summary>
        /// <returns>Una instancia de la clase cPh_RolTurno</returns>
        /// ///<param name="idrol">idNumero del empleado requerido</param>
        public async Task<List<cPh_RolTurno>> GetRolTurno(int idrol)
        {
            List<cPh_RolTurno> rolturno = new();
            try
            {
                rolturno = (from e in await _context.Ph_Roles_Turnos
                                .Include(e => e.Turno)
                                .Where(e => e.IDROL == idrol).ToListAsync()
                            select new cPh_RolTurno
                            {
                                IDREGISTRO = e.IDREGISTRO,
                                IDROL = e.IDROL,
                                IDTURNO = e.IDTURNO,
                                
                                Turno = e.Turno == null ? null :
                                               new cTurno
                                               {
                                                   IdTurno = e.Turno.IdTurno,
                                                   Descripcion = e.Turno.Descripcion,
                                               },

                            }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); throw;
            }
            return rolturno;
        }

        //Creado por: Allan Prieto
        //Fecha: 2023-12-27
        //Sincronizar RolTurno
        //Parametro: Recibe una instancia de RolTurno, se verifica si existe en cuyo caso
        //actualiza el registro, de lo contrario lo crea.
        public async Task<EventResponse> Sincronizar_RolTurno(IEnumerable<cPh_RolTurno> roles_Turno)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                foreach (var rlTurno in roles_Turno)
                {
                    cPh_RolTurno? rturno = await _context.Ph_Roles_Turnos
                                                        .FirstOrDefaultAsync(e => e.IDREGISTRO == rlTurno.IDREGISTRO && e.IDROL == rlTurno.IDROL);
                    //si existe se actualiza 
                    //de lo contrario se agrega el registro
                    if (rturno is not null)
                    {
                        rturno.IDTURNO = rlTurno.IDTURNO;

                        _context.Ph_Roles_Turnos.Update(rturno);
                    }
                    else
                    {
                        rlTurno.IDREGISTRO = 0;
                        _context.Add(rlTurno);
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de RolTurno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de RolTurno. Detalle de Error: " + e.InnerException.Message;
            }
            return respuesta;

        }

        /// <summary>
        /// Elimina_RolTurn:  Metodo borrado de datos de la tabla RolesTurnos
        /// </summary>
        /// <param name="idregistro"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Elimina_RolTurno(int idregistro)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                cPh_RolTurno? model = await _context.Ph_Roles_Turnos
                    .FirstOrDefaultAsync(e => e.IDREGISTRO == idregistro);

                if (model is not null)
                {
                    _context.Ph_Roles_Turnos.Remove(model);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException is null ? e.Message : e.InnerException.Message);
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                if (e.InnerException == null)
                    respuesta.Descripcion = "No se pudo eliminar el registro de Empleado Turno. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo eliminar el registro de Empleado Turno. Detalle de Error: " + e.InnerException.Message;

            }
            return respuesta;
        }



    }
}
