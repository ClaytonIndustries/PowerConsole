using System;
using System.Collections.Generic;
using CI.UnityTerminal.Core;
using UnityEngine;

namespace CI.UnityTerminal
{
    public static class UnityTerminal
    {
        public static bool IsEnabled
        {
            get => _controller.IsEnabled;
            set => _controller.IsEnabled = value;
        }

        public static bool IsVisible
        {
            get => _controller.IsVisible;
            set => _controller.IsVisible = value;
        }

        public static LogLevel LogLevel
        {
            get => _controller.LogLevel;
            set => _controller.LogLevel = value;
        }

        public static string Title
        {
            get => _controller.Title;
            set => _controller.Title = value;
        }

        public static List<KeyCode> OpenCloseHotkeys
        {
            get => _controller.OpenCloseHotkeys;
            set => _controller.OpenCloseHotkeys = value;
        }

        public static event EventHandler<CommandEnteredEventArgs> CommandEntered;

        private static TerminalController _controller;

        public static void Initialise() => Initialise(new TerminalConfig());

        public static void Initialise(TerminalConfig config)
        {
            if (_controller == null)
            {
                _controller = GameObject.Find("Console").GetComponent<TerminalController>();
                _controller.CommandEntered += (s, e) => CommandEntered?.Invoke(s, e);
            }

            _controller.Initialise(config);
        }

        public static void Log(LogLevel logLevel, string message)
        {
            if (IsEnabled && logLevel >= LogLevel)
            {
                _controller.Log(logLevel, message, false);
            }
        }

        public static void Clear()
        {
            if (IsEnabled)
            {
                _controller.ClearDisplay();
            }
        }

        public static void RegisterCommand(CustomCommand command) => _controller.RegisterCommand(command);

        public static void UnregisterCommand(string command) => _controller.UnregisterCommand(command);
    }
}