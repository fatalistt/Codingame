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
    class HorseComparerByVelocityFirst : IComparer<Horse>
    {
        public int Compare(Horse x, Horse y)
        {
            if (x.velocity == y.velocity) return x.elegance.CompareTo(y.elegance);
            return x.velocity.CompareTo(y.velocity);
        }
    }

    class HorseComparerByEleganceFirst : IComparer<Horse>
    {
        public int Compare(Horse x, Horse y)
        {
            if (x.elegance == y.elegance) return x.velocity.CompareTo(y.velocity);
            return x.elegance.CompareTo(y.elegance);
        }
    }

    struct Horse
    {
        public int velocity;
        public int elegance;

        public int Total => Math.Abs(velocity - elegance);
        
        public Horse(int Velocity, int Elegance)
        {
            velocity = Velocity;
            elegance = Elegance;
        }
    }

    static int FindMinDistance(IEnumerable<Horse> horses)
    {
        Horse prev = horses.First();
        int curMin = int.MaxValue;
        foreach (var horse in horses.Skip(1))
        {

            int distance = Math.Abs(horse.velocity - prev.velocity) + Math.Abs(horse.elegance - prev.elegance);
            if (distance < curMin) curMin = distance;
            prev = horse;
        }
        return curMin;
    }

    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        var horses1 = new SortedSet<Horse>(new HorseComparerByVelocityFirst());
        var horses2 = new SortedSet<Horse>(new HorseComparerByEleganceFirst());
        int curMin = int.MaxValue;
        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int V = int.Parse(inputs[0]);
            int E = int.Parse(inputs[1]);
            var h = new Horse(V, E);
            if (!horses1.Add(h)) curMin = 0;
            if (!horses2.Add(h)) curMin = 0;
        }

        curMin = Math.Min(FindMinDistance(horses1), curMin);
        curMin = Math.Min(FindMinDistance(horses2), curMin);

        Console.WriteLine(horses1.Count == 1 ? 0 : curMin);
        Console.Read();
    }
}