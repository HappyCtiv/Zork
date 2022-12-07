﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            Player.Health = 100;
            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
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

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    break;

                case Commands.Reward:
                    Output.WriteLine("Congratz, You`ve earned a point!");
                    Player.Score++;
                    break;

                case Commands.Score:
                    Output.Write($"Your score would be {Player.Score}, in {Player.Moves} move(s).");
                    break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    break;

                case Commands.Health:
                    Output.WriteLine($"Current Health: {Player.Health}.");
                    break;

                case Commands.DamagePlayer: // DEBUG PURPOSES
                    if(string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("How much damage do you want to apply to your character?");
                    }
                    else
                    {
                        Player.Damage(int.Parse(subject));
                        Output.WriteLine($"Player`s health now is {Player.Health}");
                    }
                    break;

                case Commands.HealPlayer:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("How much health do you want to restore to your character?");
                    }
                    else
                    {
                        Player.Heal(int.Parse(subject));
                        Output.WriteLine($"Player`s health now is {Player.Health}");
                    }
                    break;

                case Commands.Use:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("What do you want to use?");
                    }
                    else
                    {
                        Use(subject);
                    }
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }
            
            if (Player.Health <= 0) //Not Sure. Decide on it later. Mb Add restart if you have time.
            {
                Output.WriteLine("You died.");
                IsRunning= false;
            }

            Output.WriteLine($"\n{Player.CurrentRoom}");
        }
        
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Output.WriteLine($"{itemToTake} was taken.");
            }
        }

        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Output.WriteLine($"{itemToDrop} was dropped.");
            }
        }

        private void Use(string itemName)
        {
            Item itemToUse = Player.Inventory.FirstOrDefault(item =>string.Compare(item.Name, itemName, ignoreCase:true) == 0);
            if (itemToUse == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else if (itemToUse.Name == "Potion") //DUMB SOLUTION
                                                 // (itemToUse.ItemConsumable == "Yes")
            {
                Player.RemoveItemFromInventory(itemToUse);
                Player.Heal(15);
                Output.WriteLine($"{itemToUse} was used.");
                Output.WriteLine($"Player`s health now is {Player.Health}");
            }

        }
        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}