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
    public class TileTest
    {
        [Test]
        public void EqualsTokensAndIndexOnA()
        {
            var tile1 = new Tile<GSTToken<char>> (GSTHelper.FromString("Hallo"), 1, 5);
            var tile2 = new Tile<GSTToken<char>>(GSTHelper.FromString("Hallo"), 1, 5);

            Assert.True(tile1.EqualsTokensAndIndexOnA(tile2), "identical tiles do not match");

            tile2 = new Tile<GSTToken<char>>(GSTHelper.FromString("Hallo"), 1, 10);

            Assert.True(tile1.EqualsTokensAndIndexOnA(tile2), "IndexOnB influences behaviour");
        }

        [Test]
        public void EqualsValue()
        {
            var tile1 = new Tile<GSTToken<char>>(GSTHelper.FromString("Hallo"), 1, 5);
            var tile2 = new Tile<GSTToken<char>>(GSTHelper.FromString("Hallo"), 1, 5);

            Assert.True(tile1.EqualsValue(tile2), "identical tiles do not match");

            tile2 = new Tile<GSTToken<char>>(GSTHelper.FromString("Hallo"), 1, 10);

            Assert.False(tile1.EqualsValue(tile2), "IndexOnB does not influence behaviour");
        }
    }
}
