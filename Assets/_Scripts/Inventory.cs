using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> items = new List<Item>();
    public float currency = 0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddItem(Item itemToAdd)
    {
        bool itemExists = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == itemToAdd.name && items[i].type == itemToAdd.type)
            {
                items[i].count += itemToAdd.count;
                itemExists = true;
                break;
            }
        }

        if (!itemExists)
        {
            items.Add(new Item(itemToAdd.name, itemToAdd.count, itemToAdd.type, itemToAdd.price));
        }

        if (itemToAdd.type == ItemType.Currency)
        {
            AddCurrency(itemToAdd.price);
        }

        Debug.Log(itemToAdd.count + " " + itemToAdd.name + " of type " + itemToAdd.type + " added to inventory.");
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == itemToRemove.name && items[i].type == itemToRemove.type)
            {
                items[i].count -= itemToRemove.count;
                if (items[i].count <= 0)
                {
                    items.RemoveAt(i);
                }
                break;
            }
        }

        if (itemToRemove.type == ItemType.Currency)
        {
            RemoveCurrency(itemToRemove.count);
        }

        Debug.Log(itemToRemove.count + " " + itemToRemove.name + " of type " + itemToRemove.type + " removed from inventory.");
    }

    public void AddCurrency(float amount)
    {
        currency += amount; // Increase the player's currency amount
        Debug.Log("Added " + amount + " currency to player's inventory.");
    }

    public void RemoveCurrency(float amount)
    {
        currency -= amount; // Decrease the player's currency amount
        Debug.Log("Removed " + amount + " currency from player's inventory.");
    }
    public float GetCurrency()
    {
        return currency;
    }

    public bool HasWeapon(PlayerController.WeaponType weaponType)
    {
        foreach (Item item in items)
        {
            if (item.type == ItemType.Weapon && item.name == weaponType.ToString())
            {
                return true;
            }
        }
        return false;
    }
}