using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    class MacroAttribute:Attribute
    {
    }
    partial class Command
    {
        [Macro]
        public static string Date()
        {
            return DateTime.Today.ToString("yyyy-MM-dd");
        }
        [Macro]
        public static string Date(string format)
        {
            return DateTime.Today.ToString(format);
        }
    }
}
