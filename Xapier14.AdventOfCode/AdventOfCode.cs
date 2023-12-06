using System.Net;

namespace Xapier14.AdventOfCode
{
    public static class AdventOfCode
    {
        private static readonly HttpClient _http = new();
        private static string _workDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Xapier14.AdventOfCode");
        private static string _cacheDir = Path.Join(_workDir, "cache");

        public static int Year { get; set; } = DateTime.Now.Year;
        public static int Day { get; set; } = 1;

        static AdventOfCode()
        {
            Auth.EnsureSession();
            _http.DefaultRequestVersion = HttpVersion.Version20;
            _http.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            _http.DefaultRequestHeaders.Add("Cookie", $"session={Auth.GetSession()}");
        }

        private static void ValidateYearAndDay()
        {
            Year = Math.Clamp(Year, 2015, DateTime.Now.Year);
            Day = Math.Clamp(Day, 1, 25);
        }

        public static void SetYearAndDay(int year, int day)
        {
            Year = year;
            Day = day;
            ValidateYearAndDay();
        }

        public static string GetInput(bool force = false)
        {
            var cachePath = Path.Combine(_cacheDir, $"{Year}/{Day}");
            Directory.CreateDirectory(cachePath);
            var inputFilePath = Path.Combine(cachePath, "input.txt");
            if (!force && File.Exists(inputFilePath))
                return File.ReadAllText(inputFilePath);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://adventofcode.com/{Year}/day/{Day}/input")
            };

            HttpResponseMessage response;
            do
            {
                response = _http.Send(request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error fetching input data.");
                    Console.WriteLine("Please login again.");
                    var session = Auth.InitiateAuth();
                    _http.DefaultRequestHeaders.Clear();
                    _http.DefaultRequestHeaders.Add("Cookie", $"session={session}");
                }
            } while (!response.IsSuccessStatusCode);

            var resultTask = response.Content.ReadAsStringAsync();
            resultTask.Wait();
            var result = resultTask.Result;
            File.WriteAllText(inputFilePath, result);

            return result;
        }

        public static string[] GetInputAsLines(bool force = false)
            => GetInput(force).Split('\n').SkipLast(1).ToArray();
    }
}
