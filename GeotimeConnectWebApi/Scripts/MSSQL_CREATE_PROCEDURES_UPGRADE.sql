
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
ALTER PROCEDURE [salvcr].[apruebo_extra] 
    @CANTIDAD VARCHAR(5), 
    @COMENTARIO VARCHAR(1024), 
    @USUARIO VARCHAR(15), 
    @IDREGISTRO BIGINT, 
    @IDCCOSTO VARCHAR(25),
	@CANTIDAD_COMPENSAR VARCHAR(5)
AS 
BEGIN 
	
    DECLARE @IDPLANILLA VARCHAR(8),
		 @IDNUMERO VARCHAR(20),
		 @FECHA DATETIME,
		 @HORA VARCHAR(5),
		 @EXTC VARCHAR(5),
		 @NIVEL_APROB INT,
		 @USA_COMPENSACION CHAR(1),
		 @TCOMPENSADO_ANT VARCHAR(5)='00:00'

    SELECT 
        @IDPLANILLA = IDPLANILLA, 
        @IDNUMERO = IDNUMERO, 
        @FECHA = FECHA_ENTRA, 
        @HORA = HORA_ENTRA,
        @EXTC = EXTC 
    FROM MARCAS_PROCESO 
    WHERE IDREGISTRO = @IDREGISTRO

	SELECT @USA_COMPENSACION=USA_COMPENSACION FROM PH_OPCIONES

    SET @NIVEL_APROB = (
        SELECT ISNULL(NIVEL_APROB_EXT, 1) 
        FROM PH_USUARIO 
        WHERE IDUSUARIO = (
            SELECT IDUSUARIO 
            FROM CTADMIN.PH_LOGIN 
            WHERE USUARIO = @USUARIO
        )
    )

    BEGIN TRY
		
		if (@USA_COMPENSACION='T')
		begin
			IF DBO.HRS_MIN(@EXTC) < (DBO.HRS_MIN(@CANTIDAD) +  DBO.HRS_MIN(@CANTIDAD_COMPENSAR))
			begin
				RAISERROR ('La suma de las horas extras aprobadas más las horas compensadas no debe ser mayor que las horas extras calculadas.', -- Message text.
					16, -- Severity.
					1 -- State.
				);
			end 				
		end
		else
		begin
			IF DBO.HRS_MIN(@EXTC) < DBO.HRS_MIN(@CANTIDAD)
				SET @CANTIDAD = @EXTC
		end 

        IF @NIVEL_APROB = 1 
        BEGIN
			declare @CCosto varchar(25)

			set @CCosto = case when @IDCCOSTO = 'NAP_' then null else  @IDCCOSTO end

			if exists(select 1 from MARCAS_EXTRAS_APB 
			          WHERE IDPLANILLA = @IDPLANILLA 
						AND IDNUMERO = @IDNUMERO 
						AND FECHA = @FECHA 
						AND HORA = @HORA)
			begin
				set @TCOMPENSADO_ANT=(select top 1 cant_comp_aprob_nivel1 
										from MARCAS_EXTRAS_APB 
										WHERE IDPLANILLA = @IDPLANILLA 
										AND IDNUMERO = @IDNUMERO 
										AND FECHA = @FECHA 
										AND HORA = @HORA)
				print @TCOMPENSADO_ANT

				UPDATE MARCAS_EXTRAS_APB 
                SET 
                    CANTIDAD = @CANTIDAD, 
                    CANTIDAD_APROB_NIVEL1 = @CANTIDAD,
                    COMENTARIO = @COMENTARIO, 
                    USUARIO = @USUARIO,
                    CCOSTO = @CCosto, 
                    APROB_NIVEL1 = 'T', 
                    USUARIO_APROB_NIVEL1 = @USUARIO, 
                    COMENTARIO_APROB_NIVEL1 = @COMENTARIO,
                    FECHA_APROB_NIVEL1 = GETDATE(),
					CANT_COMP_APROB_NIVEL1 = @CANTIDAD_COMPENSAR
                WHERE 
                    IDPLANILLA = @IDPLANILLA 
                    AND IDNUMERO = @IDNUMERO 
                    AND FECHA = @FECHA 
                    AND HORA = @HORA

			end
			else
			begin
				INSERT INTO MARCAS_EXTRAS_APB (
                    IDPLANILLA, IDNUMERO, FECHA, HORA, CANTIDAD, 
                    COMENTARIO, USUARIO, CCOSTO, APROB_NIVEL1, 
                    USUARIO_APROB_NIVEL1, COMENTARIO_APROB_NIVEL1, 
                    FECHA_APROB_NIVEL1, CANTIDAD_APROB_NIVEL1, CANT_COMP_APROB_NIVEL1
                ) VALUES (
                    @IDPLANILLA, @IDNUMERO, @FECHA, @HORA, @CANTIDAD, 
                    @COMENTARIO, @USUARIO, @CCosto, 'T', @USUARIO, 
                    @COMENTARIO, GETDATE(), @CANTIDAD,@CANTIDAD_COMPENSAR
                )
                    
			end


            UPDATE MARCAS_PROCESO 
            SET EXTT = @CANTIDAD 
            WHERE IDREGISTRO = @IDREGISTRO

			if (@USA_COMPENSACION='T')
			BEGIN
				UPDATE empleados 
				SET TCompensacionAprobado = dbo.min_hrs(DBO.HRS_MIN(isnull(TCompensacionAprobado,'00:00')) - DBO.HRS_MIN(@TCOMPENSADO_ANT) + DBO.HRS_MIN(@CANTIDAD_COMPENSAR))
				where IDNUMERO = @IDNUMERO
			END
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

    SELECT
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    -- Use RAISERROR inside the CATCH block to return error
    -- information about the original error that caused
    -- execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
        @ErrorSeverity, -- Severity.
        @ErrorState -- State.
    );
    END CATCH
END

