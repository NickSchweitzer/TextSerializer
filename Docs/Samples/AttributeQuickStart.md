# Attribute Based Configuration

Attribute based configuration is designed to be similar to the .NET XML Serializer. This requires your model class to be marked up with CSV or FixedWidth specific attributes.

```csharp
[TextSerializable]
public class MyCsvRecord
{
    [TextField(0, Name = "UniqueId")]
    public int Id { get; set; }

    [TextField(1)]
    public string Name;     // Properties and Fields can be Serialized

    [TextField(2, Optional = true)]
    public string Organization { get; set; }
}

[TextSerializable]
public class MyFixedWidthRecord
{
    [FixedWidthField(0, 5, Padding = '0')]
    public int Id { get; set; }

    [FixedWidthField(1, 20)]
    public string Name { get; set; }

    [FixedWidthField(2, 20)]
    public string Organization { get; set; }
}

// ...

var csvSer = new CsvSerializer<MyCsvRecord>();
IEnumerable<MyCsvRecord> sampleCsv = null;
using (StreamReader reader = new File.OpenText("sample.csv"))
{
    sampleCsv = csvSer.DeserializeArray(reader);
    reader.Close();
}

var fixedSer = new FixedWidthSerializer<MyFixedWidthRecord>();
IEnumerable<MyCsvRecord> sampleFixedWidth = null;
using (StreamReader reader = new File.OpenText("fixedwidth.txt"))
{
    sampleFixedWidth = fixedSer.DeserializeArray(reader);
    reader.Close();
}
```