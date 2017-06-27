using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
public class Player
{
    private static void Main(string[] args)
    {
        string[] inputs;
        int size = int.Parse(Console.ReadLine());

        var map = new Map(size);

        int unitsPerPlayer = int.Parse(Console.ReadLine());

        var humanUnits = new PlayerUnits(unitsPerPlayer);
        var opponentUnits = new PlayerUnits(unitsPerPlayer);

        // game loop
        while (true)
        {
            for (int i = 0; i < size; i++)
            {
                string row = Console.ReadLine();
                var count = 0;

                foreach (var letter in row)
                {
                    if (letter == '.')
                    {
                        map.FindTile(count, i).UpdateLevel(-1);
                    }
                    else
                    {
                        map.FindTile(count, i).UpdateLevel(letter);
                    }
                    
                    count++;
                }
            }
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int unitX = int.Parse(inputs[0]);
                int unitY = int.Parse(inputs[1]);

                var pos = new Position(unitX, unitY);
                humanUnits.Units[i].UpdatePosition(pos);
            }
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int otherX = int.Parse(inputs[0]);
                int otherY = int.Parse(inputs[1]);

                var pos = new Position(otherX, otherY);
                opponentUnits.Units[i].UpdatePosition(pos);
            }
            int legalActions = int.Parse(Console.ReadLine());
            var legalOrders = new OrderList();
            for (int i = 0; i < legalActions; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string atype = inputs[0];
                int index = int.Parse(inputs[1]);
                string dir1 = inputs[2];
                string dir2 = inputs[3];

                var turn = new Turn(dir1, dir2);
                var order = new Order(OrderType.MoveBuild, turn);
                legalOrders.AddOrder(order, map, humanUnits.GetFirstUnit().Position);
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
        }
    }
}

public class Bot
{

}

public class PlayerUnits
{
    public List<Unit> Units { get; set; }

    public PlayerUnits(int number)
    {
        this.Units = new List<Unit>();

        for (int i = 0; i < number; i++)
        {
            var pos = new Position(0, 0);
            var unit = new Unit(Owner.Human, pos);
            this.Units.Add(unit);
        }
    }

    public Unit GetFirstUnit()
    {
        var unit = this.Units.FirstOrDefault();

        if (unit == null)
        {
            throw new Exception();
        }

        return unit;
    }
}

public class Unit
{
    public Owner Owner { get; private set; }
    public Position Position { get; private set; }

    public Unit(Owner owner, Position pos)
    {
        this.Owner = owner;
        this.Position = pos;
    }

    public void UpdatePosition(Position pos)
    {
        this.Position = pos;
    }
}

public class Map
{
    public List<Tile> Tiles { get; set; }
    public int MapTileCount
    {
        get
        {
            return this.Tiles.Count;
        }
    }

    public Map(int size)
    {
        this.Tiles = new List<Tile>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var pos = new Position(i, j);
                var tile = new Tile(pos);
                this.AddTile(tile);
            }
        }
    }

    public void AddTile(Tile tile)
    {
        if (this.Tiles
            .Exists(ti => ti.Position.X.Number == tile.Position.X.Number 
            && ti.Position.Y.Number == tile.Position.Y.Number))
        {
            throw new ArgumentException();
        }

        this.Tiles.Add(tile);
    }

    public Tile FindTile(Position pos)
    {
        return this.FindTile(pos.X.Number, pos.Y.Number);
    }

    public Tile FindTile(int x, int y)
    {
        var tile = this.Tiles
            .Where(currentTile => currentTile.Position.X.Number == x)
            .Where(currentTile => currentTile.Position.Y.Number == y)
            .FirstOrDefault();

        if (tile == null)
        {
            throw new Exception();
        }

        return tile;
    }
}

public class Tile
{
    public Position Position { get; private set; }
    public int Level { get; private set; }

    public Tile(Position pos, int level = 0)
    {
        this.Position = pos;
        this.Level = level;
    }

    public void UpdateLevel(int level)
    {
        this.Level = level;
    }
}

public class OrderList
{
    public Dictionary<Order,Tile> OrdersList { get; set; }

    public OrderList()
    {
        this.OrdersList = new Dictionary<Order, Tile>();
    }

    public void AddOrder(Order order, Map map, Position unitPos)
    {
        var tile = map.FindTile(unitPos);

        this.OrdersList.Add(order, tile);
    }
}

public class Order
{
    private Dictionary<OrderType, string> OrderTypesDictionary { get; set; }
    public OrderType OrderType { get; set; }
    public int Index { get; set; }
    public Turn Turn { get; set; }

    public Order(OrderType order, Turn turn)
    {
        this.OrderTypesDictionary = new Dictionary<OrderType, string>()
        {
            {OrderType.MoveBuild, "MOVE&BUILD"}
        };

        this.OrderType = order;

        this.Index = 0;

        this.Turn = turn;
    }

    public string WriteOrder()
    {
        var order = $"{this.OrderTypesDictionary[this.OrderType]} {this.Index} {this.Turn}";

        return order;
    }
}

public class Turn
{
    public Direction MoveDirection { get; set; }
    public Direction BuildDirection { get; set; }

    public Turn(Direction move, Direction build)
    {
        this.MoveDirection = move;
        this.BuildDirection = build;
    }

    public Turn(string move, string build)
    {
        this.MoveDirection = this.ParseStringToDirection(move);
        this.BuildDirection = this.ParseStringToDirection(build);
    }

    public Direction ParseStringToDirection(string letter)
    {
        switch (letter)
        {
            case "N":
                return Direction.N;
            case "NE":
                return Direction.NE;
            case "E":
                return Direction.E;
            case "SE":
                return Direction.SE;
            case "S":
                return Direction.S;
            case "SW":
                return Direction.SW;
            case "W":
                return Direction.W;
            case "NW":
                return Direction.NW;
            default:
                throw new ArgumentOutOfRangeException();                
        }
    }

    public override string ToString()
    {
        return $"{this.MoveDirection} {this.BuildDirection}";
    }
}

public class Position
{
    public Coordinate X { get; set; }
    public Coordinate Y { get; set; }

    public Position(int x, int y)
    {
        this.X = new Coordinate(x);
        this.Y = new Coordinate(y);
    }
}

public class Coordinate
{
    public int Number { get; set; }

    public Coordinate(int number)
    {
        this.Number = number;
    }
}

public enum Direction
{
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW
}

public enum Owner
{
    Human,
    Other
}

public enum OrderType
{
    MoveBuild
}