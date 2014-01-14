using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    class MacroAttribute:Attribute
    {
        public bool LazyArguments;
        public MacroAttribute(bool LazyArguments=true)
        {
            this.LazyArguments = LazyArguments;
        }
    }
    partial class Command
    {
        [Macro]
        public static string now()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        [Macro(false)]
        public static string now(string format)
        {
            return DateTime.Now.ToString(format);
        }
        [Macro]
        public static string timeFormat()
        {
            return "hh:mm";
        }
    }
}
