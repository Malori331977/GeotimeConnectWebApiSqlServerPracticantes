--procedure roles

create or replace procedure geotime.roles(pidplanilla varchar2, pidnumero varchar2, pinicio date, pfin date) is
begin
  declare
    ptipoh char(1);
    pfechar date;
    pidrol number;
    pidturnos number;
    phentra varchar2(5);
    i number;

    cursor turnos is select idturno from ph_roles_turnos where idrol = pidrol order by idregistro;

    begin
      select  tipo_marca,inicio_rol,idhorario into ptipoh,pfechar,pidrol from empleados where idplanilla = pidplanilla and idnumero = pidnumero;
      
      if ptipoh <> 'R' then return; end if;
      
      if ptipoh = 'R' then
        begin
          while pfechar < pfin
            loop
              begin
                for t in turnos loop
                 begin
                  select hora into phentra from marcas where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfechar and tipo = 1;
                  exception when no_data_found then phentra := '00:00';
                 end;
                 update marcas_mov_turnos set turno = t.idturno,hora = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfechar;
                 i := sql%rowcount;
                 commit;
                 if i = 0 then
                   begin
                     insert into marcas_mov_turnos (idregistro,idplanilla,idnumero,turno,fecha,hora,estado) values (reg_marcas_movturnos.nextval,pidplanilla,pidnumero,t.idturno,pfechar,phentra,'T');
                     commit;
                   end;
                 end if;
                 pfechar := pfechar + 1;
                end loop;
              end;
            end loop;
        end;
      end if;
    end;

end roles;
/

--procedure in_marcas
create or replace procedure geotime..in_marcas AS
 
begin
 declare 

  pidnumero varchar2(16);

  pidplanilla varchar2(8);
  pfecha date;
  phora varchar2(5);
  ptipo integer;
  pterminal varchar2(4);
  pregistro number;
  pidregistro number;
  pid varchar2(5);
	pfd varchar2(5);
	pdesc_pro number;
  cfecha_ant date;
  cfecha_desp date;  
  cfecha date;
  i number;
  

  cursor marca is
   select idplanilla, idnumero,fecha,hora,tipo,idterminal from marcas_in where idnumero is not null order by idplanilla,idnumero,fecha,hora;
   cursor marca_proc is
   select registro, idplanilla, idnumero, fecha, hora, tipo, idterminal,fecha_reg,long_reg,lat_reg from marcas where estado = 'N' order by idplanilla, idnumero, fecha, hora;

