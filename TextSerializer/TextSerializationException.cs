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
        public TextSerializationException( string msg )
        : base( msg )
        { }
    }
}