using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IRelojesServices
    {
        /// <summary>
        /// GetRelojDispositivoAdm:  Obtine lista de dispositivos registrados 
        /// </summary>
        /// <returns>Lista de dispositivos registrados</returns>
        public Task<IEnumerable<cRelojDispositivoAdmin>> GetRelojDispositivoAdmin();

        /// <summary>
        /// GetRelojDispositivoAdm:  Obtine lista de dispositivos registrados por compañia y estado
        /// </summary>
        /// <param name="compania"</param>
        /// <param name="estado"></param>
        /// <returns>Lista de dispositivos registrados</returns>
        public Task<IEnumerable<cRelojDispositivoAdmin>> GetRelojDispositivoAdmin(string compania, string estado);

        /// <summary>
        /// GetRelojDispositivoAdmin:  obtiene los datos de un dispositivo especifico de acuerdo al Id de dispositivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<cRelojDispositivoAdmin> GetRelojDispositivoAdmin(string id);

        /// <summary>
        /// PostRelojDispositivoAdmin: metodo crear o modificar uno o varios registros de Relojes_Dispositivos en esquema ctadmin 
        /// </summary>
        /// <param name="relojes"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public Task<EventResponse> PostRelojDispositivoAdmin(IEnumerable<cRelojDispositivoAdmin> relojes);

        /// <summary>
        /// GetRelojDispositivo:  Obtine lista de dispositivos registrados en una compañia
        /// </summary>
        /// <returns>Lista de dispositivos registrados</returns>
        public Task<IEnumerable<cRelojDispositivo>> GetRelojDispositivo();
        /// <summary>
        /// GetRelojDispositivo:  obtiene los datos de un dispositivo especifico de acuerdo al Id de dispositivo en una compañia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<cRelojDispositivo> GetRelojDispositivo(int id);

        /// <summary>
        /// PostRelojDispositivo: metodo crear o modificar uno o varios registros de Relojes_Dispositivos en esquema de la compañia 
        /// </summary>
        /// <param name="relojes"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public Task<EventResponse> PostRelojDispositivo(IEnumerable<cRelojDispositivo> relojes);

        /// <summary>
        /// GetRelojUsuario:  Obtiene lista de usuarios registrados en relojes de una compañia
        /// </summary>
        /// <returns>Lista de usuarios registrados en relojes</returns>
        public Task<IEnumerable<cRelojUsuario>> GetRelojUsuario();

        /// <summary>
        /// GetRelojUsuario:  Obtiene un usuario asociado los relojes de una compañia 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<cRelojUsuario> GetRelojUsuario(string id);

        /// <summary>
        /// PostRelojUsuario: metodo crear o modificar uno o varios registros de usuarios de Relojes en esquema de la compañia 
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public Task<EventResponse> PostRelojUsuario(IEnumerable<cRelojUsuario> usuarios);


        /// <summary>
        /// GetRelojTemplate:  Obtiene lista de templates de huellas registrados para compañia
        /// </summary>
        /// <returns>Lista de templates de huellas registrados</returns>
        public Task<IEnumerable<cRelojTemplate>> GetRelojTemplate();

        /// <summary>
        /// GetRelojTemplate:  Obtiene los templates de huellas registrados para un empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<cRelojTemplate>> GetRelojTemplate(string id);

        /// <summary>
        /// PostRelojTemplate: metodo crear o modificar uno o varios registros de templates de huellas en esquema de la compañia 
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public Task<EventResponse> PostRelojTemplate(IEnumerable<cRelojTemplate> templates);


        /// <summary>
        /// GetRelojTemplate:  Obtiene lista de templates de huellas registrados para compañia
        /// </summary>
        /// <returns>Lista de templates de huellas registrados</returns>
        public Task<IEnumerable<cRelojTemplateFace>> GetRelojTemplateFace();

        /// <summary>
        /// GetRelojTemplateFace:  Obtiene los templates de huellas registrados para un empleado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<cRelojTemplateFace>> GetRelojTemplateFace(string id);

        /// <summary>
        /// PostRelojTemplateFace: metodo crear o modificar uno o varios registros de templates de huellas en esquema de la compañia 
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public Task<EventResponse> PostRelojTemplateFace(IEnumerable<cRelojTemplateFace> usuarios);

    }
}
