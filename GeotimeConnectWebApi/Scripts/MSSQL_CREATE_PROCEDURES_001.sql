/****** Object:  StoredProcedure [dbo].[add_accjust]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[roles] @idplanilla varchar(8), @idnumero varchar(16), @inicio datetime, @fin datetime
AS
BEGIN

   declare @tipoh char(1)
   declare @fechar datetime
   declare @idrol int
   declare @idturnos int
   declare @hentra varchar(5)

   select @tipoh = tipo_marca,@fechar = inicio_rol,@idrol = idhorario from empleados where idplanilla = @idplanilla and idnumero = @idnumero
   
   
   if @tipoh = 'R'
   begin
   while @fechar < @fin
    begin
	if @fechar <= @fin
	 begin
	    declare turnos cursor for 
	    select idturno from ph_roles_turnos where idrol = @idrol order by idregistro
		open turnos
		fetch next from turnos into @idturnos
		while @@FETCH_STATUS = 0
		begin
		set @hentra = isnull((select top 1 hora from marcas where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fechar and tipo = 1),'00:00')
		update marcas_mov_turnos set turno = @idturnos,hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fechar
		if @@ROWCOUNT = 0
		 begin
		  insert into marcas_mov_turnos (idplanilla,idnumero,turno,fecha,hora,estado) values (@idplanilla,@idnumero,@idturnos,@fechar,@hentra,'T')
		 end
		 set @fechar = DATEADD(day,1,@fechar)

		fetch next from turnos into @idturnos
		end
		close turnos
		deallocate turnos

		update empleados set inicio_rol = @fechar where idplanilla = @idplanilla and idnumero = @idnumero
	 end
	end
	 
   end

END
GO
ALTER AUTHORIZATION ON [dbo].[roles] TO  SCHEMA OWNER 
GO

create PROCEDURE [dbo].[ini_marcas_all] @IDPERIODO VARCHAR(8) 
AS 
BEGIN 
DECLARE @INICIO DATETIME 
DECLARE @FIN DATETIME 
DECLARE @IDNUMERO VARCHAR(20) 
DECLARE @IDREGISTRO BIGINT 
DECLARE @FFIN DATETIME 
DECLARE @IDGRUPO INT
DECLARE @tipo_planilla char(1)
DECLARE @planilla varchar(8)
Declare @idplanilla varchar(8)
 
   SELECT @INICIO = INICIO, @FIN = FIN,@tipo_planilla = tipo_planilla FROM PH_PERIODOS WHERE IDPERIODO = @IDPERIODO 


   DECLARE EMPLEADOS CURSOR FOR SELECT IDNUMERO,idplanilla FROM EMPLEADOS WHERE IDPLANILLA in (select idplanilla from ph_planilla where tipo_planilla = @tipo_planilla) AND ESTADO = 'T' 
    OPEN EMPLEADOS 
     FETCH NEXT FROM EMPLEADOS INTO @IDNUMERO,@planilla 
      WHILE @@FETCH_STATUS = 0 
      BEGIN 
       SET @FFIN = @INICIO 
       WHILE @FFIN < @FIN + 1 
        BEGIN SET @IDREGISTRO = (SELECT TOP 1 IDREGISTRO FROM MARCAS_PROCESO WHERE IDPLANILLA = @planilla AND IDNUMERO = @IDNUMERO AND FECHA_ENTRA = @FFIN) 
         IF @IDREGISTRO IS NULL INSERT INTO MARCAS_PROCESO (IDPLANILLA, IDNUMERO, FECHA_ENTRA, FECHA_SALE, HORA_ENTRA, HORA_SALE, IDTURNO) VALUES (@planilla,@IDNUMERO,@FFIN,@FFIN,'00:00','00:00',0) 
         SET @FFIN = @FFIN + 1 
         exec [dbo].[roles] @planilla,@idnumero,@INICIO,@FIN
    END 
    FETCH NEXT FROM EMPLEADOS INTO @IDNUMERO,@planilla
    END 
    CLOSE EMPLEADOS 
    DEALLOCATE EMPLEADOS 
    UPDATE PH_PERIODOS SET ESTADO = 'T' WHERE IDPERIODO = @IDPERIODO 

	Declare datos_p
	Cursor for select idplanilla from ph_planilla where tipo_planilla = @tipo_planilla
	open datos_p
	fetch next from datos_p into @idplanilla
	while @@FETCH_STATUS = 0
	begin

    DECLARE DATOSP 
	CURSOR FOR SELECT DISTINCT IDGRUPO FROM EMPLEADOS WHERE IDPLANILLA = @idplanilla
	OPEN DATOSP 
	FETCH NEXT FROM DATOSP INTO @IDGRUPO 
	WHILE @@FETCH_STATUS = 0 
	BEGIN 
	UPDATE PH_GRUPO_PERIODO SET ESTADO = 'A' WHERE IDGRUPO = @IDGRUPO AND IDPERIODO = @IDPERIODO AND IDPLANILLA = @IDPLANILLA 
	--IF @@ROWCOUNT = 0 UPDATE PH_GRUPO_PERIODO SET ESTADO = 'A', IDPLANILLA = @IDPLANILLA WHERE IDGRUPO = @IDGRUPO AND IDPERIODO = @IDPERIODO AND IDPLANILLA IS NULL 
	IF @@ROWCOUNT = 0 INSERT INTO PH_GRUPO_PERIODO (IDGRUPO,IDPERIODO,ESTADO,IDPLANILLA) VALUES (@IDGRUPO,@IDPERIODO,'A',@IDPLANILLA) 
	FETCH NEXT FROM DATOSP INTO @IDGRUPO 
	END  
	CLOSE DATOSP  
	DEALLOCATE DATOSP 

	fetch next from datos_p into @idplanilla
	end
	close datos_p
	deallocate datos_p

	END
GO
ALTER AUTHORIZATION ON [dbo].[ini_marcas_all] TO  SCHEMA OWNER 
GO

CREATE PROCEDURE [dbo].[add_accjust] @idregistro bigint, @idjust int, @usuario varchar(15), @comentario varchar(2048),@horas varchar(5)
	
AS
BEGIN
	
	declare @idplanilla varchar(8)
	declare @idnumero varchar(20)
	declare @fecha datetime
	declare @hora varchar(5)
	declare @indice bigint
	declare @idj int

	select @idplanilla = idplanilla,@idnumero = idnumero,@fecha = fecha_entra,@hora = hora_entra from marcas_proceso where idregistro = @idregistro
	set @indice = 0

	
	  select top 1 @indice = indice,@idj = idincidencia from marcas_incidencias where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and hentra = @hora
	  if @indice > 0
	    begin
		 update marcas_incidencias set incidencia_just = @idj,idincidencia = @idjust, comentario = @comentario,c_tiempo = @horas where indice = @indice
	    end
      else
	   begin
	     insert into marcas_incidencias (idplanilla,idnumero,fecha,hentra,idincidencia,idregistro,hsale,est_p,estado,usuario,fecha_just,comentario,c_tiempo) 
		                         values (@idplanilla,@idnumero,@fecha,@hora,@idjust,@idregistro,'00:00','N','A',@usuario,getdate(),@comentario,@horas)
	   end 


END
GO
ALTER AUTHORIZATION ON [dbo].[add_accjust] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[add_accpersonal]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[add_accpersonal]
	@idplanilla varchar(8),@idnumero varchar(20), @inicio datetime, @fin datetime, @idincidencia int, @comentario text, @dias int, @usuario varchar(15), @dias_apl varchar(15)
AS
BEGIN
	
	insert into acciones_personal (idplanilla, idnumero, inicio, fin, idincidencia, estado, comentario, dias, usuario, fecha_just,dias_apl) values (@idplanilla,@idnumero,@inicio,@fin,@idincidencia,'N',@comentario, @dias, @usuario, getdate(),@dias_apl)

END
GO
ALTER AUTHORIZATION ON [dbo].[add_accpersonal] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[add_dist_cc]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[add_dist_cc]
	@planilla varchar(8),@idnumero varchar(20),@fecha datetime, @inicio varchar(5), @fin varchar(5), @iddist int, @idreg int, @comentario text
AS
BEGIN
	
	declare @ccosto varchar(25)

	set @ccosto = (select idccosto from ph_distribuciones_ccosto where idregistro = @iddist)

	if @idreg = 0
	 begin
	  insert into marcas_distribuciones_conceptos (idplanilla,idnumero,fecha,inicio,fin,idccosto,iddist,comentario) 
	                                       values (@planilla,@idnumero,@fecha,@inicio,@fin,@ccosto,@iddist,@comentario)
	 end
    else
	 begin
	   update marcas_distribuciones_conceptos set fecha = @fecha,inicio = @inicio,fin = @fin,idccosto = @ccosto,iddist = @iddist,comentario = @comentario where idregistro = @idreg
	 end
END
GO
ALTER AUTHORIZATION ON [dbo].[add_dist_cc] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[add_registro_tiempo]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[add_registro_tiempo] @planilla varchar(8), @idnumero varchar(20),@fentra datetime, @fsale datetime, @hentra varchar(5), @hsale varchar(5), @turno int, @usuario varchar(15), @comentario varchar(2048)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (@planilla,@idnumero,@fentra,@hentra,1,'RM','P')
	insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@planilla,@idnumero,getdate(),@usuario,null,@fentra,'00:00',@hentra,@comentario)
	insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (@planilla,@idnumero,@fsale,@hsale,2,'RM','P')
	insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@planilla,@idnumero,getdate(),@usuario,null,@fsale,'00:00',@hsale,@comentario)
	insert into marcas_proceso(idplanilla, idnumero,fecha_entra,fecha_sale,hora_entra,hora_sale,idturno) values (@planilla,@idnumero,@fentra,@fsale,@hentra,@hsale,@turno)
   
END
GO
ALTER AUTHORIZATION ON [dbo].[add_registro_tiempo] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[anulo_accpersonal]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[anulo_accpersonal] @idregistro bigint
AS
BEGIN

 update acciones_personal set estado = 'D' where idregistro = @idregistro and estado <> 'S'
 if @@ROWCOUNT = 1
 begin
  delete from marcas_incidencias where idacc = @idregistro
 end

END
GO
ALTER AUTHORIZATION ON [dbo].[anulo_accpersonal] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[apertura_periodo]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[apertura_periodo] @idplanilla varchar(8), @grupo int,@periodo varchar(8), @inicio datetime, @fin datetime, @usuario int
	
AS
BEGIN
	
	SET NOCOUNT ON;

update marcas_distribuciones_conceptos set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin
	
update marcas_distribuciones set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin

update marcas_extras_apb set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin
	
update marcas_incidencias set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin
	
update marcas_mov_turnos set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin
	
update marcas_proceso set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha_entra between @inicio and @fin
	
update marcas_tiempo_adicional set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo = @grupo) and periodo = @periodo

update marcas_reportes set estado = 'A' where  idplanilla = @idplanilla and   idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin

update marcas_proyecciones set estado = 'A' where  idplanilla = @idplanilla and   idnumero in  (select idnumero from empleados where idgrupo = @grupo) and fecha between @inicio and @fin

update ph_grupo_periodo set estado = 'A' where idplanilla = @idplanilla and idgrupo = @grupo and idperiodo = @periodo
END
GO
ALTER AUTHORIZATION ON [dbo].[apertura_periodo] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[aplico_accpersonal]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[aplico_accpersonal] @idregistro bigint
AS
BEGIN
	
	declare @inicio datetime
	declare @fin datetime
    declare @idplanilla varchar(8)
	declare @idnumero varchar(20)
	declare @idincidencia int
	declare @fecha datetime
	declare @entra varchar(5)
	declare @sale varchar(5)
	declare @comentario varchar(2048)
	declare @usuario varchar(15)
	declare @dl varchar(15)
	declare @da int
	declare @cdias int
	declare @idjust int


	set @cdias = 0
	declare datos cursor for
	  select idplanilla,idnumero,inicio,fin,idincidencia,comentario,usuario,dias_apl from acciones_personal where idregistro = @idregistro and estado = 'N'

    open datos
	Fetch Next from datos into @idplanilla,@idnumero,@inicio,@fin,@idincidencia,@comentario,@usuario,@dl
	while @@FETCH_STATUS = 0
	begin
	set @fecha = @inicio
	while @fecha <= @fin
	  begin
	  set @da = (DATEPART(DW,@fecha))
	  if @da in (select * from dbo.splitstring(@dl))
	   begin
	   	 set @idjust = ISNULL((select top 1 idincidencia from marcas_incidencias where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and idincidencia < 7),0)     
	     update marcas_incidencias set idincidencia = @idincidencia,comentario = @comentario,usuario = @usuario,idacc = @idregistro,incidencia_just = @idincidencia where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and idincidencia < 6	and idregistro in (select top 1 idregistro from marcas_incidencias where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and idincidencia < 6 order by hentra asc) 
	     if @@ROWCOUNT = 0 
	    begin
		 
       begin try  
		 set @entra = isnull((select hora from marcas where  idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and tipo = 1),'00:00')
		 set @sale = isnull((select hora from marcas where  idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and tipo = 2),'00:00')
		 insert into marcas_incidencias ( idplanilla, idnumero, fecha, idincidencia, idregistro, hentra, hsale, est_p, comentario, usuario, idacc, incidencia_just) 
	                                            values (@idplanilla,@idnumero,@fecha,@idincidencia,0,@entra,@sale,'N',@comentario,@usuario, @idregistro, @idjust)  
       end try
	   begin catch
	   end catch												   

        end
		set @cdias = @cdias + 1
       end
       set @fecha = DATEADD(Day,1,@fecha)
	  end
     update acciones_personal set estado = 'A',dias = @cdias where idregistro = @idregistro
	Fetch Next from datos into @idplanilla,@idnumero,@inicio,@fin,@idincidencia,@comentario,@usuario,@dl
	end

	close datos
	deallocate datos
	
END
GO
ALTER AUTHORIZATION ON [dbo].[aplico_accpersonal] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[apruebo_extra]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[apruebo_extra] @cantidad varchar(5), @comentario varchar(1024), @usuario varchar(100), @idregistro bigint,@IDCCOSTO VARCHAR(25)
 AS 
 BEGIN 
 DECLARE @IDPLANILLA VARCHAR(8) 
 DECLARE @IDNUMERO VARCHAR(20) 
 DECLARE @FECHA DATETIME 
 DECLARE @HORA VARCHAR(5) 
 DECLARE @EXTC VARCHAR(5) 
 SELECT @IDPLANILLA = IDPLANILLA, @IDNUMERO = IDNUMERO, @FECHA = FECHA_ENTRA, @HORA = HORA_ENTRA,@EXTC = EXTC FROM MARCAS_PROCESO WHERE IDREGISTRO = @IDREGISTRO 
 BEGIN TRY 
 IF DBO.HRS_MIN(@EXTC) < DBO.HRS_MIN(@CANTIDAD) SET @CANTIDAD = @EXTC 
 IF @IDCCOSTO = 'NAP_' 
 BEGIN 
 UPDATE MARCAS_EXTRAS_APB SET CANTIDAD = @CANTIDAD, COMENTARIO = @COMENTARIO, USUARIO = @USUARIO,CCOSTO = NULL WHERE IDPLANILLA = @IDPLANILLA AND IDNUMERO = @IDNUMERO AND FECHA = @FECHA AND HORA = @HORA 
 IF @@ROWCOUNT = 0 INSERT INTO MARCAS_EXTRAS_APB (IDPLANILLA,IDNUMERO,FECHA,HORA,CANTIDAD,COMENTARIO, USUARIO) VALUES (@IDPLANILLA,@IDNUMERO,@FECHA,@HORA,@CANTIDAD,@COMENTARIO,@USUARIO) 
 END 
 ELSE 
 BEGIN 
 UPDATE MARCAS_EXTRAS_APB SET CANTIDAD = @CANTIDAD, COMENTARIO = @COMENTARIO, USUARIO = @USUARIO,CCOSTO = @IDCCOSTO WHERE IDPLANILLA = @IDPLANILLA AND IDNUMERO = @IDNUMERO AND FECHA = @FECHA AND HORA = @HORA 
 IF @@ROWCOUNT = 0 INSERT INTO MARCAS_EXTRAS_APB (IDPLANILLA,IDNUMERO,FECHA,HORA,CANTIDAD,COMENTARIO, USUARIO,CCOSTO) VALUES (@IDPLANILLA,@IDNUMERO,@FECHA,@HORA,@CANTIDAD,@COMENTARIO,@USUARIO,@IDCCOSTO) 
 END 
 UPDATE MARCAS_PROCESO SET EXTT = @CANTIDAD WHERE IDREGISTRO = @IDREGISTRO 
 END TRY 
 BEGIN CATCH END CATCH 
 END
GO
ALTER AUTHORIZATION ON [dbo].[apruebo_extra] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[apruebo_extra_masiva]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[apruebo_extra_masiva] @registro bigint, @cantidad varchar(5),@comentario varchar(1024) ,@usuario varchar(15)
AS
BEGIN
	
	declare @planilla varchar(8)
	declare @idnumero varchar(20)
	declare @fecha datetime
	declare @entra varchar(5)


	if @comentario is null or @comentario = '' set @comentario = 'Aprobada mediante proceso masivo de Extras por ' + @usuario

	select @planilla = idplanilla, @idnumero = idnumero, @fecha = fecha_entra, @entra = hora_entra from marcas_proceso where idregistro = @registro

	if @cantidad = '00:00'
	 begin
	  delete from marcas_extras_apb where idplanilla = @planilla and idnumero = @idnumero and fecha = @fecha and hora = @entra
	  update marcas_proceso set EXTT = @cantidad where idregistro = @registro
	 end
    else
	 begin
	  update marcas_extras_apb set cantidad = @cantidad,usuario = @usuario,comentario = @comentario where idplanilla = @planilla and idnumero = @idnumero and fecha = @fecha and hora = @entra
	  if @@ROWCOUNT = 0 
	   begin
	    insert into marcas_extras_apb (idplanilla, idnumero, fecha, hora, cantidad, comentario, usuario) values (@planilla,@idnumero,@fecha,@entra,@cantidad,@comentario,@usuario)
       end
      update marcas_proceso set EXTT = @cantidad where idregistro = @registro
	 end


END
GO
ALTER AUTHORIZATION ON [dbo].[apruebo_extra_masiva] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[apruebo_preextra_masiva]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[apruebo_preextra_masiva] @planilla varchar(8),@idnumero varchar(16),@fecha datetime,  @cantidad varchar(5),@comentario varchar(1024) ,@usuario varchar(15)
AS
BEGIN
	
	
	declare @entra varchar(5)


	if @comentario is null or @comentario = '' set @comentario = 'Aprobada mediante proceso masivo de Extras por ' + @usuario

	set @entra = isnull((select top 1 hora from marcas where idplanilla = @planilla and idnumero = @idnumero and fecha = @fecha and tipo = 1),'00:00')
	
	update marcas_extras_apb set cantidad = @cantidad, comentario = @comentario,usuario=@usuario where idplanilla = @planilla and idnumero = @idnumero and fecha = @fecha and hora = @entra
	if @@ROWCOUNT = 0 insert into marcas_extras_apb (idplanilla,idnumero,fecha,hora,cantidad,comentario,usuario) values (@planilla,@idnumero,@fecha,@entra,@cantidad,@comentario,@usuario)

END
GO
ALTER AUTHORIZATION ON [dbo].[apruebo_preextra_masiva] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[apruebo_extra_periodo]    Script Date: 28/02/2019 08:23:49 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[apruebo_extra_periodo] @planilla varchar(8),@idnumero varchar(16), @comentario varchar(1024), @inicio datetime, @fin datetime, @usuario varchar(100)
AS
BEGIN
	
	declare @fecha datetime
	declare @horas varchar(5)
	declare @hora varchar(5)

	declare datos cursor for
	 select fecha_entra,hora_entra,extc from marcas_proceso where idplanilla = @planilla and idnumero = @idnumero and fecha_entra between @inicio and @fin and extc <> '00:00'
	 open datos
	 fetch next from datos into @fecha,@hora,@horas
	 while @@fetch_status = 0
	 begin

	     update marcas_extras_apb set cantidad = @horas,comentario = @comentario,usuario = @usuario where idplanilla = @planilla and idnumero = @idnumero and fecha = @fecha and hora = @hora
		 if @@rowcount = 0 begin insert into marcas_extras_apb (idplanilla,idnumero,fecha,hora,cantidad,comentario,usuario) values (@planilla,@idnumero,@fecha,@hora,@horas,@comentario,@usuario) end
		 

	 fetch next from datos into @fecha,@hora,@horas
	 end
	 close datos
	 deallocate datos

	 update marcas_proceso set EXTT = EXTC where idplanilla = @planilla and idnumero = @idnumero and fecha_entra between @inicio and @fin and extc <> '00:00'

END
GO
/****** Object:  StoredProcedure [dbo].[borro_accpersonal]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[borro_accpersonal] @idregistro bigint
AS
BEGIN
	
	delete from acciones_personal where idregistro = @idregistro and estado = 'N'

END


GO
ALTER AUTHORIZATION ON [dbo].[borro_accpersonal] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[borro_registro]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[borro_registro] @idregistro int, @usuario varchar(15)
	
AS
BEGIN
	
	SET NOCOUNT ON;

    declare @fentra datetime
	declare @fsale datetime
	declare @hentra varchar(5)
	declare @hsale varchar(5)
	declare @planilla varchar(8)
	declare @idnumero varchar(20)

	select @planilla = idplanilla,@idnumero = idnumero, @fentra = fecha_entra, @fsale = fecha_sale, @hentra = hora_entra, @hsale = hora_sale from marcas_proceso where idregistro = @idregistro

        delete from marcas where idplanilla = @planilla and idnumero = @idnumero and fecha = @fentra and hora = @hentra
	delete from marcas where idplanilla = @planilla and idnumero = @idnumero and fecha = @fsale and hora = @hsale
	delete from marcas_proceso where idregistro = @idregistro
	delete from marcas_incidencias where idplanilla = @planilla and idnumero = @idnumero and fecha = @fentra and hentra = @hentra and idincidencia <= 7

	insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@planilla,@idnumero,getdate(),@usuario,@fentra,null,@hentra,null,'Registro Eliminado')
	insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@planilla,@idnumero,getdate(),@usuario,@fsale,null,@hsale,null,'Registro Eliminado')
END
GO
ALTER AUTHORIZATION ON [dbo].[borro_registro] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[cierro_periodo]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cierro_periodo] @idplanilla varchar(8), @grupo varchar(max),@periodo varchar(8), @inicio datetime, @fin datetime, @usuario int, @est_m char(1) 
  AS BEGIN 	
  SET NOCOUNT ON; 
  declare @marcas_abiertas int 
  declare @estp char(1) 
  if @est_m = 'T' set @est_m = 'C' 
  if @est_m = 'F' set @est_m = 'T' 
  update marcas_distribuciones_conceptos set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @inicio and @fin 
  update marcas_distribuciones set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @inicio and @fin 
  update marcas_extras_apb set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @inicio and @fin 
  update marcas_incidencias set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @inicio and @fin 
  update marcas_mov_turnos set estado = @est_m where idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @inicio and @fin 
  update marcas_proceso set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha_entra between @inicio and @fin 
  update marcas_tiempo_adicional set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and periodo = @periodo
  update marcas_reportes set estado = @est_m where  idplanilla = @idplanilla and   idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @inicio and @fin set @marcas_abiertas = 1 set @marcas_abiertas = (select count(*) from marcas_proceso where fecha_entra between @inicio and @fin and estado = 'A' and idplanilla = @idplanilla) if @marcas_abiertas = 0 
  update ph_periodos set estado = 'C' where idperiodo = @periodo 
  update ph_grupo_periodo set estado = @est_m where idgrupo in (select * from dbo.splitstring(@grupo)) and idperiodo = @periodo set @estp = (select top 1 proyecta from ph_planilla where tipo_planilla = (select top 1 tipo_planilla from ph_periodos where idperiodo = @periodo)) 
  update ph_grupo_periodo set usuario_cierre = @usuario, fecha_cierre = GETDATE() where usuario_cierre is null
  if @estp = 'T'  
  begin   
   declare @iniciop datetime   
   declare @finp datetime   
   declare @iniciopant datetime   
   declare @finpant datetime   
   declare @perant varchar(8)   
   select @iniciop = inicio_proy,@finp = fin_proy,@perant = periodo_proy from ph_periodos where idperiodo = @periodo
   
  update marcas_proyecciones set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp   
  update marcas_distribuciones_conceptos set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp   
  update marcas_distribuciones set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp  
  update marcas_extras_apb set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp  
  update marcas_incidencias set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp   
  update marcas_mov_turnos set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp   
  update marcas_proceso set estado = 'A' where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha_entra between @iniciop and @finp   
  update marcas_reportes set estado = 'A' where  idplanilla = @idplanilla and   idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciop and @finp   

  if @perant is not null    
    begin     
	 select @iniciopant = inicio_proy,@finpant = fin_proy from ph_periodos where idperiodo = @perant 
	 update marcas_proyecciones set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant	  
	 update marcas_distribuciones_conceptos set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant  
	 update marcas_distribuciones set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant  
	 update marcas_extras_apb set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant  
	 update marcas_incidencias set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant  
	 update marcas_mov_turnos set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant  
	 update marcas_proceso set estado = @est_m where  idplanilla = @idplanilla and idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha_entra between @iniciopant and @finpant  
	 update marcas_reportes set estado = @est_m where  idplanilla = @idplanilla and   idnumero in  (select idnumero from empleados where idgrupo in (select * from dbo.splitstring(@grupo))) and fecha between @iniciopant and @finpant    
   end  
  end 
 END
GO
/****ALTER AUTHORIZATION ON [dbo].[dep_add] TO  SCHEMA OWNER ****/
/***GO***/
/****** Object:  StoredProcedure [dbo].[DM_POST_CALCULO_EMPLEADO]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DM_POST_CALCULO_EMPLEADO]  @idplanilla varchar(8),@idnumero varchar(20) ,@periodo varchar(8), @inicio datetime, @fin datetime
AS
BEGIN
	
	--delete from marcas_resumen where idnumero = @idnumero

	return

END
GO
ALTER AUTHORIZATION ON [dbo].[DM_POST_CALCULO_EMPLEADO] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[DM_POST_SINCRONIZA]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DM_POST_SINCRONIZA] @planilla varchar(8)	
AS
BEGIN
	
	return
	--update empleados set idgrupo = ubicacion where ubicacion in (select idgrupo from ph_grupos) and idcompania = @compania and idplanilla = @planilla

END
GO
ALTER AUTHORIZATION ON [dbo].[DM_POST_SINCRONIZA] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[in_marcas]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[in_marcas]
	
AS
BEGIN
	
	declare @idnumero varchar(20)
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
    declare @long_reg VARCHAR(max)
    declare @lat_reg VARCHAR(max)
	declare @est_gps char(1)
	
	update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.idnumero
	update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.tarjeta
	
	declare marca cursor for 
	 select idplanilla, idnumero,fecha,hora,tipo,idterminal,fecha_reg,long_reg,lat_reg,gps_cell from marcas_in where idnumero is not null order by idplanilla,idnumero,fecha,hora
	 
	 open marca
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@est_gps
	 while @@FETCH_STATUS = 0
	 begin
	 
	  set @registro = (select top 1 registro from marcas where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and dbo.hrs_min(hora) between dbo.hrs_min(@hora) - 5  and dbo.hrs_min(@hora) + 5)
	  if @registro is null
	   begin
	   
	    insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal,fecha_reg,long_reg,lat_reg,gps_cell)
	                 values (@idplanilla, @idnumero, @fecha, @hora, @tipo, @terminal,@fecha_reg,@long_reg,@lat_reg,@est_gps)
	    
	   end	  
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@est_gps
	 end
	 
	 close marca
	 deallocate marca
	
	 delete from marcas_in where idnumero is not null

	 declare marca_proc cursor for select registro, idplanilla, idnumero, fecha, hora, tipo from marcas where estado = 'N' order by idplanilla, idnumero, fecha, hora
	 open marca_proc
	 
	 fetch next from marca_proc into @idregistro, @idplanilla, @idnumero, @fecha, @hora, @tipo
	 while @@FETCH_STATUS = 0
	 begin
	  --set @tipo = dbo.tipo(@idcomp,@idplanilla,@idnumero,@fecha,@hora)
	  --update marcas set tipo = @tipo where idcomp = @idcomp and idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and hora = @hora 
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
GO
ALTER AUTHORIZATION ON [dbo].[in_marcas] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[ini_marcas]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ini_marcas] @idplanilla varchar(8), @idperiodo varchar(8)

as
begin
  --Proceso Creacion tabla de marcas de edicion de tiempos
 declare @inicio datetime
  declare @fin datetime
  declare @idnumero varchar(20)
  declare @idregistro bigint
  declare @ffin datetime
  declare @idgrupo int

  select @inicio = inicio, @fin = fin from ph_periodos where idperiodo = @idperiodo
  
  declare empleados cursor for 
    select idnumero from empleados where idplanilla = @idplanilla and estado = 'T'
    open empleados
    Fetch next from empleados into @idnumero
    while @@FETCH_STATUS = 0
    begin
    
      set @ffin = @inicio
        
      while @ffin < @fin + 1
      begin
      
       set @idregistro = (select top 1 idregistro from marcas_proceso where idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @ffin)
       if @idregistro is null insert into marcas_proceso (idplanilla, idnumero, fecha_entra, fecha_sale, hora_entra, hora_sale, idturno) values (@idplanilla,@idnumero,@ffin,@ffin,'00:00','00:00',0)
       set @ffin = @ffin + 1
       
      end 
    exec [dbo].roles @idplanilla,@idnumero,@inicio,@fin
    Fetch next from empleados into @idnumero 
    end 
    
    close empleados
    deallocate empleados

update ph_periodos set estado = 'T' where idperiodo = @idperiodo

declare datosp cursor for
 select distinct idgrupo from empleados where idplanilla = @idplanilla
 open datosp
 fetch next from datosp into @idgrupo
 while @@FETCH_STATUS = 0
 begin

   update ph_grupo_periodo set estado = 'A' where idgrupo = @idgrupo and idperiodo = @idperiodo and (idplanilla = @idplanilla or idplanilla is null)
   if @@ROWCOUNT = 0 insert into ph_grupo_periodo (idgrupo,idperiodo,estado,idplanilla) values (@idgrupo,@idperiodo,'A',@idplanilla)

 fetch next from datosp into @idgrupo
 end
 close datosp
 deallocate datosp
end
GO
ALTER AUTHORIZATION ON [dbo].[ini_marcas] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[marca_web]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[marca_web] @idnumero varchar(50), @tipo int
AS
BEGIN
	
	SET NOCOUNT ON;

	insert into marcas_in (idtarjeta,fecha,hora,tipo,idterminal) values (@idnumero,convert(varchar(10),dateadd(HH,-6,GETUTCDATE()),120),convert(varchar(5),dateadd(HH,-6,GETUTCDATE()),114),@tipo,'99')

END
GO
ALTER AUTHORIZATION ON [dbo].[marca_web] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[plan_add]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[plan_add]
	@idplanilla varchar(8), @planilla varchar(60), @nom_conector varchar(10), @ed char(1), @oldid varchar(8), @ids bigint, @extc char(1), @inic_c char(1), @adic char(1), @mdesc char(1), @tnom char(1), @proyecta char(1),@diasem int
AS
BEGIN
	declare @ex int
	declare @plans varchar(max)

	set @ex = (select idusuario from CTADMIN.ph_login where idsesion = @ids)

	if @ex is not null and @ex > 0
	begin
	
	  if @ed = 'F'
	   begin
	     insert into ph_planilla (idplanilla, planilla, nom_conector,c_ext,c_inci,c_adic,m_desc,tipo_planilla,proyecta,dia_inicio) values (@idplanilla, @planilla, @nom_conector,@extc,@inic_c,@adic,@mdesc,@tnom,@proyecta,@diasem)
		 set @plans = (select planillas from CTADMIN.ph_login where idsesion = @ids)
		 if @plans is null or @plans = ''
		  begin
		   update CTADMIN.ph_login set planillas = '°' + @idplanilla + '°' where idsesion = @ids
		  end
         else
		  begin
		   update CTADMIN.ph_login set planillas = planillas + ',°' + @idplanilla + '°' where idsesion = @ids
		  end 
	   end
      else
	  begin
	   if @ed = 'T'
	    begin
	     update ph_planilla set idplanilla = @idplanilla, planilla = @planilla, nom_conector = @nom_conector,c_ext = @extc,c_inci = @inic_c,c_adic = @adic, m_desc = @mdesc,tipo_planilla=@tnom, proyecta = @proyecta,dia_inicio = @diasem where idplanilla = @oldid 
	    end
	  end
    end
END

GO
ALTER AUTHORIZATION ON [dbo].[plan_add] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[post_sincronizo_erp]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[post_sincronizo_erp] @planilla varchar(8)	
AS
BEGIN
	
	--update empleados set idgrupo = ubicacion where ubicacion in (select idgrupo from ph_grupos) and idcompania = @idcomp and idplanilla = @planilla
	return
END
GO
ALTER AUTHORIZATION ON [dbo].[post_sincronizo_erp] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[PRE_PROCESO_COMP]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[PRE_PROCESO_COMP] @idplanilla varchar(8),@periodo varchar(8)
AS
BEGIN  
	return
END

GO
ALTER AUTHORIZATION ON [dbo].[PRE_PROCESO_COMP] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[proceso_emp]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[proceso_emp]  @idplanilla varchar(8),@idnumero varchar(20) ,@periodo varchar(8), @inicio datetime, @fin datetime
AS
BEGIN
	
	update marcas_incidencias set hentra = x.hora_entra from marcas_proceso x where marcas_incidencias.idplanilla = x.idplanilla and marcas_incidencias.idnumero = x.idnumero and marcas_incidencias.fecha = x.fecha_entra and marcas_incidencias.idplanilla = @idplanilla and marcas_incidencias.idnumero = @idnumero and marcas_incidencias.fecha between @inicio and @fin

   declare @indice int
   declare @diasp int
   declare @fecha_ultpago datetime
   declare @dias_continua int
   declare @dias_pagados int
   declare @fecha_incidencia datetime

   set @indice = 12
   set @diasp = 3

   update marcas_incidencias set est_p = 'F' where idplanilla = @idplanilla and idnumero = @idnumero and idincidencia = @indice and fecha between @inicio and @fin
   --fecha ultimo pago incapacidad
   set @fecha_ultpago = (select top 1 fecha from marcas_incidencias where idplanilla = @idplanilla and idnumero = @idnumero and idincidencia = @indice and est_p = 'P' order by fecha desc)

   --cantidad incapacidades
   --set @dias_continua = (select count(*) from marcas_incidencias where idcompania = @idcomp and idplanilla = @idplanilla and idnumero = @idnumero and idincidencia = @indice and fecha between @inicio - 30 and @inicio - 1)


   --ultimo pago incidencia fue mas 30 dias
   if @fecha_ultpago < @inicio - 30
    begin
	 set @diasp = 3
    end
   else
    begin
	  set @dias_pagados = (select count(*) from marcas_incidencias where idplanilla = @idplanilla and idnumero = @idnumero and idincidencia = @indice and fecha between @inicio - 30 and @inicio - 1 and est_p = 'P')
	  if @dias_pagados >= 3 set @diasp = 0
	  else set @diasp = 3 - @dias_pagados
	end

	if @diasp > 0
	 begin
      declare datos cursor for 
	    select fecha from marcas_incidencias where idplanilla = @idplanilla and idnumero = @idnumero and idincidencia = @indice and fecha between @inicio and @fin
		open datos
		fetch next from datos into @fecha_incidencia
		while @@FETCH_STATUS = 0
		 begin
		   if @diasp > 0
		    begin
			 update marcas_incidencias set est_p = 'P' where idplanilla = @idplanilla and idnumero = @idnumero and idincidencia = @indice and fecha = @fecha_incidencia
			 set @diasp = @diasp - 1
			end
		 
		 fetch next from datos into @fecha_incidencia
		 end
		close datos
		deallocate datos  
	end	 
   --update marcas_incidencias set est_p = 'P' where idincidencia = @indice and idcompania = @idcomp and idplanilla = @idplanilla and idnumero = @idnumero and fecha between @inicio and @fin
   

   
   return
END
GO
ALTER AUTHORIZATION ON [dbo].[proceso_emp] TO  SCHEMA OWNER 
GO
/****** Object:  StoredProcedure [dbo].[salvo_marca]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[salvo_marca]
	@fentra datetime, @fsale datetime, @hentra varchar(5), @hsale varchar(5), @turno int, @idregistro bigint, @usuario varchar(15), @comentario varchar(2048)
AS
BEGIN
	

	declare @idplanilla varchar(8)
	declare @idnumero varchar(20)
	declare @fecha_entra datetime
	declare @fecha_sale datetime
	declare @hora_entra varchar(5)
	declare @hora_sale varchar(5)
	declare @idturno int

	select @idplanilla = idplanilla, @idnumero = idnumero, @fecha_entra = fecha_entra, @fecha_sale = fecha_sale, @hora_entra = hora_entra, @hora_sale = hora_sale,@idturno = idturno from marcas_proceso where idregistro = @idregistro
	update marcas_proceso set fecha_entra = @fentra, fecha_sale = @fsale, hora_entra = @hentra, hora_sale = @hsale, idturno = @turno where idregistro = @idregistro

	if @fecha_entra <> @fentra and @hora_entra <> @hentra
	begin
	 update marcas_extras_apb set fecha = @fentra,hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hora_entra
	 update marcas_mov_turnos set fecha = @fentra,hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hora_entra   
	end

	if @hora_entra <> @hentra
	 begin
	  update marcas set hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hora_entra and tipo = 1
	  if @@ROWCOUNT = 0 insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (@idplanilla,@idnumero,@fentra,@hentra,1,'RM','P')
	  update marcas_extras_apb set hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hora_entra
	  update marcas_incidencias set hentra = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hentra = @hora_entra
	  update marcas_mov_turnos set hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fentra and hora = @hora_entra
	  insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@idplanilla,@idnumero,getdate(),@usuario,@fecha_entra,@fentra,@hora_entra,@hentra,@comentario)
	  
	 end
	 	 
	 if @hora_sale <> @hsale
	 begin
	  update marcas set hora = @hsale where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_sale and hora = @hora_sale and tipo = 2
	  if @@ROWCOUNT = 0 insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal, estado) values (@idplanilla,@idnumero,@fsale,@hsale,2,'RM','P')
	  update marcas_incidencias set hsale = @hsale where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hsale = @hora_sale
	  insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@idplanilla,@idnumero,getdate(),@usuario,@fecha_sale,@fsale,@hora_sale,@hsale,@comentario)	  
	  
	 end

	 if @fecha_entra <> @fentra
	  begin
	    update marcas set fecha = @fentra where  idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hora_entra and tipo = 1
		update marcas_extras_apb set fecha = @fentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hora_entra
		update marcas_mov_turnos set fecha = @fentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_entra and hora = @hentra
		insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@idplanilla,@idnumero,getdate(),@usuario,@fecha_entra,@fentra,@hora_entra,@hora_entra,@comentario)
		
	  end

	  if @fecha_sale <> @fsale
	  begin
	    update marcas set fecha = @fsale where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha_sale and hora = @hora_sale and tipo = 2
		insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario) values (@idplanilla,@idnumero,getdate(),@usuario,@fecha_sale,@fsale,@hora_sale,@hsale,@comentario)
		
	  end	 
	  
	  if @idturno <> @turno
	  begin
	    update marcas_mov_turnos set turno = @turno where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fentra and hora = @hentra
		if @@ROWCOUNT = 0 insert into marcas_mov_turnos (idplanilla, idnumero, fecha, hora, turno) values (@idplanilla,@idnumero,@fentra,@hentra,@turno)
	  end 
END


GO
ALTER AUTHORIZATION ON [dbo].[salvo_marca] TO  SCHEMA OWNER 
GO

Create PROCEDURE [dbo].[ordeno_marcas] @idplanilla varchar(8), @idnumero varchar(16)
	
AS
BEGIN
		
	declare @fecha datetime
	declare @hora varchar(5)
	declare @tipo int
	declare @terminal varchar(4)
	declare @registro bigint
	declare @idregistro bigint
	declare @id varchar(5)
	declare @fd varchar(5)
	declare @desc_pro int
	
	
	 declare marca_proc cursor for select registro, fecha, hora, tipo from marcas where estado = 'C' order by idplanilla, idnumero, fecha, hora
	 open marca_proc
	 
	 fetch next from marca_proc into @idregistro, @fecha, @hora, @tipo
	 while @@FETCH_STATUS = 0
	 begin
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
	  set @desc_pro = (select isnull((select top 1 iddesc from marcas_descansos where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha order by iddesc),0))
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
	 	 
	 update marcas set estado = 'P' where registro = @idregistro
	 
	 fetch next from marca_proc into @idregistro, @fecha, @hora, @tipo
	 end
	 
	 close marca_proc
	 deallocate marca_proc
END

go

ALTER AUTHORIZATION ON [dbo].[ordeno_marcas] TO  SCHEMA OWNER 
GO


/****** Object:  StoredProcedure [dbo].[salvo_prog_turno]    Script Date: 29/05/2017 08:29:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[salvo_prog_turno] @idplanilla varchar(8), @idnumero varchar(20), @fecha datetime, @turno int, @usuario varchar(15)
AS
BEGIN
	
    declare @hora varchar(5)
	set @hora = ISNULL((select top 1 hora_entra from marcas_proceso where idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @fecha),'00:00')

	update marcas_mov_turnos set turno = @turno,usuario = @usuario,fecha_reg = getdate() where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and hora = @hora
	if @@ROWCOUNT = 0
	 begin
	  insert into marcas_mov_turnos (idplanilla,idnumero,fecha,hora,turno,usuario,fecha_reg)
	                         values (@idplanilla,@idnumero,@fecha,@hora,@turno,@usuario,GETDATE()) 
	 end

END


GO
ALTER AUTHORIZATION ON [dbo].[salvo_prog_turno] TO  SCHEMA OWNER 
GO

create PROCEDURE [dbo].[salvo_prog_turno_mlinea] @idplanilla varchar(8), @idnumero varchar(20), @fecha datetime, @turno int, @usuario varchar(15), @linea int
AS
BEGIN
	
    declare @hora varchar(5)
	if @linea = 0 set @linea = 1
	set @hora = ISNULL((select top 1 hora_entra from marcas_proceso where idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @fecha),'00:00')

	update marcas_mov_turnos set turno = @turno,usuario = @usuario,fecha_reg = getdate() where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and hora = @hora and linea = @linea
	if @@ROWCOUNT = 0
	 begin
	  insert into marcas_mov_turnos (idplanilla,idnumero,fecha,hora,turno,usuario,fecha_reg,linea)
	                         values (@idplanilla,@idnumero,@fecha,@hora,@turno,@usuario,GETDATE(),@linea) 
	 end

END
GO
ALTER AUTHORIZATION ON [dbo].[salvo_prog_turno_mlinea] TO  SCHEMA OWNER 
GO

create PROCEDURE [dbo].[in_marcas_movil]
	
AS
BEGIN
	
	declare @idnumero varchar(20)
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
	
	update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.idnumero
	--update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.tarjeta
	
	declare marca cursor for 
	 select idplanilla, idnumero,fecha,hora,tipo,idterminal,fecha_reg,long_reg,lat_reg,gps_cell from marcas_in where idnumero is not null and gps_cell is not null order by idplanilla,idnumero,fecha,hora
	 
	 open marca
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@gps
	 while @@FETCH_STATUS = 0
	 begin
	 
	  set @registro = (select top 1 registro from marcas where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and dbo.hrs_min(hora) between dbo.hrs_min(@hora) - 5  and dbo.hrs_min(@hora) + 5)
	  if @registro is null
	   begin
	   
	    insert into marcas (idplanilla, idnumero, fecha, hora, tipo, idterminal,fecha_reg,long_reg,lat_reg,gps_cell)
	                 values (@idplanilla, @idnumero, @fecha, @hora, @tipo, @terminal,@fecha_reg,@long_reg,@lat_reg,@gps)

        update marcas set imagen_reg = x.imagen_reg from marcas_in x where marcas.idplanilla = x.idplanilla and marcas.idnumero = x.idnumero and marcas.fecha_reg = x.fecha_reg
	    
	   end	  
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@gps
	 end
	 
	 close marca
	 deallocate marca
	
	 delete from marcas_in where idnumero is not null

	 declare marca_proc cursor for select registro, idplanilla, idnumero, fecha, hora, tipo from marcas where estado = 'N' order by idplanilla, idnumero, fecha, hora
	 open marca_proc
	 
	 fetch next from marca_proc into @idregistro, @idplanilla, @idnumero, @fecha, @hora, @tipo
	 while @@FETCH_STATUS = 0
	 begin
	  --set @tipo = dbo.tipo(@idcomp,@idplanilla,@idnumero,@fecha,@hora)
	  --update marcas set tipo = @tipo where idcomp = @idcomp and idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and hora = @hora 
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
	  set @desc_pro = (select isnull((select top 1 iddesc from marcas_descansos where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha order by iddesc),0))
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

ALTER AUTHORIZATION ON [dbo].[in_marcas_movil] TO  SCHEMA OWNER 
GO

CREATE PROCEDURE [dbo].[salvo_marca_descanso]
	@fentra datetime, @fsale datetime, @hentra varchar(5), @hsale varchar(5), @idregistro bigint, @usuario varchar(100), @comentario varchar(4096)
AS
BEGIN
	
	declare @fini datetime
	declare @ffin datetime
	declare @hini varchar(5)
	declare @hfin varchar(5)
	declare @idplanilla varchar(8)
	declare @idnumero varchar(16)

	select @idplanilla = idplanilla,@idnumero = idnumero,@fini = fecha,@hini = inicio_desc,@hfin = fin_desc from marcas_descansos where idregistro = @idregistro

	if dbo.hrs_min(@hini) > 720 and dbo.hrs_min(@hfin) < 720
	 begin
	  set @ffin = DATEADD(DAY,1, @fini)
	 end
	else
	 begin
	  set @ffin = @fini;
	 end

	 update marcas_descansos set fecha = @fentra, inicio_desc = @hentra, fin_desc = @hsale where idregistro = @idregistro
	 update marcas set fecha = @fentra, hora = @hentra where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fini and hora = @hini and tipo = 3
	 if @@ROWCOUNT = 0 insert into marcas (idplanilla,idnumero,fecha,hora,tipo,idterminal,estado) values (@idplanilla,@idnumero,@fentra,@hentra,3,'RM','P')
	 update marcas set fecha = @fsale, hora = @hsale where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @ffin and hora = @hfin and tipo = 3
	 if @@ROWCOUNT = 0 insert into marcas (idplanilla,idnumero,fecha,hora,tipo,idterminal,estado) values (@idplanilla,@idnumero,@fsale,@hsale,3,'RM','P')

	 if @hini <> @hentra
	  begin
	   insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario)
	   values (@idplanilla,@idnumero,GETDATE(),@usuario,@fini,@fentra,@hini,@hentra,@comentario)
	  end

	  if @fini <> @hfin
	   begin
	    insert into marcas_audit (idplanilla,idnumero,fecha,usuario,fecha_orig,fecha_chg,hora_orig,hora_chg,comentario)
	    values (@idplanilla,@idnumero,GETDATE(),@usuario,@ffin,@fsale,@hfin,@hsale,@comentario)
	   end

	return
END
GO
ALTER AUTHORIZATION ON [dbo].[salvo_marca_descanso] TO  SCHEMA OWNER 
GO

CREATE PROCEDURE [dbo].[agrego_marca_descanso] @IDPLANILLA VARCHAR(8),@IDNUMERO VARCHAR(16), @FENTRA DATETIME, @FSALE DATETIME, @HENTRA VARCHAR(5), @HSALE VARCHAR(5), @USUARIO VARCHAR(100), @COMENTARIO VARCHAR(4096) 
AS 
BEGIN 
DECLARE @FINI DATETIME 
DECLARE @FFIN DATETIME 
DECLARE @HINI VARCHAR(5) 
DECLARE @HFIN VARCHAR(5) 
declare @iddesc int
 

IF DBO.HRS_MIN(@HINI) > 720 AND DBO.HRS_MIN(@HFIN) < 720 
BEGIN SET @FFIN = DATEADD(DAY,1, @FINI) 
END 
ELSE 
BEGIN 
SET @FFIN = @FINI 
END 

set @iddesc = (select count(*) from marcas_descansos where idplanilla = @IDPLANILLA and idnumero = @IDNUMERO and fecha = @FENTRA)

set @iddesc = @iddesc + 1

insert into marcas_descansos (iddesc,idplanilla,idnumero,fecha,inicio_desc,fin_desc) values (@iddesc,@IDPLANILLA,@IDNUMERO,@FENTRA,@HENTRA,@HSALE)
INSERT INTO MARCAS (IDPLANILLA,IDNUMERO,FECHA,HORA,TIPO,IDTERMINAL,estado) VALUES (@IDPLANILLA,@IDNUMERO,@FENTRA,@HENTRA,3,'RM','P') 
INSERT INTO MARCAS (IDPLANILLA,IDNUMERO,FECHA,HORA,TIPO,IDTERMINAL,estado) VALUES (@IDPLANILLA,@IDNUMERO,@FSALE,@HSALE,3,'RM','P') 
INSERT INTO MARCAS_AUDIT (IDPLANILLA,IDNUMERO,FECHA,USUARIO,FECHA_ORIG,FECHA_CHG,HORA_ORIG,HORA_CHG,COMENTARIO) VALUES (@IDPLANILLA,@IDNUMERO,GETDATE(),@USUARIO,@FINI,@FENTRA,@HINI,@HENTRA,@COMENTARIO) 
INSERT INTO MARCAS_AUDIT (IDPLANILLA,IDNUMERO,FECHA,USUARIO,FECHA_ORIG,FECHA_CHG,HORA_ORIG,HORA_CHG,COMENTARIO) VALUES (@IDPLANILLA,@IDNUMERO,GETDATE(),@USUARIO,@FFIN,@FSALE,@HFIN,@HSALE,@COMENTARIO) 

RETURN 
END
GO

ALTER AUTHORIZATION ON [dbo].[agrego_marca_descanso] TO  SCHEMA OWNER 
GO

CREATE PROCEDURE [dbo].[in_marcas_web] 
@idnumero varchar(25)
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

	
	update marcas_in set idplanilla = e.idplanilla, idnumero = e.idnumero from empleados e where idtarjeta = e.idnumero and idtarjeta = @idnumero
	
	declare marca cursor for 
	 select idplanilla, idnumero,fecha,hora,tipo,idterminal,fecha_reg,long_reg,lat_reg,gps_cell,dir_ip,host 
		from marcas_in where idnumero = @idnumero order by idplanilla,idnumero,fecha,hora
	 
	 open marca
	 
	 Fetch next from marca into @idplanilla,@idnumero,@fecha,@hora,@tipo,@terminal,@fecha_reg,@long_reg,@lat_reg,@gps,@dir_ip,@host
	 while @@FETCH_STATUS = 0
	 begin
	 
	  set @registro = (select top 1 registro from marcas where idplanilla = @idplanilla and idnumero = @idnumero and fecha = @fecha and dbo.hrs_min(hora) between dbo.hrs_min(@hora) - 5  and dbo.hrs_min(@hora) + 5)
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
	  if @tipo = 1 /*entrada*/
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
	   
	    if @tipo = 2 /*salida*/
	   begin
	   set @registro = (select top 1 idregistro from marcas_proceso where  idplanilla = @idplanilla and idnumero = @idnumero and reg_sale = @idregistro)
	   if @registro is null
	    begin
	      set @registro = (select top 1 idregistro from marcas_proceso where  idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = Dateadd(day,-1,@fecha) and hora_sale = '00:00' and (1440 - dbo.hrs_min(hora_entra) + dbo.hrs_min(@hora)) < 1080)  /*nocturno 18 horas para par*/
		  if @registro is not null 
		    begin
			  update marcas_proceso set fecha_sale = @fecha, hora_sale = @hora,reg_sale = @idregistro where idregistro = @registro
            end
		  else
		   begin
		    set @registro = (select top 1 idregistro from marcas_proceso where idplanilla = @idplanilla and idnumero = @idnumero and fecha_entra = @fecha and hora_sale = '00:00' and hora_entra < @hora)
			if @registro is null 
			 begin 
			   insert into marcas_proceso (idplanilla,idnumero,fecha_entra,hora_sale,fecha_sale,hora_entra,reg_sale) values (@idplanilla,@idnumero,@fecha,@hora,@fecha,'00:00',@idregistro)
			   set @registro = (select @@IDENTITY)			   
             end
	        else update marcas_proceso set hora_sale = @hora,reg_sale = @idregistro where idregistro = @registro
		   end
	    end 
		update marcas_incidencias set idregistro = @registro,hsale = @hora where  idplanilla = @idplanilla and idnumero = @idnumero and (idregistro = 0 OR idregistro = @registro) and hsale = '00:00'
	   end

     if @tipo = 3 /*descanso*/
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
	 
	 if @tipo = 4 /*marca comedor*/
	  begin
	   insert into marcas_comedor (idplanilla,idnumero,fecha,hora,idterminal) values (@idplanilla,@idnumero,@fecha,@hora,@terminal)
	  end
	 update marcas set estado = 'P' where registro = @idregistro
	 
	 fetch next from marca_proc into @idregistro, @idplanilla, @idnumero, @fecha, @hora, @tipo
	 end
	 
	 close marca_proc
	 deallocate marca_proc

