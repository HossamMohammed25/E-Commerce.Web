using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.Web.CustomMiddlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(RequestDelegate next , ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
           _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                await NotFoundEndPoint(context);
            }
            catch (Exception ex)
            {
                // log error 
                _logger.LogError(ex, "Something Went Error");
                //status code for response 
                await HandleExceptionAsync(context, ex);

            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var Response = new ErrorToReturn()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            };
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException=>StatusCodes.Status401Unauthorized,
                BadRequestException badRequestException => GetBadRequestErrors(badRequestException,Response) ,
                _ => StatusCodes.Status500InternalServerError
            };
            // content type
            //context.Response.ContentType = "application/json";
            // response object 
           
            await context.Response.WriteAsJsonAsync(Response);
        }

        private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.ErrorMessage = badRequestException.Message;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task NotFoundEndPoint(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {context.Request.Path} is Not Found"
                };
                await context.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
