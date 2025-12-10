SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter table [ctadmin].[ph_login] add RolUsuario varchar(100) null
GO
alter table [ctadmin].[ph_login] add [email] varchar(200) null
GO
alter table [ctadmin].[ph_login] add [global_clave] varchar(400) null
go
alter table [ctadmin].[ph_login] add [estado] char(1) not null default('A') 
go
alter table CTADMIN.ph_login add omite_lic char(1) null default('F')
go
alter table CTADMIN.ph_login add activo char(1) not null default('T')
go
alter table [ctadmin].[ph_companias] add [ApiClientId] varchar(100) null
GO
alter table [ctadmin].[ph_companias] add [ApiPassword] varchar(100) null
GO
alter table [ctadmin].[ph_companias] add [ApiUser] varchar(25) null
GO
alter table [ctadmin].[ph_companias] add [ApiDataBase] varchar(50) null
GO
alter table [ctadmin].[ph_companias] add [ApiUrl] varchar(1000) null
GO
ALTER TABLE [ctadmin].[ph_companias] ADD [IN_MARCAS] CHAR(1) NULL
go
alter table [CTADMIN].[ph_companias] add [HORA_CALC] [varchar](5) NULL
go
alter table [CTADMIN].[ph_companias] add [MAIL_TLS]  [char](1) NULL
go
alter table [CTADMIN].[ph_companias] add [IN_MARCAS] [char](1) NULL
go
alter table [CTADMIN].[ph_companias] add [PROCESO_DIST_MARCAS] [char](1) NULL
go
alter table [CTADMIN].[ph_companias] add [PROCESO_DIST_MARCAS_EMP] [char](1) NULL
go
alter table CTADMIN.ph_companias add remote_erpservice varchar(4000) null
go
alter table CTADMIN.ph_companias add auto_proceso char(1) null default('F')
go
alter table CTADMIN.ph_companias add mail_server varchar(400) null
go
alter table CTADMIN.ph_companias add mail_user varchar(400) null
go
alter table CTADMIN.ph_companias add mail_password varchar(400) null
go
alter table CTADMIN.ph_companias add mail_port int null
go
alter table CTADMIN.ph_companias add mail_auth char(1) null default('F')
go
alter table CTADMIN.ph_companias add mail_ssl char(1) null default('F')
go
alter table CTADMIN.ph_companias add hora_sup varchar(5) null
go
alter table CTADMIN.ph_companias add hora_emp varchar(5) null
go
alter table CTADMIN.ph_companias add supervisor_acum char(1) null default('F')
go
alter table ctadmin.ph_companias add ZonaHoraria int not null default (1)
go
alter table ctadmin.ph_sistema add dist_lic char(1) null default('F')
go
alter table ctadmin.ph_sistema add dist_lic_emp char(1) null default('F')
go
alter table ctadmin.ph_sistema add dist_lic_usr char(1) null default('F')
go
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
alter table [ctadmin].[ph_roles_sistema] add [IdComp] varchar(10) null
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
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('') FOR [ApiDataBase]
GO
ALTER TABLE [ctadmin].[ph_companias] ADD  DEFAULT ('') FOR [ApiUrl]
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
alter table [ctadmin].[Portal_Config] add AutoRegistroRostro bit not null default 0
GO
alter table [ctadmin].[Portal_Config] add CantMaxPlantillas int not null default 5
GO
alter table [ctadmin].[ph_roles_sistema] add IdComp varchar(10) null
go
alter table [ctadmin].[ph_roles_sistema] add idnivel int null