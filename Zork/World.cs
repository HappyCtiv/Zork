using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Zork
{
    public class World
    {
        public HashSet<Room> Rooms{ get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<string, Room> RoomsByName => mRoomsByName;
        [JsonIgnore]
        public Dictionary<string, Item> ItemsByName { get; }

        public Player SpawnPlayer() => new Player(this, StartingLocation);

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            mRoomsByName = Rooms.ToDictionary(room => room.Name, room => room);
            
            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
            }
        }

        public World(Room[] rooms, Item[] items) // finish Constructor
        {

        }

        [JsonProperty]
        private string StartingLocation { get; set; }

        private Dictionary<string, Room> mRoomsByName;
    }
}