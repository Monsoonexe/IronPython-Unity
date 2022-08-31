using UnityEngine;
using QFSW.QC;
using IronPython;
using IronPython.Compiler;
using IronPython.Hosting;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;
using Sirenix.OdinInspector;

public delegate void Logger(string msg);

/// <summary>
/// 
/// </summary>
public class PythonInterpreter : MonoBehaviour
{
    public static PythonInterpreter Instance;
    private ScriptEngine pyEngine;
    private ScriptScope globalScope;

    [TextArea]
    public string script = @"
import UnityEngine as U
U.Debug.Log('hello, world')";

    private void Start()
    {
        Instance = this;
        pyEngine = InitInterpreter();
        pyEngine.CreateScriptSourceFromString(
@"from UnityEngine import *

        ");
    }

    [Command, Button]
    public void RunScript()
    {
        RunScript(script);
    }

    public static void DoMethod()
    {
        Debug.Log("Hello from inside static method!");
    }

    [Command("py")]
    public static void RunScript(string script)
    {
        var pyEngine = Instance.pyEngine;
        var src = pyEngine.CreateScriptSourceFromString(script);
        src.Execute(Instance.globalScope);
    }

    [Command("pyl")]
    public static void RunScripty(params string[] script)
    {
        var pyEngine = Instance.pyEngine;
        var src = pyEngine.CreateScriptSourceFromString(string.Join(" ", script));
        src.Execute(Instance.globalScope);
    }

    private ScriptEngine InitInterpreter()
    {
        ScriptEngine pyEngine = Python.CreateEngine();

        // load assemblies
        pyEngine.Runtime.LoadAssembly(typeof(GameObject).Assembly);
        pyEngine.Runtime.LoadAssembly(typeof(PythonInterpreter).Assembly);

        globalScope = pyEngine.CreateScope();
        globalScope.SetVariable("uprint", (Logger)((s) => Debug.Log(s))); // uprint("Hello world!") -> Debug.Log("Hello World");
        return pyEngine;
    }
}
