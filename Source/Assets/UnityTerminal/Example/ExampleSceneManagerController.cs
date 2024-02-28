using System.Collections.Generic;
using CI.UnityTerminal;
using CI.UnityTerminal.Core;
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

            Debug.Log(e.Command);
        };

        UnityTerminal.RegisterCommand("npm run start", "starts the dev server", Command1, new List<CommandArgument>());
        UnityTerminal.RegisterCommand("npm run lint", "lints the project", Command1, new List<CommandArgument>()
        {
            new CommandArgument() { Name = "-f", Description = "Fixed detected issues" }
        });
        UnityTerminal.RegisterCommand("npm run build", "builds the project", Command1, new List<CommandArgument>()
        {
            new CommandArgument() { Name = "-p", Description = "port number to host on" },
            new CommandArgument() { Name = "-t", Description = "title of the window" }
        });

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

    void Command1(CommandCallback callback)
    {
        Debug.Log(callback.Command);
        Debug.Log(callback.Args);
    }
}