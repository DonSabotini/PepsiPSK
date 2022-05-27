using Pepsi.Data;
using PepsiPSK.Entities;

namespace PepsiPSK.Logger
{
    public class DatabaseAndConsoleLogger : IActionRecordLogger
    {
        private readonly DatabaseLogger _logger;
        public DatabaseAndConsoleLogger(DatabaseLogger logger)
        {
            _logger = logger;
        }
        public void LogAction(ActionRecord record)
        {
            Console.WriteLine($"Decorated {record.UsedMethod} {record.UserId} {record.Role} {record.UserName} {record.Time} ");
            _logger.LogAction(record);
        }
    }
}
