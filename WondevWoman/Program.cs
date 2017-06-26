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
internal class Player
{
    private static void Main(string[] args)
    {
        string[] inputs;
        int size = int.Parse(Console.ReadLine());
        int unitsPerPlayer = int.Parse(Console.ReadLine());

        // game loop
        while (true)
        {
            for (int i = 0; i < size; i++)
            {
                string row = Console.ReadLine();
            }
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int unitX = int.Parse(inputs[0]);
                int unitY = int.Parse(inputs[1]);
            }
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int otherX = int.Parse(inputs[0]);
                int otherY = int.Parse(inputs[1]);
            }
            int legalActions = int.Parse(Console.ReadLine());
            for (int i = 0; i < legalActions; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string atype = inputs[0];
                int index = int.Parse(inputs[1]);
                string dir1 = inputs[2];
                string dir2 = inputs[3];
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
        }
    }
}

internal class Unit
{
    public Owner Owner { get; set; }
    public Position Position { get; set; }

    public Unit(Owner owner, Position pos)
    {
        this.Owner = owner;
        this.Position = pos;
    }
}

internal class Map
{
    public List<Tile> Tiles { get; set; }

    public Map()
    {
        this.Tiles = new List<Tile>();
    }

    internal void AddTile(Tile tile)
    {
        this.Tiles.Add(tile);
    }
}

internal class Tile
{
    public Position Position { get; set; }
    public int Level { get; set; }

    public Tile(Position pos, int level = 0)
    {
        this.Position = pos;
        this.Level = level;
    }
}

internal class Order
{
    public Dictionary<OrderType, string> OrderTypesDictionary { get; set; }
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
        var order = $"{this.OrderTypesDictionary[this.OrderType]} {this.Index} {this.Turn.ToString()}";

        return order;
    }
}

internal class Turn
{
    public Direction MoveDirection { get; set; }
    public Direction BuildDirection { get; set; }

    public Turn(Direction move, Direction build)
    {
        this.MoveDirection = move;
        this.BuildDirection = build;
    }

    public override string ToString()
    {
        return $"{this.MoveDirection} {this.BuildDirection}";
    }
}

internal class Position
{
    public Coordinate X { get; set; }
    public Coordinate Y { get; set; }

    public Position(int x, int y)
    {
        this.X = new Coordinate(x);
        this.Y = new Coordinate(y);
    }
}

internal class Coordinate
{
    public int Number { get; set; }

    public Coordinate(int number)
    {
        this.Number = number;
    }
}

internal enum Direction
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

internal enum Owner
{
    Human,
    Other
}

internal enum OrderType
{
    MoveBuild
}