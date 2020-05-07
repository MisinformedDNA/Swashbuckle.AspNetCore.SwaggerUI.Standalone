using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI.Standalone;
using System.IO;
using System.Net.Http;

namespace AzureFunctionsV3.Sample
{
    public class SwaggerFunctions
    {
        private readonly SwaggerUIService _swaggerService;

        public SwaggerFunctions(SwaggerUIService swaggerService)
        {
            _swaggerService = swaggerService;
        }

        [FunctionName("GetSwaggerSpecFunction")]
        public HttpResponseMessage GetSwaggerSpec(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger.json")] HttpRequestMessage req,
            ILogger log)
        {
            var fileStream = File.OpenRead("swagger.json");

            return new HttpResponseMessage
            {
                Content = new StreamContent(fileStream)
            };
        }

        [FunctionName("ViewSwaggerSpecFunction")]
        public HttpResponseMessage GetSwaggerStaticFile(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/{path?}")] HttpRequestMessage req, 
            string path,
            ILogger log)
        {
            return _swaggerService.GetFileResponse(path);
        }
    }
}
