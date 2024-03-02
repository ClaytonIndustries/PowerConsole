
namespace CI.UnityTerminal
{
    public class TerminalConfig
    {
        /// <summary>
        /// Set colours for each log level
        /// </summary>
        public TerminalColours Colours { get; set; }

        /// <summary>
        /// The number of log messages the terminal will keep in memory. The default is 100 - setting this too high could affect performance
        /// </summary>
        public int MaxBufferSize { get; set; }

        /// <summary>
        /// The position of the terminal
        /// </summary>
        public TerminalPosition Position { get; set; }

        /// <summary>
        /// The height of the terminal. Ignored if the Postion is Fullscreen
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The width of the terminal. Null to stretch across the screen, ignored if the Postion is Fullscreen
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Can the terminal be moved
        /// </summary>
        public bool IsFixed { get; set; }

        public TerminalConfig()
        {
            Colours = new TerminalColours();
            MaxBufferSize = 100;
            Position = TerminalPosition.Bottom;
            Height = 400;
            Width = null;
            IsFixed = false;
        }
    }
}