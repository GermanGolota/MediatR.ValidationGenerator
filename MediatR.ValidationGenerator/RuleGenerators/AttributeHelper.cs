namespace MediatR.ValidationGenerator.RuleGenerators
{
    public static class AttributeHelper
    {
        public static string GetProperName(string name)
        {
            string attributeStr = "Attribute";
            if (name.EndsWith(attributeStr))
            {
                name = name.Substring(0, name.Length - attributeStr.Length);
            }
            return name;
        }
    }
}
