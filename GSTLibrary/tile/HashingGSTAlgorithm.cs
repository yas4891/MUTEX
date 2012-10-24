using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GSTLibrary.exception;
using GSTLibrary.token;
using System.Threading;
using log4net;

namespace GSTLibrary.tile
{
    /// <summary>
    /// implements a speed-optimized version of the GSTAlgorithm
    /// The optimization is achieved by using elements of the Karp-Rabin-Algorithm.
    /// 
    /// The main benefit is a reduced complexity of normally only O(n) - compared to O(n^3) in the simple GST algorithm.
    /// 
    /// To achieve this the algorithm first calculates hash values for all sub sequences of length MML in both token streams. 
    /// Then it compares only those sub sequences that have the same hash value token by token
    /// 
    /// 
    /// For further information please read the Studienarbeit covering MUTEX
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashingGSTAlgorithm<T> : AbstractGSTAlgorithm<T> where T : GSTToken
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(HashingGSTAlgorithm<T>).Name);
        /// <summary>
        /// handles the hashing of any number of tokens and stores the hash values. 
        /// Used to abstract the implementation out of the algorithm
        /// </summary>
        internal class HashingEntity
        {
            /// <summary>
            /// comparer used to compare two instances of this class
            /// </summary>
            internal class HashingEntityComparer : IEqualityComparer<HashingEntity>
            {
                public bool Equals(HashingEntity x, HashingEntity y)
                {
                    if (null == x && null == y)
                        return true;

                    if (null == x || null == y)
                        return false;

                    return x.GetHashCode() == y.GetHashCode();
                }

                public int GetHashCode(HashingEntity obj)
                {
                    return obj.GetHashCode();
                }
            }

            /// <summary>
            /// "singleton" comparer for this class
            /// </summary>
            internal static readonly HashingEntityComparer Comparer = new HashingEntityComparer();

            /// <summary>
            /// factors used during computation of the hash values. This array contains the 12 first prime numbers
            /// </summary>
            private static readonly Int32[] multiplicators = new[] {3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59};

            /// <summary>
            /// the array of underlying tokens
            /// </summary>
            public T[] Tokens { get; private set; }

            /// <summary>
            /// calculated hash
            /// </summary>
            private readonly Int32 hash;

            /// <summary>
            /// creates a HashingEntity and calculates the hash value for the underlying token set
            /// </summary>
            /// <param name="list"></param>
            /// <param name="startIndex"></param>
            /// <param name="mml"></param>
            /// <returns></returns>
            internal HashingEntity(GSTTokenList<T> list, int startIndex, int mml)
            {
                var l = list.ToArray();
                var arr = new T[mml];

                for (int i = 0; i < mml; i++)
                {
                    arr[i] = l[i + startIndex];
                }

                var thing = arr.Select((t, i) => (t.GetHashCode()*(multiplicators[i%multiplicators.Length])));
                unchecked
                {
                    foreach (var element in thing)
                        hash += element;
                }
                

                //hash = unchecked(thing.Sum());
                Tokens = arr;
            }

            /// <summary>
            /// returns the hash calculated in the constructor
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return hash;
            }

            /// <summary>
            /// just a bit of a nice output
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var builder = new StringBuilder("HashingEntity:\"");
                builder.Append(Tokens[0].GetType());
                foreach(var element in Tokens)
                {
                    builder.Append(element.ToString());
                }
                
                return builder.Append('"').ToString();
            }
        }

        private bool initialized;
        internal Dictionary<Int32, IList<HashingEntity>> HashesA;
        internal Dictionary<Int32, IList<HashingEntity>> HashesB;

        /// <summary>
        /// contains the matches found during one run of the algorithm
        /// </summary>
        private List<Tile<T>> MatchesList;


        public HashingGSTAlgorithm(GSTTokenList<T> a, GSTTokenList<T> b) : base(a, b)
        {
        }

        public override void DoOneRun()
        {
            if (Finished)
                throw new GSTException("algorithm is finished");
            
            
            // can not do in constructor, because client code may change MML after constructor
            if(!initialized)
            {
                InitializeHashes();

                /*
                for (int i = 0; i < ListA.Count; i++)
                {
                    cLogger.DebugFormat("A: {0}, B:{1}", ListA[i].GetHashCode(), ListB[i].GetHashCode());
                }
                /* */
            }

            MatchesList = new List<Tile<T>>();

            LastMaximumMatch = MinimumMatchLength;

            // because of the minimization performed previously 
            // every element in A is also present in B!
            foreach(KeyValuePair<Int32, IList<HashingEntity>> kvpA in HashesA)
            {
                CompareLists(kvpA.Value, HashesB[kvpA.Key]);
            }

            foreach (var tile in MatchesList)
            {
                MarkTileAsMatched(tile);
                ListTiles.Add(tile);
            }

            TilesMatchedInLastRun = MatchesList;
        }

        /// <summary>
        /// compares two lists of HashingEntity objects
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        private void CompareLists(IList<HashingEntity> listA, IList<HashingEntity> listB)
        {
            foreach (var entityA in listA)
            {
                var indA = ListA.IndexOf(entityA.Tokens[0]);
                //Console.WriteLine("indexA: " + indA);
                foreach (var entityB in listB)
                {
                    var indB = ListB.IndexOf(entityB.Tokens[0]);
                    
                    int matchLength = CalculateMatchLength(indA, indB);
                    //Console.WriteLine("indexB: {0}, ML: {1}", indB, matchLength);
                    if(matchLength >= LastMaximumMatch)
                    {
                        var tile = AbstractGSTAlgorithm.CreateTile(ListA, indA, indB, matchLength);    
                        AddTileToMatchList(MatchesList, matchLength, tile);
                    }
                } // foreach in B
            } // foreach in A
        }

        /// <summary>
        /// creates both hash dictionaries and minimizes them. 
        /// side-effect: sets initialized = true
        /// </summary>
        private void InitializeHashes()
        {
            InitializeA();
            InitializeB();

            // this could be a possible optimization, but so far 
            // hash calculation does not seem like a bottleneck
            /*
            var thread1 = new Thread(InitializeA);
            var thread2 = new Thread(InitializeB);
            thread1.Start(); 
            thread2.Start();

            thread1.Join();
            thread2.Join();
            /* */

            /*
            foreach(var asdf in HashesA)
            {
                Console.WriteLine("hashA:" + asdf.Key + "|" + asdf.Value[0]);
            }

            foreach (var asdf in HashesB)
            {
                Console.WriteLine("hashB:" + asdf.Key + "|" + asdf.Value[0]);
            }
            /* */

            MinimizeHashes();
            initialized = true;
        }

        /// <summary>
        /// deletes all hashed values from A that have no counterpart in B and vice versa
        /// </summary>
        private void MinimizeHashes()
        {
            foreach (var key in HashesA.Keys.ToArray().Where(kvp => !HashesB.ContainsKey(kvp)))
            {
                HashesA.Remove(key);
            }

            foreach (var key in HashesB.Keys.ToArray().Where(kvp => !HashesA.ContainsKey(kvp)))
            {
                HashesB.Remove(key);
            }
        }

        /// <summary>
        /// initializes the hash map for ListA
        /// </summary>
        private void InitializeA()
        {
            HashesA = CreateHashMap(ListA, MinimumMatchLength);
        }

        /// <summary>
        /// initializes hash map for ListB
        /// </summary>
        private void InitializeB()
        {
            HashesB = CreateHashMap(ListB, MinimumMatchLength);
        }

        /// <summary>
        /// 1. generates the hash values for all sub sequences of list with a length of mml
        /// 2. stores the created HashingEntity objects in the returned dictionary
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mml"></param>
        /// <returns></returns>
        internal static Dictionary<int, IList<HashingEntity>> CreateHashMap(GSTTokenList<T> list, Int32 mml)
        {
            var dict = new Dictionary<int, IList<HashingEntity>>();

            // it needs to run exactly until COUNT - MML
            for(int i = 0; i <= list.Count - mml; i++)
            {
                var entity = new HashingEntity(list, i, mml);
                var hash = entity.GetHashCode();

                if(dict.ContainsKey(hash))
                {
                    dict[hash].Add(entity);
                }
                else
                {
                    dict[hash] = new List<HashingEntity> {entity};
                }
            }

            return dict;
        }
    }
}
