using System.Drawing.Drawing2D;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;

namespace Xapier14.AdventOfCode
{
    /// <summary>
    /// A set of methods for authentication.
    /// </summary>
    public static class Auth
    {
        private static string _workDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Xapier14.AdventOfCode");
        private static string? _session;

        static Auth()
        {
            Directory.CreateDirectory(_workDir);
            var sessionPath = Path.Join(_workDir, "session");
            if (File.Exists(sessionPath))
            {
                _session = File.ReadAllText(sessionPath);
            }
        }

        /// <summary>
        /// Returns the currently used session.
        /// </summary>
        /// <returns>Returns the session or <c>null</c> if there is no session.</returns>
        public static string? GetSession() => _session;

        /// <summary>
        /// <para>Ensures that there is currently a session.</para>
        /// <para>Calls <c>InitiateAuth()</c> if there is no session.</para>
        /// </summary>
        public static void EnsureSession()
        {
            if (_session != null)
                return;
            InitiateAuth();
        }

        /// <summary>
        /// Sets the session via a token string.
        /// </summary>
        /// <param name="sessionToken">The session token to use.</param>
        public static void SetSessionToken(string sessionToken)
        {
            _session = sessionToken;
        }
        
        /// <summary>
        /// <para>Launches a monitored Chrome session to log into Advent of Code.</para>
        /// <para>Retrieves the session token once successfully logged in and uses it.</para>
        /// <remarks>Requires the latest version of Chrome.</remarks>
        /// </summary>
        /// <returns>The session token retrieved.</returns>
        public static string InitiateAuth()
        {
            var webDriverPath = Path.Join(_workDir, "webdriver");
            var manager = new DriverManager(webDriverPath);
            var driverConfig = new WebDriverManager.DriverConfigs.Impl.ChromeConfig();
            var chromeDriverInfo = new FileInfo(manager.SetUpDriver(driverConfig));
            var service = ChromeDriverService.CreateDefaultService(chromeDriverInfo.DirectoryName, chromeDriverInfo.Name);
            var chrome = new ChromeDriver(service);
            chrome.Navigate().GoToUrl($"https://adventofcode.com/{DateTime.Now.Year}/auth/login");
            Cookie? session = null;
            while (session == null)
            {
                session = chrome.Manage().Cookies.GetCookieNamed("session");
                Thread.Sleep(100);
            }
            _session = session.Value;
            chrome.Close();
            var sessionPath = Path.Join(_workDir, "session");
            File.WriteAllText(sessionPath, _session);
            chrome.Dispose();

            return _session;
        }
    }
}
