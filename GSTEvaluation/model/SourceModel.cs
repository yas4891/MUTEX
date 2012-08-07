using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTEvaluation.storage;

namespace GSTEvaluation.model
{
    /// <summary>
    /// represents a source with token streams
    /// </summary>
    class SourceModel
    {
        public Int64 ID { get; private set; }
        public string Tokens { get; private set; }
        public string Name { get; private set; }

        public SourceModel(string name, string tokens)
        {
            Name = name;
            Tokens = tokens;
            ID = SQLFacade.Instance.InsertSource(tokens, name);
        }
    }
}
