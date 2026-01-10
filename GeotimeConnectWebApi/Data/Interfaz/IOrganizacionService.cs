using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IOrganizacionService
    {
        /// <summary>
        /// GetOrganizacionNivel: obtener todos los registros de Organizacion Niveles
        /// </summary>
        /// <returns>Lista con todos los registros de Organizacion Niveles</returns>
        public Task<List<cOrganizacionNivel>> GetOrganizacionNivel();

        /// <summary>
        /// GetOrganizacionNivel: Obtiene un registro de Organizacion Niveles
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de Organizacion Niveles segun el id de parametro indicado</returns>
        public Task<cOrganizacionNivel> GetOrganizacionNivel(string id);

        /// <summary>
        /// PostOrganizacionNivel:  Recibe un registro de OrganizacionNivel, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PostOrganizacionNivel(IEnumerable<cOrganizacionNivel> model);
        /// <summary>
        /// DeleteOrganizacionNivel: Elimina un registro de Nivel de Organización
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> DeleteOrganizacionNivel(string id);

        /// <summary>
        /// GetOrganizacion: obtener todos los registros de Organizacion
        /// </summary>
        /// <returns>Lista con todos los registros de Organizacion</returns>
        public Task<List<cOrganizacion>> GetOrganizacion();
        /// <summary>
        /// GetOrganizacion: Obtiene un registro de Organizacion 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de Organizacion segun el id de parametro indicado</returns>
        public Task<cOrganizacion> GetOrganizacion(int id);

        /// <summary>
        /// GetOrganizacionNivel: Obtiene un registro de Organizacion Niveles
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un registro de Organizacion Niveles segun el id de parametro indicado</returns>
        public cOrganizacionNivel GetOrganizacionNivelById(string id);

        /// <summary>
        /// PostOrganizacion:  Recibe un registro de Organizacion, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PostOrganizacion(IEnumerable<cOrganizacion> model);

        /// <summary>
        /// DeleteOrganizacionNivel: Elimina un registro de Nivel de Organización
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> DeleteOrganizacion(string id);

        /// <summary>
        /// GetOrganizacionResponsable: Obtiene lista de responsables por organizacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>lista de responsables por organizacion segun el id de parametro indicado</returns>
        public Task<IEnumerable<cOrganizacionResponsable>> GetOrganizacionResponsable(int id);

        /// <summary>
        /// PostOrganizacionResponsable:  Recibe una lista de registros de PostOrganizacionResponsable, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PostOrganizacionResponsable(IEnumerable<cOrganizacionResponsable> model, int OrganizacionId);

        public Task<cAutorizante> GetResponsableOrganizacion(int OrganizacionId, int ordenPrioridad, DateTime? fechaAutoriza);


        /// <summary>
        /// GetOrganizacionBaseResponsable: Obtiene lista de responsables por organizacion base
        /// </summary>
        /// <param name="id"></param>
        /// <returns>lista de responsables por organizacion segun el id de parametro indicado</returns>
        public Task<IEnumerable<cOrganizacionBaseResponsable>> GetOrganizacionBaseResponsable(string id);

        public Task<IEnumerable<cOrganizacionBaseResponsable>> GetOrganizacionBaseByJefe(string id);

        /// <summary>
        /// PostOrganizacionBaseResponsable:  Recibe una lista de registros de Responsables de la organización, se verifica si existen en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Respuesta del evento con el resultado de la operación</returns>
        public Task<EventResponse> PostOrganizacionBaseResponsable(IEnumerable<cOrganizacionBaseResponsable> model);

        public Task<List<cEmpleadoJefatura>> GetEmpleadoJefatura();
        public Task<cEmpleadoJefatura> GetEmpleadoJefatura(string id);
        /// <summary>
        /// GetEmpleadoJefatura: muestra lista de empledos donde el id figura con autorizante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="esJefe"></param>
        /// <returns></returns>
        public Task<IEnumerable<cEmpleadoJefatura>> GetEmpleadoJefatura(string id, bool esJefe);
        public Task<EventResponse> PostEmpleadoJefatura(IEnumerable<cEmpleadoJefatura> empleadosJefaturas);
        public Task<EventResponse> DeleteEmpleadoJefatura(string id);



    }
}
