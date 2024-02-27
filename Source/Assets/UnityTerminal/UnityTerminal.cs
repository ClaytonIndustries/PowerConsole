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

        public static TerminalColours Colours
        {
            get => _controller.Colours;
            set => _controller.Colours = value;
        }

        public static string Title
        {
            get => _controller.Title;
            set => _controller.Title = value;
        }

        public static int MaxBufferSize
        {
            get => _controller.MaxBufferSize;
            set => _controller.MaxBufferSize = value;
        }

        public static List<KeyCode> OpenCloseHotkeys
        {
            get => _controller.OpenCloseHotkeys;
            set => _controller.OpenCloseHotkeys = value;
        }

        public static event EventHandler<CommandEnteredEventArgs> CommandEntered;

        private static TerminalController _controller;

        public static void Initialise()
        {
            if (_controller == null)
            {
                _controller = GameObject.Find("Console").GetComponent<TerminalController>();
                _controller.CommandEntered += (s, e) => CommandEntered?.Invoke(s, e);
            }
        }

        public static void Log(LogLevel logLevel, string message)
        {
            if (IsEnabled)
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
    }
}