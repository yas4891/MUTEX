using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using log4net;

namespace CTokenizer
{
    /// <summary>
    /// token in relation to MutexCLexer
    /// </summary>
    public class MutexTokenImpl : IToken
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(MutexTokenImpl).Name);

        public string TokenName { get; private set; }

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
            try
            {
                TokenName = tokenName.ToUpperInvariant();

                Type = (int)typeof(MutexCLexer).GetField(TokenName).GetValue(null);
            }
            catch (Exception ex)
            {
                var msg = string.Format("could not find token '{0}'", TokenName);
                cLogger.Warn(msg);
                cLogger.Debug(msg, ex);
                //Console.WriteLine("tokenName:" + tokenName);
                throw;
            }
        }

        public override string ToString()
        {
            return TokenName;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
