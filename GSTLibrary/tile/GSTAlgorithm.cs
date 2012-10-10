using System;
using System.Collections.Generic;
using System.Linq;
using GSTLibrary.exception;
using GSTLibrary.token;
using System.Diagnostics;

namespace GSTLibrary.tile
{
    /// <summary>
    /// implements the greedy string tiling algorithm in a basic, non-optimized version
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GSTAlgorithm<T> : AbstractGSTAlgorithm<T> where T : GSTToken
    {
        public GSTAlgorithm(GSTTokenList<T> a, GSTTokenList<T> b) : base(a, b)
        {
        }

        /// <summary>
        /// performs exactly one iteration of the algorithm
        /// </summary>
        public override void DoOneRun()
        {
            if(Finished)
                throw new GSTException("algorithm is finished");

            var watch = Stopwatch.StartNew();

            var listMatches = new List<Tile<T>>();

            LastMaximumMatch = MinimumMatchLength;

            // for every token in A that is unmarked
            foreach(var tA in ListA.Where(t => !t.Marked))
            {
                var tokA = tA; // CLOSURE
                int indA = ListA.IndexOf(tokA);

                // for every token in B that is unmarked and matches tokA
                foreach(var tB in ListB.Where(t => !t.Marked).Where(tokA.EqualsTokenValue))
                {
                    //counter++;
                    var tokB = tB; // CLOSURE
                    int indB = ListB.IndexOf(tokB);
                    
                    int matchLength = CalculateMatchLength(indA, indB);

                    if(matchLength >= LastMaximumMatch)
                    {
                        var tile = AbstractGSTAlgorithm.CreateTile(ListA, indA, indB, matchLength);
                        AddTileToMatchList(listMatches, matchLength, tile);
                    }
                } // foreach in B
            } // foreach in A

            foreach (var tile in listMatches)
            {
                MarkTileAsMatched(tile);
                ListTiles.Add(tile);
            }

            TilesMatchedInLastRun = listMatches;
            //Console.WriteLine("one run({1}) took {0} ms", watch.ElapsedMilliseconds, counter);
        }
    }
}
