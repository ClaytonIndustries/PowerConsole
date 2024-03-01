using System.Collections.Generic;
using CI.UnityTerminal;
using TMPro;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    public TMP_InputField LogMessageInputField;

    private float _nextTimeCall;

    public void Start()
    {
        // Initialise the terminal - make sure this is called once before trying to interact with it
        UnityTerminal.Initialise();

        // Enable the terminal - it is disabled by default
        UnityTerminal.IsEnabled = true;

        // Log some messages to the terminal at different severity levels
        UnityTerminal.Log(LogLevel.Trace, "Hello World");
        UnityTerminal.Log(LogLevel.Debug, "Hello World");
        UnityTerminal.Log(LogLevel.Information, "Hello World");
        UnityTerminal.Log(LogLevel.Warning, "Hello World");
        UnityTerminal.Log(LogLevel.Error, "Hello World");
        UnityTerminal.Log(LogLevel.Critical, "Hello World");

        // Listen for any user entered command
        UnityTerminal.CommandEntered += (s, e) =>
        {
            var enteredCommand = e.Command;
        };

        // Register a command with a descripton and two arguments
        UnityTerminal.RegisterCommand(new CustomCommand()
        {
            Command = "npm run build",
            Description = "Builds the project",
            Args = new List<CommandArgument>()
            {
                new CommandArgument() { Name = "-p", Description = "Port number to host on" },
                new CommandArgument() { Name = "-t", Description = "Title of the window" }
            },
            Callback = Command1Callback
        });
    }

    public void Update()
    {
        if (Time.time >= _nextTimeCall)
        {
            //UnityTerminal.Log(LogLevel.Error, "Hello World");
            _nextTimeCall += 0.1f;
        }
    }

    public void WriteLogMessage()
    {
        UnityTerminal.Log(LogLevel.Trace, LogMessageInputField.text);
    }

    private void Command1Callback(CommandCallback callback)
    {
        // Raised when "npm run" build is entered into the terminal
        // Arguments are parsed and available in callback.Args
    }
}