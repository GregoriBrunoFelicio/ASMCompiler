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

            var codes = CreateListOfCode(assemblyCode);

        }

        //TODO: refactor this method
        static List<(string, string)> CreateListOfCode(string assemblyCode)
        {
            var codeLines = CreateCodeLines(assemblyCode);
            var functions = new StringBuilder();
            var codes = new List<(string, string)>();

            foreach (var line in codeLines)
            {
                if (line.Contains(":") || !string.IsNullOrWhiteSpace(functions.ToString()))
                {
                    functions.AppendLine(line);

                    if (line.Contains("ret"))
                    {
                        codes.Add((GetFirstWord(functions.ToString()), functions.ToString()));
                        functions.Clear();
                    }
                }
                else
                {
                    codes.Add((GetFirstWord(line), line));
                }
            }

            return codes;
        }

        static string[] CreateCodeLines(string assemblyCode) => assemblyCode.Split('\n');
        static string GetFirstWord(string words) => words.Split(' ', StringSplitOptions.RemoveEmptyEntries).First();
    }
}