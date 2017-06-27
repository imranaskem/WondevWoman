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

    [Test]
    public void AddTileTest()
    {
        var map = new Map(2);

        var tile = new Tile(new Position(2, 2));

        map.AddTile(tile);

        var foundTile = map.Tiles.SingleOrDefault(ti => ti.Position.X.Number == tile.Position.X.Number
            && ti.Position.Y.Number == tile.Position.Y.Number);

        Assert.That(foundTile.Position.X.Number, Is.EqualTo(tile.Position.X.Number));
        Assert.That(foundTile.Position.Y.Number, Is.EqualTo(tile.Position.Y.Number));
    }

    [Test]
    public void FindTileTest()
    {
        var map = new Map(3);

        var tile = map.FindTile(1, 1);

        Assert.That(tile.Position.X.Number, Is.EqualTo(1));
        Assert.That(tile.Position.Y.Number, Is.EqualTo(1));
    }
}