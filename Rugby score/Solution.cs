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
        int N = int.Parse(Console.ReadLine());
        int[,,] scores = new int[N / 5 + 1, N / 2 + 1, N / 3 + 1];
        scores[0, 0, 0] = 0;
        for (int Try = 0; Try <= N / 5; ++Try)
        {
            if (Try != 0) scores[Try, 0, 0] = scores[Try - 1, 0, 0] + 5;
            for (int Trans = 0; Trans <= N / 2; ++Trans)
            {
                if (Trans != 0) scores[Try, Trans, 0] = scores[Try, Trans - 1, 0] + 2;
                for (int Drop = 0; Drop <= N / 3; ++Drop)
                {
                    if (Drop != 0) scores[Try, Trans, Drop] = scores[Try, Trans, Drop - 1] + 3;
                    if (scores[Try, Trans, Drop] == N) Console.WriteLine($"{Try} {Trans} {Drop}");
                }
            }
        }
        Console.Read();
    }
}