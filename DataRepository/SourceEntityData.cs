using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRepository
{
    /// <summary>
    /// contains information about a source that is used solely to store or load 
    /// </summary>
    public struct SourceEntityData
    {
        /// <summary>
        /// unique identifier for the student 
        /// </summary>
        public readonly string StudentIdentifier;

        /// <summary>
        /// unique identifier for the assignment 
        /// </summary>
        public readonly string AssignmentIdentifier;

        /// <summary>
        /// tokens of this source
        /// </summary>
        public readonly IEnumerable<string> Tokens;

        /// <summary>
        /// the raw source 
        /// </summary>
        public readonly string RawSource;


        public SourceEntityData(string student, string assignment, IEnumerable<string> tokens, string source )
        {
            StudentIdentifier = student;
            AssignmentIdentifier = assignment;
            Tokens = tokens;
            RawSource = source;
        }
    }
}
