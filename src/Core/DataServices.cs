using System.Collections.Generic;

public interface IDataProvider
{
    IEnumerable<DataRecord> GetRecords();
}

public record DataRecord(string Id, string Name, decimal Amount, bool IsValid);
