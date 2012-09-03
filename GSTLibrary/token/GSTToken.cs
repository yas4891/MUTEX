using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSTLibrary.token
{
    
    // Don't want to implement the equality members because 
    // then the subclass members will not be called

#pragma warning disable 660,661
    /// <summary>
    /// base class for tokens. 
    /// This is mainly implemented so that GSTTokenList and GSTAlgorithm 
    /// don't need a second type argument
    /// </summary>
    public abstract class GSTToken
#pragma warning restore 660,661
    {
        /// <summary>
        /// get/set: Token was marked as part of a tile
        /// </summary>
        public bool Marked { get; set; }

        /// <summary>
        /// true if the token values match
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool EqualsTokenValue(GSTToken other);

    }

    /// <summary>
    /// generic implementation of the GSTToken class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GSTToken<T> : GSTToken
    {
        /// <summary>
        /// the real token
        /// </summary>
        public T Token { get; private set; }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="token"></param>
        public GSTToken(T token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return Token.ToString();
        }

        public override bool EqualsTokenValue(GSTToken other)
        {
            
            if(other is GSTToken<T>)
            {
                return ((GSTToken<T>) other).Token.Equals(Token);
            }

            return false;
        }
    }
}
