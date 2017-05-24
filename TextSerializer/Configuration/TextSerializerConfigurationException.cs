using System;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Thrown if there is a problem detected during Fluent Configuration.</summary>
    [Serializable]
    public class TextSerializationConfigurationException : Exception
    {
        /// <summary>Default Constructor</summary>
        public TextSerializationConfigurationException()
        { }

        /// <summary>Standard Constructor</summary>
        /// <param name="message">Exception Message</param>
        public TextSerializationConfigurationException(string message) 
        : base(message)
        { }

        /// <summary>Standard Constructor</summary>
        /// <param name="message">Exception Message</param>
        /// <param name="innerException">Additional exception thrown with more information</param>
        public TextSerializationConfigurationException(string message, Exception innerException) 
        : base(message, innerException)
        { }
    }
}