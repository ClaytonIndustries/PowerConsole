using System.Collections.Generic;

namespace CI.UnityTerminal
{
    public class CommandCallback
    {
        public string Command { get; set; }
        public Dictionary<string, string> Args { get; set; }
    }
}