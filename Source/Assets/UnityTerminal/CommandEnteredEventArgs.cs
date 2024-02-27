using System;

namespace CI.UnityTerminal
{
    public class CommandEnteredEventArgs : EventArgs
    {
        public string Command { get; set; }
    }
}