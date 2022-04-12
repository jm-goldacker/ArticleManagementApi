using System.Net;
using ArticleManagementApi.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArticleManagementApi.Filters;

public class ValidationFilter : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
		if (!context.ModelState.IsValid)
		{
			throw new HttpResponseException(HttpStatusCode.BadRequest, "The input is not valid");
		}
	}

	public void OnActionExecuted(ActionExecutedContext context)
	{
	}
}
