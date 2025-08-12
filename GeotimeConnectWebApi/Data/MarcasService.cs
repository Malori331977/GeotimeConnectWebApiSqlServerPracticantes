using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace com.gsitcr.geotime.Data
{
    public class MarcasService : IMarcasService
    {
        private readonly SqlServerDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _schema = "";
        private string _dataBase = "";
        private readonly ILogger<MarcasService> _logger;

        public MarcasService(SqlServerDataBaseContext context, IHttpContextAccessor httpContextAccessor, ILogger<MarcasService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext!.User.Claims;
            string schema = "";
            string bdname = "";
            _logger = logger;

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

                schema = config.GetConnectionString("Schema")!;
                bdname = config.GetConnectionString("DBName")!;
            }
            _schema = schema!;
            _dataBase = bdname!;
            _context = SchemaChangeDbContext.GetSchemaChangeDbContext(schema, bdname);
        }


        /// <summary>
        /// GetMarcaMovTurnoEstadoEnvio: obtener todos los registros con estado falso (no enviados) de MarcasMovTurnos
        /// </summary>
        /// <returns>Lista con todos los registros no eneviados de MarcasMovTurnos</returns>
        public async Task<List<cMarcaMovTurno>> GetMarcaMovTurnoEstadoEnvio()
        {
            List<cMarcaMovTurno> model = new();
            try
            {
                model = (from e in await _context.Marcas_Mov_Turnos.Where(e=>e.estado_envio==false)
                             //.Include(e => e.cTurno)
                             //.Include(e => e.cPh_Planilla)
                              .ToListAsync()
                         select new cMarcaMovTurno
                         {
                             idregistro = e.idregistro,
                             idplanilla = e.idplanilla,
                             idnumero = e.idnumero,
                             fecha = e.fecha,
                             hora = e.hora,
                             turno = e.turno,
                             estado = e.estado,
                             usuario = e.usuario,
                             fecha_reg = e.fecha_reg,
                             linea = e.linea,
                             hentra2 = e.hentra2,
                             estado_envio = true,
                             //cPh_Planilla = e.cPh_Planilla == null ? null :
                             //   new cPh_Planilla
                             //   {
                             //       idplanilla =  e.cPh_Planilla.idplanilla,
                             //       planilla =  e.cPh_Planilla.planilla,
                             //       nom_conector =  e.cPh_Planilla.nom_conector,
                             //       tipo_planilla =  e.cPh_Planilla.tipo_planilla,
                             //       c_ext =  e.cPh_Planilla.c_ext,
                             //       c_inci =  e.cPh_Planilla.c_inci,
                             //       c_adic =  e.cPh_Planilla.c_adic,
                             //       m_desc =  e.cPh_Planilla.m_desc,
                             //       proyecta =  e.cPh_Planilla.proyecta,
                             //       dia_inicio =  e.cPh_Planilla.dia_inicio,
                             //       auto_proceso =  e.cPh_Planilla.auto_proceso,
                             //       tipo_dist =  e.cPh_Planilla.tipo_dist,
                             //       est_nomina =  e.cPh_Planilla.est_nomina,
                             //       ext_per_ant =  e.cPh_Planilla.ext_per_ant,
                             //       ext_det =  e.cPh_Planilla.ext_det,
                             //       agrup_salida =  e.cPh_Planilla.agrup_salida,
                             //       tipo_adic =  e.cPh_Planilla.tipo_adic,
                             //       nivel_aprob_ext =  e.cPh_Planilla.nivel_aprob_ext,
                             //   },
                             //cTurno = e.cTurno == null? null :
                             //   new cTurno
                             //   {
                             //       IdTurno = e.cTurno.IdTurno,
                             //       Descripcion = e.cTurno.Descripcion,
                             //       HEntra = e.cTurno.HEntra,
                             //       HSale = e.cTurno.HSale,
                             //       tar_apl = e.cTurno.tar_apl,
                             //       ant_apl = e.cTurno.ant_apl,
                             //       des_1_in = e.cTurno.des_1_in,
                             //       des_1_out = e.cTurno.des_1_out,
                             //       des_2_in = e.cTurno.des_2_in,
                             //       des_2_out = e.cTurno.des_2_out,
                             //       des_3_in = e.cTurno.des_3_in,
                             //       des_3_out = e.cTurno.des_3_out,
                             //       apl_des_1 = e.cTurno.apl_des_1,
                             //       apl_des_2 = e.cTurno.apl_des_2,
                             //       apl_des_3 = e.cTurno.apl_des_3,
                             //       des_1_tiem = e.cTurno.des_1_tiem,
                             //       des_2_tiem = e.cTurno.des_2_tiem,
                             //       des_3_tiem = e.cTurno.des_3_tiem,
                             //       marca_des_1 = e.cTurno.marca_des_1,
                             //       marca_des_2 = e.cTurno.marca_des_2,
                             //       marca_des_3 = e.cTurno.marca_des_3,
                             //       tar_tiem = e.cTurno.tar_tiem,
                             //       ant_tiem = e.cTurno.ant_tiem,
                             //       con_1 = e.cTurno.con_1,
                             //       con_2 = e.cTurno.con_2,
                             //       con_3 = e.cTurno.con_3,
                             //       con_4 = e.cTurno.con_4,
                             //       con_5 = e.cTurno.con_5,
                             //       con_6 = e.cTurno.con_6,
                             //       cant_con_1 = e.cTurno.cant_con_1,
                             //       cant_con_2 = e.cTurno.cant_con_2,
                             //       cant_con_3 = e.cTurno.cant_con_3,
                             //       cant_con_4 = e.cTurno.cant_con_4,
                             //       cant_con_5 = e.cTurno.cant_con_5,
                             //       cant_con_6 = e.cTurno.cant_con_6,
                             //       min_con_1 = e.cTurno.min_con_1,
                             //       min_con_2 = e.cTurno.min_con_2,
                             //       min_con_3 = e.cTurno.min_con_3,
                             //       min_con_4 = e.cTurno.min_con_4,
                             //       min_con_5 = e.cTurno.min_con_5,
                             //       min_con_6 = e.cTurno.min_con_6,
                             //       Tipo = e.cTurno.Tipo,
                             //       Tipo_Jor = e.cTurno.Tipo_Jor,
                             //       fuerza_calc = e.cTurno.fuerza_calc,
                             //       idagrupamiento = e.cTurno.idagrupamiento,
                             //       apl_trans1 = e.cTurno.apl_trans1,
                             //       id_trans1 = e.cTurno.id_trans1,
                             //       apl_trans2 = e.cTurno.apl_trans2,
                             //       id_trans2 = e.cTurno.id_trans2,
                             //       apl_trans3 = e.cTurno.apl_trans3,
                             //       id_trans3 = e.cTurno.id_trans3,
                             //       apl_trans4 = e.cTurno.apl_trans4,
                             //       id_trans4 = e.cTurno.id_trans4,
                             //       apl_trans5 = e.cTurno.apl_trans5,
                             //       id_trans5 = e.cTurno.id_trans5,
                             //       apl_trans6 = e.cTurno.apl_trans6,
                             //       id_trans6 = e.cTurno.id_trans6,
                             //       apl_ben1 = e.cTurno.apl_ben1,
                             //       id_ben1 = e.cTurno.id_ben1,
                             //       apl_ben2 = e.cTurno.apl_ben2,
                             //       id_ben2 = e.cTurno.id_ben2,
                             //       apl_ben3 = e.cTurno.apl_ben3,
                             //       id_ben3 = e.cTurno.id_ben3,
                             //       apl_ben4 = e.cTurno.apl_ben4,
                             //       id_ben4 = e.cTurno.id_ben4,
                             //       apl_ben5 = e.cTurno.apl_ben5,
                             //       id_ben5 = e.cTurno.id_ben5,
                             //       apl_ben6 = e.cTurno.apl_ben6,
                             //       id_ben6 = e.cTurno.id_ben6,
                             //       conc_ben1 = e.cTurno.conc_ben1,
                             //       conc_ben2 = e.cTurno.conc_ben2,
                             //       conc_ben3 = e.cTurno.conc_ben3,
                             //       conc_ben4 = e.cTurno.conc_ben4,
                             //       conc_ben5 = e.cTurno.conc_ben5,
                             //       conc_ben6 = e.cTurno.conc_ben6,
                             //       apl_trans_post = e.cTurno.apl_trans_post,
                             //       id_trans_post = e.cTurno.id_trans_post,
                             //       apl_redond_entrada = e.cTurno.apl_redond_entrada,
                             //       cant_redond_entrada = e.cTurno.cant_redond_entrada,
                             //       auto_pan = e.cTurno.auto_pan,
                             //       ColorId = e.cTurno.ColorId,
                             //   }

                         }).ToList();

              
            }
            catch (Exception e)
            {
                model = new List<cMarcaMovTurno>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"MarcasService.GetMarcaMovTurnoEstadoEnvio: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }


        /// <summary>
        /// PostMarcaMovTurnoEstadoEnvio: Actualizar estado de envio para las marcas listadas
        /// </summary>
        /// <param name="listaMarcasMovTurno"></param>
        /// <returns>EventResponse: con estado de la actualizacion</returns>
        public async Task<EventResponse> PostMarcaMovTurnoEstadoEnvio(List<cMarcaMovTurno> listaMarcasMovTurno)
        {
            EventResponse respuesta = new EventResponse();
            List<cMarcaMovTurno> marcasMovTurnoActualizar = new List<cMarcaMovTurno>();
            try
            {
                foreach (cMarcaMovTurno item in listaMarcasMovTurno)
                {
                    cMarcaMovTurno? marca = await _context.Marcas_Mov_Turnos
                        .FirstOrDefaultAsync(e => e.idregistro == item.idregistro);

                    if (marca != null)
                    {
                        marca.estado_envio = true;
                        marcasMovTurnoActualizar.Add(marca);
                       
                    }
                   
                }
                _context.Marcas_Mov_Turnos.UpdateRange(marcasMovTurnoActualizar);
                await _context.SaveChangesAsync();


            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = $"No se pudo realizar la actualización de Marcas_Mov_Turnos. Detalle de Error: {error}";
               

            }

            return respuesta;

        }


        /// <summary>
        /// GetMarcaIncidenciaEstadoEnvio: obtener todos los registros con estado falso (no enviados) de Marcas Incidencias
        /// </summary>
        /// <returns>Lista con todos los registros no enviados de Marcas Incidencias</returns>
        public async Task<List<cMarcaIncidencia>> GetMarcaIncidenciaEstadoEnvio()
        {
            List<cMarcaIncidencia> model = new();
            try
            {
                model = (from e in await _context.Marcas_Incidencias.Where(e => e.ESTADO_ENVIO == false)
                             //.Include(e => e.cIncidencia)
                             //.Include(e => e.cIncidenciaJust)
                              .ToListAsync()
                         select new cMarcaIncidencia
                         {
                             INDICE = e.INDICE,
                             IDPLANILLA = e.IDPLANILLA,
                             IDNUMERO = e.IDNUMERO,
                             FECHA = e.FECHA,
                             IDINCIDENCIA = e.IDINCIDENCIA,
                             IDREGISTRO = e.IDREGISTRO,
                             HENTRA = e.HENTRA,
                             HSALE = e.HSALE,
                             EST_P = e.EST_P,
                             COMENTARIO = e.COMENTARIO,
                             INCIDENCIA_JUST = e.INCIDENCIA_JUST,
                             ESTADO = e.ESTADO,
                             C_TIEMPO = e.C_TIEMPO,
                             USUARIO = e.USUARIO,
                             FECHA_JUST = e.FECHA_JUST,
                             IDACC = e.IDACC,
                             HENTRA2 = e.HENTRA2,
                             ESTADO_ENVIO = e.ESTADO_ENVIO,                             
                             //cIncidencia = e.cIncidencia == null ? null :
                             //   new cIncidencia
                             //   {
                             //       Id  = e.cIncidencia.Id,
                             //       Codigo = e.cIncidencia.Codigo,
                             //       Descripcion = e.cIncidencia.Descripcion,
                             //       id_pago = e.cIncidencia.id_pago,
                             //       nom_conector = e.cIncidencia.nom_conector,
                             //       tipo = e.cIncidencia.tipo,
                             //       ed_tiempo = e.cIncidencia.ed_tiempo,
                             //       requiere_accper = e.cIncidencia.requiere_accper,
                             //       marca_web = e.cIncidencia.marca_web,
                             //   },
                             //cIncidenciaJust = e.cIncidenciaJust == null ? null :
                             //   new cIncidencia
                             //   {
                             //       Id = e.cIncidenciaJust.Id,
                             //       Codigo = e.cIncidenciaJust.Codigo,
                             //       Descripcion = e.cIncidenciaJust.Descripcion,
                             //       id_pago = e.cIncidenciaJust.id_pago,
                             //       nom_conector = e.cIncidenciaJust.nom_conector,
                             //       tipo = e.cIncidenciaJust.tipo,
                             //       ed_tiempo = e.cIncidenciaJust.ed_tiempo,
                             //       requiere_accper = e.cIncidenciaJust.requiere_accper,
                             //       marca_web = e.cIncidenciaJust.marca_web,
                             //   }

                         }).ToList();


            }
            catch (Exception e)
            {
                model = new List<cMarcaIncidencia>();
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"MarcasService.GetMarcaIncidenciaEstadoEnvio: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                throw;
            }
            return model;
        }


        /// <summary>
        /// PostMarcaMovTurnoEstadoEnvio: Actualizar estado de envio para las marcas listadas
        /// </summary>
        /// <param name="listaMarcasMovTurno"></param>
        /// <returns>EventResponse: con estado de la actualizacion</returns>
        public async Task<EventResponse> PostMarcaIncidenciaEstadoEnvio(List<cMarcaIncidencia> listaMarcasMovTurno)
        {
            EventResponse respuesta = new EventResponse();
            List<cMarcaIncidencia> marcasIncidenciaActualizar = new List<cMarcaIncidencia>();
            try
            {
                foreach (cMarcaIncidencia item in listaMarcasMovTurno)
                {
                    cMarcaIncidencia? marca = await _context.Marcas_Incidencias
                        .FirstOrDefaultAsync(e => e.INDICE == item.INDICE);

                    if (marca != null)
                    {
                        marca.ESTADO_ENVIO = true;
                        marcasIncidenciaActualizar.Add(marca);

                    }

                }
                _context.Marcas_Incidencias.UpdateRange(marcasIncidenciaActualizar);
                await _context.SaveChangesAsync();


            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"{error}");
                respuesta.Id = "1";
                respuesta.Respuesta = "Error";
                respuesta.Descripcion = $"No se pudo realizar la actualización de Marcas_Incidencias. Detalle de Error: {error}";
            }

            return respuesta;

        }


    }
}
