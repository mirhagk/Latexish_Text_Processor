﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    partial class Command
    {
        public class Macro
        {
            public string Name { get; set; }
            public bool LazyParse { get; set; }
            public int NumParameters { get; set; }
            public Func<string[], string> Execute;
        }
        static Command()
        {
            AddSystemMacros();
        }
        private static Func<string[], string> CreateWrapper(MethodInfo method)
        {
            return (arguments)=>
                {
                    return method.Invoke(null, arguments).ToString();
                };
            //method.GetParameters().
        }
        private static void AddSystemMacros()
        {
            var methods = typeof(Command).GetMethods().Where((x) => x.CustomAttributes.Any((y) => y.AttributeType == typeof(MacroAttribute)));
            macros.AddRange(methods.Select((x) => new Macro { Name = x.Name, LazyParse = (x.GetCustomAttribute(typeof(MacroAttribute)) as MacroAttribute).LazyArguments, NumParameters = x.GetParameters().Length, Execute = CreateWrapper(x) }));
        }
        public static List<Macro> macros = new List<Macro>();
        public static string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            //find macro that matches the number of arguments
            var macro = macros.FirstOrDefault((x) => x.Name == CommandName && x.NumParameters == Parameters.Length);
            if (macro == null)//do some stuff to find it among included command engines
                throw new InvalidOperationException("Macro " + CommandName + " not found");
            if (!macro.LazyParse)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i] = Parser.Process(Parameters[i]);
                }
            }
            return macro.Execute(Parameters); ;
            //Parser.Tokenizer()
        }
    }
}
