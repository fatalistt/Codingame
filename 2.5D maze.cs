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
class Solution
{
    const bool OVER = true;
    const bool UNDER = false;
    const bool HORIZONTAL = true;
    const bool VERTICAL = false;

    enum Tile
    {
        Floor = '.',
        ShortWall = '+', HighWall = '#',
        VerticalSlope = '|', HorizontalSlope = '-',
        Bridge = 'X', Tunnel='O'
    }

    class Point
    {
        public int x, y, length;
        public bool onTop;
        public Point parent;

        internal Point(int X, int Y, bool OnTop)
        {
            x = X;
            y = Y;
            onTop = OnTop;
            length = int.MaxValue;
            parent = null;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", x, y, onTop ? "top" : "bottom");
        }
    }

    static Point[,,] MakeGoodMaze(Tile[,] maze)
    {
        var goodMaze = new Point[maze.GetLength(0), maze.GetLength(1), 2];
        for (int y = 0; y < maze.GetLength(0); ++y)
            for (int x = 0; x < maze.GetLength(1); ++x)
            {
                if (maze[y, x] == Tile.Floor || maze[y, x] == Tile.VerticalSlope || maze[y, x] == Tile.HorizontalSlope ||
                    maze[y, x] == Tile.Bridge || maze[y, x] == Tile.Tunnel)
                {
                    goodMaze[y, x, 0] = new Point(x, y, UNDER);
                }
                if (maze[y, x] == Tile.ShortWall || maze[y, x] == Tile.VerticalSlope || maze[y, x] == Tile.HorizontalSlope ||
                    maze[y, x] == Tile.Bridge)
                {
                    goodMaze[y, x, 1] = new Point(x, y, OVER);
                }
            }
        return goodMaze;
    }

    static void UpdateLengthAndEnqueue(Point current, Point next, Queue<Point> toVisit)
    {
        if (current.length + 1 < next.length)
        {
            next.length = current.length + 1;
            next.parent = current;
        }
        toVisit.Enqueue(next);
    }

    static bool CanGoOver(Tile next, bool onTop, bool horisontal)
    {
        bool res;
        if (next == Tile.Floor) res = false;
        else if (next == Tile.ShortWall) res = onTop;
        else if (next == Tile.VerticalSlope) res = horisontal ? false : !onTop;
        else if (next == Tile.HorizontalSlope) res = horisontal ? !onTop : false;
        else if (next == Tile.HighWall) res = false;
        else if (next == Tile.Bridge) res = onTop;
        else if (next == Tile.Tunnel) res = false;
        else throw new ArgumentException();
        //Console.Error.WriteLine($"Over) ({next}) => {res}");
        return res;
    }

    static bool CanGoUnder(Tile next, bool onTop, bool horisontal)
    {
        bool res;
        if (next == Tile.Floor) res = !onTop;
        else if (next == Tile.ShortWall) res = false;
        else if (next == Tile.VerticalSlope) res = horisontal ? false :onTop;
        else if (next == Tile.HorizontalSlope) res = horisontal ? onTop : false;
        else if (next == Tile.HighWall) res = false;
        else if (next == Tile.Bridge) res = !onTop;
        else if (next == Tile.Tunnel) res = !onTop;
        else throw new ArgumentException();
        //Console.Error.WriteLine($"Under) ({next}) => {res}");
        return res;
    }

    static bool CanGoLeft(Tile[,] maze, Point point, bool over)
    {
        //Console.Error.Write($"\t=> ({point.x - 1}, {point.y}) (Left");
        Tile next = maze[point.y, point.x - 1];
        return over ? CanGoOver(next, point.onTop, HORIZONTAL) : CanGoUnder(next, point.onTop, HORIZONTAL);
    }

    static bool CanGoTop(Tile[,] maze, Point point, bool over)
    {
        //Console.Error.Write($"\t=> ({point.x}, {point.y - 1}) (Top");
        Tile next = maze[point.y - 1, point.x];
        return over ? CanGoOver(next, point.onTop, VERTICAL) : CanGoUnder(next, point.onTop, VERTICAL);
    }

