using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSTConsole
{
    static class GeneralHelper
    {
        internal static string GetStudentIdentifier(string queryString)
        {
            return queryString.Substring(0, queryString.IndexOf('&'));
        }
    }
}
