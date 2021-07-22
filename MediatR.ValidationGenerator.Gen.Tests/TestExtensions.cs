using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.ValidationGenerator.Gen.Tests
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
