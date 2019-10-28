using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace CompilerASM
{
    class Program
    {
        static void Main(string[] args)
        {
            var typeSignature = "Emitter";
            var assemblyName = new AssemblyName(typeSignature);

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run
            );


            var moduleBuilder = assemblyBuilder.DefineDynamicModule("EmitterModule");
            var typeBuilder = moduleBuilder.DefineType(typeSignature,
                TypeAttributes.Class |
                TypeAttributes.Public |
                TypeAttributes.AnsiClass |
                TypeAttributes.AutoClass |
                TypeAttributes.BeforeFieldInit);

            var methodBuilder = typeBuilder.DefineMethod("RunAsm", MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.Standard);
            var ilGenerator = methodBuilder.GetILGenerator();

            var local = ilGenerator.DeclareLocal(typeof(int));
            var localString = ilGenerator.DeclareLocal(typeof(string));

            ilGenerator.Emit(OpCodes.Ldc_I4_4);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldc_I4, 1000);
            ilGenerator.Emit(OpCodes.Add);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.EmitWriteLine(local);

            ilGenerator.Emit(OpCodes.Ldstr, "xablau");
            ilGenerator.Emit(OpCodes.Stloc_1);

            var concatLixo = Type.GetType("System.String").GetMethod("Concat", 0, new[] { typeof(object), typeof(object) });
            ilGenerator.Emit(OpCodes.Ldstr, "a");
            ilGenerator.Emit(OpCodes.Ldstr, "b");
            ilGenerator.EmitCall(OpCodes.Call, concatLixo, null);
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.EmitWriteLine(localString);

            ilGenerator.Emit(OpCodes.Ret);

            var tipoGerado = typeBuilder.CreateType();

            var teste = Activator.CreateInstance(tipoGerado);
            var metodos = teste.GetType().GetMethod("RunAsm");
            metodos.Invoke(teste, new object[0]);

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