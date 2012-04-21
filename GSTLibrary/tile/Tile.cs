using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.token;

namespace GSTLibrary.tile
{
    /// <summary>
    /// a Tile represents a matching sequence of tokens in both Token streams.
    /// It contains information about the matching token sequence and its absolute position
    /// in both Token streams
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tile<T> where T : GSTToken
    {
        /// <summary>
        /// creates a tile with the passed sequence of tokens and indices
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="indA"></param>
        /// <param name="indB"></param>
        public Tile(IEnumerable<T> tokens, int indA, int indB)
        {
            Tokens = tokens.AsEnumerable();
            IndexOnA = indA;
            IndexOnB = indB;
        }

        /// <summary>
        /// the token sequence
        /// </summary>
        public IEnumerable<T> Tokens { get; private set; }

        /// <summary>
        /// absolute position of the token sequence in Token stream A
        /// </summary>
        public int IndexOnA { get; private set; }

        /// <summary>
        /// absolute position of the token sequence in Token stream B
        /// </summary>
        public int IndexOnB { get; private set; }

        /// <summary>
        /// returns a readable representation of this tile
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<{0}, {1}, {2}>", GetTokensAsString(), IndexOnA, IndexOnB);
        }

        public string GetTokensAsString()
        {
            var builder = new StringBuilder();

            foreach (var token in Tokens)
            {
                builder.Append(token.ToString());
            }

            return builder.ToString();
        }


        /// <summary>
        /// checks whether the token sequences match and the indices for Stream A are equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsTokensAndIndexOnA(Tile<T> other)
        {
            if (null == other) return false;

            if (IndexOnA != other.IndexOnA)
                return false;

            return EqualsTokens(other);
        }

        /// <summary>
        /// returns true if the tokens equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsTokens(Tile<T> other)
        {
            var ownTokens = Tokens.ToList();
            var otherTokens = other.Tokens.ToList();

            if (ownTokens.Count != otherTokens.Count)
                return false;

            // check each token if the values match
            for (int i = Tokens.Count() - 1; i >= 0; i--)
            {
                if (!ownTokens[i].EqualsTokenValue(otherTokens[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// returns TRUE if  all indices match and token sequences match
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsValue(Tile<T> other)
        {
            return null != other && 
                other.IndexOnB == IndexOnB &&
                EqualsTokensAndIndexOnA(other);
        }
    }
}
