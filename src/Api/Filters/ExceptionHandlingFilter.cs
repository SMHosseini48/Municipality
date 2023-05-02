using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;  

public class ExceptionHandlingFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = _exceptionDic.FirstOrDefault(x => x.Key == context.Exception.GetType()).Value;
        context.Result = new ObjectResult(context)
        {
            StatusCode = statusCode,
            Value = context.Exception.Message
        };
    }
    
    readonly Dictionary<Type, int> _exceptionDic = new Dictionary<Type, int>()
    {
        {typeof(UnAuthorizedException), 401},
        {typeof(AlreadyExistException), 403},
        {typeof(NotFoundException), 404},
        {typeof(InvalidArgumentException), 422}
    };
}