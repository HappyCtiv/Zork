namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }
        
        public bool Consumable { get; }
        
        public int Heal { get; }


        public Item(string name, string lookDescription, string inventoryDescription, bool itemConsumable, int heal)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Consumable = itemConsumable;
            Heal = heal;
        }

        public override string ToString() => Name;
    }
}