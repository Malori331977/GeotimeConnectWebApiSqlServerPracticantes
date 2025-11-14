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
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'301', 0, N'/historicohorasextras', N'more_time', N'Historico_Horas_Extra', N'300')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'302', 0, N'/historicoincidencias', N'crisis_alert', N'Historico_Incidencias', N'300')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'303', 0, N'/historicomarcas', N'punch_clock', N'Historico_Marcas', N'300')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'304', 0, N'/historicocalculostiempos', N'summarize', N'Historico_Calculo_Tiempo', N'300')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'305', 0, N'/historicoconceptosresumen', N'assessment', N'ResumenConceptos', N'300')
GO
INSERT [ctadmin].[ph_opciones_sistema] ([Id], [Principal], [Href], [IconId], [MenuText], [ParentId]) VALUES (N'306', 0, N'/historicoconceptos', N'devices_other', N'ResumenConceptosEmpleado', N'300')
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
INSERT [ctadmin].[ph_roles_sistema] ([Id], [Descripcion], [Habilitado]) VALUES (N'0001', N'Administrador', 1)
GO

INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'101', 1, 1, 1, 1, 1)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'102', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'103', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'104', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'105', 1, 1, 1, 1, 1)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'106', 1, 1, 1, 1, 1)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'107', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'108', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'109', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'110', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'111', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'112', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'100', N'113', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'201', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'202', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'203', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'204', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'205', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'206', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'207', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'208', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'209', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'210', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'211', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'212', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'213', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'200', N'214', 1, 1, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'401', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'402', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'403', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'404', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'405', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'406', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'407', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'408', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'409', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'400', N'410', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'501', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'502', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'503', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'504', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'505', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'506', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'507', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'508', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'509', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'510', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'500', N'511', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'600', N'601', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'600', N'602', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'600', N'603', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'700', N'701', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'700', N'702', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'700', N'703', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'700', N'704', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'700', N'705', 1, 0, 0, 0, 0)
GO
INSERT [ctadmin].[ph_roles_sistemadet] ([RolSistemaId], [MenuSistemaId], [OpcionSistemaId], [Habilitado], [Agrega], [Modifica], [Elimina], [Consulta]) VALUES (N'0001', N'999', N'998', 1, 0, 0, 0, 0)
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