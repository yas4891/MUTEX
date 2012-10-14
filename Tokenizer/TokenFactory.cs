using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Antlr.Runtime;
using System.IO;

namespace Tokenizer
{
    /// <summary>
    /// factory creates all tokens according to the passed in lexer
    /// </summary>
    public abstract class TokenFactory
    {
        protected abstract Type LexerType { get; }

        /// <summary>
        /// returns the IToken object for this tokenName
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public abstract IToken GetToken(string tokenName);

        /// <summary>
        /// returns the TokenWrapper for this tokenName
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public TokenWrapper GetTokenWrapper(string tokenName)
        {
            return new TokenWrapper(GetToken(tokenName), tokenName);
        }

        /// <summary>
        /// reads the file and returns the TokenWrapper list representing this file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<TokenWrapper> GetTokenWrapperListFromFile(string path)
        {
            return GetTokenWrapperListFromSource(File.ReadAllText(path));
        }

        /// <summary>
        /// parses the source into a list of TokenWrapper objects
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public abstract List<TokenWrapper> GetTokenWrapperListFromSource(string source);

        /// <summary>
        /// turns the tokenNamens into TokenWrapper objects
        /// </summary>
        /// <param name="tokenNameEnumerable"></param>
        /// <returns></returns>
        public IEnumerable<TokenWrapper> GetTokenWrapperEnumerable(IEnumerable<string> tokenNameEnumerable)
        {
            return tokenNameEnumerable.Select(tokenString => new TokenWrapper(GetToken(tokenString), tokenString));
        }

        /// <summary>
        /// returns the name for the passed in token
        /// </summary>
        /// <param name="tokenType">the token represented as an integer</param>
        /// <param name="lexerType">the lexer that is used to find the token name</param>
        /// <returns></returns>
        public string GetTokenName(int tokenType, Type lexerType)
        {
            var fields =
                lexerType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(
                    field => field.FieldType == typeof(int)).Where(field => ((int)field.GetValue(null)) == tokenType).
                    Select(field => field.Name);


            return fields.First();
        }

        /// <summary>
        /// joins the name of strings with whitespaces
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public string GetJoinedTokenString(IEnumerable<TokenWrapper> tokens)
        {
            var builder = new StringBuilder();

            foreach (var token in tokens)
            {
                builder.AppendFormat("{0} ", GetTokenName(token.Type.Type, LexerType));
            }

            return builder.ToString().TrimEnd(new[] { ' ' });
        }
    }
}
