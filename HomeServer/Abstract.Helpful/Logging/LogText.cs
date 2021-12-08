using System;

namespace Abstract.Helpful.Lib.Logging
{
    public struct LogText
    {
        public string Text { get; }

        public LogText(string text)
        {
            Text = text;
        }

        public LogText(Exception exception)
        {
            Text = $"Exception!{Environment.NewLine}" +
                   $"{exception.ToPrettyDevelopersString()}";
        }

        public LogText(Exception exception, string comments)
        {
            Text = $"Exception!{Environment.NewLine}" +
                   $"Comments: {comments}{Environment.NewLine}" +
                   $"Exception: {exception.ToPrettyDevelopersString()}";
        }

        public static implicit operator LogText((Exception exception, string text) tuple)
        {
            return new(tuple.exception, tuple.text);
        }

        public static implicit operator LogText(string text)
        {
            return new(text);
        }
        
        public static implicit operator string(LogText text)
        {
            return text.Text;
        }
        
        public static implicit operator LogText(Exception exception)
        {
            return new(exception);
        }
                
        public static LogText From(string text)
        {
            return new(text);
        }

        public static LogText From(Exception exception)
        {
            return new(exception);
        }
        
        public static LogText From(Exception exception, string comments)
        {
            return new(exception, comments);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}