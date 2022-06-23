using HotRS.Tools.Core.ConsoleApp;
Console.WriteLine("Hello World!");

#region GetSingleKeyInputOrQuit tests
Console.WriteLine("For these tests, hit any key when prompted, (or the defined key to exit).");
Console.WriteLine("Press any regular keyboard character.");
var result = ConsoleAppHelpers.GetSingleKeyInputOrQuit();
Console.WriteLine("Press any regular keyboard character other than ESC");
result = ConsoleAppHelpers.GetSingleKeyInputOrQuit(quitIfKey: ConsoleKey.Escape);
Console.WriteLine($"{Environment.NewLine}Returned: {result.Key}");
Console.WriteLine("Try alpabetic characters first. It should not return until you hit a number.");
result = ConsoleAppHelpers.GetSingleKeyInputOrQuit(onlyNumeric: true);
Console.WriteLine($"{Environment.NewLine}Returned: {result.Key}");
Console.WriteLine("Try alpabetic characters first. It should not return until you hit a number. But you shoud NOT be  prompted.");
result = ConsoleAppHelpers.GetSingleKeyInputOrQuit(onlyNumeric: true, promptUser: false);
Console.WriteLine($"{Environment.NewLine}Returned: {result.Key}");
Console.WriteLine();
#endregion

#region CloseIfNotAborted tests

Console.WriteLine("For these tests, you need to press the key indicated in the console output");
ConsoleAppHelpers.CloseIfNotAborted();
Console.WriteLine();
ConsoleAppHelpers.CloseIfNotAborted(seconds: 10);
Console.WriteLine();
ConsoleAppHelpers.CloseIfNotAborted(targetKey: ConsoleKey.X);
Console.WriteLine();
ConsoleAppHelpers.CloseIfNotAborted(seconds: 10, targetKey: ConsoleKey.X);
Console.WriteLine("Goodbye World!");
#endregion

