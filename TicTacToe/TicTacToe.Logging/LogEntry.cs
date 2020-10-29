using System;

namespace TicTacToe.Logging
{
    public class LogEntry
    {
        public int EventId { get; internal set; }
        public string Message { get; set; }
        public string LogLevel { get; internal set; }
        public DateTime CreatedTime { get; internal set; }
    }
}
