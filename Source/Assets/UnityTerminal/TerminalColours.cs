using UnityEngine;

namespace CI.UnityTerminal
{
    public class TerminalColours
    {
        public Color TraceColour { get; set; } = Color.white;
        public Color DebugColor { get; set; } = Color.white;
        public Color InformationColor { get; set; } = Color.white;
        public Color WarningColor { get; set; } = Color.yellow;
        public Color ErrorColor { get; set; } = Color.red;
        public Color CriticalColor { get; set; } = Color.magenta;
        public Color NoneColour { get; set; } = Color.white;
    }
}