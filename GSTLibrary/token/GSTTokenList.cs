using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSTLibrary.token
{
    public class GSTTokenList<T> : List<T> where T : GSTToken
    {
        public GSTTokenList()
        {
        }

        public GSTTokenList(IEnumerable<T> enumerable)
        {
            AddRange(enumerable);
        }
    }
}
