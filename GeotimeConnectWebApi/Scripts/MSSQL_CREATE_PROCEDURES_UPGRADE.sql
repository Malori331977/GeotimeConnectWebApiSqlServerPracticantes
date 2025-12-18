
create procedure [dbo].[DM_CONSULTAR_MARCAS_PERIODO]
(
	@IdsGrupos varchar(1000),
	@IdPlanilla varchar(8),
	@FechaInicio DateTime,
	@FechaFin DateTime,
	@idnumero varchar(20),
	@idcomp varchar(100)
)
as
begin 

	declare @strSQL nvarchar(max)

	declare @tGrupos table (idgrupo int)

	set @strSQL='select idgrupo from ' + @idcomp + '.PH_GRUPOS where idgrupo IN (' + @IdsGrupos + ')'

	INSERT @tGrupos
	EXEC sp_executesql @strSQL

	declare @tEmpleadosMarcas table (idnumero varchar(20) not null, nombre varchar(70) not null,
	                                 fecha_entra datetime, hora_entra varchar(5), hora_sale varchar(5),
	                                 idturno int, cantidad decimal(18,2), columna_concepto  varchar(15),
									 tipo_extra varchar(10), estado char(1),manticipo varchar(5), mtardia varchar(5),
									 fecha_sale datetime,reg_sale bigint,idregistro bigint, fecha_ingreso datetime, 
									 iddepartamento varchar(15))

	insert into @tEmpleadosMarcas

	SELECT mp.idnumero,
	    e.nombre,
		mp.fecha_entra,
		mp.hora_entra,
		mp.hora_sale,
		mp.idturno,
		sum(isnull(dbo.hrs_min(mp.ORDC),0)/60) as cantidad,
		'Ordinario' as columna_concepto,
		null as tipo_extra,
		mp.estado as estado,
		mp.manticipo,
		mp.mtardia,
		mp.fecha_sale,
		mp.reg_sale,
		mp.idregistro,
	    e.fecha_ingreso as fecha_ingreso,
		e.iddepartamento as iddepartamento
		
	FROM marcas_proceso mp
	inner join empleados e on e.idnumero=mp.idnumero 
		and e.idnumero=(case when @idnumero='-1' then e.idnumero else @idnumero end) 
		and e.idgrupo in (select idgrupo FROM @tGrupos)
		and e.estado='T'
		and e.idplanilla = @IdPlanilla
	WHERE mp.fecha_entra between @FechaInicio and @FechaFin	
	group by mp.idnumero,e.nombre,mp.fecha_entra,mp.hora_entra,mp.hora_sale, mp.idturno,mp.estado,
	         mp.manticipo,mp.mtardia,mp.fecha_sale,mp.reg_sale,mp.idregistro, e.fecha_ingreso, e.iddepartamento


	insert into @tEmpleadosMarcas
	SELECT mp.idnumero,
	    e.nombre,
		mp.fecha_entra,
		mp.hora_entra,
		mp.hora_sale,
		mp.idturno,
		sum(isnull(dbo.hrs_min(mp.EXTC),0)/60) as cantidad,
		'HorasExtras' as columna_concepto,
		null as tipo_extra,
		mp.estado as estado,
		mp.manticipo,
		mp.mtardia,
		mp.fecha_sale,
		mp.reg_sale,
		mp.idregistro,
	    e.fecha_ingreso as fecha_ingreso,
		e.iddepartamento as iddepartamento
		
	FROM marcas_proceso mp
	inner join empleados e on e.idnumero=mp.idnumero 
		and e.idnumero=(case when @idnumero='-1' then e.idnumero else @idnumero end) 
		and e.idgrupo in (select idgrupo FROM @tGrupos)
		and e.estado='T'
		and e.idplanilla = @IdPlanilla
	WHERE mp.fecha_entra between @FechaInicio and @FechaFin	
	and mp.EXTC!='00:00'
	group by mp.idnumero,e.nombre,mp.fecha_entra,mp.hora_entra,mp.hora_sale, mp.idturno,mp.estado,
	         mp.manticipo,mp.mtardia,mp.fecha_sale,mp.reg_sale,mp.idregistro, e.fecha_ingreso, e.iddepartamento
	

	insert into @tEmpleadosMarcas

	SELECT mp.idnumero,
	    e.nombre,
		mp.fecha_entra,
		mp.hora_entra,
		mp.hora_sale,
		mp.idturno,
		sum(isnull(m.cantidad,0)) as cantidad,
		case when c.tipo_h=2 then case when c.ordinario='T' and c.autorizado='T' then 'TOrdinario' else 'Extras' end 
			 when c.tipo_h=3 then case when c.ordinario='T' and c.autorizado='T' then 'TOrdinario' else 'Extras' end  
			 when c.tipo_h=4 then case when c.ordinario='T' and c.autorizado='T' then 'TOrdinario' else 'Extras' end
	    end as columna_concepto,
		case when c.tipo_h=2 then 'Extras' 
			 when c.tipo_h=3 then 'Dobles'  
			 when c.tipo_h=4 then 'Otros' 
	    end as tipo_extra,
		case when m.estado is not null then m.estado else mp.estado end as estado,
		mp.manticipo,
		mp.mtardia,
		mp.fecha_sale,
		mp.reg_sale,
		mp.idregistro,
	    e.fecha_ingreso as fecha_ingreso,
		e.iddepartamento as iddepartamento
		
	FROM marcas_proceso mp
	inner join marcas_distribuciones m 
		on m.fecha=mp.fecha_entra and m.idplanilla=mp.idplanilla and m.idnumero=mp.idnumero and m.entrada=mp.hora_entra
	inner join ph_conceptos c on c.id=m.idconcepto and c.tipo_h in (1,2,3,4)	
	inner join empleados e on e.idnumero=mp.idnumero 
		and e.idnumero=(case when @idnumero='-1' then e.idnumero else @idnumero end) 
		and e.idgrupo in (select idgrupo FROM @tGrupos)
		and e.estado='T'
		and e.idplanilla = @IdPlanilla
	WHERE mp.fecha_entra between @FechaInicio and @FechaFin	
	and c.tipo_h in (2,3,4)
	group by mp.idnumero,e.nombre,mp.fecha_entra,mp.hora_entra,mp.hora_sale, mp.idturno,m.estado,
	         c.tipo_h,c.ordinario,c.autorizado,mp.manticipo,mp.mtardia,
			 case when m.estado is not null then m.estado else mp.estado end,
			 mp.fecha_sale,mp.reg_sale,mp.idregistro, e.fecha_ingreso, e.iddepartamento
	

	if (@idnumero!='-1')
	begin


		select idnumero, nombre, fecha_entra, hora_entra, hora_sale, x.idturno,t.descripcion as turno, 
			  sum(ordinario) as ordinario, 
			  sum(extras) as extras, sum(suma_extras) as suma_extras, 
			  sum(suma_dobles) as suma_dobles, sum(suma_otros) as suma_otros, estado, 
			  max(manticipo) as manticipo, min(mtardia) as mtardia,fecha_sale,isnull(reg_sale,0) as reg_sale,
			  idregistro, fecha_ingreso, iddepartamento

		from (
			
			select idnumero, nombre, fecha_entra, hora_entra, hora_sale, idturno,  
				   case columna_concepto when 'Ordinario' then cantidad else 0 end as ordinario,
				   case columna_concepto when 'HorasExtras' then cantidad else 0 end as extras,
				   case tipo_extra when 'Extras' then cantidad else 0 end as suma_extras,
				   case tipo_extra when 'Dobles' then cantidad else 0 end as suma_dobles,
				   case tipo_extra when 'Otros' then cantidad else 0 end as suma_otros,
				   estado, manticipo, mtardia, fecha_sale,reg_sale,idregistro, fecha_ingreso, iddepartamento		   
			from @tEmpleadosMarcas) x inner join ph_turnos t on t.idturno=x.idturno
		group by idnumero, nombre, fecha_entra, hora_entra, hora_sale, x.idturno,t.descripcion,estado,fecha_sale,isnull(reg_sale,0),idregistro, fecha_ingreso,iddepartamento  
		order by nombre,fecha_entra 
	end
	else
	begin

	

		select idnumero, nombre, fecha_entra, min(hora_entra) as hora_entra, max(hora_sale) as hora_sale ,min(x.idturno) as idturno ,min(t.descripcion) as turno, 
			  sum(ordinario) as ordinario, sum(extras) as extras, sum(suma_extras) as suma_extras, 
			  sum(suma_dobles) as suma_dobles, sum(suma_otros) as suma_otros, min(estado) as estado,
			  max(manticipo) as manticipo, min(mtardia) as mtardia,fecha_sale,isnull(reg_sale,0) as reg_sale,
			  idregistro, fecha_ingreso, iddepartamento
		from (
			
			select idnumero, nombre, fecha_entra, hora_entra, hora_sale, idturno,  
				   case columna_concepto when 'Ordinario' then cantidad else 0 end as ordinario,
				   case columna_concepto when 'HorasExtras' then cantidad else 0 end as extras,
				   case tipo_extra when 'Extras' then cantidad else 0 end as suma_extras,
				   case tipo_extra when 'Dobles' then cantidad else 0 end as suma_dobles,
				   case tipo_extra when 'Otros' then cantidad else 0 end as suma_otros,
				   estado,manticipo, mtardia, fecha_sale,reg_sale,idregistro, fecha_ingreso, iddepartamento
		   
			from @tEmpleadosMarcas) x inner join ph_turnos t on t.idturno=x.idturno
		group by idnumero, nombre,fecha_entra, fecha_sale,isnull(reg_sale,0),idregistro,fecha_ingreso, iddepartamento
		order by nombre,fecha_entra
	end
 
