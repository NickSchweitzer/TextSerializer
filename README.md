# Text Serializer Library
[![Build status](https://ci.appveyor.com/api/projects/status/sp5aeexncrl2083d/branch/master?svg=true)](https://ci.appveyor.com/project/NickSchweitzer/textserializer/branch/master)

Library for reading and writing CSV and Fixed Width files into a class or struct similar to XmlSerializer.

# Attribute Based Configuration

```csharp
[TextSerializable]
public class MyCsvRecord
{
    [TextField(0)]
    public int Id { get; set; }

    [TextField(1)]
    public string Name { get; set; }

    [TextField(2)]
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

# Fluent Configuration