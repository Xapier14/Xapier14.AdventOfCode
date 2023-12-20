using Xapier14.AdventOfCode;
using Xapier14.AdventOfCode.Caching;

var input = AdventOfCode.GetInputLines();

Console.WriteLine(Cache.Shared.Part1(input, 1));
Console.WriteLine(Cache.Shared.Part1(input, 1));
Console.WriteLine(Cache.Shared.Part1(input, 2));
Console.WriteLine(Cache.Shared.Part1(input, 2));
Console.WriteLine(Cache.Shared.Part1(input, 3));

public static class Solution
{
    [Cached]
    public static long Part1(string[] input, int v)
        => Random.Shared.Next();
}