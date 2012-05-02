using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace CTokenizer
{
    /// <summary>
    /// token in relation to MutexCLexer
    /// </summary>
    public class MutexTokenImpl : IToken
    {
        public int Channel { get; set; }

        public int CharPositionInLine { get; set; }

        public ICharStream InputStream { get; set; }

        public int Line { get; set; }

        public int StartIndex { get; set; }

        public int StopIndex { get; set; }

        public string Text { get; set; }

        public int TokenIndex { get; set; }

        public int Type { get; set; }

        public MutexTokenImpl(string tokenName)
        {
            Type = (int) typeof (MutexCLexer).GetField(tokenName.ToUpper()).GetValue(null);
        }
    }
}
