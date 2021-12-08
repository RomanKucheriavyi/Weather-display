using System.Text;

namespace HomeServerApi.Logic
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Cutting out stringBuilder not implemented!!!!
        /// </summary>
        public static StringBuilder EnsureHasSameLength(this StringBuilder stringBuilder, string otherString)
        {
            if (stringBuilder.Length == otherString.Length)
                return stringBuilder;

            if (stringBuilder.Length > otherString.Length)
                return stringBuilder; // TODO: handle case when stringBuilder.Length > otherString.Length
            
            stringBuilder.Append(new string(' ', otherString.Length - stringBuilder.Length));
            return stringBuilder;
        }
    }
}