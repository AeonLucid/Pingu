using System;

namespace Pingu
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var boot = new Boot();
            var server = boot.StartServer();

            server.KeepRunning.WaitOne();
            server.KeepLooping = false;
            server.Dispose();

            Console.ResetColor();
            Console.WriteLine("Exiting, press a key to close..");
            Console.ReadKey();
        }
    }
}
