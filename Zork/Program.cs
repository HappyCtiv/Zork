using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Zork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string defaultGameFilename = @"Content\Zork.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);

            Game game = Game.Load(gameFilename);
            Console.WriteLine("Welcome to Zork!");
            game.Run();
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}