end
GO
ALTER AUTHORIZATION ON [dbo].[DM_CONSULTAR_MARCAS_PERIODO] TO  SCHEMA OWNER 
GO
CREATE   PROCEDURE [ctadmin].[VerificaCompaniaUsuarioWeb] 
@IdNumero varchar(20),	
@DataBase varchar(50)
AS
BEGIN	
	/*Verifica las companias para las que esta habilitado el usuario*/
	declare @tCompanias Table (idcomp varchar(10), compania varchar(60), nom_conector varchar(10), rowId int)
	declare @tCompaniasResp Table (idcomp varchar(10), compania varchar(60), nom_conector varchar(10),idnumero varchar(20))
	declare @cont int = 0, @maxCompanias int, @idcomp varchar(10),@compania varchar(60), @schemaDB varchar(10),@strSql nvarchar(250)

	insert @tCompanias(c.idcomp,c.compania,c.nom_conector,rowId)
	select c.idcomp,c.compania,c.nom_conector, 0 
	from CTADMIN.ph_companias c
	where ApiDataBase=@DataBase

	update @tCompanias
	set rowId = @cont,
	@cont = @cont+1

	select @maxCompanias=max(rowId) from @tCompanias
	select @cont=0

	while (@cont<=@maxCompanias)
	begin
		select @idcomp = c.idcomp,
			   @compania = c.compania,
		       @schemaDB = c.idcomp 		
		from @tCompanias c where rowId=@cont

		set @strSql = N'select ' + char(39) + @idcomp + char(39) + ',' + char(39) + @compania + char(39) +  ',' + char(39) + @schemaDB + char(39) + ', e.idnumero ' + 
		             'from ' + @schemaDB + '.empleados e where e.estado=''T'' and e.idnumero=' + char(39) + @IdNumero + char(39)


		insert @tCompaniasResp(idcomp,compania,nom_conector,idnumero)
		EXEC sp_executesql @strSql

		set @cont=@cont+1


	end

	SELECT idcomp,compania,nom_conector,idnumero 
	FROM @tCompaniasResp

