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
    static List<int> MakePrimes(int n)
    {
        var primes = new List<int>();
        BitArray a = new BitArray(n + 1, true);
        primes.Add(2);
        for (int i = 3; i <= n; i += 2)
        {
            if (a[i])
            {
                primes.Add(i);
                for (int j = i * i; j <= n && j > 0; j += i)
                    a[j] = false;
            }
        }
        return primes;
    }

    static int CountCoeff(ref int n, int prime)
    {
        int res = 0;
        while (n % prime == 0)
        {
            ++res;
            n /= prime;
        }
        return res;
    }

    static Dictionary<int, int> Factorize(int n)
    {
        var factors = new Dictionary<int, int>();
        var primes = MakePrimes(n / 2).Reverse<int>();
        foreach (var prime in primes)
        {
            int coeff = CountCoeff(ref n, prime);
            if (coeff != 0) factors.Add(prime, coeff);
        }
        return factors;
    }

    static bool IsSquareFree(Dictionary<int, int> factors)
    {
        bool res = factors.All(factor => factor.Value % 2 != 0);
        return res;
    }

    static bool IsCarmichael(int n)
    {
        var factors = Factorize(n);

        bool res = factors.Keys.Count > 1;
        if (!IsSquareFree(factors)) res = false;
        if (res)
        {
            int n1 = n - 1;
            res = factors.Keys.All(prime => n1 % (prime - 1) == 0);
        }

        return res;
    }

    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine(IsCarmichael(n) ? "YES" : "NO");
        Console.Read();
    }
}