END
GO
ALTER AUTHORIZATION ON [dbo].[in_marcas_web] TO  SCHEMA OWNER 
GO
Create procedure [dbo].[DM_CONSULTAR_MARCAS_PERIODO]
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
	                                 idturno int, cantidad decimal(18,2), columna_concepto  varchar(10),
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
		sum(isnull(m.cantidad,0)) as cantidad,
		case when c.tipo_h=1 then 'Ordinario'
			 when c.tipo_h=2 then case when c.ordinario='T' and c.autorizado='T' then 'Ordinario' else 'Extras' end 
			 when c.tipo_h=3 then case when c.ordinario='T' and c.autorizado='T' then 'Ordinario' else 'Extras' end  
			 when c.tipo_h=4 then case when c.ordinario='T' and c.autorizado='T' then 'Ordinario' else 'Extras' end
			 else 'Ordinario'
	    end as columna_concepto,
		case when c.tipo_h=1 then null
			 when c.tipo_h=2 then 'Extras' 
			 when c.tipo_h=3 then 'Dobles'  
			 when c.tipo_h=4 then 'Otros' 
			 else null
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
	left join marcas_distribuciones m 
		on m.fecha=mp.fecha_entra and m.idplanilla=mp.idplanilla and m.idnumero=mp.idnumero and m.entrada=mp.hora_entra
	left join ph_conceptos c on c.id=m.idconcepto and c.tipo_h in (1,2,3,4)	
	inner join empleados e on e.idnumero=mp.idnumero 
		and e.idnumero=(case when @idnumero='-1' then e.idnumero else @idnumero end) 
		and e.idgrupo in (select idgrupo FROM @tGrupos)
		and e.estado='T'
		and e.idplanilla = @IdPlanilla
	WHERE mp.fecha_entra between @FechaInicio and @FechaFin	
	group by mp.idnumero,e.nombre,mp.fecha_entra,mp.hora_entra,mp.hora_sale, mp.idturno,m.estado,
	         c.tipo_h,c.ordinario,c.autorizado,mp.manticipo,mp.mtardia,
			 case when m.estado is not null then m.estado else mp.estado end,
			 mp.fecha_sale,mp.reg_sale,mp.idregistro, e.fecha_ingreso, e.iddepartamento
	

	


	if (@idnumero!='-1')
	begin


		select idnumero, nombre, fecha_entra, hora_entra, hora_sale, x.idturno,t.descripcion as turno, 
			  sum(ordinario) as ordinario, sum(extras) as extras, sum(suma_extras) as suma_extras, 
			  sum(suma_dobles) as suma_dobles, sum(suma_otros) as suma_otros, estado, 
			  max(manticipo) as manticipo, min(mtardia) as mtardia,fecha_sale,isnull(reg_sale,0) as reg_sale,
			  idregistro, fecha_ingreso, iddepartamento

		from (
			
			select idnumero, nombre, fecha_entra, hora_entra, hora_sale, idturno,  
				   case columna_concepto when 'Ordinario' then cantidad else 0 end as ordinario,
				   case columna_concepto when 'Extras' then cantidad else 0 end as extras,
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
				   case columna_concepto when 'Extras' then cantidad else 0 end as extras,
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