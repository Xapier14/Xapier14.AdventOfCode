using System.Net;
using System.Web;
using HtmlAgilityPack;

namespace Xapier14.AdventOfCode
{
    /// <summary>
    /// A set of methods for interacting with Advent of Code.
    /// </summary>
    public static class AdventOfCode
    {
        private static readonly HttpClient _http = new(new HttpClientHandler
        {
            AllowAutoRedirect = false
        });
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

        /// <summary>
        /// Sets the year and day to interface with Advent of Code.
        /// </summary>
        /// <param name="year">The year to use. (Must be greater than or equal 2015)</param>
        /// <param name="day">The day to use. (Must be in the range of 1-25)</param>
        public static void SetYearAndDay(int year, int day)
        {
            Year = year;
            Day = day;
            ValidateYearAndDay();
        }

        /// <summary>
        /// <para>Retrieves the input from Advent of Code using the session provided and caches it.</para>
        /// <para>Returns the cached version of the input as text unless <paramref name="invalidateCache"/> is <c>true</c>.</para>
        /// </summary>
        /// <param name="invalidateCache">Forces a download of the input from Advent of Code.</param>
        /// <returns>The input as text.</returns>
        public static string GetInputText(bool invalidateCache = false)
        {
            var cachePath = Path.Combine(_cacheDir, $"{Year}/{Day}");
            Directory.CreateDirectory(cachePath);
            var inputFilePath = Path.Combine(cachePath, "input.txt");
            if (!invalidateCache && File.Exists(inputFilePath))
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
        
        /// <summary>
        /// <para>Retrieves the input from Advent of Code using the session provided and caches it.</para>
        /// <para>Returns the cached version of the input as lines unless <paramref name="invalidateCache"/> is <c>true</c>.</para>
        /// </summary>
        /// <param name="invalidateCache">Forces a download of the input from Advent of Code.</param>
        /// <returns>The input as lines of text.</returns>
        public static string[] GetInputLines(bool invalidateCache = false)
            => GetInputText(invalidateCache).Split('\n').SkipLast(1).ToArray();

        /// <summary>
        /// <para>Submits an answer to Advent of Code.</para>
        /// <remarks>
        /// Warning, this method does not implement a wait timer for rate limiting.
        /// Improper use of this method may be considered abusive.
        /// Please use it responsibly.
        /// </remarks>
        /// </summary>
        /// <typeparam name="T">The data type of the answer.</typeparam>
        /// <param name="level">The 'level' corresponding to what part of the day it is. (Part 1 or Part 2)</param>
        /// <param name="answer">The answer to be submitted.</param>
        public static void Submit<T>(byte level, T answer)
        {
            var content = new Dictionary<string, string>()
            {
                { "level", $"{level}" },
                { "answer", $"{answer}" }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://adventofcode.com/{Year}/day/{Day}/answer"),
                Content = new FormUrlEncodedContent(content)
            };
            request.Headers.Add("Origin", "https://adventofcode.com");
            request.Headers.Add("Referrer", $"https://adventofcode.com/{Year}/day/{Day}");

            HttpResponseMessage response;
            do
            {
                char key;
                do
                {
                    Console.WriteLine("You are submitting \"{0}\" as your answer for Part {1} of Day {2}, Year {3}.",
                        answer,
                        level,
                        Day,
                        Year);
                    Console.Write("Do you want to continue? (y/n): "); 
                    key = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    if (key != 'n')
                        continue;
                    Console.WriteLine("Cancelled submission.");
                    return;
                } while (key != 'y');
                
                Console.WriteLine("Sending answer...");
                response = _http.Send(request);
                if (response.IsSuccessStatusCode)
                    continue;
                Console.WriteLine("Error submitting answer.");
                Console.WriteLine("Please login again.");
                var session = Auth.InitiateAuth();
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Cookie", $"session={session}");
            } while (!response.IsSuccessStatusCode);

            var task = response.Content.ReadAsStringAsync();
            task.Wait();

            var html = new HtmlDocument();
            html.LoadHtml(task.Result);
            var message = html.DocumentNode.SelectSingleNode("//article/p").InnerText;

            Console.WriteLine("Result for Part {0} submission:", level);
            Console.WriteLine(message);
        }

        /// <summary>
        /// <para>Submits an answer to Advent of Code as Part 1.</para>
        /// <remarks>
        /// Warning, this method does not implement a wait timer for rate limiting.
        /// Improper use of this method may be considered abusive.
        /// Please use it responsibly.
        /// </remarks>
        /// </summary>
        /// <typeparam name="T">The data type of the answer.</typeparam>
        /// <param name="answer">The answer to be submitted.</param>
        public static void SubmitPart1<T>(T answer)
            => Submit(1, answer);
        
        /// <summary>
        /// <para>Submits an answer to Advent of Code as Part 2.</para>
        /// <remarks>
        /// Warning, this method does not implement a wait timer for rate limiting.
        /// Improper use of this method may be considered abusive.
        /// Please use it responsibly.
        /// </remarks>
        /// </summary>
        /// <typeparam name="T">The data type of the answer.</typeparam>
        /// <param name="answer">The answer to be submitted.</param>
        public static void SubmitPart2<T>(T answer)
            => Submit(2, answer);
    }
}
