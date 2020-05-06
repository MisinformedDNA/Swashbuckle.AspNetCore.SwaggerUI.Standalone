using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerUI.Standalone;

namespace SwashBuckle.AspNetCore.SwaggerUI.Standalone
{
    public class SwaggerUIFileService
    {
        private readonly SwaggerUIOptions _options;
        private readonly SwaggerFileProvider _staticFileProvider;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SwaggerUIFileService(SwaggerUIOptions options)
        {
            _options = options ?? new SwaggerUIOptions();

            _staticFileProvider = CreateStaticFileProvider(_options);

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            _jsonSerializerOptions.IgnoreNullValues = true;
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
        }

        public HttpResponseMessage GetFileResponse(string path)
        {
            const string httpMethod = "GET";
            var response = new HttpResponseMessage();

            if (httpMethod == "GET" &&
                (string.IsNullOrEmpty(path) || Regex.IsMatch(path, $@"^index\.html$")))
            {
                RespondWithIndexHtml(response);
                return response;
            }

            _staticFileProvider.RespondWithStaticFile(path, response);

            return response;
        }

        private SwaggerFileProvider CreateStaticFileProvider(SwaggerUIOptions options)
        {
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = string.IsNullOrEmpty(options.RoutePrefix) ? string.Empty : $"/{options.RoutePrefix}"
            };

            return new SwaggerFileProvider(staticFileOptions);
        }

        private void RespondWithIndexHtml(HttpResponseMessage response)
        {
            response.StatusCode = HttpStatusCode.OK;

            using (var stream = _options.IndexStream())
            {
                var htmlBuilder = new StringBuilder(new StreamReader(stream).ReadToEnd());
                foreach (var entry in GetIndexArguments())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }

                response.Content = new StringContent(htmlBuilder.ToString(), Encoding.UTF8);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            }
        }

        private IDictionary<string, string> GetIndexArguments()
        {
            return new Dictionary<string, string>()
            {
                { "%(DocumentTitle)", _options.DocumentTitle },
                { "%(HeadContent)", _options.HeadContent },
                { "%(ConfigObject)", JsonSerializer.Serialize(_options.ConfigObject, _jsonSerializerOptions) },
                { "%(OAuthConfigObject)", JsonSerializer.Serialize(_options.OAuthConfigObject, _jsonSerializerOptions) }
            };
        }
    }
}
