using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using MailKit.Net.Smtp;
using MimeKit;
using Multipart = MimeKit.Multipart;
using ContentDisposition = MimeKit.ContentDisposition;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace com.gsitcr.geotime.Data
{
    public class GraphSendMail: IGraphSendMail
    {
        
        private readonly string scope = "https://graph.microsoft.com/.default";
        private readonly string instance = "https://login.microsoftonline.com";
        private string apiName = $"https://graph.microsoft.com/v1.0/users/Remitente/sendMail";
        private string clientId = "";
        private string tenantId = "";
        private string clientSecret = "";
        private readonly ILogger<GraphSendMail> _logger;
        public GraphSendMail(ILogger<GraphSendMail> logger)
        {
            _logger = logger;
        }

        private async Task<string> GenerateToken()
        {
            try
            {

                using (HttpClient httpClient = new HttpClient())
                {
                    var tokenUrl = $"{instance}/{tenantId}/oauth2/v2.0/token";
                    var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

                    //request.Headers.Add("Referer", "login.microsoftonline.com");
                    //request.Headers.Add("Accept", "application/x-www-form-urlencoded");
                    //request.Headers.Add("CacheControl", "no-cache");

                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");

                    var AuthenticationContext = new Dictionary<string, string>
                    {
                        { "grant_type", "client_credentials" },
                        { "client_id", $"{clientId}" },
                        { "client_secret", $"{clientSecret}" },
                        { "scope", $"{scope}" },
                    };

                    request.Content = new FormUrlEncodedContent(AuthenticationContext);

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var respuesta = await response.Content.ReadFromJsonAsync<OAuthResponse>();
                        return respuesta!.access_token!;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);throw;
            }
            return "";

        }

        public EventResponse SendMailMSGraph(IEnumerable<Email> Mensajes, cParametroEmail parametrosCorreo)
        {
            EventResponse respuesta = new();
            try
            {
                clientId = parametrosCorreo.ClientId!;
                tenantId = parametrosCorreo.TenantId!;
                clientSecret = parametrosCorreo.ClientSecret!;
                var task = new Task(() =>
                {
                    foreach (Email Mensaje in Mensajes)
                    {
                        var task2 = new Task(async () =>
                        {
                            Object email_msg;

                            if (String.IsNullOrEmpty(Mensaje.Adjunto))
                            {
                                email_msg = new
                                {
                                    message = new
                                    {
                                        subject = $"{Mensaje.Asunto}",
                                        body = new
                                        {
                                            contentType = "HTML",
                                            content = $"{Mensaje.Cuerpo}"
                                        },
                                        toRecipients = new[]
                                        {
                                            new {
                                                emailAddress = new {
                                                        address= $"{Mensaje.Para}"
                                                    }
                                            }
                                        },
                                    }
                                };
                            }
                            else
                            {
                                Dictionary<string, string> adjunto = new Dictionary<string, string>()
                                {
                                    {"name" ,$"{Mensaje.Adjunto}"},
                                    {"contentBytes" ,$"{Convert.ToBase64String(Mensaje.StreamAdjunto)}" },
                                    {"contentType" ,"application/pdf"},
                                    {"@odata.type" ,"#microsoft.graph.fileAttachment"},
                                };

                                email_msg = new
                                {
                                    message = new
                                    {
                                        subject = Mensaje.Asunto,
                                        body = new
                                        {
                                            contentType = "HTML",
                                            content = Mensaje.Cuerpo
                                        },
                                        toRecipients = new[]
                                        {
                                            new {
                                                emailAddress = new {
                                                    address= Mensaje.Para
                                                }
                                            }
                                        },
                                        attachments = new[]
                                        {
                                            adjunto
                                        }
                                    }
                                };
                            }

                            apiName = apiName.Replace("Remitente", parametrosCorreo.DefaultEmail);

                            using var client = new HttpClient();
                            client.BaseAddress = new Uri(apiName);
                            var token = await GenerateToken();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                            client.DefaultRequestHeaders.Accept.Add(contentType);
                            
                            var postData = JsonSerializer.Serialize(email_msg);
                            var contentData = new StringContent(postData, Encoding.UTF8, "application/json");

                            var response = await client.PostAsync(apiName, contentData);

                            if (!response.IsSuccessStatusCode)
                            {
                                respuesta.Id = response.StatusCode.ToString();
                                respuesta.Descripcion = $"Error al enviar el correo. Detalle de error:{response.StatusCode} {response.ReasonPhrase}";
                                respuesta.Respuesta = "Error";
                            }
                        });
                        task2.Start();
                        task2.Wait(5000);
                    }
                });

                task.Start();

            }
            catch (Exception e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GraphSendMail.SendMailMSGraph: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");

                respuesta.Id = "1";
                respuesta.Descripcion = $"Error al enviar el correo. Detalle de error:{e.Message}";
                respuesta.Respuesta = "Error";
            }
            return respuesta;
        }

        public EventResponse EnviarCorreoElectronicoWithMailKit(IEnumerable<Email> correos, cParametroEmail parametrosCorreo)
        {
            EventResponse respuesta = new EventResponse();



            foreach (var correo in correos)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(parametrosCorreo.DefaultEmail, parametrosCorreo.DefaultEmail));
                message.To.Add(new MailboxAddress(correo.Para, correo.Para));
                message.Subject = correo.Asunto;

                // create our message text, just like before (except don't set it as the message.Body)
                var body = new TextPart("html")
                {
                    Text = correo.Cuerpo

                };

                var multipart = new Multipart("mixed");
                if (correo.Adjunto != "")
                {
                    var attachment = new MimePart("application", "pdf")
                    {
                        Content = new MimeContent(new MemoryStream(correo.StreamAdjunto!), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = correo.Adjunto
                    };

                    multipart.Add(attachment);
                }


                multipart.Add(body);

                // now set the multipart/mixed as the message body
                message.Body = multipart;

                using (var client = new SmtpClient())
                {
                    client.Connect(parametrosCorreo.SmtpServer, parametrosCorreo.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect, System.Threading.CancellationToken.None);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(parametrosCorreo.DefaultEmail, parametrosCorreo.DefaultPassWord);

                    client.Send(message);
                    client.Disconnect(true);
                }

            }

            return respuesta;
        }

        public EventResponse SendMailSMTP(IEnumerable<Email> correos, cParametroEmail parametrosCorreo)
        {
            EventResponse respuesta = new EventResponse();

            try
            {
                string password = parametrosCorreo.DefaultPassWord;

                var task = new Task(() =>
                {
                    foreach (var correo in correos)
                    {
                        var task2 = new Task(() =>
                        {
                            try
                            {
                                var message = new MimeMessage();
                                message.From.Add(new MailboxAddress(parametrosCorreo.DefaultEmail, parametrosCorreo.DefaultEmail));
                                message.To.Add(new MailboxAddress(correo.Para, correo.Para));
                                message.Subject = correo.Asunto;

                                // create our message text, just like before (except don't set it as the message.Body)
                                var body = new TextPart("html")
                                {
                                    Text = correo.Cuerpo

                                };

                                var multipart = new Multipart("mixed");
                                if (correo.Adjunto != "")
                                {
                                    var attachment = new MimePart("application", "pdf")
                                    {
                                        Content = new MimeContent(new MemoryStream(correo.StreamAdjunto!), ContentEncoding.Default),
                                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                        ContentTransferEncoding = ContentEncoding.Base64,
                                        FileName = correo.Adjunto
                                    };

                                    multipart.Add(attachment);
                                }

                                multipart.Add(body);

                                // now set the multipart/mixed as the message body
                                message.Body = multipart;

                                using (var client = new SmtpClient())
                                {
                                    client.Connect(parametrosCorreo.SmtpServer, parametrosCorreo.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect, System.Threading.CancellationToken.None);

                                    // Note: only needed if the SMTP server requires authentication
                                    client.Authenticate(parametrosCorreo.DefaultEmail, parametrosCorreo.DefaultPassWord);

                                    client.Send(message);
                                    client.Disconnect(true);
                                }
                                
                            }
                            catch (Exception ex1)
                            {
                                string error = (ex1.InnerException is null ? ex1.Message : ex1.InnerException.Message);
                                _logger.LogError($"GraphSendMail.SendMailSMTP: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");
                            }


                        });


                        task2.Start();
                        task2.Wait(5000);
                    }
                });


                task.Start();



            }
            catch (System.Net.WebException e)
            {
                string error = (e.InnerException is null ? e.Message : e.InnerException.Message);
                _logger.LogError($"GraphSendMail.SendMailSMTP: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");

            }
            catch (Exception ex)
            {
                string error = (ex.InnerException is null ? ex.Message : ex.InnerException.Message);
                _logger.LogError($"GraphSendMail.SendMailSMTP: Se ha presentado un error al ejecutar el proceso. Detalle de Error: {error}");


            }
            return respuesta;
        }
        public async Task<EventResponse> SendMailMSGraphAsync(IEnumerable<Email> Mensajes, cParametroEmail parametrosCorreo)
        {
            EventResponse respuesta = new();
            clientId = parametrosCorreo.ClientId!;
            tenantId = parametrosCorreo.TenantId!;
            clientSecret = parametrosCorreo.ClientSecret!;

            var tasks = Mensajes.Select(async Mensaje =>
            {
                try
                {
                    object email_msg;
                    if (string.IsNullOrEmpty(Mensaje.Adjunto))
                    {
                        email_msg = new
                        {
                            message = new
                            {
                                subject = Mensaje.Asunto,
                                body = new
                                {
                                    contentType = "HTML",
                                    content = Mensaje.Cuerpo
                                },
                                toRecipients = new[]
                                {
                                    new {
                                        emailAddress = new {
                                            address = Mensaje.Para
                                        }
                                    }
                                }
                            }
                        };
                    }
                    else
                    {
                        var adjunto = new Dictionary<string, string>
                        {
                            { "name", Mensaje.Adjunto },
                            { "contentBytes", Convert.ToBase64String(Mensaje.StreamAdjunto) },
                            { "contentType", "application/pdf" },
                            { "@odata.type", "#microsoft.graph.fileAttachment" }
                        };

                        email_msg = new
                        {
                            message = new
                            {
                                subject = Mensaje.Asunto,
                                body = new
                                {
                                    contentType = "HTML",
                                    content = Mensaje.Cuerpo
                                },
                                toRecipients = new[]
                                {
                                    new {
                                        emailAddress = new {
                                            address = Mensaje.Para
                                        }
                                    }
                                },
                                attachments = new[] { adjunto }
                            }
                        };
                    }

                    apiName = apiName.Replace("Remitente", parametrosCorreo.DefaultEmail);
                    using var client = new HttpClient();
                    client.BaseAddress = new Uri(apiName);
                    var token = await GenerateToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var postData = JsonSerializer.Serialize(email_msg);
                    var contentData = new StringContent(postData, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiName, contentData);

                    if (!response.IsSuccessStatusCode)
                    {
                        respuesta.Id = response.StatusCode.ToString();
                        respuesta.Descripcion = $"Error al enviar el correo. Detalle de error: {response.StatusCode} {response.ReasonPhrase}";
                        respuesta.Respuesta = "Error";
                        _logger.LogError($"GraphSendMail.SendMailMSGraph: Error al enviar correo a {Mensaje.Para}. Detalle: {response.StatusCode} {response.ReasonPhrase}");
                    }
                }
                catch (Exception e)
                {
                    string error = e.InnerException is null ? e.Message : e.InnerException.Message;
                    _logger.LogError($"GraphSendMail.SendMailMSGraph: Error al enviar correo a {Mensaje.Para}. Detalle: {error}");
                }
            });

            await Task.WhenAll(tasks);

            return respuesta;
        }
    }
}
