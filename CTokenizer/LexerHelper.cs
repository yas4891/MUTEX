using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Antlr.Runtime;
using Tokenizer;

namespace CTokenizer
{
    /// <summary>
    /// provides a bunch of useful (extension) methods for use with the Lexer class
    /// </summary>
    public static class LexerHelper
    {
        /*
        private static readonly Type usedLexer = typeof(MutexCLexer);


        internal static ConstructorInfo UsedConstructor
        {
           get { return usedLexer.GetConstructor(new[] {typeof (ICharStream)}); }
        } 

        /// <summary>
        /// creates a lexer from the passed file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Lexer CreateLexer(string path)
        {
            return (Lexer) UsedConstructor.Invoke(new object[] { new ANTLRFileStream(path) });
            
        }

        public static Lexer CreateLexerFromSource(string source)
        {
            return (Lexer)UsedConstructor.Invoke(new object[] { new ANTLRStringStream(source)});
        }
        
        public static string GetTokenNameFromMUTEXLexer(this int tokenType)
        {
            return tokenType.GetTokenName(typeof (MutexCLexer));
        }

        public static string GetTokenNameFromCLexer(this int tokenType)
        {
            return tokenType.GetTokenName(typeof (CLexer));
        }

        internal static string GetTokenName(this int tokenType)
        {
            return tokenType.GetTokenName(usedLexer);
        }

        internal static string GetTokenName(this int tokenType, Type lexerType)
        {
            var fields =
                lexerType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(
                    field => field.FieldType == typeof (int)).Where(field => ((int) field.GetValue(null)) == tokenType).
                    Select(field => field.Name);


            return fields.First();
        }

        /// <summary>
        /// returns enumerable containing all tokens in the lexer except EOF.
        /// automatically resets the lexer 
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static IEnumerable<IToken> GetTokens(this Lexer lexer)
        {
            var list = new List<IToken>();
            IToken myToken;
            const int eof = -1;

            while (eof != (myToken = lexer.NextToken()).Type)
            {
                if (BaseRecognizer.Hidden == myToken.Channel)
                    continue;

                list.Add(myToken);
            }

            lexer.Reset();
            return list;
        }

        /// <summary>
        /// joins the name of strings with whitespaces
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static string GetJoinedTokenString(this Lexer lexer)
        {
            var builder = new StringBuilder();
            var lexerType = lexer.GetType();

            foreach (var token in lexer.GetTokens())
            {
                builder.AppendFormat("{0} ", token.Type.GetTokenName(lexerType));
            }

            return builder.ToString().TrimEnd(new[] { ' ' });
        }

        /// <summary>
        /// returns the tokens with a bunch of debug information
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetDebugTokenStrings(this Lexer lexer)
        {
            var lexerType = lexer.GetType();

            return lexer.GetTokens().Select(
                token => string.Format("[{0:000}:{1:00}] {2}\t\t- {3}", 
                                        token.Line, 
                                        token.CharPositionInLine, 
                                        token.Type.GetTokenName(lexerType), 
                                        token.Text))
                .ToList();
        }

        /// <summary>
        /// joins the name of strings with whitespaces
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static string GetJoinedTokenString(this IEnumerable<TokenWrapper> tokens)
        {
            var builder = new StringBuilder();
            
            foreach (var token in tokens)
            {
                builder.AppendFormat("{0} ", token.Type.Type.GetTokenName(UsedLexer));
            }

            return builder.ToString().TrimEnd(new[] { ' ' });
        }

        /// <summary>
        /// fetches all tokens from the lexer and wraps the ANTLR IToken objects into TokenWrapper objects
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static IEnumerable<TokenWrapper> GetTokenWrappers(this Lexer lexer)
        {
            return lexer.GetTokens().Select(token => new TokenWrapper(token, GetTokenName(token.Type)));
        }

        /// <summary>
        /// splits the string up along the white spaces and returns an enumerable of TokenWrapper objects
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static IEnumerable<TokenWrapper> GetTokens(this string tokenString)
        {
            return tokenString.Trim().Split(new[] {' '}).GetTokens();
        }

        /// <summary>
        /// looks up the type of the tokens on MutexCLexer and returns an enumeration of TokenWrapper objects
        /// </summary>
        /// <param name="enumTokens"></param>
        /// <returns></returns>
        public static IEnumerable<TokenWrapper> GetTokens(this IEnumerable<string> enumTokens )
        {
            return enumTokens.Select(tokenName => new TokenWrapper(new MutexTokenImpl(tokenName), tokenName));
        }

        /// <summary>
        /// turns enumerable of TokenWrapper objects into the corresponding token names enumerable
        /// </summary>
        /// <param name="enumTokens"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToStringEnumerable(this IEnumerable<TokenWrapper> enumTokens)
        {
            return enumTokens.Select(token => token.Type.Type.GetTokenName());
        }
        /* */


        /// <summary>
        /// returns enumerable containing all tokens in the lexer except EOF.
        /// automatically resets the lexer 
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public static IEnumerable<IToken> GetTokens(this Lexer lexer)
        {
            var list = new List<IToken>();
            IToken myToken;
            const int eof = -1;

            while (eof != (myToken = lexer.NextToken()).Type)
            {
                if (BaseRecognizer.Hidden == myToken.Channel)
                    continue;

                list.Add(myToken);
            }

            lexer.Reset();
            return list;
        }
        
        /// <summary>
        /// returns a bunch of debug information for the tokens
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetDebugTokenStrings(this IEnumerable<TokenWrapper> tokens)
        {
            return tokens.Select(token => string.Format("[{0:000}:{1:00}] {2}\t\t- {3}",
                                                 token.Token.Line,
                                                 token.Token.CharPositionInLine,
                                                 token.TokenName,
                                                 token.Text));
        }

        /// <summary>
        /// turns enumerable of TokenWrapper objects into the corresponding token names enumerable
        /// </summary>
        /// <param name="enumTokens"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToStringEnumerable(this IEnumerable<TokenWrapper> enumTokens)
        {
            return enumTokens.Select(token => token.TokenName);
        }
    }
}
