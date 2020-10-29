using System;
using System.IO;
using System.Threading;

namespace TicTacToe.Logging
{
    public class FileLoggerHelper
    {
        private string fileName;
        static ReaderWriterLock locker = new ReaderWriterLock();

        public FileLoggerHelper(string fileName)
        {
            this.fileName = fileName;
        }

        public void InsertLog(LogEntry logEntry)
        {
            var directory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            try
            {
                locker.AcquireWriterLock(int.MaxValue);

                File.AppendAllText(fileName, $"{logEntry.EventId} {logEntry.LogLevel} {logEntry.Message}" + Environment.NewLine);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
