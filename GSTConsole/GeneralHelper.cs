using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSTConsole
{
    static class GeneralHelper
    {
        /// <summary>
        /// extracts the student identifier from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetStudentIdentifier(string queryString)
        {
            var firstPart = queryString.Substring(0, queryString.IndexOf('&'));
            var afterSecondSlash = firstPart.Substring(firstPart.IndexOf('/', firstPart.IndexOf('/') + 1) + 1);
            return afterSecondSlash.Substring(0, afterSecondSlash.IndexOf('/'));
        }

        /// <summary>
        /// extracts the assignment identifier from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetAssignmentIdentifier(string queryString)
        {
            var middlePart = queryString.Substring(queryString.IndexOf('&') + 1);
            middlePart = middlePart.Substring(0, middlePart.IndexOf('&'));

            return middlePart.Substring(middlePart.LastIndexOf('/') + 1);
        }

        /// <summary>
        /// extracts the path from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetPath(string queryString)
        {
            return queryString.Substring(0, queryString.IndexOf('&'));
        }
        
        internal static Int32 GetThreshold(string queryString)
        {
            var parts = queryString.Split(new[] {'&'});

            return Int32.Parse(parts.Where(part => part.StartsWith("thres=")).Select(part => part.Substring(part.IndexOf("=") + 1)).First());
            
        }
    }
}
