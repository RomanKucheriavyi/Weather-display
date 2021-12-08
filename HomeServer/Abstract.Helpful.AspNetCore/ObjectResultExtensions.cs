using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Abstract.Helpful.AspNetCore
{
    public static class ObjectResultExtensions
    {
        public static ObjectResult ToObjectResult<T>(this T value)
        {
            return new ObjectResult(value);
        }
        
        public static int ToInt(this HttpStatusCode code)
        {
            return (int) code;
        }
    }
}