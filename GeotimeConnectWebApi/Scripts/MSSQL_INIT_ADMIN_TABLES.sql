INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'100', N'Procesos', NULL, N'developer_board')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'200', N'Mantenimientos', NULL, N'construction')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'300', N'Reportes', NULL, N'assessment')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'400', N'Configuracion', NULL, N'manage_accounts')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'500', N'Configuracion_Adicional', NULL, N'settings_suggest')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'600', N'Seguridad', NULL, N'security')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'700', N'Integracion_ERP', NULL, N'cast_connected')
GO
INSERT [ctadmin].[ph_menus_sistema] ([Id], [MenuText], [Href], [IconId]) VALUES (N'999', N'Ayuda', NULL, N'help')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'101', 0, N'/periodomarcalist', N'calendar_clock', N'Períodos_de_Marcas', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'102', 0, N'/empleadomarcafiltro', N'edit_calendar', N'Editar_Marcas_del_Periodo', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'103', 0, N'/actperiodoproceso', N'linear_scale', N'Activar_Período', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'104', 0, N'/clperiodomarca', N'calculate', N'Calcular_Período_de_Marca', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'105', 0, N'/pcondicionesespeciales', N'radio_button_unchecked', N'Edición_Condiciones_Especiales', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'106', 0, N'/paccionpersonal', N'file_open', N'Acciones_de_Personal', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'107', 0, N'/pprogramadorturnos', N'calendar_month', N'Programador_de_Turnos', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'108', 0, N'/pprogramadorhorarios', N'radio_button_unchecked', N'Programador_de_Horarios', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'109', 0, N'/pamasivaextras', N'radio_button_unchecked', N'Aprobacion_Masiva_de_Extras', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'110', 0, N'/pcambiomasivo', N'radio_button_unchecked', N'Cambio_Masivo_de_Entrada_y_Salida', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'113', 0, N'/pcierreperiodo', N'radio_button_unchecked', N'Cierre_de_Periodo', N'100')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'201', 0, N'/companialist', N'business', N'Compañias', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'202', 0, N'/planillalist', N'radio_button_unchecked', N'Nómina', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'203', 0, N'/departamentolist', N'radio_button_unchecked', N'Departamentos', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'204', 0, N'/grupolist', N'radio_button_unchecked', N'Grupo', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'205', 0, N'/empleadolist', N'radio_button_unchecked', N'Empleado', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'206', 0, N'/turnolist', N'radio_button_unchecked', N'Turnos', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'207', 0, N'/horarioturnoList', N'radio_button_unchecked', N'Horarios', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'208', 0, N'/rollist', N'radio_button_unchecked', N'Roles', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'209', 0, N'/incidlist', N'radio_button_unchecked', N'Incidencias', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'212', 0, N'/centrocostolist', N'radio_button_unchecked', N'Centro_de_Costo', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'213', 0, N'/paletacolorlist', N'radio_button_unchecked', N'Administración_de_Colores', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'214', 0, N'/programacionturnoscmlist', N'punch_clock', N'Carga_Masiva_Turnos', N'200')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'402', 0, N'/importartipoaccion', N'radio_button_unchecked', N'Importar_Definición_Acciones_Per', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'403', 0, N'/conceptolist', N'radio_button_unchecked', N'Tipos_de_Hora', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'404', 0, N'/formulacionlist', N'radio_button_unchecked', N'Formulas', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'405', 0, N'/transformacionlist', N'radio_button_unchecked', N'Transformaciones_en_Turnos', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'406', 0, N'/transformaciongloballist', N'radio_button_unchecked', N'Transformaciones_Post_Calculo', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'407', 0, N'/incidenciaconfpagolist', N'radio_button_unchecked', N'Configuracion_Pago_Incidencias', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'408', 0, N'/transformaciontipomarcalist', N'radio_button_unchecked', N'Transformaciones_Funcion_Tipo', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'409', 0, N'/opcionessistema', N'radio_button_unchecked', N'Opciones_del_Sistema', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'410', 0, N'/preferenciausuario', N'contact_emergency', N'Preferencias_de_Usuario', N'400')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'501', 0, N'/menusistemalist', N'radio_button_unchecked', N'Menús_de_Sistema', N'500')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'502', 0, N'/opcionsistemalist', N'radio_button_unchecked', N'Menús_Secundarios', N'500')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'503', 0, N'/parametroemail', N'radio_button_unchecked', N'Parámetros_Email', N'500')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'508', 0, N'/cargarlicencia', N'radio_button_unchecked', N'Cargar_Licencia', N'500')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'511', 0, N'/temas', N'radio_button_unchecked', N'Temas', N'500')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'601', 0, N'/rolsistemalist', N'safety_check', N'Perfiles_de_seguridad', N'600')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'602', 0, N'/usuariosistemalist', N'people', N'Usuarios_del_Sistema', N'600')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'603', 0, N'/usuariocompanialist', N'person_pin', N'Usuarios_Compañia', N'600')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'701', 0, N'/sincronizaerp', N'sync', N'Sincronizar_ERP', N'700')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'702', 0, N'/importarconcepto', N'sync_lock', N'Importar_Concepto', N'700')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'703', 0, N'/sincronizaacciones', N'arrow_downward', N'Importar_Acciones_Personal', N'700')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'704', 0, N'/exportaraccionespersonal', N'file_copy', N'Exportar_Acciones_de_Personal', N'700')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'705', 0, N'/exportarconceptos', N'import_export', N'Exportar_Conceptos', N'700')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'998', 1, NULL, N'radio_button_unchecked', N'Ayuda', N'999')
GO
INSERT [ctadmin].[ph_roles_sistema] ([Id], [Descripcion], [Habilitado]) VALUES (N'adm', N'Administrador', 1)
GO

INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'101', 1, 1, 1, 1, 1)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'102', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'103', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'104', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'105', 1, 1, 1, 1, 1)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'106', 1, 1, 1, 1, 1)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'107', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'108', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'109', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'110', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'111', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'112', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'100', N'113', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'201', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'202', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'203', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'204', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'205', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'206', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'207', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'208', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'209', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'210', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'211', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'212', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'213', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'200', N'214', 1, 1, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'401', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'402', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'403', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'404', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'405', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'406', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'407', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'408', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'409', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'400', N'410', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'501', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'502', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'503', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'504', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'505', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'506', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'507', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'508', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'509', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'510', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'500', N'511', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'600', N'601', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'600', N'602', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'600', N'603', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'700', N'701', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'700', N'702', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'700', N'703', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'700', N'704', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'700', N'705', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'adm', N'999', N'998', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_login] ([usuario], [clave], [descripcion], [privilegio], [grupo_base], [idsesion], [ultimo_login], [fcomp], [fplani], [fperiodo], [grupos], [proceso], [ultimo_estado], [companias], [planillas], [idioma], [stat_msg], [turnos], [usa_wusuario], [ESTADO], [email], [omite_lic], [global_clave], [ACTIVO], [RolUsuario]) VALUES ( N'admin', N'eCE/J9PZQ7jCqKFKPhC0hV9cR+W/IS8IiIj1uc+0nbw=', N'Administrador', 1, 1, 603694682, getdate(), null, N'', N'', N'', 0, getdate(), N'', NULL, N'esp', N'', NULL, N'F', N'A', N'', N'F', N'd033e22ae348aeb5660fc2140aec35850c4da997', N'T', NULL)
go
INSERT [ctadmin].[ph_sistema] ([data_01], [data_02], [data_03], [ver_db], [post_emp], [post_sinc], [num_alm], [f_install], [f_contrato], [f_control], [data_04], [data_05], [dist_lic_usr], [dist_lic_emp], [dist_lic]) VALUES (N'712B6B3176422F656B6E4C70796B3055413356724A6C5A55356B7A6B39676D35484E7245577A71526F77633DXF51', N'3143613876593938655679647364364B4B49743932327264344F786F45543163514C6745466A57426833513DXE0A', N'67324E4F5874344C4C656541572F4B526A4C492F4A65494556777A4533336D7550334A43795454766F37453DXE0A', N'3.3', N'F', N'F', 3, N'446D624F764F63514A4F70324B42336761515677656B65354F4A626B75344C43422B2F53716A45684459773DXE8C', NULL, N'656752374C316C71567264794E65327047682F6542754434677558764C442F7A6B3275797446707A674F413DXF61', N'6E6D52304F466B704C43336A4D583155556367755557714C486653794D624C5856765745744E5836624E773DXF0A', N'5379776C5A395539672B706E39394C64616D686E7750702F79786C2B6A7753477274774B553150546439453DXF60', N'F', N'F', N'F')
GO
INSERT [ctadmin].[Ph_Usuarios_Roles] ([IdUsuario], [Rol], [IdUsuarioRegistra], [FechaRegistro], [IdUsuarioModifica], [FechaModifica]) VALUES (1, N'v03YnVDmetWGgh0WUcDFQJG2zPRvEnN15VoB65gDkPg=', 1, getdate(), 1, getdate())
GO
INSERT [ctadmin].[ph_catalogo_generico] ([NombreCatalogo], [Id], [Descripcion], [UsuarioCreacion], [FechaCreacion], [UsuarioUltModificacion], [FechaUltModificacion]) VALUES (N'DESTINO_SOLICITUD', N'DT', N'Distribucion Conceptos', N'admin',GETDATE(), N'admin', GETDATE())
GO
INSERT [ctadmin].[ph_catalogo_generico] ([NombreCatalogo], [Id], [Descripcion], [UsuarioCreacion], [FechaCreacion], [UsuarioUltModificacion], [FechaUltModificacion]) VALUES (N'DESTINO_SOLICITUD', N'HE', N'Horas Extra', N'admin', GETDATE(), N'admin', GETDATE())
GO
INSERT [ctadmin].[ph_catalogo_generico] ([NombreCatalogo], [Id], [Descripcion], [UsuarioCreacion], [FechaCreacion], [UsuarioUltModificacion], [FechaUltModificacion]) VALUES (N'DESTINO_SOLICITUD', N'TA', N'Tiempo Adicional', N'admin', GETDATE(), N'admin', GETDATE())
GO
INSERT [ctadmin].[ph_catalogo_generico] ([NombreCatalogo], [Id], [Descripcion], [UsuarioCreacion], [FechaCreacion], [UsuarioUltModificacion], [FechaUltModificacion]) VALUES (N'ESTADO_PERIODO_MARCA', N'C', N'Cierre Definitivo', N'admin', GETDATE(), N'admin', GETDATE())
GO
INSERT [ctadmin].[ph_catalogo_generico] ([NombreCatalogo], [Id], [Descripcion], [UsuarioCreacion], [FechaCreacion], [UsuarioUltModificacion], [FechaUltModificacion]) VALUES (N'ESTADO_PERIODO_MARCA', N'N', N'Creado', N'admin', GETDATE(), N'admin', GETDATE())
GO
INSERT [ctadmin].[ph_catalogo_generico] ([NombreCatalogo], [Id], [Descripcion], [UsuarioCreacion], [FechaCreacion], [UsuarioUltModificacion], [FechaUltModificacion]) VALUES (N'ESTADO_PERIODO_MARCA', N'T', N'Activo', N'admin', GETDATE(), N'admin', GETDATE())
GO