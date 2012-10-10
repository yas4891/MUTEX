using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.tile;
using GSTLibrary.token;
using NUnit.Framework;
using System.Reflection;

namespace GSTLibrary.test.tile
{
    /// <summary>
    /// this is not a test fixture per-se, but contains all the
    /// common algorithm test
    /// </summary>
    public class AbstractGSTAlgorithmTest
    {
        private Type AlgoType;
        private ConstructorInfo AlgoConstructor;
        internal AbstractGSTAlgorithm<GSTToken<char>> Algorithm;


        public Type GSTAlgorithmType
        {
            get { return AlgoType; }
            set
            {
                AlgoType = value;
                var parameterType = typeof (GSTTokenList<GSTToken<char>>);
                AlgoConstructor = value.GetConstructor(new[] {parameterType, parameterType});
            }
        }

        public AbstractGSTAlgorithmTest(Type algoType)
        {
            GSTAlgorithmType = algoType;
        }

        public void CreateAlgorithm(object[] parameters)
        {
            Algorithm = (AbstractGSTAlgorithm<GSTToken<char>>) AlgoConstructor.Invoke(parameters);
            Algorithm.MinimumMatchLength = 5;
        }

        public void CreateAlgorithm(GSTTokenList<GSTToken<char>> a, GSTTokenList<GSTToken<char>> b)
        {
            CreateAlgorithm(new[] {a, b});
        }

        public void TwoTilesInString()
        {
            CreateAlgorithm(GSTHelper.FromString("Hallo Welt! Wie du mir gefällst!"),
                            GSTHelper.FromString("Hallo Welt?ASDF Wie du mir gefällst_"));

            Algorithm.DoOneRun();

            Assert.AreEqual(1, Algorithm.Tiles.Count(), "no matching tiles found");

            var tile = Algorithm.Tiles.First();

            Assert.True(" Wie du mir gefällst".ToCharTile(11, 15).EqualsValue(tile));

            Algorithm.DoOneRun();
            Assert.AreEqual(2, Algorithm.Tiles.Count(), "too few tiles found");
        }

        public void DontBeCaseInsensitive()
        {
            CreateAlgorithm(
                GSTHelper.FromString("hallo welt! wie du mir gefällst!"),
                GSTHelper.FromString("Hallo Welt?ASDF Wie du mir gefällst_"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(0, Algorithm.Tiles.Count(), "found UNEXPECTED match");
        }

        public void DontMatchTileTwice()
        {
            CreateAlgorithm(
                GSTHelper.FromString("Hallo Welt!"),
                GSTHelper.FromString("Hallo Welt!ASDF Hallo Welt!"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(1, Algorithm.Tiles.Count(), "matched a tile more than once");
        }

        public void MatchesTwoDifferentTiles()
        {
            CreateAlgorithm(
                GSTHelper.FromString("Hallo Welt! Du bist Deutschland"),
                GSTHelper.FromString("*BlaBlub*Hallo Welt!ASDF Du bist Deutschland!"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(2, Algorithm.Tiles.Count(), "unexpected number of matches");
            var listTiles = Algorithm.Tiles.ToList();

            Assert.True(" Du bist Deutschland".ToCharTile(11, 24).EqualsValue(listTiles[0]), "first tile does not match");
            Assert.True("Hallo Welt!".ToCharTile(0, 9).EqualsValue(listTiles[1]), "second tile does not match");
        }

        public void HandlesEscapeSequences()
        {
            CreateAlgorithm(
                GSTHelper.FromString("Hallo Welt!\r\n\t Du bist Deutschland"),
                GSTHelper.FromString("*Bla\nBlub*Hallo Welt!\tAS\rDF Du bist Deutschland!"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(2, Algorithm.Tiles.Count(), "unexpected number of matches");
            var listTiles = Algorithm.Tiles.ToList();

            Assert.True(" Du bist Deutschland".ToCharTile(14, 27).EqualsValue(listTiles[0]), "first tile does not match");
            Assert.True("Hallo Welt!".ToCharTile(0, 10).EqualsValue(listTiles[1]), "second tile does not match");
        }

        public void SimilarityCalculation()
        {
            CreateAlgorithm(
                GSTHelper.FromString("Hallo Welt!\r\n\t Du bist Deutschland"),
                GSTHelper.FromString("*Bla\nBlub*Hallo Welt!\tAS\rDF Du bist Deutschland!"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(91, Algorithm.Similarity, "similarity has unexpected value");
        }

        public void OverlappingMatchesOfSameSize()
        {
            CreateAlgorithm(
                 GSTHelper.FromString("Hallo Welt!"),
                 GSTHelper.FromString("allo Welt!|SEP|Hallo Welt"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(1, Algorithm.Tiles.Count(), "too many tiles found");
        }

        public void DoNotMatchTwoIdenticalTilesOnA()
        {
            CreateAlgorithm(
                GSTHelper.FromString("Hallo Welt! Hallo Welt!"),
                GSTHelper.FromString("Hallo Welt!"));

            Algorithm.RunToCompletion();

            Assert.AreEqual(1, Algorithm.Tiles.Count());
        }
    }
}
