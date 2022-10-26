using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zork
{
    public class Item
    {
        public string Name { get; }
        public string Description { get; }

        public Item (string name, string description)
        {
            Name = name;
            Description = description;
        }


    }
}
