using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class CoordinateTests
{
    [Test]
    public void CoordinateCreationTest()
    {
        var coordinate = new Coordinate(2);

        Assert.That(coordinate.Number, Is.EqualTo(2));
    }
}