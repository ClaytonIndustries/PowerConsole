
namespace CI.UnityTerminal
{
    public class TerminalConfig
    {
        public TerminalColours Colours { get; set; }
        public int MaxBufferSize { get; set; }

        public TerminalConfig()
        {
            Colours = new TerminalColours();
            MaxBufferSize = 150;
        }
    }
}