﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSTConsole
{
    static class GeneralHelper
    {
        public static readonly int DEFAULT_THRESHOLD = 50;

        /// <summary>
        /// extracts the student identifier from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetStudentIdentifier(string queryString)
        {
            return GetValueFromQueryString("pin", queryString);
        }

        /// <summary>
        /// extracts the assignment identifier from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetAssignmentIdentifier(string queryString)
        {
            return GetValueFromQueryString("repository", queryString);
        }

        /// <summary>
        /// extracts the path from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetPath(string queryString)
        {
            return GetValueFromQueryString("path", queryString);
        }

        /// <summary>
        /// extracts the template path from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetTemplate(string queryString)
        {
            return GetValueFromQueryString("template", queryString);
        }
        
        /// <summary>
        /// gets the threshold from the query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static Int32 GetThreshold(string queryString)
        {
            var first = GetValueFromQueryString("thres", queryString);
            int result;

            return Int32.TryParse(first, out result) ? result : DEFAULT_THRESHOLD;
        }

        /// <summary>
        /// returns the value from the query string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal static string GetValueFromQueryString(string key, string queryString)
        {
            var parts = queryString.Split(new[] { '&' });
            var first = parts.Where(part => part.StartsWith(key)).Select(part => part.Substring(part.IndexOf("=", StringComparison.Ordinal) + 1)).
                FirstOrDefault();

            return first;
        }

        /// <summary>
        /// returns the student identifier from args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static string GetStudentIdentifierFromArgs(string[] args)
        {
            return GetValueForOption("--student", args);
        }

        /// <summary>
        /// returns the assignment identifier from the args[]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetAssignmentIdentifierFromArgs(string[] args)
        {
            return GetValueForOption("--assignment", args);
        }

        /// <summary>
        /// returns the threshold
        /// </summary>
        /// <param name="args"></param>
        /// <returns>50 by default</returns>
        internal static int GetThresholdFromArgs(string[] args)
        {
            var opt = GetValueForOption("--threshold", args);

            
            return !string.IsNullOrWhiteSpace(opt) ? Int32.Parse(opt) : DEFAULT_THRESHOLD;
        }

        /// <summary>
        /// returns the path to the source code from the passed arguments[]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetPathFromArgs(string[] args)
        {
            if (args.Length < 1)
                return string.Empty;
            return args[args.Length - 1];
        }

        /// <summary>
        /// returns the template from the passed arguments[]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static string GetTemplateFromArgs(string[] args)
        {
            return GetValueForOption("--template", args);
        }

        /// <summary>
        /// returns the string after the "option" from args
        /// </summary>
        /// <param name="option"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetValueForOption(string option, string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == option)
                    return args[i + 1];
            }

            return string.Empty;
        }
    }
}
