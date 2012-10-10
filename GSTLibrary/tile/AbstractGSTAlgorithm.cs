using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.token;

namespace GSTLibrary.tile
{
    public abstract class AbstractGSTAlgorithm
    {
        /// <summary>
        /// creates the token sequence from ListA by using indA and matchLength
        /// </summary>
        /// <param name="indA"></param>
        /// <param name="indB"></param>
        /// <param name="matchLength"></param>
        /// <param name="listA"></param>
        /// <returns></returns>
        internal static Tile<T> CreateTile<T>(GSTTokenList<T> listA, int indA, int indB, int matchLength) where T : GSTToken
        {
            var list = new List<T>();
            for (int i = indA; i < indA + matchLength; i++)
            {
                list.Add(listA[i]);
            }

            return new Tile<T>(list, indA, indB);
        }

        /// <summary>
        /// true if any of the indices are identical on both tiles
        /// </summary>
        /// <param name="element"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        internal static bool HasOverlappingIndices<T>(Tile<T> element, Tile<T> tile) where T : GSTToken
        {
            if (element.IndexOnA == tile.IndexOnA)
                return true;

            if (element.IndexOnB == tile.IndexOnB)
                return true;

            // lb ==> lower bound
            // ub ==> upper bound
            int lbElementA = element.IndexOnA;
            int ubElementA = element.IndexOnA + element.Tokens.Count();
            int lbElementB = element.IndexOnB;
            int ubElementB = element.IndexOnB + element.Tokens.Count();
            int lbTileA = tile.IndexOnA;
            int ubTileA = tile.IndexOnA + tile.Tokens.Count();
            int lbTileB = tile.IndexOnB;
            int ubTileB = tile.IndexOnB + tile.Tokens.Count();

            if (IsInRange(lbElementA, lbTileA, ubTileA) ||
                IsInRange(ubElementA, lbTileA, ubTileA) ||
                IsInRange(lbElementB, lbTileB, ubTileB) ||
                IsInRange(ubElementB, lbTileB, ubTileB) ||
                IsInRange(lbTileA, lbElementA, ubElementA) ||
                IsInRange(ubTileA, lbElementA, ubElementA) ||
                IsInRange(lbTileB, lbElementB, ubElementB) ||
                IsInRange(ubTileB, lbElementB, ubElementB))
                return true;



            return false;
        }

        internal static bool IsInRange(int index, int lowerBound, int upperBound)
        {
            return index >= lowerBound && index <= upperBound;
        }
    }

