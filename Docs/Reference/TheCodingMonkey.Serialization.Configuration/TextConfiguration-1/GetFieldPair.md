# TextConfiguration&lt;TTargetType&gt;.GetFieldPair method

Retrieves the Field configuration for the specified member, along with the position in the file where it should be serialized.

```csharp
protected KeyValuePair<int, Field>? GetFieldPair(MemberInfo member)
```

| parameter | description |
| --- | --- |
| member | The Reflection MemberInfo for the field to find in this configuration |

## Return Value

A KeyValuePair containing the configured member if its already been configured, otherwise null.

## See Also

* class [Field](../../TheCodingMonkey.Serialization/Field.md)
* class [TextConfiguration&lt;TTargetType&gt;](../TextConfiguration-1.md)
* namespace [TheCodingMonkey.Serialization.Configuration](../../TextSerializer.md)

<!-- DO NOT EDIT: generated by xmldocmd for TextSerializer.dll -->
