--
-- Creating function DIA_NUMERO
-- ============================
--
create or replace function geotime.dia_numero(pfecha in date)
return number
IS
pdiasem number;
begin
       select case to_char (pfecha, 'FmDay', 'nls_date_language=english')
          when 'Monday' then 2
          when 'Tuesday' then 3
          when 'Wednesday' then 4
          when 'Thursday' then 5
          when 'Friday' then 6
          when 'Saturday' then 7
          when 'Sunday' then 1
       end d into (pdiasem)
  from dual;

  return pdiasem;

end;
/

--
-- Creating function HRS_MIN
-- =========================
--
create or replace function geotime.hrs_min(phora in varchar2)
return number
IS pcantidad number;
phoras number;
pminutos number;
begin  
	   
	   pminutos := 0;
	   phoras := 0;
     phoras := TO_NUMBER(SUBSTR(phora,1,2)) * 60;
     pminutos :=   TO_NUMBER(SUBSTR(phora,4,2));

  return phoras + pminutos;

end;
/

--
-- Creating function SPLIT_STRING
-- ==============================
--
CREATE OR REPLACE FUNCTION geotime.split_String(
  i_str    IN  VARCHAR2, 
  i_delim  IN  VARCHAR2 DEFAULT ',' 
) RETURN SYS.ODCINUMBERLIST DETERMINISTIC 
AS 
  p_result       SYS.ODCINUMBERLIST := SYS.ODCINUMBERLIST(); 
  p_start        NUMBER(5) := 1; 
  p_end          NUMBER(5); 
  c_len CONSTANT NUMBER(5) := LENGTH( i_str ); 
  c_ld  CONSTANT NUMBER(5) := LENGTH( i_delim ); 
BEGIN 
  IF c_len > 0 THEN 
    p_end := INSTR( i_str, i_delim, p_start ); 
    WHILE p_end > 0 LOOP 
      p_result.EXTEND; 
      p_result( p_result.COUNT ) := SUBSTR( i_str, p_start, p_end - p_start ); 
      p_start := p_end + c_ld; 
      p_end := INSTR( i_str, i_delim, p_start ); 
    END LOOP; 
    IF p_start <= c_len + 1 THEN 
      p_result.EXTEND; 
      p_result( p_result.COUNT ) := SUBSTR( i_str, p_start, c_len - p_start + 1 ); 
    END IF; 
  END IF; 
  RETURN p_result; 
END; 
/

--
-- Creating function SPLITSTRING
-- =============================
--
create or replace function geotime.splitstring(stringToSplit in varchar2)
RETURN T_TABLE_COLL
IS nam varchar2(15);
posd number;
l_res_coll T_TABLE_COLL;
begin


  return l_res_coll;
end;
/

--
-- Creating function Tipo
-- =============================
--
create or replace function geotime.tipo (pidplanilla in varchar2, pidnumero in varchar2, pfecha in date, phora in varchar2)
return integer is
  poldtipo int;
begin
   --poldtipo := 2;
 begin 
  select tipo into poldtipo from (select tipo from marcas where idplanilla = pidplanilla and idnumero = pidnumero and fecha_hora between to_date(to_char(pfecha,'YYYY-MM-DD')||' '||phora,'YYYY-MM-DD HH24:MI') - 1 and to_date(to_char(pfecha,'YYYY-MM-DD')||' '||phora,'YYYY-MM-DD HH24:MI') + 1/1440*-1 order by fecha_hora desc) where rownum = 1;
  exception when no_data_found then poldtipo := 2;
 end;   
 
  if poldtipo = 1 then 
    begin
      --update extradm_ca.marcas set tipo = 2 where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hora = phora;
      --commit;
     return 2;
    end; 
  else 
    begin
      --update extradm_ca.marcas set tipo = 1 where idplanilla = pidplanilla and idnumero = pidnumero and fecha = pfecha and hora = phora;
      --commit;
      return 1;
    end;  
  end if;
    
  --return(1);
end tipo;
/