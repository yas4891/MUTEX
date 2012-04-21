﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace CTokenizer
{
    /// <summary>
    /// wrapper around IToken to override Equals.
    /// Needed to make it work with GSTToken / GSTAlgorithm
    /// </summary>
    public class TokenWrapper
    {
        /// <summary>
        /// the saved token object from ANTLR lexers
        /// </summary>
        public IToken Token { get; private set; }

        public TokenWrapper(IToken token)
        {
            Token = token;
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

            return other.Token.Type == Token.Type;
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
