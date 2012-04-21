using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSTLibrary.exception
{
    /// <summary>
    /// used to keep the thrown exception consistent with the interface
    /// </summary>
    public class GSTException : Exception
    {
        public GSTException(string msg) : base (msg)
        {
            
        }
    }
}
