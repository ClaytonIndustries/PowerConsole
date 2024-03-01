using System;
using System.Collections.Generic;

namespace CI.UnityTerminal
{
    public class CustomCommand
    {
        /// <summary>
        /// Arguments that can be added after the command beginning with - or --. Shown by the help command
        /// </summary>
        public List<CommandArgument> Args { get; set; }

        /// <summary>
        /// The command
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Description of the command shown by the help
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Callback raised when this command in entered
        /// </summary>
        public Action<CommandCallback> Callback { get; set; }
    }
}