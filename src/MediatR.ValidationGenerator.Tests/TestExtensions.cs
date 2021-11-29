namespace MediatR.ValidationGenerator.Tests
{
    public static class TestExtensions
    {
        public static string RemoveFirstNewLine(this string str)
        {
            return str.TrimStart('\r')
                      .TrimStart('\n');
        }
    }
}
