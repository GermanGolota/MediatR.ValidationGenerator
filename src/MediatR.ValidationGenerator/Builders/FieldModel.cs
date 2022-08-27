namespace MediatR.ValidationGenerator.Builders;

public record FieldModel(
    string Name, string Type, 
    AccessModifier Modifier = AccessModifier.Private, 
    bool IsReadonly = false, bool IsStatic = false);
