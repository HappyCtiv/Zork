using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public IOutputService Output { get; private set; }
        public IInputService Input { get; private set; } 

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IOutputService output)
        {
            Output = output;

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.CurrentRoom);   
                if (previousRoom != Player.CurrentRoom)
                {
                    Output.WriteLine(Player.CurrentRoom.Description);

                    foreach (Item item in Player.CurrentRoom.Inventory)
                    {
                        Output.WriteLine(item.Description);
                    }

                    previousRoom = Player.CurrentRoom;
                }

                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                char  separator = ' ';
                string[] commandTokens = inputString.Split(separator);
                
                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
                }
                else if (commandTokens.Length == 1)
                {
                    verb = commandTokens[0];
                }
                else
                {
                    verb = commandTokens[0];
                    subject = commandTokens[1];
                }

                Commands command = ToCommand(verb);

                string outputString;
                List<string> outputSubjects = new List<string>();

                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.Look:
                        outputString = Player.CurrentRoom.Description;
                        foreach (Item item in Player.CurrentRoom.Inventory)
                        {
                            outputSubjects.Add(item.Description);
                        }
                        break;
                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.Take:
                        switch (commandTokens.Length)
                        {
                            case 1:
                                outputString = "What do you want to take?";
                                break;
                            default:
                                if (World.ItemsByName.TryGetValue(commandTokens[1].ToUpper(), out Item item)) // if we add enum instead of ifs we can go with --  
                                {                                                     //-- if (World.ItemsByName.TryGetValue(commandTokens[(int)ToCommand(subject)].ToUpper(), out Item item))
                                    if (Player.CurrentRoom.Inventory.Contains(item)) 
                                    {
                                        outputString = $"You took {item.Name}";
                                        Player.Take(item);
                                        Player.CurrentRoom.deleteItem(item);
                                    }
                                    else if (Player.Inventory.Contains(item))
                                    {
                                        outputString = "You already have that.";
                                    }
                                    else
                                    {
                                        outputString = "You can`t see any such thing.";
                                    }
                                }
                                else
                                {
                                    outputString = "You can`t see any such thing.";
                                }
                                break;
                        }
                        break;

                    case Commands.Drop:
                        switch (commandTokens.Length)
                        {
                            case 1:
                                outputString = "What do you want to drop?";
                                break;
                            default:
                                if (World.ItemsByName.TryGetValue(commandTokens[1].ToUpper(), out Item item))
                                {
                                    if (Player.Inventory.Contains(item))
                                    {
                                        outputString = $"Dropped {item.Name}.";
                                        Player.Drop(item);
                                        Player.CurrentRoom.addItem(item);
                                    }
                                    else if (Player.CurrentRoom.Inventory.Contains(item))
                                    {
                                        outputString = $"{item.Name} is already here.";
                                    }
                                    else
                                    {
                                        outputString = "You do not have any such thing";
                                    }
                                }
                                else
                                {
                                    outputString = "You do not have any such thing.";
                                }
                                break;
                        }
                        break;
 
                    case Commands.Inventory:
                        if (Player.Inventory.Count > 0)
                        {
                            outputString = "Your inventory contains: ";
                            foreach (Item item in Player.Inventory)
                            {
                                outputSubjects.Add(item.Name); // Outputs Items before everything.
                            }
                        }
                        else
                        {
                            outputString = "You are empty handed.";
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Output.WriteLine(outputString);

                foreach (string outputS in outputSubjects)
                {
                    Output.WriteLine(outputS);
                }

                Output.WriteLine("");
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}
