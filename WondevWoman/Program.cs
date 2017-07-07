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

            var bot = new Bot();

            var orderToWrite = bot.CalculateOrder(legalOrders);

            Console.WriteLine(orderToWrite.ToString());
        }
    }
}

public class Bot
{  
    public Order CalculateOrder(OrderList legalMoves)
    {
        var moveTile = legalMoves.PreferredMoveTile();

        var buildTile = legalMoves.PreferredBuildTile(moveTile);

        var order = legalMoves.GetOrderBasedOnTiles(moveTile, buildTile);

        return order;
    }
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
    public List<Tile> Tiles { get; private set; }
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

public class Tile : IEquatable<Tile>
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

    public bool Equals(Tile other)
    {
        if (this.Level == other.Level && this.Position.Equals(other.Position))
        {
            return true;
        }

        return false;
    }
}

public class OrderList
{
    public Dictionary<Order,OrderTiles> OrdersList { get; set; }

    public OrderList()
    {
        this.OrdersList = new Dictionary<Order, OrderTiles>();
    }

    public void AddOrder(Order order, Map map, Position unitPos)
    {
        var moveTilePos = unitPos.CalculateNewPositionOnDirection(order.Turn.MoveDirection);
        var moveTile = map.FindTile(moveTilePos);

        var buildTilePos = moveTilePos.CalculateNewPositionOnDirection(order.Turn.BuildDirection);
        var buildTile = map.FindTile(buildTilePos);

        var orderTile = new OrderTiles(moveTile, buildTile);

        this.OrdersList.Add(order, orderTile);
    }

    public Tile PreferredMoveTile()
    {
        var kvp = this.OrdersList
            .Where(dict => dict.Value.MoveTile.Level < 3 && dict.Value.MoveTile.Level > -1)
            .OrderByDescending(dict => dict.Value.MoveTile.Level)
            .FirstOrDefault();

        if (kvp.Value == null)
        {
            throw new Exception();
        }

        var tile = kvp.Value.MoveTile;                   

        return tile;
    }

    public Tile PreferredBuildTile(Tile moveTile)
    {
        var kvp = this.OrdersList
            .Where(dict => dict.Value.MoveTile.Equals(moveTile))
            .Where(dict => dict.Value.BuildTile.Level < 3 && dict.Value.BuildTile.Level > -1)
            .OrderByDescending(dict => dict.Value.BuildTile.Level)
            .FirstOrDefault();

        if (kvp.Value == null)
        {
            throw new Exception();
        }

        var tile = kvp.Value.BuildTile;

        return tile;
    }

    public Order GetOrderBasedOnTiles(Tile moveTile, Tile buildTile)
    {
        var order = this.OrdersList.Single(dict => dict.Value.MoveTile.Equals(moveTile) 
                        && dict.Value.BuildTile.Equals(buildTile)).Key;

        return order;
    }
}

public class OrderTiles
{
    public Tile MoveTile { get; set; }
    public Tile BuildTile { get; set; }

    public OrderTiles(Tile move, Tile build)
    {
        this.MoveTile = move;
        this.BuildTile = build;
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

    public override string ToString()
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
        this.MoveDirection = (Direction)Enum.Parse(typeof(Direction), move);
        this.BuildDirection = (Direction)Enum.Parse(typeof(Direction), build);
    }   

    public override string ToString()
    {
        return $"{this.MoveDirection} {this.BuildDirection}";
    }
}

public class Position : IEquatable<Position>
{
    public Coordinate X { get; set; }
    public Coordinate Y { get; set; }

    public Position(int x, int y)
    {
        this.X = new Coordinate(x);
        this.Y = new Coordinate(y);
    }

    public Position(Coordinate x, Coordinate y)
    {
        this.X = x;
        this.Y = y;
    }

    public Position CalculateNewPositionOnDirection(Direction direction)
    {        
        switch (direction)
        {
            case Direction.N:
                var newPosN = new Position(this.X, this.Y.CalculateNewCoordinate(-1));
                return newPosN;
            case Direction.NE:
                var newPosNE = new Position(this.X.CalculateNewCoordinate(1), this.Y.CalculateNewCoordinate(-1));
                return newPosNE;
            case Direction.E:
                var newPosE = new Position(this.X.CalculateNewCoordinate(1), this.Y);
                return newPosE;
            case Direction.SE:
                var newPosSE = new Position(this.X.CalculateNewCoordinate(1), this.Y.CalculateNewCoordinate(1));
                return newPosSE;
            case Direction.S:
                var newPosS = new Position(this.X, this.Y.CalculateNewCoordinate(1));
                return newPosS;
            case Direction.SW:
                var newPosSW = new Position(this.X.CalculateNewCoordinate(-1), this.Y.CalculateNewCoordinate(1));
                return newPosSW;
            case Direction.W:
                var newPosW = new Position(this.X.CalculateNewCoordinate(-1), this.Y);
                return newPosW;
            case Direction.NW:
                var newPosNW = new Position(this.X.CalculateNewCoordinate(-1), this.Y.CalculateNewCoordinate(-1));
                return newPosNW;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool Equals(Position other)
    {
        if (this.X.Equals(other.X) && this.Y.Equals(other.Y))
        {
            return true;
        }

        return false;
    }
}

public class Coordinate : IEquatable<Coordinate>
{
    public int Number { get; set; }

    public Coordinate(int number)
    {
        if (number < 0)
        {
            throw new ArgumentOutOfRangeException($"{number}");
        }
        this.Number = number;
    }

    public Coordinate CalculateNewCoordinate(int move)
    {
        var newCoord = new Coordinate(this.Number + move);

        return newCoord;
    }

    public bool Equals(Coordinate other)
    {
        return this.Number == other.Number;
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