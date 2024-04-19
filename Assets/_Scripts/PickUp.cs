using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item item;
    private Interactor interactor;
    private bool inRange = false;

    // Adjust this value to increase the interaction range
    public float interactionRange = 3f;

    private void Start()
    {
        interactor = GameObject.FindGameObjectWithTag("Interact").GetComponent<Interactor>();
    }

    private void Update()
    {
        // Check for "E" key press
        if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            // Show the item pickup UI when the player enters the trigger collider
            interactor.Show();

            // Automatically interact with currency items when the player walks over them
            if (item.type == ItemType.Currency)
            {
                Interact();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            // Hide the item pickup UI when the player exits the trigger collider
            interactor.Hide();
        }
    }

    private void Interact()
    {
        // Add the item to the player's inventory
        Inventory.Instance.AddItem(item);

        // Hide the item pickup UI after interaction
        interactor.Hide();

        // Destroy the pickup item
        Destroy(gameObject);
    }

    // Optionally, you can draw a gizmo to visualize the interaction range in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
