namespace HotRS.Tools.Core.ConsoleApp;

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
    /// <param name="seconds"></param>
    public static void CloseIfNotAborted(int seconds = 60)
    {
        System.Console.WriteLine($"Application will close in {seconds} seconds unless a key is pressed.");
        var waitTill = DateTime.Now.AddSeconds(seconds);
        var abortClose = false;
        while (DateTime.Now < waitTill)
        {
            if (System.Console.KeyAvailable)
            {
                abortClose = true;
                waitTill = DateTime.Now.AddSeconds(-1);
            }
            System.Threading.Thread.Sleep(1000);
            var remainingSeconds = (int)(waitTill - DateTime.Now).TotalSeconds;
            System.Console.Write("{0,4}\b\b\b\b", remainingSeconds);
        }
        if (abortClose)
        {
            System.Console.WriteLine($"Shutdown aborted.{Environment.NewLine}When you have finished reviewing the messages press 'ESC' to close the application.");
            while (System.Console.ReadKey().Key != ConsoleKey.Escape)
            {
            }
        }
    }
}
