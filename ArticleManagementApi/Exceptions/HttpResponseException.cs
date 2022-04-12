using System.Net;

namespace ArticleManagementApi.Exceptions;

public class HttpResponseException : Exception
{
	public HttpResponseException(HttpStatusCode statusCode, string message)
	{
		StatusCode = statusCode;
		Message = message;
	}

	public HttpStatusCode StatusCode { get; }

	public override string Message { get; }
}
