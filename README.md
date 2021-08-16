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

<h1 id="2">Supported attributes</h1>
<h2>System.ComponentModel:</h2>
<h4>Required</h4>
<h4>Regex</h4>
