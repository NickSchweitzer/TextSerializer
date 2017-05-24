using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TheCodingMonkey.Serialization.Tests.Helpers
{
    public static class Utilities
    {
        public static StreamReader OpenEmbeddedFile(string testFile)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"TheCodingMonkey.Serialization.Tests.Files.{testFile}";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return new StreamReader(stream);
        }

        public static List<string> GetLines(string testFile)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"TheCodingMonkey.Serialization.Tests.Files.{testFile}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                return GetLines(stream);
            }
        }

        public static List<string> GetLines(Stream stream)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            return lines;
        }

        public static T[] ToArray<T>(this ICollection<T> collection)
        {
            T[] returnArray = new T[collection.Count];
            collection.CopyTo(returnArray, 0);
            return returnArray;
        }

        public static T[] ToArray<T>(this ICollection collection)
        {
            T[] returnArray = new T[collection.Count];
            collection.CopyTo(returnArray, 0);
            return returnArray;
        }
    }
}