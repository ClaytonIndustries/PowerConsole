using System;
using System.Collections.Generic;

namespace CI.UnityTerminal
{
    public class CustomCommand
    {
        public List<CommandArgument> Args { get; set; }
        public string Command { get; set; }
        public string Description { get; set; }
        public Action<CommandCallback> Callback { get; set; }
    }
}