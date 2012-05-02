using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;

namespace GSTAppLogic.app
{
    /// <summary>
    /// interface to the application logic layer
    /// </summary>
    public interface IAppLogic
    {
        /// <summary>
        /// returns the maximum found similarity
        /// </summary>
        int MaximumSimilarity { get; }

        /// <summary>
        /// starts the comparison using the passed in parameter
        /// </summary>
        /// <param name="student"></param>
        /// <param name="assignment"></param>
        /// <param name="source"></param>
        void Start(string student, string assignment, string source);
    }

    public class AppLogic
    {
        public static IAppLogic GetAppLogic()
        {
            return new AppLogicImpl();
        }

    }
}
