namespace GitManager.Helpers;

public static class ConsoleHelper
{
    public static void WaitForAction()
    {
        Console.WriteLine("Press key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}