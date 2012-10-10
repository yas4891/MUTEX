using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.tile;
using GSTLibrary.token;
using NUnit.Framework;

namespace GSTLibrary.test.tile
{
    [TestFixture]
    public class HashingGSTAlgorithmTest
    {
        private HashingGSTAlgorithm<GSTToken<char>> Algorithm;
        private AbstractGSTAlgorithmTest abstractGSTAlgorithmTest;

        [SetUp]
        public void SetUp()
        {
            abstractGSTAlgorithmTest = new AbstractGSTAlgorithmTest(typeof(GSTAlgorithm<GSTToken<char>>));
            var listA = GSTHelper.FromString("Hallo");
            var listB = GSTHelper.FromString("Hallo");

            Algorithm = new HashingGSTAlgorithm<GSTToken<char>>(listA, listB)
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
        public void InitializesHashes()
        {
            Algorithm.DoOneRun();
            Assert.AreEqual(3, Algorithm.HashesA.Count, string.Format("expected 3, but: A = {0}", Algorithm.HashesA.Count));
            Assert.AreEqual(3, Algorithm.HashesB.Count, string.Format("expected 3, but: B = {0}", Algorithm.HashesB.Count));
        }

        [Test]
        public void MinimizesHashes()
        {
            Algorithm = new HashingGSTAlgorithm<GSTToken<char>>(GSTHelper.FromString("XeLATst"), GSTHelper.FromString("LATunik")) { MinimumMatchLength = 3};
            Algorithm.DoOneRun();

            Assert.AreEqual(1, Algorithm.HashesA.Count, string.Format("expected 1 after minimize, but: A = {0}", Algorithm.HashesA.Count));
            Assert.AreEqual(1, Algorithm.HashesB.Count, string.Format("expected 1 after minimize, but: B = {0}", Algorithm.HashesB.Count));
        }

        [Test]
        public void DoOneRun()
        {
            Algorithm.DoOneRun();
            Assert.AreEqual(1, Algorithm.Tiles.Count());
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
