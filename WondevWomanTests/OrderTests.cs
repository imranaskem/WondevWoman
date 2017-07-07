using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class OrderTests
{
    [Test]
    public void OrderCreationTest()
    {
        var order = new Order(OrderType.MoveBuild, new Turn(Direction.N, Direction.S));

        Assert.That(order.Index, Is.EqualTo(0));        
    }

    [Test]
    public void WriteOrderTest()
    {
        var order = new Order(OrderType.MoveBuild, new Turn(Direction.N, Direction.S));

        var writeOrder = order.ToString();

        Assert.That(writeOrder, Is.EqualTo("MOVE&BUILD 0 N S"));
    }
}