insert into [dbo].[PaletaColores](Descripcion, ColorFondo, ColorFuente) values ('Blanco y Negro','#ffffff','#000000')
go
INSERT [dbo].[Portal_Menu] ([Id], [MenuText], [Href], [IconId]) VALUES (N'100', N'Procesos', NULL, 231)
GO
INSERT [dbo].[Portal_Menu] ([Id], [MenuText], [Href], [IconId]) VALUES (N'200', N'Configuración', NULL, 1745)
GO
INSERT [dbo].[Portal_Menu] ([Id], [MenuText], [Href], [IconId]) VALUES (N'900', N'Marcas Web', NULL, 1536)
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'101', 0, N'/marcas', 706, N'Registro de Marcas', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'102', 0, N'/horasextralist', 545, N'Solicitud Horas Extra', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'103', 0, N'/calendario', 316, N'Calendario de Trabajo', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'104', 0, N'/incidenciaslist', 465, N'Pre-Justificar Incidencias', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'105', 0, N'/accionPersonallist', 352, N'Acciones de Personal', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'106', 0, N'/programacionlist', 350, N'Estado de Programación', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'107', 0, N'/empleadoextlist', 1424, N'Registro de Rostros de Colaboradores', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'108', 0, N'/horasextraaprobacionlist', 355, N'Autorización Horas Extra', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'109', 0, N'/solicitudlist', 394, N'Solicitudes', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'110', 0, N'/solicitudlistautorizacion', 491, N'Autorizar Solicitud', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'111', 0, N'/solicitudlistconsulta', 323, N'Consulta Solicitudes ', N'100')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'201', 0, N'/portalmenulist', 1309, N'Menus de Sistema', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'202', 0, N'/portalopcionlist', 1315, N'Menús Secundarios', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'203', 0, N'/portalconfigdetalle', 669, N'Parámetros de Sistema', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'204', 0, N'/portalrollist', 1459, N'Roles de Sistema', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'205', 0, N'/portalempleadolist', 1286, N'Habilitar Empleados en Marcas Web', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'206', 0, N'/parametroemail', 776, N'Parámetros de Correo', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'207', 0, N'/organizacionnivellist', 308, N'Niveles de Organización', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'208', 0, N'/organizacionlist', 685, N'Estructura de Organización', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'209', 0, N'/flujoautorizacionlist', 684, N'Flujos de Autorización', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'210', 0, N'/estadolist', 204, N'Estado de Solicitudes', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'211', 0, N'/tiposolicitudlist', 1370, N'Tipos de Solicitudes', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'212', 0, N'/grupolist', 1126, N'Grupos', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'213', 0, N'/empleadojefaturalist', 1447, N'Jefaturas directas', N'200')
GO
INSERT [dbo].[Portal_Opciones] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'214', 0, N'/configuraciontiposolicitudlist', 1375, N'Configuración de Tipos de Solicitud', N'200')
GO
INSERT [dbo].[Portal_Roles] ([Id], [Descripcion], [RolDefault], [Habilitado]) VALUES (N'ADMI', N'Administrador de Sistema', 0, 1)
GO
INSERT [dbo].[Portal_Roles] ([Id], [Descripcion], [RolDefault], [Habilitado]) VALUES (N'ADMW', N'Administrador Marcas Web', 0, 1)
GO
INSERT [dbo].[Portal_Roles] ([Id], [Descripcion], [RolDefault], [Habilitado]) VALUES (N'DFMW', N'Rol Marcas Web Default', 1, 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'101', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'102', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'103', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'104', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'105', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'106', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'107', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'108', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'100', N'109', 0)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'201', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'202', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'203', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'204', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'205', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'206', 1)
GO
INSERT [dbo].[Portal_RolesDet] ([PortalRolId], [PortalMenuId], [PortalOpcionId], [Habilitado]) VALUES (N'ADMI', N'200', N'207', 1)
GO
INSERT [ctadmin].[Portal_Config] ([IdAplicacion], [IdVersion], [Compania], [BaseDatos], [IdLicencia], [Activa], [UsoRestringido], [RegsitroLic], [Permanente], [UsaReconocimientoFacial], [FechaUltModifica], [IdUsuarioModifica], [USARGEOLOCALIZACION], [MAPAPIKEY], [FACEDIST], [FACETEXT], [VerLogMarcas]) VALUES (N'com.gsitcr.portalmarcasweb', N'1.0.2', N'dbo', N'SoftlandCA', N'NnZ4QYIwvLJSwA3GRz+VDWCf7/MwIHdrxCmtFYbSm8CeHAmhYrFOyfN8bw/ATlplpVjv7u5ntNbpUH1BuNfnXWeVaJP3Z7q1RbGgJ6DAYIOdhS5kVd3H5kNQChBopSmglxOyY3o4MHkNzW6HVxKpcHvZdyCA9km9C3mFRky8gdnUybYyRzTLP75ZmWts6gw4nPlnWuJNrF/ZPf9pqFmkTvhbc8iGTyz/C3mFRky8gdlYXh/VaJ4yvoMnRafsuvZHd6QIt2expO2vIeWWurFVR2q8mUIi1fl+q3YzYWmrULs=', 1, 1, N'z6w3kGWY58U=', 1, 1, CAST(N'2025-02-01T15:33:48.9898738' AS DateTime2), N'admin', 1, N'cv6DudJqb203gG+kPySsTtawEb0hkZRsxz+4hhg3y2jZJvJk6jozCw==', CAST(0.25 AS Decimal(4, 2)), CAST(40.00 AS Decimal(4, 2)), 1)
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (0, N'En proceso de Registro', CAST(N'2025-07-24T18:57:33.4495394' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (1, N'En Proceso de Autorizacion', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (2, N'Autorizado Jefe Inmediato', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (3, N'Autorizado Gerente Area', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (4, N'Autorizado Gerente General', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (99, N'Autorizado Final', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (100, N'Anulado empleado', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Estados] ([Id], [Descripcion], [FechaRegistro]) VALUES (101, N'Rechazada', CAST(N'2025-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[OrganizacionNiveles] ([Id], [Descripcion], [IdUsuarioModifica], [FechaModifica]) VALUES (N'01', N'GERENCIA GENERAL', N'admin', CAST(N'2025-07-10T17:54:45.3038133' AS DateTime2))
GO
INSERT [dbo].[OrganizacionNiveles] ([Id], [Descripcion], [IdUsuarioModifica], [FechaModifica]) VALUES (N'02', N'DIRECCIONES', N'admin', CAST(N'2025-07-10T17:55:42.0056623' AS DateTime2))
GO
INSERT [dbo].[OrganizacionNiveles] ([Id], [Descripcion], [IdUsuarioModifica], [FechaModifica]) VALUES (N'03', N'DEPARTAMENTOS', N'admin', CAST(N'2025-07-10T18:00:59.9201344' AS DateTime2))
GO
INSERT [dbo].[SolicitudesConfiguracion] ([Id], [Descripcion], [FechaInicio], [FechaFin], [HoraInicio], [HoraFin], [CantidadHoras], [CantidadNumerico], [CentroCosto], [MultipleCentroCosto], [FiltrarPuesto], [PuestosHabilitados], [Concepto], [Destino], [MostrarCantidadHoras], [MostrarCantidadNum], [CalcularHoras], [CalcularHorasNum], [CalcularDias], [MaxCantidadDiasPasados], [MaxCantidadDiasFuturos], [FiltrarNomina], [NominasHabilitadas], [AdmiteDuplicados]) VALUES (N'HE', N'HORAS EXTRA', 1, 0, 1, 1, 1, 0, 1, 0, 0, N'', -1, N'HE', 1, 0, 1, 0, 0, 0, 30, 0, N'', 0)
GO
INSERT [dbo].[SolicitudesConfiguracion] ([Id], [Descripcion], [FechaInicio], [FechaFin], [HoraInicio], [HoraFin], [CantidadHoras], [CantidadNumerico], [CentroCosto], [MultipleCentroCosto], [FiltrarPuesto], [PuestosHabilitados], [Concepto], [Destino], [MostrarCantidadHoras], [MostrarCantidadNum], [CalcularHoras], [CalcularHorasNum], [CalcularDias], [MaxCantidadDiasPasados], [MaxCantidadDiasFuturos], [FiltrarNomina], [NominasHabilitadas], [AdmiteDuplicados]) VALUES (N'MD', N'Distrbución de Tiempo', 1, 0, 1, 1, 0, 1, 1, 1, 0, N'059|072|061', -1, N'DT', 0, 1, 0, 1, 0, 7, 0, 0, N'', 1)
GO
INSERT [dbo].[SolicitudesConfiguracion] ([Id], [Descripcion], [FechaInicio], [FechaFin], [HoraInicio], [HoraFin], [CantidadHoras], [CantidadNumerico], [CentroCosto], [MultipleCentroCosto], [FiltrarPuesto], [PuestosHabilitados], [Concepto], [Destino], [MostrarCantidadHoras], [MostrarCantidadNum], [CalcularHoras], [CalcularHorasNum], [CalcularDias], [MaxCantidadDiasPasados], [MaxCantidadDiasFuturos], [FiltrarNomina], [NominasHabilitadas], [AdmiteDuplicados]) VALUES (N'HA', N'HORAS ADICIONALES', 1, 0, 1, 1, 0, 1, 1, 1, 0, N'', 8, N'TA', 1, 0, 0, 1, 1, 7, 0, 1, N'1|2|20|21', 1)
GO
SET IDENTITY_INSERT [dbo].[TiposSolicitudes] ON 
GO
INSERT [dbo].[TiposSolicitudes] ([Id], [Descripcion], [FlujoAutorizacionId], [IdUsuarioModifica], [FechaModifica], [Activa], [TipoConfiguracion]) VALUES (1, N'SOLICITUD DE HORAS EXTRA', 1, N'admin', CAST(N'2025-09-04T16:42:51.6196465' AS DateTime2), 1, N'HE')
GO
INSERT [dbo].[TiposSolicitudes] ([Id], [Descripcion], [FlujoAutorizacionId], [IdUsuarioModifica], [FechaModifica], [Activa], [TipoConfiguracion]) VALUES (2, N'SOLICITUD DE DITRIBUCION DE TIEMPO', 1, N'admin', CAST(N'2025-09-04T16:43:09.8371943' AS DateTime2), 1, N'MD')
GO
INSERT [dbo].[TiposSolicitudes] ([Id], [Descripcion], [FlujoAutorizacionId], [IdUsuarioModifica], [FechaModifica], [Activa], [TipoConfiguracion]) VALUES (3, N'HORAS ADICIONALES', 1, N'admin', CAST(N'2025-09-04T16:42:10.8958219' AS DateTime2), 1, N'HA')
GO
SET IDENTITY_INSERT [dbo].[TiposSolicitudes] OFF
GO
update [dbo].[ph_turnos] set ColorId=1
GO
update [dbo].[ph_usuario] set filt_prgt='G' where filt_prgt is null
GO
update [dbo].[ph_planilla] set [dia_inicio]=0 where [dia_inicio] is null
GO