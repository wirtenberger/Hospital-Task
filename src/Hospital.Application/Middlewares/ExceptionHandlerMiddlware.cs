using Hospital.Application.Configuration;
using Hospital.Core.Exceptions;

namespace Hospital.Application.Middlewares;

public class ExceptionHandlerMiddlware(ILogger<ExceptionHandlerMiddlware> logger) : IMiddleware
{
	private readonly ILogger<ExceptionHandlerMiddlware> _logger = logger;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (BaseException b)
		{
			context.Response.StatusCode = b.StatusCode;
			await context.Response.WriteAsJsonAsync(new ApiErrorResponse(b.Detail ?? "Error message was not provided"));
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Unexpected exception");
			context.Response.StatusCode = 500;
			await context.Response.WriteAsJsonAsync(new ApiErrorResponse("Server error occurred"));
		}
	}
}