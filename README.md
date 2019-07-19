# Text Serializer Library
![AppVeyor](https://img.shields.io/appveyor/ci/NickSchweitzer/TextSerializer.svg?logo=appveyor&style=for-the-badge)
![AppVeyor tests](https://img.shields.io/appveyor/tests/NickSchweitzer/TextSerializer.svg?logo=appveyor&style=for-the-badge)
[![Nuget](https://img.shields.io/nuget/v/TheCodingMonkey.Serialization.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/TheCodingMonkey.Serialization/)
[![Nuget](https://img.shields.io/nuget/dt/TheCodingMonkey.Serialization.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/TheCodingMonkey.Serialization/)
[![GitHub](https://img.shields.io/github/license/NickSchweitzer/TextSerializer.svg?logo=github&style=for-the-badge)](https://github.com/NickSchweitzer/TextSerializer/blob/master/LICENSE.txt)

Library for reading and writing CSV and Fixed Width files into a class or struct similar to XmlSerializer.

# Table of Contents
* [Attribute Based Configuration](#attribute-based-configuration)
* [Fluent Configuration](#fluent-configuration)

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
All of the same "common sense" defaults from Attribute based configuration are used. Fluent configuration should feel very similar to AutoMapper.

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

If your POCO class contains either all Fields or all Properties, then you can use the ByConvention method for CSV files, which defaults all properties to common sense defaults, and takes the fields or properties in the order listed.
Because of limitiations of .NET Reflection, this does not work if you have a mix of fields and properties and an exception will be thrown.
After calling ByConvention, you can still override any of the defaults on a property by property basis if you'd like, without having to specify all the values for all the rest of the properties.

```csharp
public class CsvPocoRecord
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public bool Enabled { get; set; }
}

// ...
var serializer = new CsvSerializer<CsvPocoRecord>(config => config.ByConvention());

```