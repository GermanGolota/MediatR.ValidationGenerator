using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ValidationGenerator.Gen.Tests.TestCommands
{
    public class ValueCommandHandler : IRequestHandler<ValueCommand, string>
    {
        public Task<string> Handle(ValueCommand request, CancellationToken cancellationToken)
        {
            Guid id = request.ValueId;

            var length = int.Parse(id.ToString()[0].ToString());

            string value = request.ValueId.ToString().Substring(length);
            return Task.FromResult(value);
        }
    }
}
