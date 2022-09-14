using System;
using System.Collections.Generic;

namespace Zork
{
    internal class Program
    {
        private static Room CurrentRoom //property,  private static string GetCurrentRoom() is method. Functionally equivalent
        {
            get
            {
                return _rooms[_location.Row, _location.Column];
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Zork!");

            InitializeRoomDescription();

            bool isRunning = true;
            Commands command = Commands.UNKNOWN;
            while (isRunning)
            {
                Console.Write($"{CurrentRoom}\n> ");
                command = ToCommand(Console.ReadLine().Trim());
                //PascalCase
                //thisIsCamelCase
                //snake_case

                string outputString;

                switch (command)
                {
                    case Commands.LOOK:
                        outputString = CurrentRoom.Description;
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
        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool didMove = false;

            switch (command)
            {
                case Commands.NORTH when _location.Row < _rooms.GetLength(0) - 1: // _rooms.GetLength(0) - number of rows, _rooms.GetLength(1) - number of columns
                    _location.Row++;
                    didMove = true;
                    break;

                case Commands.SOUTH when _location.Row > 0:
                    _location.Row--;
                    didMove = true;
                    break;

                case Commands.EAST when _location.Column < _rooms.GetLength(1) - 1:
                    _location.Column++;
                    didMove = true;
                    break;

                case Commands.WEST when _location.Column > 0:
                    _location.Column--;
                    didMove = true;
                    break;

            }

            return didMove;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool IsDirection(Commands command) => _directions.Contains(command);

        private static void InitializeRoomDescription()
        {
            _rooms[0, 0].Description = "You are on a rock-strewn trail.";
            _rooms[0, 1].Description = "You are facing the south side of a white house. There is no doorr here, and all the windows are barred.";
            _rooms[0, 2].Description = "You are at the top of the Great Canyon on its south wall.";

            _rooms[1, 0].Description = "This is a forest, with trees in all directions around you.";
            _rooms[1, 1].Description = "This is an open field west of a white house, with a boarded front door.";
            _rooms[1, 2].Description = "You are begind the white house. In one corner of the house there is a small window which is slightly ajar.";

            _rooms[2, 0].Description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight.";
            _rooms[2, 1].Description = "You are facing the north side of a white house. There is no doorr here, and all the windows are barred.";
            _rooms[2, 2].Description = "You are in a clearing, with a forest surrounding you on the west and south.";
        }

        private static readonly Room[,] _rooms = {
            { new Room("Rocky Trail"), new Room("South Of House"), new Room("Canyon View") },
            { new Room("Forrest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };                                    //class member

        private static readonly List<Commands> _directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static (int Row, int Column) _location = (1,1);
    }
}
