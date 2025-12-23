using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models.Utils;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IGenericService
    {
        public event Action OnChange;

        #region MetodosCRUD
        /// <summary>
        /// setCompania: Metodo utilizado para cambiar de compañia 
        /// </summary>
        /// <param name="schema">Datos de la compañia</param>
        public void SetCompania(UrlCompania Compania);

        /// <summary>
        /// Post: Método para crear o actualizar registros en objetos de la base de datos a traves del método Post
        /// </summary>
        /// <returns>Objeto de la clase EventResponse </returns>
        /// <param name="apiName">Nombre del método de la Api a Ejecutar</param>
        /// <param name="model">Objeto de la clase a enviar en el cuerpo del mensaje</param>

        public Task<EventResponse> Post(string apiName, object model);

        /// <summary>
        /// Put: Método para actualizar registros en objetos de la base de datos a traves del método Put
        /// </summary>
        /// <returns>Objeto de la clase EventResponse </returns>
        /// <param name="apiName">Nombre del método de la Api a Ejecutar</param>
        /// <param name="model">Objeto de la clase a enviar en el cuerpo del mensaje</param>
        public Task<EventResponse> Put(string apiName, object model);

        /// <summary>
        /// Delete:  Metodo generico para boorado de datos de las tablas
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Delete(string apiName, string id);

        /// <summary>
        /// Delete:  Metodo generico para borado de tipoMarca
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="id"></param>
        /// <param name="nivel"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Delete(string apiName, string id, string nivel);

        public Task<HttpResponseMessage> Get(string method);
        public Task<IEnumerable<T>> Get<T>(string controlador);
        public Task<IEnumerable<T>> Get<T>(string controlador, bool SoloActivos);
        public Task<T> Get<T>(string controlador, string id);

        #endregion
    }
}
