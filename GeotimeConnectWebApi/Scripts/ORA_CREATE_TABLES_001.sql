--
-- Creating table PH_PLANILLA
-- ==========================
--
create table GEOTIME.PH_PLANILLA
(
  idplanilla    VARCHAR2(8) not null,
  planilla      VARCHAR2(60) not null,
  nom_conector  VARCHAR2(10),
  tipo_planilla CHAR(1),
  c_ext         CHAR(1) default 'F',
  c_inci        CHAR(1) default 'F',
  c_adic        CHAR(1) default 'F',
  m_desc        CHAR(1) default 'F',
  proyecta      CHAR(1) default 'F',
  dia_inicio    NUMBER  default 0 NULL,
  auto_proceso  CHAR(1) default 'F',
  tipo_dist     CHAR(1) default 'C',
  est_nomina    VARCHAR2(1) default 'M',
  ext_per_ant     CHAR(1) default 'F',
  ext_det         CHAR(1) default 'F',
  agrup_salida    CHAR(1) default 'F',
  tipo_adic       CHAR(1) default 'H',
  nivel_aprob_ext Number default 0 Null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_PLANILLA
  add constraint PK_IDPLANILLA primary key (IDPLANILLA)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table ACCIONES_PERSONAL
-- ================================
--
create table GEOTIME.ACCIONES_PERSONAL
(
  idregistro   NUMBER not null,
  idplanilla   VARCHAR2(8) not null,
  idnumero     VARCHAR2(16) not null,
  inicio       DATE not null,
  fin          DATE not null,
  idincidencia NUMBER not null,
  estado       CHAR(1),
  idaccion     NUMBER,
  comentario   VARCHAR2(2048),
  dias         NUMBER,
  usuario      VARCHAR2(100),
  fecha_just   DATE default sysdate,
  dias_apl     VARCHAR2(15)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create unique index GEOTIME.IX_ACCIONES on GEOTIME.ACCIONES_PERSONAL (IDREGISTRO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_ACCIONES_1 on GEOTIME.ACCIONES_PERSONAL (IDPLANILLA, IDNUMERO, INICIO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_ACCIONES_2 on GEOTIME.ACCIONES_PERSONAL (IDPLANILLA, IDNUMERO, INICIO, IDINCIDENCIA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.ACCIONES_PERSONAL
  add constraint ACCIONES_PH_PLANILLA foreign key (IDPLANILLA)
  references GEOTIME.PH_PLANILLA (idplanilla);

--
-- Creating table PH_CCOSTOS
-- =========================
--
create table GEOTIME.PH_CCOSTOS
(
  idccosto    VARCHAR2(25) not null,
  descripcion VARCHAR2(200) not null,
  distribuye  CHAR(1) default 'F',
  alias_ccosto VARCHAR2(200)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_CCOSTOS
  add constraint PK_CC primary key (IDCCOSTO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create table
create table GEOTIME.PH_GRUPO_PERIODO
(
  idgrupo    number not null,
  idperiodo  varchar2(8) not null,
  idplanilla varchar2(8) not null,
  estado     char(1) not null,
  fecha_cierre DATE,
  usuario_cierre number
)
tablespace TBGEOTIME
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table GEOTIME.PH_GRUPO_PERIODO
  add constraint pk_grupo_periodo primary key (IDGRUPO, IDPERIODO, IDPLANILLA);



--
--Creating table ph_proyectos
-- Create table
create table GEOTIME.PH_PROYECTO
(
  proyecto     VARCHAR2(25) not null,
  descripcion  VARCHAR2(50) not null,
  centro_costo VARCHAR2(25) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate indexes 
create index GEOTIME.IX_PROYECTO on GEOTIME.PH_PROYECTO (PROYECTO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--creating table proyecto-fase
-- Create table
create table GEOTIME.PH_FASEPROYECTO
(
  proyecto     VARCHAR2(25) not null,
  fase         VARCHAR2(25) not null,
  nombre       VARCHAR2(80) not null,
  descripcion  VARCHAR2(400),
  acepta_datos CHAR(1) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table GEOTIME.PH_FASEPROYECTO
  add constraint PK_PROYECTOFASE primary key (PROYECTO, FASE)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
--
-- Creating table PH_DEPARTAMENTO
-- ==============================
--
create table GEOTIME.PH_DEPARTAMENTO
(
  iddepart    VARCHAR2(10) not null,
  descripcion VARCHAR2(60) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_DEPARTAMENTO
  add constraint PK_IDDEPART primary key (IDDEPART)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_GRUPOS
-- ========================
--
create table GEOTIME.PH_GRUPOS
(
  idgrupo        NUMBER not null,
  descripcion    VARCHAR2(30) not null,
  idplanilla     VARCHAR2(8),
  estado         CHAR(1),
  idagrupamiento NUMBER default 0 not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_GRUPOS
  add constraint PKIDGRUPO primary key (IDGRUPO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_HORARIOS
-- ==========================
--
create table GEOTIME.PH_HORARIOS
(
  idhorario   NUMBER not null,
  descripcion VARCHAR2(20) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_HORARIOS
  add constraint PK_IDHORARIO primary key (IDHORARIO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table EMPLEADOS
-- ========================
--
create table GEOTIME.EMPLEADOS
(
  idplanilla     VARCHAR2(8) not null,
  idnumero       VARCHAR2(16) not null,
  nombre         VARCHAR2(50) not null,
  tarjeta        VARCHAR2(16) not null,
  idgrupo        NUMBER not null,
  iddepartamento VARCHAR2(10) not null,
  idhorario      NUMBER not null,
  estado         CHAR(1),
  idagrupamiento NUMBER default 0 not null,
  foto           BLOB,
  idccosto       VARCHAR2(25) not null,
  exporta        CHAR(1),
  ubicacion      VARCHAR2(8),
  rubro1         VARCHAR2(40),
  rubro2         VARCHAR2(40),
  rubro3         VARCHAR2(40),
  rubro4         VARCHAR2(40),
  rubro5         VARCHAR2(40),
  rubro6         VARCHAR2(40),
  rubro7         VARCHAR2(40),
  rubro8         VARCHAR2(40),
  rubro9         VARCHAR2(40),
  rubro10        VARCHAR2(40),
  rubro11        VARCHAR2(40),
  rubro12        VARCHAR2(40),
  rubro13        VARCHAR2(40),
  rubro14        VARCHAR2(40),
  rubro15        VARCHAR2(40),
  rubro16        VARCHAR2(40),
  rubro17        VARCHAR2(40),
  rubro18        VARCHAR2(40),
  rubro19        VARCHAR2(40),
  rubro20        VARCHAR2(40),
  rubro21        VARCHAR2(40),
  rubro22        VARCHAR2(40),
  rubro23        VARCHAR2(40),
  rubro24        VARCHAR2(40),
  rubro25        VARCHAR2(40),
  fecha_ingreso  DATE,
  email          VARCHAR2(80),
  tipo_marca     CHAR(1) default ('H') not null,
  inicio_rol     DATE,
  web_pass       VARCHAR2(400),
  id_transfo_conc number default (0),
  widioma        varchar2(5) default('esp'),
  nit_empresa    VARCHAR2(255),
  global_code varchar2(10),
  fecha_act_code DATE
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_EMPLEADO on GEOTIME.EMPLEADOS (IDNUMERO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_EMPLEADO1 on GEOTIME.EMPLEADOS (IDPLANILLA, IDNUMERO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.EMPLEADOS
  add constraint PK_EMPLEADO primary key (IDNUMERO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.EMPLEADOS
  add constraint FK_EMPLEADO_CCOSTO foreign key (IDCCOSTO)
  references GEOTIME.PH_CCOSTOS (IDCCOSTO);
alter table GEOTIME.EMPLEADOS
  add constraint FK_EMPLEADO_DEPARTAMENTO foreign key (IDDEPARTAMENTO)
  references GEOTIME.PH_DEPARTAMENTO (IDDEPART);
alter table GEOTIME.EMPLEADOS
  add constraint FK_EMPLEADO_GRUPO foreign key (IDGRUPO)
  references GEOTIME.PH_GRUPOS (IDGRUPO);
alter table GEOTIME.EMPLEADOS
  add constraint FK_EMPLEADO_HORARIO foreign key (IDHORARIO)
  references GEOTIME.PH_HORARIOS (IDHORARIO);
alter table GEOTIME.EMPLEADOS
  add constraint FK_EMPLEADO_PLANILLA foreign key (IDPLANILLA)
  references GEOTIME.PH_PLANILLA (IDPLANILLA);

--
-- Creating table INCIDENCIAS
-- ==========================
--
create table GEOTIME.INCIDENCIAS
(
  id           NUMBER not null,
  codigo       VARCHAR2(5) not null,
  descripcion  VARCHAR2(50) not null,
  id_pago      NUMBER,
  nom_conector VARCHAR2(8),
  tipo         NUMBER,
  ed_tiempo    CHAR(1),
  requiere_accper CHAR(1)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.INCIDENCIAS
  add constraint PK_INCIDENCIA primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table INCIDENCIAS_CONF_PAGO
-- ====================================
--
create table GEOTIME.INCIDENCIAS_CONF_PAGO
(
  id           NUMBER not null,
  descripcion  VARCHAR2(60) not null,
  id_apl       NUMBER not null,
  id_hrs       NUMBER not null,
  id_con       NUMBER not null,
  id_adicional NUMBER not null,
  apl_turno    NUMBER,
  tran_turno   NUMBER
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.INCIDENCIAS_CONF_PAGO
  add constraint PK_INCI_CONF_PAGO primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table MARCAS
-- =====================
--
create table GEOTIME.MARCAS
(
  registro   NUMBER not null,
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  hora       VARCHAR2(5) not null,
  tipo       NUMBER not null,
  idterminal VARCHAR2(4) not null,
  fecha_reg DATE NULL,
  long_reg VARCHAR2(500) NULL,
  lat_reg VARCHAR2(500) NULL,
  gps_cell char(1) null,
  img_verif VARCHAR2(5) null,
  estado     CHAR(1) default 'N',
  fecha_hora DATE NULL
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS
  add constraint PK_MARCAS primary key (IDPLANILLA, IDNUMERO, FECHA, HORA)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS
  add constraint FK_MARCAS_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_AUDIT
-- ===========================
--
create table GEOTIME.MARCAS_AUDIT
(
  idregistro NUMBER not null,
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  usuario    VARCHAR2(100),
  fecha_orig DATE,
  fecha_chg  DATE,
  hora_orig  VARCHAR2(5),
  hora_chg   VARCHAR2(5),
  comentario VARCHAR2(4000)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_AUDIT on GEOTIME.MARCAS_AUDIT (IDPLANILLA, IDNUMERO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_AUDIT_1 on GEOTIME.MARCAS_AUDIT (USUARIO, FECHA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_AUDIT_2 on GEOTIME.MARCAS_AUDIT (FECHA_CHG, HORA_CHG)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_AUDIT
  add constraint PK_MAUDIT primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table MARCAS_COMEDOR
-- =============================
--
create table GEOTIME.MARCAS_COMEDOR
(
  idregistro NUMBER not null,
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  hora       VARCHAR2(5) not null,
  tipo       NUMBER default 4,
  idterminal VARCHAR2(4)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_COMEDOR on GEOTIME.MARCAS_COMEDOR (IDPLANILLA, IDNUMERO, FECHA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_COMEDOR
  add constraint PK_MARCAS_COMEDOR primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table MARCAS_DESCANSOS
-- ===============================
--
create table GEOTIME.MARCAS_DESCANSOS
(
  idregistro  NUMBER not null,
  iddesc      NUMBER not null,
  idplanilla  VARCHAR2(8) not null,
  idnumero    VARCHAR2(16) not null,
  fecha       DATE not null,
  inicio_desc VARCHAR2(5),
  fin_desc    VARCHAR2(5)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_DESCANSOS on GEOTIME.MARCAS_DESCANSOS (IDPLANILLA, IDNUMERO, FECHA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_DESCANSOS
  add constraint PK_MARCAS_DESCANSOS primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_CONCEPTOS
-- ===========================
--
create table GEOTIME.PH_CONCEPTOS
(
  id          NUMBER not null,
  concepto    VARCHAR2(6) not null,
  descripcion VARCHAR2(50) not null,
  tipo_j      NUMBER not null,
  tipo_h      NUMBER not null,
  columnar    NUMBER not null,
  nominaeq    VARCHAR2(20),
  factor      NUMBER not null,
  tolerancia  NUMBER not null,
  ordinario   CHAR(1) not null,
  autorizado  CHAR(1) not null,
  transferir  CHAR(1),
  adicional   CHAR(1) not null,
  tipo_ext_alm CHAR(1) default 'D',
  muestra_resumen CHAR(1)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_CONCEPTO on GEOTIME.PH_CONCEPTOS (CONCEPTO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_CONCEPTOS
  add constraint PK_IDCONCEPTO primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table MARCAS_DISTRIBUCIONES
-- ====================================
--
create table GEOTIME.MARCAS_DISTRIBUCIONES
(
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  idconcepto NUMBER not null,
  nominaeq   VARCHAR2(20) not null,
  cantidad   NUMBER not null,
  idccosto   VARCHAR2(25),
  proyecto   VARCHAR2(25) default '',
  fase   VARCHAR2(25) default '',
  concepto   VARCHAR2(6),
  tipo       CHAR(1),
  estado     CHAR(1) default 'A',
  entrada    VARCHAR2(5) default '00:00' not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_DISTRIBUCIONES on GEOTIME.MARCAS_DISTRIBUCIONES (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_DISTRIBUCIONES
  add constraint FK_DISTRIBUCIO_EMPLEADOS foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);
alter table GEOTIME.MARCAS_DISTRIBUCIONES
  add constraint FK_DISTRIBUCIO_PH_CCOSTOS foreign key (IDCCOSTO)
  references GEOTIME.PH_CCOSTOS (IDCCOSTO);
alter table GEOTIME.MARCAS_DISTRIBUCIONES
  add constraint FK_DISTRIBUCIO_PH_CONCEPTOS foreign key (IDCONCEPTO)
  references GEOTIME.PH_CONCEPTOS (ID);

--
-- Creating table MARCAS_DISTRIBUCIONES_CONCEPTO
-- =============================================
--
create table GEOTIME.MARCAS_DISTRIBUCIONES_CONCEPTO
(
  idregistro NUMBER not null,
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  idccosto   VARCHAR2(25) not null,
  proyecto   VARCHAR2(25) ,
  fase   VARCHAR2(25) ,
  cantidad   NUMBER,
  inicio     VARCHAR2(5),
  fin        VARCHAR2(5),
  estado     CHAR(1) default 'A' not null,
  iddist     NUMBER not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MAR_DIS_CONC on GEOTIME.MARCAS_DISTRIBUCIONES_CONCEPTO (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_DISTRIBUCIONES_CONCEPTO
  add constraint PK_M_DUST_CONC primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_DISTRIBUCIONES_CONCEPTO
  add constraint FK_M_DIST_CC foreign key (IDCCOSTO)
  references GEOTIME.PH_CCOSTOS (IDCCOSTO);

--
-- Creating table MARCAS_EXTRAS_APB
-- ================================
--
create table GEOTIME.MARCAS_EXTRAS_APB
(
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  hora       VARCHAR2(5) not null,
  cantidad   VARCHAR2(5) not null,
  comentario VARCHAR2(1024),
  estado     CHAR(1) default 'A' not null,
  usuario    VARCHAR2(100),
  ccosto     varchar2(25)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_EXTRAS_APB on GEOTIME.MARCAS_EXTRAS_APB (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_EXTRAS_APB
  add constraint PK_EXTRAS_APB primary key ( IDPLANILLA, IDNUMERO, FECHA, HORA)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_EXTRAS_APB
  add constraint FK_EXTRAS_APB_EMPLEADO foreign key ( IDNUMERO)
  references GEOTIME.EMPLEADOS ( IDNUMERO);
  --
-- Creating table MARCAS_EXTRAS_DETALLE
-- ================================
--
create table GEOTIME.MARCAS_EXTRAS_DETALLE
(
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  hora       VARCHAR2(5) not null,
  cantidad   VARCHAR2(5) not null,
  cantidad_aprob   VARCHAR2(5) DEFAULT '00:00' not null,
  comentario VARCHAR2(1024),
  estado     CHAR(1) default 'A' not null,
  usuario    VARCHAR2(100),
  ccosto     varchar2(25),
  idconcepto number
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_EXTRAS_DETALLE on GEOTIME.MARCAS_EXTRAS_DETALLE (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_EXTRAS_DETALLE
  add constraint PK_EXTRAS_DETALLE primary key ( IDPLANILLA, IDNUMERO, FECHA, HORA)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_EXTRAS_DETALLE
  add constraint FK_EXTRAS_DETALLE_EMPLEADO foreign key ( IDNUMERO)
  references GEOTIME.EMPLEADOS ( IDNUMERO);
--
-- Creating table MARCAS_IN
-- ========================
--
create table GEOTIME.MARCAS_IN
(
  idtarjeta  VARCHAR2(16),
  fecha      DATE,
  hora       VARCHAR2(5),
  idterminal VARCHAR2(50),
  tipo       NUMBER,
  idplanilla VARCHAR2(8),
  idnumero   VARCHAR2(16),
  fecha_reg DATE NULL,
  long_reg VARCHAR2(500) NULL,
  lat_reg VARCHAR2(500) NULL,
  gps_cell char(1) null,
  estado     CHAR(1) default 'N'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_IN on GEOTIME.MARCAS_IN (IDNUMERO, FECHA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table MARCAS_INCIDENCIAS
-- =================================
--
create table GEOTIME.MARCAS_INCIDENCIAS
(
  indice          NUMBER,
  idplanilla      VARCHAR2(8),
  idnumero        VARCHAR2(16),
  fecha           DATE,
  idincidencia    NUMBER,
  idregistro      NUMBER,
  hentra          VARCHAR2(5),
  hsale           VARCHAR2(5),
  est_p           CHAR(1),
  comentario      VARCHAR2(2048),
  incidencia_just NUMBER,
  estado          CHAR(1) default 'A',
  c_tiempo        VARCHAR2(5) default '00:00',
  usuario         VARCHAR2(100),
  fecha_just      DATE,
  idacc           NUMBER
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create unique index GEOTIME.IX_MARCAS_INCIDENCIAS on GEOTIME.MARCAS_INCIDENCIAS (INDICE)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_INCIDENCIAS1 on GEOTIME.MARCAS_INCIDENCIAS (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_INCIDENCIAS2 on GEOTIME.MARCAS_INCIDENCIAS (IDACC)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_INCIDENCIAS
  add constraint FK_MARCA_INIC_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);
alter table GEOTIME.MARCAS_INCIDENCIAS
  add constraint FK_MARCA_INIC_INCI foreign key (IDINCIDENCIA)
  references GEOTIME.INCIDENCIAS (ID);

--
-- Creating table MARCAS_MOV_TURNOS
-- ================================
--
create table GEOTIME.MARCAS_MOV_TURNOS
(
  idregistro NUMBER not null,
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  hora       VARCHAR2(5) not null,
  turno      NUMBER not null,
  estado     CHAR(1) default 'A' not null,
  usuario    VARCHAR2(100),
  fecha_reg  DATE default sysdate,
  linea      number default 1
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_MOV on GEOTIME.MARCAS_MOV_TURNOS (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_MOV_TURNOS
  add constraint FK_MARCAS_MOV_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_MOV_TURNOS
-- ================================
--
create table GEOTIME.marcas_mov_horarios
(
  idregistro NUMBER not null,
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  hora       VARCHAR2(5) not null,
  idhorario      NUMBER not null,
  estado     CHAR(1) default 'A' not null,
  usuario    VARCHAR2(100),
  fecha_reg  DATE default sysdate,
  linea      number default 1
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_HOR on GEOTIME.marcas_mov_horarios (IDPLANILLA, IDNUMERO, FECHA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.marcas_mov_horarios
  add constraint FK_MARCAS_MOVH_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_PROCESO
-- =============================
--
create table GEOTIME.MARCAS_PROCESO
(
  idregistro       NUMBER not null,
  idplanilla       VARCHAR2(8) not null,
  idnumero         VARCHAR2(16) not null,
  fecha_entra      DATE not null,
  fecha_sale       DATE not null,
  hora_entra       VARCHAR2(5) not null,
  hora_sale        VARCHAR2(5) not null,
  idturno          NUMBER default 1,
  ordc             VARCHAR2(5) default '00:00',
  extc             VARCHAR2(5) default '00:00',
  ordt             VARCHAR2(5) default '00:00',
  extt             VARCHAR2(5) default '00:00',
  tc1              VARCHAR2(5) default '00:00',
  tc2              VARCHAR2(5) default '00:00',
  tc3              VARCHAR2(5) default '00:00',
  tc4              VARCHAR2(5) default '00:00',
  tc5              VARCHAR2(5) default '00:00',
  con_1            NUMBER default 1,
  con_2            NUMBER default 1,
  con_3            NUMBER default 1,
  con_4            NUMBER default 1,
  con_5            NUMBER default 1,
  id1              VARCHAR2(5) default '00:00',
  id2              VARCHAR2(5) default '00:00',
  id3              VARCHAR2(5) default '00:00',
  fd1              VARCHAR2(5) default '00:00',
  fd2              VARCHAR2(5) default '00:00',
  fd3              VARCHAR2(5) default '00:00',
  tod              VARCHAR2(5) default '00:00',
  ted              VARCHAR2(5) default '00:00',
  tdd              VARCHAR2(5) default '00:00',
  tiempo_calc_jorn VARCHAR2(5) default '00:00',
  tdc              VARCHAR2(5) default '00:00',
  estado           CHAR(1) default 'A',
  estado_inc       CHAR(1),
  manticipo        VARCHAR2(5) default '00:00',
  mtardia          VARCHAR2(5) default '00:00',
  proyectado       CHAR(1) default 'F' not null,
  reg_sale         NUMBER
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_PROCESO on GEOTIME.MARCAS_PROCESO (IDPLANILLA, IDNUMERO, FECHA_ENTRA, ESTADO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_PROCESO
  add constraint PK_MARCAS_PROCESO primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_PROCESO
  add constraint FK_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_PROYECCIONES
-- ==================================
--
create table GEOTIME.MARCAS_PROYECCIONES
(
  idplanilla       VARCHAR2(8) not null,
  idnumero         VARCHAR2(16) not null,
  fecha            DATE not null,
  concepto         NUMBER not null,
  cantidad_proyect VARCHAR2(5) default '00:00',
  cantidad_lab     VARCHAR2(5) default '00:00',
  estado           CHAR(1) default 'A'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_PROYECCION on GEOTIME.MARCAS_PROYECCIONES (IDPLANILLA, IDNUMERO, FECHA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_PROYECCIONES
  add constraint FK_PROYECCION_CONCEPTO foreign key (CONCEPTO)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.MARCAS_PROYECCIONES
  add constraint FK_PROYECCION_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_REPORTES
-- ==============================
--
create table GEOTIME.MARCAS_REPORTES
(
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  fecha      DATE not null,
  t1         VARCHAR2(5) default '00:00',
  t2         VARCHAR2(5) default '00:00',
  t3         VARCHAR2(5) default '00:00',
  t4         VARCHAR2(5) default '00:00',
  t5         VARCHAR2(5) default '00:00',
  t6         VARCHAR2(5) default '00:00',
  t7         VARCHAR2(5) default '00:00',
  t8         VARCHAR2(5) default '00:00',
  t9         VARCHAR2(5) default '00:00',
  t10        VARCHAR2(5) default '00:00',
  t11        VARCHAR2(5) default '00:00',
  t12        VARCHAR2(5) default '00:00',
  t13        VARCHAR2(5) default '00:00',
  periodo    VARCHAR2(8),
  estado     CHAR(1) default 'A'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_REPORTES on GEOTIME.MARCAS_REPORTES (IDPLANILLA, IDNUMERO, FECHA)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_REPORTES1 on GEOTIME.MARCAS_REPORTES (IDPLANILLA, IDNUMERO, PERIODO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_REPORTES
  add constraint FK_MARCAS_REP_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_RESUMEN
-- =============================
--
create table GEOTIME.MARCAS_RESUMEN
(
  idplanilla VARCHAR2(8) not null,
  idnumero   VARCHAR2(16) not null,
  idconcepto NUMBER,
  nominaeq   VARCHAR2(20),
  cantidad   NUMBER,
  monto      NUMBER,
  idccosto   VARCHAR2(25),
  proyecto   VARCHAR2(25),
  fase       VARCHAR2(25),
  idperiodo  VARCHAR2(8)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_MARCAS_RESUMEN on GEOTIME.MARCAS_RESUMEN (IDPLANILLA, IDNUMERO, IDPERIODO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_RESUMEN
  add constraint FK_MARCA_RESUMEN_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table MARCAS_TIEMPO_ADICIONAL
-- ======================================
--
create table GEOTIME.MARCAS_TIEMPO_ADICIONAL
(
  idregistro        NUMBER not null,
  idplanilla        VARCHAR2(8) not null,
  idnumero          VARCHAR2(16) not null,
  periodo           VARCHAR2(8) not null,
  idconcepto        NUMBER not null,
  cantidad          VARCHAR2(5) not null,
  fecha_referencia  DATE not null,
  usuario           VARCHAR2(100) not null,
  fecha_registro    DATE not null,
  fecha_actualiza   DATE,
  centro_costo      VARCHAR2(25),
  comentario        VARCHAR2(2048),
  usuario_actualiza VARCHAR2(100),
  estado            CHAR(1) default 'A' not null,
  tcantidad         number,
  proyecto          VARCHAR2(25),
  fase              VARCHAR2(25)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_TIEMPO_AIDCIONAL on GEOTIME.MARCAS_TIEMPO_ADICIONAL (IDPLANILLA, IDNUMERO, PERIODO)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_TIEMPO_ADICIONAL
  add constraint PK_TIEMPO_ADICIONAL primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.MARCAS_TIEMPO_ADICIONAL
  add constraint FK_TIEMPO_ADIC_CONCEPTO foreign key (IDCONCEPTO)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.MARCAS_TIEMPO_ADICIONAL
  add constraint FK_TIEMPO_ADIC_EMPLEADO foreign key (IDNUMERO)
  references GEOTIME.EMPLEADOS (IDNUMERO);

--
-- Creating table PH_COND_ESPECIALES
-- =================================
--
create table GEOTIME.PH_COND_ESPECIALES
(
  idregistro  NUMBER not null,
  descripcion VARCHAR2(15) not null,
  concepto    NUMBER not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_COND_ESPECIALES
  add constraint PK_COND_ESP primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_COND_ESPECIALES
  add constraint FK_CONCONC_CONCEP foreign key (CONCEPTO)
  references GEOTIME.PH_CONCEPTOS (ID);

--
-- Creating table PH_TURNOS
-- ========================
--
create table GEOTIME.PH_TURNOS
(
  idturno        NUMBER not null,
  descripcion    VARCHAR2(30) not null,
  hentra         VARCHAR2(5) not null,
  hsale          VARCHAR2(5) not null,
  tar_apl        CHAR(1),
  ant_apl        CHAR(1),
  des_1_in       VARCHAR2(5) default '00:00',
  des_1_out      VARCHAR2(5) default '00:00',
  des_2_in       VARCHAR2(5) default '00:00',
  des_2_out      VARCHAR2(5) default '00:00',
  des_3_in       VARCHAR2(5) default '00:00',
  des_3_out      VARCHAR2(5) default '00:00',
  apl_des_1      CHAR(1),
  apl_des_2      CHAR(1),
  apl_des_3      CHAR(1),
  des_1_tiem     VARCHAR2(5) default '00:00',
  des_2_tiem     VARCHAR2(5) default '00:00',
  des_3_tiem     VARCHAR2(5) default '00:00',
  marca_des_1    CHAR(1),
  marca_des_2    CHAR(1),
  marca_des_3    CHAR(1),
  tar_tiem       VARCHAR2(5) default '00:00',
  ant_tiem       VARCHAR2(5) default '00:00',
  con_1          NUMBER,
  con_2          NUMBER,
  con_3          NUMBER,
  con_4          NUMBER,
  con_5          NUMBER,
  con_6          NUMBER,
  cant_con_1     VARCHAR2(5) default '00:00',
  cant_con_2     VARCHAR2(5) default '00:00',
  cant_con_3     VARCHAR2(5) default '00:00',
  cant_con_4     VARCHAR2(5) default '00:00',
  cant_con_5     VARCHAR2(5) default '00:00',
  cant_con_6     VARCHAR2(5) default '00:00',
  min_con_1      VARCHAR2(5) default '00:00',
  min_con_2      VARCHAR2(5) default '00:00',
  min_con_3      VARCHAR2(5) default '00:00',
  min_con_4      VARCHAR2(5) default '00:00',
  min_con_5      VARCHAR2(5) default '00:00',
  min_con_6      VARCHAR2(5) default '00:00',
  tipo           CHAR(1) default 'R',
  tipo_jor       CHAR(1) default 'D',
  fuerza_calc    CHAR(1) default 'F',
  idagrupamiento NUMBER,
  apl_trans1     NUMBER,
  id_trans1      NUMBER,
  apl_trans2     NUMBER,
  id_trans2      NUMBER,
  apl_trans3     NUMBER,
  id_trans3      NUMBER,
  apl_trans4     NUMBER,
  id_trans4      NUMBER,
  apl_trans5     NUMBER,
  id_trans5      NUMBER,
  apl_trans6     NUMBER,
  id_trans6      NUMBER,
  apl_ben1       NUMBER,
  id_ben1        NUMBER,
  apl_ben2       NUMBER,
  id_ben2        NUMBER,
  apl_ben3       NUMBER,
  id_ben3        NUMBER,
  apl_ben4       NUMBER,
  id_ben4        NUMBER,
  apl_ben5       NUMBER,
  id_ben5        NUMBER,
  apl_ben6       NUMBER,
  id_ben6        NUMBER,
  apl__trans_post       NUMBER,
  id__trans_post       NUMBER,
  conc_ben1      NUMBER,
  conc_ben2      NUMBER,
  conc_ben3      NUMBER,
  conc_ben4      NUMBER,
  conc_ben5      NUMBER,
  conc_ben6      NUMBER,
  apl_trans_post NUMBER,
  id_trans_post NUMBER,
  apl_redond_entrada CHAR(1) default 'F',
  cant_redond_entrada VARCHAR2(5) default '00:00'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_TURNOS
  add constraint PK_IDTURNO primary key (IDTURNO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_TURNOS
  add constraint PH_TURNO_CONCEPTO foreign key (CON_1)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.PH_TURNOS
  add constraint PH_TURNO_CONCEPTO1 foreign key (CON_2)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.PH_TURNOS
  add constraint PH_TURNO_CONCEPTO2 foreign key (CON_3)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.PH_TURNOS
  add constraint PH_TURNO_CONCEPTO3 foreign key (CON_4)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.PH_TURNOS
  add constraint PH_TURNO_CONCEPTO4 foreign key (CON_5)
  references GEOTIME.PH_CONCEPTOS (ID);

--
-- Creating table PH_DESCANSOS_TURNOS
-- ==================================
--
create table GEOTIME.PH_DESCANSOS_TURNOS
(
  idturno   NUMBER not null,
  idtiempo  NUMBER not null,
  inicio    VARCHAR2(5) not null,
  fin       VARCHAR2(5) not null,
  tiempo    VARCHAR2(5) not null,
  descuenta CHAR(1),
  tiemp_ext CHAR(1) default 'F'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_DESCANSOS_TURNOS
  add constraint PK_DESC_TUR primary key (IDTURNO, IDTIEMPO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_DESCANSOS_TURNOS
  add constraint FK_TURNO foreign key (IDTURNO)
  references GEOTIME.PH_TURNOS (IDTURNO) on delete cascade;

--
-- Creating table PH_DISTRIBUCIONES_CCOSTO
-- =======================================
--
create table GEOTIME.PH_DISTRIBUCIONES_CCOSTO
(
  idregistro  NUMBER not null,
  codigo      VARCHAR2(25) not null,
  descripcion VARCHAR2(60) not null,
  idccosto    VARCHAR2(25) not null,
  proyecto    VARCHAR2(25) default 'F' null,
  fase    VARCHAR2(25) default 'F' null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_DISTRIBUCIONES_CCOSTO
  add constraint PK_DISTCC primary key (IDREGISTRO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_DISTRIBUCIONES_CCOSTO
  add constraint FK_DIST_CCO foreign key (IDCCOSTO)
  references GEOTIME.PH_CCOSTOS (IDCCOSTO);

--
-- Creating table PH_FORMULACION
-- =============================
--
create table GEOTIME.PH_FORMULACION
(
  id          NUMBER not null,
  descripcion VARCHAR2(20) not null,
  formula     VARCHAR2(1000) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_FORMULACION
  add constraint PK_FORMULACION primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_HORARIO_TURNO
-- ===============================
--
create table GEOTIME.PH_HORARIO_TURNO
(
  idhorario NUMBER not null,
  id_dia    NUMBER not null,
  t_1       NUMBER not null,
  t_2       NUMBER not null,
  t_3       NUMBER not null,
  t_4       NUMBER not null,
  t_5       NUMBER not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_HORARIO_TURNO
  add constraint PK_HOR_TUR primary key (IDHORARIO, ID_DIA)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_HORARIO_TURNO
  add constraint FK_HORARIO_HORARIO foreign key (IDHORARIO)
  references GEOTIME.PH_HORARIOS (IDHORARIO);

--
-- Creating table PH_NIVELES
-- =========================
--
create table GEOTIME.PH_NIVELES
(
  idnivel     NUMBER not null,
  descripcion VARCHAR2(80) not null,
  variables   CLOB not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_NIVELES
  add constraint PK_IDNIVEL primary key (IDNIVEL)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_PERIODOS
-- ==========================
--
create table GEOTIME.PH_PERIODOS
(
  idperiodo     VARCHAR2(8) not null,
  tipo_planilla CHAR(1) not null,
  inicio        DATE not null,
  fin           DATE not null,
  estado        CHAR(1),
  inicio_proy   DATE,
  fin_proy      DATE,
  periodo_proy  VARCHAR2(8),
  dia_inicio    Number DEFAULT 0 null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_PERIODOS
  add constraint PK_IDPERIODO primary key (IDPERIODO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_ROLES
-- =======================
--
create table GEOTIME.PH_ROLES
(
  idrol       NUMBER not null,
  descripcion VARCHAR2(60)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_ROLES
  add constraint PH_ROLES primary key (IDROL)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table PH_ROLES_TURNOS
-- ==============================
--
create table GEOTIME.PH_ROLES_TURNOS
(
  idregistro NUMBER not null,
  idrol      NUMBER not null,
  idturno    NUMBER not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index GEOTIME.IX_ROL on GEOTIME.PH_ROLES_TURNOS (IDROL)
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_ROLES_TURNOS
  add constraint PH_ROLTURNO primary key (IDREGISTRO, IDROL)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_ROLES_TURNOS
  add constraint PK_ROLTURNO foreign key (IDTURNO)
  references GEOTIME.PH_TURNOS (IDTURNO);

--
-- Creating table PH_SISTEMA
-- =========================
--
create table GEOTIME.PH_SISTEMA
(
  data_01    VARCHAR2(1024) not null,
  data_02    VARCHAR2(1024) not null,
  data_03    VARCHAR2(1024) not null,
  ver_db     VARCHAR2(5),
  post_emp   CHAR(1) not null,
  post_sinc  CHAR(1) not null,
  num_alm    NUMBER not null,
  f_install  VARCHAR2(1024),
  f_contrato VARCHAR2(1024),
  f_control  VARCHAR2(1024),
  data_04    VARCHAR2(1024)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table RELOJES_DISPOSITIVO
-- ==================================
--
create table GEOTIME.RELOJES_DISPOSITIVO
(
  clock_id           NUMBER not null,
  clock_description  VARCHAR2(40),
  clock_ip           VARCHAR2(15),
  clock_port         NUMBER,
  clock_state        VARCHAR2(10),
  clock_comm         CHAR(1),
  clock_commid       NUMBER,
  site_id            NUMBER not null,
  clock_type         VARCHAR2(4),
  clock_use_function CHAR(1),
  clock_function     VARCHAR2(2),
  clock_pass         VARCHAR2(4),
  borro_m            CHAR(1),
  idlocacion         NUMBER,
  FG_MODEL           VARCHAR2(80),
  usa_face           char(1)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table RELOJES_SITIO
-- ============================
--
create table GEOTIME.RELOJES_SITIO
(
  id                NUMBER not null,
  description       VARCHAR2(40),
  estado            VARCHAR2(10),
  conexion_primaria CHAR(1),
  bbackup           VARCHAR2(120),
  dbconnector       VARCHAR2(300),
  dbuser            VARCHAR2(50),
  dbpass            VARCHAR2(50),
  archivo           VARCHAR2(120),
  usa_primario      CHAR(1)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.RELOJES_SITIO
  add constraint PK_IDSITIO primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table RELOJ_TEMPLATES
-- ==============================
--
create table GEOTIME.RELOJ_TEMPLATES
(
  fp_enrollid VARCHAR2(16) not null,
  fp_indexid  NUMBER,
  fp_template VARCHAR2(4000) not null,
  fp_length   NUMBER,
  fp_user     VARCHAR2(16),
  fp_sec      NUMBER,
  iduser      NUMBER
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table RELOJ_USUARIO
-- ============================
--
create table GEOTIME.RELOJ_USUARIO
(
  fp_enrollid   NUMBER,
  fp_username   VARCHAR2(20),
  fp_userpass   VARCHAR2(20),
  fp_privilegio CHAR(1) not null,
  fp_estado     CHAR(1) not null,
  fp_tarjeta    VARCHAR2(50),
  face          VARCHAR2(4000),
  face_leng     NUMBER
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table REPORTES
-- =======================
--
create table GEOTIME.REPORTES
(
  idreporte   NUMBER not null,
  descripcion VARCHAR2(50) not null,
  ruta        VARCHAR2(300) not null,
  idmenu      NUMBER not null,
  estado      CHAR(1) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.REPORTES
  add constraint PK_REPORTE primary key (IDREPORTE)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table REPORTES_MENU
-- ============================
--
create table GEOTIME.REPORTES_MENU
(
  id          NUMBER not null,
  descripcion VARCHAR2(50) not null,
  estado      CHAR(1) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.REPORTES_MENU
  add constraint PK_MREPORTE primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table TIPO_AUSENCIA
-- ============================
--
create table GEOTIME.TIPO_AUSENCIA
(
  id          NUMBER not null,
  descripcion VARCHAR2(40) not null,
  tipo        VARCHAR2(4) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.TIPO_AUSENCIA
  add constraint PK_TIPOAUS primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table TIPOS_PLANILLA
-- =============================
--
create table GEOTIME.TIPOS_PLANILLA
(
  tipo_planilla CHAR(1) not null,
  planilla      VARCHAR2(10) not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.TIPOS_PLANILLA
  add constraint PK_TIPOPLAN primary key (TIPO_PLANILLA)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table TRANSFORMACIONES
-- ===============================
--
create table GEOTIME.TRANSFORMACIONES
(
  id             NUMBER not null,
  descripcion    VARCHAR2(40) not null,
  idconcepto_1   NUMBER not null,
  idconcepto_2   NUMBER not null,
  idconcepto_3   NUMBER not null,
  idconcepto_4   NUMBER not null,
  idconcepto_5   NUMBER not null,
  idconcepto_6   NUMBER not null,
  idconcepto_7   NUMBER not null,
  calculadas     CHAR(1) default 'F' not null,
  hrs_concepto_1 VARCHAR2(5) default '00:00' not null,
  hrs_concepto_2 VARCHAR2(5) default '00:00' not null,
  hrs_concepto_3 VARCHAR2(5) default '00:00' not null,
  hrs_concepto_4 VARCHAR2(5) default '00:00' not null,
  hrs_concepto_5 VARCHAR2(5) default '00:00' not null,
  hrs_concepto_6 VARCHAR2(5) default '00:00' not null,
  hrs_concepto_7 VARCHAR2(5) default '00:00' not null,
  min_concepto_1 VARCHAR2(5) default '00:00' not null,
  min_concepto_2 VARCHAR2(5) default '00:00' not null,
  min_concepto_3 VARCHAR2(5) default '00:00' not null,
  min_concepto_4 VARCHAR2(5) default '00:00' not null,
  min_concepto_5 VARCHAR2(5) default '00:00' not null,
  min_concepto_6 VARCHAR2(5) default '00:00' not null,
  min_concepto_7 VARCHAR2(5) default '00:00' not null,
  turno_continuo CHAR(1) default 'F',
  usa_trans_ant CHAR(1) default 'F'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.TRANSFORMACIONES
  add constraint PK_TRANSF primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC1 foreign key (IDCONCEPTO_1)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC2 foreign key (IDCONCEPTO_2)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC3 foreign key (IDCONCEPTO_3)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC4 foreign key (IDCONCEPTO_4)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC5 foreign key (IDCONCEPTO_5)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC6 foreign key (IDCONCEPTO_6)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.TRANSFORMACIONES
  add constraint FK_IDC7 foreign key (IDCONCEPTO_7)
  references GEOTIME.PH_CONCEPTOS (ID);

--
-- Creating table TRANSFORMACIONES_GLOBALES
-- ========================================
--
create table GEOTIME.TRANSFORMACIONES_GLOBALES
(
  id               NUMBER not null,
  id_orden         NUMBER not null,
  descripcion      VARCHAR2(50) not null,
  idplanilla       VARCHAR2(8) not null,
  estado           CHAR(1) default 'T' not null,
  formula_apl      NUMBER not null,
  formula_hrs      NUMBER not null,
  formula_concepto NUMBER not null
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.TRANSFORMACIONES_GLOBALES
  add constraint PK_TRANSF_GLOB primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

--
-- Creating table TRANSFORMACIONES_TURNOS
-- ======================================
--
create table GEOTIME.TRANSFORMACIONES_TURNOS
(
  id           NUMBER not null,
  idtrans      NUMBER not null,
  turno_orig   NUMBER not null,
  turno_cambio NUMBER not null,
  descripcion  VARCHAR2(20)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.TRANSFORMACIONES_TURNOS
  add constraint PK_TRANS_TURNO primary key (ID)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
--
-- Creating table PH_USUARIO
-- ======================================
--
create table GEOTIME.PH_USUARIO
(
  idusuario NUMBER not null,
  planillas VARCHAR2(4000),
  nivel     NUMBER,
  grupos    VARCHAR2(4000),
  estado    CHAR(1) default 'F',
  fperiodo  VARCHAR2(8),
  fplanilla VARCHAR2(8),
  turnos    VARCHAR2(4000),
  orden_emp CHAR(1) default 'A',
  tipo_edt CHAR(1) default 'D',
  nivel_aprob_ext Number default 0 Null,
  pt_agrup CHAR(1)
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_USUARIO
  add constraint PK_IDUSUARIOC primary key (IDUSUARIO)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create table GEOTIME.PH_OPCIONES
(
  idopcion number default 1,
  post_emp  CHAR(1) default 'F',
  post_sinc CHAR(1) default 'F',
  num_alm   NUMBER default 1,
  utiliza_desc CHAR(1) default 'F',
  desc_abiert CHAR(1) default 'F',
  ver_db varchar2(5) NOT NULL,
  corte_diurno varchar2(5)  default '00:00' NOT NULL,
  corte_nocturno varchar2(5) default '00:00' NOT NULL,
  usa_alerta_exd char(1) default 'F',
  alerta_extras_diarias varchar2(5) default '00:00',
  color_fondo_alertd varchar2(50) default '255,255,255',
  color_fuente_alertd varchar2(50) default '0,0,0',
  dist_tadic char(1) default 'F',
  dist_lic_usr varchar2(1024),
  dist_lic_emp varchar2(1024),
  tipo_dist char(1) DEFAULT 'C',
  acc_bloc_pt char(1) DEFAULT 'F'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

  create table GEOTIME.ph_transformacion
(
  id_transformacion number not null,
  descripcion       varchar2(60) not null,
  usr_transf        char(1) default 'F'
)
  tablespace TBGEOTIME
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

  alter table GEOTIME.ph_transformacion
  add constraint PK_ph_transformacion primary key (ID_TRANSFORMACION);

  create table GEOTIME.PH_TRANSFORMACION_CONCEPTOS
(
  id_transformacion NUMBER not null,
  prioridad         NUMBER not null,
  concepto_orig     NUMBER not null,
  concepto_cambio   NUMBER not null,
  usr_transf        varchar2(1) default 'F'
)
tablespace TBGEOTIME
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

alter table GEOTIME.PH_TRANSFORMACION_CONCEPTOS
  add constraint PK_PH_TRANSFORMACION_CONCEPTOS primary key (ID_TRANSFORMACION, PRIORIDAD)
  using index 
  tablespace TBGEOTIME
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table GEOTIME.PH_TRANSFORMACION_CONCEPTOS
  add constraint FK_TRANSFORMACION foreign key (ID_TRANSFORMACION)
  references GEOTIME.PH_TRANSFORMACION (ID_TRANSFORMACION);
alter table GEOTIME.PH_TRANSFORMACION_CONCEPTOS
  add constraint FK_TRANSF_PH_CONCEPTOS_CAMBIO foreign key (CONCEPTO_CAMBIO)
  references GEOTIME.PH_CONCEPTOS (ID);
alter table GEOTIME.PH_TRANSFORMACION_CONCEPTOS
  add constraint FK_TRANSF_PH_CONCEPTOS_ORIG foreign key (CONCEPTO_ORIG)
  references GEOTIME.PH_CONCEPTOS (ID);



