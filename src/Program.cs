using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerASM
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyCode = @"
                               ; My first program
                               mov  a, 5
                               inc  a
                               call function
                               msg  '(5+1)/2 = ', a    ; output message
                               end

                               function:
                               div  a, 2
                               ret";

            var codes = CreateListOfCode2(assemblyCode);
        }

        static IDictionary<string, List<string>> CreateListOfCode2(string assemblyCode)
        {
            var codeLines = CreateCodeLines(assemblyCode);
            var codes = new Dictionary<string, List<string>>();
            var main = new List<string>();
            var functions = new List<string>();

            foreach (var codeLine in codeLines)
            {
                if (codeLine.Contains(":") || functions.Any())
                {
                    /// TODO
                    functions.Add(codeLine);

                    if (codeLine.Contains("ret"))
                    {
                        codes.Add("functions", functions);
                        functions.Clear();
                    }
                }
                else
                {
                    main.Add(codeLine);
                }

            }

            codes.Add("main", main);

            return codes;
        }
        
        static string[] CreateCodeLines(string assemblyCode) => assemblyCode.Split('\n');
        static string GetFirstWord(string words) => words.Split(' ', StringSplitOptions.RemoveEmptyEntries).First();
    }
}