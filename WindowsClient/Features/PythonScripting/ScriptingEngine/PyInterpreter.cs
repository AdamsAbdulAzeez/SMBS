using System.IO;
using Python.Runtime;
using System.Reflection;

namespace PyScripting.ScriptingEngine
{
    public class PyInterpreter
    {
        private PyStream outStream;
        PyModule ns;
        public PyInterpreter(PyStream outStream)
        {
            Runtime.PythonDLL = @"C:\Users\ehocaha\AppData\Local\Programs\Python\Python310\python310.dll";
            this.outStream = outStream;

            // TODO: VISIT: Make proprietary scripts embedded resources.
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WindowsClient.Features.PythonScripting.ScriptingEngine.SmbsApi.startup.py";
            PythonEngine.Initialize();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string initScript = reader.ReadToEnd();
                using (Py.GIL())
                {
                    PyObject initcode = PythonEngine.Compile(initScript);
                    PyModule smbs = new PyModule("smbs");
                    smbs.Execute(initcode);
                    PyModule.SysModules.SetItem("smbs", smbs);

                    ns = Py.CreateScope("ns");
                    //dynamic sys = Py.Import("sys");
                }
            }

        }

        public void RunSource(string src)
        {
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.stdout = outStream;
                ns.Set("sys", sys);

                PyObject code = PythonEngine.Compile(src);
                ns.Execute(code);
            }
        }
    }
}
