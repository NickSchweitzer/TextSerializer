namespace TheCodingMonkey.Serialization
{
    /// <summary>Interface which allows an object to be seralized/deserialized according to custom rules depending a given
    /// file format.</summary>
    public interface ITextFormatter
    {
        /// <summary>Serializes an object to a string value using custom rules.</summary>
        /// <param name="objValue">Object to serialize.</param>
        /// <returns>String to write out to a file.</returns>
        string Serialize  ( object objValue );

        /// <summary>Deserializes a string and creates a new instance of the specified object.</summary>
        /// <param name="strValue">String value from file that must be deserialized.</param>
        /// <returns>Resulting object from the string value.</returns>
        object Deserialize( string strValue );
    }
}