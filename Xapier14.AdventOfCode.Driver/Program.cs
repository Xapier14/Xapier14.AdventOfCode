using Xapier14.AdventOfCode;
AdventOfCode.SetYearAndDay(2022, 16);

var input = AdventOfCode.GetInputAsLines();
AdventOfCode.Assert(Part1, input, 2);
AdventOfCode.Assert(Part2, input[0], 3);

foreach (var line in input)
    Console.WriteLine(line);

AdventOfCode.SubmitPart1(2);
return;

int Part1(string[] lines)
{
    return 2;
}
int Part2(string line)
{
    return 3;
}