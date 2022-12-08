using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        public EventHandler<Room> LocationChange;
        public EventHandler<int> MoveChange;
        public EventHandler<int> ScoreChange;
        public EventHandler<int> HealthChange;
        public Room CurrentRoom
        {
            get => _currentRoom;
            set 
            {
                if (_currentRoom != value) 
                { 
                    _currentRoom = value;
                    LocationChange?.Invoke(this, _currentRoom);
                }
            }
        }
        public int Moves
        {
            get => _moves;
            set 
            {
                if (_moves != value)
                { 
                    _moves = value;
                    MoveChange?.Invoke(this, _moves);
                }
            }

        }

        public int Score
        {
            get => _score;
            set
            {
                if (_score != value)
                { 
                    _score = value;
                    ScoreChange?.Invoke(this, _score);
                }
            }
 
        }

        public int Health
        {
            get => _health;
            set
            {
                if(_health != value)
                {
                    _health = value;
                    HealthChange?.Invoke(this, _health);
                }
            }
        }

        public IEnumerable<Item> Inventory => _inventory;

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
        }

        public int Damage(int dmg) //to be reworked
        {
            Health -= dmg;
            return Health;
        }

        public int Heal(int heal) //to be reworked
        {
            if((Health+heal) < 100)
            {
                Health += heal;
            }
            else
            {
                Health = 100;
            }
            return Health;
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }
            Moves++;
            return didMove;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
            Moves++;
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
            Moves++;
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
        private int _moves, _score;
        private int _health;
    }
}
