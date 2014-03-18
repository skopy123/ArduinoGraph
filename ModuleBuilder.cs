using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using ArduinoGraphModuleInterface;

namespace ArduinoGraph {
    class ModuleBuilder {
        CSharpCodeProvider csc;
        CompilerResults results;
        
        public void BuildModule(string filename) {
            string dllname = filename.Replace(".cs", ".dll");
            csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            CompilerParameters parameters = new CompilerParameters(new[] { 
                "mscorlib.dll", 
                "System.dll", 
                "System.Core.dll", 
                "System.Drawing.dll", 
                "System.Windows.Forms.dll", 
                "System.Windows.Forms.DataVisualization.dll", 
                "ArduinoGraphModuleInterface.dll" });
            parameters.OutputAssembly = dllname;
            filename = ".\\modules\\" + filename;
            results = csc.CompileAssemblyFromFile(parameters, filename);

           // results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
        }

        public IArduinoGraphModule GetModuleObjectInstance(string filename) {
            BuildModule(filename);
            if (!results.Errors.HasErrors) {
                foreach (Type t in results.CompiledAssembly.GetTypes()) {
                    if (typeof(IArduinoGraphModule).IsAssignableFrom(t)) {
                        return (IArduinoGraphModule)Activator.CreateInstance(t);
                    }
                }
            }
            return null;
        }
    }
}