    /// <summary>
    /// abstract base class for the different GST implementations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractGSTAlgorithm<T> where T : GSTToken
    {
        /// <summary>
        /// the default minimum match length used 
        /// </summary>
        public const int DEFAULT_MINIMUM_MATCH_LENGTH = 12;

        /// <summary>
        /// internal storage for found Tiles
        /// </summary>
        protected readonly List<Tile<T>> ListTiles = new List<Tile<T>>();

        /// <summary>
        /// field to hold the MinimumMatchLength
        /// </summary>
        private int MML;

        /// <summary>
        /// the length of the last found match
        /// </summary>
        protected int LastMaximumMatch;

        /// <summary>
        /// the token list of stream A
        /// </summary>
        public GSTTokenList<T> ListA { get; private set; }

        /// <summary>
        /// the token list of stream B
        /// </summary>
        public GSTTokenList<T> ListB { get; private set; }

        /// <summary>
        /// returns the Tiles found so far
        /// </summary>
        public IEnumerable<Tile<T>> Tiles
        {
            get { return ListTiles.AsEnumerable(); }
        }

        /// <summary>
        /// returns true if no more runs are needed
        /// </summary>
        public bool Finished
        {
            get { return LastMaximumMatch <= MinimumMatchLength; }
        }

        /// <summary>
        /// the minimum length of a tile to be considered a match
        /// </summary>
        public int MinimumMatchLength
        {
            get
            {
                return MML;
            }
            set
            {
                MML = value;
                // this needs to be set to be greater than MML
                // because else the initial state of the algorithmB
                // would be FINISHED
                LastMaximumMatch = value + 1;
            }
        }

        /// <summary>
        /// returns the similarity between the token streams.
        /// 
        /// Similarity is calculated by adding up the length of all the found tiles and dividing it 
        /// by the length of the shorter list
        /// </summary>
        public int Similarity
        {
            get
            {
                double lenList = ListA.Count < ListB.Count ? ListA.Count : ListB.Count;
                double lenTiles = Tiles.Select(tile => tile.Tokens.Count()).Sum();
                /*
                Console.WriteLine("list length: {0}, {1}", ListA.Count, ListB.Count);
                Console.WriteLine("simil: {0} / {1}", lenTiles, lenList);
                /* */
                return (int)(100 * lenTiles / lenList);
            }
        }

        /// <summary>
        /// returns the Tiles that were matched during the last run
        /// </summary>
        public IEnumerable<Tile<T>> TilesMatchedInLastRun { get; protected set; }

        protected AbstractGSTAlgorithm(GSTTokenList<T> a, GSTTokenList<T> b)
        {
            if (null == a)
                throw new ArgumentNullException("a");

            if (null == b)
                throw new ArgumentNullException("b");

            
            LastMaximumMatch = DEFAULT_MINIMUM_MATCH_LENGTH + 1;
            
            MinimumMatchLength = DEFAULT_MINIMUM_MATCH_LENGTH;
            ListA = a;
            ListB = b;
        }

        /// <summary>
        /// marks all tokens of the tile as matched in both ListA and ListB
        /// </summary>
        /// <param name="tile"></param>
        protected void MarkTileAsMatched(Tile<T> tile)
        {
            for (int i = 0; i < tile.Tokens.Count(); i++)
            {
                if (ListA[i + tile.IndexOnA].Marked)
                    throw new ArgumentOutOfRangeException("tile", tile, "tile contains marked token on A");

                if (ListB[i + tile.IndexOnB].Marked)
                    throw new ArgumentOutOfRangeException("tile", tile, "tile contains marked token on B");
                ListA[i + tile.IndexOnA].Marked = true;
                ListB[i + tile.IndexOnB].Marked = true;
            }
        }

        /// <summary>
        /// Adds the tile to the match list, if matchLength == LastMaximumMatch
        /// and the indices don't overlap
        /// --
        /// if LastMaximumMatch is less than matchLength
        /// - the listMatches is cleared
        /// - the tile is added as the only remaining match
        /// - LastMaximumMatch is updated
        /// </summary>
        /// <param name="listMatches"></param>
        /// <param name="matchLength"></param>
        /// <param name="tile"></param>
        protected void AddTileToMatchList(List<Tile<T>> listMatches, int matchLength, Tile<T> tile)
        {
            if (matchLength == LastMaximumMatch)
            {
                if (!listMatches.Any(element => AbstractGSTAlgorithm.HasOverlappingIndices(element, tile)))
                    listMatches.Add(tile);
            }
            else if (matchLength > LastMaximumMatch)
            {
                listMatches.Clear();
                listMatches.Add(tile);
                LastMaximumMatch = matchLength;
            }
        }

        /// <summary>
        /// calculates the length of the found match by:
        /// - taking tokens from A and B
        /// - check if either is marked ==> if so, break
        /// - check if tokens have equal value ==> if so break
        /// - if neither increase index and count
        /// - else return the count
        /// </summary>
        /// <param name="offsetA"></param>
        /// <param name="offsetB"></param>
        /// <returns></returns>
        protected int CalculateMatchLength(int offsetA, int offsetB)
        {
            var index = 0;
            var cListA = ListA.Count;
            var cListB = ListB.Count;

            do
            {
                var tokA = ListA[offsetA + index];
                var tokB = ListB[offsetB + index];

                // if either is marked, break
                if (tokA.Marked || tokB.Marked)
                    break;

                // if they are no longer the same
                if (!tokA.EqualsTokenValue(tokB))
                    break;

                index++;

                // don't move the index out of range
            } while (offsetA + index < cListA &&
                    offsetB + index < cListB);

            return index;
        }

        /// <summary>
        /// runs the algorithm for however long is needed to finish it
        /// </summary>
        public virtual void RunToCompletion()
        {
            while (!Finished)
                DoOneRun();
        }

        /// <summary>
        /// performs exactly one iteration of the algorithm
        /// </summary>
        public abstract void DoOneRun();
    }
}
