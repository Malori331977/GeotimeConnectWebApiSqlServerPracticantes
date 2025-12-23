using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models.Utils;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace com.gsitcr.geotime.Data
{
    public class GenericService:IGenericService
    {
        private readonly ILogger<GenericService> _logger;
        private string InterfaceName = "ErpConnect";
        private UrlCompania? ApiData;

        public event Action OnChange;
        public GenericService(ILogger<GenericService> logger)
        {
            _logger = logger;
            ApiData = null;
        }

        
        public void SetCompania(UrlCompania Compania)
        {
            ApiData = Compania;
            NotifyStateChanged();
        }
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        /// <summary>
        /// GetUrlApi: Método para obtener los datos de configuracion que se encuentran en el appsettings.json
        /// </summary>
        /// <returns>Instancia de la clase AppSettings</returns>
        private async Task<AppSettings> GetUrlApi()
        {
            AppSettings? appSettings = null;

            try
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var section = config.GetSection("AppSettings");

                if (ApiData is null)
                {


                    appSettings = new AppSettings
                    {
                        ApiErpUrl = section.GetSection("ApiErpUrl").Value!,
                        ApiErpDataBase = section.GetSection("ApiErpDataBase").Value!,
                        ApiErpSchema = section.GetSection("ApiApiSchema").Value!,
                        ERPTimeOut = section.GetSection("ERPTimeOut").Value!,
                    };
                }
                else
                {
                    appSettings = new AppSettings
                    {
                        ApiErpUrl = ApiData.Url,
                        ApiErpDataBase = ApiData.BaseDatos,
                        ApiErpSchema = ApiData.SchemaBd,
                        ApiClientId = ApiData.ApiClientId,
                        ApiPassword = ApiData.ApiPassword,
                        ApiUser = ApiData.ApiUser,
                        ERPTimeOut = section.GetSection("ERPTimeOut").Value!,
                    };
                }


            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{InterfaceName}.GetUrlApi: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
            }


            return appSettings!;

        }

        /// <summary>
        /// getToken: Método para obtener token de acceso a la Api
        /// </summary>
        /// <returns>String con el token solicitado</returns>

        public async Task<string> GetToken()
        {

            string? token = "";
            try
            {
                AppSettings appSettings = await GetUrlApi();

                // Use HttpClientHandler to configure low-level settings if needed
                var handler = new HttpClientHandler();

                // Create HttpClient with optional timeout from settings (default 100s)
                using var client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, int.Parse(appSettings.ERPTimeOut!), 0),
                };

                var api = appSettings.ApiErpUrl + "/User";
                client.BaseAddress = new Uri(appSettings.ApiErpUrl!);
                var userRequest = new UserRequest
                {
                    ClientId = appSettings.ApiClientId ?? "197ac2e4bd0843c3974725a6544e1089c4a7dcae59087543ba6428c9914c35d9",
                    User = appSettings.ApiUser ?? "GSITCR",
                    Password = appSettings.ApiPassword ?? "c5bbf3d10de5c6dfdad016e6e948a27d343b5e22f35471324388460c4e14a27c",
                    Schema = appSettings.ApiErpSchema,
                    BDName = appSettings.ApiErpDataBase
                };

                //_logger.LogError($"GeotimeApi.GetToken: Data: {userRequest.ClientId}|{userRequest.User}|{userRequest.Password}|{userRequest.Schema}|{userRequest.BDName}");

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var postData = JsonSerializer.Serialize(userRequest);
                var contentData = new StringContent(postData, Encoding.UTF8, "application/json");

                // Use cancellation token with the same timeout as HttpClient to be explicit
                using var cts = new CancellationTokenSource(client.Timeout);

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(api, contentData, cts.Token);
                }
                catch (TaskCanceledException tex)
                {
                    if (cts.IsCancellationRequested)
                    {
                        _logger.LogError($"GetToken: Request timed out after {client.Timeout.TotalSeconds} seconds.");
                    }
                    else
                    {
                        _logger.LogError($"GetToken: Request was cancelled. {tex.Message}");
                    }
                    throw;
                }

                if (response.IsSuccessStatusCode)
                {
                    var respuesta = await response.Content.ReadFromJsonAsync<UserResponse>();

                    if (respuesta is not null)
                        token = respuesta.Token;
                }
                else
                {
                    _logger.LogError($"GetToken: Error: {response.StatusCode}, Detalle: {response.Content}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //_logger.LogError($"GeotimeApi.GetToken: {token}");
            return token ?? "";
        }

        /// <summary>
        /// Post: Método para crear o actualizar registros en objetos de la base de datos a traves del método Post
        /// </summary>
        /// <returns>Objeto de la clase EventResponse </returns>
        /// <param name="apiName">Nombre del método de la Api a Ejecutar</param>
        /// <param name="model">Objeto de la clase a enviar en el cuerpo del mensaje</param>
        public async Task<EventResponse> Post(string apiName, object model)
        {
            EventResponse eventoAdd = new();

            try
            {
                AppSettings appSettings = await GetUrlApi();

                // Use HttpClientHandler to configure low-level settings if needed
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                };
                // Create HttpClient with optional timeout from settings (default 100s)
                using var client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, int.Parse(appSettings.ERPTimeOut!), 0),
                };

                apiName = appSettings.ApiErpUrl + "/" + apiName;
                client.BaseAddress = new Uri(appSettings.ApiErpUrl!);
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var postData = JsonSerializer.Serialize(model);
                var contentData = new StringContent(postData, Encoding.UTF8, "application/json");

                // Use cancellation token with the same timeout as HttpClient to be explicit
                using var cts = new CancellationTokenSource(client.Timeout);

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(apiName, contentData, cts.Token);
                }
                catch (TaskCanceledException tex)
                {
                    if (cts.IsCancellationRequested)
                    {
                        _logger.LogError($"Post: Request to '{apiName}' timed out after {client.Timeout.TotalSeconds} seconds.");
                    }
                    else
                    {
                        _logger.LogError($"Post: Request to '{apiName}' was cancelled. {tex.Message}");
                    }
                    return new EventResponse
                    {
                        Respuesta = "ERROR",
                        Descripcion = "Request timed out or was cancelled.",
                        Id = "1",
                        ValorRetorno = ""
                    };
                }

                // Verify response and handle errors
                if (response.IsSuccessStatusCode)
                {
                    eventoAdd = await response.Content.ReadFromJsonAsync<EventResponse>() ?? new EventResponse();
                }
                else
                {
                    EventResponse respBadRequest = new();
                    try
                    {
                        respBadRequest = await response.Content.ReadFromJsonAsync<EventResponse>() ?? new EventResponse();

                    }
                    catch
                    {

                    }
                    eventoAdd.Id = "1";
                    eventoAdd.Respuesta = "ERROR";
                    eventoAdd.Descripcion = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";

                    if (!String.IsNullOrEmpty(respBadRequest.Descripcion))
                    {
                        eventoAdd.Descripcion = eventoAdd.Descripcion + "" + respBadRequest.Descripcion;
                    }
                }
            }
            catch (Exception e)
            {
                string errorMessage = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError(errorMessage);

                eventoAdd = new EventResponse
                {
                    Respuesta = "ERROR",
                    Descripcion = $"{errorMessage}",
                    Id = "1",
                };
            }
            return eventoAdd;
        }

        /// <summary>
        /// Put: Método para actualizar registros en objetos de la base de datos a traves del método Put
        /// </summary>
        /// <returns>Objeto de la clase EventResponse </returns>
        /// <param name="apiName">Nombre del método de la Api a Ejecutar</param>
        /// <param name="model">Objeto de la clase a enviar en el cuerpo del mensaje</param>
        public async Task<EventResponse> Put(string apiName, object model)
        {
            EventResponse? eventoAdd = new();

            try
            {
                AppSettings appSettings = await GetUrlApi();
                // Use HttpClientHandler to configure low-level settings if needed
                // Use HttpClientHandler to configure low-level settings if needed
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                };

                // Create HttpClient with optional timeout from settings (default 100s)
                using var client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, int.Parse(appSettings.ERPTimeOut!), 0),
                };

                apiName = appSettings.ApiErpUrl + "/" + apiName;
                client.BaseAddress = new Uri(appSettings.ApiErpUrl);
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var putData = JsonSerializer.Serialize(model);
                var contentData = new StringContent(putData, Encoding.UTF8, "application/json");

                // Use cancellation token with the same timeout as HttpClient to be explicit
                using var cts = new CancellationTokenSource(client.Timeout);

                HttpResponseMessage response;
                try
                {
                    response = await client.PutAsync(apiName, contentData, cts.Token);
                }
                catch (TaskCanceledException tex)
                {
                    if (cts.IsCancellationRequested)
                    {
                        _logger.LogError($"Put: Request to '{apiName}' timed out after {client.Timeout.TotalSeconds} seconds.");
                    }
                    else
                    {
                        _logger.LogError($"Put: Request to '{apiName}' was cancelled. {tex.Message}");
                    }

                    return new EventResponse
                    {
                        Respuesta = "ERROR",
                        Descripcion = "Request timed out or was cancelled.",
                        Id = "1",
                        ValorRetorno = ""
                    };
                }

                // Verify response and handle errors
                if (response.IsSuccessStatusCode)
                {
                    eventoAdd = await response.Content.ReadFromJsonAsync<EventResponse>() ?? new EventResponse();
                }
                else
                {
                    eventoAdd.Id = "1";
                    eventoAdd.Respuesta = "ERROR";
                    eventoAdd.Descripcion = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";
                }
            }
            catch (Exception e)
            {
                string errorMessage = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError(errorMessage);

                eventoAdd = new EventResponse
                {
                    Respuesta = "ERROR",
                    Descripcion = $"{errorMessage}",
                    Id = "1",
                };

            }
            return eventoAdd!;
        }

        /// <summary>
        /// Delete:  Metodo generico para borrado de datos de las tablas
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Delete(string apiName, string id)
        {
            EventResponse respuesta = new();
            try
            {
                AppSettings appSettings = await GetUrlApi();

                // Use HttpClientHandler to configure low-level settings if needed
                var handler = new HttpClientHandler();

                // Create HttpClient with optional timeout from settings (default 100s)
                using var client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, int.Parse(appSettings.ERPTimeOut!), 0),
                };

                apiName = $"{appSettings.ApiErpUrl}/{apiName}/{id}";
                client.BaseAddress = new Uri(appSettings.ApiErpUrl);
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                // Use cancellation token with the same timeout as HttpClient to be explicit
                using var cts = new CancellationTokenSource(client.Timeout);

                HttpResponseMessage response;
                try
                {
                    response = await client.DeleteAsync(apiName, cts.Token);
                }
                catch (TaskCanceledException tex)
                {
                    if (cts.IsCancellationRequested)
                    {
                        _logger.LogError($"Delete: Request to '{apiName}' timed out after {client.Timeout.TotalSeconds} seconds.");
                    }
                    else
                    {
                        _logger.LogError($"Delete: Request to '{apiName}' was cancelled. {tex.Message}");
                    }

                    return new EventResponse
                    {
                        Respuesta = "ERROR",
                        Descripcion = "Request timed out or was cancelled.",
                        Id = "1",
                        ValorRetorno = ""
                    };
                }

                // Verify response and handle errors
                if (response.IsSuccessStatusCode)
                {
                    respuesta = await response.Content.ReadFromJsonAsync<EventResponse>() ?? new EventResponse();
                }
                else
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Delete: Error calling '{apiName}'. Status: {response.StatusCode}. Content: {contentString}");

                    try
                    {
                        respuesta = JsonSerializer.Deserialize<EventResponse>(contentString) ?? new EventResponse();
                        // If the API indicates an error, normalize the Respuesta/Descripcion
                        if (respuesta.Respuesta is null || respuesta.Respuesta.ToUpper() != "OK")
                        {
                            respuesta.Id = "1";
                            respuesta.Respuesta = respuesta.Respuesta ?? "ERROR";
                            respuesta.Descripcion = respuesta.Descripcion ?? $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Delete: Failed to deserialize error response from '{apiName}'. Exception: {ex.Message}");
                        respuesta = new EventResponse
                        {
                            Respuesta = "ERROR",
                            Descripcion = $"HTTP {(int)response.StatusCode} - {response.ReasonPhrase}: {contentString}",
                            Id = "0",
                            ValorRetorno = "-1"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                string errorMessage = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError(errorMessage);

                respuesta = new EventResponse
                {
                    Respuesta = "ERROR",
                    Descripcion = $"{errorMessage}",
                    Id = "1",
                };

            }

            return respuesta;

        }

        /// <summary>
        /// Delete:  Metodo generico para borrado de datos de TipoMarca
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public async Task<EventResponse> Delete(string apiName, string id, string nivel)
        {
            EventResponse respuesta = new();
            try
            {
                AppSettings appSettings = await GetUrlApi();
                // Use HttpClientHandler to configure low-level settings if needed
                var handler = new HttpClientHandler();

                // Create HttpClient with optional timeout from settings (default 100s)
                using var client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, int.Parse(appSettings.ERPTimeOut!), 0),
                };

                apiName = $"{appSettings.ApiErpUrl}/{apiName}/{id}/{nivel}";
                client.BaseAddress = new Uri(appSettings.ApiErpUrl);
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                // Use cancellation token with the same timeout as HttpClient to be explicit
                using var cts = new CancellationTokenSource(client.Timeout);

                HttpResponseMessage response;
                try
                {
                    response = await client.DeleteAsync(apiName, cts.Token);
                }
                catch (TaskCanceledException tex)
                {
                    if (cts.IsCancellationRequested)
                    {
                        _logger.LogError($"Delete: Request to '{apiName}' timed out after {client.Timeout.TotalSeconds} seconds.");
                    }
                    else
                    {
                        _logger.LogError($"Delete: Request to '{apiName}' was cancelled. {tex.Message}");
                    }

                    return new EventResponse
                    {
                        Respuesta = "ERROR",
                        Descripcion = "Request timed out or was cancelled.",
                        Id = "1",
                        ValorRetorno = ""
                    };
                }

                // Verify response and handle errors
                if (response.IsSuccessStatusCode)
                {
                    respuesta = await response.Content.ReadFromJsonAsync<EventResponse>() ?? new EventResponse();
                }
                else
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Delete: Error calling '{apiName}'. Status: {response.StatusCode}. Content: {contentString}");

                    try
                    {
                        respuesta = JsonSerializer.Deserialize<EventResponse>(contentString) ?? new EventResponse();
                        // If the API indicates an error, normalize the Respuesta/Descripcion
                        if (respuesta.Respuesta is null || respuesta.Respuesta.ToUpper() != "OK")
                        {
                            respuesta.Id = "1";
                            respuesta.Respuesta = respuesta.Respuesta ?? "ERROR";
                            respuesta.Descripcion = respuesta.Descripcion ?? $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Delete: Failed to deserialize error response from '{apiName}'. Exception: {ex.Message}");
                        respuesta = new EventResponse
                        {
                            Respuesta = "ERROR",
                            Descripcion = $"HTTP {(int)response.StatusCode} - {response.ReasonPhrase}: {contentString}",
                            Id = "0",
                            ValorRetorno = "-1"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                string errorMessage = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError(errorMessage);

                respuesta = new EventResponse
                {
                    Respuesta = "ERROR",
                    Descripcion = $"{errorMessage}",
                    Id = "1",
                };

            }

            return respuesta;

        }
        public async Task<HttpResponseMessage> Get(string method)
        {
            AppSettings appSettings = await GetUrlApi();
            using var client = new HttpClient();
            client.BaseAddress = new Uri(appSettings.ApiErpUrl!);
            var api = $"{appSettings.ApiErpUrl}{method}";
            var token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await client.GetAsync(api);
        }

        public async Task<IEnumerable<T>> Get<T>(string controlador)
        {
            try
            {
                var method = $"/{controlador}";
                AppSettings appSettings = await GetUrlApi();
                using var client = new HttpClient();
                client.BaseAddress = new Uri(appSettings.ApiErpUrl!);
                var api = $"{appSettings.ApiErpUrl}{method}";
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(api);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
                }
                return default;
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{InterfaceName}.Get{controlador}: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                return default;
            }
        }
        public async Task<IEnumerable<T>> Get<T>(string controlador, bool SoloActivos)
        {
            try
            {
                var method = $"/{controlador}";
                AppSettings appSettings = await GetUrlApi();
                using var client = new HttpClient();
                client.BaseAddress = new Uri(appSettings.ApiErpUrl);
                var api = $"{appSettings.ApiErpUrl}{method}";
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(api);

                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
                    var tipo = typeof(T);
                    var propiedad = tipo.GetProperty("Activo");
                    if (propiedad != null)
                    {
                        var listaFiltrada = lista
                        .Where(item => propiedad.GetValue(item)?.Equals(true) == true)
                        .ToList();
                        return listaFiltrada;
                    }

                    return lista;
                }
                return default;
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{InterfaceName}.Get{controlador}: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                return default;
            }
        }
        public async Task<T> Get<T>(string controlador, string id)
        {
            try
            {
                var method = $"/{controlador}/{id}";
                AppSettings appSettings = await GetUrlApi();
                using var client = new HttpClient();
                client.BaseAddress = new Uri(appSettings.ApiErpUrl);
                var api = $"{appSettings.ApiErpUrl}{method}";
                var token = await GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(api);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                return default;
            }
            catch (Exception e)
            {
                string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                _logger.LogError($"{InterfaceName}.Get{controlador}: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                return default;
            }
        }


    }
}
