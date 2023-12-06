using System.Drawing.Drawing2D;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;

namespace Xapier14.AdventOfCode
{
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

        public static string? GetSession() => _session;

        public static void EnsureSession()
        {
            if (_session != null)
                return;
            InitiateAuth();
        }
        
        public static string InitiateAuth()
        {
            var webDriverPath = Path.Join(_workDir, "webdriver");
            var manager = new DriverManager(webDriverPath);
            var driverConfig = new WebDriverManager.DriverConfigs.Impl.ChromeConfig();
            var chromeDriverInfo = new FileInfo(manager.SetUpDriver(driverConfig));
            var service = ChromeDriverService.CreateDefaultService(chromeDriverInfo.DirectoryName, chromeDriverInfo.Name);
            var chrome = new ChromeDriver(service);
            chrome.Navigate().GoToUrl("https://adventofcode.com/2023/auth/login");
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
