using System;
using System.Collections.Generic;
using System.Linq;
using CI.UnityTerminal.Core;
using UnityEngine;

namespace CI.UnityTerminal
{
    public static class UnityTerminal
    {
        /// <summary>
        /// Should the terminal collect logs
        /// </summary>
        public static bool IsEnabled
        {
            get => _controller.IsEnabled;
            set => _controller.IsEnabled = value;
        }

        /// <summary>
        /// Is the terminal visible to the user
        /// </summary>
        public static bool IsVisible
        {
            get => _controller.IsVisible;
            set => _controller.IsVisible = value;
        }

        /// <summary>
        /// Sets the minimum log level. Logs below this level won't be collected
        /// </summary>
        public static LogLevel LogLevel
        {
            get => _controller.LogLevel;
            set => _controller.LogLevel = value;
        }

        /// <summary>
        /// Sets the title of the terminal
        /// </summary>
        public static string Title
        {
            get => _controller.Title;
            set => _controller.Title = value;
        }

        /// <summary>
        /// The key or keys that need to be pressed to open or close the console
        /// </summary>
        public static List<KeyCode> OpenCloseHotkeys
        {
            get => _controller.OpenCloseHotkeys;
            set => _controller.OpenCloseHotkeys = value;
        }

        /// <summary>
        /// Raised when the user enters a command. Call this once before attempting to interact with the terminal
        /// </summary>
        public static event EventHandler<CommandEnteredEventArgs> CommandEntered;

        private static TerminalController _controller;

        /// <summary>
        /// Initialises the terminal. Call this once before attempting to interact with the terminal
        /// </summary>
        public static void Initialise() => Initialise(new TerminalConfig());

        /// <summary>
        /// Initialises the terminal with the specified configuration
        /// </summary>
        /// <param name="config">The configuration</param>
        public static void Initialise(TerminalConfig config)
        {
            if (_controller == null)
            {
                _controller = UnityEngine.Object.FindObjectsByType<TerminalController>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();
                _controller.gameObject.SetActive(true);
                _controller.CommandEntered += (s, e) => CommandEntered?.Invoke(s, e);

                _controller.Initialise(config);
            }
        }

        /// <summary>
        /// Logs a message to the terminal
        /// </summary>
        /// <param name="logLevel">The level to log this message at</param>
        /// <param name="message">The message to log</param>
        public static void Log(LogLevel logLevel, string message)
        {
            if (IsEnabled && logLevel >= LogLevel)
            {
                _controller.Log(logLevel, message, false);
            }
        }

        /// <summary>
        /// Clears all text from the terminal
        /// </summary>
        public static void Clear()
        {
            if (IsEnabled)
            {
                _controller.ClearDisplay();
            }
        }

        /// <summary>
        /// Registers a command. If the command already exists it will be overwritten
        /// </summary>
        /// <param name="command">The command to add</param>
        public static void RegisterCommand(CustomCommand command) => _controller.RegisterCommand(command);

        /// <summary>
        /// Unregisters the specified command if it exists
        /// </summary>
        /// <param name="command">The command to remove</param>
        public static void UnregisterCommand(string command) => _controller.UnregisterCommand(command); 
    }
}