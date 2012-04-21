using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.tile;

namespace GSTLibrary.token
{
    public static class GSTHelper
    {
        /// <summary>
        /// turns the 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static GSTTokenList<GSTToken<char>> FromString(string source)
        {
            var list = new GSTTokenList<GSTToken<char>>(source.ToCharArray().Select(c => new GSTToken<char>(c)));

            return list;
        }

        /// <summary>
        /// creates a tile based on char tokens from the given sequence and indices
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="indexOnA"></param>
        /// <param name="indexOnB"></param>
        /// <returns></returns>
        public static Tile<GSTToken<char>> ToCharTile(this string sequence, int indexOnA, int indexOnB)
        {
            return new Tile<GSTToken<char>>(FromString(sequence), indexOnA, indexOnB);
        }
    }
}
