# TextSerializer assembly

## TheCodingMonkey.Serialization namespace

| public type | description |
| --- | --- |
| abstract class [BaseSerializer&lt;TTargetType&gt;](TheCodingMonkey.Serialization/BaseSerializer-1.md) | Base class that contains common code for all serializers in the library. This class primary handles common reflection and configuration items. |
| class [CsvField](TheCodingMonkey.Serialization/CsvField.md) | Contains configuration properties for a field/property that are specific to CSV files |
| class [CsvSerializer&lt;TTargetType&gt;](TheCodingMonkey.Serialization/CsvSerializer-1.md) | Used to serialize a TargetType object to a CSV file. |
| abstract class [Field](TheCodingMonkey.Serialization/Field.md) | Class which contains the configuration details of a field/property that is created either using Attributes or Fluent Configuration |
| class [FixedWidthField](TheCodingMonkey.Serialization/FixedWidthField.md) | Contains configuration properties for a field/property that are specific to Fixed Width files |
| class [FixedWidthFieldAttribute](TheCodingMonkey.Serialization/FixedWidthFieldAttribute.md) | This attribute is applied to Fields or Properties of a class to control where in the fixed width file this field belongs. |
| class [FixedWidthSerializer&lt;TTargetType&gt;](TheCodingMonkey.Serialization/FixedWidthSerializer-1.md) | Used to serialize a TargetType object to a CSV file. |
| class [FormatEnumAttribute](TheCodingMonkey.Serialization/FormatEnumAttribute.md) | Controls how to format enumerations in a file, either String or Integer |
| interface [ITextFormatter](TheCodingMonkey.Serialization/ITextFormatter.md) | Interface which allows an object to be seralized/deserialized according to custom rules depending a given file format. |
| class [TextFieldAttribute](TheCodingMonkey.Serialization/TextFieldAttribute.md) | This attribute is applied to Fields or Properties of a class to control where in the CSV file this field belongs. |
| class [TextSerializableAttribute](TheCodingMonkey.Serialization/TextSerializableAttribute.md) | This attribute must be applied to a class to be serialized, unless the Fluent Configuration model is used |
| class [TextSerializationException](TheCodingMonkey.Serialization/TextSerializationException.md) | Exception class for Text Serialization exceptions. |
| abstract class [TextSerializer&lt;TTargetType&gt;](TheCodingMonkey.Serialization/TextSerializer-1.md) | Base class that contains common code for the [`CsvSerializer`](TheCodingMonkey.Serialization/CsvSerializer-1.md) and [`FixedWidthSerializer`](TheCodingMonkey.Serialization/FixedWidthSerializer-1.md) class. |

## TheCodingMonkey.Serialization.Configuration namespace

| public type | description |
| --- | --- |
| class [CsvConfiguration&lt;TTargetType&gt;](TheCodingMonkey.Serialization.Configuration/CsvConfiguration-1.md) | Fluent Configuration class for the [`CsvSerializer`](TheCodingMonkey.Serialization/CsvSerializer-1.md) class. |
| class [CsvFieldConfiguration](TheCodingMonkey.Serialization.Configuration/CsvFieldConfiguration.md) | Fluent Configuration class used to configure fields and properties which are serialized using the [`CsvSerializer`](TheCodingMonkey.Serialization/CsvSerializer-1.md) class. |
| class [FixedWidthConfiguration&lt;TTargetType&gt;](TheCodingMonkey.Serialization.Configuration/FixedWidthConfiguration-1.md) | Fluent Configuration class for the [`FixedWidthSerializer`](TheCodingMonkey.Serialization/FixedWidthSerializer-1.md) class. |
| class [FixedWidthFieldConfiguration](TheCodingMonkey.Serialization.Configuration/FixedWidthFieldConfiguration.md) | Fluent Configuration class used to configure fields and properties which are serialized using the [`FixedWidthSerializer`](TheCodingMonkey.Serialization/FixedWidthSerializer-1.md) class. |
| abstract class [TextConfiguration&lt;TTargetType&gt;](TheCodingMonkey.Serialization.Configuration/TextConfiguration-1.md) | Base class for Fluent Configuration classes |
| class [TextSerializationConfigurationException](TheCodingMonkey.Serialization.Configuration/TextSerializationConfigurationException.md) | Thrown if there is a problem detected during Fluent Configuration. |

## TheCodingMonkey.Serialization.Formatters namespace

| public type | description |
| --- | --- |
| class [BooleanIntFormatter](TheCodingMonkey.Serialization.Formatters/BooleanIntFormatter.md) | Formatter which allows serialization and deserialization of Booleans to Integer values in a file. |
| class [EnumFormatter](TheCodingMonkey.Serialization.Formatters/EnumFormatter.md) | Formatter which allows serialization and deserialization of Enums to Integer or String values in a file. |
| enum [EnumOptions](TheCodingMonkey.Serialization.Formatters/EnumOptions.md) | Used to control how Enumerations are serialized to files. |

## TheCodingMonkey.Serialization.Utilities namespace

| public type | description |
| --- | --- |
| static class [ReflectionHelper](TheCodingMonkey.Serialization.Utilities/ReflectionHelper.md) | Static utility class which has common reflection code used throughout the library. |

<!-- DO NOT EDIT: generated by xmldocmd for TextSerializer.dll -->
