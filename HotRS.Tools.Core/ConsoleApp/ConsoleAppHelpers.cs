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

    /// <summary>
    /// Retrieves the first key pressed on the keyboard, Optionally exits the application if developer defined key (default is ESC) is pressed.
    /// </summary>
    /// <param name="quitIfKey">Developer defined key that will cause the application to exit. Default is null which bypasses this feature.</param>
    /// <param name="onlyNumeric">Only returns numeric values. Reprompts the user as needed.</param>
    /// <param name="promptUser">Informs the user of the defined "Quit" key, and reprompts if an alphabetic character is press when onlyNumeric is true. [default = true]</param>
    /// <returns>ConsoleKeyInfo for the key that was pressed.</returns>
    public static ConsoleKeyInfo GetSingleKeyInputOrQuit(ConsoleKey? quitIfKey = null, bool onlyNumeric = false, bool promptUser = true)
    {
        if (quitIfKey.HasValue && promptUser)
        {
            Console.WriteLine($"Press {quitIfKey} to quit.");
        }
        var done = false;
        var input = Console.ReadKey();
        while (!done)
        {
            if (quitIfKey.HasValue && input.Key == quitIfKey.Value)
                Environment.Exit(0);
            if (onlyNumeric)
            {
                if (char.IsDigit(input.KeyChar))
                {
                    done = true;
                }
                else
                {
                    if (promptUser)
                    {
                        Console.Write($"{Environment.NewLine}Numeric input required! Try Again.{Environment.NewLine}");
                    }
                    input = Console.ReadKey();
                }
            }
            else
            {
                done = true;
            }
        }
        return input;
    }

}

