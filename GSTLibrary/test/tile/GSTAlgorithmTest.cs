using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSTLibrary.tile;
using GSTLibrary.token;
using NUnit.Framework;

namespace GSTLibrary.test.tile
{
    [TestFixture]
    public class GSTAlgorithmTest
    {
        private GSTAlgorithm<GSTToken<char>> Algorithm;

        [SetUp]
        public void SetUp()
        {
            var listA = GSTHelper.FromString("Hallo");
            var listB = GSTHelper.FromString("Hallo");

            Algorithm = new GSTAlgorithm<GSTToken<char>>(listA, listB)
                            {
                                MinimumMatchLength = 3
                            };
        }


        [Test]
        public void InitialState()
        {
            Assert.False(Algorithm.Finished);
            Assert.AreEqual(0, Algorithm.Tiles.Count(), "tiles not empty");
        }

        [Test]
        public void DoOneRun()
        {
            Algorithm.DoOneRun();
            Assert.True(Algorithm.Tiles.Count() == 1, "no matching tiles found");

            var foundTile = Algorithm.Tiles.FirstOrDefault();

            Assert.NotNull(foundTile, "no tile found");
            Assert.True("Hallo".ToCharTile(0,0).EqualsValue(foundTile));
        }

        [Test]
        public void TwoRunsNeededToFinishDefaultSet()
        {
            Algorithm.DoOneRun();
            Algorithm.DoOneRun();
            Assert.True(Algorithm.Finished, "algorithm not finished");
        }

        [Test]
        public void TestIsInRange()
        {
            Assert.True(GSTAlgorithm<GSTToken>.IsInRange(5, 1, 10), "most basic");
            Assert.False(GSTAlgorithm<GSTToken>.IsInRange(5, 3, 4), "above range");
            Assert.False(GSTAlgorithm<GSTToken>.IsInRange(5, 6, 7), "below range");
            Assert.True(GSTAlgorithm<GSTToken>.IsInRange(5, 3, 5), "upper edge");
            Assert.True(GSTAlgorithm<GSTToken>.IsInRange(5, 5, 7), "lower edge");
        }

        [Test]
        public void TwoTilesInString()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("Hallo Welt! Wie du mir gefällst!"), 
                GSTHelper.FromString("Hallo Welt?ASDF Wie du mir gefällst_"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.DoOneRun();

            Assert.AreEqual(1, Algorithm.Tiles.Count(), "no matching tiles found");

            var tile = Algorithm.Tiles.First();

            Assert.True(" Wie du mir gefällst".ToCharTile(11, 15).EqualsValue(tile));
            
            Algorithm.DoOneRun();
            Assert.AreEqual(2, Algorithm.Tiles.Count(), "too few tiles found");
        }

        public void DontBeCaseInsensitive()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("hallo welt! wie du mir gefällst!"),
                GSTHelper.FromString("Hallo Welt?ASDF Wie du mir gefällst_"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.RunToCompletion();

            Assert.AreEqual(0, Algorithm.Tiles.Count(), "found UNEXPECTED match");

            
        }

        [Test]
        public void DontMatchTileTwice()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("Hallo Welt!"),
                GSTHelper.FromString("Hallo Welt!ASDF Hallo Welt!"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.RunToCompletion();


            Assert.AreEqual(1, Algorithm.Tiles.Count(), "matched a tile more than once");
        }

        [Test]
        public void RunToCompletion()
        {
            Assert.False(Algorithm.Finished);
            Algorithm.RunToCompletion();

            Assert.True(Algorithm.Finished);
        }

        [Test]
        public void MatchesTwoDifferentTiles()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("Hallo Welt! Du bist Deutschland"),
                GSTHelper.FromString("*BlaBlub*Hallo Welt!ASDF Du bist Deutschland!"))
            {
                MinimumMatchLength = 5
            };
            Algorithm.RunToCompletion();

            Assert.AreEqual(2, Algorithm.Tiles.Count(), "unexpected number of matches");
            var listTiles = Algorithm.Tiles.ToList();

            Assert.True(" Du bist Deutschland".ToCharTile(11 ,24).EqualsValue(listTiles[0]), "first tile does not match");
            Assert.True("Hallo Welt!".ToCharTile(0, 9).EqualsValue(listTiles[1]), "second tile does not match");
        }

        [Test]
        public void HandlesEscapeSequences()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("Hallo Welt!\r\n\t Du bist Deutschland"),
                GSTHelper.FromString("*Bla\nBlub*Hallo Welt!\tAS\rDF Du bist Deutschland!"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.RunToCompletion();

            Assert.AreEqual(2, Algorithm.Tiles.Count(), "unexpected number of matches");
            var listTiles = Algorithm.Tiles.ToList();

            Assert.True(" Du bist Deutschland".ToCharTile(14, 27).EqualsValue(listTiles[0]), "first tile does not match");
            Assert.True("Hallo Welt!".ToCharTile(0, 10).EqualsValue(listTiles[1]), "second tile does not match");
        }

        [Test]
        public void SimilarityCalculation()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("Hallo Welt!\r\n\t Du bist Deutschland"),
                GSTHelper.FromString("*Bla\nBlub*Hallo Welt!\tAS\rDF Du bist Deutschland!"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.RunToCompletion();

            Assert.AreEqual(91, Algorithm.Similarity, "similarity has unexpected value");
        }

        [Test]
        public void OverlappingMatchesOfSameSize()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                 GSTHelper.FromString("Hallo Welt!"),
                 GSTHelper.FromString("allo Welt!|SEP|Hallo Welt"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.RunToCompletion();

            Assert.AreEqual(1, Algorithm.Tiles.Count(), "too many tiles found"); 
        }

        [Test]
        public void DoNotMatchTwoIdenticalTilesOnA()
        {
            Algorithm = new GSTAlgorithm<GSTToken<char>>(
                GSTHelper.FromString("Hallo Welt! Hallo Welt!"), 
                GSTHelper.FromString("Hallo Welt!"))
            {
                MinimumMatchLength = 5
            };

            Algorithm.RunToCompletion();

            Assert.AreEqual(1, Algorithm.Tiles.Count());
        }
    }
}
