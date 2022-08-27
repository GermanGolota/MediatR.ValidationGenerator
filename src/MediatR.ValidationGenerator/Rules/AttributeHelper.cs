namespace MediatR.ValidationGenerator.Rules;

public static class AttributeHelper
{
    public static bool IsTheSameAttribute(string attributeName, string attributeName2)
    {
        return RemoveEndAttribute(attributeName) == RemoveEndAttribute(attributeName2);
    }

    private static string RemoveEndAttribute(string name)
    {
        string attributeStr = "Attribute";
        if (name.EndsWith(attributeStr))
        {
            name = name.Substring(0, name.Length - attributeStr.Length);
        }
        return name;
    }
}
