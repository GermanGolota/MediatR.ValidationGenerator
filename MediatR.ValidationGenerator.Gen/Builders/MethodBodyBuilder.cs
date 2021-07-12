using MediatR.ValidationGenerator.Gen.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders
{
    public class MethodBodyBuilder
    {
        private readonly int _initialLeftMargin = 0;

        private List<KeyValuePair<string, int>> _lineToLength = new List<KeyValuePair<string, int>>();

        public MethodBodyBuilder()
        {

        }

        public MethodBodyBuilder(int initialLeftMargin)
        {
            _initialLeftMargin = initialLeftMargin;
        }

        public void AppendLine(string line, int margin = 0)
        {
            if (line.NotEndsWith(";"))
            {
                line = $"{line};";
            }
            _lineToLength.Add(new KeyValuePair<string, int>(line, margin));
        }

        public string Build()
        {
            StringBuilder sb = new StringBuilder();
            AppendMargin(sb, _initialLeftMargin);
            sb.Append("{\n");
            foreach (var linePair in _lineToLength)
            {
                StringBuilder lineBuilder = new StringBuilder();
                int margin = _initialLeftMargin + linePair.Value + 1;
                string line = linePair.Key;
                AppendMargin(lineBuilder, margin);
                lineBuilder.Append(line);
                sb.AppendLine(lineBuilder.ToString());
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AppendMargin(StringBuilder lineBuilder, int margin)
        {
            for (int i = 0; i < margin; i++)
            {
                lineBuilder.Append("\t");
            }
        }
    }
}
