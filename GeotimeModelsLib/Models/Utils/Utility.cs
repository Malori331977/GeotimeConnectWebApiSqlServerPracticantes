namespace com.gsitcr.geotime.Models.Utils
{
    public static class Utility
    {
        
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        public static string MunitosAHoras(decimal valor)
        {
          
            //pasar valor en horas a minutos
            var minutosTotal = (valor * 60);
            var horasEnteras = Math.Truncate(minutosTotal / 60);
            var minutos = Math.Truncate(minutosTotal % 60);

            string horasStr = ((int)horasEnteras).ToString().PadLeft(2, '0');
            string minutosStr = ((int)minutos).ToString().PadLeft(2, '0');
            

            return $"{horasStr}:{minutosStr}";

            
        }

        public static int HorasAMinutos(string valor)
        {
            //valor=formato de hora 00:00

            int Hora = int.Parse(valor.Substring(0,2));
            int Minutos = int.Parse(valor.Substring(3, 2));
            var minutosTotal = (Hora * 60) + Minutos;
           
            return minutosTotal;
        }




    }
}
