insert into geotime.ph_conceptos (id,concepto, descripcion, tipo_j, tipo_h, columnar, factor, tolerancia, ordinario, autorizado, transferir, adicional) values (geotime.idconcepto.nextval,'0','No Calcula',4,4,'1',1,0,'F','F','F','F')
go


insert into geotime.ph_turnos (idturno, descripcion, hentra, hsale, tar_apl, ant_apl, tar_tiem, ant_tiem, con_1, con_2, con_3, con_4, con_5, cant_con_1, cant_con_2, cant_con_3, cant_con_4, cant_con_5, min_con_1, min_con_2, min_con_3, min_con_4, min_con_5, tipo_jor,tipo, fuerza_calc, idagrupamiento) values ('0','No Calcula','00:00','00:00','F','F','00:00','00:00',1,1,1,1,1,'00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','D','E','F',0)
go

insert into geotime.ph_horarios (idhorario,descripcion) values ('1','General')
go

insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',1,0,0,0,0,0)
go
insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',2,0,0,0,0,0)
go
insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',3,0,0,0,0,0)
go
insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',4,0,0,0,0,0)
go
insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',5,0,0,0,0,0)
go
insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',6,0,0,0,0,0)
go
insert into geotime.ph_horario_turno (idhorario, id_dia, t_1, t_2, t_3, t_4, t_5) values ('1',7,0,0,0,0,0)
go

insert into geotime.tipos_planilla (tipo_planilla,planilla) values ('S','Semanal')
go
insert into geotime.tipos_planilla (tipo_planilla,planilla) values ('Q','Quincenal')
go
insert into geotime.tipos_planilla (tipo_planilla,planilla) values ('M','Mensual')
go
insert into geotime.tipos_planilla (tipo_planilla,planilla) values ('B','Bisemanal')
go

insert into geotime.ph_grupos (idgrupo, descripcion, idagrupamiento) values (1,'General',0)
go

insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'AUS','Ausencia')
go
insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'TAR','Tardia')
go
insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'ANT','Anticipo')
go
insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'FME','Falta Marca Entrada')
go
insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'FMS','Falta Marca Salida')
go
insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'FMD','Falta Marca Descanso')
go
insert into geotime.incidencias (id,codigo, descripcion) values (geotime.idincidencia.nextval,'EXD','Exceso Descanso')
go

insert into geotime.ph_niveles (idnivel,descripcion, variables) values (geotime.Idnivel.nextval,'Administracion','WCTNDfPJmJ26lHu24eLKrZAY8ScqkuFMgsnESkAodjhhuV0zhOxgv1yn//sf42zK5L9RSqMS5D2JXBm/6EsetYNKomdd4RuBuHELUYv95N8hcT1FXS0jQGO6BL8JwaMB+CoP2fCYBAtxZEOf3E9v/ZsIkPyoAsagmXrHzpbd9BUBqOyMOMUILEKg/4K+CKSNlIR3w+T1tqEuH0D0ethQRbIKD9Jxh6dcfw3c2OyGYo2KlYeV8Em4zXk+Gf6XxrsfzRscBN3PPhv/VGrYsghZI3i0SA67Brfgp6xyKwZoXw+OclkTZD4ov7LygQ4msWnwBD7qXHAQq54b6BMlWm/650clVUGWFfVMdGkKoNaAv4h8aFlUqRLQQSQBkrdlz/szn3QwQ++hNzvL5vr56KdNMxHIUl3nflMAtUf/TjFbeVDCknQFS3vfADkC9uI7yNb3UZOuITwzTfolL2vjlKBIToPyvmKbckgiGPcU3+/JmoyDrUoaXPVgibPxBLRIhjXQ0UE7BsLuI8T9NdQ2DujF4i8xcS+WFz9DO8tD4WqQabNFUCde+QeJy/cfDp41wY0rwEdXNdt1+il1tdWvJ7gj3ejlTzs0F+5v1BSCX6YFos8PccEqTz/nfXJXTHfiuQOTcTUoIrbCpXxtsSEFe13mDdqe66A5kWt3C3KUtVycHRUv6PwysUANj8SqjTkncO3LwtNm1/pDLzFuOvZDZGZ3rIBCkK1nNNgTnwA2ShlXC1ZOQzwcsHjun2eguI8+0vYdJEEE/Vo8TUCYc+JqeYWgz9aMVmpFj9kVpdIUB/kzUJeDab5N4gJc7O2ZyF0LzmF5ki2WTJocla1EdXIi902NlD2ovdIftubW+NHoXLRxvI8yiBBJt9fxT/twwdbUcU7UlhvSjgS4/nu82TcrVmtWp3s5HzjSkAEn+PXHcD18PPs1qrsUVELxGPV4M1zyWZKuWa7mrFc6Zk+FMBYedvJFkxqMpi8q1PkrvDbl/yMo/2le41nb4q/iGp0qLy88dhAxJo8DE6d/VK6OSQOIwkIuIt7B8Vg3+5aHIAV1br0X+5BLgZSN+ADyLi5p1Vu3qxuIyDCd7zRaYtyfEAV/kjcpPGRnvG+Abitl+U4pm6rWFnsG5hlEtOPt4unkCI3SXLO8')
go


insert into geotime.reportes_menu (id,descripcion,estado) values (geotime.idmreporte.nextval,'Tiempos e Incidencias','T')
go

insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Historico Incidencias','/reportes/historico_incidencias.rpt',1,'T')
go
insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Marcas Periodo','/reportes/historico_marcas.rpt',1,'T')
go
insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Historico de Calculos','/reportes/historico_calculos.rpt',1,'T')
go
insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Resumen de Conceptos por Empleado','/reportes/resumen_conceptos.rpt',1,'T')
go
insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Resumen de Conceptos con Total','/reportes/historico_calculos_totalizado.rpt',1,'T')
go
insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Historico de Extras','/reportes/historico_extras.rpt',1,'T')
go
insert into geotime.reportes (idreporte,descripcion, ruta, idmenu, estado) values (geotime.idreporte.nextval,'Resumen de Conceptos del Periodo','/reportes/resumen_exp.rpt',1,'T')
go

insert into geotime.ph_formulacion (id,descripcion,formula) values (geotime.idformulacion.nextval,'Si','true')
go
insert into geotime.ph_formulacion (id,descripcion,formula) values (geotime.idformulacion.nextval,'No','false')
go
insert into geotime.ph_formulacion (id,descripcion,formula) values (geotime.idformulacion.nextval,'Nulo','0')
go

insert into geotime.transformaciones (id,descripcion,idconcepto_1, idconcepto_2, idconcepto_3, idconcepto_4, idconcepto_5, idconcepto_6, idconcepto_7, calculadas, hrs_concepto_1, hrs_concepto_2, hrs_concepto_3, hrs_concepto_4, hrs_concepto_5, hrs_concepto_6, hrs_concepto_7, min_concepto_1, min_concepto_2, min_concepto_3, min_concepto_4, min_concepto_5, min_concepto_6, min_concepto_7) values (1,'No Calcula',1,1,1,1,1,1,1,'F','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00','00:00')
go

insert into geotime.ph_opciones (idopcion,post_emp,post_sinc,num_alm,ver_db) values (1,'F','F',3,'3.3')
go