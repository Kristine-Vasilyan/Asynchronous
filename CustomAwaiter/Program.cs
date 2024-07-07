using CustomAwaiter;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($"Start time: {DateTime.Now}");

        await new Delay(3000); 

        Console.WriteLine($"End Time: {DateTime.Now}");
    }
}