using Message.Integration.Common;
using Message.Integration.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Message.Integration.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errorResponse = new ErrorResponse();
            var exception = context.Exception as MessageException;

            if (exception != null)
            {
                errorResponse.Code = exception.Code;
                errorResponse.Message = exception.Message;
            }
            else
            {
                string message = context.Exception.InnerException == null
                    ? context.Exception.Message : context.Exception.InnerException.Message;

                errorResponse.Code = Constants.CODE_UNKONWN_ERROR;
                errorResponse.Message = message;
            }

            context.HttpContext.Response.Headers[Constants.STATUS] = "Failed";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Result = new JsonResult(errorResponse);
        }
    }
}
