namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }
        
        public string ItemConsumable { get; }


        public Item(string name, string lookDescription, string inventoryDescription, string itemConsumable)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            ItemConsumable = itemConsumable;
        }

        public override string ToString() => Name;
    }
}