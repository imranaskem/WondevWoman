using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class MapTests
{
    [Test]
    public void MapCreationTest()
    {
        var map = new Map(2);

        Assert.That(map.Tiles.Count, Is.EqualTo(4));
    }
}