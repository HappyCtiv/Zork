using System;

namespace Zork
{
    internal class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Room(string name) //Constractor, intialize the member of the class
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
