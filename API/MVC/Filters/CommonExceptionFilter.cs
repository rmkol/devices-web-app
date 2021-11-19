using API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.MVC.Filters
{
	public class CommonExceptionFilter : IActionFilter, IOrderedFilter
	{
		public int Order => int.MaxValue - 10;

		public void OnActionExecuting(ActionExecutingContext context) { }

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception is InvalidArgumentException iaEx)
			{
				context.Result = new BadRequestObjectResult(iaEx.ErrorMessage);
				context.ExceptionHandled = true;
				return;
			}

			if (context.Exception is EntityNotFoundException enfEx)
			{
				context.Result = new NotFoundObjectResult(enfEx.ErrorMessage);
				context.ExceptionHandled = true;
				return;
			}
		}
	}
}