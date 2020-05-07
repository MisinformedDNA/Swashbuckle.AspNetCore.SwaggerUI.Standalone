using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.AzureFunctions.Annotations;
using Swashbuckle.AspNetCore.AzureFunctions.Extensions;
using SwashBuckle.AspNetCore.SwaggerUI.Standalone;
using System;
using System.Net.Http;

namespace DynamicSpec
{
    public class SwaggerFunctions
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SwaggerUIFileService _swaggerService;

        public SwaggerFunctions(IServiceProvider serviceProvider
            , SwaggerUIFileService swaggerService
            )
        {
            _serviceProvider = serviceProvider;
            _swaggerService = swaggerService;
        }

        [FunctionName("GetSwaggerSpecFunction")]
        [SwaggerIgnore]
        public HttpResponseMessage GetSwaggerSpec(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger.json")] HttpRequestMessage req,
            ILogger log)
        {
            var content = _serviceProvider.GetSwagger("v1");

            return new HttpResponseMessage
            {
                Content = new StringContent(content)
            };
        }

        [FunctionName("ViewSwaggerSpecFunction")]
        [SwaggerIgnore]
        public HttpResponseMessage GetSwaggerStaticFile(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/{path?}")] HttpRequestMessage req,
            string path,
            ILogger log)
        {
            return _swaggerService.GetFileResponse(path);
        }
    }
}
