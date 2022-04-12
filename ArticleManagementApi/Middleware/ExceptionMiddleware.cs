using System.Net;
using System.Text.Json;
using ArticleManagementApi.Exceptions;

namespace ArticleManagementApi.Middleware;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;

	public ExceptionMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await _next(httpContext);
		}
		catch (HttpResponseException ex)
		{
			await HandleHttpResponseException(httpContext, ex);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(httpContext, ex);
		}
	}

	private async Task HandleHttpResponseException(HttpContext context, HttpResponseException exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)exception.StatusCode;
		var error = new ErrorDetails(context.Response.StatusCode, exception.Message);
		await context.Response.WriteAsync(error.ToString());
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		var error = new ErrorDetails(context.Response.StatusCode, $"An internal error occured: {exception.Message}");
		await context.Response.WriteAsync(error.ToString());
	}
}

internal class ErrorDetails
{
	internal ErrorDetails(int statusCode, string message)
	{
		StatusCode = statusCode;
		Message = message;
	}

	public int StatusCode { get; }
	public string Message { get; }

	public override string ToString()
	{
		return JsonSerializer.Serialize(this);
	}
}
