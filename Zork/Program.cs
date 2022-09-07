using System;
using System.Runtime.CompilerServices;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            bool isRunning = true;
            Commands command = Commands.UNKNOWN;
            while (isRunning)
            {
                Console.Write($"{_rooms[_currentRoom]}\n> ");
                command = ToCommand(Console.ReadLine().Trim());
                //PascalCase
                //thisIsCamelCase
                //snake_case

                string outputString;
                
                switch (command)
                {
                    case Commands.LOOK:
                        outputString = "\t\rThis is an open field west of a white house, with a boarded front door.\r\n\rA rubber mat saying 'Welcome to Zork!' lies by the door.\"";
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:                 
                    case Commands.WEST:
                        if (Move(command))
                        {
                            outputString = $"You move {command}.";
                        }
                        else
                        {
                            outputString = "The way is shut";
                        }
                        break;

                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        isRunning = false;
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                };

                Console.WriteLine(outputString);
            }


        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool Move(Commands command)
        {
            bool didMove = false;

            switch (command)
            {
                case Commands.NORTH:
                case Commands.SOUTH:
                    break;
                case Commands.EAST when _currentRoom < _rooms.Length - 1:
                    _currentRoom++;
                    didMove = true;
                    break;

                case Commands.WEST when _currentRoom > 0:
                    _currentRoom--;
                    didMove = true;
                    break;

            }

            return didMove;
        }
        private static readonly string[] _rooms = { "Forrest", "West of House", "Behind House", "Clearing", "Canyon view" }; //class member
        private static int _currentRoom = 1; //Class member
        
    }
}
