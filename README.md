# Xapier14.AdventOfCode
A simple Advent of Code API for .NET.
Provides authentication, input retrieval, answer submission and assertion.

## Installation
1. Add the package to your project.
   
   ```
   dotnet add package Xapier14.AdventOfCode
   ```
3. Have the latest version of Chrome. (Only needed for authenticating.)

   *Note: Authentication is only performed on first run and authentication errors.*

### Usage
```csharp
using Xapier14.AdventOfCode;
AdventOfCode.SetYearAndDay(2023, 1);

// Input Retrieval
var text = AdventOfCode.GetInputText();
var lines = AdventOfCode.GetInputLines();

// Testing
var input1 =
    """
    Some sample input.
    """.Split(Environment.NewLine);
var control1 = 1;
Utility.Assert(Part1, input1, control1);
var input2 = "Another test";
var control2 = 2;
Utility.Assert(Part2, input2, control2);

// Solve
var part1 = Part1(lines);
Console.WriteLine("Part 1: {0}", Part1(lines));
var part2 = Part2(text);
Console.WriteLine("Part 2: {0}", Part2(text));

// Submit
AdventOfCode.SubmitPart1(part1);
AdventOfCode.SubmitPart2(part2);
return;

int Part1(string[] lines)
    => return 1;

int Part2(string text)
    => return 2;
```
