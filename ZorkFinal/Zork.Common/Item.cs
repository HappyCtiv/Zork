namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }
        public string EffectDescription { get; }


        public bool Consumable { get; }
        
        public int Heal { get; }
        public int Attack { get; }


        public Item(string name, string lookDescription, string inventoryDescription, string effectDescription,bool itemConsumable, int heal, int attack)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Consumable = itemConsumable;
            Heal = heal;
            Attack = attack;
            EffectDescription = effectDescription;
        }

        public override string ToString() => Name;
    }
}