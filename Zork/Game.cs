using System;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    public class Game
    {
        public World World { get; private set; }
        [JsonIgnore]
        public Player Player { get; private set; }
        [JsonIgnore]
        private bool IsRunning { get; set; }
    

    public Game(World world, Player player)
    {
        World = world;
        Player = player;
    }

    public void Run()
    {
        IsRunning = true;
        Room previousRoom = null;
        while (IsRunning)
        {
            Console.WriteLine(Player.Location);
            if (ReferenceEquals(previousRoom, Player.Location) == false) // similar to if (previousRoom != CurrentRoom)
            {
                Console.WriteLine(Player.Location.Description);
                previousRoom = Player.Location;
            }
            Console.Write(">");

            string inputString = Console.ReadLine().Trim();
            Commands command = ToCommand(inputString);

            string outputString;

            switch (command)
            {
                case Commands.LOOK:
                    outputString = Player.Location.Description;
                    break;


                case Commands.NORTH:
                case Commands.SOUTH:
                case Commands.EAST:
                case Commands.WEST:
                    Directions direction = Enum.Parse<Directions>(command.ToString(), true);
                    if (Player.Move(direction))
                    {
                        outputString = $"You move {direction}.";
                    }
                    else
                    {
                        outputString = "The way is shut";
                    }
                    break;

                case Commands.QUIT:
                    outputString = "Thank you for playing!";
                    IsRunning = false; 
                    break;

                default:
                    outputString = "Unknown command.";
                    break;
            };

            Console.WriteLine(outputString);
        }

    }


    public static Game Load(string filename)
    {
        Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
        game.Player = game.World.SpawnPlayer();

        return game;
    }

    private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result: Commands.UNKNOWN;
    }
}
