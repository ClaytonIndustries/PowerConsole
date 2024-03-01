
namespace CI.UnityTerminal
{
    public class TerminalConfig
    {
        /// <summary>
        /// Set colours for each log level
        /// </summary>
        public TerminalColours Colours { get; set; }

        /// <summary>
        /// The number of log message the terminal will keep in memory. Setting this to a too high value could cause performance issues
        /// </summary>
        public int MaxBufferSize { get; set; }

        /// <summary>
        /// The position of the terminal
        /// </summary>
        public TerminalPosition Position { get; set; }

        /// <summary>
        /// The height of the terminal
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The width of the terminal. Null to stretch across the screen
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