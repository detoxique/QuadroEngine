using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace QuadroEngine
{
    public class Script
    {
        public string Source, Path, NamespaceAndClass;

        private bool Loaded = false;
        private CompilerResults comp;

        public void Load()
        {
            try
            {
                StreamReader sr = new StreamReader(Path);
                Source = sr.ReadToEnd();
                comp = Compile();
                Loaded = true;
            }
            catch
            {
                
            }
        }

        public void Run()
        {
            if (comp != null && comp.Errors.Capacity == 0)
            {
                object o = comp.CompiledAssembly.CreateInstance("NamespaceAndClass");
                MethodInfo mi = o.GetType().GetMethod("Run");
                mi.Invoke(o, null);
            }
        }

        public void Update()
        {
            if (comp != null && comp.Errors.Capacity == 0)
            {
                object o = comp.CompiledAssembly.CreateInstance("NamespaceAndClass");
                MethodInfo mi = o.GetType().GetMethod("Update");
                mi.Invoke(o, null);
            }
        }

        public CompilerResults Compile()
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v3.5"}
                };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            // References
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("sfml-window.dll");
            compilerParams.ReferencedAssemblies.Add("sfml-audio.dll");
            compilerParams.ReferencedAssemblies.Add("sfml-graphics.dll");
            compilerParams.ReferencedAssemblies.Add("sfml-system.dll");

            if (Loaded)
                return provider.CompileAssemblyFromSource(compilerParams, Source);
            else
                return null;
        }
    }
}
