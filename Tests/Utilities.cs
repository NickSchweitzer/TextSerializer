using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TheCodingMonkey.Serialization.Tests
{
    public static class Utilities
    {
        public static StreamReader OpenEmbeddedFile(string testFileBase, string extension)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"TheCodingMonkey.Serialization.Tests.Files.{testFileBase}File.{extension}";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return new StreamReader(stream);
        }

        public static IExpectations<T> GetExpectations<T>(string testFileBase)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var expectationType = assembly.GetTypes().Where(t => t.Namespace == "TheCodingMonkey.Serialization.Tests.Expectations" &&
                t.Name == $"{testFileBase}Expectations").First();

            return (IExpectations<T>)Activator.CreateInstance(expectationType);
        }

        public static List<string> GetLines(string testFileBase, string extension)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"TheCodingMonkey.Serialization.Tests.Files.{testFileBase}File.{extension}";

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
    }
}