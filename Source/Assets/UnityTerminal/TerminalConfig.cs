﻿
namespace CI.UnityTerminal
{
    public class TerminalConfig
    {
        public TerminalColours Colours { get; set; }
        public int MaxBufferSize { get; set; }
        public TerminalPosition Position { get; set; }
        public int Height { get; set; }
        public int? Width { get; set; }
        public bool IsFixed { get; set; }

        public TerminalConfig()
        {
            Colours = new TerminalColours();
            MaxBufferSize = 150;
            Position = TerminalPosition.Bottom;
            Height = 400;
            Width = null;
            IsFixed = false;
        }
    }
}