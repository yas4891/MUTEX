
using Antlr.Runtime;

namespace Tokenizer
{
    /// <summary>
    /// wrapper around ANTLR's IToken to override Equals.
    /// Needed to make it work with GSTToken / GSTAlgorithm
    /// </summary>
    public class TokenWrapper
    {
        /// <summary>
        /// the saved token object from ANTLR lexers
        /// </summary>
        private IToken Token { get; set; }

        private string Text { get; set; }

        public TokenType Type { get; private set; }

        public TokenWrapper(IToken token)
        {
            Token = token;
            Text = token.Text;
            Type = new TokenType {Type = token.Type};
        }

        /// <summary>
        /// compares for value equality using the token's Type 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (TokenWrapper) obj;

            if (null == Token && null == other.Token)
                return true;

            if (null == Token ||
                null == other.Token)
                return false;

            return other.Type.Equals(Type);
        }

        public override int GetHashCode()
        {
            return null == Token ? GetType().GetHashCode() : Token.GetHashCode();
        }

        public override string ToString()
        {
            return Token.Text;
        }
    }
}
