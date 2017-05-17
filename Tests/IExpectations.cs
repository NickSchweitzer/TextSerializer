using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCodingMonkey.Serialization.Tests
{
    public interface IExpectations<T>
    {
        ICollection<T> List();
        T Item(int index);
    }
}