    static bool CanGoRight(Tile[,] maze, Point point, bool over)
    {
        //Console.Error.Write($"\t=> ({point.x + 1}, {point.y}) (Right");
        Tile next = maze[point.y, point.x + 1];
        return over ? CanGoOver(next, point.onTop, HORIZONTAL) : CanGoUnder(next, point.onTop, HORIZONTAL);
    }

    static bool CanGoBottom(Tile[,] maze, Point point, bool over)
    {
        //Console.Error.Write($"\t=> ({point.x}, {point.y + 1}) (Bottom");
        Tile next = maze[point.y + 1, point.x];
        return over ? CanGoOver(next, point.onTop, VERTICAL) : CanGoUnder(next, point.onTop, VERTICAL);
    }

    static int FindLengthOfShortestPath(Tile[,] tileMaze, Point[,,] maze, Point start, Point end)
    {
        var visited = new HashSet<Point>();
        var toVisit = new Queue<Point>();
        toVisit.Enqueue(start);
        start.length = 0;
        while (toVisit.Count != 0)
        {
            var current = toVisit.Dequeue();
            visited.Add(current);
            //Console.Error.WriteLine(current);
            if (!visited.Contains(maze[current.y, current.x - 1, 1]) && CanGoLeft(tileMaze, current, OVER)) UpdateLengthAndEnqueue(current, maze[current.y, current.x - 1, 1], toVisit);
            if (!visited.Contains(maze[current.y, current.x - 1, 0]) && CanGoLeft(tileMaze, current, UNDER)) UpdateLengthAndEnqueue(current, maze[current.y, current.x - 1, 0], toVisit);
            if (!visited.Contains(maze[current.y - 1, current.x, 1]) && CanGoTop(tileMaze, current, OVER)) UpdateLengthAndEnqueue(current, maze[current.y - 1, current.x, 1], toVisit);
            if (!visited.Contains(maze[current.y - 1, current.x, 0]) && CanGoTop(tileMaze, current, UNDER)) UpdateLengthAndEnqueue(current, maze[current.y - 1, current.x, 0], toVisit);
            if (!visited.Contains(maze[current.y, current.x + 1, 1]) && CanGoRight(tileMaze, current, OVER)) UpdateLengthAndEnqueue(current, maze[current.y, current.x + 1, 1], toVisit);
            if (!visited.Contains(maze[current.y, current.x + 1, 0]) && CanGoRight(tileMaze, current, UNDER)) UpdateLengthAndEnqueue(current, maze[current.y, current.x + 1, 0], toVisit);
            if (!visited.Contains(maze[current.y + 1, current.x, 1]) && CanGoBottom(tileMaze, current, OVER)) UpdateLengthAndEnqueue(current, maze[current.y + 1, current.x, 1], toVisit);
            if (!visited.Contains(maze[current.y + 1, current.x, 0]) && CanGoBottom(tileMaze, current, UNDER)) UpdateLengthAndEnqueue(current, maze[current.y + 1, current.x, 0], toVisit);
        }
        return end.length;
    }

    static void PrintPathToError(Point end)
    {
        if (end.parent != null) PrintPathToError(end.parent);
        Console.Error.WriteLine(end);
    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int starty = int.Parse(inputs[0]);
        int startx = int.Parse(inputs[1]);
        inputs = Console.ReadLine().Split(' ');
        int endy = int.Parse(inputs[0]);
        int endx = int.Parse(inputs[1]);
        inputs = Console.ReadLine().Split(' ');
        int h = int.Parse(inputs[0]);
        int w = int.Parse(inputs[1]);
        var maze = new Tile[h, w];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; ++j)
                maze[i, j] = (Tile)Console.Read();
            Console.ReadLine();
        }

        var goodMaze = MakeGoodMaze(maze);
        Point start = goodMaze[starty, startx, 0];
        Point end = goodMaze[endy, endx, 0];

        int answer = FindLengthOfShortestPath(maze, goodMaze, start, end);
        PrintPathToError(end);
        Console.WriteLine(answer);
    }
}
