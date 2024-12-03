using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public enum ObjectPlaced { health, ammo, coin }
    public ObjectPlaced objectPlaced;

    private GameManager gameManager;

    private string uniqueItemID;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        // Use the object's name to create a unique ID, or you could use a custom ID field
        uniqueItemID = objectPlaced.ToString() + "_" + gameObject.name; // Name should be unique for each object

        // Check if this specific item has been picked up on game load
        if (gameManager.HasItemBeenPickedUp(uniqueItemID))
        {
            gameObject.SetActive(false); // Item has been picked up, so disable it
        }
        else
        {
            gameObject.SetActive(true); // Item has not been picked up, so ensure it is active
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.MarkItemAsPickedUp(uniqueItemID);
            gameObject.SetActive(false); // Makes item disappear
        }
    }
}
