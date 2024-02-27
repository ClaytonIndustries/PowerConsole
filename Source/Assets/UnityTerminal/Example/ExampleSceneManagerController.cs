using CI.UnityTerminal;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    private float _nextTimeCall;

    // Start is called before the first frame update
    void Start()
    {
        UnityTerminal.Initialise();
        UnityTerminal.IsEnabled = true;

        UnityTerminal.Log(LogLevel.Trace, "Hello World");
        UnityTerminal.Log(LogLevel.Debug, "Hello World");
        UnityTerminal.Log(LogLevel.Information, "Hello World");
        UnityTerminal.Log(LogLevel.Warning, "Hello World");
        UnityTerminal.Log(LogLevel.Error, "Hello World");
        UnityTerminal.Log(LogLevel.Critical, "Hello World");


        UnityTerminal.CommandEntered += (s, e) =>
        {
            // Dispatch to ui thread
        };

        _nextTimeCall = Time.time + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= _nextTimeCall)
        {
            //UnityTerminal.Log(LogLevel.Error, "Hello World");
            _nextTimeCall += 0.1f;
        }
    }
}