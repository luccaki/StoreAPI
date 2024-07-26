using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Functions
{
    public class ProductFunctions
    {
        private readonly AppDbContext _context;

        public ProductFunctions(AppDbContext context)
        {
            _context = context;
        }

        [Function("CreateProduct")]
        public Task<HttpResponseData> CreateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return ExceptionHandlingMiddleware.HandleExceptionAsync(req, executionContext, async () =>
            {
                var logger = executionContext.GetLogger("CreateProduct");
                logger.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Product>(requestBody);

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var response = req.CreateResponse(System.Net.HttpStatusCode.Created);
                await response.WriteAsJsonAsync(product);

                return response;
            });
        }

        [Function("GetProducts")]
        public Task<HttpResponseData> GetProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return ExceptionHandlingMiddleware.HandleExceptionAsync(req, executionContext, async () =>
            {
                var logger = executionContext.GetLogger("GetProducts");
                logger.LogInformation("C# HTTP trigger function processed a request.");

                var product = await _context.Products.ToListAsync();

                if (product == null)
                {
                    var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                    return notFoundResponse;
                }

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteAsJsonAsync(product);

                return response;
            });
        }

        [Function("GetProductById")]
        public Task<HttpResponseData> GetProductById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id}")] HttpRequestData req,
            FunctionContext executionContext, long id)
        {
            return ExceptionHandlingMiddleware.HandleExceptionAsync(req, executionContext, async () =>
            {
                var logger = executionContext.GetLogger("GetProductById");
                logger.LogInformation("C# HTTP trigger function processed a request.");

                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                    return notFoundResponse;
                }

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteAsJsonAsync(product);

                return response;
            });
        }

        [Function("UpdateProduct")]
        public Task<HttpResponseData> UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "product/{id}")] HttpRequestData req,
            FunctionContext executionContext, long id)
        {
            return ExceptionHandlingMiddleware.HandleExceptionAsync(req, executionContext, async () =>
            {
                var logger = executionContext.GetLogger("UpdateProduct");
                logger.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedProduct = JsonConvert.DeserializeObject<Product>(requestBody);

                var existingProduct = await _context.Products.FindAsync(id);

                if (existingProduct == null)
                {
                    var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                    return notFoundResponse;
                }

                existingProduct.Name = updatedProduct.Name;
                existingProduct.Value = updatedProduct.Value;
                existingProduct.StoreId = updatedProduct.StoreId;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteAsJsonAsync(existingProduct);

                return response;
            });
        }

        [Function("DeleteProduct")]
        public Task<HttpResponseData> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{id}")] HttpRequestData req,
            FunctionContext executionContext, long id)
        {

            return ExceptionHandlingMiddleware.HandleExceptionAsync(req, executionContext, async () =>
            {
                var logger = executionContext.GetLogger("DeleteProduct");
                logger.LogInformation("C# HTTP trigger function processed a request.");

                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                    return notFoundResponse;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                return response;
            });
        }
    }
}