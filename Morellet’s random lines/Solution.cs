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
    struct Point
    {
        public int x, y;

        public Point(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    struct Line
    {
        public int a, b, c;

        public Line(int A, int B, int C)
        {
            a = A;
            b = B;
            c = C;
        }
    }

    static int GCD(int x, int y)
    {
        int res;
        if (x == 0) res = y;
        else if (y == 0) res = x;
        else if (x == y) res = x;
        else if (x == 1 || y == 1) res = 1;
        else
        {
            bool xIsEven = x << 1 >> 1 == x;
            bool yIsEven = y << 1 >> 1 == y;
            if (xIsEven && yIsEven) res = GCD(x >> 1, y >> 1) << 1;
            else if (xIsEven) res = GCD(x >> 1, y);
            else if (yIsEven) res = GCD(x, y >> 1);
            else if (x > y) res = GCD((x - y) >> 1, y);
            else res = GCD(x, (y - x) >> 1);
        }
        return res;
    }

    static int GCD(int a, int b, int c)
    {
        return GCD(GCD(Math.Abs(a), Math.Abs(b)), Math.Abs(c));
    }

    static void ReadPoints(out Point A, out Point B)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        A.x = int.Parse(inputs[0]);
        A.y = int.Parse(inputs[1]);
        B.x = int.Parse(inputs[2]);
        B.y = int.Parse(inputs[3]);
    }

    static void ReadLines(ISet<Line> lines, int n)
    {
        for (int i = 0; i < n; i++)
        {
            var inputs = Console.ReadLine().Split(' ');
            int a = int.Parse(inputs[0]);
            int b = int.Parse(inputs[1]);
            int c = int.Parse(inputs[2]);
            int gcd = GCD(a, b, c);
            a /= gcd;
            b /= gcd;
            c /= gcd;
            var reverseLine = new Line(-a, -b, -c);
            if (!lines.Contains(reverseLine)) lines.Add(new Line(a, b, c));
        }
    }

    static bool IsPointOnLine(Point point, ISet<Line> lines)
    {
        bool res = false;
        foreach (var line in lines)
        {
            if (line.a * point.x + line.b * point.y + line.c == 0) res = true;
        }
        return res;
    }

    static bool HasSameSign(int a, int b)
    {
        return !(a > 0 ^ b > 0);
    }

    static bool PointAtLeft(Point point, Line line)
    {
        return line.a * point.x + line.b * point.y + line.c < 0;
    }

    static bool PointAtRight(Point point, Line line)
    {
        return line.a * point.x + line.b * point.y + line.c > 0;
    }

    static bool PointAtBottom(Point point, Line line)
    {
        bool t1 = line.a * point.x + line.b * point.y + line.c > 0;
        bool t2 = HasSameSign(line.a, line.b);
        return !(t1 ^ t2);
    }

    static bool PointAtTop(Point point, Line line)
    {
        bool t1 = line.a * point.x + line.b * point.y + line.c > 0;
        bool t2 = HasSameSign(line.a, line.b);
        return t1 ^ t2;
    }

    static int CountLinesBetween2Points(Point a, Point b, ISet<Line> lines)
    {
        int count = 0;
        var visited = new HashSet<Line>();
        if (a.x != b.x)
        {
            Point left = a.x < b.x ? a : b;
            Point right = a.x > b.x ? a : b;
            foreach (var line in lines)
            {
                if (line.a != 0 && PointAtLeft(left, line) && PointAtRight(right, line))
                {
                    ++count;
                    visited.Add(line);
                }
            }
        }
        if (a.y != b.y)
        {
            Point bottom = a.y < b.y ? a : b;
            Point top = a.y > b.y ? a : b;
            foreach (var line in lines.Except(visited))
            {
                if (line.b != 0 && PointAtBottom(bottom, line) && PointAtTop(top, line))
                    ++count;
            }
        }
        return count;
    }

    static bool HaveSameColour(Point a, Point b, ISet<Line> lines)
    {
        return CountLinesBetween2Points(a, b, lines) % 2 == 0;
    }

    static void Main(string[] args)
    {
        ReadPoints(out Point a, out Point b);
        int n = int.Parse(Console.ReadLine());
        var lines = new HashSet<Line>();
        ReadLines(lines, n);
        if (IsPointOnLine(a, lines) || IsPointOnLine(b, lines)) Console.WriteLine("ON A LINE");
        else if (HaveSameColour(a, b, lines)) Console.WriteLine("YES");
        else Console.WriteLine("NO");
        Console.Read();
    }
}