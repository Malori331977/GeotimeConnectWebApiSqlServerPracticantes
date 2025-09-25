
CREATE TABLE [ctadmin].[ph_companias](
	[idcomp] [varchar](10) NOT NULL,
	[compania] [varchar](60) NOT NULL,
	[nom_conector] [varchar](10) NULL,
	[STRING_SQL] [varchar](4000) NULL,
	[STRING_SQL_ERP] [varchar](4000) NULL,
	[PAIS] [varchar](5) NULL,
	[auto_proceso] [char](1) NULL,
	[remote_erpservice] [varchar](4000) NULL,
	[mail_server] [varchar](400) NULL,
	[mail_user] [varchar](400) NULL,
	[mail_password] [varchar](400) NULL,
	[mail_port] [int] NULL,
	[mail_auth] [char](1) NULL,
	[mail_ssl] [char](1) NULL,
	[hora_sup] [varchar](5) NULL,
	[hora_emp] [varchar](5) NULL,
	[supervisor_acum] [char](1) NULL,
	[mail_tls] [char](1) NULL,
	[in_marcas] [char](1) NULL,
	[hora_calc] [varchar](5) NULL,
	[ApiClientId] [varchar](100) NULL,
	[ApiPassword] [varchar](100) NULL,
	[ApiUser] [varchar](20) NULL,
	[ApiDataBase] [varchar](50) NOT NULL,
	[ApiUrl] [varchar](1000) NOT NULL,
	[PROCESO_DIST_MARCAS] [char](1) NULL,
	[PROCESO_DIST_MARCAS_EMP] [char](1) NULL,
 CONSTRAINT [PK_ph_companias] PRIMARY KEY CLUSTERED 
(
	[idcomp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_login]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_login](
	[idusuario] [int] IDENTITY(1,1) NOT NULL,
	[usuario] [varchar](100) NOT NULL,
	[clave] [varchar](250) NOT NULL,
	[descripcion] [varchar](150) NOT NULL,
	[privilegio] [int] NULL,
	[grupo_base] [int] NULL,
	[idsesion] [bigint] NULL,
	[ultimo_login] [datetime] NULL,
	[fcomp] [varchar](10) NULL,
	[fplani] [varchar](8) NULL,
	[fperiodo] [varchar](8) NULL,
	[grupos] [varchar](max) NULL,
	[proceso] [int] NULL,
	[ultimo_estado] [datetime] NULL,
	[companias] [varchar](400) NULL,
	[planillas] [varchar](400) NULL,
	[idioma] [varchar](20) NULL,
	[stat_msg] [text] NULL,
	[turnos] [varchar](4096) NULL,
	[usa_wusuario] [char](1) NULL,
	[ESTADO] [char](1) NOT NULL,
	[email] [varchar](200) NULL,
	[omite_lic] [char](1) NULL,
	[global_clave] [varchar](400) NULL,
	[ACTIVO] [char](1) NULL,
	[RolUsuario] [varchar](100) NULL,
 CONSTRAINT [PK_ph_login] PRIMARY KEY CLUSTERED 
(
	[idusuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_menus_sistema]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_menus_sistema](
	[Id] [varchar](4) NOT NULL,
	[MenuText] [varchar](50) NOT NULL,
	[Href] [varchar](255) NULL,
	[IconId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_movile]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_movile](
	[id_disp] [int] IDENTITY(1,1) NOT NULL,
	[dispositivo] [varchar](400) NOT NULL,
	[descripcion] [varchar](60) NULL,
	[estado] [char](1) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_opciones_sistema]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_opciones_sistema](
	[Id] [varchar](4) NOT NULL,
	[Principal] [bit] NOT NULL,
	[Href] [varchar](255) NULL,
	[IconId] [varchar](50) NOT NULL,
	[MenuText] [varchar](50) NOT NULL,
	[ParentId] [varchar](4) NULL,
 CONSTRAINT [PK_ph_opciones_sistema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_roles_sistema]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_roles_sistema](
	[Id] [varchar](4) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Habilitado] [bit] NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_roles_sistemadet]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_roles_sistemadet](
	[RolSistemaId] [varchar](4) NOT NULL,
	[MenuSistemaId] [varchar](4) NOT NULL,
	[OpcionSistemaId] [varchar](4) NOT NULL,
	[Habilitado] [bit] NOT NULL,
	[Agrega] [bit] NOT NULL,
	[Modifica] [bit] NOT NULL,
	[Elimina] [bit] NOT NULL,
	[Consulta] [bit] NOT NULL,
 CONSTRAINT [PK_PhRolesSistemaDet] PRIMARY KEY CLUSTERED 
(
	[RolSistemaId] ASC,
	[MenuSistemaId] ASC,
	[OpcionSistemaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[ph_sistema]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[ph_sistema](
	[data_01] [varchar](1024) NOT NULL,
	[data_02] [varchar](1024) NOT NULL,
	[data_03] [varchar](1024) NOT NULL,
	[ver_db] [varchar](5) NULL,
	[post_emp] [char](1) NOT NULL,
	[post_sinc] [char](1) NOT NULL,
	[num_alm] [int] NOT NULL,
	[f_install] [varchar](1024) NULL,
	[f_contrato] [varchar](1024) NULL,
	[f_control] [varchar](1024) NULL,
	[data_04] [varchar](1024) NULL,
	[data_05] [varchar](1024) NULL,
	[dist_lic_usr] [char](1) NULL,
	[dist_lic_emp] [char](1) NULL,
	[dist_lic] [char](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[Ph_Usuarios_Roles]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[Ph_Usuarios_Roles](
	[IdUsuario] [int] NOT NULL,
	[IdRegistro] [int] IDENTITY(1,1) NOT NULL,
	[Rol] [varchar](255) NOT NULL,
	[IdUsuarioRegistra] [int] NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
	[IdUsuarioModifica] [int] NOT NULL,
	[FechaModifica] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PhUsuariosRoles] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC,
	[IdRegistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[Portal_Config]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[Portal_Config](
	[IdAplicacion] [varchar](30) NOT NULL,
	[IdVersion] [varchar](10) NOT NULL,
	[Compania] [varchar](100) NOT NULL,
	[BaseDatos] [varchar](50) NOT NULL,
	[IdLicencia] [varchar](1000) NOT NULL,
	[Activa] [bit] NOT NULL,
	[UsoRestringido] [bit] NOT NULL,
	[RegsitroLic] [varchar](20) NOT NULL,
	[Permanente] [bit] NOT NULL,
	[UsaReconocimientoFacial] [bit] NOT NULL,
	[FechaUltModifica] [datetime2](7) NOT NULL,
	[IdUsuarioModifica] [varchar](20) NOT NULL,
	[USARGEOLOCALIZACION] [bit] NOT NULL,
	[MAPAPIKEY] [varchar](64) NULL,
	[FACEDIST] [decimal](4, 2) NOT NULL,
	[FACETEXT] [decimal](4, 2) NOT NULL,
	[VerLogMarcas] [bit] NOT NULL,
 CONSTRAINT [PK_portal_config] PRIMARY KEY CLUSTERED 
(
	[IdAplicacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ctadmin].[relojes_dispositivo]    Script Date: 28-08-2025 5:57:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ctadmin].[relojes_dispositivo](
	[CLOCK_ID] [int] IDENTITY(1,1) NOT NULL,
	[CLOCK_DESCRIPTION] [varchar](40) NULL,
	[CLOCK_IP] [varchar](15) NULL,
	[CLOCK_PORT] [int] NULL,
	[CLOCK_STATE] [varchar](10) NULL,
	[CLOCK_COMM] [char](1) NULL,
	[CLOCK_COMMID] [smallint] NULL,
	[SITE_ID] [int] NOT NULL,
	[CLOCK_TYPE] [varchar](4) NULL,
	[CLOCK_USE_FUNCTION] [char](1) NULL,
	[CLOCK_FUNCTION] [varchar](2) NULL,
	[clock_pass] [varchar](4) NULL,
	[borro_m] [char](1) NULL,
	[idlocacion] [int] NULL,
	[FG_MODEL] [varchar](80) NULL,
	[usa_face] [char](1) NOT NULL,
	[usuario_hik] [varchar](200) NULL,
	[password_hik] [varchar](200) NULL,
	[IDCOMP] [varchar](10) NULL,
	[CLOCK_SERIE] [varchar](50) NULL,
 CONSTRAINT [PK_relojes_dispositivo] PRIMARY KEY CLUSTERED 
(
	[CLOCK_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [ctadmin].[ph_catalogo_generico](
	[NombreCatalogo] [varchar](30) NOT NULL,
	[Id] [varchar](10) NOT NULL,
	[Descripcion] [varchar](255) NULL,
	[UsuarioCreacion] [varchar](25) NULL,
	[FechaCreacion] [datetime2](7) NULL,
	[UsuarioUltModificacion] [varchar](25) NULL,
	[FechaUltModificacion] [datetime2](7) NULL,
 CONSTRAINT [PKCATALOGO_GENERICO] PRIMARY KEY CLUSTERED 
(
	[NombreCatalogo] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('F') FOR [auto_proceso]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('F') FOR [supervisor_acum]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('F') FOR [mail_tls]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('F') FOR [in_marcas]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('') FOR [ApiDataBase]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('') FOR [ApiUrl]
GO
ALTER TABLE [ctadmin].[ph_login] ADD  CONSTRAINT [DF__ph_login__idioma__3AA1AEB8]  DEFAULT ('esp') FOR [idioma]
GO
ALTER TABLE [ctadmin].[ph_login] ADD  DEFAULT ('F') FOR [usa_wusuario]
GO
ALTER TABLE [ctadmin].[ph_login] ADD  DEFAULT ('A') FOR [ESTADO]
GO
ALTER TABLE [ctadmin].[ph_login] ADD  DEFAULT ('F') FOR [omite_lic]
GO
ALTER TABLE [ctadmin].[ph_movile] ADD  CONSTRAINT [DF_ph_movile_estado]  DEFAULT ('T') FOR [estado]
GO
ALTER TABLE [ctadmin].[ph_sistema] ADD  DEFAULT ('F') FOR [post_emp]
GO
ALTER TABLE [ctadmin].[ph_sistema] ADD  DEFAULT ('F') FOR [post_sinc]
GO
ALTER TABLE [ctadmin].[ph_sistema] ADD  DEFAULT ((3)) FOR [num_alm]
GO
ALTER TABLE [ctadmin].[ph_sistema] ADD  DEFAULT ('F') FOR [dist_lic_usr]
GO
ALTER TABLE [ctadmin].[ph_sistema] ADD  DEFAULT ('F') FOR [dist_lic_emp]
GO
ALTER TABLE [ctadmin].[ph_sistema] ADD  DEFAULT ('F') FOR [dist_lic]
GO
ALTER TABLE [ctadmin].[Portal_Config] ADD  DEFAULT ((0)) FOR [USARGEOLOCALIZACION]
GO
ALTER TABLE [ctadmin].[Portal_Config] ADD  DEFAULT ((0)) FOR [FACEDIST]
GO
ALTER TABLE [ctadmin].[Portal_Config] ADD  DEFAULT ((0)) FOR [FACETEXT]
GO
ALTER TABLE [ctadmin].[Portal_Config] ADD  DEFAULT ((0)) FOR [VerLogMarcas]
GO
ALTER TABLE [ctadmin].[relojes_dispositivo] ADD  CONSTRAINT [DF_TAC_CLOCK_clock_pass]  DEFAULT ('0000') FOR [clock_pass]
GO
ALTER TABLE [ctadmin].[ph_opciones_sistema]  WITH CHECK ADD  CONSTRAINT [FK_phOpcionesmenusistema] FOREIGN KEY([ParentId])
REFERENCES [ctadmin].[ph_menus_sistema] ([Id])
GO
ALTER TABLE [ctadmin].[ph_opciones_sistema] CHECK CONSTRAINT [FK_phOpcionesmenusistema]
GO
ALTER TABLE [ctadmin].[ph_roles_sistemadet]  WITH CHECK ADD  CONSTRAINT [FK_PhRolesSistemaDet_RolesSistema] FOREIGN KEY([RolSistemaId])
REFERENCES [ctadmin].[ph_roles_sistema] ([Id])
GO
ALTER TABLE [ctadmin].[ph_roles_sistemadet] CHECK CONSTRAINT [FK_PhRolesSistemaDet_RolesSistema]
GO