BEGIN 

  
  update marcas_in set (idplanilla,idnumero) = (select idplanilla,idnumero from empleados where marcas_in.idtarjeta = empleados.tarjeta and empleados.estado = 'T');
   commit;
  
   
   

   for r in marca loop
     begin  
       cfecha_ant := to_date(to_char(r.fecha,'YYYY-MM-DD')||' '||r.hora,'YYYY-MM-DD HH24:MI') + 5/1440*-1;
       cfecha_desp := to_date(to_char(r.fecha,'YYYY-MM-DD')||' '||r.hora,'YYYY-MM-DD HH24:MI') + 5/1440*1;     
       cfecha := to_date(to_char(r.fecha,'YYYY-MM-DD')||' '||r.hora,'YYYY-MM-DD HH24:MI');
      select registro into pregistro from (select registro from marcas where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha_hora between cfecha_ant and cfecha_desp ) where rownum = 1;
      exception   
       when no_data_found then pregistro := 0;
     end;  

     if pregistro = 0 then begin       
        begin       
        insert into marcas (registro,idplanilla, idnumero, fecha, hora, tipo, idterminal, fecha_hora) values (reg_marcas.nextval, r.idplanilla, r.idnumero, r.fecha, r.hora, r.tipo, r.idterminal,cfecha);
        commit;     
           exception when others then pregistro := 0;
           end;          
     end;
     end if;   
     delete from marcas_in where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = r.fecha and hora = r.hora and tipo = r.tipo;
     commit;
   end loop;
  
   for c in marca_proc loop      
     ptipo := c.tipo;
     if ptipo = 1 then       
       begin
        select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_entra = c.hora) where rownum = 1;
       exception when no_data_found then pregistro := 0;
       end;

       

       if pregistro = 0 then 
         begin
           select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_entra = '00:00' and (hora_sale > c.hora or hora_sale = '00:00')) where rownum = 1;
           exception when no_data_found then pregistro := 0;
         end;

         

         if pregistro = 0 then
           begin
             insert into marcas_proceso (idregistro,idplanilla,idnumero,fecha_entra,hora_entra,fecha_sale,hora_sale) values (reg_marcas_proceso.nextval,c.idplanilla,c.idnumero,c.fecha,c.hora,c.fecha,'00:00');
             commit;
             select reg_marcas_proceso.currval into pregistro from dual;
           end;
         else
          begin 
            update marcas_proceso set hora_entra = c.hora where idregistro = pregistro;
            commit;
          end; 
         end if;       
       end if;         

       update marcas_incidencias set idregistro = pregistro,hentra = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and (idregistro = 0 OR idregistro = pregistro) and hentra = '00:00';
       commit;

		   update marcas_mov_turnos set hora = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha;
       commit;

		   update marcas_extras_apb set hora = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha;
       commit;
     end if;

     

     if ptipo = 2 then        
       begin
        begin  
         select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and reg_sale = c.registro) where rownum = 1;
        exception when no_data_found then pregistro := 0;
        end;
        
        if pregistro = 0 then
          begin
            begin
             select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = (c.fecha - 1) and hora_sale = '00:00' and (1440 - hrs_min(hora_entra) + hrs_min(c.hora)) < 1080) where rownum = 1;
            exception when no_data_found then pregistro := 0;
            end; 
            
            if pregistro > 0 then
              begin
                update marcas_proceso set fecha_sale = c.fecha, hora_sale = c.hora,reg_sale = c.registro where idregistro = pregistro;
                commit;
              end;

            else
              begin
                begin
                 select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_sale = '00:00' and hora_entra < c.hora) where rownum = 1;
                exception when no_data_found then pregistro := 0;
                end;

                if pregistro = 0 then
                  begin
                    insert into marcas_proceso (idregistro,idplanilla,idnumero,fecha_entra,hora_sale,fecha_sale,hora_entra,reg_sale) values (reg_marcas_proceso.nextval,c.idplanilla,c.idnumero,c.fecha,c.hora,c.fecha,'00:00',c.registro);
                    commit;
                    select reg_marcas_proceso.currval into pregistro from dual;
                  end;
                  else
                   begin
                    update marcas_proceso set hora_sale = c.hora,reg_sale = pidregistro where idregistro = pregistro; 
                    commit;  
                   end;
                end if;  
              end;
            end if;    
          end;
          update marcas_incidencias set idregistro = c.registro,hsale = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and (idregistro = 0 OR idregistro = pregistro) and hsale = '00:00';
          commit;
        end if;  
       end;
     end if;

     

     if ptipo = 3 then
      begin
        begin
         select iddesc into pdesc_pro from (select iddesc from marcas_descansos where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha order by iddesc) where rownum = 1;
        exception when no_data_found then pdesc_pro := 0; 
        end;  
        pdesc_pro := pdesc_pro + 1;
        if pdesc_pro = 1 then
         begin
           insert into marcas_descansos (idregistro,iddesc, idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (c.registro,pdesc_pro,c.idplanilla,c.idnumero,c.fecha,c.hora,'00:00');
           commit;
         end;
        else
          begin
            update marcas_descansos set fin_desc = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha and inicio_desc <> '00:00' and fin_desc = '00:00';
            i := sql%rowcount;
			commit;            
            if i = 0 then insert into marcas_descansos (idregistro,iddesc, idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (c.registro,pdesc_pro,c.idplanilla,c.idnumero,c.fecha,c.hora,'00:00'); commit; end if;
          end;
        end if;   
      end;
     end if; 

   

     if ptipo = 4 then
       begin
         insert into marcas_comedor (idplanilla,idnumero,fecha,hora,idterminal) values (c.idplanilla,c.idnumero,c.fecha,c.hora,c.idterminal);
         commit;
       end;
     end if;  
   

     update marcas set estado = 'P' where registro = c.registro;
     commit;

   end loop;
 end;
END in_marcas;
/

--
-- Creating procedure ADD_ACCJUST
-- ==============================
--
create or replace procedure geotime.add_accjust (pidregistro number, pidjust number, pusuario varchar2, pcoment varchar2) AS

BEGIN
  declare 
  pidplanilla varchar2(8);
	pidnumero varchar2(16);
	pfecha date;
	phora varchar2(5);
	pindice number;
	pidj number;

  begin

    select idplanilla,idnumero,fecha_entra,hora_entra into pidplanilla,pidnumero,pfecha,phora from marcas_proceso where idregistro = pidregistro;
    pindice := 0;

    select indice,idincidencia into pindice,pidj from (select indice,idincidencia from (select indice,idincidencia from marcas_incidencias where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hentra = phora order by indice) where rownum = 1);

    if pindice > 0 then
      begin
        update marcas_incidencias set incidencia_just = pidj, idincidencia = pidjust, comentario = pcoment where indice = pindice;
        commit;
      end;
    else
      begin
        insert into marcas_incidencias (idplanilla,idnumero,fecha,hentra,idincidencia,idregistro,hsale,est_p,estado,usuario,fecha_just,comentario)
		                         values (pidplanilla,pidnumero,pfecha,phora,pidjust,pidregistro,'00:00','N','A',pusuario,sysdate,pcoment);
      end;
    end if;

  end;


END add_accjust;
/

--
-- Creating procedure ADD_ACCPERSONAL
-- ==================================
--
create or replace procedure geotime.add_accpersonal (pidplanilla varchar2,pidnumero varchar2, pinicio date, pfin date, pidincidencia number, pcomentario varchar2, pdias number, pusuario varchar2, pdias_apl varchar2) AS

BEGIN

 insert into acciones_personal (idregistro, idplanilla, idnumero, inicio, fin, idincidencia, estado, comentario, dias, usuario, fecha_just, dias_apl) values (idaccionper.nextval,pidplanilla,pidnumero,pinicio,pfin,pidincidencia,'N',pcomentario, pdias, pusuario, sysdate, pdias_apl);
 commit;
END add_accpersonal;
/

--
-- Creating procedure ADD_DIST_CC
-- ==============================
--
create or replace procedure geotime.add_dist_cc (pplanilla varchar2,pidnumero varchar2,pfecha date, pinicio varchar2, pfin varchar2, piddist number, pidreg number)
as

BEGIN

   declare pccosto varchar2(25);

   begin
     begin
      select idccosto into pccosto from ph_distribuciones_ccosto where idregistro = piddist;
      exception
      when no_data_found then return;
     end;

   if pidreg = 0 then
     begin
       insert into marcas_distribuciones_concepto (idplanilla,idnumero,fecha,inicio,fin,idccosto,iddist)
	                                       values (pplanilla,pidnumero,pfecha,pinicio,pfin,pccosto,piddist);
       commit;
     end;
   else
     begin
       update marcas_distribuciones_concepto set fecha = pfecha,inicio = pinicio,fin = pfin,idccosto = pccosto,iddist = piddist where idregistro = pidreg;
       commit;
     end;
   end if;

   end;

end add_dist_cc;
/

--
-- Creating procedure ADD_REGISTRO_TIEMPO
-- ======================================
--
create or replace procedure geotime.add_registro_tiempo (pplanilla varchar2, pidnumero varchar2,pfentra date, pfsale date, phentra varchar2, phsale varchar2, pturno number, pusuario varchar2, pcomentario varchar2)
as

BEGIN

   insert into marcas (registro,idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (reg_marcas.nextval,pplanilla,pidnumero,pfentra,phentra,1,'RM','P');
   commit;
   insert into marcas_audit (idregistro,idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (reg_marcas_audit.nextval,pplanilla,pidnumero,sysdate,pusuario,null,pfentra,'00:00',phentra,pcomentario);
   commit;
   insert into marcas (registro,idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (reg_marcas.nextval,pplanilla,pidnumero,pfsale,phsale,2,'RM','P');
   commit;
   insert into marcas_audit (idregistro,idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (reg_marcas_audit.nextval,pplanilla,pidnumero,sysdate,pusuario,null,pfsale,'00:00',phsale,pcomentario);
   commit;
   insert into marcas_proceso(idregistro,idplanilla, idnumero,fecha_entra,fecha_sale,hora_entra,hora_sale,idturno) values  (reg_marcas_proceso.nextval,pplanilla,pidnumero,pfentra,pfsale,phentra,phsale,pturno);
   commit;

end add_registro_tiempo;
/

--
-- Creating procedure ANULO_ACCPERSONAL
-- ====================================
--
create or replace procedure geotime.anulo_accpersonal (pidregistro number) AS

Begin
   declare
     i number;

BEGIN
      update acciones_personal set estado = 'D' where idregistro = pidregistro and estado <> 'S';
      i := sql%rowcount;
	  commit;      
      if i = 1 then
         delete from marcas_incidencias where idacc = pidregistro;
         commit;
      end if;

end;
END anulo_accpersonal;
/

--
-- Creating procedure APERTURA_PERIODO
-- ===================================
--
create or replace procedure geotime.apertura_periodo (pidplanilla varchar2, pgrupo number,pperiodo varchar2, pinicio date, pfin date, pusuario number) AS

begin

    update marcas_distribuciones_concepto set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;
    update marcas_distribuciones set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;

    update marcas_extras_apb set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;

    update marcas_incidencias set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;

    update marcas_mov_turnos set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;

    update marcas_proceso set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha_entra between pinicio and pfin;
    commit;

    update marcas_tiempo_adicional set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo)  and periodo = pperiodo;
    commit;

    update marcas_reportes set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;

    update marcas_proyecciones set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
    commit;

END apertura_periodo;
/

--
-- Creating procedure APLICO_ACCPERSONAL
-- =====================================
--
create or replace procedure geotime.aplico_accpersonal (pidregistro in number) AS

BEGIN
declare
  
  pfecha date;
  pentra varchar(5);
  psale varchar(5);
  
  pdl varchar(15);
  pda number;
  pcdias number;
  cpda number;
  i number;
  pidjust number;

  CURSOR datos IS
   select idplanilla,idnumero,inicio,fin,idincidencia,comentario,usuario,dias_apl from acciones_personal where idregistro = pidregistro and estado = 'N';

begin

   pcdias := 0;
   FOR r IN datos LOOP
     pfecha := r.inicio;     
     WHILE pfecha <= r.fin
       Loop         
         pda := dia_numero(pfecha);
         select count(*) into (cpda) from table(split_String(r.dias_apl)) where column_value = pda; 
         
         if cpda > 0 then
           begin 
             begin
               select idincidencia into pidjust from (select idincidencia from marcas_incidencias where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = pfecha and idincidencia < 7) where rownum = 1;
               exception when no_data_found then pidjust := 0;
             end;
             update marcas_incidencias set idincidencia = r.idincidencia,comentario = r.comentario,usuario = r.usuario,idacc = pidregistro,incidencia_just = pidjust where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = pfecha and idincidencia < 6  and idregistro in (select idregistro from (select idregistro from marcas_incidencias where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = pfecha and idincidencia < 6 order by hentra asc) where rownum = 1 );
             i := SQL%ROWCOUNT;
             commit;
         --dbms_output.put_line(i);
         if i = 0 then
           begin
             begin
               select hora into pentra from marcas where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = pfecha and tipo = 1;
               exception when no_data_found then pentra := '00:00';
             end;
             begin
               select hora into psale from marcas where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = pfecha and tipo = 2;
               exception when no_data_found then psale := '00:00';
             end;
             insert into marcas_incidencias (indice, idplanilla, idnumero, fecha, idincidencia, idregistro, hentra, hsale, est_p, comentario, usuario, idacc,incidencia_just)
                                              values (Reg_Marcas_Incidencias.Nextval,r.idplanilla,r.idnumero,pfecha,r.idincidencia,0,pentra,psale,'N',r.comentario,r.usuario, pidregistro,pidjust);
           end;
         end if;
            pcdias := pcdias + 1;
           end; 
         end if;
                 
         pfecha := pfecha + INTERVAL '1' DAY;
       end loop;
       update acciones_personal set estado = 'A',dias = pcdias where idregistro = pidregistro;
       commit;
   end loop;
delete from marcas_incidencias where indice is null;
commit;
END;
END aplico_accpersonal;
/

--
-- Creating procedure APRUEBO_EXTRA
-- ================================
--
create or replace procedure geotime.apruebo_extra (pcantidad varchar2, pcomentario varchar2, pusuario varchar2, pidregistro number, pccosto varchar2)
as

BEGIN
 declare
  
  pidplanilla varchar2(8);
  pidnumero varchar2(16);
  pfecha date;
  phora varchar2(5);
  i number;
 begin

   select  idplanilla, idnumero, fecha_entra, hora_entra into pidplanilla,pidnumero,pfecha,phora from marcas_proceso where idregistro = pidregistro;
     update marcas_proceso set EXTT = pcantidad where idregistro = pidregistro;
     commit;
     update marcas_extras_apb set cantidad = pcantidad, comentario = pcomentario, usuario = pusuario where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hora = phora;     
     i := sql%rowcount;
	 commit;
     if i = 0 then insert into marcas_extras_apb (idplanilla,idnumero,fecha,hora,cantidad,comentario, usuario) values (pidplanilla,pidnumero,pfecha,phora,pcantidad,pcomentario,pusuario); commit; end if;     
  end;
end apruebo_extra;
/

--
-- Creating procedure APRUEBO_EXTRA_MASIVA
-- =======================================
--
create or replace procedure geotime.apruebo_extra_masiva (pregistro number, pcantidad varchar2,pcomentario varchar2 ,pusuario varchar2) AS

BEGIN
	declare
	pplanilla varchar2(8);
	pidnumero varchar2(16);
	pfecha date;
	pentra varchar2(5);
  ccomentario varchar2(1024);
  i number;

begin

   if pcomentario is null or pcomentario = '' then ccomentario := 'Aprobada mediante proceso masivo de Extras por ' || pusuario;
   else ccomentario := pcomentario;
   end if;

   select  idplanilla, idnumero, fecha_entra, hora_entra into pplanilla,pidnumero,pfecha,pentra from marcas_proceso where idregistro = pregistro;

   if pcantidad = '00:00' then
     begin
       delete from marcas_extras_apb where idplanilla = pplanilla and idnumero = pidnumero and fecha = pfecha and hora = pentra;
       commit;
       update marcas_proceso set EXTT = pcantidad where idregistro = pregistro;
       commit;
     end;
   else
     begin
       update marcas_extras_apb set cantidad = pcantidad,usuario = pusuario,comentario = pcomentario where idplanilla = pplanilla and idnumero = pidnumero and fecha = pfecha and hora = pentra;       
       i := sql%rowcount;
	   commit;
       if i = 0 then
         insert into marcas_extras_apb (idplanilla, idnumero, fecha, hora, cantidad, comentario, usuario) values (pplanilla,pidnumero,pfecha,pentra,pcantidad,pcomentario,pusuario);
       end if;
       update marcas_proceso set EXTT = pcantidad where idregistro = pregistro;
     end;
   end if;


END;
END apruebo_extra_masiva;
/

--
-- Creating procedure APRUEBO_EXTRA_PERIODO
-- =======================================
--
create or replace procedure geotime.apruebo_extra_periodo (pidplanilla varchar2,pidnumero varchar2, pcomentario varchar2, pinicio date, pfin date, pusuario varchar2)
is
BEGIN

  declare
  pfecha date;
  phoras varchar2(5);
  phora varchar2(5);
  i     number;

  cursor datos is select fecha_entra,hora_entra,extc from marcas_proceso where idplanilla = pidplanilla and idnumero = pidnumero and fecha_entra between pinicio and pfin and extc <> '00:00';

  begin
    FOR r IN datos LOOP
       update marcas_extras_apb set cantidad = r.extc,comentario = pcomentario where idplanilla = pidplanilla and idnumero = pidnumero and fecha = r.fecha_entra and hora = r.hora_entra;
       i := SQL%ROWCOUNT;
       commit;
       if i = 0 then
          insert into marcas_extras_apb (idplanilla,idnumero,fecha,hora,cantidad,comentario,usuario) values (pidplanilla,pidnumero,r.fecha_entra,r.hora_entra,r.extc,pcomentario,pusuario);
          commit;
       end if;
       update marcas_proceso set extt = extc where idplanilla = pidplanilla and idnumero = pidnumero and fecha_entra = r.fecha_entra and hora_entra = r.hora_entra;
       commit;
    end loop;

  end;

END apruebo_extra_periodo;
/


--

--
-- Creating procedure BORRO_ACCPERSONAL
-- ====================================
--
create or replace procedure geotime.borro_accpersonal (pidregistro in number) AS

BEGIN

 delete from acciones_personal where idregistro = pidregistro and estado = 'N';
 commit;

END borro_accpersonal;
/

--
-- Creating procedure BORRO_REGISTRO
-- =================================
--
create or replace procedure geotime.borro_registro (pidregistro number, pusuario varchar2) AS
 
BEGIN 
declare 
 pfentra date;
 pfsale date;
 phentra varchar2(5);
 phsale varchar2(5);
 pcompania varchar2(8);
 pplanilla varchar2(8);
 pidnumero varchar2(16);

begin

 select idplanilla,idnumero, fecha_entra, fecha_sale, hora_entra, hora_sale into pplanilla,pidnumero,pfentra,pfsale,phentra,phsale from marcas_proceso where idregistro = pidregistro;
 
        delete from marcas where idplanilla = pplnilla and idnumero = pidnumero and fecha = pfentra and hora = phentra;
  commit;
	delete from marcas where idplanilla = pplnilla and idnumero = pidnumero and fecha = pfsale and hora = phsale;
  commit;
	delete from marcas_proceso where idregistro = pidregistro;
  commit;
	delete from marcas_incidencias where idplanilla = pplnilla and idnumero = pidnumero and fecha = pfentra and hentra = phentra and idincidencia <= 7;
  commit;
  
  insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (pplanilla,pidnumero,sysdate,pusuario,pfentra,null,phentra,null,'Registro Eliminado');
  commit;
	insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (pplanilla,pidnumero,sysdate,pusuario,pfsale,null,phsale,null,'Registro Eliminado');
  commit;

END;
END borro_registro;
/

--
-- Creating procedure CIERRO_PERIODO
-- =================================
--
create or replace procedure geotime.cierro_periodo (pidplanilla varchar2, pgrupo number,pperiodo varchar2, pinicio date, pfin date, pusuario number) AS
BEGIN

declare
  marcas_abiertas number;
  estp char(1);
  piniciop date;
  pfinp date;
  piniciopant date;
  pfinpant date;
  pperant varchar2(8);

begin

 update marcas_distribuciones_concepto set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
 commit;
 update marcas_distribuciones set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
 commit;
 update marcas_extras_apb set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
 commit;
 update marcas_incidencias set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
 commit;
 update marcas_mov_turnos set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
 commit;
 update marcas_proceso set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha_entra between pinicio and pfin;
 commit;
 update marcas_tiempo_adicional set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and periodo = pperiodo;
 commit;
 update marcas_reportes set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between pinicio and pfin;
 commit;
 update ph_grupo_periodo set usuario_cierre = pusuario, fecha_cierre = sysdate where usuario_cierre is null
 commit;

 marcas_abiertas := 1;
 select count(*) into marcas_abiertas from marcas_proceso where fecha_entra between pinicio and pfin and estado = 'A' and idplanilla = pidplanilla;
 if marcas_abiertas = 0 then update ph_periodos set estado = 'C' where idperiodo = pperiodo; end if;

 select proyecta into estp from ph_planilla where tipo_planilla = (select tipo_planilla from ph_periodos where idperiodo = pperiodo);

 if estp = 'T' then
   begin
     select inicio_proy,fin_proy,periodo_proy into piniciop,pfinp,pperant from ph_periodos where idperiodo = pperiodo;

     update marcas_proyecciones set estado = 'P' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;
     commit;
     update marcas_distribuciones_concepto set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;
     commit;
     update marcas_distribuciones set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;
     commit;
     update marcas_extras_apb set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;
     commit;
     update marcas_incidencias set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;commit;
     commit;
     update marcas_mov_turnos set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;
     commit;
     update marcas_proceso set estado = 'A' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha_entra between piniciop and pfinp;
     commit;
     update marcas_reportes set estado = 'A' where idplanilla = pidplanilla and   idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciop and pfinp;
     commit;

     if pperant is not null then
       begin
         select inicio_proy,fin_proy into piniciopant,pfinpant from ph_periodos where idperiodo = pperant;
         update marcas_proyecciones set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
         update marcas_distribuciones_concepto set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
         update marcas_distribuciones set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
         update marcas_extras_apb set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
         update marcas_incidencias set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
         update marcas_mov_turnos set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
         update marcas_proceso set estado = 'C' where idplanilla = pidplanilla and idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha_entra between piniciopant and pfinpant;
         commit;
         update marcas_reportes set estado = 'C' where idplanilla = pidplanilla and   idnumero in  (select idnumero from empleados where idgrupo = pgrupo) and fecha between piniciopant and pfinpant;
         commit;
       end;
     end if;

   end;
 end if;

END;
END cierro_periodo;
/


--
-- Creating procedure DEP_ADD
-- ==========================
--
create or replace procedure geotime.dep_add (pdepartamento varchar2, piddep varchar2, ped char, poldid varchar2, polddep varchar2, pids number)
as

BEGIN

   declare ex number;

   begin

   if ped = 'F' then
    begin
      insert into ph_departamento (iddepart, descripcion) values (piddep, pdepartamento) ;
    end;
  else
    begin
      if ped = 'T' then update ph_departamento set iddepart = piddep, descripcion = pdepartamento where iddepart = polddep ; end if;
    end;
   end if;

   end;

end dep_add;
/

--
-- Creating procedure DM_POST_CALCULO_EMPLEADO
-- ===========================================
--
create or replace procedure geotime.DM_POST_CALCULO_EMPLEADO (pidplanilla varchar2,pidnumero varchar2 ,pperiodo varchar2, pinicio date, pfin date) AS
 
BEGIN 
  
 delete from marcas_resumen where idplanilla = pidplanilla and idnumero = pidnumero and cantidad = 0;
 commit;
 return;
END DM_POST_CALCULO_EMPLEADO;
/

--
-- Creating procedure DM_POST_SINCRONIZA
-- =====================================
--
create or replace procedure geotime.DM_POST_SINCRONIZA (pidplanilla varchar2) AS
 
BEGIN 
 return;
END DM_POST_SINCRONIZA;
/

--
-- Creating procedure INI_MARCAS
-- =============================
--
create or replace procedure geotime.ini_marcas (pidplanilla in varchar2 , pperiodo in varchar2) AS

BEGIN 
declare 
        pinicio date; 
        pfin date; 
        pidregistro number; 
        pffin date; 
        i number;
 
        cursor empleado is 
        select idnumero from empleados where idplanilla = pidplanilla and estado = 'T'; 
        
        cursor grupos_i is
        select distinct idgrupo from empleados where idplanilla = pidplanilla and estado = 'T';
 
begin 
 
     select inicio,fin into pinicio,pfin from ph_periodos where idperiodo = pperiodo; 
 
     for e in empleado loop 
 
      pffin := pinicio; 
       while pffin <= pfin 
         loop 
           begin 
            select idregistro into pidregistro from (select idregistro from marcas_proceso where idplanilla = pidplanilla and idnumero = e.idnumero and fecha_entra = pffin) where rownum = 1; 
           exception when no_data_found then pidregistro := 0; 
           end; 
 
           if pidregistro = 0 or pidregistro is null then 
             insert into marcas_proceso (idregistro,idplanilla, idnumero, fecha_entra, fecha_sale, hora_entra, hora_sale, idturno) values (reg_marcas_proceso.nextval,pidplanilla,e.idnumero,pffin,pffin,'00:00','00:00',0); 
             commit; 
           end if; 
 
          pffin := pffin + 1; 
         end loop; 
		 begin roles (pidplanilla, e.idnumero,pinicio,pfin); end;
     end loop; 
update ph_periodos set estado = 'T' where idperiodo = pperiodo; 
commit; 
update CTADMIN.ph_login set fperiodo = pperiodo; 
commit; 
update ph_usuario set fperiodo = pperiodo;
commit;

for g in grupos_i loop
  update ph_grupo_periodo set estado = 'A' where idgrupo = g.idgrupo and idperiodo = pperiodo and idplanilla = pidplanilla;  
  i := sql%rowcount;
  commit;
  if i = 0 then 
  insert into ph_grupo_periodo (idgrupo,idperiodo,estado,idplanilla) values (g.idgrupo,pperiodo,'A',pidplanilla);
  commit;  
   end if;
  
end loop;

END; 
END ini_marcas; 
/

create or replace procedure geotime.ini_marcas_all (pperiodo in varchar2) AS

BEGIN 
declare 
        pinicio date; 
        pfin date; 
        pidregistro number; 
        pffin date; 
        i number;
        p_tipo_planilla char(1);
        p_planilla varchar2(8);
        p_idplanilla varchar2(8);
 
        cursor empleado is 
        select idnumero from empleados where idplanilla in (select idplanilla from ph_planilla where tipo_planilla = p_tipo_planilla) and estado = 'T'; 
        
        cursor grupos_i is
        select distinct idgrupo from empleados where idplanilla = p_idplanilla and estado = 'T';

        cursor lplanillas is
        select idplanilla from ph_planilla where tipo_planilla in (select tipo_planilla from GSIT.ph_periodos where idperiodo = pperiodo)
 
begin 
 
     select inicio,fin,tipo_planilla into pinicio,pfin,p_tipo_planilla from ph_periodos where idperiodo = pperiodo; 
 
     for e in empleado loop 
 
      pffin := pinicio; 
       while pffin <= pfin 
         loop 
           begin 
            select idregistro into pidregistro from (select idregistro from marcas_proceso where idplanilla = e.idplanilla and idnumero = e.idnumero and fecha_entra = pffin) where rownum = 1; 
           exception when no_data_found then pidregistro := 0; 
           end; 
 
           if pidregistro = 0 or pidregistro is null then 
             insert into marcas_proceso (idregistro,idplanilla, idnumero, fecha_entra, fecha_sale, hora_entra, hora_sale, idturno) values (reg_marcas_proceso.nextval,e.idplanilla,e.idnumero,pffin,pffin,'00:00','00:00',0); 
             commit; 
           end if; 
 
          pffin := pffin + 1; 
         end loop; 
		 begin roles (e.idplanilla, e.idnumero,pinicio,pfin); end;
     end loop; 
update ph_periodos set estado = 'T' where idperiodo = pperiodo; 
commit;  

for p in lplanillas loop
p_idplanilla := p.idplanilla;
for g in grupos_i loop
  update ph_grupo_periodo set estado = 'A' where idgrupo = g.idgrupo and idperiodo = pperiodo and idplanilla = g.idplanilla;  
  i := sql%rowcount;
  commit;
  if i = 0 then 
  insert into ph_grupo_periodo (idgrupo,idperiodo,estado,idplanilla) values (g.idgrupo,pperiodo,'A',pidplanilla);
  commit;  
   end if;
  
end loop;

end loop;

END; 
END ini_marcas_all; 


--
-- Creating procedure IN_MARCAS
-- ============================
--
create or replace procedure geotime.in_marcas AS
 
begin
 declare 

  pidnumero varchar2(16);

  pidplanilla varchar2(8);
  pfecha date;
  phora varchar2(5);
  ptipo integer;
  pterminal varchar2(4);
  pregistro number;
  pidregistro number;
  pid varchar2(5);
	pfd varchar2(5);
	pdesc_pro number;
  cfecha_ant date;
  cfecha_desp date;  
  i number;
  

  cursor marca is
   select idplanilla, idnumero,fecha,hora,tipo,idterminal from marcas_in where idnumero is not null order by idplanilla,idnumero,fecha,hora;
   cursor marca_proc is
   select registro, idplanilla, idnumero, fecha, hora, tipo, idterminal,fecha_reg,long_reg,lat_reg from marcas where estado = 'N' order by idplanilla, idnumero, fecha, hora;

BEGIN 

  update marcas_in set (idplanilla,idnumero) = (select idplanilla,idnumero from empleados where marcas_in.idtarjeta = empleados.tarjeta);
   commit;
   update marcas_in set (idplanilla,idnumero) = (select idplanilla,idnumero from empleados where marcas_in.idtarjeta = empleados.idnumero);
   commit;   

   for r in marca loop
     begin  
       cfecha_ant := to_date(to_char(r.fecha,'YYYY-MM-DD')||' '||r.hora,'YYYY-MM-DD HH24:MI') + 5/1440*-1;
       cfecha_desp := to_date(to_char(r.fecha,'YYYY-MM-DD')||' '||r.hora,'YYYY-MM-DD HH24:MI') + 5/1440*1;     
      select registro into pregistro from (select registro from marcas where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha_hora between cfecha_ant and cfecha_desp ) where rownum = 1;
      exception   
       when no_data_found then pregistro := 0;
     end;  

     if pregistro = 0 then begin       
        insert into marcas (registro,idplanilla, idnumero, fecha, hora, tipo, idterminal, fecha_hora) values (reg_marcas.nextval, r.idplanilla, r.idnumero, r.fecha, r.hora, r.tipo, r.idterminal,to_date(to_char(r.fecha,'YYYY-MM-DD')||' '||r.hora,'YYYY-MM-DD HH24:MI'));
        commit;        
     end;
     end if;   
     delete from marcas_in where idplanilla = r.idplanilla and idnumero = r.idnumero and fecha = r.fecha and hora = r.hora and tipo = r.tipo;
     commit;
   end loop;
  
   for c in marca_proc loop      
     ptipo := tipo(c.idplanilla,c.idnumero,c.fecha,c.hora);       
     update marcas set tipo = ptipo where registro = c.registro;       
     commit;

     if ptipo = 1 then       
       begin
        select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_entra = c.hora) where rownum = 1;
       exception when no_data_found then pregistro := 0;
       end;

       

       if pregistro = 0 then 
         begin
           select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_entra = '00:00' and (hora_sale > c.hora or hora_sale = '00:00')) where rownum = 1;
           exception when no_data_found then pregistro := 0;
         end;

         

         if pregistro = 0 then
           begin
             insert into marcas_proceso (idregistro,idplanilla,idnumero,fecha_entra,hora_entra,fecha_sale,hora_sale) values (reg_marcas_proceso.nextval,c.idplanilla,c.idnumero,c.fecha,c.hora,c.fecha,'00:00');
             commit;
             select reg_marcas_proceso.currval into pregistro from dual;
           end;
         else
          begin 
            update marcas_proceso set hora_entra = c.hora where idregistro = pregistro;
            commit;
          end; 
         end if;       
       end if;         

       update marcas_incidencias set idregistro = pregistro,hentra = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and (idregistro = 0 OR idregistro = pregistro) and hentra = '00:00';
       commit;

		   update marcas_mov_turnos set hora = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha;
       commit;

		   update marcas_extras_apb set hora = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha;
       commit;
     end if;

     

     if ptipo = 2 then        
       begin
        begin  
         select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and reg_sale = c.registro) where rownum = 1;
        exception when no_data_found then pregistro := 0;
        end;
        
        if pregistro = 0 then
          begin
            begin
             select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = (c.fecha - 1) and hora_sale = '00:00' and (1440 - hrs_min(hora_entra) + hrs_min(c.hora)) < 1080) where rownum = 1;
            exception when no_data_found then pregistro := 0;
            end; 
            
            if pregistro > 0 then
              begin
                update marcas_proceso set fecha_sale = c.fecha, hora_sale = c.hora,reg_sale = c.registro where idregistro = pregistro;
                commit;
              end;

            else
              begin
                begin
                 select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_sale = '00:00' and hora_entra < c.hora) where rownum = 1;
                exception when no_data_found then pregistro := 0;
                end;

                if pregistro = 0 then
                  begin
                    insert into marcas_proceso (idregistro,idplanilla,idnumero,fecha_entra,hora_sale,fecha_sale,hora_entra,reg_sale) values (reg_marcas_proceso.nextval,c.idplanilla,c.idnumero,c.fecha,c.hora,c.fecha,'00:00',c.registro);
                    commit;
                    select reg_marcas_proceso.currval into pregistro from dual;
                  end;
                  else
                   begin
                    update marcas_proceso set hora_sale = c.hora,reg_sale = pidregistro where idregistro = pregistro; 
                    commit;  
                   end;
                end if;  
              end;
            end if;    
          end;
          update marcas_incidencias set idregistro = c.registro,hsale = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and (idregistro = 0 OR idregistro = pregistro) and hsale = '00:00';
          commit;
        end if;  
       end;
     end if;

     

     if ptipo = 3 then
      begin
        begin
         select iddesc into pdesc_pro from (select iddesc from marcas_descansos where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha order by iddesc) where rownum = 1;
        exception when no_data_found then pdesc_pro := 0; 
        end;  
        pdesc_pro := pdesc_pro + 1;
        if pdesc_pro = 1 then
         begin
           insert into marcas_descansos (idregistro,iddesc, idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (c.registro,pdesc_pro,c.idplanilla,c.idnumero,c.fecha,c.hora,'00:00');
           commit;
         end;
        else
          begin
            update marcas_descansos set fin_desc = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha and inicio_desc <> '00:00' and fin_desc = '00:00';
            i := sql%rowcount;
			commit;            
            if i = 0 then insert into marcas_descansos (idregistro,iddesc, idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (c.registro,pdesc_pro,c.idplanilla,c.idnumero,c.fecha,c.hora,'00:00'); commit; end if;
          end;
        end if;   
      end;
     end if; 

   

     if ptipo = 4 then
       begin
         insert into marcas_comedor (idplanilla,idnumero,fecha,hora,idterminal) values (c.idplanilla,c.idnumero,c.fecha,c.hora,c.idterminal);
         commit;
       end;
     end if;  
   

     update marcas set estado = 'P' where registro = c.registro;
     commit;

   end loop;
 end;
END in_marcas;
/

--
-- Creating procedure PLAN_ADD
-- ===========================
--
create or replace procedure geotime.plan_add (pidplanilla in varchar, pplanilla in varchar, pnom_conector in varchar, ped in char, poldid in varchar, pids in number, pextc in char, pinic_c in char, padic in char, pmdesc in char, ptnom in char, pproyecta in char, diasem in number) AS
 
BEGIN 
declare ex number;

begin

  if ped = 'F' then 
    begin 
      insert into ph_planilla (idplanilla, planilla, nom_conector,c_ext,c_inci,c_adic,m_desc,tipo_planilla,proyecta,dia_inicio) values (pidplanilla, pplanilla, pnom_conector,pextc,pinic_c,padic,pmdesc,ptnom,pproyecta,diasem);
    end;  
  else
    begin
      if ped = 'T' then update ph_planilla set idplanilla = pidplanilla, planilla = pplanilla, nom_conector = pnom_conector,c_ext = pextc,c_inci = pinic_c,c_adic = padic, m_desc = pmdesc,tipo_planilla=ptnom, proyecta = pproyecta, dia_inicio = diasem where idplanilla = poldid ; end if; 
    end;
   end if; 

END;
END plan_add;
/

--
-- Creating procedure POST_SINCRONIZO_ERP
-- ======================================
--
create or replace procedure geotime.post_sincronizo_erp (pidplanilla in varchar2) AS
 
BEGIN 
declare ex number;

begin

 return; 

END;
END post_sincronizo_erp;
/

--
-- Creating procedure PRE_PROCESO_COMP
-- ===================================
--
create or replace procedure geotime.PRE_PROCESO_COMP (pidplanilla varchar2,pperiodo varchar2) AS

BEGIN
  
return;

END PRE_PROCESO_COMP;
/

--
-- Creating procedure PROCESO_EMP
-- ==============================
--
create or replace procedure geotime.proceso_emp (pidplanilla in varchar2,pidnumero in varchar2 ,pperiodo in varchar2, pinicio in date, pfin in date) AS
 
BEGIN 
declare 
   pindice number;
   pdiasp number;
   pfecha_ultpago date;
   pdias_continua number;
   pdias_pagados number;
   pfecha_incidencia date;
   
   cursor inci_empleado is
        select fecha from marcas_incidencias where idplanilla = pidplanilla and idnumero = pidnumero and idincidencia = pindice and fecha between pinicio and pfin;
   
begin

 pindice := 42;
 pdiasp := 3;
 
  update marcas_incidencias set est_p = 'F' where idplanilla = pidplanilla and idnumero = pidnumero and idincidencia = pindice and fecha between pinicio and pfin;
  commit;
  
  begin
   select fecha into pfecha_ultpago from (select fecha from marcas_incidencias where idplanilla = pidplanilla and idnumero = pidnumero and idincidencia = pindice and est_p = 'P' order by fecha desc) where rownum = 1;
  exception when no_data_found then pfecha_ultpago := to_date('2000-01-01','yyyy-mm-dd');
  end;
  
  --selecciono cant dias a pagar en incidencia Caja
  
  if pfecha_ultpago < pinicio - 30 then
    begin
      pdiasp := 3;
    end;  
  else
    begin
      select count(*) into pdias_pagados from marcas_incidencias where idplanilla = pidplanilla and idnumero = pidnumero and idincidencia = pindice and fecha between pinicio - 30 and pinicio - 1 and est_p = 'P';
      if pdias_pagados >= 3 then pdiasp := 0; 
      else pdiasp := 3 - pdias_pagados;
      end if;      
    end;  
  end if;  
  
  --pago incidencias
  if pdiasp > 0 then
    begin
      for e in inci_empleado loop
        if pdiasp > 0 then
          begin
            update marcas_incidencias set est_p = 'P' where idplanilla = pidplanilla and idnumero = pidnumero and idincidencia = pindice and fecha = e.fecha;
            commit;
            pdiasp := pdiasp - 1;
          end;
        end if;  
      end loop;
    end;
  end if;  
  
  
END;
END proceso_emp;
/

--
-- Creating procedure SALVO_MARCA
-- ==============================
--
create or replace procedure geotime.salvo_marca (pfentra date, pfsale date, phentra varchar2, phsale varchar2, pturno number, pidregistro number, pusuario varchar2, pcomentario varchar2)
as

BEGIN

   declare
	 pidplanilla varchar2(8);
	 pidnumero varchar2(16);
	 pfecha_entra date;
	 pfecha_sale date;
	 phora_entra varchar2(5);
	 phora_sale varchar2(5);
	 pidturno number;
   i number;

   begin

     begin
      select idplanilla, idnumero, fecha_entra, fecha_sale, hora_entra, hora_sale,idturno into pidplanilla,pidnumero,pfecha_entra,pfecha_sale,phora_entra,phora_sale,pidturno from marcas_proceso where idregistro = pidregistro;
     exception
      when no_data_found then return;
     end;

     update marcas_proceso set fecha_entra = pfentra, fecha_sale = pfsale, hora_entra = phentra, hora_sale = phsale, idturno = pturno where idregistro = pidregistro;
     commit;

     if pfecha_entra <> pfentra and phora_entra <> phentra then
       begin
         update marcas_extras_apb set fecha = pfentra,hora = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phora_entra;
         commit;
         update marcas_mov_turnos set fecha = pfentra,hora = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phora_entra;
         commit;
       end;
     end if;

     if phora_entra <> phentra then
       begin
         update marcas set hora = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phora_entra and tipo = 1;         
         i := sql%rowcount;
		 commit;
         if i = 0 then insert into marcas (registro, idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (reg_marcas.nextval,pidplanilla,pidnumero,pfentra,phentra,1,'RM','P'); commit; end if;
         update marcas_extras_apb set hora = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phora_entra;
         commit;
	       update marcas_incidencias set hentra = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hentra = phora_entra;
         commit;
	       update marcas_mov_turnos set hora = phentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfentra and hora = phora_entra;
         commit;
         insert into marcas_audit (idregistro,idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (reg_marcas_audit.nextval,pidplanilla,pidnumero,sysdate,pusuario,pfecha_entra,pfentra,phora_entra,phentra,pcomentario);
         commit;
       end;
     end if;

     if phora_sale <> phsale then
       begin
         update marcas set hora = phsale where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_sale and hora = phora_sale and tipo = 2;
         commit;
         update marcas_incidencias set hsale = phsale where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hsale = phora_sale;         
         i := sql%rowcount;
		 commit;
         if i = 0 then insert into marcas (registro, idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (reg_marcas.nextval,pidplanilla,pidnumero,pfsale,phsale,2,'RM','P'); commit; end if;
         insert into marcas_audit (idregistro,idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (reg_marcas_audit.nextval,pidplanilla,pidnumero,sysdate,pusuario,pfecha_sale,pfsale,phora_sale,phsale,pcomentario);
         commit;
       end;
     end if;

     if pfecha_entra <> pfentra then
       begin
         update marcas set fecha = pfentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phora_entra and tipo = 1;
         commit;
		     update marcas_extras_apb set fecha = pfentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phora_entra;
         commit;
		     update marcas_mov_turnos set fecha = pfentra where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_entra and hora = phentra;
         commit;
         insert into marcas_audit (idregistro,idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (reg_marcas_audit.nextval,pidplanilla,pidnumero,sysdate,pusuario,pfecha_entra,pfentra,phora_entra,phora_entra,pcomentario);
         commit;
       end;
     end if;

     if pfecha_sale <> pfsale then
       begin
         update marcas set fecha = pfsale where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha_sale and hora = phora_sale and tipo = 2;
         commit;
         insert into marcas_audit (idregistro,idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (reg_marcas_audit.nextval,pidplanilla,pidnumero,sysdate,pusuario,pfecha_sale,pfsale,phora_sale,phsale,pcomentario);
         commit;
       end;
     end if;

     if pidturno <> pturno then
       begin
         update marcas_mov_turnos set turno = pturno where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfentra and hora = phentra;         
         i := sql%rowcount;
		 commit;
         if i = 0 then insert into marcas_mov_turnos (idregistro,idplanilla, idnumero, fecha, hora, turno) values (REG_MARCAS_MOVTURNOS.NEXTVAL,pidplanilla,pidnumero,pfentra,phentra,pturno); commit; end if;
       end;
     end if;

   end;

end salvo_marca;
/

--
-- Creating procedure SALVO_PROG_TURNO
-- ===================================
--
create or replace procedure geotime.salvo_prog_turno (pidplanilla varchar2, pidnumero varchar2, pfecha date, pturno number, pusuario varchar2) AS

BEGIN
  declare
  phora varchar2(5);
  i number;

  begin
   begin
      select hora_entra into phora from (select hora_entra from marcas_proceso where idplanilla = pidplanilla and idnumero = pidnumero and fecha_entra = pfecha order by hora_entra) where rownum = 1;
      exception when no_data_found then phora := '00:00';
   end;
   update marcas_mov_turnos set turno = pturno,usuario = pusuario,fecha_reg = sysdate where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hora = phora;
   i := sql%rowcount;
   commit;   
   if i = 0 then
     begin
       insert into marcas_mov_turnos (idregistro,idplanilla,idnumero,fecha,hora,turno,usuario,fecha_reg)
	                         values (idtransturno.nextval,pidplanilla,pidnumero,pfecha,phora,pturno,pusuario,sysdate);
       commit;
     end;
   end if;
  end;


END salvo_prog_turno;
/

--
-- Creating procedure SALVO_PROG_Horario
-- ===================================
--
create or replace procedure geotime.salvo_prog_horario (pidplanilla varchar2, pidnumero varchar2, pfecha date, phorario number, pusuario varchar2) AS

BEGIN
  declare
  phora varchar2(5);
  i number;

  begin
   begin
      select hora_entra into phora from (select hora_entra from marcas_proceso where idplanilla = pidplanilla and idnumero = pidnumero and fecha_entra = pfecha order by hora_entra) where rownum = 1;
      exception when no_data_found then phora := '00:00';
   end;
   update marcas_mov_horarios set idhorario = phorario,usuario = pusuario,fecha_reg = sysdate where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hora = phora;
   i := sql%rowcount;
   commit;   
   if i = 0 then
     begin
       insert into marcas_mov_horarios (idregistro,idplanilla,idnumero,fecha,hora,idhorario,usuario,fecha_reg)
	                         values (idtransturno.nextval,pidplanilla,pidnumero,pfecha,phora,phorario,pusuario,sysdate);
       commit;
     end;
   end if;
  end;


END salvo_prog_horario;
/

--
-- Creating procedure SALVO_PROG_TURNO_mlinea
-- ===================================
--
create or replace procedure geotime.salvo_prog_turno_mlinea (pidplanilla varchar2, pidnumero varchar2, pfecha date, pturno number, pusuario varchar2, plinea number) AS

BEGIN
  declare
  phora varchar2(5);
  i number;

  begin
   begin
      select hora_entra into phora from (select hora_entra from marcas_proceso where idplanilla = pidplanilla and idnumero = pidnumero and fecha_entra = pfecha order by hora_entra) where rownum = 1;
      exception when no_data_found then phora := '00:00';
   end;
   update marcas_mov_turnos set turno = pturno,usuario = pusuario,fecha_reg = sysdate where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hora = phora and linea = plinea;
   i := sql%rowcount;
   commit;   
   if i = 0 then
     begin
       insert into marcas_mov_turnos (idregistro,idplanilla,idnumero,fecha,hora,turno,usuario,fecha_reg,linea)
	                         values (idtransturno.nextval,pidplanilla,pidnumero,pfecha,phora,pturno,pusuario,sysdate,plinea);
       commit;
     end;
   end if;
  end;


END salvo_prog_turno_mlinea;
/

--
-- Creating procedure Ordeno Marcas
-- ===================================
--
create or replace procedure geotime.ordeno_marcas(pplanilla varchar2, pidnumero varchar2) AS
begin
  declare
      
      pidplanilla varchar2(8);
      pfecha date;
      phora varchar2(5);
      ptipo integer;
      pterminal varchar2(4);
      pregistro number;
      pidregistro number;
      pid varchar2(5);
	    pfd varchar2(5);
	    pdesc_pro number;
      cfecha_ant date;
      cfecha_desp date; 
      i number;
  
      cursor marca_proc is
      select registro, idplanilla, idnumero, fecha, hora, tipo, idterminal,fecha_reg,long_reg,lat_reg from extradm_ca.marcas where idplanilla = pplanilla and idnumero = pidnumero and estado = 'C' order by idplanilla, idnumero, fecha, hora;
      
   begin
     for c in marca_proc loop
     --dbms_output.Put_line(c.registro);  
     
       if c.tipo = 1 then      
       begin
        select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_entra = c.hora) where rownum = 1;
       exception when no_data_found then pregistro := 0;
       end;

       dbms_output.Put_line(pregistro); 

       if pregistro = 0 then 
         begin
           select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_entra = '00:00' and (hora_sale > c.hora or hora_sale = '00:00')) where rownum = 1;
           exception when no_data_found then pregistro := 0;
         end;         

         if pregistro = 0 then
           begin
             insert into marcas_proceso (idregistro,idplanilla,idnumero,fecha_entra,hora_entra,fecha_sale,hora_sale) values (reg_marcas_proceso.nextval,c.idplanilla,c.idnumero,c.fecha,c.hora,c.fecha,'00:00');
             commit;
             select reg_marcas_proceso.currval into pregistro from dual;
           end;
         else
          begin  
            update marcas_proceso set hora_entra = c.hora where idregistro = pregistro;
            commit;
          end; 
         end if;         
       end if;         
       update marcas_incidencias set idregistro = pregistro,hentra = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and (idregistro = 0 OR idregistro = pregistro) and hentra = '00:00';
       commit;
       update marcas_mov_turnos set hora = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha;
       commit;
       update marcas_extras_apb set hora = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha;
       commit;      
     end if;

     

     if c.tipo = 2 then        
       begin
        begin  
         select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and reg_sale = c.registro) where rownum = 1;
        exception when no_data_found then pregistro := 0;
        end;
        
        if pregistro = 0 then
          begin
            begin
             select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = (c.fecha - 1) and hora_sale = '00:00' and (1440 - EXTRADM_CA.hrs_min(hora_entra) + EXTRADM_CA.hrs_min(c.hora)) < 1080) where rownum = 1;
            exception when no_data_found then pregistro := 0;
            end; 
            
            if pregistro > 0 then
              begin
                update marcas_proceso set fecha_sale = c.fecha, hora_sale = c.hora,reg_sale = c.registro where idregistro = pregistro;
                commit;
              end;
            else
              begin
                begin
                 select idregistro into pregistro from (select idregistro from marcas_proceso where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha_entra = c.fecha and hora_sale = '00:00' and hora_entra < c.hora) where rownum = 1;
                exception when no_data_found then pregistro := 0;
                end;
                if pregistro = 0 then
                  begin
                    insert into marcas_proceso (idregistro,idplanilla,idnumero,fecha_entra,hora_sale,fecha_sale,hora_entra,reg_sale) values (reg_marcas_proceso.nextval,c.idplanilla,c.idnumero,c.fecha,c.hora,c.fecha,'00:00',c.registro);
                    commit;
                    select reg_marcas_proceso.currval into pregistro from dual;
                  end;
                  else
                   begin
                    update marcas_proceso set hora_sale = c.hora,reg_sale = pidregistro where idregistro = pregistro; 
                    commit;  
                   end;
                end if;  
              end;
            end if;    
          end;
          update marcas_incidencias set idregistro = c.registro,hsale = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and (idregistro = 0 OR idregistro = pregistro) and hsale = '00:00';
          commit;
        end if;
       end;
     end if;

     

     if c.tipo = 3 then
      begin
        begin
         select iddesc into pdesc_pro from (select iddesc from marcas_descansos where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha order by iddesc) where rownum = 1;
        exception when no_data_found then pdesc_pro := 0;
        end;  

        pdesc_pro := pdesc_pro + 1;
        if pdesc_pro = 1 then
         begin
           insert into marcas_descansos (idregistro,iddesc, idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (c.registro,pdesc_pro,c.idplanilla,c.idnumero,c.fecha,c.hora,'00:00');
           commit;
         end;
        else
          begin
            update marcas_descansos set fin_desc = c.hora where idplanilla = c.idplanilla and idnumero = c.idnumero and fecha = c.fecha and inicio_desc <> '00:00' and fin_desc = '00:00';
            i := sql%rowcount;
            commit;
            if i = 0 then insert into marcas_descansos (idregistro,iddesc, idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (c.registro,pdesc_pro,c.idplanilla,c.idnumero,c.fecha,c.hora,'00:00'); commit; end if;
          end;
        end if;
      end;
     end if; 

   

     --if c.tipo = 4 then
       --begin
         --insert into marcas_comedor (idregistro,idplanilla,idnumero,fecha,hora,idterminal) values (c.registro,c.idplanilla,c.idnumero,c.fecha,c.hora,c.idterminal);
         --commit;
       --end;
     --end if;  

   

     update marcas set estado = 'P' where registro = c.registro;
     commit;
     end loop;
   
  end;
end ordeno_marcas;
/
