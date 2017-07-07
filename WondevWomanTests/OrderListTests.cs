using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class OrderListTests
{
    [Test]
    public void AddOrderTest()
    {
        var orderList = new OrderList();

        var map = new Map(3);

        var pos = new Position(1, 1);

        var turn = new Turn(Direction.N, Direction.S);

        var order = new Order(OrderType.MoveBuild, turn);

        orderList.AddOrder(order, map, pos);

        var orderTile = orderList.OrdersList[order];

        var movePos = new Position(1, 0);

        var buildPos = new Position(1, 1);     

        Assert.That(orderTile.MoveTile.Position, Is.EqualTo(movePos));
        Assert.That(orderTile.BuildTile.Position, Is.EqualTo(buildPos));
    }

    [Test]
    public void PreferredMoveTileTest()
    {
        var orderList = new OrderList();

        var pos = new Position(2, 2);

        var map = new Map(5);

        var movePos = new Position(2, 1);

        map.FindTile(movePos).UpdateLevel(2);        

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var turn = new Turn((Direction)i, (Direction)j);
                var order = new Order(OrderType.MoveBuild, turn);
                orderList.AddOrder(order, map, pos);
            }
        }

        var moveTile = orderList.PreferredMoveTile();

        Assert.That(moveTile.Position, Is.EqualTo(movePos));
    }

    [Test]
    public void PreferredBuildTileTest()
    {
        var orderList = new OrderList();

        var pos = new Position(2, 2);

        var map = new Map(5);

        var movePos = new Position(2, 1);
        var buildPos = new Position(2, 0);

        map.FindTile(movePos).UpdateLevel(2);
        map.FindTile(buildPos).UpdateLevel(1);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var turn = new Turn((Direction)i, (Direction)j);
                var order = new Order(OrderType.MoveBuild, turn);
                orderList.AddOrder(order, map, pos);
            }
        }

        var moveTile = orderList.PreferredMoveTile();

        var buildTile = orderList.PreferredBuildTile(moveTile);

        Assert.That(buildTile.Position, Is.EqualTo(buildPos));
    }
}