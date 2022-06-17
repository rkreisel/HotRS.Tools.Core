namespace HotRS.Tools.Core.ConsoleApp;

[ExcludeFromCodeCoverage]
public static class ConsoleAppHelpers
{
    /// <summary>
    /// Pauses execution of the application for n seconds (default 60), to allow the user to abort the shutdown.
    /// Usage:
    ///     CloseIfNotAborted();
    ///     CloseIfNotAborted(n);
    ///     
    /// Execution of the code will continue after the method completes.
    /// </summary>
    /// <param name="seconds">The number of seconds to wait</param>
    /// <param name="targetKey">Optional. Wait for specific key. Default = ESC</param>
    public static void CloseIfNotAborted(int seconds = 60, ConsoleKey targetKey = ConsoleKey.Escape)
    {
        Console.WriteLine($"Application will close in {seconds} seconds unless {targetKey} is pressed.");
        var waitTill = DateTime.Now.AddSeconds(seconds);
        var abortClose = false;
        while (DateTime.Now < waitTill)
        {
            if (Console.KeyAvailable)
            {
                abortClose = Console.ReadKey(true).Key.Equals(targetKey);
                if (abortClose)
                    waitTill = DateTime.Now.AddSeconds(-1);
            }
            Thread.Sleep(250);
            var remainingSeconds = (int)(waitTill - DateTime.Now).TotalSeconds;
            Console.Write("{0,4}\b\b\b\b", remainingSeconds);
        }
        if (abortClose)
        {
            Console.WriteLine($"Shutdown aborted.{Environment.NewLine}When you have finished reviewing the messages press 'ESC' to close the application.");
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
            }
        }
    }
}

