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
      services.AddGeneratedValidators();
  }
```
Add attributes from list of <a href="#2">supported attributes </a> to request models
```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.Sample
{
    public class ValueCommand : IRequest<string>
    {
        [Required]
        [RegularExpression("[A-Z,a-z,0-9,-]")]
        public Guid ValueId { get; set; }
    }
}
```

<h1 id="2">Supported attributes</h1>
<h2>System.ComponentModel.DataAnnotations:</h2>
<h4>Required</h4>
<h4>Regex</h4>