END
go
ALTER AUTHORIZATION ON [ctadmin].[VerificaCompaniaUsuarioWeb]  TO  SCHEMA OWNER 
go 

create PROCEDURE [ctadmin].[VerificaDatosPMW] 
@Name varchar(50),
@Object varchar(50),
@field varchar(30),
@filter varchar(50)
AS
BEGIN	

	declare @tCompanias Table (idcomp varchar(10), rowId int)
	declare @tData Table (idcomp varchar(10), numval varchar(20))
	declare @cont int = 0, @maxCompanias int, @idcomp varchar(10),@strSql nvarchar(250)

	insert @tCompanias(c.idcomp,rowId)
	select c.idcomp, 0 
	from CTADMIN.ph_companias c
	where upper(ApiDataBase)=upper(@Name)

	update @tCompanias
	set rowId = @cont,
	@cont = @cont+1

	select @maxCompanias=max(rowId) from @tCompanias
	select @cont=0

	while (@cont<=@maxCompanias)
	begin
		select @idcomp = c.idcomp					   
		from @tCompanias c where rowId=@cont

		IF (EXISTS (SELECT * 
					FROM INFORMATION_SCHEMA.TABLES 
					WHERE upper(TABLE_SCHEMA) = upper(@idcomp)
					AND  upper(TABLE_NAME) = upper(@Object)))
		BEGIN
			set @strSql = N'select ' + char(39) + @idcomp + char(39) + ',' + @field +  
						 ' from ' + @idcomp + '.' + @Object + ' where ' + @filter 
			insert @tData(idcomp,numval)
			EXEC sp_executesql @strSql
		END
		set @cont=@cont+1
	end

	SELECT ENCRYPTBYPASSPHRASE(@Name,numval) as Valor 
	FROM @tData

