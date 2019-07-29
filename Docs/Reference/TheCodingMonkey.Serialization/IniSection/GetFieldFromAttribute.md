# IniSection.GetFieldFromAttribute method

Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.

```csharp
protected IniField GetFieldFromAttribute(MemberInfo member)
```

| parameter | description |
| --- | --- |
| member | Property or Field to return a configuration for. |

## Return Value

Field configuration if this property should be serialized, otherwise null to ignore.

## See Also

* class [IniField](../IniField.md)
* class [IniSection](../IniSection.md)
* namespace [TheCodingMonkey.Serialization](../../TextSerializer.md)

<!-- DO NOT EDIT: generated by xmldocmd for TextSerializer.dll -->