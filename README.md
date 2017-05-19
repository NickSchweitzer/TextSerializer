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
    public string Name;     // Properties and Fields can be Serialized

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

The fluent configuration model is new for version 2.0. It allows a POCO to be serialized, or some other class, without additional attributing just for Serialization sake. 
All of the same "common sense" defaults from Attribute based configuration are used.

```csharp
public class CsvPocoRecord
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description;      // Properties and Fields are supported
    public double Value { get; set; }
    public bool Enabled { get; set; }
}

// ...
var serializer = new CsvSerializer<CsvPocoRecord>(config => config
    .ForMember(field => field.Id, opt => opt.Position(0))
    .ForMember(field => field.Name, opt => opt.Position(1))
    .ForMember(field => field.Description, opt => opt.Position(2))
    .ForMember(field => field.Value, opt => opt.Position(3))
    .ForMember(field => field.Enabled, opt => opt.Optional().Position(4)));

IEnumerable<CsvPocoRecord> sampleCsv = null;
using (StreamReader reader = new File.OpenText("sample.csv"))
{
    sampleCsv = csvSer.DeserializeArray(reader);
    reader.Close();
}
```