using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTLibrary.token;
using NUnit.Framework;

namespace GSTLibrary.test.token
{
    [TestFixture]
    public class GSTTokenTest
    {
        private GSTTokenList<GSTToken<char>> ListA;
        private GSTTokenList<GSTToken<char>> ListB;

        [SetUp]
        public void SetUp()
        {
            ListA = GSTHelper.FromString("Hallo");
            ListB = GSTHelper.FromString("Hallo");
        }

        [Test]
        public void Equals()
        {
            for(int i = 0; i < ListA.Count; i++)
            {
                Assert.True(ListA[i].EqualsTokenValue(ListB[i]), "values did not match. A {0}, B{1}", ListA[i], ListB[i]);
                Assert.True(ListA[i] != ListB[i]);
            }
        }
    }
}
