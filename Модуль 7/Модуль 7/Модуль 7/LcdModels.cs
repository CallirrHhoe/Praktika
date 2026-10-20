using System;
namespace EmuDisplayWpf.Models
{
    public enum LcdColorTheme
    {
        BlueWhite,
        GreenBlack,
        YellowBlack
    }
    public class LcdCommandEventArgs : EventArgs
    {
        public string CommandName { get; }
        public string Details { get; }
        public DateTime Timestamp { get; }
        public LcdCommandEventArgs(string cmd, string details)
        {
            CommandName = cmd;
            Details = details;
            Timestamp = DateTime.Now;
        }
    }
}