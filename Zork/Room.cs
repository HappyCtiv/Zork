using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Zork
{
    public class Room: IEquatable<Room>
    {
        [JsonProperty(Order = 1)]
        public string Name { get; private set; }
        [JsonProperty(Order = 2)]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "Neighbors", Order = 3)]
        private Dictionary<Directions, string> NeighborNames{ get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<Directions, Room> Neighbors { get; private set; }
        [JsonProperty]
        public List<Item> Inventory { get; set; }

        private string[] InventoryNames { get; set; }

        public Room(string name, string description, Dictionary<Directions, string> neighborNames, string[] inventroyNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
            //Inventory = inventory ?? new List<Item>(); // if (Inventory != null) { Inventory = inventory } else { Inventory = new List<Item>() }
            InventoryNames = inventroyNames ?? new string[0];

        }


        public static bool operator ==(Room lhs, Room rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            { 
                return true;
            }
            if (lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.Name == rhs.Name; // return string.Compare(lhs,Name, rhs.Name, ignoreCase: true) == 0

        }


        public static bool operator !=(Room lhs, Room rhs) => !(lhs == rhs);
        public override bool Equals(object obj) => obj is Room room ? this == room : false;
        public bool Equals(Room other) => this == other;
        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
        public void UpdateNeighbors(World world) => Neighbors = (from entry in NeighborNames
                                                                 let room = world.RoomsByName.GetValueOrDefault(entry.Value)
                                                                 where room != null
                                                                 select (Direction: entry.Key, Room: room))
                                                                 .ToDictionary(pair => pair.Direction, pair => pair.Room);
        public void UpdateInventory()
        {
            Inventory = new List<Item>();
            foreach (var inventoryName in Inventory)
            {
                Inventory.Add(World.ItemsByName[inventoryName]);
            }
            InventoryNames = null;
        }

    }
}
