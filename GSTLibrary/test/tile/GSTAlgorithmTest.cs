﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private AbstractGSTAlgorithmTest abstractGSTAlgorithmTest;

        [SetUp]
        public void SetUp()
        {
            abstractGSTAlgorithmTest = new AbstractGSTAlgorithmTest(typeof(GSTAlgorithm<GSTToken<char>>));
            
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
            Assert.True(AbstractGSTAlgorithm.IsInRange(5, 1, 10), "most basic");
            Assert.False(AbstractGSTAlgorithm.IsInRange(5, 3, 4), "above range");
            Assert.False(AbstractGSTAlgorithm.IsInRange(5, 6, 7), "below range");
            Assert.True(AbstractGSTAlgorithm.IsInRange(5, 3, 5), "upper edge");
            Assert.True(AbstractGSTAlgorithm.IsInRange(5, 5, 7), "lower edge");
        }

        [Test]
        public void RunToCompletion()
        {
            Assert.False(Algorithm.Finished);
            Algorithm.RunToCompletion();

            Assert.True(Algorithm.Finished);
        }

        [Test]
        public void TwoTilesInString()
        {
            abstractGSTAlgorithmTest.TwoTilesInString();
        }

        [Test]
        public void DontMatchTileTwice()
        {
            abstractGSTAlgorithmTest.DontMatchTileTwice();
        }

        [Test]
        public void MatchesTwoDifferentTiles()
        {
            abstractGSTAlgorithmTest.MatchesTwoDifferentTiles();
        }

        [Test]
        public void HandlesEscapeSequences()
        {
            abstractGSTAlgorithmTest.HandlesEscapeSequences();
        }

        [Test]
        public void SimilarityCalculation()
        {
            abstractGSTAlgorithmTest.SimilarityCalculation();
        }

        [Test]
        public void OverlappingMatchesOfSameSize()
        {
            abstractGSTAlgorithmTest.OverlappingMatchesOfSameSize();
        }

        [Test]
        public void DoNotMatchTwoIdenticalTilesOnA()
        {
            abstractGSTAlgorithmTest.DoNotMatchTwoIdenticalTilesOnA();
        }
    }
}
