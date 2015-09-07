using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace Latexish_Text_Processor.MacroProviders
{
    public class HumanizerProvider:CommandProvider
    {
        [Macro]
        public string Upper(string sentence)
        {
            return sentence.Transform(To.UpperCase);
        }
        [Macro]
        public string Lower(string sentence)
        {
            return sentence.Transform(To.LowerCase);
        }
        [Macro]
        public string TitleCase(string sentence)
        {
            return sentence.Transform(To.TitleCase);
        }
        [Macro]
        public string SentenceCase(string sentence)
        {
            return sentence.Transform(To.SentenceCase);
        }
        [Macro]
        public string Pluralize(string word)
        {
            return word.Pluralize(false);
        }
        [Macro]
        public string Singularize(string word)
        {
            return word.Singularize(false);
        }
        [Macro]
        public string Humanize(string sentence)
        {
            return sentence.Humanize();
        }
        [Macro]
        public string Dehumanize(string sentence)
        {
            return sentence.Dehumanize();
        }
        [Macro]
        public string ToQuantity(string sentence, int quantity, string type="Numeric")
        {
            return sentence.ToQuantity(quantity, type.DehumanizeTo<ShowQuantityAs>());
        }
        [Macro]
        public string Truncate(string sentence, int length, string truncationString = null)
        {
            if (truncationString == null)
                return sentence.Truncate(length);
            return sentence.Truncate(length, truncationString);
        }
    }
}
