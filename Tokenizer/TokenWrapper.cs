
using Antlr.Runtime;
using System;

namespace Tokenizer
{
    /// <summary>
    /// wrapper around ANTLR's IToken to override Equals.
    /// Needed to make it work with GSTToken / GSTAlgorithm
    /// </summary>
    public class TokenWrapper
    {
        /// <summary>
        /// stores the hash code of the underlying token to speed up comparison
        /// </summary>
        private readonly long tokenHashCode; 

        /// <summary>
        /// the saved token object from ANTLR lexers
        /// </summary>
        public IToken Token { get; private set; }

        /// <summary>
        /// returns the text of the underlying token
        /// </summary>
        public string Text { get { return Token.Text; } }

        /// <summary>
        /// a human readable representation of the token type
        /// </summary>
        public string TokenName { get; private set;}

        /// <summary>
        /// class representing the token's type. 
        /// This is used to abstract the implementation type
        /// </summary>
        public TokenType Type { get; private set; }

        public TokenWrapper(IToken token, string tokenName)
        {
            TokenName = tokenName;
            Token = token;
            Type = new TokenType {Type = token.Type};
            tokenHashCode = Type.GetHashCode();
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

            return tokenHashCode == other.tokenHashCode;

            //return other.Type.Equals(Type);
        }

        public override int GetHashCode()
        {
            return null == Token ? Type.GetHashCode() : Token.GetHashCode();
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}
