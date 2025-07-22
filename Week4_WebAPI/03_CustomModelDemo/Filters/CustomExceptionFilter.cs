using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _03_CustomModelDemo.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var message = $"[{DateTime.Now}] Exception: {exception.Message}{Environment.NewLine}";
            File.AppendAllText("exceptions.txt", message);

            context.Result = new ObjectResult("An internal server error occurred.")
            {
                StatusCode = 500
            };
        }
    }
}
