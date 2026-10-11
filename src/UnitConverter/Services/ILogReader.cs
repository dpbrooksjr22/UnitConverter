using UnitConverter.Models;

namespace UnitConverter.Models;

public interface ILogReader
{
    IEnumerable<LogEntry> Read();
}
