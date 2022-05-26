using PepsiPSK.Entities;

namespace PepsiPSK.Logger
{
    public interface IActionRecordLogger
    {
        public void LogAction(ActionRecord record);
    }
}
