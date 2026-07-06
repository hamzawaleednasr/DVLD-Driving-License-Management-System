using System.Diagnostics;

namespace DVLD.DAL
{
    public static class EventLogger
    {
        private static readonly string _sourceName;

        static EventLogger()
        {
            Config();
            _sourceName = "DVLD";
        }

        private static void Config()
        {
            if (!EventLog.Exists(_sourceName))
            {
                EventLog.CreateEventSource(_sourceName, "Application");
            }
        }

        public static void LogInfo(string className, string methodSignature, string info)
        {
            string message = $"This Information was logged from:\n  - Class: {className}\n  - Method: {methodSignature}\nWith this message: {info}";

            EventLog.WriteEntry(_sourceName, message, EventLogEntryType.Information);
        }

        public static void LogWarning(string className, string methodSignature, string info)
        {
            string message = $"This Warning was logged from:\n  - Class: {className}\n  - Method: {methodSignature}\nWith this message: {info}";

            EventLog.WriteEntry(_sourceName, message, EventLogEntryType.Warning);
        }

        public static void LogError(string className, string methodSignature, string info)
        {
            string message = $"This Error was logged from:\n  - Class: {className}\n  - Method: {methodSignature}\nWith this message: {info}";

            EventLog.WriteEntry(_sourceName, message, EventLogEntryType.Error);
        }
    }
}
