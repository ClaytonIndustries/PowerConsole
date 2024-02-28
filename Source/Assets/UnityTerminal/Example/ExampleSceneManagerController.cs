using System.Collections.Generic;
using CI.UnityTerminal;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    private float _nextTimeCall;

    public void Start()
    {
        UnityTerminal.Initialise(new TerminalConfig()
        {
        });
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

        UnityTerminal.RegisterCommand(new CustomCommand()
        {
            Command = "npm run start",
            Description = "Starts the dev server",
            Callback = Command1
        });
        UnityTerminal.RegisterCommand(new CustomCommand()
        {
            Command = "npm run lint",
            Description = "lints the project",
            Args = new List<CommandArgument>()
            {
                new CommandArgument() { Name = "-f", Description = "Fixes detected issues" }
            },
            Callback = Command1
        });
        UnityTerminal.RegisterCommand(new CustomCommand()
        {
            Command = "npm run build",
            Description = "Builds the project",
            Args = new List<CommandArgument>()
            {
            new CommandArgument() { Name = "-p", Description = "Port number to host on" },
            new CommandArgument() { Name = "-t", Description = "Title of the window" }
            },
            Callback = Command1
        });

        _nextTimeCall = Time.time + 0.1f;
    }

    public void Update()
    {
        if (Time.time >= _nextTimeCall)
        {
            //UnityTerminal.Log(LogLevel.Error, "Hello World");
            _nextTimeCall += 0.1f;
        }
    }

    public void Command1(CommandCallback callback)
    {
        Debug.Log(callback.Command);
        Debug.Log(callback.Args);
    }
}