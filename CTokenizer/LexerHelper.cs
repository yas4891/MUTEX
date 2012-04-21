using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CTokenizer
{
    /// <summary>
    /// 
    /// </summary>
    public static class LexerHelper
    {
        public static string GetTokenNameFromMUTEXLexer(this int tokenType)
        {
            return tokenType.GetTokenName(typeof(MutexCLexer));
        }
        public static string GetTokenNameFromCLexer(this int tokenType)
        {
            return tokenType.GetTokenName(typeof(CLexer));
        }

        public static string GetTokenName(this int tokenType, Type lexerType)
        {
            var fields =
               lexerType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(
                    field => field.FieldType == typeof(int)).Where(field => ((int)field.GetValue(null)) == tokenType).Select(field => field.Name);


            return fields.First();
        }
    }
}
