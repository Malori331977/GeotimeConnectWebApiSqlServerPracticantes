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
                empleado = await _context.Empleados.Where(e => e.Estado == 'T').ToListAsync();
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
                empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.IdNumero == idNumero && e.Estado == 'T');
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
                        empleado.IdGrupo = 1;
                        empleado.IdHorario = 1;
                        empleado.Tipo_Marca = "H";
                        empleado.IdAgrupamiento = 0;
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
        //Obtener un Concepto especifico
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
                horarios = await _context.Ph_Horarios.ToListAsync();
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
                foreach (var horario in horarios)
                {
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
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios. Detalle de Error: " + e.Message;
                else
                    respuesta.Descripcion = "No se pudo realizar la sincronización de Horarios. Detalle de Error: " + e.InnerException.Message;

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
    }
}
