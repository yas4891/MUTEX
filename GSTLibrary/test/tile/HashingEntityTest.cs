using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.token;
using NUnit.Framework;
using GSTLibrary.tile;

namespace GSTLibrary.test.tile
{
    [TestFixture]
    public class HashingEntityTest
    {
        [Test]
        public void EqualityComparerTest()
        {
            var dict1 = HashingGSTAlgorithm<GSTToken<char>>.CreateHashMap(GSTHelper.FromString("Hallo"), 3).ToArray();
            var dict2 = HashingGSTAlgorithm<GSTToken<char>>.CreateHashMap(GSTHelper.FromString("Hallo"), 3).ToArray();
            var comparer = HashingGSTAlgorithm<GSTToken<char>>.HashingEntity.Comparer;

            for(int i = 0; i < dict1.Length; i++)
            {
                Assert.AreEqual(dict1[i].Key, dict2[i].Key, string.Format("dict1 {0}, dict2 {1}", dict1[i].Value[0], dict2[i].Value[0]));
                Assert.IsTrue(comparer.Equals(dict1[i].Value[0], dict2[i].Value[0]));
            }
        }
    }
}
