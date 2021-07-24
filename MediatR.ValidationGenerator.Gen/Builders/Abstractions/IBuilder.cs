using MediatR.ValidationGenerator.Gen.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders.Abstractions
{
    public interface IBuilder
    {
        public ValueOrNull<string> Build();
    }
}
