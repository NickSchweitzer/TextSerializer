# Overview

By default, built in data types are deserialized using TryParse or Convert.ChangeType. This is adequate if your CSV or Fixed Width file uses normal .NET string formatting. However, if you have a field which uses non-standard formatting, then you need to implement a Custom Formatter.

# Creating a Custom Formatter

Custom formatters are created by first creating a formatter class which implements [ITextFormatter](../Reference/TheCodingMonkey.Serialization/ITextFormatter.md). There is an example formatter called [BooleanIntFormatter](https://github.com/NickSchweitzer/TextSerializer/blob/master/TextSerializer/Formatters/BooleanIntFormatter.cs) which can be used as a sample. This converts a 1/0 value into a bool (instead of relying on true/false).

```csharp
/// <summary>Formatter which allows serialization and deserialization of Booleans to Integer values in a file.</summary>
public class BooleanIntFormatter : ITextFormatter
{
    /// <summary>Deserializes a string and returns the boolean equivalent.</summary>
    /// <param name="strValue">String value from file that must be deserialized.</param>
    /// <returns>False for 0, True for any other integer value.</returns>
    public object Deserialize(string strValue)
    {
        if (int.TryParse(strValue, out int value))
            return value == 0 ? false : true;

        throw new FormatException($"{strValue} is not an integer that can be converted to boolean");
    }

    /// <summary>Serializes a boolean to an equivalent integer</summary>
    /// <param name="objValue">Boolean to serialize.</param>
    /// <returns>0 for False, 1 for True.</returns>
    public string Serialize(object objValue)
    {
        bool value = (bool)objValue;
        return value ? "1" : "0";
    }
}
```

# Configuring the Serializer to Use a Custom Formatter

## Attribute Based Configuration

```csharp
[TextSerializable]
public class MyCsvRecord
{
    [TextField(0)]
    public int Id { get; set; }

    [TextField(1)]
    public string Name { get; set; }

    [TextField(2, FormatterType(typeof(BooleanIntFormatter)))]
    public bool Enabled { get; set; }
}

var csvSer = new CsvSerializer<MyCsvRecord>();
IEnumerable<MyCsvRecord> sampleCsv = null;
using (StreamReader reader = new File.OpenText("sample.csv"))
{
    sampleCsv = csvSer.DeserializeArray(reader);
    reader.Close();
}
```

## Fluent Configuration

```csharp
public class PocoCsvRecord
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Enabled { get; set; }
}

var csvSer = new CsvSerializer<PocoCsvRecord>(config => config.ByConvention()
    .ForMember(field => field.Enabled, opt => opt.FormatterType(typeof(BooleanIntFormatter))));

IEnumerable<PocoCsvRecord> sampleCsv = null;
using (StreamReader reader = new File.OpenText("sample.csv"))
{
    sampleCsv = csvSer.DeserializeArray(reader);
    reader.Close();
}
```