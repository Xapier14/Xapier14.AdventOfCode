using Xapier14.AdventOfCode;
AdventOfCode.SetYearAndDay(2022, 16);

var input = AdventOfCode.GetInputLines();
Utility.Assert(Part1, input, 2);
Utility.Assert(Part2, input[0], 3);

const long val1 = 5, val2 = 30;
Console.WriteLine("Hash1: {0}, Hash2: {1}",
    Utility.Hash(val1, val2),
    Utility.Hash(val2, val1));
Console.WriteLine("ComHash1: {0}, ComHash2: {1}",
    Utility.CommutativeHash(val1, val2),
    Utility.CommutativeHash(val2, val1));
(long X, long Y) coord1 = (1, 2), coord2 = (3, 4);
Console.WriteLine("Hash1: {0}, Hash2: {1}",
    Utility.Hash(coord1),
    Utility.Hash(coord2));
Console.WriteLine("ComHash1: {0}, ComHash2: {1}",
    Utility.CommutativeHash(coord1, coord2),
    Utility.CommutativeHash(coord2, coord1));
return;

int Part1(string[] lines)
{
    return 2;
}
int Part2(string line)
{
    return 3;
}