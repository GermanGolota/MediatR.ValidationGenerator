namespace MediatR.ValidationGenerator.Builders;

public class MethodParameter
{
    public MethodParameter(string type, string name)
    {
        Type = type;
        Name = name;
    }

    public MethodParameter(string type, string name, string defaultValue) : this(type, name)
    {
        DefaultValue = defaultValue;
    }

    public string Type { get; }
    public string Name { get; }
    public string? DefaultValue { get; } = null;
}
