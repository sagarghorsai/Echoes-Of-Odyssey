public enum ItemType { Currency, Weapon }

[System.Serializable]
public class Item
{
    public string name;
    public int count;
    public ItemType type;
    public float price;

    public Item(string name, int count, ItemType type, float price)
    {
        this.name = name;
        this.count = count;
        this.type = type;
        this.price = price;
    }
}