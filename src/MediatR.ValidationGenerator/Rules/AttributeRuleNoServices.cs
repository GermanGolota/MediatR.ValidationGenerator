using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.Rules
{
    public abstract class AttributeRuleNoServices : AttributeRule
    {
        public override IEnumerable<ITypeSymbol> GetRequiredServices(AttributeData attribute)
        {
            return Enumerable.Empty<ITypeSymbol>();
        }
    }
}
