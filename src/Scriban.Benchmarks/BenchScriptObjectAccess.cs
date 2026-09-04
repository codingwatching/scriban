using BenchmarkDotNet.Attributes;
using Scriban.Runtime;

namespace Scriban.Benchmarks;

/// <summary>
/// Measures contextless helpers when built-in objects are referenced through IScriptObject.
/// </summary>
[MemoryDiagnoser]
public class BenchScriptObjectAccess
{
    private IScriptObject _scriptObject;

    [Params("Object", "Array", "TypedArray")]
    public string Kind { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _scriptObject = Kind switch
        {
            "Array" => new ScriptArray(),
            "TypedArray" => new ScriptArray<int>(),
            _ => new ScriptObject()
        };
        _scriptObject.SetValue("value", "expected", false);
    }

    [Benchmark]
    public object GetValue()
    {
        _scriptObject.TryGetValue("value", out var value);
        return value;
    }

    [Benchmark]
    public void SetValue()
    {
        _scriptObject.SetValue("value", "expected", false);
    }
}
