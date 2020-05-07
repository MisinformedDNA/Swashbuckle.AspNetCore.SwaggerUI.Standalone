using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace Swashbuckle.AspNetCore.SwaggerUI.Standalone
{
    internal class SwaggerFileProvider : EmbeddedFileProvider
    {
        private const string EmbeddedFileNamespace = "Swashbuckle.AspNetCore.SwaggerUI.node_modules.swagger_ui_dist";
        private readonly StaticFileOptions _options;

        public SwaggerFileProvider(StaticFileOptions options)
            : base(typeof(SwaggerUIMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace)
        {
            _options = options;
        }

        public HttpResponseMessage RespondWithStaticFile(string path, HttpResponseMessage response)
        {
            var file = GetFileInfo(path);

            if (!file.Exists)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StreamContent(file.CreateReadStream());
            if (new FileExtensionContentTypeProvider().TryGetContentType(path, out var contentType))
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }

            return response;
        }
    }
}
