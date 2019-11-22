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
                               mov  a, 5
                               inc  a
                               call function
                               msg  '(5+1)/2 = ', a    ; output message
                               end

                               function:
                               div  a, 2
                               ret";

            var codes = CreateCode(assemblyCode);

            Console.ReadKey();
        }


        static Dictionary<string, List<string>> CreateCode(string assemblyCode)
        {
            var codes = new Dictionary<string, List<string>>();

            foreach (var code in CreateCodeLines(assemblyCode))
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    var arguments = new List<string>();

                    foreach (var code2 in SplitInComma(RemoveFirstWord(code)))
                    {
                        arguments.Add(code2.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Trim());
                    }

                    //TODO: verify if is function

                    codes.Add(GetFirstWord(code), arguments);
                }
            }

            return codes;
        }

        static string[] CreateCodeLines(string assemblyCode) => assemblyCode.Split('\n');
        static string GetFirstWord(string words) => words.Split(' ', StringSplitOptions.RemoveEmptyEntries).First();
        static string RemoveFirstWord(string words) => words.Replace(GetFirstWord(words), string.Empty);
        static string[] SplitInComma(string words) => words.Split(',');
    }
}