END
go
ALTER AUTHORIZATION ON [ctadmin].[VerificaDatosPMW]  TO  SCHEMA OWNER 

GO

CREATE PROCEDURE [dbo].[in_marcas_web] @idnumero varchar(25)
	
AS
BEGIN
	
	
	declare @idplanilla varchar(8)
	declare @fecha datetime
	declare @hora varchar(5)
	declare @tipo int
	declare @terminal varchar(4)
	declare @registro bigint
	declare @idregistro bigint
	declare @id varchar(5)
	declare @fd varchar(5)
	declare @desc_pro int
	declare @fecha_reg datetime
    declare @long_reg varchar(max)
    declare @lat_reg varchar(max)
	declare @gps char(1)
	declare @fecha_hora datetime
	declare @dir_ip varchar(50)
	declare @host varchar(400)
	declare @tiempoEspera int = 5;

	
	update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.idnumero and idtarjeta = @idnumero
	--update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.tarjeta
	
	declare marca cursor for 
	 select idplanilla, idnumero,fecha,hora,tipo,idterminal,fecha_reg,long_reg,lat_reg,gps_cell,dir_ip,host 
		from marcas_in where idnumero = @idnumero order by idplanilla,idnumero,fecha,hora
	 
	 open marca
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@gps,@dir_ip,@host
	 while @@FETCH_STATUS = 0
	 begin
	 
	  set @registro = (select top 1 registro 
	                   from marcas 
	                   where idplanilla = @idplanilla 
	                   and idnumero = @idnumero 
					   and fecha = @fecha 
					   and dbo.hrs_min(hora) between dbo.hrs_min(@hora) - @tiempoEspera  and dbo.hrs_min(@hora) + @tiempoEspera)
	  if @registro is null
	   begin

	    set @fecha_hora = convert(datetime,convert(varchar(10),@fecha,120) + ' ' + @hora) 
	    insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal,fecha_reg,long_reg,lat_reg,gps_cell,fecha_hora,dir_ip,host)
	                 values (@idplanilla, @idnumero, @fecha, @hora, @tipo, @terminal,@fecha_reg,@long_reg,@lat_reg,@gps,@fecha_hora,@dir_ip,@host)

        update marcas set imagen_reg = x.imagen_reg from marcas_in x where marcas.idplanilla = x.idplanilla and marcas.idnumero = x.idnumero and marcas.fecha_reg = x.fecha_reg
	    
	   end	  
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@gps,@dir_ip,@host
	 end
	 
	 close marca
	 deallocate marca
	
	 delete from marcas_in where idnumero = @idnumero

	 declare marca_proc cursor for select registro, idplanilla, idnumero, fecha, hora, tipo from marcas where idnumero = @idnumero and estado = 'N' order by idplanilla, idnumero, fecha_hora
	 open marca_proc
	 
	 fetch next from marca_proc into @idregistro, @idplanilla, @idnumero, @fecha, @hora, @tipo
	 while @@FETCH_STATUS = 0
	 begin
	  --set @tipo = gsit.tipo(@idplanilla,@idnumero,@fecha,@hora)
	  --update marcas set tipo = @tipo where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and hora = @hora 
	  if @tipo = 1 --entrada
	   begin
	    set @registro = (select top 1 idregistro from marcas_proceso where  idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @fecha and hora_entra = @hora)
	    if @registro is null
	     begin
	       set @registro = (select top 1 idregistro from marcas_proceso where  idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @fecha and hora_entra = '00:00' and (hora_sale > @hora or hora_sale = '00:00'))
	       if @registro is null 
		    begin
			  insert into marcas_proceso (idplanilla,idnumero,fecha_entra,hora_entra,fecha_sale,hora_sale) values (@idplanilla,@idnumero,@fecha,@hora,@fecha,'00:00')
			  set @registro = (select @@IDENTITY)
            end
	       else update marcas_proceso set hora_entra = @hora where idregistro = @registro    
	     end
		 update marcas_incidencias set idregistro = @registro,hentra = @hora where idplanilla = @idplanilla and idnumero = @idnumero and (idregistro = 0 OR idregistro = @registro) and hentra = '00:00'
		 update marcas_mov_turnos set hora = @hora where  idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha
		 update marcas_extras_apb set hora = @hora where  idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha
	   end
	   
	    if @tipo = 2 --salida
	   begin
	   set @registro = (select top 1 idregistro from marcas_proceso where  idplanilla = @idplanilla and idnumero = @idnumero and reg_sale = @idregistro)
	   if @registro is null
	    begin
	      set @registro = (select top 1 idregistro from marcas_proceso where  idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = Dateadd(day,-1,@fecha) and hora_sale = '00:00' and (1440 - dbo.hrs_min(hora_entra) + dbo.hrs_min(@hora)) < 1080)  --nocturno 18 horas para par
		  if @registro is not null 
		    begin
			  update marcas_proceso set fecha_sale = @fecha, hora_sale = @hora,reg_sale = @idregistro where idregistro = @registro
			  --update marcas set estado = 'P' where registro = @idregistro
            end
		  else
		   begin
		    set @registro = (select top 1 idregistro from marcas_proceso where idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @fecha and hora_sale = '00:00' and hora_entra < @hora)
			if @registro is null 
			 begin 
			   insert into marcas_proceso (idplanilla,idnumero,fecha_entra,hora_sale,fecha_sale,hora_entra,reg_sale) values (@idplanilla,@idnumero,@fecha,@hora,@fecha,'00:00',@idregistro)
			   set @registro = (select @@IDENTITY)
			   --update marcas set estado = 'P' where registro = @idregistro
             end
	        else update marcas_proceso set hora_sale = @hora,reg_sale = @idregistro where idregistro = @registro
		   end
	    end 
		update marcas_incidencias set idregistro = @registro,hsale = @hora where  idplanilla = @idplanilla and idnumero = @idnumero and (idregistro = 0 OR idregistro = @registro) and hsale = '00:00'
	   end

     if @tipo = 3 --descanso
	 begin
	  set @desc_pro = (select isnull((select top 1 iddesc from marcas_descansos where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha order by iddesc desc),0))
	  set @desc_pro = @desc_pro + 1
	  if @desc_pro = 1 
	   begin
	    insert into marcas_descansos (iddesc,  idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (@desc_pro,@idplanilla,@idnumero,@fecha,@hora,'00:00')
       end
	  else 
	   begin
	    update marcas_descansos set fin_desc = @hora where  idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and inicio_desc <> '00:00' and fin_desc = '00:00'
		if @@ROWCOUNT = 0 insert into marcas_descansos (iddesc,  idplanilla, idnumero, fecha, inicio_desc, fin_desc) values (@desc_pro,@idplanilla,@idnumero,@fecha,@hora,'00:00')
	   end
	 end  
	 
	 if @tipo = 4 --marca comedor
	  begin
	   insert into marcas_comedor (idplanilla,idnumero,fecha,hora,idterminal) values (@idplanilla,@idnumero,@fecha,@hora,@terminal)
	  end
	 update marcas set estado = 'P' where registro = @idregistro
	 
	 fetch next from marca_proc into @idregistro, @idplanilla, @idnumero, @fecha, @hora, @tipo
	 end
	 
	 close marca_proc
	 deallocate marca_proc

END
go
ALTER AUTHORIZATION ON [dbo].[in_marcas_web]  TO  SCHEMA OWNER 