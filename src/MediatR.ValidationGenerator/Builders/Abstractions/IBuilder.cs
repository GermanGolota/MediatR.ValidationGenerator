using MediatR.ValidationGenerator.Models;

namespace MediatR.ValidationGenerator.Builders.Abstractions
{
    public interface IBuilder
    {
        public ValueOrNull<string> Build();
    }
}
