using Pepsi.Data;
using PepsiPSK.Entities;

namespace PepsiPSK.Logger
{
    public class DatabaseLogger : IActionRecordLogger
    {
        private readonly DataContext _context;
        public DatabaseLogger(DataContext context)
        {
            _context = context;
        }
        public void LogAction(ActionRecord record)
        {
            _context.ActionRecords.Add(record);
            _context.SaveChanges();
        }
    }
}
