using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName; // Name of the item
        public int price; // Price of the item
        public Item item; // Reference to the item object
        public TMP_Text priceText; // Reference to the price TextMeshPro text component
        public TMP_Text titleText; // Reference to the title TextMeshPro text component
    }

    public Inventory playerInventory; // Reference to the player's inventory
    public GameObject[] itemObjects; // Array of GameObjects containing the price and title TextMeshPro text components
    public ShopItem[] shopItems; // Array of items available for sale

    public void PurchaseItem(int index)
    {
        // Check if playerInventory and shopItems are assigned
        if (playerInventory != null && shopItems != null)
        {
            if (index >= 0 && index < shopItems.Length)
            {
                int price = shopItems[index].price;
                if (playerInventory.GetCurrency() >= price)
                {
                    playerInventory.RemoveCurrency(price);

                    playerInventory.AddItem(shopItems[index].item);
                    Debug.Log("Item purchased successfully and added to inventory: " + shopItems[index].itemName);
                }
                else
                {
                    Debug.Log("Not enough currency to purchase this item: " + shopItems[index].itemName);
                }
            }
            else
            {
                Debug.LogError("Index is out of range!");
            }
        }
        else
        {
            Debug.LogError("playerInventory or shopItems is not assigned!");
        }
    }

    public void UpdateTexts()
    {
        if (itemObjects != null && shopItems != null && itemObjects.Length == shopItems.Length)
        {
            for (int i = 0; i < shopItems.Length; i++)
            {
                if (shopItems[i].priceText != null)
                {
                    shopItems[i].priceText.text = shopItems[i].price.ToString();
                }
                if (shopItems[i].titleText != null)
                {
                    shopItems[i].titleText.text = shopItems[i].itemName;
                }
            }
        }
    }
}
