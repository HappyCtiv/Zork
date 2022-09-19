using System;

namespace Zork
{
    internal class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Room(string name, string description = null) //Constractor, intialize the member of the class (same class name)
        {
            Name = name;
            Description = description;
        }

        public override string ToString() => Name;
    }
}
