# MediatR.ValidationGenerator
An add-on for MediatR library, that creates validators for request models based on property attributes

# Table of content:
<a href="#1">1. Usage</a> </br>
<a href="#2">2. Supported attributes</a></br>

<h1 id="1">Usage</h1>
<h2>ASP.NET Core</h2> </br>
Add validators to DI container with AddGeneratedValidators </br> </br>

```csharp
  public void ConfigureServices(IServiceCollection services)
  {
    var assemblies = new[] { typeof(Startup).Assembly };
    services.AddMediatR(assemblies);
    services.AddGeneratedValidators();
  }
```
Add attributes from list of <a href="#2">supported attributes </a> to request models
```csharp
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Requests
{
    public class DuplicateRequest : IRequest<string>
    {
        [Required]
        [RegularExpression("[A-Z,a-z,0-9,-]")]
        public string Text { get; set; }
    }
}
```

<h1 id="2">Supported attributes</h1>
<h2>System.ComponentModel.DataAnnotations:</h2>
<h3>Required</h3>
<h3>Regex</h3>
<h2>MediatR.ValidationGenerator</h2>
<h3>CustomValidatorAttribute</h3>
Receives the type of validator and the name of validation method
Would get the validator type from DI container
