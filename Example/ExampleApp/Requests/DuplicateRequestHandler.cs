using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleApp.Requests
{
    public class DuplicateRequestHandler : IRequestHandler<DuplicateRequest, string>
    {
        public Task<string> Handle(DuplicateRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.Text + request.Text);
        }
    }
}
