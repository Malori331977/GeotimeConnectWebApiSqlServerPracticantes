
create  FUNCTION [dbo].[tipo] 
(
	@idplanilla varchar(8), @idnumero varchar(20), @fecha datetime, @hora varchar(5)
)
RETURNS int
AS
BEGIN
	  
	  declare @horam varchar(5)
	  
	  set @horam = (select MIN(hora) from marcas where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha)
	  if @hora = @horam return 1
	  
	  set @horam = (select MAX(hora) from marcas where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha)
	  if @hora = @horam return 2
	  
	  return 3

END
GO
ALTER AUTHORIZATION ON [dbo].[tipo] TO  SCHEMA OWNER 
GO


CREATE TABLE [dbo].[PaletaColores](
       [ColorId] [int] IDENTITY(1,1) NOT NULL,
       [Descripcion] [varchar](30) NULL,
       [ColorFondo] [varchar](7) NOT NULL DEFAULT ('#FFFFFF'),
       [ColorFuente] [varchar](7) NOT NULL DEFAULT ('#000000'),
CONSTRAINT [PK_PaletaColores] PRIMARY KEY CLUSTERED
(
       [ColorId] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ph_opciones]    Script Date: 31/05/2017 08:08:53 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ph_opciones](
    [idopcion] [int] NOT NULL DEFAULT (1),
	[post_emp] [char](1) NOT NULL DEFAULT ('F'),
	[post_sinc] [char](1) NOT NULL DEFAULT ('F'),
	[num_alm] [int] NOT NULL DEFAULT ((3)),
	[utiliza_desc] [char](1) NOT NULL DEFAULT ('F'),
	[desc_abiert] [char](1) NOT NULL DEFAULT ('F'),
	[ver_db] [varchar](5) NOT NULL,
	[corte_diurno] [varchar](5) NOT NULL DEFAULT ('00:00'),
	[corte_nocturno] [varchar](5) NOT NULL DEFAULT ('00:00'),
	[usa_alerta_exd] [char](1) NULL DEFAULT ('F'),
	[alerta_extras_diarias] [varchar](5) NULL DEFAULT ('00:00'),
	[color_fondo_alertd] [varchar](50) NULL DEFAULT ('255,255,255'),
	[color_fuente_alertd] [varchar](50) NULL DEFAULT ('0,0,0'),
	[dist_tadic] [char](1) NULL DEFAULT('F'),
	[dist_lic_usr] [varchar](1024) NULL,
	[dist_lic_emp] [varchar](1024) NULL,
	[tipo_dist] [char](1) NULL DEFAULT('C'),
	[acc_bloc_pt][char](1) NULL DEFAULT('F'),

) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_opciones] TO  SCHEMA OWNER
/****** Object:  Table [dbo].[acciones_personal]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[acciones_personal](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[inicio] [datetime] NOT NULL,
	[fin] [datetime] NOT NULL,
	[idincidencia] [int] NOT NULL,
	[estado] [char](1) NULL,
	[idaccion] [int] NULL,
	[comentario] [varchar](2048) NULL,
	[dias] [int] NULL,
	[usuario] [varchar](100) NULL,
	[fecha_just] [datetime] NULL,
	[dias_apl] [varchar](15) NULL,
 CONSTRAINT [PK_acciones_personal] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[acciones_personal] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[empleados]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[empleados](
	[idnumero] [varchar](20) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[nombre] [varchar](70) NOT NULL,
	[tarjeta] [varchar](16) NOT NULL,
	[identificacion] [varchar](20) NULL,
	[idgrupo] [int] NOT NULL,
	[iddepartamento] [varchar](15) NOT NULL,
	[idhorario] [int] NOT NULL,
	[estado] [char](1) NULL,
	[idagrupamiento] [int] NOT NULL,
	[foto] [image] NULL,
	[idccosto] [varchar](25) NOT NULL,
	[exporta] [char](1) NULL,
	[ubicacion] [varchar](8) NULL,
	[rubro1] [varchar](40) NULL,
	[rubro2] [varchar](40) NULL,
	[rubro3] [varchar](40) NULL,
	[rubro4] [varchar](40) NULL,
	[rubro5] [varchar](40) NULL,
	[rubro6] [varchar](40) NULL,
	[rubro7] [varchar](40) NULL,
	[rubro8] [varchar](40) NULL,
	[rubro9] [varchar](40) NULL,
	[rubro10] [varchar](40) NULL,
	[rubro11] [varchar](40) NULL,
	[rubro12] [varchar](40) NULL,
	[rubro13] [varchar](40) NULL,
	[rubro14] [varchar](40) NULL,
	[rubro15] [varchar](40) NULL,
	[rubro16] [varchar](40) NULL,
	[rubro17] [varchar](40) NULL,
	[rubro18] [varchar](40) NULL,
	[rubro19] [varchar](40) NULL,
	[rubro20] [varchar](40) NULL,
	[rubro21] [varchar](40) NULL,
	[rubro22] [varchar](40) NULL,
	[rubro23] [varchar](40) NULL,
	[rubro24] [varchar](40) NULL,
	[rubro25] [varchar](40) NULL,
	[fecha_ingreso] [datetime] NULL,
	[email] [varchar](80) NULL,
	[tipo_marca] [char](1) NOT NULL,
	[inicio_rol] [datetime] NULL,
	[web_pass] [varchar](400) NULL,
	[id_transfo_conc] [int] null default(0),
	[widioma][varchar](5) default('esp'),
	[nit_empresa][varchar](255) NULL,
	[global_code][varchar](10) NULL,
	[fecha_act_code][datetime] NULL,
 CONSTRAINT [PK_empleados] PRIMARY KEY CLUSTERED 
(
	[idnumero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[empleados] TO  SCHEMA OWNER 
GO

/****** Object:  Table [dbo].[ph_niveles]    Script Date: 29/05/2017 11:35:47 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO




CREATE TABLE [dbo].[ph_niveles](
	[idnivel] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](80) NOT NULL,
	[variables] [varchar](max) NOT NULL,
 CONSTRAINT [PK_ph_niveles] PRIMARY KEY CLUSTERED 
(
	[idnivel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER AUTHORIZATION ON [dbo].[ph_niveles] TO  SCHEMA OWNER 
GO

/****** Object:  Table [dbo].[ph_usuario]    Script Date: 16/06/2017 08:46:13 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ph_usuario](
	[idusuario] [int] NOT NULL,
	[planillas] [varchar](max) NULL,
	[nivel] [int] NULL,
	[grupos] [varchar](max) NULL,
	[estado] [char](1) NULL CONSTRAINT [DF_ph_usuario_estado]  DEFAULT ('F'),
	[fperiodo] [varchar](8) NULL,
	[fplanilla] [varchar](8) NULL,
	[turnos] [varchar](max) NULL,
	[orden_emp][char](1) NOT NULL DEFAULT('A'),
	[tipo_edt] [char](1) NOT NULL DEFAULT('D'),
	[nivel_aprob_ext] [int] Default(0),
	[pt_agrup][char](1) NULL,
 CONSTRAINT [PK_ph_usuario_planilla] PRIMARY KEY CLUSTERED 
(
	[idusuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ph_usuario]  WITH CHECK ADD  CONSTRAINT [FK_ph_usuario_ph_niveles] FOREIGN KEY([nivel])
REFERENCES [dbo].[ph_niveles] ([idnivel])
GO

ALTER TABLE [dbo].[ph_usuario] CHECK CONSTRAINT [FK_ph_usuario_ph_niveles]
GO

/****** Object:  Table [dbo].[incidencias]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[incidencias](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codigo] [varchar](5) NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[id_pago] [int] NULL,
	[nom_conector] [varchar](8) NULL,
	[tipo] [int] NULL,
	[ed_tiempo] [char](1) NULL,
	[requiere_accper] [char](1) NULL DEFAULT ('F')
 CONSTRAINT [PK_incidencias] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[incidencias] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[incidencias_conf_pago]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[incidencias_conf_pago](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](60) NOT NULL,
	[id_apl] [int] NOT NULL,
	[id_hrs] [int] NOT NULL,
	[id_con] [int] NOT NULL,
	[id_adicional] [int] NOT NULL,
	[apl_turno] [int] NULL,
	[tran_turno] [int] NULL,
 CONSTRAINT [PK_incidencias_conf_pago] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[incidencias_conf_pago] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas](
	[registro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] [varchar](5) NOT NULL,
	[tipo] [int] NOT NULL,
	[idterminal] [varchar](4) NOT NULL,
    [fecha_reg] [datetime] NULL,
	[long_reg] [varchar](max) NULL,
	[lat_reg] [varchar](max) NULL,
	[gps_cell] [char](1) null,
	[imagen_reg] [image] NULL,
	[img_verif] [varchar](5) null,
	[estado] [char](1) NULL,
 CONSTRAINT [PK_marcas] PRIMARY KEY CLUSTERED 
(
	[registro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_audit]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_audit](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[usuario] [varchar](100) NULL,
	[fecha_orig] [datetime] NULL,
	[fecha_chg] [datetime] NULL,
	[hora_orig] [varchar](5) NULL,
	[hora_chg] [varchar](5) NULL,
	[comentario] [varchar](4096) NULL,
 CONSTRAINT [PK_marcas_audit] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_audit] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_comedor]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_comedor](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] [varchar](5) NOT NULL,
	[tipo] [int] NULL,
	[idterminal] [varchar](4) NULL,
 CONSTRAINT [PK_marcas_comedor] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_comedor] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_descansos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_descansos](
	[idregistro] [int] IDENTITY(1,1) NOT NULL,
	[iddesc] [int] NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[inicio_desc] [varchar](5) NULL,
	[fin_desc] [varchar](5) NULL,
 CONSTRAINT [PK_marcas_descansos] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_descansos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_distribuciones]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_distribuciones](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[idconcepto] [int] NOT NULL,
	[nominaeq] [varchar](20) NOT NULL,
	[cantidad] [decimal](18, 2) NOT NULL,
	[idccosto] [varchar](25) NULL,
	[proyecto] [varchar](25) NULL default(''),
	[fase] [varchar](25) NULL default(''),
	[concepto] [varchar](6) NULL,
	[tipo] [char](1) NULL,
	[estado] [char](1) NULL,
	[entrada] [varchar](5) NOT NULL,
 CONSTRAINT [PK_marcas_distribuciones] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_distribuciones] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_distribuciones_conceptos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_distribuciones_conceptos](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[idccosto] [varchar](25) NOT NULL,
	[proyecto] [varchar](25) NULL,
	[fase] [varchar](25) NULL,
	[cantidad] [decimal](18, 2) NULL,
	[inicio] [varchar](5) NULL,
	[fin] [varchar](5) NULL,
	[estado] [char](1) NOT NULL,
	[iddist] [int] NOT NULL,
	[fecha_dist] [datetime] NOT NULL,
	[lon_reg] [varchar](max) NULL,
	[lat_reg] [varchar](max) NULL,
	[comentario][text] NULL,
 CONSTRAINT [PK_marcas_distribuciones_conceptos] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_distribuciones_conceptos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_proyecto]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_proyecto](
	[Proyecto] [varchar](25) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Centro_Costo] [varchar](25) NOT NULL
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ph_proyecto] ON [dbo].[ph_proyecto]
(
	[Proyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_proyecto] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_faseproyecto]    Script Date: 31/07/2017 11:01:02 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ph_faseproyecto](
	[Proyecto] [varchar](25) NOT NULL,
	[Fase] [varchar](25) NOT NULL,
	[Nombre] [varchar](80) NULL,
	[Descripcion] [varchar](400) NOT NULL,
	[Acepta_Datos] [char](1) NOT NULL,
 CONSTRAINT [PK_ph_faseproyecto] PRIMARY KEY CLUSTERED 
(
	[Proyecto] ASC,
	[Fase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ph_faseproyecto] ADD  CONSTRAINT [DF_ph_faseproyecto_Acepta_Datos]  DEFAULT ('N') FOR [Acepta_Datos]
GO
ALTER AUTHORIZATION ON [dbo].[ph_faseproyecto] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_extras_apb]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_extras_apb](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] [varchar](5) NOT NULL,
	[cantidad] [varchar](5) NOT NULL,
	[comentario] [varchar](1024) NULL,
	[estado] [char](1) NOT NULL,
	[usuario] [varchar](100) NULL,
	[ccosto] [varchar] (25) NULL,
 CONSTRAINT [PK_marcas_extras_apb] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_extras_apb] TO  SCHEMA OWNER 
GO
CREATE TABLE [dbo].[marcas_extras_detalle](
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] [varchar](5) NOT NULL,
	[cantidad] [varchar](5) NOT NULL,
	[cantidad_aprob] [varchar](5) NOT NULL DEFAULT('00:00'),
	[comentario] [varchar](1024) NULL,
	[estado] [char](1) NOT NULL DEFAULT('A'),
	[usuario] [varchar](100) NULL,
	[ccosto] [varchar](25) NULL,
	[idconcepto] [int] NULL
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_marcas_extras_detalle] ON [dbo].[marcas_extras_detalle]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC,
	[hora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_extras_detalle] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_in]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_in](
	[idtarjeta] [varchar](16) NULL,
	[fecha] [datetime] NULL,
	[hora] [varchar](5) NULL,
	[idterminal] [varchar](50) NULL,
	[tipo] [int] NULL,
	[idplanilla] [varchar](8) NULL,
	[idnumero] [varchar](20) NULL,
	[fecha_reg] [datetime] NULL,
	[long_reg] [varchar](max) NULL,
	[lat_reg] [varchar](max) NULL,
	[imagen_reg] [image] NULL,
	[gps_cell] [char](1) null,
	[estado] [char](1) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_in] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_incidencias]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_incidencias](
	[indice] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[idincidencia] [int] NOT NULL,
	[idregistro] [bigint] NOT NULL,
	[hentra] [varchar](5) NOT NULL,
	[hsale] [varchar](5) NOT NULL,
	[est_p] [char](1) NULL,
	[comentario] [varchar](2048) NULL,
	[incidencia_just] [int] NULL,
	[estado] [char](1) NOT NULL,
	[c_tiempo] [varchar](5) NULL,
	[usuario] [varchar](100) NULL,
	[fecha_just] [datetime] NULL,
	[idacc] [bigint] NULL,
 CONSTRAINT [PK_marcas_incidencias] PRIMARY KEY CLUSTERED 
(
	[indice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_incidencias] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_mov_turnos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_mov_turnos](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] [varchar](5) NOT NULL,
	[turno] [int] NOT NULL,
	[estado] [char](1) NOT NULL,
	[usuario] [varchar](100) NULL,
	[fecha_reg] [datetime] NULL,
	[linea] [int] NULL default(1),
 CONSTRAINT [PK_marcas_mov_turnos] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_mov_turnos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_mov_horarios]    Script Date: 7/5/2023 11:45:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[marcas_mov_horarios](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] [varchar](5) NOT NULL,
	[idhorario] [int] NOT NULL,
	[estado] [char](1) NOT NULL,
	[usuario] [varchar](100) NULL,
	[fecha_reg] [datetime] NULL,
 CONSTRAINT [PK_marcas_mov_horarios] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_mov_horarios] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_proceso]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_proceso](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha_entra] [datetime] NOT NULL,
	[fecha_sale] [datetime] NOT NULL,
	[hora_entra] [varchar](5) NOT NULL,
	[hora_sale] [varchar](5) NOT NULL,
	[idturno] [int] NULL,
	[ORDC] [varchar](5) NULL,
	[EXTC] [varchar](5) NULL,
	[ORDT] [varchar](5) NULL,
	[EXTT] [varchar](5) NULL,
	[TC1] [varchar](5) NULL,
	[TC2] [varchar](5) NULL,
	[TC3] [varchar](5) NULL,
	[TC4] [varchar](5) NULL,
	[TC5] [varchar](5) NULL,
	[CON_1] [int] NULL,
	[CON_2] [int] NULL,
	[CON_3] [int] NULL,
	[CON_4] [int] NULL,
	[CON_5] [int] NULL,
	[ID1] [varchar](5) NULL,
	[ID2] [varchar](5) NULL,
	[ID3] [varchar](5) NULL,
	[FD1] [varchar](5) NULL,
	[FD2] [varchar](5) NULL,
	[FD3] [varchar](5) NULL,
	[TOD] [varchar](5) NULL,
	[TED] [varchar](5) NULL,
	[TDD] [varchar](5) NULL,
	[TIEMPO_CALC_JORN] [varchar](5) NULL,
	[TDC] [varchar](5) NULL,
	[estado] [char](1) NOT NULL,
	[estado_inc] [char](1) NULL,
	[manticipo] [varchar](5) NULL,
	[mtardia] [varchar](5) NULL,
	[proyectado] [char](1) NOT NULL,
	[reg_sale] [bigint] NULL,
 CONSTRAINT [PK_marcas_proc] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_proceso] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_proyecciones]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_proyecciones](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[concepto] [int] NOT NULL,
	[cantidad_proyect] [varchar](5) NULL,
	[cantidad_lab] [varchar](5) NULL,
	[estado] [char](1) NULL,
 CONSTRAINT [PK_marcas_proyecciones] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_proyecciones] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_reportes]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_reportes](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[T1] [varchar](5) NULL,
	[T2] [varchar](5) NULL,
	[T3] [varchar](5) NULL,
	[T4] [varchar](5) NULL,
	[T5] [varchar](5) NULL,
	[T6] [varchar](5) NULL,
	[T7] [varchar](5) NULL,
	[T8] [varchar](5) NULL,
	[T9] [varchar](5) NULL,
	[T10] [varchar](5) NULL,
	[T11] [varchar](5) NULL,
	[T12] [varchar](5) NULL,
	[T13] [varchar](5) NULL,
	[periodo] [varchar](8) NULL,
	[estado] [char](1) NOT NULL,
 CONSTRAINT [PK_marcas_reportes] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_reportes] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_resumen]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_resumen](
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[idconcepto] [int] NULL,
	[nominaeq] [varchar](20) NULL,
	[cantidad] [decimal](18, 6) NULL,
	[monto] [decimal](18, 2) NULL,
	[idccosto] [varchar](25) NULL,
	[proyecto] [varchar](25) NULL,
	[fase] [varchar](25) NULL,
	[idperiodo] [varchar](8) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_resumen] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[marcas_tiempo_adicional]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[marcas_tiempo_adicional](
	[idregistro] [bigint] IDENTITY(1,1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[idnumero] [varchar](20) NOT NULL,
	[periodo] [varchar](8) NOT NULL,
	[idconcepto] [int] NOT NULL,
	[cantidad] [varchar](5) NOT NULL,
	[fecha_referencia] [datetime] NOT NULL,
	[usuario] [varchar](100) NOT NULL,
	[fecha_registro] [date] NOT NULL,
	[fecha_actualiza] [date] NULL,
	[centro_costo] [varchar](25) NULL,
	[comentario] [varchar](2048) NULL,
	[usuario_actualiza] [varchar](100) NULL,
	[estado] [char](1) NOT NULL,
	[tcantidad][decimal](18, 2) NULL,
	[proyecto][varchar](25) NULL,
	[fase][varchar](25) NULL,
 CONSTRAINT [PK_marcas_tiempo_adicional] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[marcas_tiempo_adicional] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_ccostos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_ccostos](
	[idccosto] [varchar](25) NOT NULL,
	[descripcion] [varchar](200) NOT NULL,
	[distribuye] [char](1) NULL,
	[alias_ccosto] [VARCHAR](200) NULL,
 CONSTRAINT [PK_ph_ccostos] PRIMARY KEY CLUSTERED 
(
	[idccosto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_ccostos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_companias]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_companias](
	[idcomp] [varchar](8) NOT NULL,
	[compania] [varchar](60) NOT NULL,
	[nom_conector] [varchar](10) NULL,
 CONSTRAINT [PK_ph_companias] PRIMARY KEY CLUSTERED 
(
	[idcomp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_companias] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_conceptos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_conceptos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[concepto] [varchar](6) NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[tipo_j] [int] NOT NULL,
	[tipo_h] [int] NOT NULL,
	[columnar] [int] NOT NULL,
	[nominaeq] [varchar](20) NULL,
	[factor] [int] NOT NULL,
	[tolerancia] [int] NOT NULL,
	[ordinario] [char](1) NOT NULL,
	[autorizado] [char](1) NOT NULL,
	[transferir] [char](1) NULL,
	[adicional] [char](1) NOT NULL,
	[tipo_ext_alm][char](1),
	[muestra_resumen][char](1),
 CONSTRAINT [PK_ph_conceptos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_conceptos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_cond_especiales]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_cond_especiales](
	[idregistro] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](10) NOT NULL,
	[concepto] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_cond_especiales] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_departamento]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_departamento](
	[iddepart] [varchar](15) NOT NULL,
	[descripcion] [varchar](60) NOT NULL,
 CONSTRAINT [PK_ph_departamento] PRIMARY KEY CLUSTERED 
(
	[iddepart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_departamento] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_descansos_turnos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_descansos_turnos](
	[idturno] [int] NOT NULL,
	[idtiempo] [int] NOT NULL,
	[inicio] [varchar](5) NOT NULL,
	[fin] [varchar](5) NOT NULL,
	[tiempo] [varchar](5) NOT NULL,
	[descuenta] [char](1) NULL,
	[tiemp_ext][char](1) NULL DEFAULT ('F')
 CONSTRAINT [PK_ph_descansos_turnos] PRIMARY KEY CLUSTERED 
(
	[idturno] ASC,
	[idtiempo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_descansos_turnos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_distribuciones_ccosto]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_distribuciones_ccosto](
	[idregistro] [int] IDENTITY(1,1) NOT NULL,
	[codigo] [varchar](25) NOT NULL,
	[descripcion] [varchar](60) NOT NULL,
	[idccosto] [varchar](25) NOT NULL,
	[proyecto] [varchar](25) NOT NULL DEFAULT (''),
	[fase] [varchar](25) NOT NULL DEFAULT ('')
 CONSTRAINT [PK_ph_distribuciones_ccosto] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_distribuciones_ccosto] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_formulacion]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_formulacion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](20) NOT NULL,
	[formula] [varchar](4096) NOT NULL,
 CONSTRAINT [PK_ph_formulas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_formulacion] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_grupos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_grupos](
	[idgrupo] [int] NOT NULL,
	[descripcion] [varchar](30) NOT NULL,
	[idcomp] [varchar](8) NULL,
	[idplanilla] [varchar](8) NULL,
	[estado] [char](1) NULL,
	[idagrupamiento] [int] NOT NULL,
 CONSTRAINT [PK_ph_grupos] PRIMARY KEY CLUSTERED 
(
	[idgrupo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_grupos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_horario_turno]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ph_horario_turno](
	[idhorario] [int] NOT NULL,
	[id_dia] [int] NOT NULL,
	[t_1] [int] NOT NULL,
	[t_2] [int] NOT NULL,
	[t_3] [int] NOT NULL,
	[t_4] [int] NOT NULL,
	[t_5] [int] NOT NULL,
 CONSTRAINT [PK_horario_turno] PRIMARY KEY CLUSTERED 
(
	[idhorario] ASC,
	[id_dia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER AUTHORIZATION ON [dbo].[ph_horario_turno] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_horarios]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[ph_horarios](
	[idhorario] [int] NOT NULL,
	[descripcion] [varchar](20) NOT NULL,
 CONSTRAINT [PK_horarios] PRIMARY KEY CLUSTERED 
(
	[idhorario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_horarios] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_periodos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_periodos](
	[idperiodo] [varchar](8) NOT NULL,
	[tipo_planilla] [char](1) NOT NULL,
	[inicio] [datetime] NOT NULL,
	[fin] [datetime] NOT NULL,
	[estado] [char](1) NULL,
	[inicio_proy] [datetime] NULL,
	[fin_proy] [datetime] NULL,
	[periodo_proy] [varchar](8) NULL,
	[dia_inicio] [int] NULL,
 CONSTRAINT [PK_ph_periodos] PRIMARY KEY CLUSTERED 
(
	[idperiodo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_periodos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_planilla]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_planilla](
	[idplanilla] [varchar](8) NOT NULL,
	[planilla] [varchar](60) NOT NULL,
	[nom_conector] [varchar](10) NULL,
	[tipo_planilla] [char](1) NULL,
	[c_ext] [char](1) NULL,
	[c_inci] [char](1) NULL,
	[c_adic] [char](1) NULL,
	[m_desc] [char](1) NULL,
	[proyecta] [char](1) NOT NULL,
	[dia_inicio] [int] NULL,
	[auto_proceso] [char](1) Default('F') NOT NULL,
	[tipo_dist] [char](1) Default('C') NOT NULL,
	[est_nomina] [varchar](1) Default('M'),
	[ext_per_ant] [char](1) Default('F') NOT NULL,
	[ext_det][char](1) Default('F') NOT NULL,
	[agrup_salida][char](1) Default('F'),
	[tipo_adic][char](1) Default('H'),
	[nivel_aprob_ext] [int] Default(0), 
 CONSTRAINT [PK_ph_planilla] PRIMARY KEY CLUSTERED 
(
	[idplanilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ph_planilla] ADD  CONSTRAINT [DF_ph_planilla_dia_inicio]  DEFAULT ((0)) FOR [dia_inicio]
GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_planilla] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_roles]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_roles](
	[idrol] [int] NOT NULL,
	[descripcion] [varchar](60) NOT NULL,
 CONSTRAINT [PK_ph_roles] PRIMARY KEY CLUSTERED 
(
	[idrol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_roles] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_roles_turnos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ph_roles_turnos](
	[idregistro] [int] IDENTITY(1,1) NOT NULL,
	[idrol] [int] NOT NULL,
	[idturno] [int] NOT NULL,
 CONSTRAINT [PK_ph_roles_turnos] PRIMARY KEY CLUSTERED 
(
	[idregistro] ASC,
	[idrol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER AUTHORIZATION ON [dbo].[ph_roles_turnos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[ph_turnos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ph_turnos](
	[idturno] [int] NOT NULL,
	[descripcion] [varchar](30) NOT NULL,
	[hentra] [varchar](5) NOT NULL,
	[hsale] [varchar](5) NOT NULL,
	[tar_apl] [char](1) NULL,
	[ant_apl] [char](1) NULL,
	[des_1_in] [varchar](5) NULL,
	[des_1_out] [varchar](5) NULL,
	[des_2_in] [varchar](5) NULL,
	[des_2_out] [varchar](5) NULL,
	[des_3_in] [varchar](5) NULL,
	[des_3_out] [varchar](5) NULL,
	[apl_des_1] [char](1) NULL,
	[apl_des_2] [char](1) NULL,
	[apl_des_3] [char](1) NULL,
	[des_1_tiem] [varchar](5) NULL,
	[des_2_tiem] [varchar](5) NULL,
	[des_3_tiem] [varchar](5) NULL,
	[marca_des_1] [char](1) NULL,
	[marca_des_2] [char](1) NULL,
	[marca_des_3] [char](1) NULL,
	[tar_tiem] [varchar](5) NULL,
	[ant_tiem] [varchar](5) NULL,
	[con_1] [int] NULL,
	[con_2] [int] NULL,
	[con_3] [int] NULL,
	[con_4] [int] NULL,
	[con_5] [int] NULL,
	[con_6] [int] NULL,
	[cant_con_1] [varchar](5) NULL,
	[cant_con_2] [varchar](5) NULL,
	[cant_con_3] [varchar](5) NULL,
	[cant_con_4] [varchar](5) NULL,
	[cant_con_5] [varchar](5) NULL,
	[cant_con_6] [varchar](5) NULL,
	[min_con_1] [varchar](5) NULL,
	[min_con_2] [varchar](5) NULL,
	[min_con_3] [varchar](5) NULL,
	[min_con_4] [varchar](5) NULL,
	[min_con_5] [varchar](5) NULL,
	[min_con_6] [varchar](5) NULL,
	[tipo] [char](1) NULL,
	[tipo_jor] [char](1) NOT NULL,
	[fuerza_calc] [char](1) NOT NULL,
	[idagrupamiento] [int] NOT NULL,
	[apl_trans1] [int] NULL,
	[id_trans1] [int] NULL,
	[apl_trans2] [int] NULL,
	[id_trans2] [int] NULL,
	[apl_trans3] [int] NULL,
	[id_trans3] [int] NULL,
	[apl_trans4] [int] NULL,
	[id_trans4] [int] NULL,
	[apl_trans5] [int] NULL,
	[id_trans5] [int] NULL,
	[apl_trans6] [int] NULL,
	[id_trans6] [int] NULL,
	[apl_ben1] [int] NULL,
	[id_ben1] [int] NULL,
	[apl_ben2] [int] NULL,
	[id_ben2] [int] NULL,
	[apl_ben3] [int] NULL,
	[id_ben3] [int] NULL,
	[apl_ben4] [int] NULL,
	[id_ben4] [int] NULL,
	[apl_ben5] [int] NULL,
	[id_ben5] [int] NULL,
	[apl_ben6] [int] NULL,
	[id_ben6] [int] NULL,
	[conc_ben1] [int] NULL,
	[conc_ben2] [int] NULL,
	[conc_ben3] [int] NULL,
	[conc_ben4] [int] NULL,
	[conc_ben5] [int] NULL,
	[conc_ben6] [int] NULL,
	[apl_trans_post] [int] NULL,
	[id_trans_post] [int] NULL,
	[apl_redond_entrada][char](1),
	[cant_redond_entrada][varchar](5),	
	[ColorId][INT],
 CONSTRAINT [PK_turnos] PRIMARY KEY CLUSTERED 
(
	[idturno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[ph_turnos] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[reloj_templates]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[reloj_templates](
	[FP_ENROLLID] [varchar](16) NOT NULL,
	[FP_INDEXID] [tinyint] NULL,
	[FP_TEMPLATE] [varchar](max) NOT NULL,
	[FP_LENGTH] [int] NULL,
	[FP_USER] [varchar](16) NULL,
	[FP_SEC] [int] NULL,
	[IDUSER] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[reloj_templates] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[reloj_usuario]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[reloj_usuario](
	[FP_ENROLLID] [int] NULL,
	[FP_USERNAME] [varchar](20) NULL,
	[FP_USERPASS] [varchar](20) NULL,
	[FP_PRIVILEGIO] [char](1) NOT NULL,
	[FP_ESTADO] [char](1) NOT NULL,
	[FP_TARJETA] [varchar](50) NULL,
	[FP_IDCOMP] [varchar](6) NULL,
	[FACE] [varchar](max) NULL,
	[FACE_LENG][int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[reloj_usuario] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[relojes_dispositivo]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[relojes_dispositivo](
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
	[FG_MODEL][varchar](80)NULL,
	[usa_face][char](1)
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[relojes_dispositivo] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[Relojes_sitio]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Relojes_sitio](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DESCRIPTION] [varchar](40) NULL,
	[ESTADO] [varchar](10) NULL,
	[CONEXION_PRIMARIA] [char](1) NULL,
	[BBACKUP] [varchar](120) NULL,
	[DBCONNECTOR] [varchar](300) NULL,
	[DBUSER] [varchar](50) NULL,
	[DBPASS] [varchar](50) NULL,
	[ARCHIVO] [varchar](120) NULL,
	[USA_PRIMARIO] [char](1) NULL,
 CONSTRAINT [PK_TAC_SITE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[Relojes_sitio] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[reportes]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[reportes](
	[idreporte] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[ruta] [varchar](300) NOT NULL,
	[idmenu] [int] NOT NULL,
	[estado] [char](1) NOT NULL,
 CONSTRAINT [PK_reportes] PRIMARY KEY CLUSTERED 
(
	[idreporte] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[reportes] TO  SCHEMA OWNER 
GO

/****** Object:  Table [dbo].[ph_grupo_periodo]    Script Date: 16/06/2017 10:49:55 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ph_grupo_periodo](
	[idgrupo] [int] NOT NULL,
	[idperiodo] [varchar](8) NOT NULL,
	[estado] [char](1) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[fecha_cierre] [datetime] NULL,
	[usuario_cierre] [int] NULL,
 CONSTRAINT [PK_ph_grupo_periodo] PRIMARY KEY CLUSTERED 
(
	[idgrupo] ASC,
	[idperiodo] ASC,
	[idplanilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ph_grupo_periodo] ADD  CONSTRAINT [DF_ph_grupo_periodo_estado]  DEFAULT ('F') FOR [estado]
GO

/****** Object:  Table [dbo].[reportes_menu]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[reportes_menu](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[estado] [char](1) NOT NULL,
 CONSTRAINT [PK_reportes_menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[reportes_menu] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[tipo_ausencia]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tipo_ausencia](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](40) NOT NULL,
	[tipo] [varchar](4) NOT NULL,
 CONSTRAINT [PK_tipo_ausencia] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[tipo_ausencia] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[tipos_planilla]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tipos_planilla](
	[tipo_planilla] [char](1) NOT NULL,
	[planilla] [varchar](10) NOT NULL,
 CONSTRAINT [PK_tipos_planilla] PRIMARY KEY CLUSTERED 
(
	[tipo_planilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[tipos_planilla] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[transformaciones]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[transformaciones](
	[id] [int] NOT NULL,
	[descripcion] [varchar](40) NOT NULL,
	[idconcepto_1] [int] NOT NULL,
	[idconcepto_2] [int] NOT NULL,
	[idconcepto_3] [int] NOT NULL,
	[idconcepto_4] [int] NOT NULL,
	[idconcepto_5] [int] NOT NULL,
	[idconcepto_6] [int] NOT NULL,
	[idconcepto_7] [int] NOT NULL,
	[calculadas] [char](1) NOT NULL,
	[hrs_concepto_1] [varchar](5) NOT NULL,
	[hrs_concepto_2] [varchar](5) NOT NULL,
	[hrs_concepto_3] [varchar](5) NOT NULL,
	[hrs_concepto_4] [varchar](5) NOT NULL,
	[hrs_concepto_5] [varchar](5) NOT NULL,
	[hrs_concepto_6] [varchar](5) NOT NULL,
	[hrs_concepto_7] [varchar](5) NOT NULL,
	[min_concepto_1] [varchar](5) NOT NULL,
	[min_concepto_2] [varchar](5) NOT NULL,
	[min_concepto_3] [varchar](5) NOT NULL,
	[min_concepto_4] [varchar](5) NOT NULL,
	[min_concepto_5] [varchar](5) NOT NULL,
	[min_concepto_6] [varchar](5) NOT NULL,
	[min_concepto_7] [varchar](5) NOT NULL,
	[turno_continuo] [char](1) NULL,
	[usa_trans_ant] [char](1) NULL,
 CONSTRAINT [PK_transformaciones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[transformaciones] TO  SCHEMA OWNER 
GO

/****** Object:  Table [dbo].[ph_transformacion]    Script Date: 16/09/2017 09:23:43 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ph_transformacion](
	[id_transformacion] [int] NOT NULL,
	[descripcion] [varchar](60) NOT NULL,
	[usr_transf] [char](1) not null default('F'),
 CONSTRAINT [PK_ph_transformacion] PRIMARY KEY CLUSTERED 
(
	[id_transformacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER AUTHORIZATION ON [dbo].[ph_transformacion] TO  SCHEMA OWNER 
GO

/****** Object:  Table [dbo].[ph_transformacion_conceptos]    Script Date: 16/09/2017 09:23:58 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ph_transformacion_conceptos](
	[id_transformacion] [int] NOT NULL,
	[prioridad] [int] IDENTITY(1,1) NOT NULL,
	[concepto_orig] [int] NOT NULL,
	[concepto_cambio] [int] NOT NULL,
	[usr_transf][char](1) NULL Default('F'),
 CONSTRAINT [PK_ph_transformacion_conceptos] PRIMARY KEY CLUSTERED 
(
	[id_transformacion] ASC,
	[prioridad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ph_transformacion_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_transformacion] FOREIGN KEY([id_transformacion])
REFERENCES [dbo].[ph_transformacion] ([id_transformacion])
GO

ALTER TABLE [dbo].[ph_transformacion_conceptos] CHECK CONSTRAINT [FK_transformacion]
GO

ALTER TABLE [dbo].[ph_transformacion_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_transformacion_ph_conceptos_cambio] FOREIGN KEY([concepto_cambio])
REFERENCES [dbo].[ph_conceptos] ([id])
GO

ALTER TABLE [dbo].[ph_transformacion_conceptos] CHECK CONSTRAINT [FK_transformacion_ph_conceptos_cambio]
GO

ALTER TABLE [dbo].[ph_transformacion_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_transformacion_ph_conceptos_orig] FOREIGN KEY([concepto_orig])
REFERENCES [dbo].[ph_conceptos] ([id])
GO

ALTER TABLE [dbo].[ph_transformacion_conceptos] CHECK CONSTRAINT [FK_transformacion_ph_conceptos_orig]
GO

ALTER AUTHORIZATION ON [dbo].[ph_transformacion_conceptos] TO  SCHEMA OWNER 
GO

/****** Object:  Table [dbo].[transformaciones_globales]    Script Date: 31/05/2017 08:22:27 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[transformaciones_globales](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_orden] [int] NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[idplanilla] [varchar](8) NOT NULL,
	[estado] [char](1) NOT NULL,
	[formula_apl] [int] NOT NULL,
	[formula_hrs] [int] NOT NULL,
	[formula_concepto] [int] NOT NULL,
 CONSTRAINT [PK_transformaciones_globales] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ParametrosEmail](
       [Id] [int] NOT NULL,
       [SmtpServer] [varchar](100) NOT NULL,
       [SmtpPort] [int] NOT NULL,
       [DefaultEmail] [varchar](50) NOT NULL,
       [DefaultPassWord] [varchar](255) NOT NULL,
CONSTRAINT [PK_ParametrosEmail] PRIMARY KEY CLUSTERED
(
       [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[transformaciones_globales] ADD  CONSTRAINT [DF_transformaciones_globales_estado]  DEFAULT ('T') FOR [estado]
GO

ALTER TABLE [dbo].[transformaciones_globales]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_globales_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO

ALTER TABLE [dbo].[transformaciones_globales] CHECK CONSTRAINT [FK_transformaciones_globales_ph_planilla]
GO
ALTER AUTHORIZATION ON [dbo].[transformaciones_globales] TO  SCHEMA OWNER 
GO
/****** Object:  Table [dbo].[transformaciones_turnos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[transformaciones_turnos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idtrans] [int] NOT NULL,
	[turno_orig] [int] NOT NULL,
	[turno_cambio] [int] NOT NULL,
	[descripcion] [varchar](20) NULL,
 CONSTRAINT [PK_transformaciones_turnos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[transformaciones_turnos] TO  SCHEMA OWNER 
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_acciones_personal]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_acciones_personal] ON [dbo].[acciones_personal]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[inicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_acciones_personal_1]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_acciones_personal_1] ON [dbo].[acciones_personal]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[inicio] ASC,
	[idincidencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_empleado_planilla]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_empleado_planilla] ON [dbo].[empleados]
(
	[idplanilla] ASC,
	[idnumero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_empleados]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_empleados] ON [dbo].[empleados]
(
	[idgrupo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_identificacion]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_identificacion] ON [dbo].[empleados]
(
	[identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas] ON [dbo].[marcas]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_1]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_1] ON [dbo].[marcas]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC,
	[hora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_audit]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_audit] ON [dbo].[marcas_audit]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_audit_1]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_audit_1] ON [dbo].[marcas_audit]
(
	[fecha_chg] ASC,
	[usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_comedor]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_comedor] ON [dbo].[marcas_comedor]
(
	[idplanilla] ASC,
	[idnumero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_descansos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_descansos] ON [dbo].[marcas_descansos]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_distribuciones]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_distribuciones] ON [dbo].[marcas_distribuciones]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_distribuciones_conceptos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_distribuciones_conceptos] ON [dbo].[marcas_distribuciones_conceptos]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_distribuciones_conceptos_1]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_distribuciones_conceptos_1] ON [dbo].[marcas_distribuciones_conceptos]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC,
	[idccosto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_extras_apb]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_extras_apb] ON [dbo].[marcas_extras_apb]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_in]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_in] ON [dbo].[marcas_in]
(
	[idplanilla] ASC,
	[idnumero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_incidencias]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_incidencias] ON [dbo].[marcas_incidencias]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_incidencias_1]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_incidencias_1] ON [dbo].[marcas_incidencias]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC,
	[idincidencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_mov_turnos]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_mov_turnos] ON [dbo].[marcas_mov_turnos]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_proceso]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_proceso] ON [dbo].[marcas_proceso]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha_entra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_proyecciones]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_proyecciones] ON [dbo].[marcas_proyecciones]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_reportes]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_reportes] ON [dbo].[marcas_reportes]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_resumen]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_resumen] ON [dbo].[marcas_resumen]
(
	[idplanilla] ASC,
	[idnumero] ASC,
	[idperiodo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_marcas_tiempo_adicional]    Script Date: 29/05/2017 08:29:11 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_marcas_tiempo_adicional] ON [dbo].[marcas_tiempo_adicional]
(
	[idnumero] ASC,
	[idplanilla] ASC,
	[periodo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[acciones_personal] ADD  CONSTRAINT [DF_acciones_personal_estado]  DEFAULT ('N') FOR [estado]
GO
ALTER TABLE [dbo].[acciones_personal] ADD  CONSTRAINT [DF_acciones_personal_fecha_just]  DEFAULT (getdate()) FOR [fecha_just]
GO
ALTER TABLE [dbo].[empleados] ADD  CONSTRAINT [DF_empleados_idagrupamiento]  DEFAULT ((0)) FOR [idagrupamiento]
GO
ALTER TABLE [dbo].[empleados] ADD  CONSTRAINT [DF_empleados_exporta]  DEFAULT ('T') FOR [exporta]
GO
ALTER TABLE [dbo].[empleados] ADD  CONSTRAINT [DF__empleados__tipo___0425A276]  DEFAULT ('H') FOR [tipo_marca]
GO
ALTER TABLE [dbo].[incidencias] ADD  CONSTRAINT [DF__incidenci__ed_ti__07020F21]  DEFAULT ('F') FOR [ed_tiempo]
GO
ALTER TABLE [dbo].[marcas] ADD  CONSTRAINT [DF_marcas_estado]  DEFAULT ('N') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_comedor] ADD  CONSTRAINT [DF_marcas_comedor_tipo]  DEFAULT ((4)) FOR [tipo]
GO
ALTER TABLE [dbo].[marcas_descansos] ADD  CONSTRAINT [DF_marcas_descansos_inicio_desc]  DEFAULT ('00:00') FOR [inicio_desc]
GO
ALTER TABLE [dbo].[marcas_descansos] ADD  CONSTRAINT [DF_marcas_descansos_fin_desc]  DEFAULT ('00:00') FOR [fin_desc]
GO
ALTER TABLE [dbo].[marcas_distribuciones] ADD  CONSTRAINT [DF_marcas_distribuciones_cantidad]  DEFAULT ((0)) FOR [cantidad]
GO
ALTER TABLE [dbo].[marcas_distribuciones] ADD  CONSTRAINT [DF_marcas_distribuciones_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_distribuciones] ADD  CONSTRAINT [DF__marcas_di__entra__173876EA]  DEFAULT ('00:00') FOR [entrada]
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos] ADD  CONSTRAINT [DF_marcas_distribuciones_conceptos_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_extras_apb] ADD  CONSTRAINT [DF_marcas_extras_apb_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_in] ADD  CONSTRAINT [DF_marcas_in_estado]  DEFAULT ('N') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_incidencias] ADD  CONSTRAINT [DF_marcas_incidencias_incidencia_just]  DEFAULT ((0)) FOR [incidencia_just]
GO
ALTER TABLE [dbo].[marcas_incidencias] ADD  CONSTRAINT [DF_marcas_incidencias_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_incidencias] ADD  CONSTRAINT [DF__marcas_in__c_tie__22AA2996]  DEFAULT ('00:00') FOR [c_tiempo]
GO
ALTER TABLE [dbo].[marcas_incidencias] ADD  CONSTRAINT [DF_marcas_incidencias_fecha_just]  DEFAULT (getdate()) FOR [fecha_just]
GO
ALTER TABLE [dbo].[marcas_mov_turnos] ADD  CONSTRAINT [DF_marcas_mov_turnos_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_mov_turnos] ADD  CONSTRAINT [DF__marcas_mo__fecha__267ABA7A]  DEFAULT (getdate()) FOR [fecha_reg]
GO
ALTER TABLE [dbo].[marcas_mov_horarios] ADD  CONSTRAINT [DF_marcas_mov_horarios_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_mov_horarios] ADD  CONSTRAINT [DF__marcas_mov_horarios_fecha]  DEFAULT (getdate()) FOR [fecha_reg]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_idturno]  DEFAULT ((1)) FOR [idturno]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_ORDC]  DEFAULT ('00:00') FOR [ORDC]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_EXTC]  DEFAULT ('00:00') FOR [EXTC]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_ORDT]  DEFAULT ('00:00') FOR [ORDT]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_EXTT]  DEFAULT ('00:00') FOR [EXTT]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TC1]  DEFAULT ('00:00') FOR [TC1]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TC2]  DEFAULT ('00:00') FOR [TC2]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TC3]  DEFAULT ('00:00') FOR [TC3]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TC4]  DEFAULT ('00:00') FOR [TC4]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TC5]  DEFAULT ('00:00') FOR [TC5]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_CON_1]  DEFAULT ((0)) FOR [CON_1]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_CON_2]  DEFAULT ((0)) FOR [CON_2]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_CON_3]  DEFAULT ((0)) FOR [CON_3]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_CON_4]  DEFAULT ((0)) FOR [CON_4]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_CON_5]  DEFAULT ((0)) FOR [CON_5]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_ID1]  DEFAULT ('00:00') FOR [ID1]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_ID2]  DEFAULT ('00:00') FOR [ID2]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_ID3]  DEFAULT ('00:00') FOR [ID3]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_FD1]  DEFAULT ('00:00') FOR [FD1]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_FD2]  DEFAULT ('00:00') FOR [FD2]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_FD3]  DEFAULT ('00:00') FOR [FD3]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TDO]  DEFAULT ('00:00') FOR [TOD]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TED]  DEFAULT ('00:00') FOR [TED]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proc_TDD]  DEFAULT ('00:00') FOR [TDD]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proceso_TIEMPO_CALC_JORN]  DEFAULT ('00:00') FOR [TIEMPO_CALC_JORN]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proceso_TDC]  DEFAULT ('00:00') FOR [TDC]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF_marcas_proceso_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF__marcas_pr__manti__4316F928]  DEFAULT ('00:00') FOR [manticipo]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF__marcas_pr__mtard__440B1D61]  DEFAULT ('00:00') FOR [mtardia]
GO
ALTER TABLE [dbo].[marcas_proceso] ADD  CONSTRAINT [DF__marcas_pr__proye__44FF419A]  DEFAULT ('F') FOR [proyectado]
GO
ALTER TABLE [dbo].[marcas_proyecciones] ADD  CONSTRAINT [DF_Table_1_ordinarias]  DEFAULT ('00:00') FOR [cantidad_proyect]
GO
ALTER TABLE [dbo].[marcas_proyecciones] ADD  CONSTRAINT [DF_marcas_proyecciones_ordinarias_lab]  DEFAULT ('00:00') FOR [cantidad_lab]
GO
ALTER TABLE [dbo].[marcas_proyecciones] ADD  CONSTRAINT [DF_marcas_proyecciones_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T1]  DEFAULT ('00:00') FOR [T1]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T2]  DEFAULT ('00:00') FOR [T2]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T3]  DEFAULT ('00:00') FOR [T3]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T4]  DEFAULT ('00:00') FOR [T4]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T5]  DEFAULT ('00:00') FOR [T5]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T6]  DEFAULT ('00:00') FOR [T6]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T7]  DEFAULT ('00:00') FOR [T7]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T8]  DEFAULT ('00:00') FOR [T8]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T9]  DEFAULT ('00:00') FOR [T9]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T10]  DEFAULT ('00:00') FOR [T10]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T11]  DEFAULT ('00:00') FOR [T11]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T12]  DEFAULT ('00:00') FOR [T12]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_T13]  DEFAULT ('00:00') FOR [T13]
GO
ALTER TABLE [dbo].[marcas_reportes] ADD  CONSTRAINT [DF_marcas_reportes_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[marcas_resumen] ADD  CONSTRAINT [DF_marcas_resumen_cantidad]  DEFAULT ((0)) FOR [cantidad]
GO
ALTER TABLE [dbo].[marcas_resumen] ADD  CONSTRAINT [DF_marcas_resumen_monto]  DEFAULT ((0)) FOR [monto]
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional] ADD  CONSTRAINT [DF_marcas_tiempo_adicional_estado]  DEFAULT ('A') FOR [estado]
GO
ALTER TABLE [dbo].[ph_ccostos] ADD  CONSTRAINT [DF_ph_ccostos_distribuye]  DEFAULT ('F') FOR [distribuye]
GO
ALTER TABLE [dbo].[ph_conceptos] ADD  CONSTRAINT [DF_ph_conceptos_adicional]  DEFAULT ('F') FOR [adicional]
GO
ALTER TABLE [dbo].[ph_conceptos] ADD  CONSTRAINT [DF_ph_conceptos_tipo_ext_alm]  DEFAULT ('F') FOR [tipo_ext_alm]
GO
ALTER TABLE [dbo].[ph_grupos] ADD  CONSTRAINT [DF_ph_grupos_agrupamiento]  DEFAULT ((0)) FOR [idagrupamiento]
GO
ALTER TABLE [dbo].[ph_periodos] ADD  CONSTRAINT [DF_ph_periodos_estado]  DEFAULT ('T') FOR [estado]
GO
ALTER TABLE [dbo].[ph_planilla] ADD  CONSTRAINT [DF_ph_planilla_c_ext]  DEFAULT ('F') FOR [c_ext]
GO
ALTER TABLE [dbo].[ph_planilla] ADD  CONSTRAINT [DF_ph_planilla_c_inci]  DEFAULT ('F') FOR [c_inci]
GO
ALTER TABLE [dbo].[ph_planilla] ADD  CONSTRAINT [DF_ph_planilla_c_adic]  DEFAULT ('F') FOR [c_adic]
GO
ALTER TABLE [dbo].[ph_planilla] ADD  CONSTRAINT [DF__ph_planil__proye__7C4F7684]  DEFAULT ('F') FOR [proyecta]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_tar_apl]  DEFAULT ('F') FOR [tar_apl]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_ant_apl]  DEFAULT ('F') FOR [ant_apl]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_rfcal_apl]  DEFAULT ('F') FOR [apl_redond_entrada]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_trfcal]  DEFAULT ('00:00') FOR [cant_redond_entrada]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_1_in]  DEFAULT ('00:00') FOR [des_1_in]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_1_out]  DEFAULT ('00:00') FOR [des_1_out]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_2_in]  DEFAULT ('00:00') FOR [des_2_in]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_2_out]  DEFAULT ('00:00') FOR [des_2_out]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_3_in]  DEFAULT ('00:00') FOR [des_3_in]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_3_out]  DEFAULT ('00:00') FOR [des_3_out]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_1_tiem]  DEFAULT ('00:00') FOR [des_1_tiem]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_2_tiem]  DEFAULT ('00:00') FOR [des_2_tiem]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_des_3_tiem]  DEFAULT ('00:00') FOR [des_3_tiem]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_marca_des_1]  DEFAULT ('F') FOR [marca_des_1]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_marca_des_2]  DEFAULT ('F') FOR [marca_des_2]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_marca_des_3]  DEFAULT ('F') FOR [marca_des_3]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_tar_tiem]  DEFAULT ('00:00') FOR [tar_tiem]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_ant_tiem]  DEFAULT ('00:00') FOR [ant_tiem]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_cant_con_1]  DEFAULT ('00:00') FOR [cant_con_1]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_cant_con_2]  DEFAULT ('00:00') FOR [cant_con_2]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_cant_con_3]  DEFAULT ('00:00') FOR [cant_con_3]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_cant_con_4]  DEFAULT ('00:00') FOR [cant_con_4]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_cant_con_5]  DEFAULT ('00:00') FOR [cant_con_5]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_cant_con_6]  DEFAULT ('00:00') FOR [cant_con_6]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_min_con_1]  DEFAULT ('00:00') FOR [min_con_1]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_min_con_2]  DEFAULT ('00:00') FOR [min_con_2]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_min_con_3]  DEFAULT ('00:00') FOR [min_con_3]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_min_con_4]  DEFAULT ('00:00') FOR [min_con_4]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_min_con_5]  DEFAULT ('00:00') FOR [min_con_5]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_min_con_6]  DEFAULT ('00:00') FOR [min_con_6]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_turnos_tipo]  DEFAULT ('R') FOR [tipo]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_ph_turnos_tipo_jor]  DEFAULT ('D') FOR [tipo_jor]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_turnos_fuerza_calc]  DEFAULT ('F') FOR [fuerza_calc]
GO
ALTER TABLE [dbo].[ph_turnos] ADD  CONSTRAINT [DF_turnos_idagrupamiento]  DEFAULT ((0)) FOR [idagrupamiento]
GO
ALTER TABLE [dbo].[reloj_usuario] ADD  CONSTRAINT [DF_TAC_FPUSER_FP_PRIVILEGIO]  DEFAULT ('0') FOR [FP_PRIVILEGIO]
GO
ALTER TABLE [dbo].[reloj_usuario] ADD  CONSTRAINT [DF_TAC_FPUSER_FP_ESTADO]  DEFAULT ('A') FOR [FP_ESTADO]
GO
ALTER TABLE [dbo].[relojes_dispositivo] ADD  CONSTRAINT [DF_TAC_CLOCK_clock_pass]  DEFAULT ('0000') FOR [clock_pass]
GO
ALTER TABLE [dbo].[reportes_menu] ADD  CONSTRAINT [DF_reportes_menu_estado]  DEFAULT ('T') FOR [estado]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_calculadas]  DEFAULT ('F') FOR [calculadas]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_1]  DEFAULT ('00:00') FOR [hrs_concepto_1]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_2]  DEFAULT ('00:00') FOR [hrs_concepto_2]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_3]  DEFAULT ('00:00') FOR [hrs_concepto_3]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_4]  DEFAULT ('00:00') FOR [hrs_concepto_4]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_5]  DEFAULT ('00:00') FOR [hrs_concepto_5]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_6]  DEFAULT ('00:00') FOR [hrs_concepto_6]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_hrs_concepto_7]  DEFAULT ('00:00') FOR [hrs_concepto_7]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_1]  DEFAULT ('00:00') FOR [min_concepto_1]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_2]  DEFAULT ('00:00') FOR [min_concepto_2]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_3]  DEFAULT ('00:00') FOR [min_concepto_3]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_4]  DEFAULT ('00:00') FOR [min_concepto_4]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_5]  DEFAULT ('00:00') FOR [min_concepto_5]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_6]  DEFAULT ('00:00') FOR [min_concepto_6]
GO
ALTER TABLE [dbo].[transformaciones] ADD  CONSTRAINT [DF_transformaciones_min_concepto_7]  DEFAULT ('00:00') FOR [min_concepto_7]
GO
ALTER TABLE [dbo].[acciones_personal]  WITH CHECK ADD  CONSTRAINT [FK_acciones_personal_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[acciones_personal] CHECK CONSTRAINT [FK_acciones_personal_empleados]
GO
ALTER TABLE [dbo].[acciones_personal]  WITH CHECK ADD  CONSTRAINT [FK_acciones_personal_incidencias] FOREIGN KEY([idincidencia])
REFERENCES [dbo].[incidencias] ([id])
GO
ALTER TABLE [dbo].[acciones_personal] CHECK CONSTRAINT [FK_acciones_personal_incidencias]
GO
ALTER TABLE [dbo].[acciones_personal]  WITH CHECK ADD  CONSTRAINT [FK_acciones_personal_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[acciones_personal] CHECK CONSTRAINT [FK_acciones_personal_ph_planilla]
GO
ALTER TABLE [dbo].[empleados]  WITH CHECK ADD  CONSTRAINT [FK_empleados_ph_grupos] FOREIGN KEY([idgrupo])
REFERENCES [dbo].[ph_grupos] ([idgrupo])
GO
ALTER TABLE [dbo].[empleados] CHECK CONSTRAINT [FK_empleados_ph_grupos]
GO
ALTER TABLE [dbo].[empleados]  WITH CHECK ADD  CONSTRAINT [FK_empleados_ph_horarios] FOREIGN KEY([idhorario])
REFERENCES [dbo].[ph_horarios] ([idhorario])
GO
ALTER TABLE [dbo].[empleados] CHECK CONSTRAINT [FK_empleados_ph_horarios]
GO
ALTER TABLE [dbo].[empleados]  WITH CHECK ADD  CONSTRAINT [FK_empleados_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[empleados] CHECK CONSTRAINT [FK_empleados_ph_planilla]
GO
ALTER TABLE [dbo].[marcas]  WITH CHECK ADD  CONSTRAINT [FK_marcas_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas] CHECK CONSTRAINT [FK_marcas_empleados]
GO
ALTER TABLE [dbo].[marcas]  WITH CHECK ADD  CONSTRAINT [FK_marcas_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas] CHECK CONSTRAINT [FK_marcas_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_audit]  WITH CHECK ADD  CONSTRAINT [FK_marcas_audit_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_audit] CHECK CONSTRAINT [FK_marcas_audit_empleados]
GO
ALTER TABLE [dbo].[marcas_audit]  WITH CHECK ADD  CONSTRAINT [FK_marcas_audit_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_audit] CHECK CONSTRAINT [FK_marcas_audit_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_comedor]  WITH CHECK ADD  CONSTRAINT [FK_marcas_comedor_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_comedor] CHECK CONSTRAINT [FK_marcas_comedor_empleados]
GO
ALTER TABLE [dbo].[marcas_comedor]  WITH CHECK ADD  CONSTRAINT [FK_marcas_comedor_marcas_comedor] FOREIGN KEY([idregistro])
REFERENCES [dbo].[marcas_comedor] ([idregistro])
GO
ALTER TABLE [dbo].[marcas_comedor] CHECK CONSTRAINT [FK_marcas_comedor_marcas_comedor]
GO
ALTER TABLE [dbo].[marcas_comedor]  WITH CHECK ADD  CONSTRAINT [FK_marcas_comedor_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_comedor] CHECK CONSTRAINT [FK_marcas_comedor_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_descansos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_descansos_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_descansos] CHECK CONSTRAINT [FK_marcas_descansos_empleados]
GO
ALTER TABLE [dbo].[marcas_descansos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_descansos_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_descansos] CHECK CONSTRAINT [FK_marcas_descansos_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_distribuciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_distribuciones] CHECK CONSTRAINT [FK_marcas_distribuciones_empleados]
GO
ALTER TABLE [dbo].[marcas_distribuciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_ph_ccostos] FOREIGN KEY([idccosto])
REFERENCES [dbo].[ph_ccostos] ([idccosto])
GO
ALTER TABLE [dbo].[marcas_distribuciones] CHECK CONSTRAINT [FK_marcas_distribuciones_ph_ccostos]
GO
ALTER TABLE [dbo].[marcas_distribuciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_ph_conceptos] FOREIGN KEY([idconcepto])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[marcas_distribuciones] CHECK CONSTRAINT [FK_marcas_distribuciones_ph_conceptos]
GO
ALTER TABLE [dbo].[marcas_distribuciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_distribuciones] CHECK CONSTRAINT [FK_marcas_distribuciones_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_conceptos_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos] CHECK CONSTRAINT [FK_marcas_distribuciones_conceptos_empleados]
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_conceptos_marcas_distribuciones_conceptos] FOREIGN KEY([idregistro])
REFERENCES [dbo].[marcas_distribuciones_conceptos] ([idregistro])
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos] CHECK CONSTRAINT [FK_marcas_distribuciones_conceptos_marcas_distribuciones_conceptos]
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_conceptos_ph_ccostos] FOREIGN KEY([idccosto])
REFERENCES [dbo].[ph_ccostos] ([idccosto])
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos] CHECK CONSTRAINT [FK_marcas_distribuciones_conceptos_ph_ccostos]
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_distribuciones_conceptos_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_distribuciones_conceptos] CHECK CONSTRAINT [FK_marcas_distribuciones_conceptos_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_extras_apb]  WITH CHECK ADD  CONSTRAINT [FK_marcas_extras_apb_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_extras_apb] CHECK CONSTRAINT [FK_marcas_extras_apb_empleados]
GO
ALTER TABLE [dbo].[marcas_extras_apb]  WITH CHECK ADD  CONSTRAINT [FK_marcas_extras_apb_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_extras_apb] CHECK CONSTRAINT [FK_marcas_extras_apb_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_incidencias]  WITH CHECK ADD  CONSTRAINT [FK_marcas_incidencias_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_incidencias] CHECK CONSTRAINT [FK_marcas_incidencias_empleados]
GO
ALTER TABLE [dbo].[marcas_incidencias]  WITH CHECK ADD  CONSTRAINT [FK_marcas_incidencias_incidencias] FOREIGN KEY([idincidencia])
REFERENCES [dbo].[incidencias] ([id])
GO
ALTER TABLE [dbo].[marcas_incidencias] CHECK CONSTRAINT [FK_marcas_incidencias_incidencias]
GO
ALTER TABLE [dbo].[marcas_incidencias]  WITH CHECK ADD  CONSTRAINT [FK_marcas_incidencias_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_incidencias] CHECK CONSTRAINT [FK_marcas_incidencias_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_mov_turnos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_mov_turnos_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_mov_turnos] CHECK CONSTRAINT [FK_marcas_mov_turnos_empleados]
GO
ALTER TABLE [dbo].[marcas_mov_turnos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_mov_turnos_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_mov_turnos] CHECK CONSTRAINT [FK_marcas_mov_turnos_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_mov_turnos]  WITH CHECK ADD  CONSTRAINT [FK_marcas_mov_turnos_ph_turnos] FOREIGN KEY([turno])
REFERENCES [dbo].[ph_turnos] ([idturno])
GO
ALTER TABLE [dbo].[marcas_mov_turnos] CHECK CONSTRAINT [FK_marcas_mov_turnos_ph_turnos]
GO
ALTER TABLE [dbo].[marcas_mov_horarios]  WITH CHECK ADD  CONSTRAINT [FK_marcas_mov_horarios_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_mov_horarios] CHECK CONSTRAINT [FK_marcas_mov_horarios_empleados]
GO
ALTER TABLE [dbo].[marcas_mov_horarios]  WITH CHECK ADD  CONSTRAINT [FK_marcas_mov_horarios_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_mov_horarios] CHECK CONSTRAINT [FK_marcas_mov_horarios_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_mov_horarios]  WITH CHECK ADD  CONSTRAINT [FK_marcas_mov_horarios_ph_horarios] FOREIGN KEY([idhorario])
REFERENCES [dbo].[ph_horarios] ([idhorario])
GO
ALTER TABLE [dbo].[marcas_mov_horarios] CHECK CONSTRAINT [FK_marcas_mov_horarios_ph_horarios]
GO
ALTER TABLE [dbo].[marcas_proceso]  WITH CHECK ADD  CONSTRAINT [FK_marcas_proceso_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_proceso] CHECK CONSTRAINT [FK_marcas_proceso_empleados]
GO
ALTER TABLE [dbo].[marcas_proceso]  WITH CHECK ADD  CONSTRAINT [FK_marcas_proceso_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_proceso] CHECK CONSTRAINT [FK_marcas_proceso_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_proceso]  WITH CHECK ADD  CONSTRAINT [FK_marcas_proceso_ph_turnos] FOREIGN KEY([idturno])
REFERENCES [dbo].[ph_turnos] ([idturno])
GO
ALTER TABLE [dbo].[marcas_proceso] CHECK CONSTRAINT [FK_marcas_proceso_ph_turnos]
GO
ALTER TABLE [dbo].[marcas_proyecciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_proyecciones_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_proyecciones] CHECK CONSTRAINT [FK_marcas_proyecciones_empleados]
GO
ALTER TABLE [dbo].[marcas_proyecciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_proyecciones_ph_conceptos] FOREIGN KEY([concepto])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[marcas_proyecciones] CHECK CONSTRAINT [FK_marcas_proyecciones_ph_conceptos]
GO
ALTER TABLE [dbo].[marcas_proyecciones]  WITH CHECK ADD  CONSTRAINT [FK_marcas_proyecciones_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_proyecciones] CHECK CONSTRAINT [FK_marcas_proyecciones_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_reportes]  WITH CHECK ADD  CONSTRAINT [FK_marcas_reportes_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_reportes] CHECK CONSTRAINT [FK_marcas_reportes_empleados]
GO
ALTER TABLE [dbo].[marcas_reportes]  WITH CHECK ADD  CONSTRAINT [FK_marcas_reportes_ph_periodos] FOREIGN KEY([periodo])
REFERENCES [dbo].[ph_periodos] ([idperiodo])
GO
ALTER TABLE [dbo].[marcas_reportes] CHECK CONSTRAINT [FK_marcas_reportes_ph_periodos]
GO
ALTER TABLE [dbo].[marcas_reportes]  WITH CHECK ADD  CONSTRAINT [FK_marcas_reportes_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_reportes] CHECK CONSTRAINT [FK_marcas_reportes_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_resumen]  WITH CHECK ADD  CONSTRAINT [FK_marcas_resumen_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_resumen] CHECK CONSTRAINT [FK_marcas_resumen_empleados]
GO
ALTER TABLE [dbo].[marcas_resumen]  WITH CHECK ADD  CONSTRAINT [FK_marcas_resumen_ph_ccostos] FOREIGN KEY([idccosto])
REFERENCES [dbo].[ph_ccostos] ([idccosto])
GO
ALTER TABLE [dbo].[marcas_resumen] CHECK CONSTRAINT [FK_marcas_resumen_ph_ccostos]
GO
ALTER TABLE [dbo].[marcas_resumen]  WITH CHECK ADD  CONSTRAINT [FK_marcas_resumen_ph_conceptos] FOREIGN KEY([idconcepto])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[marcas_resumen] CHECK CONSTRAINT [FK_marcas_resumen_ph_conceptos]
GO
ALTER TABLE [dbo].[marcas_resumen]  WITH CHECK ADD  CONSTRAINT [FK_marcas_resumen_ph_planilla] FOREIGN KEY([idplanilla])
REFERENCES [dbo].[ph_planilla] ([idplanilla])
GO
ALTER TABLE [dbo].[marcas_resumen] CHECK CONSTRAINT [FK_marcas_resumen_ph_planilla]
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional]  WITH CHECK ADD  CONSTRAINT [FK_marcas_tiempo_adicional_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional] CHECK CONSTRAINT [FK_marcas_tiempo_adicional_empleados]
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional]  WITH CHECK ADD  CONSTRAINT [FK_marcas_tiempo_adicional_ph_ccostos] FOREIGN KEY([centro_costo])
REFERENCES [dbo].[ph_ccostos] ([idccosto])
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional] CHECK CONSTRAINT [FK_marcas_tiempo_adicional_ph_ccostos]
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional]  WITH CHECK ADD  CONSTRAINT [FK_marcas_tiempo_adicional_ph_conceptos] FOREIGN KEY([idconcepto])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional] CHECK CONSTRAINT [FK_marcas_tiempo_adicional_ph_conceptos]
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional]  WITH CHECK ADD  CONSTRAINT [FK_marcas_tiempo_adicional_ph_periodos] FOREIGN KEY([periodo])
REFERENCES [dbo].[ph_periodos] ([idperiodo])
GO
ALTER TABLE [dbo].[marcas_tiempo_adicional] CHECK CONSTRAINT [FK_marcas_tiempo_adicional_ph_periodos]
GO
ALTER TABLE [dbo].[ph_cond_especiales]  WITH CHECK ADD  CONSTRAINT [FK_ph_cond_especiales_ph_conceptos] FOREIGN KEY([concepto])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[ph_cond_especiales] CHECK CONSTRAINT [FK_ph_cond_especiales_ph_conceptos]
GO
ALTER TABLE [dbo].[ph_descansos_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_descansos_turnos_ph_descansos_turnos] FOREIGN KEY([idturno], [idtiempo])
REFERENCES [dbo].[ph_descansos_turnos] ([idturno], [idtiempo])
GO
ALTER TABLE [dbo].[ph_descansos_turnos] CHECK CONSTRAINT [FK_ph_descansos_turnos_ph_descansos_turnos]
GO
ALTER TABLE [dbo].[ph_descansos_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_descansos_turnos_ph_turnos] FOREIGN KEY([idturno])
REFERENCES [dbo].[ph_turnos] ([idturno])
GO
ALTER TABLE [dbo].[ph_descansos_turnos] CHECK CONSTRAINT [FK_ph_descansos_turnos_ph_turnos]
GO
ALTER TABLE [dbo].[ph_horario_turno]  WITH CHECK ADD  CONSTRAINT [FK_horario_turno_horarios] FOREIGN KEY([idhorario])
REFERENCES [dbo].[ph_horarios] ([idhorario])
GO
ALTER TABLE [dbo].[ph_horario_turno] CHECK CONSTRAINT [FK_horario_turno_horarios]
GO
ALTER TABLE [dbo].[ph_roles_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_roles_turnos_ph_turnos] FOREIGN KEY([idturno])
REFERENCES [dbo].[ph_turnos] ([idturno])
GO
ALTER TABLE [dbo].[ph_roles_turnos] CHECK CONSTRAINT [FK_ph_roles_turnos_ph_turnos]
GO
ALTER TABLE [dbo].[ph_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_turnos_ph_conceptos] FOREIGN KEY([con_1])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[ph_turnos] CHECK CONSTRAINT [FK_ph_turnos_ph_conceptos]
GO
ALTER TABLE [dbo].[ph_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_turnos_ph_conceptos1] FOREIGN KEY([con_2])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[ph_turnos] CHECK CONSTRAINT [FK_ph_turnos_ph_conceptos1]
GO
ALTER TABLE [dbo].[ph_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_turnos_ph_conceptos2] FOREIGN KEY([con_3])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[ph_turnos] CHECK CONSTRAINT [FK_ph_turnos_ph_conceptos2]
GO
ALTER TABLE [dbo].[ph_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_turnos_ph_conceptos3] FOREIGN KEY([con_4])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[ph_turnos] CHECK CONSTRAINT [FK_ph_turnos_ph_conceptos3]
GO
ALTER TABLE [dbo].[ph_turnos]  WITH CHECK ADD  CONSTRAINT [FK_ph_turnos_ph_conceptos4] FOREIGN KEY([con_5])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[ph_turnos] CHECK CONSTRAINT [FK_ph_turnos_ph_conceptos4]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos] FOREIGN KEY([idconcepto_1])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos1] FOREIGN KEY([idconcepto_2])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos1]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos2] FOREIGN KEY([idconcepto_3])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos2]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos3] FOREIGN KEY([idconcepto_4])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos3]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos4] FOREIGN KEY([idconcepto_5])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos4]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos5] FOREIGN KEY([idconcepto_6])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos5]
GO
ALTER TABLE [dbo].[transformaciones]  WITH CHECK ADD  CONSTRAINT [FK_transformaciones_ph_conceptos6] FOREIGN KEY([idconcepto_7])
REFERENCES [dbo].[ph_conceptos] ([id])
GO
ALTER TABLE [dbo].[transformaciones] CHECK CONSTRAINT [FK_transformaciones_ph_conceptos6]
GO
ALTER TABLE [dbo].[marcas_extras_detalle]  WITH CHECK ADD  CONSTRAINT [FK_marcas_extras_detalle_empleados] FOREIGN KEY([idnumero])
REFERENCES [dbo].[empleados] ([idnumero])
GO
ALTER TABLE [dbo].[marcas_extras_detalle] CHECK CONSTRAINT [FK_marcas_extras_detalle_empleados]
GO

ALTER TABLE [dbo].[ph_turnos]  WITH CHECK ADD  CONSTRAINT [FK_phturnos_paletacolores] FOREIGN KEY([ColorId])
REFERENCES [dbo].[PaletaColores] ([ColorId])
GO 

ALTER TABLE [dbo].[ph_turnos] CHECK CONSTRAINT [FK_phturnos_paletacolores]
GO