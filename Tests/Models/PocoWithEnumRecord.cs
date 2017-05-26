using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    public class PocoWithEnumRecord
    {
        private int id;
        public int Id { get => id; set => id = value; } // Specifically test expression bodies
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public OptionsEnum Options { get; set; }
    }

    public enum OptionsEnum
    {
        FirstOption = 0,
        SecondOption = 1
    }
}