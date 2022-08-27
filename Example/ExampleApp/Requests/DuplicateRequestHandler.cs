using MediatR;

namespace ExampleApp.Requests;

public class DuplicateRequestHandler : IRequestHandler<DuplicateRequest, string>
{
    public Task<string> Handle(DuplicateRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Text + request.Text);
    }
}
