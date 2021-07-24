using MediatR.ValidationGenerator.Gen.Builders.Abstractions;
using MediatR.ValidationGenerator.Gen.Extensions;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders
{
    public class MethodBodyBuilder : IBuilder
    {
        private int _initialLeftMargin = 0;

        private List<KeyValuePair<string, int>> _lineToLength = new List<KeyValuePair<string, int>>();

        public MethodBodyBuilder()
        {

        }

        public MethodBodyBuilder(int initialLeftMargin)
        {
            _initialLeftMargin = initialLeftMargin;
        }

        public MethodBodyBuilder AppendLine(string line, int margin = 0, bool endLine = true)
        {
            if (endLine && line.NotEndsWith(";"))
            {
                line = $"{line};";
            }
            _lineToLength.Add(new KeyValuePair<string, int>(line, margin));

            return this;
        }

        public ValueOrNull<string> Build()
        {
            StringBuilder sb = new StringBuilder();
            AppendMargin(sb, _initialLeftMargin);
            sb.AppendLine("{");
            foreach (var linePair in _lineToLength)
            {
                StringBuilder lineBuilder = new StringBuilder();
                int margin = _initialLeftMargin + linePair.Value + 1;
                string line = linePair.Key;
                AppendMargin(lineBuilder, margin);
                lineBuilder.Append(line);
                sb.AppendLine(lineBuilder.ToString());
            }
            AppendMargin(sb, _initialLeftMargin);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AppendMargin(StringBuilder lineBuilder, int margin)
        {
            lineBuilder.Repeat(BuilderConstants.TAB, margin);
        }
    }
}
