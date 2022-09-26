using System;
using System.Collections.Generic;
using System.IO;

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
            string roomsFilename = args.Length > 0 ? args[0] : @"Content\Rooms.txt";
            InitializeRoomDescription(roomsFilename);


            Console.WriteLine("Welcome to Zork!");


            Room previousRoom = null;
            bool isRunning = true;

            while (isRunning)
            {

                Console.WriteLine(CurrentRoom);
                if (ReferenceEquals(previousRoom, CurrentRoom) == false) // similar to if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
                Console.Write(">");

                string inputString = Console.ReadLine().Trim();
                Commands command = ToCommand(inputString);

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

        private static void InitializeRoomDescription(string roomsFilename)
        {
            var roomMap = new Dictionary<string, Room>();
            foreach (Room room in _rooms)
            {
                roomMap[room.Name] = room; //roomMap.Add(room.Name, room)
            }

            string[] lines = File.ReadAllLines(roomsFilename);
            foreach (string line in lines)
            {
                const string fieldDelimiter = "##";
                const int exceptedFieldCount = 2;

                string[] fields = line.Split(fieldDelimiter);
                if (fields.Length != exceptedFieldCount)
                {
                    throw new InvalidDataException("Invalid record");
                }

                string name = fields[(int)Fields.Name];              // same as string name = fields[0];
                string description = fields[(int)Fields.Description];

                roomMap[name].Description = description;
            }

        }

        private static readonly Room[,] _rooms = {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };                                    //class member

        private static readonly List<Commands> _directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static (int Row, int Column) _location = (1, 1);

        private enum Fields
        {
            Name = 0,
            Description = 1
        }
    }
}