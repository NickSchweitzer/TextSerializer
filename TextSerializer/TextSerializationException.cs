#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace TheCodingMonkey.Serialization
{
    /// <summary>Exception class for Text Serialization exceptions.</summary>
    public class TextSerializationException : Exception
    {
        /// <summary>Standard Constructor</summary>
        /// <param name="msg">Exception Message</param>
        public TextSerializationException( string msg )
        : base( msg )
        { }
    }
}