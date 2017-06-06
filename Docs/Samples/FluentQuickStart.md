# Fluent Configuration

The fluent configuration model is new for version 2.0. It allows a POCO to be serialized, or some other class, without additional attributing just for Serialization sake. 
All of the same "common sense" defaults from Attribute based configuration are used.

When using the CsvSerializer, you can start by calling the ByConvention method. By default, this makes all public properties serializable in the order they are written in the class, and if you have a header, the Header names match the names of the properties. ByConvention is not available for the FixedWidthSerializer.

```csharp
public class PocoRecord
{
    private int id;
    public int Id { get => id; set => id = value; } // Expression bodies are allowed
    public string Name { get; set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public bool Enabled { get; set; }
}

// ...
var serializer = new CsvSerializer<CsvPocoRecord>(config => config.ByConvention()));

IEnumerable<PocoRecord> sampleCsv = null;
using (StreamReader reader = new File.OpenText("sample.csv"))
{
    sampleCsv = csvSer.DeserializeArray(reader);
    reader.Close();
}
```

You can override any of the default settings using the ForMember function, or mark a field as not serialized by using the Ignore method.

```csharp
var serializer = new CsvSerializer<CsvPocoRecord>(config => config.ByConvention()
	.ForMember(field => field.Id, opt => opt.Name("UniqueId"))
	.ForMember(field => field.Enabled, opt => opt.Optional())
	.Ignore(field => field.Description));
```