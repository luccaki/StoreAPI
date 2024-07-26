using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace StoreAPI.Functions
{
    public static class ExceptionHandlingMiddleware
    {
        public static async Task<HttpResponseData> HandleExceptionAsync(
            HttpRequestData req,
            FunctionContext executionContext,
            Func<Task<HttpResponseData>> func)
        {
            var logger = executionContext.GetLogger("ExceptionHandlingMiddleware");
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred.");
                var response = req.CreateResponse(ex switch
                {
                    ArgumentException => HttpStatusCode.BadRequest,
                    KeyNotFoundException => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.InternalServerError,
                });
                await response.WriteAsJsonAsync(new { error = ex.Message });
                return response;
            }
        }
    }

}
