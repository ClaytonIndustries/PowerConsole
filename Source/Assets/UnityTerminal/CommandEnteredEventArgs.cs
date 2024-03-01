using System;

namespace CI.UnityTerminal
{
    public class CommandEnteredEventArgs : EventArgs
    {
        /// <summary>
        /// The command that was entered
        /// </summary>
        public string Command { get; set; }
    }
}