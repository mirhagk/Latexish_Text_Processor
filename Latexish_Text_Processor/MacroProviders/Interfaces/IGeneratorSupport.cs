using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProviders.Interfaces
{
    interface IGeneratorSupport
    {
        string GenerateCode(GeneratorLanguage Language, string CommandName, params string[] Parameters);
    }
    public enum GeneratorLanguage
    {
        CSharp
    }
    static class IGeneratorSupportExtensions
    {

    }
}
