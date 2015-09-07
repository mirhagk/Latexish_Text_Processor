using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor.MacroProviders
{
    class LanguageProvider : CommandProvider
    {
        public LanguageProvider(ParserEngine.Parser parser) : base(parser) { }
        static PluralizationService pluralizer =
            PluralizationService.CreateService(new CultureInfo("en-US"));
        [Macro(false)]
        public string Upper(string word)
        {
            return word.ToUpperInvariant();
        }
        [Macro(false)]
        public string Lower(string word)
        {
            return word.ToLowerInvariant();
        }
        [Macro(false)]
        public string Plural(string word)
        {
            return pluralizer.Pluralize(word);
        }
        [Macro(false)]
        public string Singular(string word)
        {
            return pluralizer.Singularize(word);
        }
    }
}
