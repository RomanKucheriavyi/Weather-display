using System;
using System.Text;

namespace Abstract.Helpful.Lib
{
    public static class ExceptionExtension
    {
        public static string ToPrettyDevelopersString(this Exception exception)
        {
            var messageBuilder = new StringBuilder();

            if (exception is AggregateException aggregateException)
            {
                foreach (var exceptionInnerException in aggregateException.InnerExceptions)
                    messageBuilder.Append(
                        $"Source:{exceptionInnerException.Source} {Environment.NewLine}" +
                        $"Message:{exceptionInnerException.Message} {Environment.NewLine}" +
                        $"Inner:{exceptionInnerException.InnerException} {Environment.NewLine}" +
                        $"StackTrace:{exceptionInnerException.StackTrace}");
            }
            else
            {
                messageBuilder.Append(
                    $"Source:{exception.Source} {Environment.NewLine}" +
                    $"Message:{exception.Message} {Environment.NewLine}" +
                    $"StackTrace:{exception.StackTrace}");
            }

            var message = $"Exception:{Environment.NewLine}{messageBuilder.ToString()}";
            return message;
        }
    }
}