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
    static void Main(string[] args)
    {
        string sequence = Console.ReadLine();
        var tags = new SortedDictionary<char, Dictionary<int, int>>();
        var stack = new Stack<char>();
        for (int i = 0; i < sequence.Length; ++i)
        {
            char c = sequence[i];
            if (c != '-')
            {
                stack.Push(c);
                if (!tags.ContainsKey(c)) tags.Add(c, new Dictionary<int, int>());
                int depth = stack.Count;
                if (tags[c].ContainsKey(depth)) ++tags[c][depth];
                else tags[c].Add(depth, 1);
            }
            else
            {
                stack.Pop();
                ++i;
            }
        }
        char maxChar = char.MinValue;
        double maxWeight = double.MinValue;
        foreach (var tag in tags)
        {
            double weight = 0;
            foreach (var depthCount in tag.Value)
            {
                weight += (double)depthCount.Value / depthCount.Key;
            }
            if (weight > maxWeight)
            {
                maxChar = tag.Key;
                maxWeight = weight;
            }
        }
        Console.WriteLine(maxChar);
        Console.Read();
    }
}