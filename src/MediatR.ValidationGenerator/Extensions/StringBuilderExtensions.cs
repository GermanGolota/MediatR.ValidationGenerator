using System.Text;

namespace MediatR.ValidationGenerator.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Repeat(this StringBuilder sb, string value, int times, bool separateNewLine = false)
        {
            for (int i = 0; i < times; i++)
            {
                if (separateNewLine)
                {
                    sb.AppendLine(value);
                }
                else
                {
                    sb.Append(value);
                }
            }
            return sb;
        }
    }
}
