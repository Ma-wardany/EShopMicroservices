namespace BuildingBlocks.Excptions.Handler
{
    public class CustomExcptionHandler (ILogger<CustomExcptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError($"Error Message: {exception.Message}, time of occurrence {DateTime.UtcNow}");

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException =>
                (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError),

                NotFoundException =>
                (exception.Message, exception.GetType().Name, StatusCodes.Status404NotFound),

                BadHttpRequestException =>
                (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),

                ValidationException =>
                (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),

                _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Status = details.StatusCode,
                Detail = details.Detail,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is ValidationException validationException)
                problemDetails.Extensions.Add("errors", validationException.Errors);

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
