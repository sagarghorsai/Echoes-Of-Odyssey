using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    public TMP_Text currencyText; // Use TMP_Text instead of Text
    public Inventory inventory; // Reference to the Inventory script


    void Update()
    {
        // Check if the currency text component and inventory are assigned
        if (currencyText != null && inventory != null)
        {
            // Update the text to display the current currency amount
            currencyText.text = "Currency: " + inventory.GetCurrency().ToString();
        }
    }
}
