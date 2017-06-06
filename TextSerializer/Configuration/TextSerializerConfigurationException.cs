using System;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Thrown if there is a problem detected during Fluent Configuration.</summary>
    [Serializable]
    public class TextSerializationConfigurationException : Exception
    {
        /// <summary>Standard Constructor</summary>
        /// <param name="message">Exception Message</param>
        public TextSerializationConfigurationException(string message) 
        : base(message)
        { }
    }
}