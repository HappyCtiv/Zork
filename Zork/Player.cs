using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zork
{
    public class Player
    {
        public World World { get; }

        [JsonIgnore]
        public Room Location { get; private set; }

        [JsonIgnore]
        public string LocationName
        {
            get 
            {
                return Location?.Name;
            }
            set
            {
                Location = World?.RoomsByName.GetValueOrDefault(value);
            }
        }

        public List<Item> Inventory { get; }


        public Player(World world, string startingLocation)
        {
            World = world;
            LocationName = startingLocation;
        }

        public bool Move(Directions direction)
        {
            bool didMove = Location.Neighbors.TryGetValue(direction, out Room destination);
            if (didMove)
            {
                Location = destination;
            }

            return didMove;
        }
    }
}